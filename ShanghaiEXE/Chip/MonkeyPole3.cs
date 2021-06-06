using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSChip
{
    internal class MonkeyPole3 : MonkeyPole1
  {
    private const int speed = 2;

    public MonkeyPole3(IAudioEngine s)
      : base(s)
    {
      this.number = 132;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.MonkeyPole3Name");
      this.element = ChipBase.ELEMENT.leaf;
      this.power = 160;
      this.subpower = 0;
      this.regsize = 34;
      this.reality = 3;
      this._break = false;
      this.shadow = false;
      this.powerprint = true;
      this.code[0] = ChipFolder.CODE.H;
      this.code[1] = ChipFolder.CODE.K;
      this.code[2] = ChipFolder.CODE.W;
      this.code[3] = ChipFolder.CODE.Y;
      this.color = 2;
      var information = NSGame.ShanghaiEXE.Translate("Chip.MonkeyPole3Desc");
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
        this._rect = new Rectangle(448, 0, 56, 48);
        dg.DrawImage(dg, "chipgraphic16", this._rect, true, p, Color.White);
      }
      base.GraphicsRender(dg, p, c, false, printstatus);
    }
  }
}

