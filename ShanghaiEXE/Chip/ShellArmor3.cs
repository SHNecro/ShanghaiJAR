using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSChip
{
    internal class ShellArmor3 : ShellArmor1
  {
    private const int speed = 2;

    public ShellArmor3(IAudioEngine s)
      : base(s)
    {
      this.number = 29;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.ShellArmor3Name");
      this.element = ChipBase.ELEMENT.normal;
      this.power = 0;
      this.subpower = 7;
      this.regsize = 51;
      this.reality = 3;
      this._break = false;
      this.shadow = false;
      this.powerprint = false;
      this.shild = true;
      this.code[0] = ChipFolder.CODE.B;
      this.code[1] = ChipFolder.CODE.H;
      this.code[2] = ChipFolder.CODE.K;
      this.code[3] = ChipFolder.CODE.R;
      var information = NSGame.ShanghaiEXE.Translate("Chip.ShellArmor3Desc");
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
        this._rect = new Rectangle(280, 0, 56, 48);
        dg.DrawImage(dg, "chipgraphic21", this._rect, true, p, Color.White);
      }
      base.GraphicsRender(dg, p, c, false, printstatus);
    }
  }
}

