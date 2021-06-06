using NSAttack;
using NSBattle;
using NSBattle.Character;
using NSChip;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEffect;
using NSGame;
using Common.Vectors;
using System;
using System.Drawing;

namespace NSEnemy
{
    internal class FlowerTank : EnemyBase
    {
        private FlowerTank.MOTION motion = FlowerTank.MOTION.left;
        private FlowerTank.MOTIONUP motionup = FlowerTank.MOTIONUP.neutral;
        private bool moveend = true;
        private readonly int moving;
        public Point animationpointUP;
        private readonly int nspeed;
        private bool shot;
        private readonly bool attackanimation;
        private readonly int roopneutral;
        private readonly int roopmove;

        public FlowerTank(IAudioEngine s, SceneBattle p, int pX, int pY, byte n, Panel.COLOR u, byte v)
          : base(s, p, pX, pY, n, u, v)
        {
            this.helpPosition.X = 8;
            this.helpPosition.Y = -8;
            for (int index = 0; index < this.dropchips.Length; ++index)
                this.dropchips[index] = new ChipFolder(this.sound);
            this.picturename = "flowertank";
            this.race = EnemyBase.ENEMY.virus;
            this.element = ChipBase.ELEMENT.leaf;
            this.wide = 56;
            this.height = 32;
            this.printhp = true;
            this.printNumber = true;
            this.Noslip = true;
            this.name = ShanghaiEXE.Translate("Enemy.FlowerTankName1");
            switch (this.version)
            {
                case 0:
                    this.power = 150;
                    this.hp = 1200;
                    this.nspeed = 1;
                    this.name = ShanghaiEXE.Translate("Enemy.FlowerTankName2");
                    break;
                case 1:
                    this.power = 50;
                    this.hp = 120;
                    this.nspeed = 3;
                    break;
                case 2:
                    this.power = 70;
                    this.hp = 200;
                    this.nspeed = 2;
                    break;
                case 3:
                    this.power = 90;
                    this.hp = 260;
                    this.nspeed = 1;
                    break;
                case 4:
                    this.power = 110;
                    this.hp = 300;
                    this.name = ShanghaiEXE.Translate("Enemy.FlowerTankName3");
                    this.printNumber = false;
                    this.nspeed = 1;
                    break;
                default:
                    this.power = 120 + (version - 4) * 20;
                    this.hp = 300 + (version - 4) * 50;
                    int num = version - 3;
                    this.name = ShanghaiEXE.Translate("Enemy.FlowerTankName4") + num.ToString();
                    this.printNumber = false;
                    this.nspeed = 1;
                    {
                        num = version - 3;
                        break;
                    }
            }
            this.speed = this.nspeed;
            this.hpmax = this.hp;
            this.hpprint = this.hp;
            this.effecting = false;
            this.Flying = false;
            this.roopmove = n * 3;
            this.PositionDirectSet();
            switch (this.version)
            {
                case 1:
                    this.dropchips[0].chip = new SeedCanon1(this.sound);
                    this.dropchips[0].codeNo = 1;
                    this.dropchips[1].chip = new SeedCanon1(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new SeedCanon1(this.sound);
                    this.dropchips[2].codeNo = 2;
                    this.dropchips[3].chip = new SeedCanon1(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new SeedCanon1(this.sound);
                    if (this.Random.Next(4) < 3)
                        this.dropchips[4].codeNo = 0;
                    else
                        this.dropchips[4].codeNo = 3;
                    this.havezenny = 300;
                    break;
                case 2:
                    this.dropchips[0].chip = new SeedCanon2(this.sound);
                    this.dropchips[0].codeNo = 3;
                    this.dropchips[1].chip = new SeedCanon2(this.sound);
                    this.dropchips[1].codeNo = 3;
                    this.dropchips[2].chip = new SeedCanon2(this.sound);
                    this.dropchips[2].codeNo = 2;
                    this.dropchips[3].chip = new SeedCanon2(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new SeedCanon2(this.sound);
                    this.dropchips[4].codeNo = 1;
                    this.havezenny = 600;
                    break;
                case 3:
                    this.dropchips[0].chip = new SeedCanon3(this.sound);
                    this.dropchips[0].codeNo = 1;
                    this.dropchips[1].chip = new SeedCanon3(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new SeedCanon3(this.sound);
                    this.dropchips[2].codeNo = 2;
                    this.dropchips[3].chip = new SeedCanon3(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new SeedCanon3(this.sound);
                    this.dropchips[4].codeNo = 3;
                    this.havezenny = 800;
                    break;
                case 8:
                    this.dropchips[0].chip = new SeedCanon1(this.sound);
                    this.dropchips[0].codeNo = 2;
                    this.dropchips[1].chip = new SeedCanon2(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new SeedCanon2(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new SeedCanon3(this.sound);
                    this.dropchips[3].codeNo = 2;
                    this.dropchips[4].chip = new SeedCanonX(this.sound);
                    this.dropchips[4].codeNo = this.Random.Next(4);
                    this.havezenny = 8000;
                    break;
                default:
                    this.dropchips[0].chip = new SeedCanon1(this.sound);
                    this.dropchips[0].codeNo = 2;
                    this.dropchips[1].chip = new SeedCanon2(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new SeedCanon2(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new SeedCanon3(this.sound);
                    this.dropchips[3].codeNo = 2;
                    this.dropchips[4].chip = new SeedCanon1(this.sound);
                    this.dropchips[4].codeNo = 3;
                    this.havezenny = 800;
                    break;
            }
        }

        public override void PositionDirectSet()
        {
            this.positionDirect = new Vector2((float)(position.X * 40.0 + 24.0) + 4 * this.UnionRebirth, (float)(position.Y * 24.0 + 62.0));
        }

        public override void Init()
        {
            this.positionre = this.position;
            this.PositionDirectSet();
            if (!(this.union == Panel.COLOR.blue ? !this.CanLeft : !this.CanRight))
                return;
            this.Angle();
        }

        protected override void Moving()
        {
            this.neutlal = true;
            float speed = 1f;
            switch (this.version)
            {
                case 1:
                    speed = 0.3f;
                    break;
                case 2:
                    speed = 0.5f;
                    break;
            }
            int angle = 0;
            switch (this.motion)
            {
                case FlowerTank.MOTION.up:
                    if (!this.CanUP && !this.slideInit)
                    {
                        this.Angle();
                        break;
                    }
                    break;
                case FlowerTank.MOTION.down:
                    if (!this.CanDown && !this.slideInit)
                    {
                        this.Angle();
                        break;
                    }
                    break;
                case FlowerTank.MOTION.left:
                    if (!this.CanLeft && !this.slideInit)
                    {
                        this.Angle();
                        break;
                    }
                    break;
                case FlowerTank.MOTION.right:
                    if (!this.CanRight && !this.slideInit)
                    {
                        this.Angle();
                        break;
                    }
                    break;
            }
            switch (this.motion)
            {
                case FlowerTank.MOTION.up:
                    angle = 2;
                    break;
                case FlowerTank.MOTION.down:
                    angle = 3;
                    break;
                case FlowerTank.MOTION.left:
                    angle = this.union == Panel.COLOR.blue ? 0 : 1;
                    break;
                case FlowerTank.MOTION.right:
                    angle = this.union == Panel.COLOR.blue ? 1 : 0;
                    break;
            }
            if ((this.CanUP || this.CanDown || (this.CanLeft || this.CanRight)) && (this.parent.nowscene != SceneBattle.BATTLESCENE.end && !this.HeviSand) || this.slideInit)
            {
                if (this.SlideMove(speed, angle))
                {
                    this.SlideMoveEnd();
                    this.PositionDirectSet();
                    if ((this.CanUP || this.CanDown || (this.CanLeft || this.CanRight)) && this.parent.nowscene != SceneBattle.BATTLESCENE.end && !this.HeviSand)
                    {
                        bool flag = false;
                        if (this.version == 0)
                            this.motion = (FlowerTank.MOTION)this.Random.Next(4);
                        switch (this.motion)
                        {
                            case FlowerTank.MOTION.up:
                                if (!this.CanUP)
                                {
                                    flag = true;
                                    break;
                                }
                                break;
                            case FlowerTank.MOTION.down:
                                if (!this.CanDown)
                                {
                                    flag = true;
                                    break;
                                }
                                break;
                            case FlowerTank.MOTION.left:
                                if (!this.CanLeft)
                                {
                                    flag = true;
                                    break;
                                }
                                break;
                            case FlowerTank.MOTION.right:
                                if (!this.CanRight)
                                {
                                    flag = true;
                                    break;
                                }
                                break;
                        }
                        this.shot = false;
                        if (flag)
                        {
                            FlowerTank.MOTION motion = this.motion;
                            this.Angle();
                            if (motion == this.motion) { }
                        }
                        this.moveend = false;
                    }
                }
                else
                {
                    switch (this.motion)
                    {
                        case FlowerTank.MOTION.up:
                            if (this.moveflame)
                            {
                                if (this.frame >= 5)
                                    this.frame = 0;
                                this.animationpoint.X = this.AnimeUP(this.frame).X;
                                break;
                            }
                            break;
                        case FlowerTank.MOTION.down:
                            if (this.moveflame)
                            {
                                if (this.frame >= 5)
                                    this.frame = 0;
                                this.animationpoint.X = this.AnimeDown(this.frame).X;
                                break;
                            }
                            break;
                        case FlowerTank.MOTION.left:
                            if (this.moveflame)
                            {
                                if (this.frame >= 6)
                                    this.frame = 0;
                                if (this.union == Panel.COLOR.blue)
                                    this.animationpoint.X = this.AnimeLeft(this.frame).X;
                                else
                                    this.animationpoint.X = this.AnimeRight(this.frame).X;
                                break;
                            }
                            break;
                        case FlowerTank.MOTION.right:
                            if (this.moveflame)
                            {
                                if (this.frame >= 6)
                                    this.frame = 0;
                                if (this.union == Panel.COLOR.blue)
                                    this.animationpoint.X = this.AnimeRight(this.frame).X;
                                else
                                    this.animationpoint.X = this.AnimeLeft(this.frame).X;
                                break;
                            }
                            break;
                    }
                }
                switch (this.motionup)
                {
                    case FlowerTank.MOTIONUP.neutral:
                        if (!this.shot || this.version == 0)
                        {
                            bool flag = false;
                            int num = 0;
                            foreach (CharacterBase characterBase in this.parent.AllChara())
                            {
                                if (characterBase.union == this.UnionEnemy && characterBase.position.Y == this.position.Y)
                                {
                                    flag = true;
                                    num = characterBase.position.X;
                                    break;
                                }
                            }
                            if (flag)
                            {
                                this.counterTiming = true;
                                this.shot = true;
                                this.sound.PlaySE(SoundEffect.canon);
                                this.motionup = FlowerTank.MOTIONUP.attack;
                                this.parent.effects.Add(new Smoke(this.sound, this.parent, new Vector2(this.positionDirect.X + 30 * this.UnionRebirth, this.positionDirect.Y - 8f), this.position, ChipBase.ELEMENT.normal));
                                Point end = new Point(this.position.X + 3 * this.UnionRebirth, this.position.Y);
                                if (this.version == 0)
                                    end = new Point(this.position.X + this.Random.Next(2, 4) * this.UnionRebirth, this.position.Y);
                                this.parent.attacks.Add(new CanonBomb(this.sound, this.parent, this.positionre.X, this.positionre.Y, this.union, this.Power, 1, new Vector2(this.positionDirect.X + 30 * this.UnionRebirth, this.positionDirect.Y - 9f), end, 40, CanonBomb.TYPE.single, false, CanonBomb.TYPE.single));
                                this.shot = true;
                            }
                            break;
                        }
                        break;
                    case FlowerTank.MOTIONUP.attack:
                        if (this.moveflame)
                        {
                            ++this.animationpointUP.X;
                            if (this.animationpointUP.X >= 4)
                            {
                                this.counterTiming = false;
                                this.animationpointUP.X = 0;
                                this.motionup = FlowerTank.MOTIONUP.neutral;
                            }
                            break;
                        }
                        break;
                }
            }
            else
            {
                this.positionre = this.position;
                this.PositionDirectSet();
                this.Angle();
            }
            this.FlameControl();
            this.MoveAftar();
        }

        private void Angle()
        {
            if (this.motion == FlowerTank.MOTION.up)
            {
                if (this.CanRight)
                    this.motion = FlowerTank.MOTION.right;
                else if (this.CanDown)
                {
                    this.motion = FlowerTank.MOTION.down;
                }
                else
                {
                    if (!this.CanLeft)
                        return;
                    this.motion = FlowerTank.MOTION.left;
                }
            }
            else if (this.motion == FlowerTank.MOTION.right)
            {
                if (this.CanDown)
                    this.motion = FlowerTank.MOTION.down;
                else if (this.CanLeft)
                {
                    this.motion = FlowerTank.MOTION.left;
                }
                else
                {
                    if (!this.CanUP)
                        return;
                    this.motion = FlowerTank.MOTION.up;
                }
            }
            else if (this.motion == FlowerTank.MOTION.down)
            {
                if (this.CanLeft)
                    this.motion = FlowerTank.MOTION.left;
                else if (this.CanUP)
                {
                    this.motion = FlowerTank.MOTION.up;
                }
                else
                {
                    if (!this.CanRight)
                        return;
                    this.motion = FlowerTank.MOTION.right;
                }
            }
            else
            {
                if (this.motion != FlowerTank.MOTION.left)
                    return;
                if (this.CanUP)
                    this.motion = FlowerTank.MOTION.up;
                else if (this.CanRight)
                    this.motion = FlowerTank.MOTION.right;
                else if (this.CanDown)
                    this.motion = FlowerTank.MOTION.down;
            }
        }

        public override void Render(IRenderer dg)
        {
            int num1 = this.union == Panel.COLOR.blue ? 5 : -5;
            int num2 = (int)this.positionDirect.X - 4 + num1;
            Point shake1 = this.Shake;
            int x1 = shake1.X;
            double num3 = num2 + x1;
            int num4 = (int)this.positionDirect.Y + 12;
            shake1 = this.Shake;
            int y1 = shake1.Y;
            double num5 = num4 + y1;
            this._position = new Vector2((float)num3, (float)num5);
            this._rect = new Rectangle(this.animationpoint.X * this.wide, Math.Min(this.version, (byte)4) * this.height, this.wide, this.height);
            if (this.Hp <= 0)
                this.Death(this._rect, new Rectangle(this.animationpoint.X * this.wide, 0, this.wide, this.height), this._position, this.picturename);
            if (this.alfha < byte.MaxValue)
                this.color = Color.FromArgb(alfha, byte.MaxValue, byte.MaxValue, byte.MaxValue);
            else
                this.color = this.mastorcolor;
            if (this.whitetime == 0)
            {
                if (this.version == 0)
                    this._rect.Y = this.height * 5;
                dg.DrawImage(dg, this.picturename, this._rect, false, this._position, this.rebirth, this.color);
                int num6 = (int)this.positionDirect.X - 4 + num1;
                Point shake2 = this.Shake;
                int x2 = shake2.X;
                double num7 = num6 + x2;
                int num8 = (int)this.positionDirect.Y - 4;
                shake2 = this.Shake;
                int y2 = shake2.Y;
                double num9 = num8 + y2;
                this._position = new Vector2((float)num7, (float)num9);
                this._rect = new Rectangle(624 + this.animationpointUP.X * this.wide, Math.Min(this.version, (byte)4) * this.height, this.wide, this.height);
                if (this.version == 0)
                    this._rect.Y = this.height * 5;
                dg.DrawImage(dg, this.picturename, this._rect, false, this._position, this.rebirth, this.color);
            }
            else
            {
                this._rect.Y = this.animationpoint.Y;
                dg.DrawImage(dg, this.picturename, this._rect, false, this._position, this.rebirth, this.color);
                this._position = new Vector2((int)this.positionDirect.X - 4 + num1 + this.Shake.X, (int)this.positionDirect.Y - 4 + this.Shake.Y);
                this._rect = new Rectangle(624 + this.animationpointUP.X * this.wide, Math.Min(this.version, (byte)4) * this.height, this.wide, this.height)
                {
                    Y = this.animationpoint.Y
                };
                dg.DrawImage(dg, this.picturename, this._rect, false, this._position, this.rebirth, this.color);
            }
            this.HPposition = new Vector2((int)this.positionDirect.X, (int)this.positionDirect.Y - this.height / 2 - 3);
            this.Nameprint(dg, this.printNumber);
        }

        private Point AnimeUP(int waitflame)
        {
            return this.Return(new int[6] { 0, 1, 2, 3, 4, 5 }, new int[6]
            {
        10,
        9,
        8,
        7,
        6,
        5
            }, 1, waitflame);
        }

        private Point AnimeDown(int waitflame)
        {
            return this.Return(new int[6] { 0, 1, 2, 3, 4, 5 }, new int[6]
            {
        5,
        6,
        7,
        8,
        9,
        10
            }, 1, waitflame);
        }

        private Point AnimeLeft(int waitflame)
        {
            return this.Return(new int[5] { 0, 1, 2, 3, 4 }, new int[5]
            {
        0,
        1,
        2,
        3,
        4
            }, 1, waitflame);
        }

        private Point AnimeRight(int waitflame)
        {
            return this.Return(new int[5] { 0, 1, 2, 3, 4 }, new int[5]
            {
        4,
        3,
        2,
        1,
        0
            }, 1, waitflame);
        }

        private Point AnimeAttack(int waitflame)
        {
            return this.Return(new int[4] { 0, 1, 2, 3 }, new int[4]
            {
        0,
        1,
        2,
        3
            }, 0, waitflame);
        }

        private enum MOTION
        {
            up,
            down,
            left,
            right,
        }

        private enum MOTIONUP
        {
            neutral,
            attack,
        }
    }
}

