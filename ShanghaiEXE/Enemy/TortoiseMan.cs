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
    internal class TortoiseMan : NaviBase
    {
        private readonly int[] pattern = new int[9]
        {
      2,
      0,
      0,
      1,
      0,
      2,
      0,
      1,
      1
        };
        private readonly int[] powers = new int[3] { 10, 20, 50 };
        private readonly int nspeed = 2;
        private readonly int aspeed = 4;
        private readonly Point[,] target = new Point[2, 9];
        private readonly Rock[] metalcube = new Rock[2];
        private int action;
        private bool attackUP;
        private readonly int attackSpeed;
        private bool ready;
        private bool end;
        private int attackCount;
        private int attackProcess;
        private readonly int moveroop;
        private TortoiseMan.ATTACK attack;
        private int roopneutral;
        private int roopmove;
        private int attackroop;
        private int waveX;
        private int waveY;

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

        public TortoiseMan(IAudioEngine s, SceneBattle p, int pX, int pY, byte n, Panel.COLOR u, byte v)
          : base(s, p, pX, pY, n, u, v)
        {
            for (int index = 0; index < this.dropchips.Length; ++index)
                this.dropchips[index] = new ChipFolder(this.sound);
            this.element = ChipBase.ELEMENT.earth;
            switch (this.version)
            {
                case 1:
                    this.name = ShanghaiEXE.Translate("Enemy.TortoiseManName1");
                    this.power = 80;
                    this.hp = 1500;
                    this.moveroop = 2;
                    this.nspeed = 4;
                    this.aspeed = 3;
                    this.attackSpeed = 8;
                    break;
                case 2:
                    this.name = ShanghaiEXE.Translate("Enemy.TortoiseManName2");
                    this.power = 100;
                    this.hp = 1800;
                    this.moveroop = 2;
                    this.nspeed = 4;
                    this.aspeed = 3;
                    this.attackSpeed = 8;
                    break;
                case 3:
                    this.name = ShanghaiEXE.Translate("Enemy.TortoiseManName3");
                    this.power = 120;
                    this.hp = 2000;
                    this.moveroop = 3;
                    this.nspeed = 4;
                    this.aspeed = 3;
                    this.attackSpeed = 9;
                    break;
                case 4:
                    this.name = ShanghaiEXE.Translate("Enemy.TortoiseManName4");
                    this.power = 200;
                    this.hp = 2300;
                    this.moveroop = 3;
                    this.nspeed = 4;
                    this.aspeed = 2;
                    this.attackSpeed = 10;
                    break;
                default:
                    this.name = ShanghaiEXE.Translate("Enemy.TortoiseManName5") + (version - 3).ToString();
                    this.power = 200;
                    this.hp = 2000 + (version - 4) * 500;
                    this.moveroop = 3;
                    this.nspeed = 4;
                    this.aspeed = 2;
                    this.attackSpeed = 10;
                    break;
            }
            this.speed = this.nspeed;
            this.picturename = nameof(TortoiseMan);
            this.race = EnemyBase.ENEMY.navi;
            this.wide = 128;
            this.height = 112;
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
                    this.dropchips[0].chip = new TortoiseManV1(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new TortoiseManV1(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new TortoiseManV1(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new TortoiseManV1(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new TortoiseManV1(this.sound);
                    this.dropchips[4].codeNo = 0;
                    this.havezenny = 0;
                    break;
                case 2:
                    this.dropchips[0].chip = new TortoiseManV2(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new TortoiseManV2(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new TortoiseManV2(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new TortoiseManV2(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new TortoiseManV2(this.sound);
                    this.dropchips[4].codeNo = 0;
                    this.havezenny = 0;
                    break;
                case 3:
                    this.dropchips[0].chip = new TortoiseManV1(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new TortoiseManV1(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new TortoiseManV2(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new TortoiseManV2(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new TortoiseManV3(this.sound);
                    this.dropchips[4].codeNo = 0;
                    this.havezenny = 8800;
                    break;
                default:
                    this.dropchips[0].chip = new TortoiseManV1(this.sound);
                    this.dropchips[0].codeNo = 1;
                    this.dropchips[1].chip = new TortoiseManV2(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new TortoiseManV2(this.sound);
                    this.dropchips[2].codeNo = 1;
                    this.dropchips[3].chip = new TortoiseManV1(this.sound);
                    this.dropchips[3].codeNo = 2;
                    this.dropchips[4].chip = new TortoiseManV3(this.sound);
                    this.dropchips[4].codeNo = 1;
                    this.havezenny = 9000;
                    if (this.version >= 8)
                    {
                        this.dropchips[4].chip = new TortoiseManX(this.sound);
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
            this.positionDirect = new Vector2((float)(position.X * 40.0 + 24.0), (float)(position.Y * 24.0 + 42.0));
        }

        protected override void Moving()
        {
            this.neutlal = this.Motion == NaviBase.MOTION.neutral;
            switch (this.Motion)
            {
                case NaviBase.MOTION.neutral:
                    if (this.moveflame)
                        ++this.waittime;
                    if (this.moveflame && this.waittime >= 16 / Math.Max(2, Math.Min((int)this.version, 4)))
                    {
                        this.waittime = 0;
                        ++this.roopneutral;
                        this.animationpoint = this.AnimeNeutral(this.waittime);
                        if (this.roopneutral >= 1 && this.parent.nowscene != SceneBattle.BATTLESCENE.end)
                        {
                            this.roopneutral = 0;
                            if (this.roopmove > this.moveroop && !this.badstatus[4])
                            {
                                this.roopmove = 0;
                                this.speed = 1;
                                ++this.attackroop;
                                this.waittime = 0;
                                this.ready = false;
                                this.attack = (TortoiseMan.ATTACK)this.pattern[this.action];
                                this.powerPlus = this.powers[this.pattern[this.action]];
                                ++this.action;
                                if (this.action >= this.pattern.Length)
                                    this.action = 0;
                                this.Motion = NaviBase.MOTION.attack;
                                this.counterTiming = true;
                            }
                            else
                            {
                                this.waittime = 0;
                                this.roopmove = this.moveroop + 1;
                                this.Motion = NaviBase.MOTION.move;
                            }
                        }
                        break;
                    }
                    break;
                case NaviBase.MOTION.attack:
                    switch (this.attack)
                    {
                        case TortoiseMan.ATTACK.ShellHockeySide:
                            if (this.moveflame)
                                ++this.waittime;
                            if (this.moveflame)
                            {
                                if (!this.ready)
                                {
                                    this.animationpoint = this.AnimeShellReady(this.waittime);
                                    if (this.waittime == this.aspeed)
                                        this.sound.PlaySE(SoundEffect.throwbomb);
                                    if (this.waittime == this.aspeed * 3)
                                        this.sound.PlaySE(SoundEffect.chain);
                                    if (this.waittime >= this.aspeed * 7)
                                    {
                                        this.attackProcess = 0;
                                        this.ready = true;
                                        this.effecting = true;
                                        this.guard = CharacterBase.GUARD.guard;
                                        this.counterTiming = false;
                                        this.Noslip = true;
                                        this.waittime = 0;
                                        this.sound.PlaySE(SoundEffect.knife);
                                    }
                                }
                                else if (!this.end)
                                {
                                    this.animationpoint = this.AnimeShellSpin(this.waittime);
                                    if (this.waittime >= this.aspeed * 3)
                                        this.waittime = 0;
                                }
                                else
                                {
                                    this.animationpoint = this.AnimeShellEnd(this.waittime);
                                    if (this.waittime >= this.aspeed * 7)
                                    {
                                        this.SlideMoveEnd();
                                        this.motion = NaviBase.MOTION.move;
                                        this.Noslip = false;
                                        this.end = false;
                                        this.ready = false;
                                        this.effecting = false;
                                        this.guard = CharacterBase.GUARD.none;
                                        this.waittime = 0;
                                        this.HitFlagReset();
                                        this.counterTiming = false;
                                    }
                                }
                            }
                            if (this.ready && !this.end)
                            {
                                if (this.attackProcess == 0)
                                {
                                    if (this.SlideMove(attackSpeed, 0))
                                    {
                                        this.SlideMoveEnd();
                                        if (!this.InAreaCheck(new Point(this.position.X + this.UnionRebirth(this.union), this.position.Y)))
                                        {
                                            switch (this.position.Y)
                                            {
                                                case 0:
                                                    this.attackUP = false;
                                                    this.attackProcess = 1;
                                                    break;
                                                case 2:
                                                    this.attackUP = true;
                                                    this.attackProcess = 1;
                                                    break;
                                                default:
                                                    this.HitFlagReset();
                                                    this.attackProcess = 2;
                                                    break;
                                            }
                                        }
                                    }
                                }
                                else if (this.attackProcess == 1)
                                {
                                    if (this.SlideMove(attackSpeed, this.attackUP ? 2 : 3))
                                    {
                                        this.SlideMoveEnd();
                                        if (!this.InAreaCheck(new Point(this.position.X, this.position.Y + (this.attackUP ? -1 : 1))))
                                            this.attackProcess = 2;
                                    }
                                }
                                else if (this.SlideMove(attackSpeed, 1))
                                {
                                    this.SlideMoveEnd();
                                    if (!this.InAreaCheck(new Point(this.position.X - this.UnionRebirth(this.union), this.position.Y)))
                                    {
                                        this.attackProcess = 0;
                                        this.waittime = 0;
                                        this.effecting = false;
                                        this.end = true;
                                    }
                                }
                                break;
                            }
                            break;
                        case TortoiseMan.ATTACK.QuakePress:
                            if (this.moveflame)
                                ++this.waittime;
                            if (this.moveflame)
                            {
                                this.animationpoint = this.AnimeQuake(this.waittime);
                                if (this.waittime == this.aspeed * 4)
                                {
                                    this.counterTiming = false;
                                    this.sound.PlaySE(SoundEffect.quake);
                                    this.ShakeStart(1, this.aspeed * 12 * this.speed);
                                    for (int index = 0; index < Math.Min(2 + version / 2, 4); ++index)
                                    {
                                        Point point = this.RandomPanel(this.UnionEnemy);
                                        this.parent.attacks.Add(new FallStone(this.sound, this.parent, point.X, point.Y, this.union, this.Power, 1, 20 * index, this.element));
                                    }
                                }
                                if (this.waittime == this.aspeed * 8)
                                {
                                    this.animationpoint = new Point(0, 0);
                                    this.waittime = 0;
                                    this.Motion = NaviBase.MOTION.neutral;
                                }
                                break;
                            }
                            break;
                        case TortoiseMan.ATTACK.MadWave:
                            if (this.moveflame)
                                ++this.waittime;
                            if (this.moveflame)
                            {
                                this.animationpoint = this.AnimeWave(this.waittime);
                                if (this.waittime == this.aspeed * 4)
                                    this.sound.PlaySE(SoundEffect.knock);
                                if (this.waittime == this.aspeed * 10)
                                    this.sound.PlaySE(SoundEffect.knock);
                                if (this.waittime == this.aspeed * 12)
                                {
                                    this.counterTiming = false;
                                    this.waveX = Eriabash.SteelX(this, this.parent);
                                    this.waveY = 1;
                                    this.parent.attacks.Add(new Dummy(this.sound, this.parent, this.waveX, this.waveY - 1, this.union, new Point(1, 2), this.aspeed * 8 * this.speed, true));
                                }
                                if (this.waittime == this.aspeed * 17)
                                    this.ShakeStart(1, this.aspeed * 8 * this.speed);
                                if (this.waittime == this.aspeed * 19)
                                {
                                    this.sound.PlaySE(SoundEffect.wave);
                                    this.parent.attacks.Add(new MadWave(this.sound, this.parent, this.waveX, this.waveY, this.union, this.Power, 6, this.element));
                                }
                                if (this.waittime == this.aspeed * 23)
                                {
                                    if (this.metalcube[0].flag)
                                        this.metalcube[0].Break();
                                    if (this.metalcube[1].flag)
                                        this.metalcube[1].Break();
                                    this.MoveRandom(false, false, this.union, true);
                                    this.sound.PlaySE(SoundEffect.enterenemy);
                                    Point positionre = this.positionre;
                                    this.metalcube[0] = new Rock(this.sound, this.parent, positionre.X, positionre.Y, this.union);
                                    this.parent.objects.Add(this.metalcube[0]);
                                    this.parent.effects.Add(new MoveEnemy(this.sound, this.parent, positionre.X, positionre.Y));
                                    this.MoveRandom(false, false, this.UnionEnemy, true);
                                    positionre = this.positionre;
                                    this.metalcube[1] = new Rock(this.sound, this.parent, positionre.X, positionre.Y, this.union);
                                    this.parent.objects.Add(this.metalcube[1]);
                                    this.parent.effects.Add(new MoveEnemy(this.sound, this.parent, positionre.X, positionre.Y));
                                    this.positionre = this.position;
                                }
                                if (this.waittime == this.aspeed * 27)
                                {
                                    this.animationpoint = new Point(0, 0);
                                    this.waittime = 0;
                                    this.Motion = NaviBase.MOTION.neutral;
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
            this.HPposition = new Vector2((int)this.positionDirect.X, (int)this.positionDirect.Y - this.height / 2 + 104);
            this.Nameprint(dg, this.printNumber);
        }

        private Point AnimeNeutral(int waitflame)
        {
            return this.Return(new int[1], new int[1], 1, waitflame);
        }

        private Point AnimeShellReady(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[7]
            {
        this.aspeed,
        this.aspeed,
        this.aspeed,
        this.aspeed,
        this.aspeed,
        this.aspeed,
        100
            }, new int[7] { 2, 3, 4, 5, 6, 7, 7 }, 0, waittime);
        }

        private Point AnimeShellSpin(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[3]
            {
        this.aspeed,
        this.aspeed,
        this.aspeed
            }, new int[3] { 8, 9, 10 }, 0, waittime);
        }

        private Point AnimeShellEnd(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[7]
            {
        this.aspeed,
        this.aspeed,
        this.aspeed,
        this.aspeed,
        this.aspeed,
        this.aspeed,
        this.aspeed
            }, new int[7] { 7, 6, 5, 4, 3, 2, 0 }, 0, waittime);
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
            ShellHockeySide,
            QuakePress,
            MadWave,
        }
    }
}

