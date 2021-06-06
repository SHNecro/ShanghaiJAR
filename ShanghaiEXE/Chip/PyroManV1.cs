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
    internal class PyroManV1 : ChipBase
  {
    private const int speed = 2;
    protected Point animePoint;

    public PyroManV1(IAudioEngine s)
      : base(s)
    {
      this.navi = true;
      this.number = 227;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.PyroManV1Name");
      this.element = ChipBase.ELEMENT.heat;
      this.power = 50;
      this.subpower = 0;
      this.regsize = 33;
      this.reality = 3;
      this._break = false;
      this.shadow = false;
      this.powerprint = true;
      this.code[0] = ChipFolder.CODE.P;
      this.code[1] = ChipFolder.CODE.F;
      this.code[2] = ChipFolder.CODE.asterisk;
      this.code[3] = ChipFolder.CODE.asterisk;
      var information = NSGame.ShanghaiEXE.Translate("Chip.PyroManV1Desc");
      this.information[0] = information[0];
      this.information[1] = information[1];
      this.information[2] = information[2];
      this.Init();
    }

    private Point Animation(int waittime)
    {
      return CharacterAnimation.ReturnKai(new int[10]
      {
        1,
        1,
        1,
        1,
        1,
        1,
        1,
        1,
        1,
        100
      }, new int[10]
      {
        24,
        25,
        26,
        27,
        28,
        29,
        30,
        31,
        32,
        33
      }, 0, waittime);
    }

    public override void Action(CharacterBase character, SceneBattle battle)
    {
      if (!this.BlackOut(character, battle, this.name, this.Power(character).ToString()))
        return;
      int waittime = character.waittime / 4;
      if (character.waittime % 4 == 0)
      {
        switch (waittime)
        {
          case 1:
            character.animationpoint.X = -1;
            this.sound.PlaySE(SoundEffect.warp);
            break;
          case 10:
            for (int index = 0; index < 3; ++index)
            {
              try
              {
                Point point = this.RandomPanel(false, false, false, character.UnionEnemy, battle, 0);
                if (index == 0)
                  point = this.RandomTarget(character, battle);
                this.sound.PlaySE(SoundEffect.quake);
                AttackBase a = new Tower(this.sound, battle, point.X, point.Y, character.union, this.Power(character), 999, this.element);
                a.invincibility = false;
                battle.attacks.Add(this.Paralyze(a));
              }
              catch
              {
              }
            }
            break;
          case 18:
            for (int index = 0; index < 3; ++index)
            {
              try
              {
                Point point = this.RandomPanel(false, false, false, character.UnionEnemy, battle, 0);
                this.sound.PlaySE(SoundEffect.quake);
                AttackBase a = new Tower(this.sound, battle, point.X, point.Y, character.union, this.Power(character), 999, this.element);
                a.invincibility = false;
                battle.attacks.Add(this.Paralyze(a));
              }
              catch
              {
              }
            }
            break;
          case 26:
            for (int index = 0; index < 3; ++index)
            {
              try
              {
                Point point = this.RandomPanel(false, false, false, character.UnionEnemy, battle, 0);
                if (index == 0)
                  point = this.RandomTarget(character, battle);
                this.sound.PlaySE(SoundEffect.quake);
                AttackBase a = new Tower(this.sound, battle, point.X, point.Y, character.union, this.Power(character), 999, this.element);
                a.invincibility = true;
                battle.attacks.Add(this.Paralyze(a));
              }
              catch
              {
              }
            }
            break;
          case 40:
            character.parent.effects.Add(new MoveEnemy(this.sound, character.parent, character.position.X, character.position.Y));
            break;
        }
        if (waittime > 40)
        {
          this.animePoint.X = -1;
          if (waittime > 50)
          {
            character.animationpoint.X = 0;
            if (this.BlackOutEnd(character, battle))
              base.Action(character, battle);
          }
        }
        else
          this.animePoint = this.Animation(waittime);
      }
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
        this._rect = new Rectangle(0, 0, 56, 48);
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
      if (character.animationpoint.X == -1)
      {
        this._rect = new Rectangle(64 * this.animePoint.X, 0, 64, 96);
        this._position = new Vector2((float) (character.position.X * 40.0 + 18.0), (float) (character.position.Y * 24.0 + 58.0));
        dg.DrawImage(dg, "PyroMan", this._rect, false, this._position, character.union == Panel.COLOR.red, Color.White);
      }
      else
        this.BlackOutRender(dg, character.union);
    }
  }
}

