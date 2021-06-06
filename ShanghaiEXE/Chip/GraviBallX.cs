using NSAttack;
using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSChip
{
    internal class GraviBallX : ChipBase
  {
    private const int speed = 4;
    private const int shotend = 16;

    public GraviBallX(IAudioEngine s)
      : base(s)
    {
      this.rockOnPoint = new Point(-1, 0);
      this.number = 351;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.GraviBallXName");
      this.element = ChipBase.ELEMENT.earth;
      this.power = 180;
      this.subpower = 0;
      this.regsize = 99;
      this.reality = 5;
      this._break = false;
      this.crack = true;
      this.powerprint = true;
      this.code[0] = ChipFolder.CODE.G;
      this.code[1] = ChipFolder.CODE.X;
      this.code[2] = ChipFolder.CODE.G;
      this.code[3] = ChipFolder.CODE.G;
      var information = NSGame.ShanghaiEXE.Translate("Chip.GraviBallXDesc");
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

    public override void Init()
    {
      base.Init();
      this.sortNumber = 42.5f;
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
        this._rect = new Rectangle(56, 0, 56, 48);
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
        this._rect = new Rectangle(64, 80 + num4 * 96, 16, 16);
        dg.DrawImage(dg, "chipicon", this._rect, true, p, Color.White);
      }
      base.IconRender(dg, p, select, custom, c, noicon);
    }

    public override void Render(IRenderer dg, CharacterBase character)
    {
      if (character.waittime < 5)
        return;
      this._rect = new Rectangle(character.Wide, character.Height, character.Wide, character.Height);
      this._position = new Vector2(character.positionDirect.X + Shake.X, character.positionDirect.Y + Shake.Y);
      if (character.waittime < 10)
        this._rect.X = 0;
      dg.DrawImage(dg, "weapons", this._rect, false, this._position, character.union == Panel.COLOR.blue, Color.White);
    }
  }
}

