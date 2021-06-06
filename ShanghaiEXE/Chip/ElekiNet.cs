using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSChip
{
    internal class ElekiNet : TrapNet
  {
    private const int start = 3;
    private const int speed = 3;

    public ElekiNet(IAudioEngine s)
      : base(s)
    {
      this.rockOnPoint = new Point(-3, 0);
      this.number = 39;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.ElekiNetName");
      this.element = ChipBase.ELEMENT.eleki;
      this.power = 200;
      this.subpower = 0;
      this.regsize = 44;
      this.reality = 4;
      this.obje = true;
      this._break = false;
      this.powerprint = true;
      this.code[0] = ChipFolder.CODE.D;
      this.code[1] = ChipFolder.CODE.E;
      this.code[2] = ChipFolder.CODE.M;
      this.code[3] = ChipFolder.CODE.Q;
      var information = NSGame.ShanghaiEXE.Translate("Chip.ElekiNetDesc");
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
        this._rect = new Rectangle(840, 0, 56, 48);
        dg.DrawImage(dg, "chipgraphic14", this._rect, true, p, Color.White);
      }
      base.GraphicsRender(dg, p, c, false, printstatus);
    }

    public override void Render(IRenderer dg, CharacterBase character)
    {
      if (character.waittime >= 6)
        return;
      this._rect = new Rectangle(112, 0, 16, 16);
      Point point = new Point();
      if (character.waittime <= 3)
      {
        point.X = -22 * this.UnionRebirth(character.union);
        point.Y = 22;
      }
      else
      {
        point.X = -10 * this.UnionRebirth(character.union);
        point.Y = 4;
      }
      double x1 = character.positionDirect.X;
      Point shake = this.Shake;
      double x2 = shake.X;
      double num1 = x1 + x2 + point.X;
      double y1 = character.positionDirect.Y;
      shake = this.Shake;
      double y2 = shake.Y;
      double num2 = y1 + y2 + point.Y;
      this._position = new Vector2((float) num1, (float) num2);
      dg.DrawImage(dg, "bombs", this._rect, false, this._position, Color.White);
    }
  }
}

