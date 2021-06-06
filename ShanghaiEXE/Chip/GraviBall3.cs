using NSAttack;
using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSChip
{
    internal class GraviBall3 : ChipBase
  {
    private const int speed = 4;
    private const int shotend = 16;

    public GraviBall3(IAudioEngine s)
      : base(s)
    {
      this.rockOnPoint = new Point(-1, 0);
      this.number = 42;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.GraviBall3Name");
      this.element = ChipBase.ELEMENT.earth;
      this.power = 120;
      this.subpower = 0;
      this.regsize = 58;
      this.reality = 4;
      this.crack = true;
      this._break = false;
      this.powerprint = true;
      this.code[0] = ChipFolder.CODE.B;
      this.code[1] = ChipFolder.CODE.D;
      this.code[2] = ChipFolder.CODE.F;
      this.code[3] = ChipFolder.CODE.I;
      var information = NSGame.ShanghaiEXE.Translate("Chip.GraviBall3Desc");
      this.information[0] = information[0];
      this.information[1] = information[1];
      this.information[2] = information[2];
      this.Init();
    }

    public override void Action(CharacterBase character, SceneBattle battle)
    {
      if (character.waittime < 5)
        character.animationpoint = new Point(4, 0);
      else if (character.waittime < 15)
        character.animationpoint = new Point(5, 0);
      else if (character.waittime < 16)
        character.animationpoint = new Point(6, 0);
      else if (character.waittime < 21)
        character.animationpoint = new Point(5, 0);
      else if (character.waittime == 40)
        base.Action(character, battle);
      if (character.waittime != 16)
        return;
      int num = this.power + this.pluspower;
      GravityBallAttack gravityBallAttack = new GravityBallAttack(this.sound, character.parent, character.position.X + this.UnionRebirth(character.union), character.position.Y, character.union, this.Power(character), 2, new Vector2(character.positionDirect.X + 20 * this.UnionRebirth(character.union), character.positionDirect.Y + 16f), this.element);
      gravityBallAttack.BadStatusSet(CharacterBase.BADSTATUS.heavy, 1200);
      character.parent.attacks.Add(this.Paralyze(gravityBallAttack));
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
        dg.DrawImage(dg, "chipgraphic2", this._rect, true, p, Color.White);
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
      this._rect = new Rectangle(3 * character.Wide, character.Height, character.Wide, character.Height);
      this._position = new Vector2(character.positionDirect.X + Shake.X, character.positionDirect.Y + Shake.Y);
      if (character.waittime < 10)
        this._rect.X = 0;
      dg.DrawImage(dg, "weapons", this._rect, false, this._position, character.union == Panel.COLOR.blue, Color.White);
    }
  }
}

