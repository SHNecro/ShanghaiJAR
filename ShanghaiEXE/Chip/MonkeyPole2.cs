using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSChip
{
    internal class MonkeyPole2 : MonkeyPole1
  {
    private const int speed = 2;

    public MonkeyPole2(IAudioEngine s)
      : base(s)
    {
      this.number = 131;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.MonkeyPole2Name");
      this.element = ChipBase.ELEMENT.leaf;
      this.power = 130;
      this.subpower = 0;
      this.regsize = 29;
      this.reality = 2;
      this._break = false;
      this.shadow = false;
      this.powerprint = true;
      this.code[0] = ChipFolder.CODE.F;
      this.code[1] = ChipFolder.CODE.O;
      this.code[2] = ChipFolder.CODE.P;
      this.code[3] = ChipFolder.CODE.U;
      this.color = 1;
      var information = NSGame.ShanghaiEXE.Translate("Chip.MonkeyPole2Desc");
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
        dg.DrawImage(dg, "chipgraphic16", this._rect, true, p, Color.White);
      }
      base.GraphicsRender(dg, p, c, false, printstatus);
    }
  }
}

