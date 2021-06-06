using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSChip
{
    internal class ShellArmor2 : ShellArmor1
  {
    private const int speed = 2;

    public ShellArmor2(IAudioEngine s)
      : base(s)
    {
      this.number = 28;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.ShellArmor2Name");
      this.element = ChipBase.ELEMENT.normal;
      this.power = 0;
      this.subpower = 5;
      this.regsize = 36;
      this.reality = 2;
      this._break = false;
      this.shadow = false;
      this.powerprint = false;
      this.shild = true;
      this.code[0] = ChipFolder.CODE.G;
      this.code[1] = ChipFolder.CODE.S;
      this.code[2] = ChipFolder.CODE.T;
      this.code[3] = ChipFolder.CODE.W;
      var information = NSGame.ShanghaiEXE.Translate("Chip.ShellArmor2Desc");
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
        this._rect = new Rectangle(224, 0, 56, 48);
        dg.DrawImage(dg, "chipgraphic21", this._rect, true, p, Color.White);
      }
      base.GraphicsRender(dg, p, c, false, printstatus);
    }
  }
}

