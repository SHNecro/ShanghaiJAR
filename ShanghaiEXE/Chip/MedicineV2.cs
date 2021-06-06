using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSChip
{
    internal class MedicineV2 : MedicineV1
  {
    private const int interval = 20;
    private const int speed = 2;

    public MedicineV2(IAudioEngine s)
      : base(s)
    {
      this.navi = true;
      this.number = 222;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.MedicineV2Name");
      this.element = ChipBase.ELEMENT.poison;
      this.power = 90;
      this.subpower = 10;
      this.regsize = 41;
      this.reality = 4;
      this._break = false;
      this.shadow = false;
      this.powerprint = true;
      this.code[0] = ChipFolder.CODE.M;
      this.code[1] = ChipFolder.CODE.P;
      this.code[2] = ChipFolder.CODE.M;
      this.code[3] = ChipFolder.CODE.P;
      var information = NSGame.ShanghaiEXE.Translate("Chip.MedicineV2Desc");
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
        dg.DrawImage(dg, "chipgraphic12", this._rect, true, p, Color.White);
      }
      base.GraphicsRender(dg, p, c, false, printstatus);
    }
  }
}

