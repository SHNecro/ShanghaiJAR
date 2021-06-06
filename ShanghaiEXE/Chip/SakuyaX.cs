using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSChip
{
    internal class SakuyaX : SakuyaV1
  {
    private const int interval = 20;
    private const int speed = 2;

    public SakuyaX(IAudioEngine s)
      : base(s)
    {
      this.navi = true;
      this.number = 402;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.SakuyaXName");
      this.element = ChipBase.ELEMENT.normal;
      this.power = 60;
      this.subpower = 0;
      this.regsize = 99;
      this.reality = 5;
      this.swordtype = true;
      this._break = false;
      this.shadow = false;
      this.powerprint = true;
      this.code[0] = ChipFolder.CODE.S;
      this.code[1] = ChipFolder.CODE.S;
      this.code[2] = ChipFolder.CODE.S;
      this.code[3] = ChipFolder.CODE.X;
      var information = NSGame.ShanghaiEXE.Translate("Chip.SakuyaXDesc");
      this.information[0] = information[0];
      this.information[1] = information[1];
      this.information[2] = information[2];
      this.Init();
    }

    public override void Init()
    {
      base.Init();
      this.sortNumber = 196.5f;
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
        this._rect = new Rectangle(112, 0, 56, 48);
        dg.DrawImage(dg, "chipgraphic18", this._rect, true, p, Color.White);
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
        this._rect = new Rectangle(592, 80 + num4 * 96, 16, 16);
        dg.DrawImage(dg, "chipicon", this._rect, true, p, Color.White);
      }
      base.IconRender(dg, p, select, custom, c, true);
    }

    public override void Render(IRenderer dg, CharacterBase character)
    {
      if (character.animationpoint.X == -1)
      {
        this._rect = new Rectangle(48 * this.animePoint.X, 128, 48, 64);
        this._position = new Vector2((float) (character.position.X * 40.0 + 18.0), (float) (character.position.Y * 24.0 + 58.0));
        dg.DrawImage(dg, "sakuya", this._rect, false, this._position, character.union == Panel.COLOR.red, Color.White);
      }
      else
        this.BlackOutRender(dg, character.union);
    }
  }
}

