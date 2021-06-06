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
    internal class IkuV1 : ChipBase
  {
    protected Point[] target = new Point[5];
    private const int speed = 2;
    protected int command;
    private Point animePoint;

    public IkuV1(IAudioEngine s)
      : base(s)
    {
      this.navi = true;
      this.number = 224;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.IkuV1Name");
      this.element = ChipBase.ELEMENT.eleki;
      this.power = 100;
      this.subpower = 0;
      this.regsize = 32;
      this.reality = 3;
      this._break = false;
      this.shadow = false;
      this.powerprint = true;
      this.code[0] = ChipFolder.CODE.I;
      this.code[1] = ChipFolder.CODE.N;
      this.code[2] = ChipFolder.CODE.asterisk;
      this.code[3] = ChipFolder.CODE.asterisk;
      var information = NSGame.ShanghaiEXE.Translate("Chip.IkuV1Desc");
      this.information[0] = information[0];
      this.information[1] = information[1];
      this.information[2] = information[2];
      this.Init();
    }

    private Point Animation(int waittime)
    {
      return CharacterAnimation.ReturnKai(new int[7]
      {
        1,
        8,
        16,
        4,
        4,
        52,
        4
      }, new int[7]{ -1, 1, 8, 9, 10, 10, -1 }, 0, waittime);
    }

    public override void Action(CharacterBase character, SceneBattle battle)
    {
      if (!this.BlackOut(character, battle, this.name, this.Power(character).ToString()))
        return;
      this.animePoint = this.Animation(character.waittime);
      if (character is Player && (character.waittime < 32 && this.command == 0))
      {
        this.CommandInput("LR", (Player) character);
        if (this.CommandCheck("LLR"))
        {
          this.command = 1;
          this.sound.PlaySE(SoundEffect.CommandSuccess);
          this.target[0] = new Point(character.position.X + 2 * this.UnionRebirth(character.union), character.position.Y);
          this.target[1] = new Point(character.position.X + 3 * this.UnionRebirth(character.union), character.position.Y);
          this.target[2] = new Point(character.position.X + 4 * this.UnionRebirth(character.union), character.position.Y);
        }
        else if (this.CommandCheck("RRL"))
        {
          this.command = 2;
          this.sound.PlaySE(SoundEffect.CommandSuccess);
          this.target[0] = new Point(character.position.X + 2 * this.UnionRebirth(character.union), character.position.Y - 1);
          this.target[1] = new Point(character.position.X + 3 * this.UnionRebirth(character.union), character.position.Y);
          this.target[2] = new Point(character.position.X + 4 * this.UnionRebirth(character.union), character.position.Y - 1);
          this.target[3] = new Point(character.position.X + 2 * this.UnionRebirth(character.union), character.position.Y + 1);
          this.target[4] = new Point(character.position.X + 4 * this.UnionRebirth(character.union), character.position.Y + 1);
        }
      }
      switch (character.waittime)
      {
        case 1:
          character.animationpoint.X = -1;
          this.sound.PlaySE(SoundEffect.warp);
          this.target[0] = new Point(character.position.X + 3 * this.UnionRebirth(character.union), 0);
          this.target[1] = new Point(character.position.X + 3 * this.UnionRebirth(character.union), 1);
          this.target[2] = new Point(character.position.X + 3 * this.UnionRebirth(character.union), 2);
          break;
        case 32:
          AttackBase a1 = new CrackThunder(this.sound, character.parent, this.target[0].X, this.target[0].Y, character.union, this.Power(character), false);
          a1.breakinvi = true;
          a1.invincibility = false;
          character.parent.attacks.Add(this.Paralyze(a1));
          break;
        case 40:
          if (this.command == 2)
          {
            AttackBase a2 = new CrackThunder(this.sound, character.parent, this.target[4].X, this.target[4].Y, character.union, this.Power(character), false);
            a2.breakinvi = true;
            a2.invincibility = false;
            character.parent.attacks.Add(this.Paralyze(a2));
            break;
          }
          break;
        case 48:
          AttackBase a3 = new CrackThunder(this.sound, character.parent, this.target[1].X, this.target[1].Y, character.union, this.Power(character), false);
          a3.breakinvi = true;
          a3.invincibility = false;
          character.parent.attacks.Add(this.Paralyze(a3));
          break;
        case 56:
          if (this.command == 2)
          {
            AttackBase a2 = new CrackThunder(this.sound, character.parent, this.target[3].X, this.target[3].Y, character.union, this.Power(character), false);
            a2.breakinvi = true;
            a2.invincibility = false;
            character.parent.attacks.Add(this.Paralyze(a2));
            break;
          }
          break;
        case 64:
          AttackBase a4 = new CrackThunder(this.sound, character.parent, this.target[2].X, this.target[2].Y, character.union, this.Power(character), false);
          a4.breakinvi = true;
          a4.invincibility = false;
          character.parent.attacks.Add(this.Paralyze(a4));
          break;
        case 84:
          character.parent.effects.Add(new MoveEnemy(this.sound, character.parent, character.position.X, character.position.Y));
          break;
      }
      if (character.waittime > 120 && this.BlackOutEnd(character, battle))
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
        this._rect = new Rectangle(0, 0, 56, 48);
        dg.DrawImage(dg, "chipgraphic15", this._rect, true, p, Color.White);
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
        this._rect = new Rectangle(80 * this.animePoint.X, 0, 80, 72);
        this._position = new Vector2((float) (character.position.X * 40.0 + 20.0), (float) (character.position.Y * 24.0 + 54.0));
        dg.DrawImage(dg, "iku", this._rect, false, this._position, character.union == Panel.COLOR.red, Color.White);
      }
      else
        this.BlackOutRender(dg, character.union);
    }
  }
}

