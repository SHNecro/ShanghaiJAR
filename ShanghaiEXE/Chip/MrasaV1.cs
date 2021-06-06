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
    internal class MrasaV1 : ChipBase
  {
    private const int start = 44;
    private const int speed = 2;
    protected bool colory;
    protected Point target;
    protected Point animePoint;

    public MrasaV1(IAudioEngine s)
      : base(s)
    {
      this.navi = true;
      this.number = 230;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.MrasaV1Name");
      this.element = ChipBase.ELEMENT.aqua;
      this.power = 130;
      this.subpower = 0;
      this.regsize = 33;
      this.reality = 3;
      this._break = true;
      this.crack = true;
      this.shadow = false;
      this.powerprint = true;
      this.code[0] = ChipFolder.CODE.M;
      this.code[1] = ChipFolder.CODE.C;
      this.code[2] = ChipFolder.CODE.asterisk;
      this.code[3] = ChipFolder.CODE.asterisk;
      this.animePoint.X = -1;
      var information = NSGame.ShanghaiEXE.Translate("Chip.MrasaV1Desc");
      this.information[0] = information[0];
      this.information[1] = information[1];
      this.information[2] = information[2];
      this.Init();
      this.colory = true;
    }

    protected Point Animation(int waittime)
    {
      return CharacterAnimation.ReturnKai(new int[2]
      {
        1,
        100
      }, new int[2]{ -1, 0 }, 0, waittime);
    }

    public override void Action(CharacterBase character, SceneBattle battle)
    {
      if (!this.BlackOut(character, battle, this.name, this.Power(character).ToString()))
        return;
      switch (character.waittime)
      {
        case 1:
          character.animationpoint.X = 1;
          character.animationpoint.Y = 0;
          break;
        case 2:
          character.animationpoint.X = 2;
          break;
        case 3:
          character.animationpoint.X = 3;
          break;
        case 4:
          character.animationpoint.X = -1;
          break;
        case 6:
          character.parent.effects.Add(new MoveEnemy(this.sound, character.parent, character.position.X, character.position.Y));
          this.animePoint.X = 0;
          this.sound.PlaySE(SoundEffect.warp);
          break;
        case 7:
          this.target = this.RandomTarget(character, battle);
          character.parent.attacks.Add(new Dummy(this.sound, battle, this.target.X, this.target.Y, character.union, new Point(), 60, true));
          break;
        case 30:
          character.parent.effects.Add(new MoveEnemy(this.sound, character.parent, character.position.X, character.position.Y));
          this.animePoint.X = -1;
          break;
        case 44:
          character.parent.attacks.Add(this.Paralyze(new MrasaAnchor(this.sound, battle, this.target.X, this.target.Y, character.union, this.Power(character), 1, new Vector2(), this.target, 1, MrasaAnchor.TYPE.single, this.colory)));
          break;
      }
      if (character.waittime > 160 && this.BlackOutEnd(character, battle))
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
        this._rect = new Rectangle(336, 0, 56, 48);
        dg.DrawImage(dg, "chipgraphic19", this._rect, true, p, Color.White);
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
      if (character.animationpoint.X == -1 && character.waittime < 44)
      {
        this._rect = new Rectangle(72 * this.animePoint.X, 0, 72, 80);
        this._position = new Vector2((float) (character.position.X * 40.0 + 32.0), (float) (character.position.Y * 24.0 + 58.0));
        dg.DrawImage(dg, "mrasa", this._rect, false, this._position, character.union == Panel.COLOR.red, Color.White);
      }
      else
        this.BlackOutRender(dg, character.union);
    }
  }
}

