using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSChip
{
    internal class ChargeCanon3 : ChargeCanon1
  {
    private const int shotend = 128;
    private bool anime;

    public ChargeCanon3(IAudioEngine s)
      : base(s)
    {
      this.rockOnPoint = new Point(-3, 0);
      this.number = 9;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.ChargeCanon3Name");
      this.element = ChipBase.ELEMENT.normal;
      this.power = 300;
      this.regsize = 64;
      this.reality = 4;
      this.subpower = 0;
      this._break = true;
      this.powerprint = true;
      this.code[0] = ChipFolder.CODE.D;
      this.code[1] = ChipFolder.CODE.T;
      this.code[2] = ChipFolder.CODE.U;
      this.code[3] = ChipFolder.CODE.V;
      var information = NSGame.ShanghaiEXE.Translate("Chip.ChargeCanon3Desc");
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
        dg.DrawImage(dg, "chipgraphic21", this._rect, true, p, Color.White);
      }
      base.GraphicsRender(dg, p, c, false, printstatus);
    }
  }
}

