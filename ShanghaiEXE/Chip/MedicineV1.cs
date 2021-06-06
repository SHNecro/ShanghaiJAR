using NSAttack;
using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEffect;
using Common.Vectors;
using System.Drawing;

namespace NSChip
{
    internal class MedicineV1 : ChipBase
  {
    private const int interval = 20;
    private const int speed = 2;
    private bool ballrend;
    private const int s = 5;
    private Point animePoint;

    public MedicineV1(IAudioEngine s)
      : base(s)
    {
      this.navi = true;
      this.number = 221;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.MedicineV1Name");
      this.element = ChipBase.ELEMENT.poison;
      this.power = 70;
      this.subpower = 10;
      this.regsize = 24;
      this.reality = 3;
      this._break = false;
      this.shadow = false;
      this.powerprint = true;
      this.code[0] = ChipFolder.CODE.M;
      this.code[1] = ChipFolder.CODE.P;
      this.code[2] = ChipFolder.CODE.asterisk;
      this.code[3] = ChipFolder.CODE.asterisk;
      var information = NSGame.ShanghaiEXE.Translate("Chip.MedicineV1Desc");
      this.information[0] = information[0];
      this.information[1] = information[1];
      this.information[2] = information[2];
      this.Init();
    }

    protected Point Animation(int waittime)
    {
      int[] interval = new int[6]{ 4, 1, 1, 8, 1, 60 };
      int[] xpoint = new int[6]{ 0, 8, 12, 13, 16, 17 };
      for (int index = 0; index < interval.Length; ++index)
        interval[index] *= 5;
      int y = 0;
      return CharacterAnimation.ReturnKai(interval, xpoint, y, waittime);
    }

    public override void Action(CharacterBase character, SceneBattle battle)
    {
      if (this.BlackOut(character, battle, this.name, this.Power(character).ToString()))
      {
        if (character.waittime <= 160)
          this.animePoint = this.Animation(character.waittime);
        switch (character.waittime)
        {
          case 0:
            battle.effects.Add(new Bomber(this.sound, battle, character.position.X, character.position.Y, Bomber.BOMBERTYPE.poison, 2));
            character.animationpoint.X = -1;
            this.sound.PlaySE(SoundEffect.bomb);
            break;
          case 30:
            this.sound.PlaySE(SoundEffect.eriasteal1);
            for (int index1 = 0; index1 < battle.panel.GetLength(0); ++index1)
            {
              for (int index2 = 0; index2 < battle.panel.GetLength(1); ++index2)
              {
                if (battle.panel[index1, index2].color == character.union && battle.panel[index1, index2].state == Panel.PANEL._poison)
                {
                  battle.panel[index1, index2].State = Panel.PANEL._nomal;
                  this.power += this.subpower;
                }
              }
            }
            this.ballrend = true;
            break;
          case 70:
            this.ballrend = false;
            this.sound.PlaySE(SoundEffect.sword);
            Point position = character.position;
            position.X += 3 * this.UnionRebirth(character.union);
            PoisonBall poisonBall = new PoisonBall(this.sound, battle, position.X, position.Y, new Vector2((float) (character.position.X * 40.0 + 22.0) + 4 * this.UnionRebirth(character.union), (float) (character.position.Y * 24.0 + 58.0 - 48.0)), character.union, this.Power(character), 2, 20, this.element);
            poisonBall.badstatus[5] = true;
            poisonBall.badstatustime[5] = 300;
            battle.attacks.Add(this.Paralyze(poisonBall));
            this.ballrend = false;
            break;
          case 160:
            this.animePoint.X = -1;
            battle.effects.Add(new MoveEnemy(this.sound, battle, character.position.X, character.position.Y));
            break;
        }
      }
      if (character.waittime < 175 || !this.BlackOutEnd(character, battle))
        return;
      base.Action(character, battle);
    }

    public override void GraphicsRender(
      IRenderer dg,
      Vector2 p,
      int c,
      bool printgraphics,
      bool printstatus)
    {
      if (printgraphics)
      {
        this._rect = new Rectangle(504, 0, 56, 48);
        dg.DrawImage(dg, "chipgraphic12", this._rect, true, p, Color.White);
      }
      base.GraphicsRender(dg, p, c, printgraphics, printstatus);
    }

    public override void IconRender(
      IRenderer dg,
      Vector2 p,
      bool select,
      bool custom,
      int c,
      bool noicon)
    {
      if (!noicon)
      {
        int num1 = this.number - 1;
        int num2 = num1 % 40;
        int num3 = num1 / 40;
        int num4 = 0;
        if (select)
          num4 = 1;
        this._rect = new Rectangle(480, 64 + num4 * 96, 16, 16);
        dg.DrawImage(dg, "chipicon", this._rect, true, p, Color.White);
      }
      base.IconRender(dg, p, select, custom, c, noicon);
    }

    public override void Render(IRenderer dg, CharacterBase character)
    {
      if (character.animationpoint.X == -1)
      {
        this._rect = new Rectangle(48 * this.animePoint.X, 0, 48, 64);
        this._position = new Vector2((float) (character.position.X * 40.0 + 22.0) + 4 * this.UnionRebirth(character.union), (float) (character.position.Y * 24.0 + 58.0));
        dg.DrawImage(dg, "medicine", this._rect, false, this._position, character.union == Panel.COLOR.red, Color.White);
        if (!this.ballrend)
          return;
        this._position = new Vector2(character.positionDirect.X - 8f, character.positionDirect.Y - 48f);
        this._rect = new Rectangle(character.waittime % 7 * 40, 352, 40, 40);
        dg.DrawImage(dg, "towers", this._rect, false, this._position, character.union == Panel.COLOR.red, Color.White);
      }
      else
        this.BlackOutRender(dg, character.union);
    }
  }
}

