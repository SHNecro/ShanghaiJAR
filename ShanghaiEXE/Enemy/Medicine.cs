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
using System.Collections.Generic;
using System.Drawing;

namespace NSEnemy
{
    internal class Medicine : NaviBase
    {
        private readonly int[] powers = new int[5] { 20, 0, 60, 0, 40 };
        private readonly int nspeed = 8;
        private readonly List<Point> stormP = new List<Point>();
        private readonly ObjectBase[] suzu = new ObjectBase[2];
        private readonly int moveroop;
        private Medicine.ATTACK attack;
        private int roopneutral;
        private int roopmove;
        private bool ballrend;
        private int atackroop;

        public Medicine(IAudioEngine s, SceneBattle p, int pX, int pY, byte n, Panel.COLOR u, byte v)
          : base(s, p, pX, pY, n, u, v)
        {
            for (int index = 0; index < this.dropchips.Length; ++index)
                this.dropchips[index] = new ChipFolder(this.sound);
            this.element = ChipBase.ELEMENT.poison;
            switch (this.version)
            {
                case 1:
                    this.name = ShanghaiEXE.Translate("Enemy.MedicineName1");
                    this.power = 30;
                    this.hp = 700;
                    this.moveroop = 2;
                    break;
                case 2:
                    this.name = ShanghaiEXE.Translate("Enemy.MedicineName2");
                    this.power = 60;
                    this.hp = 1000;
                    this.moveroop = 2;
                    break;
                case 3:
                    this.name = ShanghaiEXE.Translate("Enemy.MedicineName3");
                    this.power = 120;
                    this.hp = 1400;
                    this.moveroop = 1;
                    this.nspeed = 6;
                    break;
                case 4:
                    this.name = ShanghaiEXE.Translate("Enemy.MedicineName4");
                    this.power = 200;
                    this.hp = 2000;
                    this.moveroop = 1;
                    this.nspeed = 6;
                    break;
                default:
                    this.name = ShanghaiEXE.Translate("Enemy.MedicineName5") + (version - 3).ToString();
                    this.power = 200;
                    this.hp = 2000 + (version - 4) * 500;
                    this.moveroop = 1;
                    this.nspeed = 6;
                    break;
            }
            this.picturename = "medicine";
            this.race = EnemyBase.ENEMY.navi;
            this.Flying = false;
            this.wide = 48;
            this.height = 64;
            this.hpmax = this.hp;
            this.speed = this.nspeed;
            this.hpprint = this.hp;
            this.printhp = true;
            this.effecting = false;
            this.Flying = true;
            this.roopmove = n * 3;
            this.PositionDirectSet();
            switch (this.version)
            {
                case 1:
                    this.dropchips[0].chip = new MedicineV1(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new MedicineV1(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new MedicineV1(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new MedicineV1(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new MedicineV1(this.sound);
                    this.dropchips[4].codeNo = 0;
                    this.havezenny = 0;
                    break;
                case 2:
                    this.dropchips[0].chip = new MedicineV2(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new MedicineV2(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new MedicineV2(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new MedicineV2(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new MedicineV2(this.sound);
                    this.dropchips[4].codeNo = 0;
                    this.havezenny = 0;
                    break;
                case 3:
                    this.dropchips[0].chip = new MedicineV1(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new MedicineV1(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new MedicineV2(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new MedicineV2(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new MedicineV3(this.sound);
                    this.dropchips[4].codeNo = 0;
                    this.havezenny = 2500;
                    break;
                default:
                    this.dropchips[0].chip = new MedicineV1(this.sound);
                    this.dropchips[0].codeNo = 1;
                    this.dropchips[1].chip = new MedicineV2(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new MedicineV2(this.sound);
                    this.dropchips[2].codeNo = 1;
                    this.dropchips[3].chip = new MedicineV1(this.sound);
                    this.dropchips[3].codeNo = 2;
                    this.dropchips[4].chip = new MedicineV3(this.sound);
                    this.dropchips[4].codeNo = 1;
                    this.havezenny = 9000;
                    if (this.version >= 8)
                    {
                        this.dropchips[4].chip = new MedicineX(this.sound);
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

        private bool SuzuLive
        {
            get
            {
                if (this.suzu[0] != null)
                {
                    if (this.suzu[0].flag)
                        return true;
                }
                else if (this.suzu[1] != null && this.suzu[1].flag)
                    return true;
                return false;
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
                                    int index = this.Random.Next(this.Hp < this.HpMax / 2 ? (this.SuzuLive ? 3 : 4) : 2);
                                    this.attack = (Medicine.ATTACK)index;
                                    this.powerPlus = this.powers[index];
                                    this.waittime = 0;
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
                            case Medicine.ATTACK.poisonSmoke:
                                this.animationpoint = this.AnimePoisonSmoke(this.waittime);
                                switch (this.waittime)
                                {
                                    case 6:
                                        this.counterTiming = false;
                                        int s = 7 - (version - 1) * 3;
                                        if (s <= 0)
                                            s = 1;
                                        Tower tower = new Tower(this.sound, this.parent, this.position.X + this.UnionRebirth(this.union), this.position.Y, this.union, this.Power, s, this.element);
                                        tower.badstatus[5] = true;
                                        tower.badstatustime[5] = 60 * (version * 2);
                                        this.parent.attacks.Add(tower);
                                        break;
                                    case 14:
                                        this.roopneutral = 0;
                                        this.Motion = NaviBase.MOTION.neutral;
                                        this.speed = this.nspeed;
                                        break;
                                }
                                break;
                            case Medicine.ATTACK.bioStorm:
                                this.animationpoint = this.AnimeBioStorm(this.waittime);
                                switch (this.waittime)
                                {
                                    case 1:
                                        this.counterTiming = false;
                                        this.stormP.Clear();
                                        for (int index = 0; index < Math.Min(3, version + 1); ++index)
                                            this.stormP.Add(this.RandomPanel(this.UnionEnemy));
                                        using (List<Point>.Enumerator enumerator = this.stormP.GetEnumerator())
                                        {
                                            while (enumerator.MoveNext())
                                            {
                                                Point current = enumerator.Current;
                                                this.parent.attacks.Add(new Dummy(this.sound, this.parent, current.X, current.Y, this.union, new Point(), 60, true));
                                            }
                                            break;
                                        }
                                    case 30:
                                        this.counterTiming = false;
                                        this.roopneutral = 0;
                                        this.waittime = 0;
                                        this.Motion = NaviBase.MOTION.neutral;
                                        this.speed = this.nspeed;
                                        break;
                                }
                                int index1 = this.frame - (26 - Math.Min(12, version * 2));
                                if (this.frame == 26 - Math.Min(12, version * 2) - 8)
                                    this.counterTiming = true;
                                if (index1 < this.stormP.Count && this.frame >= 26 - Math.Min(12, version * 2))
                                {
                                    this.counterTiming = false;
                                    this.sound.PlaySE(SoundEffect.shoot);
                                    IAudioEngine sound = this.sound;
                                    SceneBattle parent = this.parent;
                                    Point point = this.stormP[index1];
                                    int x1 = point.X;
                                    point = this.stormP[index1];
                                    int y1 = point.Y;
                                    int union = (int)this.union;
                                    int po = version * 10;
                                    this.parent.attacks.Add(new NSAttack.Storm(sound, parent, x1, y1, (Panel.COLOR)union, po, 4, ChipBase.ELEMENT.leaf));
                                    Panel[,] panel = this.parent.panel;
                                    point = this.stormP[index1];
                                    int x2 = point.X;
                                    point = this.stormP[index1];
                                    int y2 = point.Y;
                                    panel[x2, y2].State = Panel.PANEL._grass;
                                    break;
                                }
                                break;
                            case Medicine.ATTACK.poisonBall:
                                this.animationpoint = this.AnimePoisonBall(this.waittime);
                                switch (this.waittime)
                                {
                                    case 1:
                                        this.sound.PlaySE(SoundEffect.eriasteal1);
                                        this.counterTiming = false;
                                        this.ballrend = true;
                                        break;
                                    case 30:
                                        this.counterTiming = false;
                                        this.roopneutral = 0;
                                        this.waittime = 0;
                                        this.Motion = NaviBase.MOTION.neutral;
                                        this.speed = this.nspeed;
                                        break;
                                }
                                int num = 20 - Math.Min(12, version * 2);
                                if (this.waittime == num - 8)
                                    this.counterTiming = true;
                                if (this.waittime == num)
                                {
                                    this.ballrend = false;
                                    this.counterTiming = false;
                                    this.sound.PlaySE(SoundEffect.sword);
                                    Point point = this.RandomTarget();
                                    this.parent.attacks.Add(new PoisonBall(this.sound, this.parent, point.X, point.Y, new Vector2(this.positionDirect.X, this.positionDirect.Y - 48f), this.union, this.Power, this.speed, 60, this.element));
                                    break;
                                }
                                break;
                            case Medicine.ATTACK.liveFrower:
                                this.animationpoint = this.AnimeLiveFrower(this.waittime);
                                switch (this.waittime)
                                {
                                    case 6:
                                        this.counterTiming = false;
                                        if (this.version >= 3)
                                        {
                                            this.SuzuWmake();
                                            this.SuzuBmake();
                                            break;
                                        }
                                        switch (this.Random.Next(2))
                                        {
                                            case 0:
                                                this.SuzuWmake();
                                                break;
                                            case 1:
                                                this.SuzuBmake();
                                                break;
                                        }
                                        break;
                                    case 14:
                                        this.roopneutral = 0;
                                        this.Motion = NaviBase.MOTION.neutral;
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
                            this.ballrend = false;
                            this.animationpoint = new Point(3, 0);
                            this.counterTiming = false;
                            this.effecting = false;
                            this.PositionDirectSet();
                            break;
                        case 3:
                            this.animationpoint = new Point(3, 0);
                            break;
                        case 15:
                            this.animationpoint = new Point(4, 0);
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
        }

        private void SuzuWmake()
        {
            if (this.suzu[0] == null)
            {
                this.MoveRandomOffPanel();
                Point positionre = this.positionre;
                this.positionre = this.position;
                this.parent.panel[positionre.X, positionre.Y].State = Panel.PANEL._grass;
                this.sound.PlaySE(SoundEffect.enterenemy);
                this.parent.effects.Add(new MoveEnemy(this.sound, this.parent, positionre.X, positionre.Y));
                this.suzu[0] = new SuzuranWhite(this.sound, this.parent, positionre.X, positionre.Y, this.union, 10 * version, 180);
                this.suzu[0].unionhit = false;
                this.parent.objects.Add(this.suzu[0]);
            }
            else
            {
                if (this.suzu[0].flag)
                    return;
                this.MoveRandomOffPanel();
                Point positionre = this.positionre;
                this.positionre = this.position;
                this.parent.panel[positionre.X, positionre.Y].State = Panel.PANEL._grass;
                this.sound.PlaySE(SoundEffect.enterenemy);
                this.parent.effects.Add(new MoveEnemy(this.sound, this.parent, positionre.X, positionre.Y));
                ObjectBase objectBase = new SuzuranWhite(this.sound, this.parent, positionre.X, positionre.Y, this.union, 10 * version, 180);
                objectBase.unionhit = false;
                objectBase.Hp = 10 * version;
                this.suzu[0] = objectBase;
                this.parent.objects.Add(this.suzu[0]);
            }
        }

        private void SuzuBmake()
        {
            if (this.suzu[1] == null)
            {
                this.MoveRandomOffPanel();
                Point positionre = this.positionre;
                this.positionre = this.position;
                this.parent.panel[positionre.X, positionre.Y].State = Panel.PANEL._grass;
                this.sound.PlaySE(SoundEffect.enterenemy);
                this.parent.effects.Add(new MoveEnemy(this.sound, this.parent, positionre.X, positionre.Y));
                ObjectBase objectBase = new SuzuranBlue(this.sound, this.parent, positionre.X, positionre.Y, this.union, 10 * version, 180);
                objectBase.unionhit = false;
                objectBase.Hp = 10 * version;
                this.suzu[1] = objectBase;
                this.parent.objects.Add(this.suzu[1]);
            }
            else
            {
                if (this.suzu[1].flag)
                    return;
                this.MoveRandomOffPanel();
                Point positionre = this.positionre;
                this.positionre = this.position;
                this.parent.panel[positionre.X, positionre.Y].State = Panel.PANEL._grass;
                this.sound.PlaySE(SoundEffect.enterenemy);
                this.parent.effects.Add(new MoveEnemy(this.sound, this.parent, positionre.X, positionre.Y));
                ObjectBase objectBase = new SuzuranBlue(this.sound, this.parent, positionre.X, positionre.Y, this.union, 10 * version, 180);
                objectBase.unionhit = false;
                objectBase.Hp = 10 * version;
                this.suzu[1] = objectBase;
                this.parent.objects.Add(this.suzu[1]);
            }
        }

        private void MoveRandomOffPanel()
        {
            const int attempts = 10;
            for (int i = 0; i < attempts; i++)
            {
                this.MoveRandom(false, false);

                if (this.positionre != this.position)
                {
                    break;
                }

                this.positionre = this.position;
            }
        }

        public override void NockMotion()
        {
            this.animationpoint = new Point(3, 0);
            this.ballrend = false;
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
            if (this.ballrend)
            {
                this._position = new Vector2(this.positionDirect.X, this.positionDirect.Y - 48f);
                this._rect = new Rectangle(this.waittime % 7 * 40, 352, 40, 40);
                dg.DrawImage(dg, "towers", this._rect, false, this._position, this.rebirth, Color.White);
            }
            this.HPposition = new Vector2((int)this.positionDirect.X + 4, (int)this.positionDirect.Y - this.height / 2 - 8);
            this.Nameprint(dg, this.printNumber);
        }

        private Point AnimeNeutral(int waitflame)
        {
            return this.Return(new int[5] { 0, 1, 2, 3, 4 }, new int[5]
            {
        0,
        1,
        2,
        1,
        0
            }, 1, waitflame);
        }

        private Point AnimePoisonSmoke(int waitflame)
        {
            return this.Return(new int[5] { 5, 6, 7, 8, 14 }, new int[5]
            {
        4,
        5,
        9,
        10,
        11
            }, 0, waitflame);
        }

        private Point AnimeBioStorm(int waitflame)
        {
            return this.ReturnKai(new int[5]
            {
        1,
        1,
        18 - Math.Min(12,  version * 2),
        1,
        60
            }, new int[5] { 8, 14, 15, 16, 17 }, 0, waitflame);
        }

        private Point AnimePoisonBall(int waitflame)
        {
            return this.ReturnKai(new int[5]
            {
        1,
        1,
        18 - Math.Min(12,  version * 2),
        1,
        60
            }, new int[5] { 8, 12, 13, 16, 17 }, 0, waitflame);
        }

        private Point AnimeLiveFrower(int waitflame)
        {
            return this.Return(new int[5] { 5, 6, 7, 8, 14 }, new int[5]
            {
        4,
        5,
        6,
        7,
        8
            }, 0, waitflame);
        }

        private Point AnimeCharge(int waitflame)
        {
            return this.Return(new int[4] { 1, 2, 3, 4 }, new int[4]
            {
        3,
        7,
        13,
        14
            }, 0, waitflame);
        }

        private enum ATTACK
        {
            poisonSmoke,
            bioStorm,
            poisonBall,
            liveFrower,
        }
    }
}

