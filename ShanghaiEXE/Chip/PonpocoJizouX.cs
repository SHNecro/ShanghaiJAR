using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSChip
{
    internal class PonpocoJizouX : PonpocoJizou1
  {
    private const int speed = 2;

    public PonpocoJizouX(IAudioEngine s)
      : base(s)
    {
      this.number = 368;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.PonpocoJizouXName");
      this.element = ChipBase.ELEMENT.eleki;
      this.power = 200;
      this.subpower = 0;
      this.regsize = 99;
      this.reality = 5;
      this._break = true;
      this.obje = true;
      this.shadow = false;
      this.powerprint = true;
      this.code[0] = ChipFolder.CODE.P;
      this.code[1] = ChipFolder.CODE.P;
      this.code[2] = ChipFolder.CODE.P;
      this.code[3] = ChipFolder.CODE.X;
      var information = NSGame.ShanghaiEXE.Translate("Chip.PonpocoJizouXDesc");
      this.information[0] = information[0];
      this.information[1] = information[1];
      this.information[2] = information[2];
      this.Init();
    }

    public override void Init()
    {
      base.Init();
      this.sortNumber = 111.5f;
    }

    public override void IconRender(
      IRenderer dg,
      Vector2 p,
      bool select,
      bool custom,
      int c,
      bool noicon)
    {
      if (!noicon)
      {
        int num1 = this.number - 1;
        int num2 = num1 % 40;
        int num3 = num1 / 40;
        int num4 = 0;
        if (select)
          num4 = 1;
        this._rect = new Rectangle(336, 80 + num4 * 96, 16, 16);
        dg.DrawImage(dg, "chipicon", this._rect, true, p, Color.White);
      }
      base.IconRender(dg, p, select, custom, c, true);
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
        this._rect = new Rectangle(1008, 0, 56, 48);
        dg.DrawImage(dg, "chipgraphic17", this._rect, true, p, Color.White);
      }
      base.GraphicsRender(dg, p, c, false, printstatus);
    }
  }
}

