using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSChip
{
    internal class OmegaSaber : ZSaber
  {
    private int[] motionX = new int[14]
    {
      0,
      2,
      3,
      3,
      2,
      3,
      3,
      1,
      1,
      1,
      1,
      1,
      1,
      1
    };
    private int[] motionY = new int[14]
    {
      1,
      1,
      1,
      1,
      2,
      2,
      2,
      3,
      1,
      0,
      0,
      0,
      0,
      0
    };
    private const int start = 3;
    private const int speed = 2;
    private int waittime;

    public OmegaSaber(IAudioEngine s)
      : base(s)
    {
      this.rockOnPoint = new Point(-1, 0);
      this.infight = true;
      this.swordtype = true;
      this.number = 370;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.OmegaSaberName");
      this.element = ChipBase.ELEMENT.normal;
      this.power = 100;
      this.subpower = 0;
      this.regsize = 99;
      this.reality = 5;
      this._break = false;
      this.powerprint = true;
      this.code[0] = ChipFolder.CODE.O;
      this.code[1] = ChipFolder.CODE.O;
      this.code[2] = ChipFolder.CODE.O;
      this.code[3] = ChipFolder.CODE.X;
      var information = NSGame.ShanghaiEXE.Translate("Chip.OmegaSaberDesc");
      this.information[0] = information[0];
      this.information[1] = information[1];
      this.information[2] = information[2];
      this.Init();
    }

    public override void Init()
    {
      base.Init();
      this.sortNumber = 138.5f;
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
        this._rect = new Rectangle(368, 80 + num4 * 96, 16, 16);
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
        this._rect = new Rectangle(1120, 0, 56, 48);
        dg.DrawImage(dg, "chipgraphic17", this._rect, true, p, Color.White);
      }
      base.GraphicsRender(dg, p, c, false, printstatus);
    }
  }
}

