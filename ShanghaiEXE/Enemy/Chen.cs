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
    internal class Chen : NaviBase
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
        private readonly int[] powers = new int[3] { 50, 50, 80 };
        private readonly int nspeed = 2;
        private readonly int aspeed = 4;
        private int manymove = 8;
        private bool movestart = false;
        private readonly Ogre[] ogres = new Ogre[2];
        private int action;
        private readonly int attackSpeed;
        private bool ready;
        private bool end;
        private int attackCount;
        private int attackProcess;
        private readonly int moveroop;
        private Chen.ATTACK attack;
        private int roopneutral;
        private int roopmove;
        private Shadow shadow;
        private int animeflame;
        private bool attackUP;
        private const int sp = 8;
        private Vector2 movespeed;
        private bool angleDOWN;
        private bool angleLEFT;
        private Ran ran;
        private readonly bool enemyaArea;
        private int refrect;
        private int attackroop;
        private readonly AncerShot ancher;
        private int noOgreTurn;
        private int wait;

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

        public Chen(IAudioEngine s, SceneBattle p, int pX, int pY, byte n, Panel.COLOR u, byte v)
          : base(s, p, pX, pY, n, u, v)
        {
            for (int index = 0; index < this.dropchips.Length; ++index)
                this.dropchips[index] = new ChipFolder(this.sound);
            this.element = ChipBase.ELEMENT.earth;
            switch (this.version)
            {
                case 0:
                    this.name = ShanghaiEXE.Translate("Enemy.ChenName1");
                    this.power = 70;
                    this.hp = 2200;
                    this.moveroop = 13;
                    this.nspeed = 3;
                    this.aspeed = 2;
                    this.attackSpeed = 9;
                    this.version = 3;
                    break;
                case 1:
                    this.name = ShanghaiEXE.Translate("Enemy.ChenName2");
                    this.power = 0;
                    this.hp = 1000;
                    this.moveroop = 4;
                    this.nspeed = 3;
                    this.aspeed = 3;
                    this.attackSpeed = 8;
                    break;
                case 2:
                    this.name = ShanghaiEXE.Translate("Enemy.ChenName3");
                    this.power = 30;
                    this.hp = 1500;
                    this.moveroop = 10;
                    this.nspeed = 3;
                    this.aspeed = 3;
                    this.attackSpeed = 8;
                    break;
                case 3:
                    this.name = ShanghaiEXE.Translate("Enemy.ChenName4");
                    this.power = 70;
                    this.hp = 1700;
                    this.moveroop = 13;
                    this.nspeed = 3;
                    this.aspeed = 2;
                    this.attackSpeed = 9;
                    break;
                case 4:
                    this.name = ShanghaiEXE.Translate("Enemy.ChenName5");
                    this.power = 100;
                    this.hp = 1900;
                    this.moveroop = 8;
                    this.nspeed = 3;
                    this.aspeed = 2;
                    this.attackSpeed = 10;
                    break;
                default:
                    this.name = ShanghaiEXE.Translate("Enemy.ChenName6") + (version - 3).ToString();
                    this.power = 120;
                    this.hp = 1900 + (version - 4) * 500;
                    this.moveroop = 8;
                    this.nspeed = 3;
                    this.aspeed = 2;
                    this.attackSpeed = 10;
                    break;
            }
            this.speed = this.nspeed;
            this.picturename = "chen";
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
                    this.dropchips[0].chip = new ChenV2(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new ChenV2(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new ChenV2(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new ChenV2(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new ChenV2(this.sound);
                    this.dropchips[4].codeNo = 0;
                    this.havezenny = 0;
                    break;
                case 3:
                    this.dropchips[0].chip = new ChenV1(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new ChenV1(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new ChenV2(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new ChenV2(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new ChenV3(this.sound);
                    this.dropchips[4].codeNo = 0;
                    this.havezenny = 8800;
                    break;
                default:
                    this.dropchips[0].chip = new ChenV1(this.sound);
                    this.dropchips[0].codeNo = 1;
                    this.dropchips[1].chip = new ChenV2(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new ChenV2(this.sound);
                    this.dropchips[2].codeNo = 1;
                    this.dropchips[3].chip = new ChenV1(this.sound);
                    this.dropchips[3].codeNo = 2;
                    this.dropchips[4].chip = new ChenV3(this.sound);
                    this.dropchips[4].codeNo = 1;
                    this.havezenny = 9000;
                    if (this.version >= 8)
                    {
                        this.dropchips[4].chip = new ChenX(this.sound);
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
            this.positionDirect = new Vector2(position.X * 40f, (float)(position.Y * 24.0 + 44.0));
            if (this.shadow == null)
                return;
            this.shadow.positionDirect = new Vector2(this.positionDirect.X - 20 * this.UnionRebirth(this.union), this.positionDirect.Y + 40f);
        }

        public override void InitAfter()
        {
            if (this.parent != null)
            {
                this.shadow = new Shadow(this.sound, this.parent, this.position.X, this.position.Y, this);
                this.shadow.slide.X = this.union == Panel.COLOR.red ? -8 : 0;
                this.parent.effects.Add(shadow);
                this.shadow.PositionDirectSet(this.position);
                this.shadow.positionDirect = new Vector2(this.positionDirect.X - 20 * this.UnionRebirth(this.union), this.positionDirect.Y + 40f);
            }
            for (int index = 0; index < this.Parent.enemys.Count; ++index)
            {
                if (index != this.number && this.Parent.enemys[index] is Ran)
                {
                    this.ran = (Ran)this.Parent.enemys[index];
                    break;
                }
            }
            if (this.ran != null)
                return;
            this.ogres[0] = new Ogre(this.sound, this.parent, this.union == Panel.COLOR.red ? 2 : 3, 0, 0, this.union, this.powers[2], true, this.version >= 4);
            this.ogres[1] = new Ogre(this.sound, this.parent, this.union == Panel.COLOR.red ? 1 : 4, 2, 0, this.union, this.powers[2], false, this.version >= 4);
            this.parent.objects.Add(this.ogres[0]);
            this.parent.objects.Add(this.ogres[1]);
        }

        protected override void Moving()
        {
            if (this.shadow != null)
                this.shadow.positionDirect = new Vector2(this.positionDirect.X - 20 * this.UnionRebirth(this.union), this.positionDirect.Y + 40f);
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
                        if (this.roopneutral >= this.nspeed && this.parent.nowscene != SceneBattle.BATTLESCENE.end)
                        {
                            this.roopneutral = 0;
                            if (this.roopmove > this.moveroop && !this.badstatus[4])
                            {
                                this.roopmove = this.Random.Next(this.moveroop - 2);
                                this.speed = 1;
                                ++this.attackroop;
                                this.waittime = 0;
                                this.ready = false;
                                this.attack = this.ran == null ? (this.position.Y != 1 ? (Chen.ATTACK)this.Random.Next(2) : Chen.ATTACK.Hockey) : (this.position.Y != 1 ? Chen.ATTACK.Boomerang : Chen.ATTACK.Hockey);
                                this.powerPlus = this.powers[(int)this.attack];
                                ++this.action;
                                if (this.action >= this.pattern.Length)
                                    this.action = 0;
                                this.attackProcess = 0;
                                this.ready = false;
                                this.end = false;
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
                    if (this.parent.panel[this.position.X, this.position.Y].Hole)
                    {
                        this.HitFlagReset();
                        this.PositionDirectSet();
                        this.roopmove = 0;
                        this.attackProcess = 0;
                        this.effecting = false;
                        this.motion = NaviBase.MOTION.move;
                        this.OgreSet();
                    }
                    switch (this.attack)
                    {
                        case Chen.ATTACK.Boomerang:
                            if (!this.ready)
                            {
                                this.animationpoint = this.AnimeReady(this.waittime);
                                int num = this.waittime / this.aspeed;
                                if (this.waittime % this.aspeed == 0)
                                {
                                    switch (num)
                                    {
                                        case 5:
                                            this.counterTiming = false;
                                            this.sound.PlaySE(SoundEffect.knife);
                                            this.ready = true;
                                            this.effecting = true;
                                            this.waittime = 0;
                                            this.refrect = 0;
                                            this.attackProcess = 0;
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                if (this.waittime % 3 == 0)
                                {
                                    ++this.animeflame;
                                    if (this.animeflame >= 3)
                                        this.animeflame = 0;
                                    this.animationpoint.X = 6 + this.animeflame;
                                }
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
                                        this.HitFlagReset();
                                        this.PositionDirectSet();
                                        this.roopmove = 0;
                                        this.attackProcess = 0;
                                        this.effecting = false;
                                        this.motion = NaviBase.MOTION.move;
                                        this.OgreSet();
                                    }
                                }
                            }
                            ++this.waittime;
                            break;
                        case Chen.ATTACK.Hockey:
                            if (!this.ready)
                            {
                                this.animationpoint = this.AnimeReady(this.waittime);
                                int num = this.waittime / this.aspeed;
                                if (this.waittime % this.aspeed == 0)
                                {
                                    switch (num)
                                    {
                                        case 1:
                                            this.sound.PlaySE(SoundEffect.shoot);
                                            break;
                                        case 7:
                                            this.counterTiming = false;
                                            this.sound.PlaySE(SoundEffect.knife);
                                            this.ready = true;
                                            this.effecting = true;
                                            this.movestart = false;
                                            this.waittime = 0;
                                            this.attackProcess = 0;
                                            this.refrect = 0;
                                            this.movespeed.X = 5f;
                                            this.movespeed.Y = 3f;
                                            this.angleDOWN = this.position.Y == 0;
                                            this.angleLEFT = this.StandPanel.color == Panel.COLOR.blue;
                                            this.manymove = 0;
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                if (this.waittime % 3 == 0)
                                {
                                    ++this.animeflame;
                                    if (this.animeflame >= 3)
                                        this.animeflame = 0;
                                    this.animationpoint.X = 9 + this.animeflame;
                                }
                                if (this.manymove >= 8)
                                {
                                    bool flag = false;
                                    Point poji = new Point(this.position.X, this.position.Y + (this.angleDOWN ? 1 : -1));
                                    if (!this.InAreaCheck(poji) || this.parent.panel[poji.X, poji.Y].color == this.union && this.enemyaArea)
                                    {
                                        this.angleDOWN = !this.angleDOWN;
                                        flag = true;
                                    }
                                    poji = new Point(this.position.X + (this.angleLEFT ? -1 : 1), this.position.Y);
                                    if (!this.InAreaCheck(poji))
                                    {
                                        this.angleLEFT = !this.angleLEFT;
                                        flag = true;
                                    }
                                    poji = new Point(this.position.X + (this.angleLEFT ? -1 : 1), this.position.Y + (this.angleDOWN ? 1 : -1));
                                    this.manymove = 0;
                                    ++this.refrect;
                                    if (this.refrect >= 9)
                                    {
                                        this.roopmove = 0;
                                        this.attackProcess = 0;
                                        this.effecting = false;
                                        this.motion = NaviBase.MOTION.move;
                                        this.OgreSet();
                                    }
                                    if (flag)
                                        this.sound.PlaySE(SoundEffect.bound);
                                }
                                this.positionDirect.X += this.angleLEFT ? -this.movespeed.X : this.movespeed.X;
                                this.positionDirect.Y += this.angleDOWN ? this.movespeed.Y : -this.movespeed.Y;
                                ++this.manymove;
                                if (this.manymove == 4)
                                {
                                    this.position.X += this.angleLEFT ? -1 : 1;
                                    this.position.Y += this.angleDOWN ? 1 : -1;
                                    this.HitFlagReset();
                                }
                            }
                            ++this.waittime;
                            break;
                    }
                    break;
                case NaviBase.MOTION.move:
                    if (this.moveflame)
                        ++this.waittime;
                    this.animationpoint.X = 0;
                    ++this.roopmove;
                    this.Motion = NaviBase.MOTION.neutral;
                    if (this.ogres[0] == null)
                    {
                        this.MoveRandom(false, false);
                        if (this.ran != null && this.ran.flag)
                            this.MoveRandom(false, false, this.union, true);
                    }
                    else
                    {
                        this.positionre.X = this.union == Panel.COLOR.red ? 0 : 5;
                        this.position.Y = this.Random.Next(3);
                        this.MoveRandom(false, false);
                        if (this.Canmove(this.positionre, this.number))
                        {
                            this.parent.effects.Add(new MoveEnemy(this.sound, this.parent, this.position.X, this.position.Y));
                            this.position = this.positionre;
                            this.PositionDirectSet();
                        }
                        else
                        {
                            this.Motion = NaviBase.MOTION.neutral;
                            this.PositionDirectSet();
                        }
                    }
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
            this.FlameControl(2);
            this.MoveAftar();
        }

        private void OgreSet()
        {
            if (this.ogres[0] == null || this.ogres[0].flag && this.ogres[1].flag)
                return;
            ++this.noOgreTurn;
            if (this.noOgreTurn >= 4)
            {
                if (!this.ogres[0].flag)
                {
                    this.ogres[0] = new Ogre(this.sound, this.parent, this.union == Panel.COLOR.red ? 2 : 3, 0, 0, this.union, this.powers[2], true, this.version >= 4);
                    this.parent.objects.Add(this.ogres[0]);
                    this.noOgreTurn = 0;
                }
                if (!this.ogres[1].flag)
                {
                    this.ogres[1] = new Ogre(this.sound, this.parent, this.union == Panel.COLOR.red ? 1 : 4, 2, 0, this.union, this.powers[2], false, this.version >= 4);
                    this.parent.objects.Add(this.ogres[1]);
                    this.noOgreTurn = 0;
                }
            }
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
                if (this.ogres[0] != null)
                {
                    this.ogres[0].Break();
                    this.ogres[1].Break();
                }
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
            this.HPposition = new Vector2((int)this.positionDirect.X + 24, (int)this.positionDirect.Y - this.height / 2 + 94);
            this.Nameprint(dg, this.printNumber);
        }

        private Point AnimeNeutral(int waitflame)
        {
            return this.Return(new int[1], new int[1], 1, waitflame);
        }

        private Point AnimeReady(int waittime)
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
            }, new int[7] { 2, 3, 4, 5, 6, 7, 8 }, 0, waittime);
        }

        private Point AnimeBoomerang(int waittime)
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

        private Point AnimeHockey(int waittime)
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

        private enum ATTACK
        {
            Boomerang,
            Hockey
        }
    }
}

