using NSAttack;
using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSChip
{
    internal class TurtleHockey : ChipBase
    {
        private const int start = 1;
        private const int speed = 2;

        public TurtleHockey(IAudioEngine s)
          : base(s)
        {
            this.rockOnPoint = new Point(-1, 0);
            this.number = 380;
            this.name = NSGame.ShanghaiEXE.Translate("Chip.TurtleHockeyName");
            this.element = ChipBase.ELEMENT.normal;
            this.power = 40;
            this.subpower = 0;
            this._break = true;
            this.powerprint = true;
            this.regsize = 30;
            this.reality = 3;
            this.code[0] = ChipFolder.CODE.T;
            this.code[1] = ChipFolder.CODE.R;
            this.code[2] = ChipFolder.CODE.E;
            this.code[3] = ChipFolder.CODE.K;
            var information = NSGame.ShanghaiEXE.Translate("Chip.TurtleHockeyDesc");
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
            else if (character.waittime <= 15)
                character.animationpoint = new Point(3, 1);
            else
                base.Action(character, battle);
            if (character.waittime != 5)
                return;
            int num = this.power + this.pluspower;
            this.sound.PlaySE(SoundEffect.knife);
            int spd = 1;
            character.parent.attacks.Add(this.Paralyze(new ShellHockeyA(this.sound, character.parent, character.position.X + this.UnionRebirth(character.union), character.position.Y, character.union, this.Power(character), spd, this.element)));
        }

        public override void Init()
        {
            base.Init();
            this.sortNumber = 189.5f;
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
                this._rect = new Rectangle(1008, 0, 56, 48);
                dg.DrawImage(dg, "chipgraphic23", this._rect, true, p, Color.White);
            }
            base.GraphicsRender(dg, p, c, false, printstatus);
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
                this._rect = new Rectangle(640, 80 + num * 96, 16, 16);
                dg.DrawImage(dg, "chipicone", this._rect, true, p, Color.White);
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


