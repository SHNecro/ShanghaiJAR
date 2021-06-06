using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSChip
{
    internal class TankmanV3 : TankmanV1
  {
    public TankmanV3(IAudioEngine s)
      : base(s)
    {
      this.navi = true;
      this.number = 199;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.TankmanV3Name");
      this.element = ChipBase.ELEMENT.normal;
      this.power = 160;
      this.subpower = 40;
      this.regsize = 71;
      this.reality = 5;
      this._break = true;
      this.shadow = false;
      this.powerprint = true;
      this.code[0] = ChipFolder.CODE.T;
      this.code[1] = ChipFolder.CODE.R;
      this.code[2] = ChipFolder.CODE.T;
      this.code[3] = ChipFolder.CODE.R;
      var information = NSGame.ShanghaiEXE.Translate("Chip.TankmanV3Desc");
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
        this._rect = new Rectangle(784, 0, 56, 48);
        dg.DrawImage(dg, "chipgraphic12", this._rect, true, p, Color.White);
      }
      base.GraphicsRender(dg, p, c, false, printstatus);
    }
  }
}

