using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSChip
{
    internal class TankmanV2 : TankmanV1
  {
    public TankmanV2(IAudioEngine s)
      : base(s)
    {
      this.navi = true;
      this.number = 198;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.TankmanV2Name");
      this.element = ChipBase.ELEMENT.normal;
      this.power = 130;
      this.subpower = 30;
      this.regsize = 42;
      this.reality = 4;
      this._break = true;
      this.shadow = false;
      this.powerprint = true;
      this.code[0] = ChipFolder.CODE.T;
      this.code[1] = ChipFolder.CODE.R;
      this.code[2] = ChipFolder.CODE.T;
      this.code[3] = ChipFolder.CODE.R;
      var information = NSGame.ShanghaiEXE.Translate("Chip.TankmanV2Desc");
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
        this._rect = new Rectangle(728, 0, 56, 48);
        dg.DrawImage(dg, "chipgraphic12", this._rect, true, p, Color.White);
      }
      base.GraphicsRender(dg, p, c, false, printstatus);
    }
  }
}

