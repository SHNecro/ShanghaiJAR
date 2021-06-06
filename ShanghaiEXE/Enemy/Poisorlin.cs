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
    internal class Poisorlin : EnemyBase
    {
        private Poisorlin.MOTION motion = Poisorlin.MOTION.neutral;
        private readonly NSAttack.PoisonShot.TYPE type;

        public Poisorlin(IAudioEngine s, SceneBattle p, int pX, int pY, byte n, Panel.COLOR u, byte v)
          : base(s, p, pX, pY, n, u, v)
        {
            this.helpPosition.Y = -4;
            for (int index = 0; index < this.dropchips.Length; ++index)
                this.dropchips[index] = new ChipFolder(this.sound);
            this.picturename = "poisorlin";
            this.race = EnemyBase.ENEMY.virus;
            this.Flying = false;
            this.wide = 48;
            this.height = 56;
            switch (this.version)
            {
                case 0:
                    this.name = ShanghaiEXE.Translate("Enemy.PoisorlinName1");
                    this.power = 200;
                    this.hp = 1200;
                    break;
                case 1:
                    this.name = ShanghaiEXE.Translate("Enemy.PoisorlinName2");
                    this.power = 20;
                    this.hp = 80;
                    break;
                case 2:
                    this.name = ShanghaiEXE.Translate("Enemy.PoisorlinName3");
                    this.power = 50;
                    this.hp = 130;
                    break;
                case 3:
                    this.name = ShanghaiEXE.Translate("Enemy.PoisorlinName4");
                    this.power = 80;
                    this.hp = 180;
                    break;
                case 4:
                    this.name = ShanghaiEXE.Translate("Enemy.PoisorlinName5");
                    this.power = 80;
                    this.hp = 200;
                    break;
                default:
                    this.name = ShanghaiEXE.Translate("Enemy.PoisorlinName6") + (version - 3).ToString();
                    this.power = 80 + (version - 4) * 20;
                    this.hp = 200 + (version - 4) * 40;
                    break;
            }
            this.Noslip = true;
            this.roop = this.number;
            this.hpmax = this.hp;
            this.speed = 7;
            this.hpprint = this.hp;
            this.printhp = true;
            this.effecting = false;
            this.PositionDirectSet();
            switch (this.version)
            {
                case 0:
                    this.dropchips[0].chip = new BreakShot(this.sound);
                    this.dropchips[0].codeNo = 3;
                    this.dropchips[1].chip = new BreakShot(this.sound);
                    this.dropchips[1].codeNo = 2;
                    this.dropchips[2].chip = new BreakShot(this.sound);
                    this.dropchips[2].codeNo = 1;
                    this.dropchips[3].chip = new BreakShot(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new BreakShot(this.sound);
                    this.dropchips[4].codeNo = 0;
                    this.havezenny = 600;
                    this.speed = 1;
                    this.type = NSAttack.PoisonShot.TYPE.sand;
                    this.element = ChipBase.ELEMENT.earth;
                    break;
                case 1:
                    this.dropchips[0].chip = new NSChip.PoisonShot(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new NSChip.PoisonShot(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new NSChip.PoisonShot(this.sound);
                    this.dropchips[2].codeNo = 2;
                    this.dropchips[3].chip = new NSChip.PoisonShot(this.sound);
                    this.dropchips[3].codeNo = 2;
                    this.dropchips[4].chip = new NSChip.PoisonShot(this.sound);
                    if (this.Random.Next(4) < 3)
                        this.dropchips[4].codeNo = 0;
                    else
                        this.dropchips[4].codeNo = 3;
                    this.havezenny = 300;
                    this.type = NSAttack.PoisonShot.TYPE.poison;
                    this.element = ChipBase.ELEMENT.poison;
                    break;
                case 2:
                    this.dropchips[0].chip = new BreakShot(this.sound);
                    this.dropchips[0].codeNo = 3;
                    this.dropchips[1].chip = new BreakShot(this.sound);
                    this.dropchips[1].codeNo = 2;
                    this.dropchips[2].chip = new BreakShot(this.sound);
                    this.dropchips[2].codeNo = 1;
                    this.dropchips[3].chip = new BreakShot(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new BreakShot(this.sound);
                    this.dropchips[4].codeNo = 0;
                    this.havezenny = 600;
                    this.type = NSAttack.PoisonShot.TYPE.break_;
                    this.element = ChipBase.ELEMENT.heat;
                    break;
                case 3:
                    this.dropchips[0].chip = new ColdShot(this.sound);
                    this.dropchips[0].codeNo = 3;
                    this.dropchips[1].chip = new ColdShot(this.sound);
                    this.dropchips[1].codeNo = 3;
                    this.dropchips[2].chip = new ColdShot(this.sound);
                    this.dropchips[2].codeNo = 2;
                    this.dropchips[3].chip = new ColdShot(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new ColdShot(this.sound);
                    this.dropchips[4].codeNo = 1;
                    this.havezenny = 800;
                    this.type = NSAttack.PoisonShot.TYPE.ice;
                    this.element = ChipBase.ELEMENT.aqua;
                    break;
                default:
                    this.dropchips[0].chip = new NSChip.PoisonShot(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new BreakShot(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new BreakShot(this.sound);
                    this.dropchips[2].codeNo = 2;
                    this.dropchips[3].chip = new ColdShot(this.sound);
                    this.dropchips[3].codeNo = 2;
                    this.dropchips[4].chip = new NSChip.PoisonShot(this.sound);
                    this.dropchips[4].codeNo = 3;
                    this.havezenny = 800;
                    this.type = NSAttack.PoisonShot.TYPE.grass;
                    this.element = ChipBase.ELEMENT.leaf;
                    if (this.version >= 8)
                    {
                        this.dropchips[4].chip = new PoisonShotX(this.sound);
                        this.dropchips[4].codeNo = this.Random.Next(4);
                        this.havezenny = 8000;
                        break;
                    }
                    break;
            }
            this.neutlal = true;
        }

        public override void PositionDirectSet()
        {
            this.positionDirect = new Vector2((float)(position.X * 40.0 + 14.0), (float)(position.Y * 24.0 + 60.0));
        }

        protected override void Moving()
        {
            this.neutlal = this.motion == Poisorlin.MOTION.neutral;
            if (this.moveflame)
            {
                this.positionre = this.position;
                switch (this.motion)
                {
                    case Poisorlin.MOTION.neutral:
                        this.animationpoint = this.AnimeNeutral(this.frame);
                        if (this.frame > 3 || this.version == 0)
                        {
                            if ((this.roop > 3 - version || this.version == 0) && !this.badstatus[4] && this.parent.nowscene != SceneBattle.BATTLESCENE.end)
                            {
                                this.animationpoint.X = 3;
                                this.motion = Poisorlin.MOTION.attack;
                                this.counterTiming = true;
                            }
                            else
                                ++this.roop;
                            this.frame = 0;
                            break;
                        }
                        break;
                    case Poisorlin.MOTION.attack:
                        switch (this.frame)
                        {
                            case 1:
                                this.sound.PlaySE(SoundEffect.chain);
                                if (this.parent.nowscene != SceneBattle.BATTLESCENE.end)
                                {
                                    Point end = this.RandomTarget();
                                    Vector2 v = new Vector2(this.positionDirect.X + (this.union == Panel.COLOR.red ? 16f : -16f), this.positionDirect.Y - 8f);
                                    int t;
                                    if (this.version > 0)
                                    {
                                        t = 80 / version;
                                    }
                                    else
                                    {
                                        t = 40;
                                        this.counterTiming = false;
                                    }
                                    this.parent.attacks.Add(new NSAttack.PoisonShot(this.sound, this.parent, this.positionre.X, this.positionre.Y, this.union, !this.badstatus[1] ? this.power : this.power / 2, 1, v, this.element, end, t, false, this.type));
                                    break;
                                }
                                break;
                            case 10:
                                this.frame = 0;
                                this.roop = 0;
                                this.motion = Poisorlin.MOTION.neutral;
                                this.counterTiming = false;
                                break;
                        }
                        break;
                }
            }
            this.MoveAftar();
            this.FlameControl();
        }

        public override void Render(IRenderer dg)
        {
            this._position = new Vector2((int)this.positionDirect.X + 6 + this.Shake.X, (int)this.positionDirect.Y + 2 + this.Shake.Y);
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
                dg.DrawImage(dg, this.picturename, this._rect, false, this._position, this.rebirth, this.color);
            }
            else
            {
                this._rect.Y = 0;
                dg.DrawImage(dg, this.picturename, this._rect, false, this._position, this.rebirth, this.color);
            }
            this.HPposition = new Vector2(this.positionDirect.X + 6f, this.positionDirect.Y + 2f - this.height / 2);
            this.Nameprint(dg, this.printNumber);
        }

        private Point AnimeNeutral(int waitflame)
        {
            return this.Return(new int[4] { 0, 1, 2, 3 }, new int[4]
            {
        1,
        2,
        1,
        0
            }, 1, waitflame);
        }

        private Point AnimeMove(int waitflame)
        {
            return this.Return(new int[8]
            {
        0,
        1,
        2,
        3,
        4,
        5,
        6,
        7
            }, new int[8] { 1, 2, 3, 4, 3, 2, 1, 0 }, 0, waitflame);
        }

        private Point AnimeAttack(int waitflame)
        {
            return this.Return(new int[23]
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
        14,
        15,
        16,
        17,
        18,
        19,
        20,
        21,
        22,
        23,
        24,
        25
            }, new int[24]
            {
        0,
        1,
        2,
        3,
        4,
        3,
        2,
        1,
        5,
        6,
        7,
        8,
        7,
        6,
        5,
        0,
        1,
        2,
        3,
        4,
        3,
        2,
        1,
        0
            }, 0, waitflame);
        }

        private enum MOTION
        {
            neutral,
            attack,
        }
    }
}

