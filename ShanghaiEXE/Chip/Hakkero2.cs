using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSChip
{
    internal class Hakkero2 : Hakkero1
  {
    private const int shotend = 68;

    public Hakkero2(IAudioEngine s)
      : base(s)
    {
      this.rockOnPoint = new Point(-3, 0);
      this.number = 12;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.Hakkero2Name");
      this.element = ChipBase.ELEMENT.eleki;
      this.power = 100;
      this.regsize = 24;
      this.reality = 2;
      this.subpower = 0;
      this._break = false;
      this.shild = true;
      this.powerprint = true;
      this.code[0] = ChipFolder.CODE.C;
      this.code[1] = ChipFolder.CODE.M;
      this.code[2] = ChipFolder.CODE.R;
      this.code[3] = ChipFolder.CODE.W;
      var information = NSGame.ShanghaiEXE.Translate("Chip.Hakkero2Desc");
      this.information[0] = information[0];
      this.information[1] = information[1];
      this.information[2] = information[2];
      this.Init();
    }

    private Point Animation(int waittime)
    {
      return CharacterAnimation.Return(new int[4]
      {
        5,
        15,
        17,
        20
      }, new int[7]{ 4, 5, 6, 5, 2, 1, 0 }, 0, waittime);
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
        this._rect = new Rectangle(504, 0, 56, 48);
        dg.DrawImage(dg, "chipgraphic7", this._rect, true, p, Color.White);
      }
      base.GraphicsRender(dg, p, c, false, printstatus);
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
        this._rect = this.IconRect(select);
        dg.DrawImage(dg, "chipicon", this._rect, true, p, Color.White);
      }
      base.IconRender(dg, p, select, custom, c, noicon);
    }
  }
}

