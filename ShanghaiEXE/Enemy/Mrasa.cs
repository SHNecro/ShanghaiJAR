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
using System.Drawing;

namespace NSEnemy
{
    internal class Mrasa : NaviBase
    {
        private readonly int[] pattern = new int[11]
        {
      0,
      1,
      2,
      2,
      1,
      2,
      1,
      0,
      2,
      1,
      2
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
        private readonly int[] powers = new int[4] { 80, 20, 30, 100 };
        private readonly int nspeed = 2;
        private readonly int aspeed = 4;
        private readonly Rock[] metalcube = new Rock[2];
        private int action;
        private readonly bool attackUP;
        private readonly int attackSpeed;
        private bool ready;
        private readonly bool end;
        private int attackCount;
        private readonly int attackProcess;
        private readonly int moveroop;
        private Mrasa.ATTACK attack;
        private int roopneutral;
        private int roopmove;
        private Point target;
        private int attackroop;
        private AncerShot ancher;
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

        public Mrasa(IAudioEngine s, SceneBattle p, int pX, int pY, byte n, Panel.COLOR u, byte v)
          : base(s, p, pX, pY, n, u, v)
        {
            for (int index = 0; index < this.dropchips.Length; ++index)
                this.dropchips[index] = new ChipFolder(this.sound);
            this.element = ChipBase.ELEMENT.aqua;
            switch (this.version)
            {
                case 0:
                    this.name = ShanghaiEXE.Translate("Enemy.MrasaName1");
                    this.power = 100;
                    this.hp = 1900;
                    this.moveroop = 4;
                    this.nspeed = 3;
                    this.aspeed = 3;
                    this.attackSpeed = 8;
                    this.version = 2;
                    break;
                case 1:
                    this.name = ShanghaiEXE.Translate("Enemy.MrasaName2");
                    this.power = 50;
                    this.hp = 1700;
                    this.moveroop = 4;
                    this.nspeed = 3;
                    this.aspeed = 3;
                    this.attackSpeed = 8;
                    break;
                case 2:
                    this.name = ShanghaiEXE.Translate("Enemy.MrasaName3");
                    this.power = 130;
                    this.hp = 1900;
                    this.moveroop = 4;
                    this.nspeed = 3;
                    this.aspeed = 3;
                    this.attackSpeed = 8;
                    break;
                case 3:
                    this.name = ShanghaiEXE.Translate("Enemy.MrasaName4");
                    this.power = 160;
                    this.hp = 2100;
                    this.moveroop = 7;
                    this.nspeed = 3;
                    this.aspeed = 2;
                    this.attackSpeed = 9;
                    break;
                case 4:
                    this.name = ShanghaiEXE.Translate("Enemy.MrasaName5");
                    this.power = 180;
                    this.hp = 2500;
                    this.moveroop = 8;
                    this.nspeed = 3;
                    this.aspeed = 2;
                    this.attackSpeed = 10;
                    break;
                default:
                    this.name = ShanghaiEXE.Translate("Enemy.MrasaName6") + (version - 3).ToString();
                    this.power = 250;
                    this.hp = 2500 + (version - 4) * 500;
                    this.moveroop = 8;
                    this.nspeed = 3;
                    this.aspeed = 2;
                    this.attackSpeed = 10;
                    break;
            }
            this.speed = this.nspeed;
            this.picturename = "mrasa";
            this.race = EnemyBase.ENEMY.navi;
            this.wide = 72;
            this.height = 80;
            this.hpmax = this.hp;
            this.hpprint = this.hp;
            this.printhp = true;
            this.effecting = false;
            this.Flying = false;
            this.roopmove = 0;
            this.PositionDirectSet();
            for (int index = 0; index < this.metalcube.Length; ++index)
            {
                this.metalcube[index] = new Rock(this.sound, this.parent, 0, 0, this.union)
                {
                    flag = false
                };
            }
            switch (this.version)
            {
                case 1:
                    this.dropchips[0].chip = new MrasaV1(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new MrasaV1(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new MrasaV1(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new MrasaV1(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new MrasaV1(this.sound);
                    this.dropchips[4].codeNo = 0;
                    this.havezenny = 0;
                    break;
                case 2:
                    this.dropchips[0].chip = new MrasaV2(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new MrasaV2(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new MrasaV2(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new MrasaV2(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new MrasaV2(this.sound);
                    this.dropchips[4].codeNo = 0;
                    this.havezenny = 0;
                    break;
                case 3:
                    this.dropchips[0].chip = new MrasaV1(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new MrasaV1(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new MrasaV2(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new MrasaV2(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new MrasaV3(this.sound);
                    this.dropchips[4].codeNo = 0;
                    this.havezenny = 8800;
                    break;
                default:
                    this.dropchips[0].chip = new MrasaV1(this.sound);
                    this.dropchips[0].codeNo = 1;
                    this.dropchips[1].chip = new MrasaV2(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new MrasaV2(this.sound);
                    this.dropchips[2].codeNo = 1;
                    this.dropchips[3].chip = new MrasaV1(this.sound);
                    this.dropchips[3].codeNo = 2;
                    this.dropchips[4].chip = new MrasaV3(this.sound);
                    this.dropchips[4].codeNo = 1;
                    this.havezenny = 9000;
                    if (this.version >= 8)
                    {
                        this.dropchips[4].chip = new MrasaX(this.sound);
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
            this.positionDirect = new Vector2((float)(position.X * 40.0 + 8.0), (float)(position.Y * 24.0 + 58.0));
        }

        public override void InitAfter()
        {
        }

        protected override void Moving()
        {
            this.neutlal = this.Motion == NaviBase.MOTION.neutral;
            switch (this.Motion)
            {
                case NaviBase.MOTION.neutral:
                    if (this.moveflame)
                        ++this.waittime;
                    if (this.moveflame && this.waittime >= 12 / Math.Max(2, Math.Min((int)this.version, 4)) + this.wait)
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
                                if (this.hp <= this.hpmax / 2)
                                {
                                    this.attack = (Mrasa.ATTACK)this.pattern2[this.action];
                                    this.powerPlus = this.powers[this.pattern2[this.action]];
                                }
                                else
                                {
                                    this.attack = (Mrasa.ATTACK)this.pattern[this.action];
                                    this.powerPlus = this.powers[this.pattern[this.action]];
                                }
                                ++this.action;
                                if (this.action >= this.pattern.Length)
                                    this.action = 0;
                                this.Motion = NaviBase.MOTION.attack;
                                this.counterTiming = true;
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
                        case Mrasa.ATTACK.AncerThrow:
                            this.animationpoint = this.AnimeAncerThrow(this.waittime);
                            switch (this.waittime)
                            {
                                case 8:
                                    this.counterTiming = false;
                                    this.sound.PlaySE(SoundEffect.throwbomb);
                                    this.target = this.RandomTarget();
                                    this.parent.attacks.Add(new AnchorBomb(this.sound, this.parent, this.positionre.X, this.positionre.Y, this.union, this.Power, 1, new Vector2(this.positionDirect.X + (this.union == Panel.COLOR.red ? 0.0f : 0.0f), this.positionDirect.Y - 8f), this.target, Math.Max(40 / (version / 2 + 1), 20), AnchorBomb.TYPE.single, -1));
                                    this.positionre = this.position;
                                    break;
                                case 16:
                                    this.roopneutral = 0;
                                    this.Motion = NaviBase.MOTION.neutral;
                                    break;
                            }
                            ++this.waittime;
                            break;
                        case Mrasa.ATTACK.WheelThrow:
                            this.animationpoint = this.AnimeWheelThrow(this.waittime);
                            switch (this.waittime)
                            {
                                case 8:
                                    this.counterTiming = false;
                                    this.sound.PlaySE(SoundEffect.knife);
                                    this.target = this.RandomTarget();
                                    Vector2 vector2 = new Vector2(this.positionDirect.X + (this.union == Panel.COLOR.red ? 0.0f : 0.0f), this.positionDirect.Y - 8f);
                                    this.parent.attacks.Add(new Wheel(this.sound, this.parent, this.position.X, this.position.Y, this.union, this.Power, 2, this.positionDirect, this.element, 5));
                                    this.positionre = this.position;
                                    break;
                                case 16:
                                    this.wait = 15;
                                    this.roopneutral = 0;
                                    this.Motion = NaviBase.MOTION.neutral;
                                    break;
                            }
                            ++this.waittime;
                            break;
                        case Mrasa.ATTACK.AncerShoot:
                            this.animationpoint = this.AnimeAncerShoot(this.waittime);
                            switch (this.waittime)
                            {
                                case 1:
                                    this.parent.attacks.Add(new Dummy(this.sound, this.parent, this.position.X + this.UnionRebirth(this.union), this.position.Y, this.union, new Point(6, 0), this.aspeed * 4, true));
                                    break;
                                case 12:
                                    this.superArmor = true;
                                    this.counterTiming = false;
                                    this.sound.PlaySE(SoundEffect.knife);
                                    this.target = this.RandomTarget();
                                    this.ancher = new AncerShot(this.sound, this.parent, this.position.X + this.UnionRebirth(this.union), this.position.Y, this.union, this.Power, 8, this.element, this);
                                    this.parent.attacks.Add(ancher);
                                    this.positionre = this.position;
                                    break;
                            }
                            if (this.ancher != null)
                            {
                                if (this.waittime > 12 && this.ancher.scene == 1)
                                {
                                    this.MoveRandom(false, false);
                                    if (this.position == this.positionre)
                                    {
                                        this.frame = 0;
                                        this.roopneutral = 0;
                                    }
                                    else
                                    {
                                        this.animationpoint.X = 12;
                                        this.parent.effects.Add(new MoveEnemy(this.sound, this.parent, this.position.X, this.position.Y));
                                        this.position = this.positionre;
                                        this.PositionDirectSet();
                                        this.frame = 0;
                                        this.roopneutral = 0;
                                    }
                                    this.parent.attacks.Add(new Dummy(this.sound, this.parent, this.position.X + this.UnionRebirth(this.union), this.position.Y, this.union, new Point(6, 0), this.aspeed * 4, true));
                                    this.ancher.position.Y = this.position.Y;
                                    this.ancher.positionDirect.Y = this.ancher.position.Y * 24 + 56;
                                    this.ancher.scene = 2;
                                }
                                else if (this.waittime > 12 && (this.ancher.scene == 2 || !this.ancher.flag) && (this.ancher.position == this.position || !this.ancher.flag))
                                {
                                    this.animationpoint.X = 11;
                                    this.superArmor = false;
                                    this.ancher.flag = false;
                                    this.roopneutral = 0;
                                    this.wait = 30;
                                    this.Motion = NaviBase.MOTION.neutral;
                                }
                            }
                            ++this.waittime;
                            break;
                        case Mrasa.ATTACK.GreatAncer:
                            switch (this.waittime)
                            {
                                case 1:
                                    this.parent.effects.Add(new MoveEnemy(this.sound, this.parent, this.position.X, this.position.Y));
                                    this.nohit = true;
                                    this.animationpoint.X = -1;
                                    break;
                                case 20:
                                    this.target = this.RandomTarget();
                                    this.parent.attacks.Add(new Dummy(this.sound, this.parent, this.target.X, this.target.Y, this.union, new Point(), this.aspeed * 40, true));
                                    break;
                                case 40:
                                    this.counterTiming = false;
                                    this.parent.attacks.Add(new MrasaAnchor(this.sound, this.parent, this.positionre.X, this.positionre.Y, this.union, this.Power, 1, new Vector2(this.positionDirect.X + (this.union == Panel.COLOR.red ? 0.0f : 0.0f), this.positionDirect.Y - 8f), this.target, 1, MrasaAnchor.TYPE.closs, this));
                                    this.positionre = this.position;
                                    break;
                                case 135:
                                    this.roopneutral = 0;
                                    this.nohit = false;
                                    this.animationpoint.X = 0;
                                    this.Motion = NaviBase.MOTION.neutral;
                                    break;
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
                            if (this.ancher != null)
                            {
                                this.ancher.flag = false;
                                this.ancher.scene = 0;
                            }
                            this.superArmor = false;
                            this.rebirth = this.union == Panel.COLOR.red;
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
                            this.animationpoint = new Point(7, 0);
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
            if (!this.nohit)
                this.HPposition = new Vector2((int)this.positionDirect.X + 16, (int)this.positionDirect.Y - this.height / 2 + 72);
            else
                this.HPposition = new Vector2(-80f, (int)this.positionDirect.Y - this.height / 2 + 72);
            this.Nameprint(dg, this.printNumber);
        }

        private Point AnimeNeutral(int waitflame)
        {
            return this.Return(new int[1], new int[1], 1, waitflame);
        }

        private Point AnimeAncerThrow(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[4]
            {
        this.aspeed,
        this.aspeed,
        this.aspeed,
        100
            }, new int[4] { 3, 4, 5, 6 }, 0, waittime);
        }

        private Point AnimeWheelThrow(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[4]
            {
        this.aspeed,
        this.aspeed,
        this.aspeed,
        100
            }, new int[4] { 7, 8, 9, 10 }, 0, waittime);
        }

        private Point AnimeAncerShoot(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[3]
            {
        this.aspeed * 3,
        this.aspeed,
        1000
            }, new int[3] { 11, 12, 13 }, 0, waittime);
        }

        private Point AnimeWave(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[14]
            {
        this.aspeed,
        this.aspeed * 3,
        this.aspeed,
        this.aspeed,
        this.aspeed,
        this.aspeed * 3,
        this.aspeed,
        this.aspeed,
        this.aspeed,
        this.aspeed * 6,
        this.aspeed,
        this.aspeed,
        this.aspeed,
        100
            }, new int[14]
            {
        11,
        12,
        16,
        17,
        11,
        12,
        16,
        17,
        11,
        12,
        13,
        14,
        15,
        15
            }, 0, waittime);
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
            AncerThrow,
            WheelThrow,
            AncerShoot,
            GreatAncer,
        }
    }
}

