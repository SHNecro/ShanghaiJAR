using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSChip
{
    internal class SeedCanonX : SeedCanon1
  {
    private int posiminus = 0;
    private const int shotend = 58;
    private const int shotstart = 40;

    public SeedCanonX(IAudioEngine s)
      : base(s)
    {
      this.rockOnPoint = new Point(-3, 0);
      this.number = 371;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.SeedCanonXName");
      this.element = ChipBase.ELEMENT.leaf;
      this.power = 270;
      this.regsize = 99;
      this.reality = 5;
      this.subpower = 0;
      this._break = false;
      this.powerprint = true;
      this.code[0] = ChipFolder.CODE.S;
      this.code[1] = ChipFolder.CODE.S;
      this.code[2] = ChipFolder.CODE.S;
      this.code[3] = ChipFolder.CODE.X;
      var information = NSGame.ShanghaiEXE.Translate("Chip.SeedCanonXDesc");
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
        40,
        42,
        45
      }, new int[7]{ 4, 5, 6, 5, 2, 1, 0 }, 0, waittime);
    }

    public override void Init()
    {
      base.Init();
      this.sortNumber = 6.5f;
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
        this._rect = new Rectangle(1512, 0, 56, 48);
        dg.DrawImage(dg, "chipgraphic17", this._rect, true, p, Color.White);
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
        this._rect = new Rectangle(384, 80 + num4 * 96, 16, 16);
        dg.DrawImage(dg, "chipicon", this._rect, true, p, Color.White);
      }
      base.IconRender(dg, p, select, custom, c, true);
    }

    public override void Render(IRenderer dg, CharacterBase character)
    {
      if (character.waittime < 5)
        return;
      this._rect = new Rectangle(960, 720, character.Wide, character.Height);
      this._position = new Vector2(character.positionDirect.X + Shake.X, character.positionDirect.Y + Shake.Y);
      if (character.waittime < 10)
        this._rect.X = 360;
      else if (character.waittime < 11)
        this._rect.X = 480;
      else if (character.waittime < 12)
        this._rect.X = 600;
      else if (character.waittime < 13)
        this._rect.X = 720;
      else if (character.waittime < 14)
        this._rect.X = 840;
      else if (character.waittime < 15)
        this._rect.X = 960;
      else if (character.waittime > 40 && character.waittime < 42)
        this._rect.X = 1080;
      dg.DrawImage(dg, "weapons", this._rect, false, this._position, character.union == Panel.COLOR.blue, Color.White);
      if (character.waittime >= 40)
      {
        this._position = character.positionDirect;
        this._position.X += 48 * this.UnionRebirth(character.union);
        this._position.Y += 14f;
        this._rect = new Rectangle((character.waittime - 40) / 3 * 64, 32, 64, 64);
        dg.DrawImage(dg, "shot", this._rect, false, this._position, character.union == Panel.COLOR.blue, Color.White);
      }
    }
  }
}

