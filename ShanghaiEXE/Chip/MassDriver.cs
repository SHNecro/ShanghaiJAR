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
    internal class MassDriver : ChipBase
    {
        private bool open;
        private const int shotend = 10;
        private int roopcount;
        private const int roop = 5;

        public MassDriver(IAudioEngine s)
          : base(s)
        {
            this.rockOnPoint = new Point(-2, 0);
            this.number = 280;
            this.name = NSGame.ShanghaiEXE.Translate("Chip.MassDriverName");
            this.element = ChipBase.ELEMENT.normal;
            this.power = 100;
            this.subpower = 0;
            this._break = false;
            this.powerprint = true;
            this.code[0] = ChipFolder.CODE.none;
            this.code[1] = ChipFolder.CODE.none;
            this.code[2] = ChipFolder.CODE.none;
            this.code[3] = ChipFolder.CODE.none;
            var information = NSGame.ShanghaiEXE.Translate("Chip.MassDriverDesc");
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
            else if (character.waittime < 10)
                character.animationpoint = new Point(6, 0);
            else if (character.waittime == 5)
                character.animationpoint = new Point(5, 0);
            else if (character.waittime == 24)
            {
                ++this.roopcount;
                if (this.roopcount >= 5)
                    base.Action(character, battle);
                else
                    character.waittime = 3;
            }
            if (character.waittime != 6)
                return;
            this.sound.PlaySE(SoundEffect.canon);
            Point point = new Point(character.position.X + 3 * this.UnionRebirth(character.union), character.position.Y);
            Vector2 vector2 = new Vector2(character.positionDirect.X + 30 * this.UnionRebirth(character.union), character.positionDirect.Y - 3f);
            character.parent.attacks.Add(this.Paralyze(new ObjectShoot(this.sound, character.parent, character.position.X, character.position.Y, new Vector2(character.positionDirect.X, character.positionDirect.Y + 8f), character.union, this.Power(character), ChipBase.ELEMENT.normal)));
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
                    this._rect = new Rectangle(56 * 1, 48 * 1, 56, 48);
                    dg.DrawImage(dg, "pagraphic1", this._rect, true, p, Color.White);
                    return;
                case 1:
                    string[] strArray =
                    {
                        ShanghaiEXE.Translate("Chip.ProgramAdvanceMassDriverCombo1Line1"),
                        ShanghaiEXE.Translate("Chip.ProgramAdvanceMassDriverCombo1Line2"),
                        ShanghaiEXE.Translate("Chip.ProgramAdvanceMassDriverCombo1Line3")
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
            int x = 10 * character.Wide;
            int height1 = character.Height;
            int wide = character.Wide;
            int height2 = character.Height;
            this._rect = new Rectangle(x, 0, wide, height2);
            this._position = new Vector2(character.positionDirect.X + Shake.X, character.positionDirect.Y + Shake.Y);
            if (character.waittime > 5 && character.waittime < 8)
                this._rect.X += 120;
            else if (character.waittime >= 15 && character.waittime < 10)
                this._position.X -= 2 * this.UnionRebirth(character.union);
            dg.DrawImage(dg, "weapons", this._rect, false, this._position, character.union == Panel.COLOR.blue, Color.White);
        }
    }
}


