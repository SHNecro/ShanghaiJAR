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
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace NSEnemy
{
    internal class Sakuya : NaviBase
    {
        private readonly int[] powers = new int[6] { 0, 0, 40, 40, 0, 0 };
        private readonly int nspeed = 2;
        private readonly int moveroop;
        private Sakuya.ATTACK attack;
        private int roopneutral;
        private int roopmove;
        private int y;
        private DammyEnemy dammy;
        private int attackroop;
        private Shadow shadow;

        public Sakuya(IAudioEngine s, SceneBattle p, int pX, int pY, byte n, Panel.COLOR u, byte v)
          : base(s, p, pX, pY, n, u, v)
        {
            for (int index = 0; index < this.dropchips.Length; ++index)
                this.dropchips[index] = new ChipFolder(this.sound);
            this.element = ChipBase.ELEMENT.normal;
            switch (this.version)
            {
                case 1:
                    this.name = ShanghaiEXE.Translate("Enemy.SakuyaName1");
                    this.power = 40;
                    this.hp = 400;
                    this.moveroop = 2;
                    this.nspeed = 2;
                    break;
                case 2:
                    this.name = ShanghaiEXE.Translate("Enemy.SakuyaName2");
                    this.power = 80;
                    this.hp = 800;
                    this.moveroop = 2;
                    this.nspeed = 2;
                    break;
                case 3:
                    this.name = ShanghaiEXE.Translate("Enemy.SakuyaName3");
                    this.power = 100;
                    this.hp = 1100;
                    this.moveroop = 3;
                    this.nspeed = 1;
                    break;
                case 4:
                    this.name = ShanghaiEXE.Translate("Enemy.SakuyaName4");
                    this.power = 200;
                    this.hp = 1300;
                    this.moveroop = 3;
                    this.nspeed = 1;
                    break;
                default:
                    this.name = ShanghaiEXE.Translate("Enemy.SakuyaName5") + (version - 3).ToString();
                    this.power = 200;
                    this.hp = 1000 + (version - 4) * 500;
                    this.moveroop = 3;
                    this.nspeed = 1;
                    break;
            }
            this.speed = this.nspeed;
            this.picturename = "sakuya";
            this.race = EnemyBase.ENEMY.navi;
            this.wide = 48;
            this.height = 64;
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
                    this.dropchips[0].chip = new SakuyaV1(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new SakuyaV1(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new SakuyaV1(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new SakuyaV1(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new SakuyaV1(this.sound);
                    this.dropchips[4].codeNo = 0;
                    this.havezenny = 0;
                    break;
                case 2:
                    this.dropchips[0].chip = new SakuyaV2(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new SakuyaV2(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new SakuyaV2(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new SakuyaV2(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new SakuyaV2(this.sound);
                    this.dropchips[4].codeNo = 0;
                    this.havezenny = 0;
                    break;
                case 3:
                    this.dropchips[0].chip = new SakuyaV1(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new SakuyaV1(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new SakuyaV2(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new SakuyaV2(this.sound);
                    this.dropchips[3].codeNo = 0;
                    if (this.Random.Next(4) < 3)
                    {
                        this.dropchips[4].chip = new SakuyaV3(this.sound);
                        this.dropchips[4].codeNo = 0;
                    }
                    else
                    {
                        this.dropchips[4].chip = new TimeStopper(this.sound);
                        this.dropchips[4].codeNo = 0;
                    }
                    this.havezenny = 8800;
                    break;
                default:
                    this.dropchips[0].chip = new SakuyaV1(this.sound);
                    this.dropchips[0].codeNo = 1;
                    this.dropchips[1].chip = new SakuyaV2(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new SakuyaV2(this.sound);
                    this.dropchips[2].codeNo = 1;
                    this.dropchips[3].chip = new SakuyaV1(this.sound);
                    this.dropchips[3].codeNo = 2;
                    this.dropchips[4].chip = new SakuyaV3(this.sound);
                    this.dropchips[4].codeNo = 1;
                    this.havezenny = 9000;
                    if (this.version < 8)
                        break;
                    this.dropchips[4].chip = new SakuyaX(this.sound);
                    this.dropchips[4].codeNo = this.Random.Next(4);
                    this.havezenny = 18000;
                    break;
            }
        }

        public override void PositionDirectSet()
        {
            this.positionDirect = new Vector2((float)(position.X * 40.0 + 18.0), (float)(position.Y * 24.0 + 58.0));
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
            this.neutlal = this.Motion == NaviBase.MOTION.neutral;
            switch (this.Motion)
            {
                case NaviBase.MOTION.neutral:
                    if (this.moveflame)
                        ++this.waittime;
                    if (this.moveflame)
                    {
                        this.animationpoint = this.AnimeNeutral(this.waittime);
                        if (this.waittime >= (this.roopneutral == 1 ? 16 - Math.Min(8, (version - 1) * 2) : 8 - Math.Min(4, version - 1)))
                        {
                            this.waittime = 0;
                            ++this.roopneutral;
                            if (this.roopneutral >= 1 && this.parent.nowscene != SceneBattle.BATTLESCENE.end)
                            {
                                this.roopneutral = 0;
                                if (this.roopmove > this.moveroop && !this.badstatus[4])
                                {
                                    this.roopmove = 0;
                                    this.speed = 1;
                                    ++this.attackroop;
                                    this.waittime = 0;
                                    this.Motion = NaviBase.MOTION.attack;
                                    this.counterTiming = true;
                                }
                                else
                                {
                                    if (this.attackroop >= 2)
                                    {
                                        int index = this.Random.Next(2);
                                        switch (index)
                                        {
                                            case 0:
                                                this.attack = Sakuya.ATTACK.throwKnife3;
                                                break;
                                            case 1:
                                                this.attack = this.Random.Next(100) % 2 != 1 ? Sakuya.ATTACK.backKnife : Sakuya.ATTACK.frontKnife;
                                                break;
                                            case 2:
                                                this.attack = Sakuya.ATTACK.timeStop;
                                                break;
                                        }
                                        this.powerPlus = this.powers[index];
                                    }
                                    else
                                    {
                                        int index = 0;
                                        if (this.version > 1 && this.Random.Next(100) % 2 == 1)
                                            index = this.Hp >= this.HpMax / 2 ? 0 : 4;
                                        this.attack = (Sakuya.ATTACK)index;
                                        this.powerPlus = this.powers[index];
                                    }
                                    this.waittime = 0;
                                    this.Motion = NaviBase.MOTION.move;
                                }
                            }
                        }
                        break;
                    }
                    break;
                case NaviBase.MOTION.attack:
                    switch (this.attack)
                    {
                        case Sakuya.ATTACK.throwKnife:
                            if (this.moveflame)
                                ++this.waittime;
                            if (this.moveflame)
                            {
                                this.animationpoint = this.AnimeThrow(this.waittime);
                                switch (this.waittime)
                                {
                                    case 1:
                                        this.speed = 1;
                                        break;
                                    case 16:
                                        this.counterTiming = false;
                                        this.sound.PlaySE(SoundEffect.knife);
                                        this.parent.attacks.Add(new ThrowKnife(this.sound, this.parent, this.position.X + this.UnionRebirth(this.union), this.position.Y, this.union, this.version < 3 ? 6 : 10, this.Power, 0, 0, 3));
                                        break;
                                    case 76:
                                        this.roopneutral = 0;
                                        this.Motion = NaviBase.MOTION.neutral;
                                        this.speed = this.nspeed;
                                        break;
                                }
                                break;
                            }
                            break;
                        case Sakuya.ATTACK.throwKnife3:
                            if (this.moveflame)
                                ++this.waittime;
                            if (this.moveflame)
                            {
                                this.animationpoint = this.AnimeThrow(this.waittime);
                                switch (this.waittime)
                                {
                                    case 1:
                                        this.speed = 1;
                                        break;
                                    case 16:
                                        int[] array = ((IEnumerable<int>)new int[3]
                                        {
                      30 - Math.Min(15, ( version - 1) * 4),
                      50 - Math.Min(25, ( version - 1) * 5),
                      70 - Math.Min(35, ( version - 1) * 6)
                                        }).OrderBy<int, Guid>(i => Guid.NewGuid()).ToArray<int>();
                                        this.counterTiming = false;
                                        this.sound.PlaySE(SoundEffect.chain);
                                        int stoptime = 20 - Math.Min(15, (version - 1) * 3);
                                        this.parent.attacks.Add(new ThrowKnife(this.sound, this.parent, this.position.X + this.UnionRebirth(this.union), 0, this.union, this.version < 3 ? 6 : 10, this.Power, array[0], stoptime, 3));
                                        this.parent.attacks.Add(new ThrowKnife(this.sound, this.parent, this.position.X + this.UnionRebirth(this.union), 1, this.union, this.version < 3 ? 6 : 10, this.Power, array[1], stoptime, 3));
                                        this.parent.attacks.Add(new ThrowKnife(this.sound, this.parent, this.position.X + this.UnionRebirth(this.union), 2, this.union, this.version < 3 ? 6 : 10, this.Power, array[2], stoptime, 3));
                                        break;
                                    case 76:
                                        this.roopneutral = 0;
                                        this.Motion = NaviBase.MOTION.neutral;
                                        this.speed = this.nspeed;
                                        this.attackroop = 0;
                                        break;
                                }
                                break;
                            }
                            else
                                break;
                        case Sakuya.ATTACK.frontKnife:
                            if (this.moveflame)
                                ++this.waittime;
                            if (this.moveflame)
                            {
                                this.animationpoint = this.AnimeSwors(this.waittime);
                                switch (this.waittime)
                                {
                                    case 1:
                                        this.effecting = true;
                                        break;
                                    case 20:
                                        ((IEnumerable<int>)new int[3]
                                        {
                      30,
                      50,
                      70
                                        }).OrderBy<int, Guid>(i => Guid.NewGuid()).ToArray<int>();
                                        this.counterTiming = false;
                                        this.sound.PlaySE(SoundEffect.sword);
                                        this.parent.attacks.Add(new KnifeAttack(this.sound, this.parent, this.position.X + this.UnionRebirth(this.union), this.position.Y, this.union, this.Power, this.speed, ChipBase.ELEMENT.normal, false));
                                        break;
                                    case 50:
                                        this.attack = Sakuya.ATTACK.throwKnife;
                                        this.roopneutral = 0;
                                        this.attackroop = 0;
                                        this.effecting = false;
                                        this.speed = this.nspeed;
                                        this.Motion = NaviBase.MOTION.move;
                                        break;
                                }
                                if (this.waittime != 20) { }
                                break;
                            }
                            break;
                        case Sakuya.ATTACK.backKnife:
                            if (this.moveflame)
                                ++this.waittime;
                            if (this.moveflame)
                            {
                                this.animationpoint = this.AnimeSwors(this.waittime);
                                switch (this.waittime)
                                {
                                    case 1:
                                        this.effecting = true;
                                        this.rebirth = !this.rebirth;
                                        break;
                                    case 20:
                                        ((IEnumerable<int>)new int[3]
                                        {
                      30,
                      50,
                      70
                                        }).OrderBy<int, Guid>(i => Guid.NewGuid()).ToArray<int>();
                                        this.counterTiming = false;
                                        this.sound.PlaySE(SoundEffect.sword);
                                        AttackBase attackBase = new KnifeAttack(this.sound, this.parent, this.position.X - this.UnionRebirth(this.union), this.position.Y, this.union == Panel.COLOR.blue ? Panel.COLOR.red : Panel.COLOR.blue, this.Power, this.speed, ChipBase.ELEMENT.normal, false);
                                        attackBase.union = this.union;
                                        this.parent.attacks.Add(attackBase);
                                        break;
                                    case 50:
                                        this.rebirth = this.union == Panel.COLOR.red;
                                        this.attack = Sakuya.ATTACK.throwKnife;
                                        this.roopneutral = 0;
                                        this.attackroop = 0;
                                        this.effecting = false;
                                        this.Motion = NaviBase.MOTION.move;
                                        break;
                                }
                                if (this.waittime != 20) { }
                                break;
                            }
                            break;
                        case Sakuya.ATTACK.timeStop:
                            if (this.moveflame)
                                ++this.waittime;
                            if (this.moveflame)
                            {
                                this.animationpoint = this.AnimeThrow(this.waittime);
                                switch (this.waittime)
                                {
                                    case 1:
                                        this.speed = 1;
                                        break;
                                    case 16:
                                        this.counterTiming = false;
                                        this.sound.PlaySE(SoundEffect.chain);
                                        int stoptime = 20 - Math.Min(15, (version - 1) * 3);
                                        int spintime = 70 - Math.Min(30, version * 10);
                                        Point point = this.RandomTarget();
                                        Point knifePosPlus;
                                        Point knifePosMinus;
                                        if (this.Random.Next(100) % 2 == 0)
                                        {
                                            knifePosPlus = new Point(point.X + this.UnionRebirth(this.union), point.Y);
                                            knifePosMinus = new Point(point.X - this.UnionRebirth(this.union), point.Y);
                                            if (this.NoObject(knifePosPlus) && this.InAreaCheck(knifePosPlus))
                                                this.parent.attacks.Add(new ThrowKnife(this.sound, this.parent, knifePosPlus.X, knifePosPlus.Y, this.union, this.version < 3 ? 6 : 10, this.Power, spintime, stoptime, 1));
                                            if (this.NoObject(knifePosMinus) && this.InAreaCheck(knifePosMinus))
                                                this.parent.attacks.Add(new ThrowKnife(this.sound, this.parent, knifePosMinus.X, knifePosMinus.Y, this.union, this.version < 3 ? 6 : 10, this.Power, spintime, stoptime, 3));
                                            break;
                                        }
                                        knifePosPlus = new Point(point.X, point.Y + 1);
                                        knifePosMinus = new Point(point.X, point.Y - 1);
                                        if (this.NoObject(knifePosPlus) && this.InAreaCheck(knifePosPlus))
                                            this.parent.attacks.Add(new ThrowKnife(this.sound, this.parent, knifePosPlus.X, knifePosPlus.Y, this.union, this.version < 3 ? 6 : 10, this.Power, spintime, stoptime, 0));
                                        if (this.NoObject(knifePosMinus) && this.InAreaCheck(knifePosMinus))
                                            this.parent.attacks.Add(new ThrowKnife(this.sound, this.parent, knifePosMinus.X, knifePosMinus.Y, this.union, this.version < 3 ? 6 : 10, this.Power, spintime, stoptime, 2));
                                        break;
                                    case 76:
                                        this.roopneutral = 0;
                                        this.Motion = NaviBase.MOTION.neutral;
                                        this.speed = this.nspeed;
                                        this.attackroop = 0;
                                        break;
                                }
                            }
                            break;
                        case Sakuya.ATTACK.kawarimi:
                            if (this.moveflame)
                                ++this.waittime;
                            if (this.moveflame)
                            {
                                this.animationpoint = this.AnimeThrow2(this.waittime);
                                switch (this.waittime)
                                {
                                    case 2:
                                        this.speed = 1;
                                        this.nohit = true;
                                        this.y = 48;
                                        this.shadow = new Shadow(this.sound, this.parent, this.position.X, this.position.Y, this);
                                        this.parent.effects.Add(shadow);
                                        this.parent.effects.Add(new MoveEnemy(this.sound, this.parent, this.position.X, this.position.Y));
                                        break;
                                    case 16:
                                        ((IEnumerable<int>)new int[3]
                                        {
                      30 - Math.Min(15, ( version - 1) * 4),
                      50 - Math.Min(25, ( version - 1) * 5),
                      70 - Math.Min(35, ( version - 1) * 6)
                                        }).OrderBy<int, Guid>(i => Guid.NewGuid()).ToArray<int>();
                                        this.counterTiming = false;
                                        this.sound.PlaySE(SoundEffect.knife);
                                        Point point1 = this.RandomTarget();
                                        this.parent.attacks.Add(new DelayKnife(this.sound, this.parent, point1.X, point1.Y, this.union, this.Power, 16, this.element));
                                        break;
                                    case 32:
                                        this.sound.PlaySE(SoundEffect.knife);
                                        Point point2 = this.RandomTarget();
                                        this.parent.attacks.Add(new DelayKnife(this.sound, this.parent, point2.X, point2.Y, this.union, this.Power, 16, this.element));
                                        break;
                                    case 48:
                                        this.sound.PlaySE(SoundEffect.knife);
                                        Point point3 = this.RandomTarget();
                                        this.parent.attacks.Add(new DelayKnife(this.sound, this.parent, point3.X, point3.Y, this.union, this.Power, 16, this.element));
                                        break;
                                    case 76:
                                        if (this.shadow != null)
                                            this.shadow.flag = false;
                                        this.y = 0;
                                        this.nohit = false;
                                        this.animationpoint.X = 0;
                                        this.roopneutral = 0;
                                        this.Motion = NaviBase.MOTION.neutral;
                                        this.speed = this.nspeed;
                                        this.attackroop = 0;
                                        break;
                                }
                            }
                            break;
                    }
                    break;
                case NaviBase.MOTION.move:
                    if (this.moveflame)
                        ++this.waittime;
                    ++this.roopmove;
                    this.Motion = NaviBase.MOTION.neutral;
                    switch (this.attack)
                    {
                        case Sakuya.ATTACK.throwKnife3:
                            if (this.Canmove(new Point(this.union == Panel.COLOR.blue ? 5 : 0, 1), this.number, this.union))
                            {
                                this.positionre = new Point(this.union == Panel.COLOR.blue ? 5 : 0, 1);
                                this.attack = Sakuya.ATTACK.throwKnife3;
                                this.powerPlus = this.powers[(int)this.attack] * version;
                                this.motion = NaviBase.MOTION.attack;
                                break;
                            }
                            this.attackroop = 0;
                            this.MoveRandom(false, false);
                            this.Motion = NaviBase.MOTION.neutral;
                            this.speed = this.nspeed;
                            break;
                        case Sakuya.ATTACK.frontKnife:
                        case Sakuya.ATTACK.backKnife:
                            this.DammySet();
                            Point point4 = this.RandomTarget();
                            if (this.Canmove(new Point(point4.X + this.UnionRebirth(this.union) * (this.attack == Sakuya.ATTACK.backKnife ? 1 : -1), point4.Y), this.number, this.union == Panel.COLOR.blue ? Panel.COLOR.red : Panel.COLOR.blue))
                            {
                                this.positionre = new Point(point4.X + this.UnionRebirth(this.union) * (this.attack == Sakuya.ATTACK.backKnife ? 1 : -1), point4.Y);
                                this.motion = NaviBase.MOTION.attack;
                                break;
                            }
                            this.attackroop = 0;
                            this.MoveRandom(false, false);
                            this.Motion = NaviBase.MOTION.neutral;
                            break;
                        default:
                            this.MoveRandom(false, false);
                            this.Motion = NaviBase.MOTION.neutral;
                            this.speed = this.nspeed;
                            this.dammy.flag = false;
                            break;
                    }
                    if (this.attackroop > 4)
                        this.attackroop = 0;
                    if (this.position == this.positionre)
                    {
                        this.frame = 0;
                        this.roopneutral = 0;
                        break;
                    }
                    if (this.version > 2)
                        this.parent.effects.Add(new StepShadow(this.sound, this.parent, new Rectangle(this.animationpoint.X * this.wide, (this.version < 4 ? 0 : 2) * this.height, this.wide, this.height), this.positionDirect, this.picturename, this.rebirth, this.position));
                    else
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
                            this.nohit = false;
                            if (this.shadow != null)
                                this.shadow.flag = false;
                            this.y = 0;
                            this.rebirth = this.union == Panel.COLOR.red;
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
                            this.animationpoint = new Point(9, 0);
                            this.PositionDirectSet();
                            break;
                        case 21:
                            this.animationpoint = new Point(0, 0);
                            this.waittime = 0;
                            if (this.version > 1)
                            {
                                this.attack = Sakuya.ATTACK.kawarimi;
                                this.powerPlus = this.powers[(int)this.attack];
                                this.Motion = NaviBase.MOTION.attack;
                                break;
                            }
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
            int num2 = (int)this.positionDirect.Y - this.y;
            Point shake = this.Shake;
            int y1 = shake.Y;
            double num3 = num2 + y1;
            this._position = new Vector2((float)num1, (float)num3);
            this._rect = new Rectangle(this.animationpoint.X * this.wide, (this.version < 4 ? 0 : 2) * this.height, this.wide, this.height);
            if (this.Hp <= 0)
            {
                this.rd = this._rect;
                this.NockMotion();
                int num4 = this.animationpoint.X * this.wide;
                shake = this.Shake;
                int x1 = shake.X;
                int x2 = num4 + x1;
                int y2 = (this.version < 4 ? 0 : 2) * this.height;
                int wide = this.wide;
                int height1 = this.height;
                shake = this.Shake;
                int y3 = shake.Y;
                int height2 = height1 + y3;
                this._rect = new Rectangle(x2, y2, wide, height2);
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
            this.HPposition = new Vector2((int)this.positionDirect.X + 8, (int)this.positionDirect.Y - this.height / 2);
            this.Nameprint(dg, this.printNumber);
        }

        private Point AnimeNeutral(int waitflame)
        {
            return this.Return(new int[1], new int[1], 1, waitflame);
        }

        private Point AnimeMove(int waitflame)
        {
            return this.Return(new int[7] { 0, 1, 2, 3, 5, 6, 7 }, new int[7]
            {
        0,
        1,
        2,
        3,
        2,
        1,
        0
            }, 1, waitflame);
        }

        private Point AnimeThrow(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[6]
            {
        4,
        12,
        4,
        4,
        40,
        4
            }, new int[6] { 0, 9, 10, 11, 12, 0 }, 0, waittime);
        }

        private Point AnimeSwors(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[8]
            {
        10,
        8,
        2,
        2,
        2,
        2,
        5,
        10
            }, new int[8] { 2, 2, 3, 4, 5, 6, 7, 8 }, 0, waittime);
        }

        private Point AnimeThrow2(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[6]
            {
        4,
        12,
        4,
        4,
        40,
        4
            }, new int[6] { 0, 13, 14, 15, 16, 16 }, 0, waittime);
        }

        private enum ATTACK
        {
            throwKnife,
            throwKnife3,
            frontKnife,
            backKnife,
            timeStop,
            kawarimi,
        }
    }
}

