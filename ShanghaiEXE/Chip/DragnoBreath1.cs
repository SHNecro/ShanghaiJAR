using NSAttack;
using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSChip
{
    internal class DragnoBreath1 : ChipBase
  {
    private bool fire;
    private const int shotend = 28;
    private int count;

    public DragnoBreath1(IAudioEngine s)
      : base(s)
    {
      this.rockOnPoint = new Point(-2, 0);
      this.number = 97;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.DragnoBreath1Name");
      this.element = ChipBase.ELEMENT.heat;
      this.power = 150;
      this.regsize = 38;
      this.reality = 3;
      this.subpower = 0;
      this._break = false;
      this.powerprint = true;
      this.code[0] = ChipFolder.CODE.D;
      this.code[1] = ChipFolder.CODE.G;
      this.code[2] = ChipFolder.CODE.N;
      this.code[3] = ChipFolder.CODE.asterisk;
      var information = NSGame.ShanghaiEXE.Translate("Chip.DragnoBreath1Desc");
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
      if (!this.fire)
      {
        if (character.waittime < 5)
          character.animationpoint = new Point(4, 0);
        else if (character.waittime < 15)
          character.animationpoint = new Point(5, 0);
        else if (character.waittime < 28)
        {
          character.animationpoint = new Point(6, 0);
          if (character.waittime < 17)
            character.positionDirect.X -= (character.waittime - 15) * this.UnionRebirth(character.union);
        }
        else if (character.waittime < 33)
        {
          character.animationpoint = new Point(5, 0);
          character.PositionDirectSet();
        }
        else if (character.waittime == 33)
          base.Action(character, battle);
        if (character.waittime == 18)
        {
          this.fire = true;
          character.waittime = 0;
          character.animationpoint = new Point(5, 0);
        }
      }
      else
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
          this.sound.PlaySE(SoundEffect.quake);
          AttackBase a1 = new ElementFire(this.sound, character.parent, character.position.X + this.count * this.UnionRebirth(character.union), character.position.Y, character.union, this.Power(character), 6, this.element, false, 1);
          a1.positionDirect.X += 16 * this.UnionRebirth(character.union);
          a1.invincibility = true;
          character.parent.attacks.Add(this.Paralyze(a1));
          if (character.waittime > 6)
          {
            AttackBase a2 = new ElementFire(this.sound, character.parent, character.position.X + this.count * this.UnionRebirth(character.union), character.position.Y - 1, character.union, this.Power(character), 6, this.element, false, 1);
            a2.positionDirect.X += 16 * this.UnionRebirth(character.union);
            a2.invincibility = true;
            character.parent.attacks.Add(this.Paralyze(a2));
            AttackBase a3 = new ElementFire(this.sound, character.parent, character.position.X + this.count * this.UnionRebirth(character.union), character.position.Y + 1, character.union, this.Power(character), 6, this.element, false, 1);
            a3.positionDirect.X += 16 * this.UnionRebirth(character.union);
            a3.invincibility = true;
            character.parent.attacks.Add(this.Paralyze(a3));
          }
          ++this.count;
        }
        if (character.waittime == 56)
        {
          character.animationpoint = new Point(0, 0);
          base.Action(character, battle);
        }
      }
      if (character.waittime != 20)
        return;
      int num = this.power + this.pluspower;
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
        this._rect = new Rectangle(168, 0, 56, 48);
        dg.DrawImage(dg, "chipgraphic16", this._rect, true, p, Color.White);
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
      if (!this.fire)
      {
        if (character.waittime < 5 && !this.fire)
          return;
        this._rect = new Rectangle(1320, 120, character.Wide, character.Height);
        double x1 = character.positionDirect.X;
        Point shake = this.Shake;
        double x2 = shake.X;
        double num1 = x1 + x2;
        double y1 = character.positionDirect.Y;
        shake = this.Shake;
        double y2 = shake.Y;
        double num2 = y1 + y2;
        this._position = new Vector2((float) num1, (float) num2);
        if (character.waittime >= 10 && (character.waittime >= 15 && character.waittime < 28))
          this._position.X -= 2 * this.UnionRebirth(character.union);
        dg.DrawImage(dg, "weapons", this._rect, false, this._position, character.union == Panel.COLOR.blue, Color.White);
      }
      else
      {
        this._rect = new Rectangle(1320, 120, character.Wide, character.Height);
        this._position = new Vector2(character.positionDirect.X + Shake.X, character.positionDirect.Y + Shake.Y);
        if (character.animationpoint.X == 6)
        {
          this._rect.X += 120;
          this._position.X -= this.UnionRebirth(character.union);
        }
        dg.DrawImage(dg, "weapons", this._rect, false, this._position, character.union == Panel.COLOR.blue, Color.White);
      }
    }
  }
}

