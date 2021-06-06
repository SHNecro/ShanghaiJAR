using NSAttack;
using NSBattle;
using NSBattle.Character;
using NSChip;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;
using Common.Vectors;
using System;
using System.Drawing;

namespace NSEnemy
{
    internal class EvilEye : EnemyBase
    {
        private EvilEye.MOTION motion = EvilEye.MOTION.left;
        private bool moveend = true;
        private int moving;
        private readonly int nspeed;
        private bool attacknow;
        private bool attackanimation;
        private readonly int moveMany;
        private readonly int roopmove;
        private bool attacked;

        public EvilEye(IAudioEngine s, SceneBattle p, int pX, int pY, byte n, Panel.COLOR u, byte v)
          : base(s, p, pX, pY, n, u, v)
        {
            this.helpPosition.X = 8;
            this.helpPosition.Y = -8;
            for (int index = 0; index < this.dropchips.Length; ++index)
                this.dropchips[index] = new ChipFolder(this.sound);
            this.picturename = "evileye";
            this.race = EnemyBase.ENEMY.virus;
            this.element = ChipBase.ELEMENT.normal;
            this.wide = 64;
            this.height = 40;
            this.printhp = true;
            this.printNumber = false;
            this.Noslip = true;
            switch (this.version)
            {
                case 0:
                    this.power = 150;
                    this.hp = 1200;
                    this.nspeed = 1;
                    this.moveMany = 3;
                    this.name = ShanghaiEXE.Translate("Enemy.EvilEyeName1");
                    break;
                case 1:
                    this.name = ShanghaiEXE.Translate("Enemy.EvilEyeName2");
                    this.power = 150;
                    this.hp = 150;
                    this.nspeed = 3;
                    this.moveMany = 2;
                    break;
                case 2:
                    this.name = ShanghaiEXE.Translate("Enemy.EvilEyeName3");
                    this.power = 180;
                    this.hp = 280;
                    this.nspeed = 2;
                    this.moveMany = 3;
                    break;
                case 3:
                    this.name = ShanghaiEXE.Translate("Enemy.EvilEyeName4");
                    this.power = 210;
                    this.hp = 450;
                    this.nspeed = 1;
                    this.moveMany = 4;
                    break;
                case 4:
                    this.power = 100;
                    this.hp = 500;
                    this.name = ShanghaiEXE.Translate("Enemy.EvilEyeName5");
                    this.printNumber = false;
                    this.nspeed = 1;
                    this.moveMany = 4;
                    break;
                default:
                    this.power = 100 + (version - 4) * 20;
                    this.hp = 500 + (version - 4) * 50;
                    int num = version - 3;
                    this.name = ShanghaiEXE.Translate("Enemy.EvilEyeName6") + num.ToString();
                    this.printNumber = false;
                    this.nspeed = 1;
                    this.moveMany = 4;
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
                    this.dropchips[0].chip = new ChargeCanon1(this.sound);
                    this.dropchips[0].codeNo = 1;
                    this.dropchips[1].chip = new ChargeCanon1(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new ChargeCanon1(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new ChargeCanon1(this.sound);
                    this.dropchips[3].codeNo = 2;
                    this.dropchips[4].chip = new ChargeCanon1(this.sound);
                    if (this.Random.Next(4) < 3)
                        this.dropchips[4].codeNo = 2;
                    else
                        this.dropchips[4].codeNo = 3;
                    this.havezenny = 300;
                    break;
                case 2:
                    this.dropchips[0].chip = new ChargeCanon2(this.sound);
                    this.dropchips[0].codeNo = 2;
                    this.dropchips[1].chip = new ChargeCanon2(this.sound);
                    this.dropchips[1].codeNo = 2;
                    this.dropchips[2].chip = new ChargeCanon2(this.sound);
                    this.dropchips[2].codeNo = 1;
                    this.dropchips[3].chip = new ChargeCanon2(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new ChargeCanon2(this.sound);
                    this.dropchips[4].codeNo = 3;
                    this.havezenny = 600;
                    break;
                case 3:
                    this.dropchips[0].chip = new ChargeCanon3(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new ChargeCanon3(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new ChargeCanon3(this.sound);
                    this.dropchips[2].codeNo = 1;
                    this.dropchips[3].chip = new ChargeCanon3(this.sound);
                    this.dropchips[3].codeNo = 2;
                    this.dropchips[4].chip = new ChargeCanon3(this.sound);
                    this.dropchips[4].codeNo = 3;
                    this.havezenny = 800;
                    break;
                case 8:
                    this.dropchips[0].chip = new MedousaEye(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new MedousaEye(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new MedousaEye(this.sound);
                    this.dropchips[2].codeNo = 1;
                    this.dropchips[3].chip = new MedousaEye(this.sound);
                    this.dropchips[3].codeNo = 2;
                    this.dropchips[4].chip = new ChargeCanonX(this.sound);
                    this.dropchips[4].codeNo = this.Random.Next(4);
                    this.havezenny = 8000;
                    break;
                default:
                    this.dropchips[0].chip = new MedousaEye(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new MedousaEye(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new MedousaEye(this.sound);
                    this.dropchips[2].codeNo = 1;
                    this.dropchips[3].chip = new MedousaEye(this.sound);
                    this.dropchips[3].codeNo = 2;
                    this.dropchips[4].chip = new MedousaEye(this.sound);
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
            if (!this.attacknow)
            {
                float speed = 1f;
                switch (this.version)
                {
                    case 0:
                        speed = 2f;
                        break;
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
                    case EvilEye.MOTION.up:
                        if (!this.CanUP && !this.slideInit)
                        {
                            this.Angle();
                            break;
                        }
                        break;
                    case EvilEye.MOTION.down:
                        if (!this.CanDown && !this.slideInit)
                        {
                            this.Angle();
                            break;
                        }
                        break;
                    case EvilEye.MOTION.left:
                        if (!this.CanLeft && !this.slideInit)
                        {
                            this.Angle();
                            break;
                        }
                        break;
                    case EvilEye.MOTION.right:
                        if (!this.CanRight && !this.slideInit)
                        {
                            this.Angle();
                            break;
                        }
                        break;
                }
                switch (this.motion)
                {
                    case EvilEye.MOTION.up:
                        angle = 2;
                        break;
                    case EvilEye.MOTION.down:
                        angle = 3;
                        break;
                    case EvilEye.MOTION.left:
                        angle = this.union == Panel.COLOR.blue ? 0 : 1;
                        break;
                    case EvilEye.MOTION.right:
                        angle = this.union == Panel.COLOR.blue ? 1 : 0;
                        break;
                }
                if ((this.CanUP || this.CanDown || (this.CanLeft || this.CanRight)) && (this.parent.nowscene != SceneBattle.BATTLESCENE.end && !this.HeviSand) || this.slideInit)
                {
                    if (this.version == 0 && !this.attacked)
                    {
                        foreach (CharacterBase characterBase in this.parent.AllChara())
                        {
                            if (characterBase.union == this.UnionEnemy && characterBase.position.Y == this.position.Y)
                            {
                                this.attacked = true;
                                EYEBall eyeBall = new EYEBall(this.sound, this.parent, this.position.X, this.position.Y, this.union, this.Power, 2, this.positionDirect, ChipBase.ELEMENT.normal, 16);
                                eyeBall.positionDirect.Y += 8f;
                                eyeBall.breakinvi = false;
                                this.parent.attacks.Add(eyeBall);
                                break;
                            }
                        }
                    }
                    if (this.SlideMove(speed, angle))
                    {
                        this.SlideMoveEnd();
                        this.PositionDirectSet();
                        if ((this.CanUP || this.CanDown || (this.CanLeft || this.CanRight)) && this.parent.nowscene != SceneBattle.BATTLESCENE.end && !this.HeviSand)
                        {
                            bool flag = false;
                            switch (this.motion)
                            {
                                case EvilEye.MOTION.up:
                                    if (!this.CanUP)
                                    {
                                        flag = true;
                                        break;
                                    }
                                    break;
                                case EvilEye.MOTION.down:
                                    if (!this.CanDown)
                                    {
                                        flag = true;
                                        break;
                                    }
                                    break;
                                case EvilEye.MOTION.left:
                                    if (!this.CanLeft)
                                    {
                                        flag = true;
                                        break;
                                    }
                                    break;
                                case EvilEye.MOTION.right:
                                    if (!this.CanRight)
                                    {
                                        flag = true;
                                        break;
                                    }
                                    break;
                            }
                            this.attacked = false;
                            if (flag)
                            {
                                EvilEye.MOTION motion = this.motion;
                                this.Angle();
                                this.attacked = false;
                                if (motion == this.motion) { }
                            }
                            ++this.moving;
                            this.moveend = false;
                            if (this.moving >= this.moveMany && this.version > 0)
                            {
                                this.moving = 0;
                                this.attacknow = true;
                                this.frame = 0;
                            }
                        }
                    }
                    else if (this.moveflame)
                    {
                        if (this.frame >= 12)
                            this.frame = 0;
                        this.animationpoint.X = this.AnimeMove(this.frame / 3).X;
                    }
                }
                else
                {
                    this.positionre = this.position;
                    this.PositionDirectSet();
                    this.Angle();
                }
            }
            else if (this.moveflame)
            {
                this.animationpoint.X = this.attackanimation ? 4 : 5;
                this.attackanimation = !this.attackanimation;
                if (this.frame == 5)
                    this.counterTiming = true;
                if (this.frame == 15)
                {
                    this.counterTiming = false;
                    AttackBase attackBase = new EYEBall(this.sound, this.parent, this.position.X, this.position.Y, this.union, this.Power, 2, this.positionDirect, ChipBase.ELEMENT.normal, 4);
                    attackBase.positionDirect.Y += 8f;
                    this.parent.attacks.Add(attackBase);
                }
                else if (this.frame > 30)
                {
                    this.attacknow = false;
                    this.animationpoint.X = 0;
                }
            }
            this.FlameControl();
            this.MoveAftar();
        }

        private void Angle()
        {
            if (this.motion == EvilEye.MOTION.up)
            {
                if (this.CanRight)
                    this.motion = EvilEye.MOTION.right;
                else if (this.CanDown)
                {
                    this.motion = EvilEye.MOTION.down;
                }
                else
                {
                    if (!this.CanLeft)
                        return;
                    this.motion = EvilEye.MOTION.left;
                }
            }
            else if (this.motion == EvilEye.MOTION.right)
            {
                if (this.CanDown)
                    this.motion = EvilEye.MOTION.down;
                else if (this.CanLeft)
                {
                    this.motion = EvilEye.MOTION.left;
                }
                else
                {
                    if (!this.CanUP)
                        return;
                    this.motion = EvilEye.MOTION.up;
                }
            }
            else if (this.motion == EvilEye.MOTION.down)
            {
                if (this.CanLeft)
                    this.motion = EvilEye.MOTION.left;
                else if (this.CanUP)
                {
                    this.motion = EvilEye.MOTION.up;
                }
                else
                {
                    if (!this.CanRight)
                        return;
                    this.motion = EvilEye.MOTION.right;
                }
            }
            else
            {
                if (this.motion != EvilEye.MOTION.left)
                    return;
                if (this.CanUP)
                    this.motion = EvilEye.MOTION.up;
                else if (this.CanRight)
                    this.motion = EvilEye.MOTION.right;
                else if (this.CanDown)
                    this.motion = EvilEye.MOTION.down;
            }
        }

        public override void Render(IRenderer dg)
        {
            int num1 = (int)this.positionDirect.X - 4 + (this.union == Panel.COLOR.blue ? 5 : -5);
            Point shake = this.Shake;
            int x = shake.X;
            double num2 = num1 + x;
            int num3 = (int)this.positionDirect.Y + 12;
            shake = this.Shake;
            int y = shake.Y;
            double num4 = num3 + y;
            this._position = new Vector2((float)num2, (float)num4);
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
            }
            else
            {
                this._rect.Y = this.animationpoint.Y;
                dg.DrawImage(dg, this.picturename, this._rect, false, this._position, this.rebirth, this.color);
            }
            this.HPposition = new Vector2((int)this.positionDirect.X, (int)this.positionDirect.Y - this.height / 2 - 3);
            this.Nameprint(dg, this.printNumber);
        }

        private Point AnimeMove(int waitflame)
        {
            return this.Return(new int[4] { 0, 1, 2, 3 }, new int[4]
            {
        0,
        1,
        2,
        3
            }, 1, waitflame);
        }

        private enum MOTION
        {
            up,
            down,
            left,
            right,
        }
    }
}

