using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSChip
{
    internal class ZeroKnuckle : ChipBase
  {
    private int speed = 3;
    private const int start = 5;
    private bool get;
    private NSAttack.ZeroKnuckle zero;

    public ZeroKnuckle(IAudioEngine s)
      : base(s)
    {
      this.rockOnPoint = new Point(-1, 0);
      this.infight = true;
      this.number = 140;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.ZeroKnuckleName");
      this.element = ChipBase.ELEMENT.normal;
      this.power = 250;
      this.subpower = 0;
      this.regsize = 34;
      this.reality = 4;
      this._break = true;
      this.powerprint = true;
      this.code[0] = ChipFolder.CODE.J;
      this.code[1] = ChipFolder.CODE.R;
      this.code[2] = ChipFolder.CODE.N;
      this.code[3] = ChipFolder.CODE.Z;
      var information = NSGame.ShanghaiEXE.Translate("Chip.ZeroKnuckleDesc");
      this.information[0] = information[0];
      this.information[1] = information[1];
      this.information[2] = information[2];
      this.Init();
    }

    public override void Action(CharacterBase character, SceneBattle battle)
    {
      if (!this.get)
      {
        if (character.waittime == 5 + this.speed * 2)
        {
          this.sound.PlaySE(SoundEffect.Zblade);
          int num = this.power + this.pluspower;
          this.zero = new NSAttack.ZeroKnuckle(this.sound, character.parent, character.position.X + this.UnionRebirth(character.union), character.position.Y, character.union, this.Power(character), 2, this.element, character);
          character.parent.attacks.Add(this.Paralyze(zero));
        }
        if (character.waittime < 5)
          character.animationpoint = new Point(0, 3);
        else if (character.waittime < 5 + this.speed * 4)
          character.animationpoint = new Point((character.waittime - 5) / this.speed, 3);
        else if (character.waittime < 5 + this.speed * 4 + 20)
          character.animationpoint = new Point(3, 3);
        else if (character.waittime < 5 + this.speed * 4 + 26)
          character.animationpoint = new Point(4, 0);
        else
          base.Action(character, battle);
        if (this.zero == null || !this.zero.get)
          return;
        this.get = true;
        this.speed = 2;
        this.sound.PlaySE(SoundEffect.getchip);
        character.waittime = 0;
      }
      else if (character.waittime <= 5)
        character.animationpoint = new Point(0, 1);
      else if (character.waittime <= this.speed * 3 + 5)
        character.animationpoint = new Point((character.waittime - 5) / this.speed, 1);
      else if (character.waittime < this.speed * 7 + 5)
        character.animationpoint = new Point(3, 1);
      else if (character.waittime != this.speed * 7 + 5 && character.waittime >= this.speed * 15 + 5)
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
        dg.DrawImage(dg, "chipgraphic10", this._rect, true, p, Color.White);
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
      if (!this.get)
      {
        this._rect = new Rectangle(character.animationpoint.X * (character.Wide * 2), 4 * character.Height, character.Wide * 2, character.Height);
        this._position = new Vector2(character.positionDirect.X + character.Wide / 2 * this.UnionRebirth(character.union) + Shake.X, character.positionDirect.Y + Shake.Y);
        dg.DrawImage(dg, "Lances", this._rect, false, this._position, character.union == Panel.COLOR.blue, Color.White);
      }
      else
      {
        this._rect = new Rectangle(character.animationpoint.X * character.Wide, 4 * character.Height, character.Wide, character.Height);
        this._position = new Vector2(character.positionDirect.X + Shake.X, character.positionDirect.Y + Shake.Y);
        dg.DrawImage(dg, character.picturename, this._rect, false, this._position, character.union == Panel.COLOR.blue, Color.White);
      }
    }
  }
}

