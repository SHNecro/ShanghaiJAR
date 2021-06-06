using NSAttack;
using NSBattle;
using NSChip;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEffect;
using NSGame;
using NSObject;
using Common.Vectors;
using System;
using System.Drawing;

namespace NSEnemy
{
    internal class BeetleMan : NaviBase
    {
        private readonly int[] powers = new int[4] { 20, 10, 30, 0 };
        private readonly int nspeed = 2;
        private Shadow shadow;
        private readonly int moveroop;
        private BeetleMan.ATTACK attack;
        private int roopneutral;
        private int roopmove;
        private bool bright;
        private int chargeCount;
        private int time;
        private Vector2 endposition;
        private float movex;
        private float movey;
        private float plusy;
        private float speedy;
        private float plusing;
        private const int startspeed = 6;
        private int flyflame;
        private int bugnonevcount;
        private int atackroop;
        private readonly bool atack;
        private int brigt;
        private bool bugholeset;
        private bool bugholesetEnd;
        private BugHole bughole;

        public BeetleMan(IAudioEngine s, SceneBattle p, int pX, int pY, byte n, Panel.COLOR u, byte v)
          : base(s, p, pX, pY, n, u, v)
        {
            for (int index = 0; index < this.dropchips.Length; ++index)
                this.dropchips[index] = new ChipFolder(this.sound);
            this.element = ChipBase.ELEMENT.earth;
            switch (this.version)
            {
                case 1:
                    this.name = ShanghaiEXE.Translate("Enemy.BeetleManName1");
                    this.power = 60;
                    this.hp = 1600;
                    this.nspeed = 3;
                    this.moveroop = 1;
                    break;
                case 2:
                    this.name = ShanghaiEXE.Translate("Enemy.BeetleManName2");
                    this.power = 90;
                    this.hp = 1800;
                    this.nspeed = 3;
                    this.moveroop = 1;
                    break;
                case 3:
                    this.name = ShanghaiEXE.Translate("Enemy.BeetleManName3");
                    this.power = 110;
                    this.hp = 2000;
                    this.nspeed = 2;
                    this.moveroop = 2;
                    break;
                case 4:
                    this.nspeed = 2;
                    this.name = ShanghaiEXE.Translate("Enemy.BeetleManName4");
                    this.power = 200;
                    this.hp = 2200;
                    this.moveroop = 4;
                    break;
                default:
                    this.nspeed = 2;
                    this.name = ShanghaiEXE.Translate("Enemy.BeetleManName5") + (version - 3).ToString();
                    this.power = 200;
                    this.hp = 2500 + (version - 4) * 500;
                    this.moveroop = 4;
                    break;
            }
            this.picturename = "beetleman";
            this.superArmor = true;
            this.race = EnemyBase.ENEMY.navi;
            this.Flying = false;
            this.wide = 112;
            this.height = 112;
            this.hpmax = this.hp;
            this.speed = this.nspeed;
            this.hpprint = this.hp;
            this.printhp = true;
            this.effecting = false;
            this.Flying = false;
            this.roopmove = n * 3;
            this.PositionDirectSet();
            switch (this.version)
            {
                case 1:
                    this.dropchips[0].chip = new BeatleManV1(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new BeatleManV1(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new BeatleManV1(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new BeatleManV1(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new BeatleManV1(this.sound);
                    this.dropchips[4].codeNo = 0;
                    this.havezenny = 0;
                    break;
                case 2:
                    this.dropchips[0].chip = new BeatleManV2(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new BeatleManV2(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new BeatleManV2(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new BeatleManV2(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new BeatleManV2(this.sound);
                    this.dropchips[4].codeNo = 0;
                    this.havezenny = 0;
                    break;
                case 3:
                    this.dropchips[0].chip = new BeatleManV1(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new BeatleManV1(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new BeatleManV2(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new BeatleManV2(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new BeatleManV3(this.sound);
                    this.dropchips[4].codeNo = 0;
                    this.havezenny = 800;
                    break;
                default:
                    this.dropchips[0].chip = new BeatleManV1(this.sound);
                    this.dropchips[0].codeNo = 1;
                    this.dropchips[1].chip = new BeatleManV2(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new BeatleManV2(this.sound);
                    this.dropchips[2].codeNo = 1;
                    this.dropchips[3].chip = new BeatleManV1(this.sound);
                    this.dropchips[3].codeNo = 2;
                    this.dropchips[4].chip = new BeatleManV3(this.sound);
                    this.dropchips[4].codeNo = 1;
                    this.havezenny = 9000;
                    if (this.version < 8)
                        break;
                    this.dropchips[4].chip = new BeatleManX(this.sound);
                    this.dropchips[4].codeNo = this.Random.Next(4);
                    this.havezenny = 18000;
                    break;
            }
        }

        public override void PositionDirectSet()
        {
            this.positionDirect = new Vector2(position.X * 40f, (float)(position.Y * 24.0 + 48.0));
        }

        public override void InitAfter()
        {
            if (this.parent == null)
                return;
            this.shadow = new Shadow(this.sound, this.parent, this.position.X, this.position.Y, this);
            this.shadow.slide.X = this.union == Panel.COLOR.red ? -8 : 0;
            this.parent.effects.Add(shadow);
            this.shadow.PositionDirectSet(this.position);
        }

        public override void Updata()
        {
            if (this.slipping && (this.neutlal || this.knockslip))
                this.Slip(this.height);
            else if (!this.badstatus[3])
            {
                if (!this.bugholesetEnd && this.Hp <= this.HpMax / 2 && (!this.parent.blackOut || this.blackOutObject))
                    this.GodMode();
                else if (!this.neutlal || !this.badstatus[7] || this.badstatus[6])
                    this.Moving();
            }
            if (this.union == Panel.COLOR.red && this.parent.turn == this.samontarn + 3)
            {
                this.parent.effects.Add(new MoveEnemy(this.sound, this.parent, this.positionDirect, this.position));
                this.sound.PlaySE(SoundEffect.enterenemy);
                this.flag = false;
            }
            if (this.alfha < byte.MaxValue)
                this.alfha += 15;
            this.BaseUpdata();
        }

        protected override void Moving()
        {
            if (this.shadow != null)
                this.shadow.positionDirect = new Vector2(this.positionDirect.X + 16f, (float)(positionDirect.Y + (double)(this.Height / 2) - 16.0));
            this.neutlal = this.Motion == NaviBase.MOTION.neutral;
            switch (this.Motion)
            {
                case NaviBase.MOTION.neutral:
                    if (this.moveflame)
                    {
                        this.speed = this.nspeed;
                        ++this.waittime;
                    }
                    if (this.moveflame)
                    {
                        this.animationpoint = this.AnimeNeutral(this.waittime);
                        if (this.waittime >= 16 / (version / 2 + 1) || this.atack)
                        {
                            this.waittime = 0;
                            ++this.roopneutral;
                            if (this.roopneutral >= 1 && this.parent.nowscene != SceneBattle.BATTLESCENE.end)
                            {
                                this.roopneutral = 0;
                                if (this.roopmove > this.moveroop && !this.badstatus[4])
                                {
                                    this.roopmove = 0;
                                    ++this.atackroop;
                                    this.speed = this.nspeed * 2;
                                    if (!this.atack)
                                    {
                                        int index = 0;
                                        if (this.atackroop > (this.version < 4 ? 2 : 1))
                                        {
                                            this.atackroop = 0;
                                            index = this.Random.Next(2) + 1;
                                        }
                                        this.attack = (BeetleMan.ATTACK)index;
                                        this.powerPlus = this.powers[index];
                                    }
                                    this.chargeCount = 0;
                                    this.waittime = 0;
                                    this.Motion = NaviBase.MOTION.attack;
                                    if (this.bugholesetEnd && this.bughole != null && !this.bughole.flag)
                                        ++this.bugnonevcount;
                                }
                                else
                                {
                                    if (this.bugholesetEnd && this.bugnonevcount > 4)
                                    {
                                        this.bugnonevcount = 0;
                                        this.bugholesetEnd = false;
                                        this.bugholeset = false;
                                    }
                                    this.Throw();
                                }
                            }
                        }
                        break;
                    }
                    break;
                case NaviBase.MOTION.attack:
                    if (this.moveflame)
                    {
                        ++this.waittime;
                        if (this.moveflame)
                        {
                            switch (this.attack)
                            {
                                case BeetleMan.ATTACK.GraviBoll:
                                    this.bright = false;
                                    this.animationpoint = this.AnimeBall(this.waittime);
                                    switch (this.waittime)
                                    {
                                        case 4:
                                            this.counterTiming = true;
                                            break;
                                        case 7:
                                            this.counterTiming = false;
                                            this.parent.attacks.Add(new GravityBallAttack(this.sound, this.parent, this.position.X + this.UnionRebirth(this.union), this.position.Y, this.union, this.Power, 2, new Vector2((this.position.X + this.UnionRebirth(this.union)) * 40 + 20, this.position.Y * 24 + 70), this.element));
                                            break;
                                        case 8:
                                            this.frame = 0;
                                            this.waittime = -15;
                                            this.motion = NaviBase.MOTION.neutral;
                                            break;
                                    }
                                    break;
                                case BeetleMan.ATTACK.PowerWave:
                                    this.animationpoint = this.AnimeMedusa(this.waittime);
                                    switch (this.waittime)
                                    {
                                        case 1:
                                            this.sound.PlaySE(SoundEffect.charge);
                                            break;
                                        case 10:
                                            this.counterTiming = true;
                                            break;
                                        case 13:
                                            this.counterTiming = false;
                                            int[] numArray = new int[3];
                                            for (int index = 0; index < numArray.Length; ++index)
                                                numArray[index] = index != this.position.Y ? Math.Max(4, 4 + (3 - version / 2)) : Math.Max(8, 8 + (6 - version));
                                            this.parent.attacks.Add(new BombWave(this.sound, this.parent, this.position.X + this.UnionRebirth(this.union), 0, this.union, this.Power, numArray[0], this.element));
                                            this.parent.attacks.Add(new BombWave(this.sound, this.parent, this.position.X + this.UnionRebirth(this.union), 1, this.union, this.Power, numArray[1], this.element));
                                            this.parent.attacks.Add(new BombWave(this.sound, this.parent, this.position.X + this.UnionRebirth(this.union), 2, this.union, this.Power, numArray[2], this.element));
                                            break;
                                        case 14:
                                            this.frame = 0;
                                            this.waittime = -30;
                                            this.motion = NaviBase.MOTION.neutral;
                                            break;
                                    }
                                    break;
                                case BeetleMan.ATTACK.BeatleCharge:
                                    if (this.chargeCount == 0)
                                    {
                                        switch (this.waittime)
                                        {
                                            case 1:
                                                this.sound.PlaySE(SoundEffect.charge);
                                                this.animationpoint.X = 7;
                                                break;
                                            case 15:
                                                this.counterTiming = true;
                                                break;
                                            case 20:
                                                this.counterTiming = false;
                                                this.HitFlagReset();
                                                this.effecting = true;
                                                this.chargeCount = 2 + version;
                                                this.sound.PlaySE(SoundEffect.shoot);
                                                break;
                                        }
                                    }
                                    break;
                            }
                        }
                    }
                    if (this.attack == BeetleMan.ATTACK.BeatleCharge && this.chargeCount > 0)
                    {
                        this.animationpoint.X = 3;
                        this.AttackMake(this.Power, 0, 0);
                        this.positionDirect.X += Math.Min(6 + version, 12) * this.UnionRebirth(this.union);
                        this.position.X = this.Calcposition(this.positionDirect, this.height, false).X;
                        if (positionDirect.X < 0.0 || positionDirect.X > 240.0)
                        {
                            --this.chargeCount;
                            if (this.chargeCount <= 0)
                            {
                                this.effecting = false;
                                this.Throw();
                                this.motion = NaviBase.MOTION.move;
                                this.animationpoint = new Point();
                                this.roopneutral = 0;
                                this.frame = 0;
                                this.waittime = 0;
                                if (!this.atack)
                                    this.speed = this.nspeed;
                            }
                            else
                            {
                                this.sound.PlaySE(SoundEffect.shoot);
                                this.HitFlagReset();
                                this.animationpoint.X = 0;
                                this.parent.effects.Add(new MoveEnemy(this.sound, this.parent, this.position.X, this.position.Y));
                                this.positionre = new Point(this.union == Panel.COLOR.blue ? 5 : 0, this.RandomTarget().Y);
                                this.position = this.positionre;
                                this.PositionDirectSet();
                            }
                        }
                        break;
                    }
                    break;
                case NaviBase.MOTION.move:
                    this.animationpoint.X = 9;
                    if (this.moveflame)
                        ++this.waittime;
                    if (this.waittime > 2)
                    {
                        if (this.flyflame == this.time)
                        {
                            this.animationpoint.X = 9;
                            this.animationpoint.X = 0;
                            this.sound.PlaySE(SoundEffect.canon);
                            this.ShakeStart(2, 10);
                            for (int index = 0; index < (this.version < 4 ? 2 : 1); ++index)
                            {
                                Point point = this.RandomPanel(this.UnionEnemy);
                                this.parent.attacks.Add(new FallStone(this.sound, this.parent, point.X, point.Y, this.union, this.Power, 1, 20 * index, ChipBase.ELEMENT.normal));
                            }
                            this.position = this.positionre;
                            this.motion = NaviBase.MOTION.neutral;
                            this.plusy = 0.0f;
                            this.frame = 0;
                            this.waittime = 0;
                        }
                        else
                        {
                            this.animationpoint.X = 10;
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
                case NaviBase.MOTION.knockback:
                    switch (this.waittime)
                    {
                        case 2:
                            this.NockMotion();
                            this.counterTiming = false;
                            this.effecting = false;
                            this.PositionDirectSet();
                            break;
                        case 3:
                            this.NockMotion();
                            break;
                        case 15:
                            this.animationpoint = new Point(10, 0);
                            this.PositionDirectSet();
                            break;
                        case 21:
                            this.animationpoint = new Point(0, 0);
                            this.waittime = 0;
                            this.Motion = NaviBase.MOTION.neutral;
                            break;
                    }
                    if (this.waittime >= 2 && this.waittime <= 6)
                        this.positionDirect.X -= this.UnionRebirth(this.union);
                    ++this.waittime;
                    break;
            }
            this.FlameControl();
            this.MoveAftar();
            ++this.brigt;
            if (this.brigt % 2 == 0)
            {
                this.brigt = 0;
                this.bright = !this.bright;
            }
            if (this.animationpoint.X == 3 || this.animationpoint.X == 4)
                this.animationpoint.X = this.bright ? 4 : 3;
            if (this.animationpoint.X == 7 || this.animationpoint.X == 8)
                this.animationpoint.X = this.bright ? 8 : 7;
            if (this.animationpoint.X != 11 && this.animationpoint.X != 12)
                return;
            this.animationpoint.X = this.bright ? 12 : 11;
        }

        public override void NockMotion()
        {
            this.animationpoint = new Point(1, 0);
        }

        public override void RenderUP(IRenderer dg)
        {
            this.BlackOutRender(dg, this.union);
        }

        public override void Render(IRenderer dg)
        {
            if (this.alfha < byte.MaxValue)
                this.color = Color.FromArgb(alfha, this.mastorcolor);
            else
                this.color = this.mastorcolor;
            double num1 = (int)this.positionDirect.X + this.Shake.X;
            int y1 = (int)this.positionDirect.Y;
            Point shake = this.Shake;
            int y2 = shake.Y;
            double num2 = y1 + y2 + (double)this.plusy;
            this._position = new Vector2((float)num1, (float)num2);
            this._rect = new Rectangle(this.animationpoint.X * this.wide, (this.version < 4 ? 0 : 2) * this.height, this.wide, this.height);
            if (this.Hp <= 0)
            {
                this.rd = this._rect;
                this.NockMotion();
                int num3 = this.animationpoint.X * this.wide;
                shake = this.Shake;
                int x1 = shake.X;
                int x2 = num3 + x1;
                int y3 = (this.version < 4 ? 0 : 2) * this.height;
                int wide = this.wide;
                int height1 = this.height;
                shake = this.Shake;
                int y4 = shake.Y;
                int height2 = height1 + y4;
                this._rect = new Rectangle(x2, y3, wide, height2);
                this.Death(this._rect, new Rectangle(this.animationpoint.X * this.wide, this.height, this.wide, this.height), this._position, this.picturename);
            }
            if (this.whitetime == 0)
            {
                dg.DrawImage(dg, this.picturename, this._rect, false, this._position, this.rebirth, this.color);
            }
            else
            {
                this._rect.Y = this.height;
                dg.DrawImage(dg, this.picturename, this._rect, false, this._position, this.rebirth, this.color);
            }
            this.HPposition = new Vector2((int)this.positionDirect.X + 24, (int)this.positionDirect.Y - 8 + this.height / 2 + 8);
            this.Nameprint(dg, this.printNumber);
        }

        private void GodMode()
        {
            if (!this.bugholeset)
            {
                this.nohit = true;
                this.bugholeset = true;
            }
            if (!this.BlackOut(
                this,
                this.parent,
                ShanghaiEXE.Translate("Enemy.BeetleManSpecial"),
                ""))
                return;
            switch (this.waittime)
            {
                case 1:
                    this.sound.PlaySE(SoundEffect.dark);
                    this.bughole = new BugHole(this.sound, this.parent, this.union == Panel.COLOR.blue ? 3 : 2, this.position.Y, this.union);
                    this.parent.objects.Add(bughole);
                    break;
            }
            if (this.waittime > 30 && this.BlackOutEnd(this, this.parent))
            {
                this.bugholesetEnd = true;
                this.waittime = 0;
            }
            else
                ++this.waittime;
        }

        private Point AnimeNeutral(int waitflame)
        {
            return this.Return(new int[1], new int[1], 1, waitflame);
        }

        private Point AnimeMove(int waitflame)
        {
            return this.ReturnKai(new int[5]
            {
        1,
        1,
        this.time,
        1,
        1
            }, new int[5] { 0, 9, 10, 9, 0 }, 1, waitflame);
        }

        private Point AnimeBall(int waitflame)
        {
            return this.ReturnKai(new int[6] { 1, 1, 3, 1, 1, 1 }, new int[6]
            {
        0,
        9,
        11,
        5,
        6,
        0
            }, 1, waitflame);
        }

        private Point AnimeMedusa(int waitflame)
        {
            return this.ReturnKai(new int[6] { 1, 10, 1, 1, 1, 1 }, new int[5]
            {
        0,
        11,
        5,
        6,
        0
            }, 1, waitflame);
        }

        private void Throw()
        {
            this.animationpoint.X = 9;
            this.waittime = 0;
            this.time = 30;
            this.flyflame = 0;
            ++this.roopmove;
            this.Motion = NaviBase.MOTION.move;
            this.powerPlus = this.powers[3];
            Point position = this.position;
            this.Noslip = true;
            this.MoveRandom(false, false);
            this.endposition = new Vector2(positionre.X * 40f, (float)(positionre.Y * 24.0 + 48.0));
            this.movex = (this.positionDirect.X - this.endposition.X) / time;
            this.movey = (this.positionDirect.Y - this.endposition.Y) / time;
            this.plusy = 0.0f;
            this.speedy = 6f;
            this.plusing = this.speedy / (this.time / 2);
            this.flyflame = 0;
        }

        private enum ATTACK
        {
            GraviBoll,
            PowerWave,
            BeatleCharge,
            MoveHop,
        }
    }
}

