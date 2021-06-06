using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSChip
{
    internal class FlaeGun2 : FlaeGun1
  {
    private bool open;
    private const int shotend = 10;

    public FlaeGun2(IAudioEngine s)
      : base(s)
    {
      this.rockOnPoint = new Point(-4, 0);
      this.number = 79;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.FlaeGun2Name");
      this.element = ChipBase.ELEMENT.heat;
      this.power = 130;
      this.subpower = 0;
      this.regsize = 42;
      this.reality = 3;
      this._break = false;
      this.powerprint = true;
      this.code[0] = ChipFolder.CODE.K;
      this.code[1] = ChipFolder.CODE.M;
      this.code[2] = ChipFolder.CODE.P;
      this.code[3] = ChipFolder.CODE.U;
      var information = NSGame.ShanghaiEXE.Translate("Chip.FlaeGun2Desc");
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
        dg.DrawImage(dg, "chipgraphic21", this._rect, true, p, Color.White);
      }
      base.GraphicsRender(dg, p, c, false, printstatus);
    }
  }
}

