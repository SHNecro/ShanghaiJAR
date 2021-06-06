using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSChip
{
    internal class SpannerManV3 : SpannerManV1
  {
    private const int speed = 2;
    private new Point animePoint;

    public SpannerManV3(IAudioEngine s)
      : base(s)
    {
      this.navi = true;
      this.number = 202;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.SpannerManV3Name");
      this.element = ChipBase.ELEMENT.normal;
      this.power = 120;
      this.subpower = 0;
      this.regsize = 64;
      this.reality = 5;
      this._break = false;
      this.shadow = false;
      this.powerprint = true;
      this.code[0] = ChipFolder.CODE.S;
      this.code[1] = ChipFolder.CODE.N;
      this.code[2] = ChipFolder.CODE.S;
      this.code[3] = ChipFolder.CODE.N;
      var information = NSGame.ShanghaiEXE.Translate("Chip.SpannerManV3Desc");
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
        this._rect = new Rectangle(616, 0, 56, 48);
        dg.DrawImage(dg, "chipgraphic15", this._rect, true, p, Color.White);
      }
      base.GraphicsRender(dg, p, c, false, printstatus);
    }
  }
}

