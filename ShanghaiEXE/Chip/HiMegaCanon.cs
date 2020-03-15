using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;
using SlimDX;
using System.Drawing;

namespace NSChip
{
    internal class HiMegaCanon : HiCanon
  {
    private const int shotend = 28;

    public HiMegaCanon(IAudioEngine s)
      : base(s)
    {
      this.number = 272;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.HiMegaCanonName");
      this.element = ChipBase.ELEMENT.normal;
      this.power = 400;
      this.regsize = 12;
      this.reality = 1;
      this.subpower = 0;
      this._break = false;
      this.powerprint = true;
      this.code[0] = ChipFolder.CODE.none;
      this.code[1] = ChipFolder.CODE.none;
      this.code[2] = ChipFolder.CODE.none;
      this.code[3] = ChipFolder.CODE.none;
      var information = NSGame.ShanghaiEXE.Translate("Chip.HiMegaCanonDesc");
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
      if (!printgraphics)
        return;
      string[] strArray = new string[3]
      {
        ShanghaiEXE.Translate("Chip.ProgramAdvanceHiMegaCanonCombo1Line1"),
        ShanghaiEXE.Translate("Chip.ProgramAdvanceHiMegaCanonCombo1Line2"),
        ShanghaiEXE.Translate("Chip.ProgramAdvanceHiMegaCanonCombo1Line3")
      };
      for (int index = 0; index < strArray.Length; ++index)
      {
        this._position = new Vector2(p.X - 12f, p.Y - 8f + index * 16);
        this.TextRender(dg, strArray[index], false, this._position, false, Color.LightBlue);
      }
    }

    public override void Render(IRenderer dg, CharacterBase character)
    {
      if (character.waittime < 5)
        return;
      this._rect = new Rectangle(8 * character.Wide, 5 * character.Height, character.Wide, character.Height);
      this._position = new Vector2(character.positionDirect.X + Shake.X, character.positionDirect.Y + Shake.Y);
      if (character.waittime < 10)
        this._rect.X -= character.Wide * 2;
      else if (character.waittime >= 15 && character.waittime < 28)
        this._position.X -= 2 * this.UnionRebirth(character.union);
      dg.DrawImage(dg, "weapons", this._rect, false, this._position, character.union == Panel.COLOR.blue, Color.White);
    }
  }
}


