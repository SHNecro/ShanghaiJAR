using NSAttack;
using NSBattle;
using NSBattle.Character;
using NSChip;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEffect;
using NSGame;
using NSObject;
using Common.Vectors;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace NSEnemy
{
    internal class Ran : NaviBase
    {
        private readonly int[] pattern = new int[11]
        {
      0,
      0,
      0,
      2,
      0,
      0,
      1,
      0,
      3,
      0,
      1
        };
        private readonly int[] pattern2 = new int[11]
        {
      1,
      1,
      2,
      1,
      0,
      1,
      1,
      2,
      2,
      1,
      3
        };
        private readonly int[] powers = new int[4] { 80, 50, 30, 50 };
        private readonly int nspeed = 2;
        private int aspeed = 4;
        private readonly Point[] target = new Point[5];
        private List<Point> targetMulti = new List<Point>();
        private readonly List<RanBarrier> barriers = new List<RanBarrier>();
        private int action;
        private readonly int attackSpeed;
        private bool ready;
        private bool end;
        private int attackCount;
        private int attackProcess;
        private readonly int moveroop;
        private Ran.ATTACK attack;
        private int roopneutral;
        private int roopmove;
        private Charge chargeEffect;
        private Chen chen;
        private int attackroop;
        private int wait;
        private readonly int waveX;
        private readonly int waveY;

        private int Action
        {
            get
            {
                return this.action;
            }
            set
            {
                this.action = value;
                if (this.action < this.pattern.Length)
                    return;
                this.action = 0;
            }
        }

        public Ran(IAudioEngine s, SceneBattle p, int pX, int pY, byte n, Panel.COLOR u, byte v)
          : base(s, p, pX, pY, n, u, v)
        {
            for (int index = 0; index < this.dropchips.Length; ++index)
                this.dropchips[index] = new ChipFolder(this.sound);
            this.element = ChipBase.ELEMENT.normal;
            switch (this.version)
            {
                case 1:
                    this.name = ShanghaiEXE.Translate("Enemy.RanName1");
                    this.power = 0;
                    this.hp = 1500;
                    this.moveroop = 4;
                    this.nspeed = 5;
                    this.aspeed = 3;
                    this.attackSpeed = 8;
                    break;
                case 2:
                    this.name = ShanghaiEXE.Translate("Enemy.RanName2");
                    this.power = 30;
                    this.hp = 1800;
                    this.moveroop = 4;
                    this.nspeed = 3;
                    this.aspeed = 3;
                    this.attackSpeed = 8;
                    break;
                case 3:
                    this.name = ShanghaiEXE.Translate("Enemy.RanName3");
                    this.power = 50;
                    this.hp = 2100;
                    this.moveroop = 7;
                    this.nspeed = 3;
                    this.aspeed = 2;
                    this.attackSpeed = 9;
                    break;
                case 4:
                    this.name = ShanghaiEXE.Translate("Enemy.RanName4");
                    this.power = 70;
                    this.hp = 2400;
                    this.moveroop = 8;
                    this.nspeed = 3;
                    this.aspeed = 2;
                    this.attackSpeed = 10;
                    break;
                default:
                    this.name = ShanghaiEXE.Translate("Enemy.RanName5") + (version - 3).ToString();
                    this.power = 100;
                    this.hp = 2500 + (version - 4) * 500;
                    this.moveroop = 8;
                    this.nspeed = 3;
                    this.aspeed = 2;
                    this.attackSpeed = 10;
                    break;
            }
            this.speed = this.nspeed;
            this.picturename = "ran";
            this.race = EnemyBase.ENEMY.navi;
            this.wide = 128;
            this.height = 96;
            this.hpmax = this.hp;
            this.hpprint = this.hp;
            this.printhp = true;
            this.effecting = false;
            this.Flying = false;
            this.roopmove = 0;
            this.PositionDirectSet();
            switch (this.version)
            {
                case 1:
                    this.dropchips[0].chip = new ChenV1(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new ChenV1(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new ChenV1(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new ChenV1(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new ChenV1(this.sound);
                    this.dropchips[4].codeNo = 0;
                    this.havezenny = 0;
                    break;
                case 2:
                    this.dropchips[0].chip = new RanV2(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new RanV2(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new RanV2(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new RanV2(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new RanV2(this.sound);
                    this.dropchips[4].codeNo = 0;
                    this.havezenny = 0;
                    break;
                case 3:
                    this.dropchips[0].chip = new RanV1(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new RanV1(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new RanV2(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new RanV2(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new RanV3(this.sound);
                    this.dropchips[4].codeNo = 0;
                    this.havezenny = 12000;
                    break;
                default:
                    this.dropchips[0].chip = new RanV1(this.sound);
                    this.dropchips[0].codeNo = 1;
                    this.dropchips[1].chip = new RanV2(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new RanV2(this.sound);
                    this.dropchips[2].codeNo = 1;
                    this.dropchips[3].chip = new RanV1(this.sound);
                    this.dropchips[3].codeNo = 2;
                    this.dropchips[4].chip = new RanV3(this.sound);
                    this.dropchips[4].codeNo = 1;
                    this.havezenny = 15000;
                    if (this.version >= 8)
                    {
                        this.dropchips[4].chip = new RanX(this.sound);
                        this.dropchips[4].codeNo = this.Random.Next(4);
                        this.havezenny = 18000;
                        break;
                    }
                    break;
            }
            this.motion = NaviBase.MOTION.move;
        }

        public override void PositionDirectSet()
        {
            this.positionDirect = new Vector2((float)(position.X * 40.0 + 8.0), (float)(position.Y * 24.0 + 44.0));
        }

        public override void InitAfter()
        {
            for (int index = 0; index < this.Parent.enemys.Count; ++index)
            {
                if (index != this.number && this.Parent.enemys[index] is Chen)
                {
                    this.chen = (Chen)this.Parent.enemys[index];
                    break;
                }
            }
        }

        protected override void Moving()
        {
            this.neutlal = this.Motion == NaviBase.MOTION.neutral;
            switch (this.Motion)
            {
                case NaviBase.MOTION.neutral:
                    if (this.moveflame)
                        ++this.waittime;
                    if (this.moveflame && this.waittime >= 10 / Math.Max(2, Math.Min((int)this.version, 4)) + this.wait)
                    {
                        this.wait = 0;
                        this.waittime = 0;
                        ++this.roopneutral;
                        this.animationpoint = this.AnimeNeutral(this.waittime);
                        if (this.roopneutral >= 1 && this.parent.nowscene != SceneBattle.BATTLESCENE.end)
                        {
                            this.roopneutral = 0;
                            if (this.roopmove > this.moveroop && !this.badstatus[4])
                            {
                                this.roopmove = this.Random.Next(this.moveroop - 2);
                                this.speed = 1;
                                ++this.attackroop;
                                this.waittime = 0;
                                this.ready = false;
                                this.attack = (Ran.ATTACK)this.pattern[this.action];
                                if (this.chen != null && (this.chen.flag && this.attack >= Ran.ATTACK.SpreadCanon))
                                    this.attack = Ran.ATTACK.TaleBit;
                                this.powerPlus = this.powers[this.pattern[this.action]];
                                ++this.action;
                                if (this.action >= this.pattern.Length)
                                    this.action = 0;
                                this.attackProcess = 0;
                                this.ready = false;
                                this.end = false;
                                this.Motion = NaviBase.MOTION.attack;
                            }
                            else
                            {
                                this.waittime = 0;
                                ++this.roopmove;
                                this.Motion = NaviBase.MOTION.move;
                            }
                        }
                        break;
                    }
                    break;
                case NaviBase.MOTION.attack:
                    switch (this.attack)
                    {
                        case Ran.ATTACK.SingleShot:
                            this.animationpoint = this.AnimeSingleShot(this.waittime);
                            switch (this.waittime)
                            {
                                case 4:
                                    this.counterTiming = true;
                                    break;
                                case 8:
                                    this.counterTiming = false;
                                    this.sound.PlaySE(SoundEffect.gun);
                                    BustorShot bustorShot1 = new BustorShot(this.sound, this.parent, this.position.X + this.UnionRebirth(this.union), this.position.Y, this.union, this.Power, BustorShot.SHOT.ranShot, this.element, false, 6)
                                    {
                                        blackOutObject = false
                                    };
                                    this.parent.attacks.Add(bustorShot1);
                                    break;
                                case 16:
                                    this.roopneutral = 0;
                                    this.Motion = NaviBase.MOTION.neutral;
                                    break;
                            }
                            ++this.waittime;
                            break;
                        case Ran.ATTACK.TaleBit:
                            int time = 30;
                            if (!this.ready)
                            {
                                this.animationpoint = this.AnimeTaleBit(this.waittime);
                                switch (this.waittime / this.aspeed)
                                {
                                    case 1:
                                        if (this.Canmove(new Point(this.union == Panel.COLOR.blue ? 4 : 1, 1), this.number, this.union))
                                        {
                                            this.parent.effects.Add(new MoveEnemy(this.sound, this.parent, this.position.X, this.position.Y));
                                            this.positionre = new Point(this.union == Panel.COLOR.blue ? 4 : 1, 1);
                                            this.position = this.positionre;
                                            this.PositionDirectSet();
                                            if (this.version > 1)
                                            {
                                                this.target[0] = new Point(this.union == Panel.COLOR.blue ? 1 : 4, 1);
                                                this.parent.attacks.Add(new Dummy(this.sound, this.parent, this.target[0].X, this.target[0].Y, this.union, new Point(), time, true));
                                            }
                                            this.target[1] = new Point(this.position.X + this.UnionRebirth(this.union), this.position.Y);
                                            this.parent.attacks.Add(new Dummy(this.sound, this.parent, this.target[1].X, this.target[1].Y, this.union, new Point(), time, true));
                                            break;
                                        }
                                        this.attackroop = 0;
                                        this.MoveRandom(false, false);
                                        this.roopneutral = 0;
                                        this.waittime = 0;
                                        this.Motion = NaviBase.MOTION.neutral;
                                        this.speed = this.nspeed;
                                        break;
                                    case 8:
                                        this.counterTiming = true;
                                        break;
                                    case 12:
                                        this.counterTiming = false;
                                        this.ready = true;
                                        if (this.version > 1)
                                        {
                                            var newV2Barrier = new RanBarrier(this.sound, this.parent, this.target[0].X, this.target[0].Y, this.Power, 2, this.union, -1, this.version >= 4);
                                            this.barriers.Add(newV2Barrier);
                                            this.parent.objects.Add(newV2Barrier);
                                        }
                                        var newBarrier = new RanBarrier(this.sound, this.parent, this.target[1].X, this.target[1].Y, this.Power, 2, this.union, -1, this.version >= 4);
                                        this.barriers.Add(newBarrier);
                                        this.parent.objects.Add(newBarrier);
                                        this.target[2] = new Point(this.union == Panel.COLOR.blue ? 0 : 5, 0);
                                        this.parent.attacks.Add(new Dummy(this.sound, this.parent, this.target[2].X, this.target[2].Y, this.union, new Point(), time, true));
                                        if (this.version >= 2)
                                        {
                                            this.target[1] = new Point(this.union == Panel.COLOR.blue ? 0 : 5, 0);
                                            this.parent.attacks.Add(new Dummy(this.sound, this.parent, this.target[1].X, this.target[1].Y, this.union, new Point(), time, true));
                                        }
                                        this.attackProcess = 0;
                                        this.waittime = 0;
                                        break;
                                }
                            }
                            else
                            {
                                int num = this.waittime / this.aspeed;
                                if (this.waittime == this.aspeed * 8)
                                {
                                    var newBarrier = new RanBarrier(this.sound, this.parent, this.target[2].X, this.target[2].Y, this.Power, 2, this.union, 3, this.version >= 4);
                                    this.barriers.Add(newBarrier);
                                    this.parent.objects.Add(newBarrier);
                                    if (this.version >= 2)
                                    {
                                        var newV2Barrier = new RanBarrier(this.sound, this.parent, this.target[1].X, this.target[1].Y, this.Power, 2, this.union, 3, this.version >= 4);
                                        this.barriers.Add(newV2Barrier);
                                        this.parent.objects.Add(newV2Barrier);
                                    }
                                    switch (this.attackProcess % 8)
                                    {
                                        case 0:
                                            this.target[2] = new Point(this.union == Panel.COLOR.blue ? 0 : 5, 1);
                                            if (this.version >= 2)
                                            {
                                                this.target[1] = new Point(this.union == Panel.COLOR.blue ? 2 : 3, 1);
                                                break;
                                            }
                                            break;
                                        case 1:
                                            this.target[2] = new Point(this.union == Panel.COLOR.blue ? 0 : 5, 2);
                                            if (this.version >= 2)
                                            {
                                                this.target[1] = new Point(this.union == Panel.COLOR.blue ? 2 : 3, 0);
                                                break;
                                            }
                                            break;
                                        case 2:
                                            this.target[2] = new Point(this.union == Panel.COLOR.blue ? 1 : 4, 2);
                                            if (this.version >= 2)
                                            {
                                                this.target[1] = new Point(this.union == Panel.COLOR.blue ? 1 : 4, 0);
                                                break;
                                            }
                                            break;
                                        case 3:
                                            this.target[2] = new Point(this.union == Panel.COLOR.blue ? 2 : 3, 2);
                                            if (this.version >= 2)
                                            {
                                                this.target[1] = new Point(this.union == Panel.COLOR.blue ? 0 : 5, 0);
                                                break;
                                            }
                                            break;
                                        case 4:
                                            this.target[2] = new Point(this.union == Panel.COLOR.blue ? 2 : 3, 1);
                                            if (this.version >= 2)
                                            {
                                                this.target[1] = new Point(this.union == Panel.COLOR.blue ? 0 : 5, 1);
                                                break;
                                            }
                                            break;
                                        case 5:
                                            this.target[2] = new Point(this.union == Panel.COLOR.blue ? 2 : 3, 0);
                                            if (this.version >= 2)
                                            {
                                                this.target[1] = new Point(this.union == Panel.COLOR.blue ? 0 : 5, 2);
                                                break;
                                            }
                                            break;
                                        case 6:
                                            this.target[2] = new Point(this.union == Panel.COLOR.blue ? 1 : 4, 0);
                                            if (this.version >= 2)
                                            {
                                                this.target[1] = new Point(this.union == Panel.COLOR.blue ? 1 : 4, 2);
                                                break;
                                            }
                                            break;
                                        case 7:
                                            this.target[2] = new Point(this.union == Panel.COLOR.blue ? 0 : 5, 0);
                                            if (this.version >= 2)
                                                this.target[1] = new Point(this.union == Panel.COLOR.blue ? 2 : 3, 2);
                                            if (this.attackProcess / 7 >= 2)
                                            {
                                                this.roopneutral = 0;
                                                this.waittime = 0;
                                                this.Motion = NaviBase.MOTION.neutral;
                                                foreach (var barrier in this.barriers)
                                                {
                                                    barrier?.Break();
                                                }
                                                this.barriers.Clear();
                                                break;
                                            }
                                            break;
                                    }
                                    ++this.attackProcess;
                                    this.waittime = 0;
                                    this.parent.attacks.Add(new Dummy(this.sound, this.parent, this.target[2].X, this.target[2].Y, this.union, new Point(), time, true));
                                    if (this.version >= 2)
                                    {
                                        this.parent.attacks.Add(new Dummy(this.sound, this.parent, this.target[1].X, this.target[1].Y, this.union, new Point(), time, true));
                                        break;
                                    }
                                    break;
                                }
                            }
                            ++this.waittime;
                            break;
                        case Ran.ATTACK.SpreadCanon:
                            if (!this.ready)
                            {
                                if (this.waittime % this.aspeed == 0)
                                {
                                    this.animationpoint = this.AnimeSpread1(this.waittime);
                                    switch (this.waittime / this.aspeed)
                                    {
                                        case 1:
                                            this.targetMulti = ((IEnumerable<Point>)this.RandomMultiPanel(Math.Min(5, version + 2), this.UnionEnemy)).ToList<Point>();
                                            for (int index = 0; index < this.targetMulti.Count; ++index)
                                            {
                                                List<AttackBase> attacks = this.parent.attacks;
                                                IAudioEngine sound = this.sound;
                                                SceneBattle parent = this.parent;
                                                int x = this.targetMulti[index].X;
                                                Point point = this.targetMulti[index];
                                                int y = point.Y;
                                                int union = (int)this.union;
                                                point = new Point();
                                                Point hitrange = point;
                                                Dummy dummy = new Dummy(sound, parent, x, y, (Panel.COLOR)union, hitrange, 30, true);
                                                attacks.Add(dummy);
                                            }
                                            break;
                                        case 12:
                                            this.aspeed = 3;
                                            this.attackProcess = 0;
                                            this.waittime = 0;
                                            this.ready = true;
                                            break;
                                    }
                                }
                            }
                            else if (this.waittime % this.aspeed == 0)
                            {
                                this.animationpoint = this.AnimeSpread2(this.waittime);
                                switch (this.waittime / this.aspeed)
                                {
                                    case 2:
                                        this.sound.PlaySE(SoundEffect.gun);
                                        for (int index = 0; index < this.targetMulti.Count; ++index)
                                        {
                                            this.parent.effects.Add(new GunHit(this.sound, this.parent, this.targetMulti[index].X, this.targetMulti[index].Y, this.union));
                                            this.parent.attacks.Add(new BombAttack(this.sound, this.parent, this.targetMulti[index].X, this.targetMulti[index].Y, this.union, this.Power, 0, this.element));
                                        }
                                        if (this.attackProcess < version + 1)
                                        {
                                            this.targetMulti = ((IEnumerable<Point>)this.RandomMultiPanel(Math.Min(5, version + 2), this.UnionEnemy)).ToList<Point>();
                                            for (int index = 0; index < this.targetMulti.Count; ++index)
                                            {
                                                List<AttackBase> attacks = this.parent.attacks;
                                                IAudioEngine sound = this.sound;
                                                SceneBattle parent = this.parent;
                                                int x = this.targetMulti[index].X;
                                                Point point = this.targetMulti[index];
                                                int y = point.Y;
                                                int union = (int)this.union;
                                                point = new Point();
                                                Point hitrange = point;
                                                Dummy dummy = new Dummy(sound, parent, x, y, (Panel.COLOR)union, hitrange, 30, true);
                                                attacks.Add(dummy);
                                            }
                                            break;
                                        }
                                        break;
                                    case 13:
                                        ++this.attackProcess;
                                        this.waittime = 0;
                                        if (this.attackProcess >= version + 2)
                                        {
                                            this.roopneutral = 0;
                                            this.waittime = 0;
                                            this.Motion = NaviBase.MOTION.neutral;
                                            break;
                                        }
                                        break;
                                }
                            }
                            ++this.waittime;
                            break;
                        case Ran.ATTACK.MachineGunRey:
                            if (!this.ready)
                            {
                                if (this.waittime % this.aspeed == 0)
                                {
                                    this.animationpoint.X = 2;
                                    switch (this.waittime / this.aspeed)
                                    {
                                        case 1:
                                            if (this.Canmove(new Point(this.union == Panel.COLOR.blue ? 5 : 0, 1), this.number, this.union))
                                            {
                                                this.parent.effects.Add(new MoveEnemy(this.sound, this.parent, this.position.X, this.position.Y));
                                                this.positionre = new Point(this.union == Panel.COLOR.blue ? 5 : 0, 1);
                                                this.position = this.positionre;
                                                this.PositionDirectSet();
                                                this.chargeEffect = new Charge(this.sound, this.parent, this.position.X, this.position.Y);
                                                this.parent.effects.Add(chargeEffect);
                                                break;
                                            }
                                            this.attackroop = 0;
                                            this.MoveRandom(false, false);
                                            this.roopneutral = 0;
                                            this.waittime = 0;
                                            this.Motion = NaviBase.MOTION.neutral;
                                            this.speed = this.nspeed;
                                            break;
                                        case 12:
                                            this.chargeEffect.chargeEffect = 2;
                                            break;
                                        case 20:
                                            this.counterTiming = true;
                                            break;
                                        case 24:
                                            this.counterTiming = false;
                                            this.aspeed = 3;
                                            this.chargeEffect.flag = false;
                                            this.attackProcess = 0;
                                            this.waittime = 0;
                                            this.ready = true;
                                            break;
                                    }
                                }
                            }
                            else if (this.waittime % this.aspeed == 0)
                            {
                                this.animationpoint = this.AnimeMachinegunRay(this.waittime);
                                switch (this.waittime / this.aspeed)
                                {
                                    case 2:
                                        this.sound.PlaySE(SoundEffect.gun);
                                        int num = this.Random.Next(2);
                                        if (num == 1)
                                            num = 2;
                                        BustorShot bustorShot2 = new BustorShot(this.sound, this.parent, this.position.X + this.UnionRebirth(this.union), this.Random.Next(100) < 60 ? this.Random.Next(3) : num, this.union, this.Power, BustorShot.SHOT.ranShot, this.element, false, 6)
                                        {
                                            blackOutObject = false
                                        };
                                        this.parent.attacks.Add(bustorShot2);
                                        break;
                                    case 4:
                                        ++this.attackProcess;
                                        this.waittime = 0;
                                        if (this.attackProcess / 8 >= version / 2 + 1)
                                        {
                                            this.roopneutral = 0;
                                            this.waittime = 0;
                                            this.Motion = NaviBase.MOTION.neutral;
                                            break;
                                        }
                                        break;
                                }
                            }
                            ++this.waittime;
                            break;
                    }
                    break;
                case NaviBase.MOTION.move:
                    if (this.moveflame)
                        ++this.waittime;
                    ++this.roopmove;
                    this.Motion = NaviBase.MOTION.neutral;
                    this.MoveRandom(false, false);
                    this.speed = this.nspeed;
                    if (this.attackroop > 4)
                        this.attackroop = 0;
                    if (this.position == this.positionre)
                    {
                        this.frame = 0;
                        this.roopneutral = 0;
                        break;
                    }
                    this.parent.effects.Add(new MoveEnemy(this.sound, this.parent, this.position.X, this.position.Y));
                    this.position = this.positionre;
                    this.PositionDirectSet();
                    this.frame = 0;
                    this.roopneutral = 0;
                    break;
                case NaviBase.MOTION.knockback:
                    switch (this.waittime)
                    {
                        case 2:
                            this.rebirth = this.union == Panel.COLOR.red;
                            foreach (var barrier in this.barriers)
                            {
                                barrier?.Break();
                            }
                            this.barriers.Clear();
                            if (this.chargeEffect != null)
                                this.chargeEffect.flag = false;
                            this.guard = CharacterBase.GUARD.none;
                            this.ready = false;
                            this.Noslip = false;
                            this.attackCount = 0;
                            this.speed = this.nspeed;
                            this.effecting = false;
                            this.NockMotion();
                            this.counterTiming = false;
                            this.effecting = false;
                            this.PositionDirectSet();
                            break;
                        case 3:
                            this.NockMotion();
                            break;
                        case 15:
                            this.animationpoint = new Point(2, 0);
                            this.PositionDirectSet();
                            break;
                        case 21:
                            this.animationpoint = new Point(0, 0);
                            this.waittime = 0;
                            this.wait = 0;
                            this.Motion = NaviBase.MOTION.neutral;
                            break;
                    }
                    if (this.waittime >= 2 && this.waittime <= 6)
                        this.positionDirect.X -= this.UnionRebirth(this.union);
                    ++this.waittime;
                    break;
            }
            if (this.effecting)
                this.AttackMake(this.Power, 0, 0);
            this.FlameControl();
            this.MoveAftar();
        }

        public override void NockMotion()
        {
            this.animationpoint = new Point(1, 0);
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
            double num2 = y1 + y2;
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
                foreach (var barrier in this.barriers)
                {
                    barrier?.Break();
                }
                this.barriers.Clear();
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
            this.HPposition = new Vector2((int)this.positionDirect.X + 16, (int)this.positionDirect.Y - this.height / 2 + 94);
            this.Nameprint(dg, this.printNumber);
        }

        private Point AnimeNeutral(int waitflame)
        {
            return this.Return(new int[1], new int[1], 1, waitflame);
        }

        private Point AnimeSingleShot(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[6]
            {
        this.aspeed,
        this.aspeed,
        this.aspeed,
        this.aspeed,
        this.aspeed,
        100
            }, new int[6] { 3, 4, 5, 6, 7, 5 }, 0, waittime);
        }

        private Point AnimeTaleBit(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[12]
            {
        this.aspeed,
        this.aspeed,
        this.aspeed,
        this.aspeed,
        this.aspeed,
        this.aspeed,
        this.aspeed,
        this.aspeed,
        this.aspeed,
        this.aspeed,
        this.aspeed,
        100
            }, new int[12]
            {
        14,
        15,
        14,
        15,
        14,
        15,
        16,
        17,
        18,
        19,
        20,
        21
            }, 0, waittime);
        }

        private Point AnimeMachinegunRay(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[6]
            {
        this.aspeed * 3,
        this.aspeed,
        this.aspeed,
        this.aspeed,
        this.aspeed,
        100
            }, new int[4] { 5, 6, 7, 5 }, 0, waittime);
        }

        private Point AnimeSpread1(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[4]
            {
        this.aspeed,
        this.aspeed,
        this.aspeed,
        1000
            }, new int[4] { 8, 9, 10, 11 }, 0, waittime);
        }

        private Point AnimeSpread2(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[4]
            {
        this.aspeed,
        this.aspeed,
        this.aspeed,
        100
            }, new int[4] { 11, 12, 13, 11 }, 0, waittime);
        }

        private Point AnimeQuake(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[5]
            {
        this.aspeed,
        this.aspeed * 3,
        this.aspeed,
        this.aspeed,
        100
            }, new int[6] { 11, 12, 16, 17, 2, 0 }, 0, waittime);
        }

        private enum ATTACK
        {
            SingleShot,
            TaleBit,
            SpreadCanon,
            MachineGunRey,
        }
    }
}

