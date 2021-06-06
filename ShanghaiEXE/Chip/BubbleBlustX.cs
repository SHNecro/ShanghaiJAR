using NSAttack;
using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSChip
{
    internal class BubbleBlustX : ChipBase
    {
        private const int shotend = 10;
        private const int shotinterval = 4;

        public BubbleBlustX(IAudioEngine s)
          : base(s)
        {
            this.rockOnPoint = new Point(-3, 0);
            this.number = 353;
            this.name = NSGame.ShanghaiEXE.Translate("Chip.BubbleBlustXName");
            this.element = ChipBase.ELEMENT.aqua;
            this.power = 40;
            this.regsize = 99;
            this.reality = 5;
            this.subpower = 0;
            this._break = false;
            this.powerprint = true;
            this.code[0] = ChipFolder.CODE.B;
            this.code[1] = ChipFolder.CODE.B;
            this.code[2] = ChipFolder.CODE.B;
            this.code[3] = ChipFolder.CODE.X;
            var information = NSGame.ShanghaiEXE.Translate("Chip.BubbleBlustXDesc");
            this.information[0] = information[0];
            this.information[1] = information[1];
            this.information[2] = information[2];
            this.Init();
        }

        public override void Action(CharacterBase character, SceneBattle battle)
        {
            character.animationpoint = BubbleBlustX.Animation(character.waittime);
            if (character.waittime == 40)
                base.Action(character, battle);
            if (character.waittime % 8 == 0)
                this.sound.PlaySE(SoundEffect.vulcan);
            if (character.waittime % 8 != 4)
                return;
            int num = this.power + this.pluspower;
            var attack = this.Paralyze(new Vulcan(this.sound, character.parent, character.position.X + this.UnionRebirth(character.union), character.position.Y, character.union, this.Power(character), Vulcan.SHOT.Bubble, ChipBase.ELEMENT.aqua, false));
            attack.invincibility = character.waittime > 40 - 8;
            attack.invincibilitytimeA = 20;
            character.parent.attacks.Add(attack);
        }

        public static Point Animation(int waittime)
        {
            int[] interval = new int[12];
            for (int index = 0; index < 12; ++index)
                interval[index] = 4 * index;
            int[] xpoint = new int[14]
            {
                5,
                6,
                5,
                6,
                5,
                6,
                5,
                6,
                5,
                6,
                5,
                6,
                5,
                6
            };
            int y = 0;
            return CharacterAnimation.Return(interval, xpoint, y, waittime);
        }

        public override void Init()
        {
            base.Init();
            this.sortNumber = 55.5f;
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
                this._rect = new Rectangle(168, 0, 56, 48);
                dg.DrawImage(dg, "chipgraphic17", this._rect, true, p, Color.White);
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
                int num1 = this.number - 1;
                int num2 = num1 % 40;
                int num3 = num1 / 40;
                int num4 = 0;
                if (select)
                    num4 = 1;
                this._rect = new Rectangle(96, 80 + num4 * 96, 16, 16);
                dg.DrawImage(dg, "chipicon", this._rect, true, p, Color.White);
            }
            base.IconRender(dg, p, select, custom, c, noicon);
        }

        public override void Render(IRenderer dg, CharacterBase character)
        {
            this._rect = new Rectangle(4 * character.Wide, character.Height, character.Wide, character.Height);
            this._position = new Vector2(character.positionDirect.X + Shake.X, character.positionDirect.Y + Shake.Y);
            if (character.animationpoint.X == 6)
            {
                this._rect.X += 120;
                this._position.X -= 2 * this.UnionRebirth(character.union);
            }
            dg.DrawImage(dg, "weapons", this._rect, false, this._position, character.union == Panel.COLOR.blue, Color.White);
        }
    }
}

