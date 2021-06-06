using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSChip
{
    internal class ChargeCanon2 : ChargeCanon1
  {
    private const int shotend = 128;
    private bool anime;

    public ChargeCanon2(IAudioEngine s)
      : base(s)
    {
      this.rockOnPoint = new Point(-3, 0);
      this.number = 8;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.ChargeCanon2Name");
      this.element = ChipBase.ELEMENT.normal;
      this.power = 250;
      this.regsize = 47;
      this.reality = 3;
      this.subpower = 0;
      this._break = true;
      this.powerprint = true;
      this.code[0] = ChipFolder.CODE.B;
      this.code[1] = ChipFolder.CODE.E;
      this.code[2] = ChipFolder.CODE.L;
      this.code[3] = ChipFolder.CODE.T;
      var information = NSGame.ShanghaiEXE.Translate("Chip.ChargeCanon2Desc");
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
        dg.DrawImage(dg, "chipgraphic21", this._rect, true, p, Color.White);
      }
      base.GraphicsRender(dg, p, c, false, printstatus);
    }
  }
}

