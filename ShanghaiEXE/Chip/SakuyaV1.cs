using NSAttack;
using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEffect;
using NSEnemy;
using Common.Vectors;
using System.Collections.Generic;
using System.Drawing;

namespace NSChip
{
    internal class SakuyaV1 : ChipBase
    {
        private Point[] targets = new Point[0];
        private const int interval = 20;
        private const int speed = 2;
        protected Point animePoint;
        private int shot;
        private bool end;
        private bool init;

        public SakuyaV1(IAudioEngine s)
          : base(s)
        {
            this.navi = true;
            this.number = 194;
            this.name = NSGame.ShanghaiEXE.Translate("Chip.SakuyaV1Name");
            this.element = ChipBase.ELEMENT.normal;
            this.power = 30;
            this.subpower = 0;
            this.regsize = 31;
            this.reality = 3;
            this.swordtype = true;
            this._break = false;
            this.shadow = false;
            this.powerprint = true;
            this.code[0] = ChipFolder.CODE.S;
            this.code[1] = ChipFolder.CODE.I;
            this.code[2] = ChipFolder.CODE.asterisk;
            this.code[3] = ChipFolder.CODE.asterisk;
            var information = NSGame.ShanghaiEXE.Translate("Chip.SakuyaV1Desc");
            this.information[0] = information[0];
            this.information[1] = information[1];
            this.information[2] = information[2];
            this.Init();
        }

        private Point Animation(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[5]
            {
        4,
        12,
        4,
        4,
        400
            }, new int[5] { 0, 9, 10, 11, 12 }, 0, waittime);
        }

        public override void Action(CharacterBase character, SceneBattle battle)
        {
            if (!this.BlackOut(character, battle, this.name, this.Power(character).ToString()))
                return;
            if (!this.init)
            {
                List<Point> pointList = new List<Point>();
                foreach (CharacterBase characterBase in battle.AllChara())
                {
                    if (characterBase.union == character.UnionEnemy && (!(characterBase is DammyEnemy) || !characterBase.nohit) && character.InAreaCheck(character.position))
                        pointList.Add(characterBase.position);
                }
                this.targets = pointList.ToArray();
                character.animationpoint.X = -1;
                this.sound.PlaySE(SoundEffect.warp);
                this.init = true;
            }
            if (character.waittime == this.shot * 20 + 20 && this.shot < this.targets.Length)
            {
                this.sound.PlaySE(SoundEffect.chain);
                for (int angle = 0; angle < 4; ++angle)
                {
                    int x = this.targets[this.shot].X;
                    int y = this.targets[this.shot].Y;
                    switch (angle)
                    {
                        case 0:
                            ++y;
                            break;
                        case 1:
                            x += this.UnionRebirth(character.union);
                            break;
                        case 2:
                            --y;
                            break;
                        case 3:
                            x -= this.UnionRebirth(character.union);
                            break;
                    }
                    var knifePos = new Point(x, y);
                    if (character.NoObject(knifePos) && character.InAreaCheck(knifePos))
                    {
                        AttackBase a = new ThrowKnife(this.sound, battle, x, y, character.union, 10, this.Power(character), 6, 30, angle);
                        a.invincibility = false;
                        battle.attacks.Add(this.Paralyze(a));
                    }
                }
                ++this.shot;
            }
            if (this.shot >= this.targets.Length && character.waittime >= this.targets.Length * 20 + 80)
            {
                if (!this.end)
                {
                    this.animePoint.X = -1;
                    battle.effects.Add(new MoveEnemy(this.sound, battle, character.position.X, character.position.Y));
                    this.end = true;
                }
                if (this.BlackOutEnd(character, battle))
                    base.Action(character, battle);
            }
            else
                this.animePoint = this.Animation(character.waittime);
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
                dg.DrawImage(dg, "chipgraphic12", this._rect, true, p, Color.White);
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
            if (character.animationpoint.X == -1)
            {
                this._rect = new Rectangle(48 * this.animePoint.X, 0, 48, 64);
                this._position = new Vector2((float)(character.position.X * 40.0 + 18.0), (float)(character.position.Y * 24.0 + 58.0));
                dg.DrawImage(dg, "sakuya", this._rect, false, this._position, character.union == Panel.COLOR.red, Color.White);
            }
            else
                this.BlackOutRender(dg, character.union);
        }
    }
}

