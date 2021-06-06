using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSChip
{
    internal class ElekiDrill : DigDrill1
  {
    private const int start = 5;
    private const int speed = 3;

    public ElekiDrill(IAudioEngine s)
      : base(s)
    {
      this.rockOnPoint = new Point(-1, 0);
      this.infight = true;
      this.number = 77;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.ElekiDrillName");
      this.element = ChipBase.ELEMENT.eleki;
      this.power = 60;
      this.subpower = 0;
      this.regsize = 43;
      this.reality = 4;
      this._break = true;
      this.powerprint = true;
      this.code[0] = ChipFolder.CODE.I;
      this.code[1] = ChipFolder.CODE.K;
      this.code[2] = ChipFolder.CODE.N;
      this.code[3] = ChipFolder.CODE.U;
      var information = NSGame.ShanghaiEXE.Translate("Chip.ElekiDrillDesc");
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
        this._rect = new Rectangle(672, 0, 56, 48);
        dg.DrawImage(dg, "chipgraphic16", this._rect, true, p, Color.White);
      }
      base.GraphicsRender(dg, p, c, false, printstatus);
    }

    public override void Render(IRenderer dg, CharacterBase character)
    {
      if (character.waittime <= 11 || character.waittime >= 53)
        return;
      this._rect = new Rectangle(character.animationpoint.X * (character.Wide * 2) + this.drillAnime * (character.Wide * 2), 2 * character.Height, character.Wide * 2, character.Height);
      double num1 = character.positionDirect.X + (double) (character.Wide / 2 * this.UnionRebirth(character.union));
      Point shake = this.Shake;
      double x = shake.X;
      double num2 = num1 + x;
      double y1 = character.positionDirect.Y;
      shake = this.Shake;
      double y2 = shake.Y;
      double num3 = y1 + y2;
      this._position = new Vector2((float) num2, (float) num3);
      dg.DrawImage(dg, "Lances", this._rect, false, this._position, character.union == Panel.COLOR.blue, Color.White);
    }
  }
}

