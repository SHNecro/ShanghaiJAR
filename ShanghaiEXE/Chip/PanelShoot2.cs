using NSAttack;
using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSChip
{
    internal class PanelShoot2 : ChipBase
  {
    private const int start = 1;
    private const int speed = 2;

    public PanelShoot2(IAudioEngine s)
      : base(s)
    {
      this.rockOnPoint = new Point(-3, 0);
      this.number = 101;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.PanelShoot2Name");
      this.element = ChipBase.ELEMENT.normal;
      this.power = 80;
      this.subpower = 0;
      this.regsize = 29;
      this.reality = 2;
      this._break = false;
      this.crack = true;
      this.powerprint = true;
      this.code[0] = ChipFolder.CODE.B;
      this.code[1] = ChipFolder.CODE.D;
      this.code[2] = ChipFolder.CODE.G;
      this.code[3] = ChipFolder.CODE.N;
      var information = NSGame.ShanghaiEXE.Translate("Chip.PanelShoot2Desc");
      this.information[0] = information[0];
      this.information[1] = information[1];
      this.information[2] = information[2];
      this.Init();
    }

    public override void Action(CharacterBase character, SceneBattle battle)
    {
      if (character.waittime <= 1)
        character.animationpoint = new Point(0, 1);
      else if (character.waittime <= 7)
        character.animationpoint = new Point((character.waittime - 1) / 2, 1);
      else if (character.waittime <= 15)
        character.animationpoint = new Point(3, 1);
      else
        base.Action(character, battle);
      if (character.waittime != 5)
        return;
      int num = this.power + this.pluspower;
      character.parent.attacks.Add(this.Paralyze(new PanelShoot(this.sound, character.parent, character.position.X + this.UnionRebirth(character.union), character.position.Y, character.union, this.Power(character), this.element)));
      character.parent.attacks.Add(this.Paralyze(new PanelShoot(this.sound, character.parent, character.position.X + this.UnionRebirth(character.union), character.position.Y - 1, character.union, this.Power(character), this.element)));
      character.parent.attacks.Add(this.Paralyze(new PanelShoot(this.sound, character.parent, character.position.X + this.UnionRebirth(character.union), character.position.Y + 1, character.union, this.Power(character), this.element)));
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
        dg.DrawImage(dg, "chipgraphic20", this._rect, true, p, Color.White);
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
      this._rect = new Rectangle(character.animationpoint.X * character.Wide, 4 * character.Height, character.Wide, character.Height);
      this._position = new Vector2(character.positionDirect.X + Shake.X, character.positionDirect.Y + Shake.Y);
      dg.DrawImage(dg, character.picturename, this._rect, false, this._position, character.union == Panel.COLOR.blue, Color.White);
    }
  }
}

