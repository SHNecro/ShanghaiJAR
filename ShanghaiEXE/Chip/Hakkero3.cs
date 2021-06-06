using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSChip
{
    internal class Hakkero3 : Hakkero1
  {
    private const int shotend = 68;

    public Hakkero3(IAudioEngine s)
      : base(s)
    {
      this.rockOnPoint = new Point(-3, 0);
      this.number = 13;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.Hakkero3Name");
      this.element = ChipBase.ELEMENT.eleki;
      this.power = 150;
      this.regsize = 39;
      this.reality = 3;
      this.subpower = 0;
      this._break = false;
      this.shild = true;
      this.powerprint = true;
      this.code[0] = ChipFolder.CODE.D;
      this.code[1] = ChipFolder.CODE.F;
      this.code[2] = ChipFolder.CODE.V;
      this.code[3] = ChipFolder.CODE.Y;
      var information = NSGame.ShanghaiEXE.Translate("Chip.Hakkero3Desc");
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
        this._rect = new Rectangle(560, 0, 56, 48);
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

