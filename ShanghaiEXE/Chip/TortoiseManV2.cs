using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSChip
{
    internal class TortoiseManV2 : TortoiseManV1
  {
    private const int speed = 2;

    public TortoiseManV2(IAudioEngine s)
      : base(s)
    {
      this.navi = true;
      this.number = 210;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.TortoiseManV2Name");
      this.element = ChipBase.ELEMENT.earth;
      this.power = 110;
      this.subpower = 0;
      this.regsize = 62;
      this.reality = 4;
      this._break = false;
      this.shadow = false;
      this.powerprint = true;
      this.code[0] = ChipFolder.CODE.T;
      this.code[1] = ChipFolder.CODE.H;
      this.code[2] = ChipFolder.CODE.T;
      this.code[3] = ChipFolder.CODE.H;
      var information = NSGame.ShanghaiEXE.Translate("Chip.TortoiseManV2Desc");
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
        this._rect = new Rectangle(392, 0, 56, 48);
        dg.DrawImage(dg, "chipgraphic15", this._rect, true, p, Color.White);
      }
      base.GraphicsRender(dg, p, c, false, printstatus);
    }
  }
}

