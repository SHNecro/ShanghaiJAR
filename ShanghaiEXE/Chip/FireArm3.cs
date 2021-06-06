using NSAttack;
using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSChip
{
    internal class FireArm3 : ChipBase
  {
    private const int shotend = 28;
    private int count;

    public FireArm3(IAudioEngine s)
      : base(s)
    {
      this.rockOnPoint = new Point(-3, 0);
      this.number = 16;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.FireArm3Name");
      this.element = ChipBase.ELEMENT.heat;
      this.power = 140;
      this.regsize = 43;
      this.reality = 2;
      this.subpower = 0;
      this._break = true;
      this.powerprint = true;
      this.code[0] = ChipFolder.CODE.I;
      this.code[1] = ChipFolder.CODE.J;
      this.code[2] = ChipFolder.CODE.O;
      this.code[3] = ChipFolder.CODE.P;
      var information = NSGame.ShanghaiEXE.Translate("Chip.FireArm3Desc");
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

    public override void GraphicsRender(
      IRenderer dg,
      Vector2 p,
      int c,
      bool printgraphics,
      bool printstatus)
    {
      if (printgraphics)
      {
        this._rect = new Rectangle(112, 0, 56, 48);
        dg.DrawImage(dg, "chipgraphic7", this._rect, true, p, Color.White);
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
        this._rect = this.IconRect(select);
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

