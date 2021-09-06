using NSAttack;
using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEffect;
using NSGame;
using Common.Vectors;
using System.Drawing;

namespace NSChip
{
    internal class FlashBurn : ChipBase
    {
        private const int start = 1;
        private const int speed = 2;

        public FlashBurn(IAudioEngine s)
          : base(s)
        {
            this.number = 276;
            this.name = NSGame.ShanghaiEXE.Translate("Chip.FlashBurnName");
            this.element = ChipBase.ELEMENT.eleki;
            this.power = 250;
            this.subpower = 0;
            this.powerprint = true;
            this._break = true;
            this.code[0] = ChipFolder.CODE.none;
            this.code[1] = ChipFolder.CODE.none;
            this.code[2] = ChipFolder.CODE.none;
            this.code[3] = ChipFolder.CODE.none;
            var information = NSGame.ShanghaiEXE.Translate("Chip.FlashBurnDesc");
            this.information[0] = information[0];
            this.information[1] = information[1];
            this.information[2] = information[2];
            this.Init();
        }

        public override void Action(CharacterBase character, SceneBattle battle)
        {
            if (character.waittime <= 1)
                character.animationpoint = new Point(0, 1);
            else if (character.waittime <= 7)
                character.animationpoint = new Point((character.waittime - 1) / 2, 1);
            else if (character.waittime < 15)
                character.animationpoint = new Point(3, 1);
            else if (character.waittime == 15)
            {
                this.sound.PlaySE(SoundEffect.docking);
                battle.effects.Add(new ScreenFlash(this.sound, battle, character.positionDirect, character.position, this.element, 1, false));
                BombAttack bombAttack = new BombAttack(this.sound, character.parent, character.union == Panel.COLOR.red ? 0 : 5, 0, character.union, this.Power(character), 1, 1, new Point(5, 3), this.element);
                bombAttack.badstatus[3] = true;
                bombAttack.badstatustime[3] = 300;
                bombAttack.bright = false;
                bombAttack.effect = true;
                bombAttack.panelChange = false;
                bombAttack.canCounter = false;
                bombAttack.invincibility = true;
                bombAttack.knock = false;
                bombAttack.breaking = true;
                character.parent.attacks.Add(bombAttack);
            }
            else
            {
                if (character.waittime < 31)
                    return;
                base.Action(character, battle);
            }
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
                    this._rect = new Rectangle(56 * 5, 48 * 0, 56, 48);
                    dg.DrawImage(dg, "pagraphic1", this._rect, true, p, Color.White);
                    return;
                case 1:
                    strArray = new string[]
                    {
                        ShanghaiEXE.Translate("Chip.ProgramAdvanceFlashBurnCombo1Line1"),
                        ShanghaiEXE.Translate("Chip.ProgramAdvanceFlashBurnCombo1Line2"),
                        ShanghaiEXE.Translate("Chip.ProgramAdvanceFlashBurnCombo1Line3")
                    };
                    break;
                case 2:
                    strArray = new string[]
                    {
                        ShanghaiEXE.Translate("Chip.ProgramAdvanceFlashBurnCombo2Line1"),
                        ShanghaiEXE.Translate("Chip.ProgramAdvanceFlashBurnCombo2Line2"),
                        ShanghaiEXE.Translate("Chip.ProgramAdvanceFlashBurnCombo2Line3")
                    };
                    break;
                case 3:
                    strArray = new string[]
                    {
                        ShanghaiEXE.Translate("Chip.ProgramAdvanceFlashBurnCombo3Line1"),
                        ShanghaiEXE.Translate("Chip.ProgramAdvanceFlashBurnCombo3Line2"),
                        ShanghaiEXE.Translate("Chip.ProgramAdvanceFlashBurnCombo3Line3")
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
            base.IconRender(dg, p, select, custom, 0, noicon);
        }

        public override void Render(IRenderer dg, CharacterBase character)
        {
            this._rect = new Rectangle(character.animationpoint.X * character.Wide, 4 * character.Height, character.Wide, character.Height);
            this._position = new Vector2(character.positionDirect.X + Shake.X, character.positionDirect.Y + Shake.Y);
            dg.DrawImage(dg, character.picturename, this._rect, false, this._position, character.union == Panel.COLOR.blue, Color.White);
        }
    }
}


