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
    internal class SwordOffGun : ChipBase
    {
        private const int shotend = 16;

        public SwordOffGun(IAudioEngine s)
          : base(s)
        {
            this.rockOnPoint = new Point(-2, 0);
            this.number = 283;
            this.name = NSGame.ShanghaiEXE.Translate("Chip.SwordOffGunName");
            this.element = ChipBase.ELEMENT.normal;
            this.power = 80;
            this.subpower = 0;
            this._break = false;
            this.powerprint = true;
            this.code[0] = ChipFolder.CODE.none;
            this.code[1] = ChipFolder.CODE.none;
            this.code[2] = ChipFolder.CODE.none;
            this.code[3] = ChipFolder.CODE.none;
            var information = NSGame.ShanghaiEXE.Translate("Chip.SwordOffGunDesc");
            this.information[0] = information[0];
            this.information[1] = information[1];
            this.information[2] = information[2];
            this.Init();
        }

        public override void Action(CharacterBase character, SceneBattle battle)
        {
            if (character.waittime < 1)
                character.animationpoint = new Point(4, 0);
            else if (character.waittime < 3)
                character.animationpoint = new Point(5, 0);
            else if (character.waittime < 16)
                character.animationpoint = new Point(6, 0);
            else if (character.waittime == 5)
                character.animationpoint = new Point(5, 0);
            else if (character.waittime == 21)
                base.Action(character, battle);
            if (character.waittime != 6)
                return;
            int num = this.power + this.pluspower;
            battle.effects.Add(new BulletBigShells(this.sound, battle, character.position, character.positionDirect.X + 4 * character.UnionRebirth, character.positionDirect.Y, 26, character.union, 20 + this.Random.Next(20), 2, 0));
            character.parent.attacks.Add(this.Paralyze(new ShotGun(this.sound, character.parent, character.position.X + this.UnionRebirth(character.union), character.position.Y, character.union, this.Power(character), this.element)
            {
                hit = 7
            }));
            character.parent.attacks.Add(this.Paralyze(new ShotGun(this.sound, character.parent, character.position.X + 2 * this.UnionRebirth(character.union), character.position.Y - 1, character.union, this.Power(character), this.element)
            {
                hit = 7
            }));
            character.parent.attacks.Add(this.Paralyze(new ShotGun(this.sound, character.parent, character.position.X + 2 * this.UnionRebirth(character.union), character.position.Y, character.union, this.Power(character), this.element)
            {
                hit = 7
            }));
            character.parent.attacks.Add(this.Paralyze(new ShotGun(this.sound, character.parent, character.position.X + 2 * this.UnionRebirth(character.union), character.position.Y + 1, character.union, this.Power(character), this.element)
            {
                hit = 7
            }));
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
            switch (c % 3)
            {
                case 0:
                    this._rect = new Rectangle(848, 320, 74, 79);
                    dg.DrawImage(dg, "menuwindows", this._rect, true, p - new Vector2(9, 16), Color.White);
                    this._rect = new Rectangle(56 * 4, 48 * 1, 56, 48);
                    dg.DrawImage(dg, "pagraphic1", this._rect, true, p, Color.White);
                    return;
                case 1:
                    strArray = new string[]
                    {
                        ShanghaiEXE.Translate("Chip.ProgramAdvanceSwordOffGunCombo1Line1"),
                        ShanghaiEXE.Translate("Chip.ProgramAdvanceSwordOffGunCombo1Line2"),
                        ShanghaiEXE.Translate("Chip.ProgramAdvanceSwordOffGunCombo1Line3")
                    };
                    break;
                case 2:
                    strArray = new string[]
                    {
                        ShanghaiEXE.Translate("Chip.ProgramAdvanceSwordOffGunCombo2Line1"),
                        ShanghaiEXE.Translate("Chip.ProgramAdvanceSwordOffGunCombo2Line2"),
                        ShanghaiEXE.Translate("Chip.ProgramAdvanceSwordOffGunCombo2Line3")
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
            if (character.waittime < 5)
                return;
            this._rect = new Rectangle(character.Wide, 6 * character.Height, character.Wide, character.Height);
            this._position = new Vector2(character.positionDirect.X + Shake.X, character.positionDirect.Y + Shake.Y);
            if (character.waittime < 6)
                this._rect.X = 0;
            else if (character.waittime >= 8 && character.waittime < 16)
                this._position.X -= 2 * this.UnionRebirth(character.union);
            dg.DrawImage(dg, "weapons", this._rect, false, this._position, character.union == Panel.COLOR.blue, Color.White);
        }
    }
}


