using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSChip
{
    internal class DragnoBreath3 : DragnoBreath1
  {
    private bool fire;
    private const int shotend = 28;
    private int count;

    public DragnoBreath3(IAudioEngine s)
      : base(s)
    {
      this.rockOnPoint = new Point(-2, 0);
      this.number = 99;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.DragnoBreath3Name");
      this.element = ChipBase.ELEMENT.heat;
      this.power = 210;
      this.regsize = 94;
      this.reality = 5;
      this.subpower = 0;
      this._break = false;
      this.powerprint = true;
      this.code[0] = ChipFolder.CODE.H;
      this.code[1] = ChipFolder.CODE.U;
      this.code[2] = ChipFolder.CODE.V;
      this.code[3] = ChipFolder.CODE.W;
      var information = NSGame.ShanghaiEXE.Translate("Chip.DragnoBreath3Desc");
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
        dg.DrawImage(dg, "chipgraphic16", this._rect, true, p, Color.White);
      }
      base.GraphicsRender(dg, p, c, false, printstatus);
    }

    public override void Render(IRenderer dg, CharacterBase character)
    {
      if (!this.fire)
      {
        if (character.waittime < 5 && !this.fire)
          return;
        this._rect = new Rectangle(1320, 360, character.Wide, character.Height);
        double x1 = character.positionDirect.X;
        Point shake = this.Shake;
        double x2 = shake.X;
        double num1 = x1 + x2;
        double y1 = character.positionDirect.Y;
        shake = this.Shake;
        double y2 = shake.Y;
        double num2 = y1 + y2;
        this._position = new Vector2((float) num1, (float) num2);
        if (character.waittime >= 10 && (character.waittime >= 15 && character.waittime < 28))
          this._position.X -= 2 * this.UnionRebirth(character.union);
        dg.DrawImage(dg, "weapons", this._rect, false, this._position, character.union == Panel.COLOR.blue, Color.White);
      }
      else
      {
        this._rect = new Rectangle(1320, 360, character.Wide, character.Height);
        this._position = new Vector2(character.positionDirect.X + Shake.X, character.positionDirect.Y + Shake.Y);
        if (character.animationpoint.X == 6)
        {
          this._rect.X += 120;
          this._position.X -= this.UnionRebirth(character.union);
        }
        dg.DrawImage(dg, "weapons", this._rect, false, this._position, character.union == Panel.COLOR.blue, Color.White);
      }
    }
  }
}

