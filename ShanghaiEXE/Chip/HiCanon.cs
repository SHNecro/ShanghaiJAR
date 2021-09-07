using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;
using Common.Vectors;
using System.Drawing;

namespace NSChip
{
    internal class HiCanon : ChipBase
    {
        private const int shotend = 28;

        public HiCanon(IAudioEngine s)
          : base(s)
        {
            this.rockOnPoint = new Point(-1, 0);
            this.number = 271;
            this.name = NSGame.ShanghaiEXE.Translate("Chip.HiCanonName");
            this.element = ChipBase.ELEMENT.normal;
            this.power = 250;
            this.regsize = 12;
            this.reality = 1;
            this.subpower = 0;
            this._break = false;
            this.powerprint = true;
            this.code[0] = ChipFolder.CODE.none;
            this.code[1] = ChipFolder.CODE.none;
            this.code[2] = ChipFolder.CODE.none;
            this.code[3] = ChipFolder.CODE.none;
            var information = NSGame.ShanghaiEXE.Translate("Chip.HiCanonDesc");
            this.information[0] = information[0];
            this.information[1] = information[1];
            this.information[2] = information[2];
            this.Init();
        }

        private Point Animation(int waittime)
        {
            return CharacterAnimation.Return(new int[4]
            {
        5,
        15,
        17,
        20
            }, new int[7] { 4, 5, 6, 5, 2, 1, 0 }, 0, waittime);
        }

        public override void Action(CharacterBase character, SceneBattle battle)
        {
            if (character.waittime < 5)
                character.animationpoint = new Point(4, 0);
            else if (character.waittime < 15)
                character.animationpoint = new Point(5, 0);
            else if (character.waittime < 28)
            {
                character.animationpoint = new Point(6, 0);
                if (character.waittime < 17)
                    character.positionDirect.X -= (character.waittime - 15) * this.UnionRebirth(character.union);
            }
            else if (character.waittime < 33)
            {
                character.animationpoint = new Point(5, 0);
                character.PositionDirectSet();
            }
            else if (character.waittime == 33)
                base.Action(character, battle);
            if (character.waittime == 18)
                this.sound.PlaySE(SoundEffect.canon);
            if (character.waittime != 20)
                return;
            int num = this.power + this.pluspower;
            character.parent.attacks.Add(new NSAttack.HiMegaCanon(this.sound, character.parent, character.position.X + this.UnionRebirth(character.union), character.position.Y, character.union, this.Power(character), 3, true));
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
            switch (c % 2)
            {
                case 0:
                    this._rect = new Rectangle(848, 320, 74, 79);
                    dg.DrawImage(dg, "menuwindows", this._rect, true, p - new Vector2(9, 16), Color.White);
                    this._rect = new Rectangle(56 * 0, 48 * 0, 56, 48);
                    dg.DrawImage(dg, "pagraphic1", this._rect, true, p, Color.White);
                    return;
                case 1:
                    string[] strArray =
                    {
                        ShanghaiEXE.Translate("Chip.ProgramAdvanceHiCanonCombo1Line1"),
                        ShanghaiEXE.Translate("Chip.ProgramAdvanceHiCanonCombo1Line2"),
                        ShanghaiEXE.Translate("Chip.ProgramAdvanceHiCanonCombo1Line3")
                    };
                    for (int index = 0; index < strArray.Length; ++index)
                    {
                        this._position = new Vector2(p.X - 12f, p.Y - 8f + index * 16);
                        this.TextRender(dg, strArray[index], false, this._position, false, Color.LightBlue);
                    }
                    return;
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
            this._rect = new Rectangle(7 * character.Wide, 5 * character.Height, character.Wide, character.Height);
            this._position = new Vector2(character.positionDirect.X + Shake.X, character.positionDirect.Y + Shake.Y);
            if (character.waittime < 10)
                this._rect.X -= character.Wide;
            else if (character.waittime >= 15 && character.waittime < 28)
                this._position.X -= 2 * this.UnionRebirth(character.union);
            dg.DrawImage(dg, "weapons", this._rect, false, this._position, character.union == Panel.COLOR.blue, Color.White);
        }
    }
}


