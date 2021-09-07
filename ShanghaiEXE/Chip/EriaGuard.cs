using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEffect;
using Common.Vectors;
using System.Drawing;

namespace NSChip
{
    internal class EriaGuard : ChipBase
    {
        private const int start = 1;
        private const int speed = 2;

        public EriaGuard(IAudioEngine s)
          : base(s)
        {
            this.number = 159;
            this.name = NSGame.ShanghaiEXE.Translate("Chip.EriaGuardName");
            this.element = ChipBase.ELEMENT.normal;
            this.power = 0;
            this.subpower = 0;
            this.regsize = 32;
            this.reality = 3;
            this._break = false;
            this.shadow = false;
            this.powerprint = false;
            this.shild = true;
            this.code[0] = ChipFolder.CODE.asterisk;
            this.code[1] = ChipFolder.CODE.asterisk;
            this.code[2] = ChipFolder.CODE.asterisk;
            this.code[3] = ChipFolder.CODE.asterisk;
            var information = NSGame.ShanghaiEXE.Translate("Chip.EriaGuardDesc");
            this.information[0] = information[0];
            this.information[1] = information[1];
            this.information[2] = information[2];
            this.Init();
        }

        public override void Action(CharacterBase character, SceneBattle battle)
        {
            var anyPanelLocked = false;
            if (character.waittime <= 1)
                character.animationpoint = new Point(0, 1);
            else if (character.waittime <= 7)
                character.animationpoint = new Point((character.waittime - 1) / 2, 1);
            else if (character.waittime < 15)
                character.animationpoint = new Point(3, 1);
            else if (character.waittime == 15)
            {
                int pX = EriaGuard.SteelX(character, battle);
                if (pX == 99)
                    return;
                for (int pY = 0; pY < battle.panel.GetLength(1); ++pY)
                {
                    if (battle.panel[pX, pY].color == character.union && !battle.panel[pX, pY].inviolability)
                    {
                        this.sound.PlaySE(SoundEffect.eriasteal2);
                        character.parent.effects.Add(new AfterSteal(this.sound, character.parent, pX, pY));
                        battle.panel[pX, pY].inviolability = true;
                        anyPanelLocked = true;
                    }
                }

                if (!anyPanelLocked)
                {
                    this.sound.PlaySE(SoundEffect.heat);
                    Vector2 pd = new Vector2(character.positionDirect.X - 8f, character.positionDirect.Y - 32f);
                    battle.effects.Add(new Smoke(this.sound, battle, pd, character.position, ChipBase.ELEMENT.normal));
                    base.Action(character, battle);
                }
            }
            else
            {
                if (character.waittime < 31)
                    return;
                if (character is Player)
                    ((Player)character).PluspointWing(80);
                base.Action(character, battle);
            }
        }

        public static int SteelX(CharacterBase character, SceneBattle battle)
        {
            int num = 99;
            if (num == 99)
            {
                if (character.union == Panel.COLOR.red)
                {
                    for (int index1 = 0; index1 < battle.panel.GetLength(0); ++index1)
                    {
                        for (int index2 = 0; index2 < battle.panel.GetLength(1); ++index2)
                        {
                            if (!battle.panel[index1, index2].inviolability)
                            {
                                num = index1;
                                break;
                            }
                        }
                        if (num != 99)
                            break;
                    }
                }
                else
                {
                    for (int index1 = battle.panel.GetLength(0) - 1; index1 >= 0; --index1)
                    {
                        for (int index2 = battle.panel.GetLength(1) - 1; index2 >= 0; --index2)
                        {
                            if (!battle.panel[index1, index2].inviolability)
                            {
                                num = index1;
                                break;
                            }
                        }
                        if (num != 99)
                            break;
                    }
                }
            }
            return num;
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
                dg.DrawImage(dg, "chipgraphic8", this._rect, true, p, Color.White);
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
            if (!noicon)
            {
                this._rect = this.IconRect(select);
                dg.DrawImage(dg, "chipicon", this._rect, true, p, Color.White);
            }
            base.IconRender(dg, p, select, custom, c, noicon);
        }

        public override void Render(IRenderer dg, CharacterBase character)
        {
            this._rect = new Rectangle(character.animationpoint.X * character.Wide, 4 * character.Height, character.Wide, character.Height);
            this._position = new Vector2(character.positionDirect.X + Shake.X, character.positionDirect.Y + Shake.Y);
            dg.DrawImage(dg, character.picturename, this._rect, false, this._position, character.union == Panel.COLOR.blue, Color.White);
        }
    }
}

