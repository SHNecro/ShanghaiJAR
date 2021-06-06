using NSAttack;
using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSChip
{
    internal class FireArmX : ChipBase
  {
    private const int shotend = 28;
    private int count;

    public FireArmX(IAudioEngine s)
      : base(s)
    {
      this.rockOnPoint = new Point(-2, 0);
      this.number = 366;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.FireArmXName");
      this.element = ChipBase.ELEMENT.heat;
      this.power = 160;
      this.regsize = 99;
      this.reality = 5;
      this.subpower = 0;
      this._break = true;
      this.powerprint = true;
      this.code[0] = ChipFolder.CODE.F;
      this.code[1] = ChipFolder.CODE.F;
      this.code[2] = ChipFolder.CODE.F;
      this.code[3] = ChipFolder.CODE.X;
      var information = NSGame.ShanghaiEXE.Translate("Chip.FireArmXDesc");
      this.information[0] = information[0];
      this.information[1] = information[1];
      this.information[2] = information[2];
      this.Init();
    }

    private Point Animation(int waittime)
    {
      return CharacterAnimation.Return(new int[4]
      {
        5,
        15,
        17,
        20
      }, new int[7]{ 4, 5, 6, 5, 2, 1, 0 }, 0, waittime);
    }

    public override void Action(CharacterBase character, SceneBattle battle)
    {
      if (character.waittime == 2)
      {
        character.animationpoint = new Point(5, 0);
        this.count = 1;
      }
      if ((character.waittime - 6) % 9 == 0)
        character.animationpoint = new Point(6, 0);
      if ((character.waittime - 11) % 9 == 0)
        character.animationpoint = new Point(5, 0);
      if (character.waittime == 6 || character.waittime == 15 || character.waittime == 24)
      {
        character.animationpoint = new Point(6, 0);
        this.sound.PlaySE(SoundEffect.fire);
        AttackBase a = new ElementFire(this.sound, character.parent, character.position.X + this.count * this.UnionRebirth(character.union), character.position.Y, character.union, this.Power(character), 6, this.element, true, 0);
        a.positionDirect.X -= 48 * this.UnionRebirth(character.union);
        a.breaking = true;
        a.invincibility = false;
        character.parent.attacks.Add(this.Paralyze(a));
        ++this.count;
      }
      if (character.waittime != 56)
        return;
      character.animationpoint = new Point(0, 0);
      base.Action(character, battle);
    }

    public override void Init()
    {
      base.Init();
      this.sortNumber = 16.5f;
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
        this._rect = new Rectangle(896, 0, 56, 48);
        dg.DrawImage(dg, "chipgraphic17", this._rect, true, p, Color.White);
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
        this._rect = new Rectangle(304, 80 + num4 * 96, 16, 16);
        dg.DrawImage(dg, "chipicon", this._rect, true, p, Color.White);
      }
      base.IconRender(dg, p, select, custom, c, noicon);
    }

    public override void Render(IRenderer dg, CharacterBase character)
    {
      if (character.waittime < 5)
        return;
      this._rect = new Rectangle(480, 480, character.Wide, character.Height);
      this._position = new Vector2(character.positionDirect.X + Shake.X, character.positionDirect.Y + Shake.Y);
      if (character.animationpoint.X == 6)
      {
        this._rect.X -= 120;
        this._position.X -= 2 * this.UnionRebirth(character.union);
      }
      dg.DrawImage(dg, "weapons", this._rect, false, this._position, character.union == Panel.COLOR.blue, Color.White);
    }
  }
}

