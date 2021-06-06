using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSChip
{
    internal class SpannerManV2 : SpannerManV1
  {
    private const int speed = 2;
    private new Point animePoint;

    public SpannerManV2(IAudioEngine s)
      : base(s)
    {
      this.navi = true;
      this.number = 201;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.SpannerManV2Name");
      this.element = ChipBase.ELEMENT.normal;
      this.power = 100;
      this.subpower = 0;
      this.regsize = 40;
      this.reality = 4;
      this._break = false;
      this.shadow = false;
      this.powerprint = true;
      this.code[0] = ChipFolder.CODE.S;
      this.code[1] = ChipFolder.CODE.N;
      this.code[2] = ChipFolder.CODE.S;
      this.code[3] = ChipFolder.CODE.N;
      var information = NSGame.ShanghaiEXE.Translate("Chip.SpannerManV2Desc");
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
        this._rect = new Rectangle(560, 0, 56, 48);
        dg.DrawImage(dg, "chipgraphic15", this._rect, true, p, Color.White);
      }
      base.GraphicsRender(dg, p, c, false, printstatus);
    }
  }
}

