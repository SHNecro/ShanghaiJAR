using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSChip
{
    internal class ShadowBody : ChipBase
  {
    private const int speed = 2;

    public ShadowBody(IAudioEngine s)
      : base(s)
    {
      this.number = 183;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.ShadowBodyName");
      this.element = ChipBase.ELEMENT.normal;
      this.power = 0;
      this.subpower = 0;
      this.regsize = 38;
      this.reality = 3;
      this._break = false;
      this.shadow = false;
      this.powerprint = false;
      this.code[0] = ChipFolder.CODE.D;
      this.code[1] = ChipFolder.CODE.N;
      this.code[2] = ChipFolder.CODE.S;
      this.code[3] = ChipFolder.CODE.asterisk;
      var information = NSGame.ShanghaiEXE.Translate("Chip.ShadowBodyDesc");
      this.information[0] = information[0];
      this.information[1] = information[1];
      this.information[2] = information[2];
      this.Init();
    }

    public override void Action(CharacterBase character, SceneBattle battle)
    {
      if (character.waittime != 1)
        return;
      this.sound.PlaySE(SoundEffect.lance);
      character.body = CharacterBase.BODY.Shadow;
      character.bodytime = 900;
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
        this._rect = new Rectangle(504, 0, 56, 48);
        dg.DrawImage(dg, "chipgraphic5", this._rect, true, p, Color.White);
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

