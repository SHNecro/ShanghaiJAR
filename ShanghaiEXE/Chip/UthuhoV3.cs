using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSChip
{
    internal class UthuhoV3 : UthuhoV1
  {
    private const int start = 44;
    private const int speed = 2;
    protected new int color;

    public UthuhoV3(IAudioEngine s)
      : base(s)
    {
      this.navi = true;
      this.number = 254;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.UthuhoV3Name");
      this.element = ChipBase.ELEMENT.heat;
      this.power = 180;
      this.subpower = 0;
      this.regsize = 66;
      this.reality = 5;
      this._break = true;
      this.shadow = false;
      this.powerprint = true;
      this.color = 0;
      this.code[0] = ChipFolder.CODE.U;
      this.code[1] = ChipFolder.CODE.R;
      this.code[2] = ChipFolder.CODE.U;
      this.code[3] = ChipFolder.CODE.R;
      var information = NSGame.ShanghaiEXE.Translate("Chip.UthuhoV3Desc");
      this.information[0] = information[0];
      this.information[1] = information[1];
      this.information[2] = information[2];
      this.Init();
    }

    public override void Init()
    {
      base.Init();
      this.sortNumber = 252.6f;
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
        this._rect = new Rectangle(448, 0, 56, 48);
        dg.DrawImage(dg, "chipgraphic22", this._rect, true, p, Color.White);
      }
      base.GraphicsRender(dg, p, c, false, printstatus);
    }
  }
}

