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
    internal class GunBatta : EnemyBase
    {
        private GunBatta.MOTION motion = GunBatta.MOTION.neutral;
        private int count;
        private int attackCount;
        private int flyflame;
        private readonly int time;
        private Vector2 endposition;
        private float movex;
        private float movey;
        private float plusy;
        private float speedy;
        private float plusing;
        private const int startspeed = 6;
        private DammyEnemy dammy;

        public GunBatta(IAudioEngine s, SceneBattle p, int pX, int pY, byte n, Panel.COLOR u, byte v)
          : base(s, p, pX, pY, n, u, v)
        {
            this.wantedPosition.X = -8;
            this.attackCount = this.Random.Next(2, 5);
            for (int index = 0; index < this.dropchips.Length; ++index)
                this.dropchips[index] = new ChipFolder(this.sound);
            this.name = ShanghaiEXE.Translate("Enemy.GunBattaName1");
            this.picturename = "gunbut";
            this.element = ChipBase.ELEMENT.leaf;
            this.race = EnemyBase.ENEMY.virus;
            this.Flying = false;
            this.Noslip = true;
            this.wide = 56;
            this.height = 40;
            this.hp = 50 * version;
            this.hpmax = this.hp;
            this.power = 5 + 15 * (version - 1);
            this.speed = 5 - version;
            this.time = 60 - 10 * version;
            this.printhp = true;
            this.printNumber = true;
            this.effecting = false;
            this.PositionDirectSet();
            switch (this.version)
            {
                case 0:
                    this.name = ShanghaiEXE.Translate("Enemy.GunBattaName2");
                    this.printNumber = false;
                    this.power = 150;
                    this.hp = 350;
                    this.hpmax = this.hp;
                    this.speed = 1;
                    this.time = 10;
                    this.dropchips[0].chip = new ShotGun1(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new ShotGun1(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new ShotGun1(this.sound);
                    this.dropchips[2].codeNo = 2;
                    this.dropchips[3].chip = new ShotGun1(this.sound);
                    this.dropchips[3].codeNo = 2;
                    this.dropchips[4].chip = new ShotGun1(this.sound);
                    this.dropchips[4].codeNo = 3;
                    this.havezenny = 300;
                    break;
                case 1:
                    this.power = 15;
                    this.dropchips[0].chip = new ShotGun1(this.sound);
                    this.dropchips[0].codeNo = 1;
                    this.dropchips[1].chip = new ShotGun1(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new ShotGun1(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new ShotGun1(this.sound);
                    this.dropchips[3].codeNo = 2;
                    this.dropchips[4].chip = new ShotGun1(this.sound);
                    if (this.Random.Next(4) < 3)
                        this.dropchips[4].codeNo = 2;
                    else
                        this.dropchips[4].codeNo = 3;
                    this.havezenny = 300;
                    break;
                case 2:
                    this.power = 60;
                    this.dropchips[0].chip = new ShotGun2(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new ShotGun2(this.sound);
                    this.dropchips[1].codeNo = 3;
                    this.dropchips[2].chip = new ShotGun2(this.sound);
                    this.dropchips[2].codeNo = 1;
                    this.dropchips[3].chip = new ShotGun2(this.sound);
                    this.dropchips[3].codeNo = 1;
                    this.dropchips[4].chip = new ShotGun2(this.sound);
                    this.dropchips[4].codeNo = 2;
                    this.havezenny = 600;
                    break;
                case 3:
                    this.power = 90;
                    this.dropchips[0].chip = new ShotGun3(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new ShotGun3(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new ShotGun3(this.sound);
                    this.dropchips[2].codeNo = 3;
                    this.dropchips[3].chip = new ShotGun3(this.sound);
                    this.dropchips[3].codeNo = 3;
                    this.dropchips[4].chip = new ShotGun3(this.sound);
                    this.dropchips[4].codeNo = 2;
                    this.havezenny = 800;
                    break;
                case 4:
                    this.power = 120;
                    this.name = ShanghaiEXE.Translate("Enemy.GunBattaName3");
                    this.speed = 1;
                    this.time = 20;
                    this.printNumber = false;
                    this.dropchips[0].chip = new ShotGun1(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new ShotGun1(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new ShotGun2(this.sound);
                    this.dropchips[2].codeNo = 2;
                    this.dropchips[3].chip = new ShotGun1(this.sound);
                    this.dropchips[3].codeNo = 3;
                    this.dropchips[4].chip = new ShotGun3(this.sound);
                    this.dropchips[4].codeNo = 3;
                    this.havezenny = 800;
                    break;
                default:
                    this.power = 120 + 30 * (version - 3);
                    int num = version - 3;
                    this.name = ShanghaiEXE.Translate("Enemy.GunBattaName4") + num.ToString();
                    this.speed = 1;
                    this.time = 20;
                    this.printNumber = false;
                    this.dropchips[0].chip = new ShotGun1(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new ShotGun1(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new ShotGun2(this.sound);
                    this.dropchips[2].codeNo = 2;
                    this.dropchips[3].chip = new ShotGun1(this.sound);
                    this.dropchips[3].codeNo = 3;
                    this.dropchips[4].chip = new ShotGun3(this.sound);
                    this.dropchips[4].codeNo = 3;
                    this.havezenny = 800;
                    if (this.version >= 8)
                    {
                        this.dropchips[4].chip = new ShotGunX(this.sound);
                        this.dropchips[4].codeNo = this.Random.Next(4);
                        this.havezenny = 8000;
                    }
                    {
                        num = version - 3;
                        break;
                    }
            }
            this.hpprint = this.hp;
        }

        public override void Init()
        {
            this.dammy = new DammyEnemy(this.sound, this.parent, this.position.X, this.position.Y, this, true)
            {
                nohit = true
            };
        }

        private void DammySet()
        {
            this.dammy.position = this.position;
            this.dammy.flag = true;
            this.dammy.nomove = true;
            this.dammy.effecting = true;
            this.parent.enemys.Add(dammy);
        }

        protected override void Moving()
        {
            switch (this.motion)
            {
                case GunBatta.MOTION.neutral:
                    if ((uint)this.animationpoint.X > 0U)
                        this.animationpoint.X = 0;
                    if (this.moveflame && this.frame > 8)
                    {
                        this.frame = 0;
                        if (this.count < this.attackCount && !this.badstatus[4] && this.parent.nowscene != SceneBattle.BATTLESCENE.end && !this.HeviSand)
                        {
                            this.motion = GunBatta.MOTION.hop;
                            Point point = this.parent.nowscene == SceneBattle.BATTLESCENE.end ? this.position : this.RandomTarget();
                            if (this.count == this.attackCount - 1)
                            {
                                if (this.union == Panel.COLOR.blue && this.Canmove(new Point(point.X - this.UnionRebirth, point.Y), this.number, Panel.COLOR.red))
                                {
                                    this.Noslip = true;
                                    this.effecting = true;
                                    this.positionre = point;
                                    this.positionre.X -= this.UnionRebirth;
                                    this.parent.attacks.Add(new Dummy(this.sound, this.parent, this.positionre.X, this.positionre.Y, this.union, new Point(), this.time, true));
                                    this.DammySet();
                                }
                                else
                                {
                                    this.Noslip = true;
                                    this.MoveRandom(true, true);
                                    this.parent.attacks.Add(new Dummy(this.sound, this.parent, this.positionre.X, this.positionre.Y, this.union, new Point(), this.time, true));
                                }
                            }
                            else
                                this.MoveRandom(false, false);
                            this.Throw();
                            ++this.count;
                        }
                        else if (this.parent.nowscene != SceneBattle.BATTLESCENE.end)
                        {
                            this.Noslip = true;
                            this.effecting = false;
                            this.motion = GunBatta.MOTION.attack;
                            this.counterTiming = true;
                            this.count = 0;
                        }
                        break;
                    }
                    break;
                case GunBatta.MOTION.hop:
                    if (this.moveflame)
                        this.animationpoint = this.AnimeMove(this.frame);
                    if (this.frame > 2)
                    {
                        if (this.flyflame == this.time)
                        {
                            this.motion = GunBatta.MOTION.neutral;
                            this.counterTiming = false;
                            this.plusy = 0.0f;
                            this.frame = 0;
                            this.dammy.flag = false;
                        }
                        else
                        {
                            this.positionDirect.X -= this.movex;
                            this.positionDirect.Y -= this.movey;
                            this.plusy -= this.speedy;
                            this.speedy -= this.plusing;
                            this.nohit = speedy * (double)this.speedy < 25.0;
                            if (speedy < 0.0)
                                this.position = this.positionre;
                        }
                        ++this.flyflame;
                        break;
                    }
                    break;
                case GunBatta.MOTION.attack:
                    if (this.moveflame)
                    {
                        this.animationpoint = this.AttackMove(this.frame);
                        if (this.frame == 1)
                        {
                            int num = this.union == Panel.COLOR.blue ? -1 : 1;
                            this.parent.attacks.Add(new ShotGun(this.sound, this.parent, this.position.X + num, this.position.Y, this.union, this.power / 3, ChipBase.ELEMENT.normal));
                            this.parent.attacks.Add(new ShotGun(this.sound, this.parent, this.position.X + num * 2, this.position.Y - 1, this.union, this.power / 3, ChipBase.ELEMENT.normal));
                            this.parent.attacks.Add(new ShotGun(this.sound, this.parent, this.position.X + num * 2, this.position.Y, this.union, this.power / 3, ChipBase.ELEMENT.normal));
                            this.parent.attacks.Add(new ShotGun(this.sound, this.parent, this.position.X + num * 2, this.position.Y + 1, this.union, this.power / 3, ChipBase.ELEMENT.normal));
                        }
                        if (this.frame == 5)
                        {
                            this.Noslip = true;
                            this.HitFlagReset();
                            this.attackCount = this.Random.Next(2, 5);
                            this.motion = GunBatta.MOTION.hop;
                            this.MoveRandom(false, false);
                            this.Throw();
                        }
                        break;
                    }
                    break;
            }
            if (this.effecting && !this.nohit)
                this.AttackMake(this.Power, 0, 0);
            this.FlameControl();
            this.MoveAftar();
        }

        private void Throw()
        {
            this.endposition = new Vector2(this.positionre.X * 40 + 22 + 6 * this.UnionRebirth, this.positionre.Y * 24 + 72);
            this.movex = (this.positionDirect.X - this.endposition.X) / time;
            this.movey = (this.positionDirect.Y - this.endposition.Y) / time;
            this.plusy = 0.0f;
            this.speedy = 6f;
            this.plusing = this.speedy / (this.time / 2);
            this.flyflame = 0;
        }

        public override void PositionDirectSet()
        {
            this.positionDirect = new Vector2((float)(position.X * 40.0 + 22.0) + 6 * this.UnionRebirth, (float)(position.Y * 24.0 + 72.0));
        }

        public override void Render(IRenderer dg)
        {
            this._position = new Vector2((int)this.positionDirect.X + this.Shake.X, (int)this.positionDirect.Y + this.plusy + Shake.Y);
            this._rect = new Rectangle(this.animationpoint.X * this.wide, Math.Min(this.version, (byte)4) * (this.height * 2), this.wide, this.height);
            if (this.version == 0)
                this._rect.Y = 5 * (this.height * 2);
            Rectangle rect = this._rect;
            rect.Y += this.height;
            if (this.Hp <= 0)
                this.Death(this._rect, new Rectangle(this.animationpoint.X * this.wide, 0, this.wide, this.height), this._position, this.picturename);
            if (this.alfha < byte.MaxValue)
                this.color = Color.FromArgb(alfha, byte.MaxValue, byte.MaxValue, byte.MaxValue);
            else
                this.color = this.mastorcolor;
            if (this.whitetime == 0)
            {
                dg.DrawImage(dg, this.picturename, rect, false, this.positionDirect, this.rebirth, this.color);
                dg.DrawImage(dg, this.picturename, this._rect, false, this._position, this.rebirth, this.color);
            }
            else
            {
                this._rect.Y = 0;
                rect = this._rect;
                rect.Y += this.height;
                dg.DrawImage(dg, this.picturename, rect, false, this.positionDirect, this.rebirth, this.color);
                dg.DrawImage(dg, this.picturename, this._rect, false, this._position, this.rebirth, this.color);
            }
            this.HPposition = new Vector2((int)this.positionDirect.X + 2, (int)this.positionDirect.Y + this.height / 2 - 40);
            this.Nameprint(dg, this.printNumber);
        }

        private Point AnimeMove(int waitflame)
        {
            return this.Return(new int[10]
            {
        0,
        1,
        2,
        3,
        4,
        15,
        16,
        17,
        18,
        19
            }, new int[10] { 1, 2, 3, 4, 5, 4, 3, 2, 1, 0 }, 0, waitflame);
        }

        private Point AttackMove(int waitflame)
        {
            return this.Return(new int[5] { 0, 1, 2, 3, 4 }, new int[5]
            {
        0,
        6,
        7,
        8,
        0
            }, 0, waitflame);
        }

        private enum MOTION
        {
            neutral,
            hop,
            attack,
        }
    }
}

