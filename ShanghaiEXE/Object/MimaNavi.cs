using NSAttack;
using NSBattle;
using NSBattle.Character;
using NSChip;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSObject
{
    internal class MimaNavi : ObjectBase
    {
        private MimaNavi.MOTION motion;
        private int attackCount;
        private bool breaked;

        public MimaNavi(IAudioEngine s, SceneBattle p, int pX, int pY, Panel.COLOR union)
          : base(s, p, pX, pY, union)
        {
            this.height = 48;
            this.wide = 32;
            this.hp = 100;
            this.hitPower = 100;
            this.speed = 4;
            this.hpmax = this.hp;
            this.unionhit = false;
            this.noslip = true;
            this.overslip = false;
            this.positionre = this.position;
            this.positionDirect = new Vector2(pX * 40 + 16, pY * 24 + 56);
        }

        public override void PositionDirectSet()
        {
            this.positionDirect = new Vector2(this.position.X * 40 + 16, this.position.Y * 24 + 56);
        }

        public override void Updata()
        {
            if (this.moveflame)
            {
                if (this.breaked)
                {
                    if (this.animationpoint.X > 8)
                        this.animationpoint.X = 8;
                    --this.animationpoint.X;
                    if (this.animationpoint.X < 0)
                        this.flag = false;
                }
                else
                {
                    switch (this.motion)
                    {
                        case MimaNavi.MOTION.init:
                            ++this.animationpoint.X;
                            if (this.animationpoint.X >= 8)
                            {
                                this.frame = -8;
                                this.motion = MimaNavi.MOTION.neutral;
                                break;
                            }
                            break;
                        case MimaNavi.MOTION.neutral:
                            if (this.frame >= 0)
                            {
                                this.motion = !this.EnemySearch(this.position.Y) ? MimaNavi.MOTION.move : MimaNavi.MOTION.attack;
                                break;
                            }
                            break;
                        case MimaNavi.MOTION.move:
                            this.animationpoint = this.AnimeMove(this.frame);
                            if (this.frame == 5)
                            {
                                bool flag = false;
                                if (this.EnemySearch(this.position.Y - 2) && !flag)
                                {
                                    this.positionre = new Point(this.position.X, this.position.Y - 1);
                                    if (this.Canmove(this.positionre, this.number, this.union))
                                    {
                                        flag = true;
                                        this.position = this.positionre;
                                        this.PositionDirectSet();
                                    }
                                    else
                                        this.positionre = this.position;
                                }
                                if (this.EnemySearch(this.position.Y - 1) && !flag)
                                {
                                    this.positionre = new Point(this.position.X, this.position.Y - 1);
                                    if (this.Canmove(this.positionre, this.number, this.union))
                                    {
                                        flag = true;
                                        this.position = this.positionre;
                                        this.PositionDirectSet();
                                    }
                                    else
                                        this.positionre = this.position;
                                }
                                if (this.EnemySearch(this.position.Y + 1) && !flag)
                                {
                                    this.positionre = new Point(this.position.X, this.position.Y + 1);
                                    if (this.Canmove(this.positionre, this.number, this.union))
                                    {
                                        flag = true;
                                        this.position = this.positionre;
                                        this.PositionDirectSet();
                                    }
                                    else
                                        this.positionre = this.position;
                                }
                                if (this.EnemySearch(this.position.Y + 2) && !flag)
                                {
                                    this.positionre = new Point(this.position.X, this.position.Y + 1);
                                    if (this.Canmove(this.positionre, this.number, this.union))
                                    {
                                        this.position = this.positionre;
                                        this.PositionDirectSet();
                                    }
                                    else
                                        this.positionre = this.position;
                                }
                            }
                            if (this.frame >= 8)
                            {
                                this.frame = -4;
                                this.motion = MimaNavi.MOTION.neutral;
                                break;
                            }
                            break;
                        case MimaNavi.MOTION.attack:
                            this.animationpoint = this.AnimeAttack(this.frame);
                            if (this.frame == 3)
                            {
                                this.sound.PlaySE(SoundEffect.shotwave);
                                this.parent.attacks.Add(new LanceAttack(this.sound, this.parent, this.position.X + this.UnionRebirth, this.position.Y, this.union, this.hitPower, 2, ChipBase.ELEMENT.poison, true));
                            }
                            if (this.frame == 8)
                            {
                                ++this.attackCount;
                                if (this.attackCount >= 3)
                                {
                                    this.animationpoint.X = 8;
                                    int pX = Eriabash.SteelX(this, this.parent);
                                    if (pX != 99 && pX > 0 && pX < 5)
                                    {
                                        for (int pY = 0; pY < this.parent.panel.GetLength(1); ++pY)
                                            this.parent.attacks.Add(new EriaSteel(this.sound, this.parent, pX, pY, this.union, 10, this.element));
                                    }
                                    this.breaked = true;
                                }
                                else
                                {
                                    this.frame = -8;
                                    this.motion = MimaNavi.MOTION.neutral;
                                }
                                break;
                            }
                            break;
                    }
                }
            }
            this.FlameControl();
            base.Updata();
        }

        public override void Break()
        {
            if (this.breaked && !this.StandPanel.Hole)
                return;
            this.breaked = true;
            this.sound.PlaySE(SoundEffect.clincher);
        }

        private bool EnemySearch(int Y)
        {
            foreach (CharacterBase characterBase in this.parent.AllChara())
            {
                if (characterBase.union == this.UnionEnemy && characterBase.position.Y == Y)
                    return true;
            }
            return false;
        }

        public override void Render(IRenderer dg)
        {
            if (this.whitetime <= 0)
                this._rect = new Rectangle(this.animationpoint.X * 72, 416 + this.animationpoint.Y * 72, 72, 72);
            else
                this._rect = new Rectangle(this.animationpoint.X * 72, 488 + this.animationpoint.Y * 72, 72, 72);
            double x1 = positionDirect.X;
            Point shake = this.Shake;
            double x2 = shake.X;
            double num1 = x1 + x2;
            double y1 = positionDirect.Y;
            shake = this.Shake;
            double y2 = shake.Y;
            double num2 = y1 + y2;
            this._position = new Vector2((float)num1, (float)num2);
            dg.DrawImage(dg, "mimaAttack", this._rect, false, this._position, this.rebirth, Color.White);
        }

        private Point AnimeMove(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[8]
            {
        1,
        1,
        1,
        1,
        1,
        1,
        1,
        100
            }, new int[8] { 8, 15, 16, 17, 17, 16, 15, 8 }, 0, waittime);
        }

        private Point AnimeAttack(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[7]
            {
        1,
        1,
        1,
        1,
        1,
        1,
        100
            }, new int[7] { 8, 9, 10, 11, 12, 13, 14 }, 0, waittime);
        }

        private enum MOTION
        {
            init,
            neutral,
            move,
            attack,
        }
    }
}
