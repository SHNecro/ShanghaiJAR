using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSChip
{
    internal class ChainGun2 : ChainGun1
  {
    private Point[] target = new Point[3];
    private const int shotend = 10;
    private const int shotinterval = 4;
    private bool rockon;
    private int shot;

    public ChainGun2(IAudioEngine s)
      : base(s)
    {
      this.number = 95;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.ChainGun2Name");
      this.element = ChipBase.ELEMENT.normal;
      this.power = 50;
      this.regsize = 36;
      this.reality = 2;
      this.subpower = 0;
      this._break = false;
      this.shadow = false;
      this.powerprint = true;
      this.code[0] = ChipFolder.CODE.B;
      this.code[1] = ChipFolder.CODE.L;
      this.code[2] = ChipFolder.CODE.O;
      this.code[3] = ChipFolder.CODE.T;
      var information = NSGame.ShanghaiEXE.Translate("Chip.ChainGun2Desc");
      this.information[0] = information[0];
      this.information[1] = information[1];
      this.information[2] = information[2];
      this.Init();
    }

    public new static Point Animation(int waittime)
    {
      int[] interval = new int[12];
      for (int index = 0; index < 12; ++index)
        interval[index] = 4 * index;
      int[] xpoint = new int[14]
      {
        5,
        6,
        5,
        6,
        5,
        6,
        5,
        6,
        5,
        6,
        5,
        6,
        5,
        6
      };
      int y = 0;
      return CharacterAnimation.Return(interval, xpoint, y, waittime);
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
        this._rect = new Rectangle(784, 0, 56, 48);
        dg.DrawImage(dg, "chipgraphic4", this._rect, true, p, Color.White);
      }
      base.GraphicsRender(dg, p, c, false, printstatus);
    }

    public override void Render(IRenderer dg, CharacterBase character)
    {
      this._rect = new Rectangle(8 * character.Wide, 3 * character.Height, character.Wide, character.Height);
      this._position = new Vector2(character.positionDirect.X + Shake.X, character.positionDirect.Y + Shake.Y);
      if (character.animationpoint.X == 6)
      {
        this._rect.X += character.Wide;
        this._position.X -= 2 * this.UnionRebirth(character.union);
      }
      dg.DrawImage(dg, "weapons", this._rect, false, this._position, character.union == Panel.COLOR.blue, Color.White);
    }
  }
}

