using NSAttack;
using NSBattle;
using NSChip;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEffect;
using NSGame;
using NSObject;
using Common.Vectors;
using System.Collections.Generic;
using System.Drawing;

namespace NSEnemy
{
    internal class PyroMan : NaviBase
    {
        private readonly int[] powers = new int[4] { 40, 20, 10, 30 };
        private readonly int nspeed = 7;
        private readonly List<Point> posi = new List<Point>();
        private readonly List<StandBurner> burner = new List<StandBurner>();
        private int noneBurnerCount = 6;
        private readonly int moveroop;
        private PyroMan.ATTACK attack;
        private int roopneutral;
        private int roopmove;
        private bool mothion2;
        private int x;
        private int atackroop;

        public PyroMan(IAudioEngine s, SceneBattle p, int pX, int pY, byte n, Panel.COLOR u, byte v)
          : base(s, p, pX, pY, n, u, v)
        {
            for (int index = 0; index < this.dropchips.Length; ++index)
                this.dropchips[index] = new ChipFolder(this.sound);
            this.element = ChipBase.ELEMENT.heat;
            switch (this.version)
            {
                case 0:
                    this.name = ShanghaiEXE.Translate("Enemy.PyroManName1");
                    this.power = 110;
                    this.hp = 2000;
                    this.moveroop = 1;
                    this.version = 3;
                    break;
                case 1:
                    this.name = ShanghaiEXE.Translate("Enemy.PyroManName2");
                    this.power = 60;
                    this.hp = 1500;
                    this.moveroop = 2;
                    break;
                case 2:
                    this.name = ShanghaiEXE.Translate("Enemy.PyroManName3");
                    this.power = 90;
                    this.hp = 1700;
                    this.moveroop = 2;
                    break;
                case 3:
                    this.name = ShanghaiEXE.Translate("Enemy.PyroManName4");
                    this.power = 120;
                    this.hp = 1900;
                    this.moveroop = 1;
                    this.nspeed = 6;
                    break;
                case 4:
                    this.name = ShanghaiEXE.Translate("Enemy.PyroManName5");
                    this.power = 200;
                    this.hp = 2200;
                    this.moveroop = 1;
                    this.nspeed = 5;
                    break;
                default:
                    this.name = ShanghaiEXE.Translate("Enemy.PyroManName6") + (version - 3).ToString();
                    this.power = 200;
                    this.hp = 2200 + (version - 4) * 500;
                    this.moveroop = 1;
                    this.nspeed = 4;
                    break;
            }
            this.picturename = nameof(PyroMan);
            this.superArmor = true;
            this.race = EnemyBase.ENEMY.navi;
            this.Flying = false;
            this.wide = 64;
            this.height = 96;
            this.hpmax = this.hp;
            this.speed = this.nspeed;
            this.hpprint = this.hp;
            this.printhp = true;
            this.effecting = false;
            this.roopmove = n * 3;
            this.PositionDirectSet();
            switch (this.version)
            {
                case 1:
                    this.dropchips[0].chip = new PyroManV1(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new PyroManV1(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new PyroManV1(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new PyroManV1(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new PyroManV1(this.sound);
                    this.dropchips[4].codeNo = 0;
                    this.havezenny = 0;
                    break;
                case 2:
                    this.dropchips[0].chip = new PyroManV2(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new PyroManV2(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new PyroManV2(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new PyroManV2(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new PyroManV2(this.sound);
                    this.dropchips[4].codeNo = 0;
                    this.havezenny = 0;
                    break;
                case 3:
                    this.dropchips[0].chip = new PyroManV1(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new PyroManV1(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new PyroManV2(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new PyroManV2(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new PyroManV3(this.sound);
                    this.dropchips[4].codeNo = 0;
                    this.havezenny = 2500;
                    break;
                default:
                    this.dropchips[0].chip = new PyroManV1(this.sound);
                    this.dropchips[0].codeNo = 1;
                    this.dropchips[1].chip = new PyroManV2(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new PyroManV2(this.sound);
                    this.dropchips[2].codeNo = 1;
                    this.dropchips[3].chip = new PyroManV1(this.sound);
                    this.dropchips[3].codeNo = 2;
                    this.dropchips[4].chip = new PyroManV3(this.sound);
                    this.dropchips[4].codeNo = 1;
                    this.havezenny = 9000;
                    if (this.version >= 8)
                    {
                        this.dropchips[4].chip = new PyroManX(this.sound);
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
            this.positionDirect = new Vector2((float)(position.X * 40.0 + 18.0), (float)(position.Y * 24.0 + 58.0));
        }

        protected override void Moving()
        {
            this.neutlal = this.Motion == NaviBase.MOTION.neutral;
            if (this.noneBurnerCount >= 6 && this.burner.Count < 2)
            {
                Point point = this.RandomPanel(this.union);
                StandBurner standBurner = new StandBurner(this.sound, this.parent, point.X, point.Y, this.powers[3], this.union);
                this.burner.Add(standBurner);
                this.parent.objects.Add(standBurner);
                if (this.burner.Count >= 2)
                    this.noneBurnerCount = 0;
            }
            this.burner.RemoveAll(b => !b.flag);
            switch (this.Motion)
            {
                case NaviBase.MOTION.neutral:
                    if (this.moveflame)
                        ++this.waittime;
                    if (this.moveflame)
                    {
                        this.animationpoint = this.AnimeNeutral(this.waittime);
                        if (this.waittime >= 4)
                        {
                            this.waittime = 0;
                            ++this.roopneutral;
                            if (this.roopneutral >= 1 && this.parent.nowscene != SceneBattle.BATTLESCENE.end)
                            {
                                this.roopneutral = 0;
                                if (this.roopmove > this.moveroop && !this.badstatus[4])
                                {
                                    ++this.atackroop;
                                    this.roopmove = 0;
                                    this.speed = this.nspeed / 2;
                                    int index = this.Random.Next(3);
                                    this.attack = (PyroMan.ATTACK)index;
                                    this.powerPlus = this.powers[index];
                                    this.waittime = 0;
                                    if (this.burner.Count < 2)
                                        ++this.noneBurnerCount;
                                    this.Motion = NaviBase.MOTION.attack;
                                    this.counterTiming = true;
                                }
                                else
                                {
                                    this.waittime = 0;
                                    this.Motion = NaviBase.MOTION.move;
                                }
                            }
                        }
                        break;
                    }
                    break;
                case NaviBase.MOTION.attack:
                    if (this.moveflame)
                        ++this.waittime;
                    if (this.moveflame)
                    {
                        switch (this.attack)
                        {
                            case PyroMan.ATTACK.pyroBlaze:
                                if (!this.mothion2)
                                {
                                    this.animationpoint = this.AnimeBreath1(this.waittime);
                                    switch (this.waittime)
                                    {
                                        case 1:
                                            this.counterTiming = false;
                                            break;
                                        case 5:
                                            this.counterTiming = true;
                                            break;
                                        case 7:
                                            this.x = 0;
                                            ++this.x;
                                            this.counterTiming = false;
                                            this.mothion2 = true;
                                            this.waittime = 0;
                                            break;
                                    }
                                }
                                else
                                {
                                    this.animationpoint = this.AnimeBreath2(this.waittime);
                                    int num = 0;
                                    switch (this.waittime)
                                    {
                                        case 1:
                                            this.sound.PlaySE(SoundEffect.fire);
                                            ElementFire elementFire1 = new ElementFire(this.sound, this.parent, this.position.X + this.x * this.UnionRebirth(this.union), 0, this.union, this.Power, 0, ChipBase.ELEMENT.heat, false, 1);
                                            elementFire1.positionDirect.Y += num;
                                            this.parent.attacks.Add(elementFire1);
                                            ElementFire elementFire2 = new ElementFire(this.sound, this.parent, this.position.X + this.x * this.UnionRebirth(this.union), 1, this.union, this.Power, 0, ChipBase.ELEMENT.heat, false, 1);
                                            elementFire2.positionDirect.Y += num;
                                            this.parent.attacks.Add(elementFire2);
                                            this.parent.attacks.Add(new ElementFire(this.sound, this.parent, this.position.X + this.x * this.UnionRebirth(this.union), 2, this.union, this.Power, 0, ChipBase.ELEMENT.heat, false, 1));
                                            break;
                                        case 8:
                                            ++this.x;
                                            if (this.position.X + this.x * this.UnionRebirth(this.union) < 0 || this.position.X + this.x * this.UnionRebirth(this.union) > 5)
                                            {
                                                this.Motion = NaviBase.MOTION.move;
                                                this.frame = 0;
                                                this.waittime = 0;
                                                this.mothion2 = false;
                                                this.speed = this.nspeed;
                                            }
                                            this.waittime = 0;
                                            break;
                                    }
                                    break;
                                }
                                break;
                            case PyroMan.ATTACK.frameShooter:
                                this.animationpoint = this.AnimeArm(this.waittime);
                                switch (this.waittime)
                                {
                                    case 5:
                                        this.counterTiming = false;
                                        break;
                                    case 10:
                                        this.parent.attacks.Add(new FireBreath(this.sound, this.parent, this.position.X + this.UnionRebirth(this.union), this.position.Y, this.union, this.Power, 2, this.element, this.RandomTarget().X));
                                        break;
                                    case 20:
                                        this.counterTiming = false;
                                        this.roopneutral = 0;
                                        this.waittime = 0;
                                        if (this.speed < 5)
                                            this.speed = 5;
                                        this.Motion = NaviBase.MOTION.neutral;
                                        this.speed = this.nspeed;
                                        break;
                                }
                                break;
                            case PyroMan.ATTACK.footBurner:
                                this.animationpoint = this.AnimeFoot(this.waittime);
                                switch (this.waittime)
                                {
                                    case 5:
                                        this.counterTiming = false;
                                        this.posi.Clear();
                                        break;
                                    case 11:
                                        for (int index = 0; index < 3; ++index)
                                        {
                                            Point point = this.RandomPanel(this.UnionEnemy);
                                            this.posi.Add(point);
                                            this.parent.attacks.Add(new Dummy(this.sound, this.parent, point.X, point.Y, this.union, new Point(), 30, true));
                                        }
                                        break;
                                    case 26:
                                        this.sound.PlaySE(SoundEffect.quake);
                                        Point point1;
                                        for (int index = 0; index < this.posi.Count; ++index)
                                        {
                                            IAudioEngine sound = this.sound;
                                            SceneBattle parent = this.parent;
                                            point1 = this.posi[index];
                                            int x = point1.X;
                                            point1 = this.posi[index];
                                            int y = point1.Y;
                                            int union = (int)this.union;
                                            int power = this.Power;
                                            int element = (int)this.element;
                                            this.parent.attacks.Add(new Tower(sound, parent, x, y, (Panel.COLOR)union, power, 999, (ChipBase.ELEMENT)element)
                                            {
                                                make = true
                                            });
                                        }
                                        this.posi.Clear();
                                        for (int index = 0; index < 3; ++index)
                                        {
                                            Point point2 = this.RandomPanel(this.UnionEnemy);
                                            this.posi.Add(point2);
                                            List<AttackBase> attacks = this.parent.attacks;
                                            IAudioEngine sound = this.sound;
                                            SceneBattle parent = this.parent;
                                            int x = point2.X;
                                            int y = point2.Y;
                                            int union = (int)this.union;
                                            point1 = new Point();
                                            Point hitrange = point1;
                                            Dummy dummy = new Dummy(sound, parent, x, y, (Panel.COLOR)union, hitrange, 30, true);
                                            attacks.Add(dummy);
                                        }
                                        break;
                                    case 41:
                                        this.sound.PlaySE(SoundEffect.quake);
                                        Point point3;
                                        for (int index = 0; index < this.posi.Count; ++index)
                                        {
                                            IAudioEngine sound = this.sound;
                                            SceneBattle parent = this.parent;
                                            point3 = this.posi[index];
                                            int x = point3.X;
                                            point3 = this.posi[index];
                                            int y = point3.Y;
                                            int union = (int)this.union;
                                            int power = this.Power;
                                            int element = (int)this.element;
                                            this.parent.attacks.Add(new Tower(sound, parent, x, y, (Panel.COLOR)union, power, 999, (ChipBase.ELEMENT)element)
                                            {
                                                make = true
                                            });
                                        }
                                        this.posi.Clear();
                                        for (int index = 0; index < 3; ++index)
                                        {
                                            Point point2 = this.RandomPanel(this.UnionEnemy);
                                            this.posi.Add(point2);
                                            List<AttackBase> attacks = this.parent.attacks;
                                            IAudioEngine sound = this.sound;
                                            SceneBattle parent = this.parent;
                                            int x = point2.X;
                                            int y = point2.Y;
                                            int union = (int)this.union;
                                            point3 = new Point();
                                            Point hitrange = point3;
                                            Dummy dummy = new Dummy(sound, parent, x, y, (Panel.COLOR)union, hitrange, 30, true);
                                            attacks.Add(dummy);
                                        }
                                        break;
                                    case 56:
                                        this.sound.PlaySE(SoundEffect.quake);
                                        for (int index = 0; index < this.posi.Count; ++index)
                                        {
                                            IAudioEngine sound = this.sound;
                                            SceneBattle parent = this.parent;
                                            Point point2 = this.posi[index];
                                            int x = point2.X;
                                            point2 = this.posi[index];
                                            int y = point2.Y;
                                            int union = (int)this.union;
                                            int power = this.Power;
                                            int element = (int)this.element;
                                            this.parent.attacks.Add(new Tower(sound, parent, x, y, (Panel.COLOR)union, power, 999, (ChipBase.ELEMENT)element)
                                            {
                                                make = true
                                            });
                                        }
                                        this.posi.Clear();
                                        break;
                                    case 71:
                                        this.counterTiming = false;
                                        this.roopneutral = 0;
                                        this.waittime = 0;
                                        this.Motion = NaviBase.MOTION.move;
                                        this.speed = this.nspeed;
                                        break;
                                }
                                break;
                        }
                        break;
                    }
                    break;
                case NaviBase.MOTION.move:
                    if (this.moveflame)
                        ++this.waittime;
                    ++this.roopmove;
                    this.animationpoint = this.AnimeNeutral(this.waittime);
                    this.Motion = NaviBase.MOTION.neutral;
                    this.MoveRandom(false, false);
                    if (this.position == this.positionre)
                    {
                        this.Motion = NaviBase.MOTION.neutral;
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
                            this.NockMotion();
                            this.counterTiming = false;
                            this.mothion2 = false;
                            this.effecting = false;
                            this.PositionDirectSet();
                            break;
                        case 3:
                            this.NockMotion();
                            break;
                        case 15:
                            this.animationpoint = new Point(5, 0);
                            this.PositionDirectSet();
                            break;
                        case 21:
                            this.animationpoint = new Point(0, 0);
                            this.waittime = 0;
                            this.Motion = NaviBase.MOTION.move;
                            break;
                    }
                    if (this.waittime >= 2 && this.waittime <= 6)
                        this.positionDirect.X -= this.UnionRebirth(this.union);
                    ++this.waittime;
                    break;
            }
            this.FlameControl();
            this.MoveAftar();
        }

        public override void NockMotion()
        {
            this.animationpoint = new Point(4, 0);
        }

        public override void Render(IRenderer dg)
        {
            if (this.alfha < byte.MaxValue)
                this.color = Color.FromArgb(alfha, this.mastorcolor);
            else
                this.color = this.mastorcolor;
            this._position = new Vector2((int)this.positionDirect.X + this.Shake.X, (int)this.positionDirect.Y + this.Shake.Y);
            this._rect = new Rectangle(this.animationpoint.X * this.wide, (this.version < 4 ? 0 : 2) * this.height, this.wide, this.height);
            if (this.Hp <= 0)
            {
                foreach (ObjectBase objectBase in this.burner)
                    objectBase.Break();
                this.rd = this._rect;
                this.NockMotion();
                this._rect = new Rectangle(this.animationpoint.X * this.wide + this.Shake.X, (this.version < 4 ? 0 : 2) * this.height, this.wide, this.height + this.Shake.Y);
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
            this.HPposition = new Vector2((int)this.positionDirect.X + 4, (int)this.positionDirect.Y + this.height / 2 - 8);
            this.Nameprint(dg, this.printNumber);
        }

        private Point AnimeNeutral(int waitflame)
        {
            return this.Return(new int[4] { 0, 1, 2, 3 }, new int[4]
            {
        0,
        1,
        2,
        3
            }, 1, waitflame);
        }

        private Point AnimeArm(int waitflame)
        {
            return this.Return(new int[7] { 1, 2, 3, 4, 5, 6, 100 }, new int[7]
            {
        5,
        6,
        7,
        8,
        9,
        10,
        11
            }, 0, waitflame);
        }

        private Point AnimeBreath1(int waitflame)
        {
            return this.ReturnKai(new int[7]
            {
        1,
        1,
        1,
        1,
        1,
        1,
        100
            }, new int[7] { 12, 13, 14, 15, 16, 17, 18 }, 0, waitflame);
        }

        private Point AnimeBreath2(int waitflame)
        {
            return this.ReturnKai(new int[9]
            {
        1,
        1,
        1,
        1,
        1,
        1,
        1,
        1,
        100
            }, new int[9] { 19, 20, 21, 22, 19, 20, 21, 22, 23 }, 0, waitflame);
        }

        private Point AnimeFoot(int waitflame)
        {
            return this.ReturnKai(new int[10]
            {
        1,
        1,
        1,
        1,
        1,
        1,
        1,
        1,
        1,
        100
            }, new int[10]
            {
        24,
        25,
        26,
        27,
        28,
        29,
        30,
        31,
        32,
        33
            }, 0, waitflame);
        }

        private enum ATTACK
        {
            pyroBlaze,
            frameShooter,
            footBurner,
            sss,
        }
    }
}

