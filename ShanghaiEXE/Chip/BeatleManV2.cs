using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSChip
{
    internal class BeatleManV2 : BeatleManV1
  {
    private const int speed = 2;

    public BeatleManV2(IAudioEngine s)
      : base(s)
    {
      this.navi = true;
      this.number = 213;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.BeatleManV2Name");
      this.element = ChipBase.ELEMENT.earth;
      this.power = 120;
      this.subpower = 20;
      this.regsize = 49;
      this.reality = 4;
      this._break = false;
      this.shadow = false;
      this.powerprint = true;
      this.code[0] = ChipFolder.CODE.B;
      this.code[1] = ChipFolder.CODE.W;
      this.code[2] = ChipFolder.CODE.B;
      this.code[3] = ChipFolder.CODE.W;
      var information = NSGame.ShanghaiEXE.Translate("Chip.BeatleManV2Desc");
      this.information[0] = information[0];
      this.information[1] = information[1];
      this.information[2] = information[2];
      this.Init();
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
        this._rect = new Rectangle(224, 0, 56, 48);
        dg.DrawImage(dg, "chipgraphic19", this._rect, true, p, Color.White);
      }
      base.GraphicsRender(dg, p, c, false, printstatus);
    }
  }
}

