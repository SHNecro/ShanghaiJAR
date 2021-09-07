using NSAttack;
using NSBattle;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSEnemy
{
    internal class HalfSoul : NaviBase
    {
        public HalfSoul.MOTION motion = HalfSoul.MOTION.neutral;
        public bool nomove = false;
        public Youmu MainEnemy;
        private const int HP = 100000;
        public Point slidePosition;
        private readonly int moveroop;
        private int roopneutral;
        private int roopmove;
        public bool attack;

        public HalfSoul(IAudioEngine s, SceneBattle p, int pX, int pY, Youmu MainEnemy, bool effect)
          : base(s, p, MainEnemy.position.X, MainEnemy.position.Y, (byte)MainEnemy.number, MainEnemy.union, MainEnemy.version)
        {
            this.position = new Point(pX, pY);
            this.MainEnemy = MainEnemy;
            this.effecting = effect;
            this.hpmax = 100000;
            this.hp = 100000;
            this.HPposition.X = -100f;
            this.PositionDirectSet();
            this.noslip = true;
            this.dropchips = MainEnemy.dropchips;
            this.race = EnemyBase.ENEMY.navi;
            this.superArmor = true;
            this.ID = MainEnemy.ID;
            this.wide = 104;
            this.height = 96;
            this.picturename = "youmu";
            this.animationpoint = new Point(0, 7);
        }

        public override void Dameged(AttackBase attack)
        {
            if (attack is Dummy)
            {
                return;
            }

            this.MainEnemy.Hp -= 100000 - this.hp;
            this.hp = 100000;
            this.MainEnemy.whitetime = this.whitetime;
            base.Dameged(attack);
        }

        public override void Updata()
        {
            this.Moving();
            if (this.hp < 100000)
            {
                this.MainEnemy.Hp -= 100000 - this.hp;
                this.hp = 100000;
            }
            this.MainEnemy.badstatus = this.badstatus;
            this.MainEnemy.badstatustime = this.badstatustime;
            this.flag = this.MainEnemy.flag;
        }

        public override void PositionDirectSet()
        {
            this.positionDirect = new Vector2((float)(position.X * 40.0 + 4.0), (float)(position.Y * 24.0 + 48.0));
        }

        protected override void Moving()
        {
            switch (this.motion)
            {
                case HalfSoul.MOTION.neutral:
                    if (this.moveflame)
                        ++this.waittime;
                    if (this.moveflame || this.attack)
                    {
                        this.animationpoint = this.AnimeNeutral(this.waittime);
                        if (this.waittime >= 4 || this.attack)
                        {
                            this.waittime = 0;
                            ++this.roopneutral;
                            if (this.roopneutral >= 8 && this.parent.nowscene != SceneBattle.BATTLESCENE.end || this.attack)
                            {
                                this.roopneutral = 0;
                                this.waittime = 0;
                                this.motion = HalfSoul.MOTION.move;
                            }
                        }
                        break;
                    }
                    break;
                case HalfSoul.MOTION.move:
                    this.animationpoint = this.AnimeMove(this.waittime);
                    if (this.moveflame)
                    {
                        switch (this.waittime)
                        {
                            case 0:
                                if (this.attack)
                                    this.MoveRandom(true, true);
                                else
                                    this.MoveRandom(false, false);
                                if (this.position == this.positionre)
                                {
                                    this.motion = !this.attack ? HalfSoul.MOTION.neutral : HalfSoul.MOTION.attack;
                                    this.waittime = 0;
                                    this.frame = 0;
                                    this.roopneutral = 0;
                                    ++this.roopmove;
                                    break;
                                }
                                break;
                            case 3:
                                this.position = this.positionre;
                                this.PositionDirectSet();
                                break;
                            case 5:
                                this.motion = !this.attack ? HalfSoul.MOTION.neutral : HalfSoul.MOTION.attack;
                                this.waittime = 0;
                                this.frame = 0;
                                this.roopneutral = 0;
                                ++this.roopmove;
                                break;
                        }
                        ++this.waittime;
                        break;
                    }
                    break;
                case HalfSoul.MOTION.attack:
                    this.animationpoint = this.AnimeAttack(this.waittime);
                    if (this.moveflame)
                    {
                        switch (this.waittime)
                        {
                            case 5:
                                this.sound.PlaySE(SoundEffect.sword);
                                AttackBase attackBase1 = new SwordAttack(this.sound, this.parent, this.position.X + this.UnionRebirth(this.union), this.position.Y, this.union, 100, 2, this.element, false, false);
                                attackBase1.invincibility = false;
                                this.parent.attacks.Add(attackBase1);
                                break;
                            case 9:
                                this.sound.PlaySE(SoundEffect.sword);
                                AttackBase attackBase2 = new LanceAttack(this.sound, this.parent, this.position.X + this.UnionRebirth(this.union), this.position.Y, this.union, 100, 2, this.element, true);
                                attackBase2.invincibility = false;
                                this.parent.attacks.Add(attackBase2);
                                break;
                            case 20:
                                this.motion = HalfSoul.MOTION.move;
                                this.waittime = 0;
                                this.attack = false;
                                break;
                        }
                        ++this.waittime;
                        break;
                    }
                    break;
                case HalfSoul.MOTION.lost:
                    if (this.waittime < 4)
                    {
                        this.animationpoint = this.AnimeLost(this.waittime);
                        if (this.waittime == 2)
                            this.nohit = true;
                        ++this.waittime;
                        break;
                    }
                    break;
                case HalfSoul.MOTION.unlost:
                    this.animationpoint = this.AnimeUnLost(this.waittime);
                    if (this.waittime == 3)
                    {
                        this.nohit = false;
                        this.waittime = 0;
                        this.motion = HalfSoul.MOTION.neutral;
                    }
                    ++this.waittime;
                    break;
            }
            this.FlameControl(4);
        }

        public void LostFlag(bool lost)
        {
            this.motion = !lost ? HalfSoul.MOTION.unlost : HalfSoul.MOTION.lost;
            this.waittime = 0;
            this.frame = 0;
            this.roopneutral = 0;
        }

        public override void Render(IRenderer dg)
        {
            this.color = Color.FromArgb(byte.MaxValue, Color.White);
            this._position = new Vector2((int)this.positionDirect.X + this.Shake.X, (int)this.positionDirect.Y + this.Shake.Y);
            this._rect = new Rectangle(this.animationpoint.X * this.wide, 864 * (this.MainEnemy.version < 4 ? 0 : 2) + this.animationpoint.Y * this.height, this.wide, this.height);
            if (this.whitetime == 0)
            {
                dg.DrawImage(dg, this.picturename, this._rect, false, this._position, this.rebirth, this.color);
            }
            else
            {
                this._rect.Y = 864 + this.animationpoint.Y * this.height;
                dg.DrawImage(dg, this.picturename, this._rect, false, this._position, this.rebirth, this.color);
            }
        }

        private Point AnimeNeutral(int waitflame)
        {
            return this.Return(new int[4] { 0, 1, 2, 3 }, new int[4]
            {
        0,
        1,
        2,
        3
            }, 7, waitflame);
        }

        private Point AnimeAttack(int waitflame)
        {
            return this.Return(new int[13]
            {
        0,
        1,
        2,
        3,
        4,
        5,
        6,
        7,
        8,
        9,
        10,
        11,
        100
            }, new int[13]
            {
        0,
        1,
        2,
        3,
        4,
        5,
        6,
        7,
        8,
        9,
        10,
        11,
        12
            }, 8, waitflame);
        }

        private Point AnimeMove(int waitflame)
        {
            return this.Return(new int[5] { 0, 1, 2, 4, 5 }, new int[5]
            {
        0,
        4,
        5,
        4,
        0
            }, 7, waitflame);
        }

        private Point AnimeLost(int waitflame)
        {
            return this.Return(new int[4] { 0, 1, 2, 3 }, new int[4]
            {
        0,
        4,
        5,
        8
            }, 7, waitflame);
        }

        private Point AnimeUnLost(int waitflame)
        {
            return this.Return(new int[4] { 0, 1, 2, 3 }, new int[4]
            {
        8,
        5,
        4,
        0
            }, 7, waitflame);
        }

        public enum MOTION
        {
            neutral,
            move,
            attack,
            lost,
            unlost,
        }
    }
}

