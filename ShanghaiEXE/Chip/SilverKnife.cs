using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSChip
{
    internal class SilverKnife : Knife
  {
    private const int start = 3;
    private const int speed = 1;

    public SilverKnife(IAudioEngine s)
      : base(s)
    {
      this.rockOnPoint = new Point(-1, 0);
      this.infight = true;
      this.swordtype = true;
      this.number = 60;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.SilverKnifeName");
      this.element = ChipBase.ELEMENT.normal;
      this.power = 130;
      this.subpower = 0;
      this.regsize = 31;
      this.reality = 3;
      this._break = true;
      this.powerprint = true;
      this.code[0] = ChipFolder.CODE.C;
      this.code[1] = ChipFolder.CODE.L;
      this.code[2] = ChipFolder.CODE.S;
      this.code[3] = ChipFolder.CODE.asterisk;
      var information = NSGame.ShanghaiEXE.Translate("Chip.SilverKnifeDesc");
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
        dg.DrawImage(dg, "chipgraphic1", this._rect, true, p, Color.White);
      }
      base.GraphicsRender(dg, p, c, false, printstatus);
    }
  }
}

