using NSAttack;
using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSChip
{
    internal class UthuhoV1 : ChipBase
    {
        private const int start = 44;
        private const int speed = 2;
        protected int color;
        private Point animePoint;

        public UthuhoV1(IAudioEngine s)
          : base(s)
        {
            this.navi = true;
            this.number = 252;
            this.name = NSGame.ShanghaiEXE.Translate("Chip.UthuhoV1Name");
            this.element = ChipBase.ELEMENT.heat;
            this.power = 150;
            this.subpower = 0;
            this.regsize = 49;
            this.reality = 3;
            this._break = true;
            this.shadow = false;
            this.powerprint = true;
            this.color = 0;
            this.code[0] = ChipFolder.CODE.U;
            this.code[1] = ChipFolder.CODE.R;
            this.code[2] = ChipFolder.CODE.asterisk;
            this.code[3] = ChipFolder.CODE.asterisk;
            var information = NSGame.ShanghaiEXE.Translate("Chip.UthuhoV1Desc");
            this.information[0] = information[0];
            this.information[1] = information[1];
            this.information[2] = information[2];
            this.Init();
        }

        protected Point Animation(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[10]
            {
        1,
        4,
        4,
        4,
        4,
        12,
        4,
        4,
        4,
        4
            }, new int[10] { -1, 0, 1, 2, 3, 0, 3, 7, 13, 14 }, 3, waittime);
        }

        public override void Action(CharacterBase character, SceneBattle battle)
        {
            if (!this.BlackOut(character, battle, this.name, this.Power(character).ToString()))
                return;
            switch (character.waittime)
            {
                case 1:
                    this.animePoint.X = 0;
                    this.animePoint.Y = 0;
                    character.animationpoint.X = -1;
                    this.sound.PlaySE(SoundEffect.warp);
                    break;
                case 10:
                    this.sound.PlaySE(SoundEffect.futon);
                    this.animePoint.X = 0;
                    this.animePoint.Y = 3;
                    break;
                case 15:
                    this.animePoint.X = 1;
                    break;
                case 20:
                    this.animePoint.X = 2;
                    break;
                case 44:
                    this.sound.PlaySE(SoundEffect.shoot);
                    character.parent.attacks.Add(this.Paralyze(new UthuhoChip(this.sound, character.parent, character.position.X, character.position.Y, character.union, this.Power(character), this.color)));
                    AttackBase attackBase1 = this.Paralyze(new UthuhoChip(this.sound, character.parent, character.position.X, character.position.Y - 1, character.union, this.Power(character), this.color));
                    attackBase1.breaking = false;
                    character.parent.attacks.Add(attackBase1);
                    AttackBase attackBase2 = this.Paralyze(new UthuhoChip(this.sound, character.parent, character.position.X, character.position.Y + 1, character.union, this.Power(character), this.color));
                    attackBase2.breaking = false;
                    character.parent.attacks.Add(attackBase2);
                    break;
            }
            if (character.waittime > 100 && this.BlackOutEnd(character, battle))
                base.Action(character, battle);
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
                this._rect = new Rectangle(336, 0, 56, 48);
                dg.DrawImage(dg, "chipgraphic22", this._rect, true, p, Color.White);
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
                this._rect = new Rectangle(480, 64 + num4 * 96, 16, 16);
                dg.DrawImage(dg, "chipicon", this._rect, true, p, Color.White);
            }
            base.IconRender(dg, p, select, custom, c, noicon);
        }

        public override void Render(IRenderer dg, CharacterBase character)
        {
            if (character.animationpoint.X == -1 && character.waittime < 44)
            {
                int num = 0;
                if (this.color == 1)
                    num = 2160;
                if (this.color == 2)
                    num = 1440;
                this._rect = new Rectangle(120 * this.animePoint.X, num + 144 * this.animePoint.Y, 120, 144);
                this._position = new Vector2(character.position.X * 40f + 24 * this.UnionRebirth(character.union), (float)(character.position.Y * 24.0 + 22.0));
                dg.DrawImage(dg, "Uthuho", this._rect, false, this._position, character.union == Panel.COLOR.red, Color.White);
            }
            else
                this.BlackOutRender(dg, character.union);
        }
    }
}

