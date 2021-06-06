using NSAttack;
using NSBattle;
using NSChip;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;
using Common.Vectors;
using System;
using System.Drawing;

namespace NSEnemy
{
    internal class GekoHuts : EnemyBase
    {
        private GekoHuts.MOTION motion = GekoHuts.MOTION.left;
        private bool moveend = true;
        private int moving;
        private readonly int nspeed;
        private bool attacknow;
        private readonly int moveMany;
        private readonly int roopmove;
        private bool angry;

        public GekoHuts(IAudioEngine s, SceneBattle p, int pX, int pY, byte n, Panel.COLOR u, byte v)
          : base(s, p, pX, pY, n, u, v)
        {
            this.helpPosition.X = 8;
            this.helpPosition.Y = -8;
            for (int index = 0; index < this.dropchips.Length; ++index)
                this.dropchips[index] = new ChipFolder(this.sound);
            this.picturename = "gekohat";
            this.race = EnemyBase.ENEMY.virus;
            this.element = ChipBase.ELEMENT.aqua;
            this.wide = 48;
            this.height = 48;
            this.printhp = true;
            this.printNumber = true;
            this.Noslip = true;
            this.name = ShanghaiEXE.Translate("Enemy.GekoHutsName1");
            switch (this.version)
            {
                case 0:
                    this.power = 150;
                    this.hp = 2000;
                    this.nspeed = 1;
                    this.moveMany = 8;
                    this.name = ShanghaiEXE.Translate("Enemy.GekoHutsName2");
                    break;
                case 1:
                    this.power = 40;
                    this.hp = 60;
                    this.nspeed = 3;
                    this.moveMany = 4;
                    break;
                case 2:
                    this.power = 80;
                    this.hp = 100;
                    this.nspeed = 2;
                    this.moveMany = 4;
                    break;
                case 3:
                    this.power = 100;
                    this.hp = 180;
                    this.nspeed = 1;
                    this.moveMany = 4;
                    break;
                case 4:
                    this.power = 120;
                    this.hp = 240;
                    this.name = ShanghaiEXE.Translate("Enemy.GekoHutsName3");
                    this.printNumber = false;
                    this.nspeed = 1;
                    this.moveMany = 4;
                    break;
                default:
                    this.power = 150 + (version - 4) * 20;
                    this.hp = 240 + (version - 4) * 40;
                    this.name = ShanghaiEXE.Translate("Enemy.GekoHutsName4") + (version - 3).ToString();
                    this.printNumber = false;
                    this.nspeed = 1;
                    this.moveMany = 4;
                    break;
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
                    this.dropchips[0].chip = new LjiOtama1(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new LjiOtama1(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new LjiOtama1(this.sound);
                    this.dropchips[2].codeNo = 1;
                    this.dropchips[3].chip = new LjiOtama1(this.sound);
                    this.dropchips[3].codeNo = 2;
                    this.dropchips[4].chip = new LjiOtama1(this.sound);
                    if (this.Random.Next(4) < 3)
                        this.dropchips[4].codeNo = 2;
                    else
                        this.dropchips[4].codeNo = 3;
                    this.havezenny = 300;
                    break;
                case 2:
                    this.dropchips[0].chip = new LjiOtama2(this.sound);
                    this.dropchips[0].codeNo = 1;
                    this.dropchips[1].chip = new LjiOtama2(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new LjiOtama2(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new LjiOtama2(this.sound);
                    this.dropchips[3].codeNo = 3;
                    this.dropchips[4].chip = new LjiOtama2(this.sound);
                    this.dropchips[4].codeNo = 2;
                    this.havezenny = 600;
                    break;
                case 3:
                    this.dropchips[0].chip = new LjiOtama3(this.sound);
                    this.dropchips[0].codeNo = 3;
                    this.dropchips[1].chip = new LjiOtama3(this.sound);
                    this.dropchips[1].codeNo = 3;
                    this.dropchips[2].chip = new LjiOtama3(this.sound);
                    this.dropchips[2].codeNo = 1;
                    this.dropchips[3].chip = new LjiOtama3(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new LjiOtama3(this.sound);
                    this.dropchips[4].codeNo = 2;
                    this.havezenny = 800;
                    break;
                case 8:
                    this.dropchips[0].chip = new LjiOtama1(this.sound);
                    this.dropchips[0].codeNo = 2;
                    this.dropchips[1].chip = new LjiOtama2(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new LjiOtama2(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new LjiOtama3(this.sound);
                    this.dropchips[3].codeNo = 3;
                    this.dropchips[4].chip = new LjiOtamaX(this.sound);
                    this.dropchips[4].codeNo = this.Random.Next(4);
                    this.havezenny = 8000;
                    break;
                default:
                    this.dropchips[0].chip = new LjiOtama1(this.sound);
                    this.dropchips[0].codeNo = 2;
                    this.dropchips[1].chip = new LjiOtama2(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new LjiOtama2(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new LjiOtama3(this.sound);
                    this.dropchips[3].codeNo = 2;
                    this.dropchips[4].chip = new LjiOtama1(this.sound);
                    this.dropchips[4].codeNo = 3;
                    this.havezenny = 800;
                    if (this.version < 8)
                        break;
                    this.dropchips[4].chip = new LjiOtamaX(this.sound);
                    this.dropchips[4].codeNo = this.Random.Next(4);
                    this.havezenny = 8000;
                    break;
            }
        }

        public override void PositionDirectSet()
        {
            this.positionDirect = new Vector2((float)(position.X * 40.0 + 24.0) + 2 * this.UnionRebirth, (float)(position.Y * 24.0 + 58.0));
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
            if (!this.angry && this.hp <= this.HpMax / 2)
            {
                this.angry = true;
                this.animationpoint.X += 2;
            }
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
                        speed = 0.4f;
                        break;
                    case 2:
                        speed = 0.8f;
                        break;
                }
                if (this.angry)
                    speed *= 3f;
                int angle = 0;
                switch (this.motion)
                {
                    case GekoHuts.MOTION.up:
                        if (!this.CanUP && !this.slideInit)
                        {
                            this.Angle();
                            break;
                        }
                        break;
                    case GekoHuts.MOTION.down:
                        if (!this.CanDown && !this.slideInit)
                        {
                            this.Angle();
                            break;
                        }
                        break;
                    case GekoHuts.MOTION.left:
                        if (!this.CanLeft && !this.slideInit)
                        {
                            this.Angle();
                            break;
                        }
                        break;
                    case GekoHuts.MOTION.right:
                        if (!this.CanRight && !this.slideInit)
                        {
                            this.Angle();
                            break;
                        }
                        break;
                }
                switch (this.motion)
                {
                    case GekoHuts.MOTION.up:
                        angle = 2;
                        break;
                    case GekoHuts.MOTION.down:
                        angle = 3;
                        break;
                    case GekoHuts.MOTION.left:
                        angle = this.union == Panel.COLOR.blue ? 0 : 1;
                        break;
                    case GekoHuts.MOTION.right:
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
                            this.motion = (GekoHuts.MOTION)this.Random.Next(4);
                            switch (this.motion)
                            {
                                case GekoHuts.MOTION.up:
                                    if (!this.CanUP)
                                    {
                                        flag = true;
                                        break;
                                    }
                                    break;
                                case GekoHuts.MOTION.down:
                                    if (!this.CanDown)
                                    {
                                        flag = true;
                                        break;
                                    }
                                    break;
                                case GekoHuts.MOTION.left:
                                    if (!this.CanLeft)
                                    {
                                        flag = true;
                                        break;
                                    }
                                    break;
                                case GekoHuts.MOTION.right:
                                    if (!this.CanRight)
                                    {
                                        flag = true;
                                        break;
                                    }
                                    break;
                            }
                            if (flag)
                            {
                                GekoHuts.MOTION motion = this.motion;
                                this.Angle();
                                if (motion == this.motion) { }
                            }
                            ++this.moving;
                            this.moveend = false;
                            if (this.moving >= this.moveMany)
                            {
                                this.moving = 0;
                                this.attacknow = true;
                                ++this.animationpoint.X;
                                this.frame = 0;
                            }
                        }
                    }
                    else if (this.moveflame && this.frame >= 12)
                        this.frame = 0;
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
                if (this.frame == 10)
                {
                    this.counterTiming = true;
                    this.sound.PlaySE(SoundEffect.bound);
                    AttackBase attackBase = new Otama(this.sound, this.parent, this.position.X, this.position.Y, this.union, this.Power, 2, this.positionDirect, this.element, 5);
                    if (this.version == 0)
                        attackBase.breaking = true;
                    this.parent.attacks.Add(attackBase);
                }
                else if (this.frame > 20)
                {
                    this.counterTiming = false;
                    this.attacknow = false;
                    --this.animationpoint.X;
                }
            }
            this.FlameControl();
            this.MoveAftar();
        }

        private void Angle()
        {
            if (this.motion == GekoHuts.MOTION.up)
            {
                if (this.CanRight)
                    this.motion = GekoHuts.MOTION.right;
                else if (this.CanDown)
                {
                    this.motion = GekoHuts.MOTION.down;
                }
                else
                {
                    if (!this.CanLeft)
                        return;
                    this.motion = GekoHuts.MOTION.left;
                }
            }
            else if (this.motion == GekoHuts.MOTION.right)
            {
                if (this.CanDown)
                    this.motion = GekoHuts.MOTION.down;
                else if (this.CanLeft)
                {
                    this.motion = GekoHuts.MOTION.left;
                }
                else
                {
                    if (!this.CanUP)
                        return;
                    this.motion = GekoHuts.MOTION.up;
                }
            }
            else if (this.motion == GekoHuts.MOTION.down)
            {
                if (this.CanLeft)
                    this.motion = GekoHuts.MOTION.left;
                else if (this.CanUP)
                {
                    this.motion = GekoHuts.MOTION.up;
                }
                else
                {
                    if (!this.CanRight)
                        return;
                    this.motion = GekoHuts.MOTION.right;
                }
            }
            else
            {
                if (this.motion != GekoHuts.MOTION.left)
                    return;
                if (this.CanUP)
                    this.motion = GekoHuts.MOTION.up;
                else if (this.CanRight)
                    this.motion = GekoHuts.MOTION.right;
                else if (this.CanDown)
                    this.motion = GekoHuts.MOTION.down;
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
            if (this.version == 0)
                this._rect.Y = 5 * this.height;
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
            this.HPposition = new Vector2((int)this.positionDirect.X, (int)this.positionDirect.Y + this.height / 2 + 8);
            this.Nameprint(dg, this.printNumber);
        }

        private Point AnimeMove(int waitflame)
        {
            return this.Return(new int[1], new int[1], 1, waitflame);
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

