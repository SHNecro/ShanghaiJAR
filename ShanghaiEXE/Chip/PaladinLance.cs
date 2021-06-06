using NSAttack;
using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSChip
{
    internal class PaladinLance : ChipBase
  {
    private const int start = 5;
    private const int speed = 3;

    public PaladinLance(IAudioEngine s)
      : base(s)
    {
      this.rockOnPoint = new Point(-1, 0);
      this.number = 73;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.PaladinLanceName");
      this.element = ChipBase.ELEMENT.normal;
      this.power = 220;
      this.subpower = 0;
      this.regsize = 41;
      this.reality = 4;
      this._break = false;
      this.powerprint = true;
      this.code[0] = ChipFolder.CODE.P;
      this.code[1] = ChipFolder.CODE.R;
      this.code[2] = ChipFolder.CODE.S;
      this.code[3] = ChipFolder.CODE.U;
      var information = NSGame.ShanghaiEXE.Translate("Chip.PaladinLanceDesc");
      this.information[0] = information[0];
      this.information[1] = information[1];
      this.information[2] = information[2];
      this.Init();
    }

    public override void Action(CharacterBase character, SceneBattle battle)
    {
      if (character.waittime == 11)
      {
        this.sound.PlaySE(SoundEffect.lance);
        int num = this.power + this.pluspower;
        character.parent.attacks.Add(this.Paralyze(new LanceAttack(this.sound, character.parent, character.position.X + this.UnionRebirth(character.union), character.position.Y, character.union, this.Power(character), 2, this.element, false)));
      }
      if (character.waittime < 5)
        character.animationpoint = new Point(0, 3);
      else if (character.waittime < 17)
        character.animationpoint = new Point((character.waittime - 5) / 3, 3);
      else if (character.waittime < 37)
        character.animationpoint = new Point(3, 3);
      else if (character.waittime < 43)
        character.animationpoint = new Point(4, 0);
      else
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
        this._rect = new Rectangle(168, 0, 56, 48);
        dg.DrawImage(dg, "chipgraphic3", this._rect, true, p, Color.White);
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
      int x = character.animationpoint.X * (character.Wide * 2);
      int height1 = character.Height;
      int width = character.Wide * 2;
      int height2 = character.Height;
      this._rect = new Rectangle(x, 0, width, height2);
      this._position = new Vector2(character.positionDirect.X + character.Wide / 2 * this.UnionRebirth(character.union) + Shake.X, character.positionDirect.Y + Shake.Y);
      dg.DrawImage(dg, "Lances", this._rect, false, this._position, character.union == Panel.COLOR.blue, Color.White);
    }
  }
}

