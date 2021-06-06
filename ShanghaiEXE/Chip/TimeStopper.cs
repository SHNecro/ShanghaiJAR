using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEffect;
using Common.Vectors;
using System.Drawing;

namespace NSChip
{
    internal class TimeStopper : ChipBase
  {
    private const int speed = 2;

    public TimeStopper(IAudioEngine s)
      : base(s)
    {
      this.number = 157;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.TimeStopperName");
      this.element = ChipBase.ELEMENT.normal;
      this.power = 0;
      this.subpower = 0;
      this.regsize = 39;
      this.reality = 5;
      this.plusing = true;
      this._break = false;
      this.shadow = false;
      this.powerprint = false;
      this.code[0] = ChipFolder.CODE.asterisk;
      this.code[1] = ChipFolder.CODE.asterisk;
      this.code[2] = ChipFolder.CODE.asterisk;
      this.code[3] = ChipFolder.CODE.asterisk;
      var information = NSGame.ShanghaiEXE.Translate("Chip.TimeStopperDesc");
      this.information[0] = information[0];
      this.information[1] = information[1];
      this.information[2] = information[2];
      this.Init();
    }

    public override void Action(CharacterBase character, SceneBattle battle)
    {
      if (character.waittime != 1)
        return;
      this.sound.PlaySE(SoundEffect.heat);
      Vector2 pd = new Vector2(character.positionDirect.X - 8f, character.positionDirect.Y - 32f);
      battle.effects.Add(new Smoke(this.sound, battle, pd, character.position, ChipBase.ELEMENT.normal));
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
        this._rect = new Rectangle(392, 0, 56, 48);
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

    public override void Render(IRenderer dg, CharacterBase player)
    {
    }
  }
}

