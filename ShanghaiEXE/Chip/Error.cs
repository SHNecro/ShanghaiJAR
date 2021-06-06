using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEffect;
using Common.Vectors;
using System.Drawing;

namespace NSChip
{
    internal class Error : ChipBase
    {
        public Error(IAudioEngine s)
          : base(s)
        {
            this.printIcon = false;
            this.number = 0;
            this.name = NSGame.ShanghaiEXE.Translate("Chip.ErrorName");
            this.element = ChipBase.ELEMENT.normal;
            this.power = 0;
            this.subpower = 0;
            this._break = false;
            this.powerprint = false;
            this.shadow = false;
            this.code[0] = ChipFolder.CODE.none;
            this.code[1] = ChipFolder.CODE.none;
            this.code[2] = ChipFolder.CODE.none;
            this.code[3] = ChipFolder.CODE.none;
            var informationDialogue = NSGame.ShanghaiEXE.Translate("Chip.ErrorDesc");
            this.information[0] = informationDialogue[0];
            this.information[1] = informationDialogue[1];
            this.information[2] = informationDialogue[2];
        }

        public override void Action(CharacterBase character, SceneBattle battle)
        {
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
                this._rect = new Rectangle(23 * 56, 0, 56, 48);
                dg.DrawImage(dg, "chipgraphic23", this._rect, true, p, Color.White);
            }
            base.GraphicsRender(dg, p, c, printgraphics, printstatus);
        }

        public override void IconRender(
          IRenderer dg,
          Vector2 p,
          bool select,
          bool custom,
          int c,
          bool noicon)
        {
        }

        public override void Render(IRenderer dg, CharacterBase player)
        {
        }
    }
}

