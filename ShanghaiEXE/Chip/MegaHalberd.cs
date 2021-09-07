using NSAttack;
using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;
using Common.Vectors;
using System.Drawing;

namespace NSChip
{
    internal class MegaHalberd : ChipBase
    {
        private const int start = 5;
        private const int speed = 4;

        public MegaHalberd(IAudioEngine s)
          : base(s)
        {
            this.rockOnPoint = new Point(-1, 0);
            this.infight = true;
            this.number = 273;
            this.name = NSGame.ShanghaiEXE.Translate("Chip.MegaHalberdName");
            this.element = ChipBase.ELEMENT.normal;
            this.power = 300;
            this.subpower = 0;
            this._break = false;
            this.powerprint = true;
            this.code[0] = ChipFolder.CODE.none;
            this.code[1] = ChipFolder.CODE.none;
            this.code[2] = ChipFolder.CODE.none;
            this.code[3] = ChipFolder.CODE.none;
            var information = NSGame.ShanghaiEXE.Translate("Chip.MegaHalberdDesc");
            this.information[0] = information[0];
            this.information[1] = information[1];
            this.information[2] = information[2];
            this.Init();
        }

        public override void Action(CharacterBase character, SceneBattle battle)
        {
            if (character.waittime == 5)
                this.sound.PlaySE(SoundEffect.sword);
            character.animationpoint = CharacterAnimation.SworsAnimation(character.waittime);
            if (character.waittime >= 30)
                base.Action(character, battle);
            if (character.waittime != 10)
                return;
            int num = this.power + this.pluspower;
            character.parent.attacks.Add(this.Paralyze(new Halberd(this.sound, character.parent, character.position.X + this.UnionRebirth(character.union), character.position.Y, character.union, this.Power(character), 3, this.element, false)));
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
            var strArray = new string[0];
            switch (c % 4)
            {
                case 0:
                    this._rect = new Rectangle(848, 320, 74, 79);
                    dg.DrawImage(dg, "menuwindows", this._rect, true, p - new Vector2(9, 16), Color.White);
                    this._rect = new Rectangle(56 * 2, 48 * 0, 56, 48);
                    dg.DrawImage(dg, "pagraphic1", this._rect, true, p, Color.White);
                    return;
                case 1:
                    strArray = new string[]
                    {
                        ShanghaiEXE.Translate("Chip.ProgramAdvanceMegaHalberdCombo1Line1"),
                        ShanghaiEXE.Translate("Chip.ProgramAdvanceMegaHalberdCombo1Line2"),
                        ShanghaiEXE.Translate("Chip.ProgramAdvanceMegaHalberdCombo1Line3")
                    };
                    break;
                case 2:
                    strArray = new string[]
                    {
                        ShanghaiEXE.Translate("Chip.ProgramAdvanceMegaHalberdCombo2Line1"),
                        ShanghaiEXE.Translate("Chip.ProgramAdvanceMegaHalberdCombo2Line2"),
                        ShanghaiEXE.Translate("Chip.ProgramAdvanceMegaHalberdCombo2Line3")
                    };
                    break;
                case 3:
                    strArray = new string[]
                    {
                        ShanghaiEXE.Translate("Chip.ProgramAdvanceMegaHalberdCombo3Line1"),
                        ShanghaiEXE.Translate("Chip.ProgramAdvanceMegaHalberdCombo3Line2"),
                        ShanghaiEXE.Translate("Chip.ProgramAdvanceMegaHalberdCombo3Line3")
                    };
                    break;
            }
            for (int index = 0; index < strArray.Length; ++index)
            {
                this._position = new Vector2(p.X - 12f, p.Y - 8f + index * 16);
                this.TextRender(dg, strArray[index], false, this._position, false, Color.LightBlue);
            }
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
                int num = 0;
                if (select)
                    num = 1;
                this._rect = new Rectangle(624, 80 + num * 96, 16, 16);
                dg.DrawImage(dg, "chipicon", this._rect, true, p, Color.White);
            }
            base.IconRender(dg, p, select, custom, c, noicon);
        }

        public override void Render(IRenderer dg, CharacterBase character)
        {
            this._rect = new Rectangle(character.animationpoint.X * character.Wide, 2 * character.Height, character.Wide, character.Height);
            this._position = new Vector2(character.positionDirect.X + Shake.X, character.positionDirect.Y + Shake.Y);
            dg.DrawImage(dg, "slash", this._rect, false, this._position, character.union == Panel.COLOR.blue, Color.White);
        }
    }
}


