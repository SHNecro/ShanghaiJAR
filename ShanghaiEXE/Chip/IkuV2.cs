using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSChip
{
    internal class IkuV2 : IkuV1
  {
    private const int speed = 2;
    private Point animePoint;

    public IkuV2(IAudioEngine s)
      : base(s)
    {
      this.navi = true;
      this.number = 225;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.IkuV2Name");
      this.element = ChipBase.ELEMENT.eleki;
      this.power = 130;
      this.subpower = 0;
      this.regsize = 41;
      this.reality = 4;
      this._break = false;
      this.shadow = false;
      this.powerprint = true;
      this.code[0] = ChipFolder.CODE.I;
      this.code[1] = ChipFolder.CODE.N;
      this.code[2] = ChipFolder.CODE.I;
      this.code[3] = ChipFolder.CODE.N;
      var information = NSGame.ShanghaiEXE.Translate("Chip.IkuV2Desc");
      this.information[0] = information[0];
      this.information[1] = information[1];
      this.information[2] = information[2];
      this.Init();
    }

    private Point Animation(int waittime)
    {
      return CharacterAnimation.ReturnKai(new int[7]
      {
        1,
        8,
        16,
        4,
        4,
        52,
        4
      }, new int[7]{ -1, 1, 8, 9, 10, 10, -1 }, 0, waittime);
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
        this._rect = new Rectangle(56, 0, 56, 48);
        dg.DrawImage(dg, "chipgraphic15", this._rect, true, p, Color.White);
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
        int num1 = this.number - 1;
        int num2 = num1 % 40;
        int num3 = num1 / 40;
        int num4 = 0;
        if (select)
          num4 = 1;
        this._rect = new Rectangle(480, 64 + num4 * 96, 16, 16);
        dg.DrawImage(dg, "chipicon", this._rect, true, p, Color.White);
      }
      base.IconRender(dg, p, select, custom, c, noicon);
    }
  }
}

