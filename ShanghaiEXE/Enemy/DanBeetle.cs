using NSAttack;
using NSBattle;
using NSBattle.Character;
using NSChip;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;
using Common.Vectors;
using System;
using System.Drawing;

namespace NSEnemy
{
    internal class DanBeetle : EnemyBase
    {
        private DanBeetle.MOTION motion = DanBeetle.MOTION.neutral;
        private bool up;
        private bool barrier;
        private float ymove;
        private readonly int nspeed;
        private readonly int moveroop;
        private int roopneutral;
        private int roopmove;

        public DanBeetle(IAudioEngine s, SceneBattle p, int pX, int pY, byte n, Panel.COLOR u, byte v)
          : base(s, p, pX, pY, n, u, v)
        {
            this.helpPosition.Y = -24;
            for (int index = 0; index < this.dropchips.Length; ++index)
                this.dropchips[index] = new ChipFolder(this.sound);
            this.element = ChipBase.ELEMENT.normal;
            switch (this.version)
            {
                case 0:
                    this.name = ShanghaiEXE.Translate("Enemy.DanBeetleName1");
                    this.barrierType = CharacterBase.BARRIER.PowerAura;
                    this.barierPower = 400;
                    this.barierTime = -1;
                    this.power = 200;
                    this.hp = 800;
                    this.moveroop = 2;
                    break;
                case 1:
                    this.name = ShanghaiEXE.Translate("Enemy.DanBeetleName2");
                    this.barrierType = CharacterBase.BARRIER.PowerAura;
                    this.barierPower = 10;
                    this.barierTime = -1;
                    this.power = 30;
                    this.hp = 60;
                    this.moveroop = 2;
                    break;
                case 2:
                    this.name = ShanghaiEXE.Translate("Enemy.DanBeetleName3");
                    this.barrierType = CharacterBase.BARRIER.PowerAura;
                    this.barierPower = 50;
                    this.barierTime = -1;
                    this.power = 45;
                    this.hp = 90;
                    this.moveroop = 2;
                    break;
                case 3:
                    this.name = ShanghaiEXE.Translate("Enemy.DanBeetleName4");
                    this.barrierType = CharacterBase.BARRIER.PowerAura;
                    this.barierPower = 100;
                    this.barierTime = -1;
                    this.power = 60;
                    this.hp = 140;
                    this.moveroop = 1;
                    break;
                case 4:
                    this.name = ShanghaiEXE.Translate("Enemy.DanBeetleName5");
                    this.barrierType = CharacterBase.BARRIER.PowerAura;
                    this.barierPower = 150;
                    this.barierTime = -1;
                    this.power = 90;
                    this.hp = 200;
                    break;
                default:
                    this.name = ShanghaiEXE.Translate("Enemy.DanBeetleName6") + (version - 3).ToString();
                    this.barrierType = CharacterBase.BARRIER.PowerAura;
                    this.barierPower = 200;
                    this.barierTime = -1;
                    this.power = 70 + (version - 4) * 20;
                    this.hp = 200 + (version - 4) * 40;
                    break;
            }
            this.picturename = "beetle";
            this.race = EnemyBase.ENEMY.virus;
            this.Flying = false;
            this.wide = 96;
            this.height = 64;
            this.hpmax = this.hp;
            this.nspeed = 5 - Math.Min((int)this.version, 4);
            this.speed = this.nspeed;
            this.hpprint = this.hp;
            this.printhp = true;
            this.effecting = false;
            this.roopmove = n * 3;
            this.PositionDirectSet();
            this.positionDirect = new Vector2((float)(position.X * 40.0 + 16.0), (float)(position.Y * 24.0 + 56.0));
            switch (this.version)
            {
                case 0:
                    this.dropchips[0].chip = new AuraSword1(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new AuraSword2(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new AuraSword2(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new AuraSword3(this.sound);
                    this.dropchips[3].codeNo = 2;
                    this.dropchips[4].chip = new AuraSword1(this.sound);
                    this.dropchips[4].codeNo = 3;
                    this.havezenny = 1500;
                    this.nspeed = 2;
                    this.speed = this.nspeed;
                    this.roopmove = 2;
                    break;
                case 1:
                    this.dropchips[0].chip = new AuraSword1(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new AuraSword1(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new AuraSword1(this.sound);
                    this.dropchips[2].codeNo = 1;
                    this.dropchips[3].chip = new AuraSword1(this.sound);
                    this.dropchips[3].codeNo = 2;
                    this.dropchips[4].chip = new AuraSword1(this.sound);
                    if (this.Random.Next(4) < 3)
                        this.dropchips[4].codeNo = 2;
                    else
                        this.dropchips[4].codeNo = 3;
                    this.havezenny = 300;
                    break;
                case 2:
                    this.dropchips[0].chip = new AuraSword2(this.sound);
                    this.dropchips[0].codeNo = 1;
                    this.dropchips[1].chip = new AuraSword2(this.sound);
                    this.dropchips[1].codeNo = 3;
                    this.dropchips[2].chip = new AuraSword2(this.sound);
                    this.dropchips[2].codeNo = 2;
                    this.dropchips[3].chip = new AuraSword2(this.sound);
                    this.dropchips[3].codeNo = 2;
                    this.dropchips[4].chip = new AuraSword2(this.sound);
                    this.dropchips[4].codeNo = 0;
                    this.havezenny = 600;
                    break;
                case 3:
                    this.dropchips[0].chip = new AuraSword3(this.sound);
                    this.dropchips[0].codeNo = 1;
                    this.dropchips[1].chip = new AuraSword3(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new AuraSword3(this.sound);
                    this.dropchips[2].codeNo = 2;
                    this.dropchips[3].chip = new AuraSword3(this.sound);
                    this.dropchips[3].codeNo = 3;
                    this.dropchips[4].chip = new AuraSword3(this.sound);
                    this.dropchips[4].codeNo = 0;
                    this.havezenny = 800;
                    break;
                case 8:
                    this.dropchips[0].chip = new AuraSword1(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new AuraSword2(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new AuraSword2(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new AuraSword3(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new AuraSwordX(this.sound);
                    this.dropchips[4].codeNo = this.Random.Next(4);
                    this.havezenny = 8000;
                    break;
                default:
                    this.dropchips[0].chip = new AuraSword1(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new AuraSword2(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new AuraSword2(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new AuraSword3(this.sound);
                    this.dropchips[3].codeNo = 2;
                    this.dropchips[4].chip = new AuraSword1(this.sound);
                    this.dropchips[4].codeNo = 3;
                    this.havezenny = 1500;
                    break;
            }
        }

        public override void PositionDirectSet()
        {
            if (this.motion == DanBeetle.MOTION.move)
                return;
            this.positionDirect = new Vector2((float)(position.X * 40.0 + 16.0), (float)(position.Y * 24.0 + 56.0));
        }

        protected override void Moving()
        {
            this.neutlal = this.motion == DanBeetle.MOTION.neutral;
            if (this.moveflame && this.version == 0 && this.barierPower < 200)
                ++this.barierPower;
            switch (this.motion)
            {
                case DanBeetle.MOTION.neutral:
                    if (this.moveflame)
                    {
                        this.animationpoint = this.AnimeNeutral(this.frame);
                        if (this.frame >= 4)
                        {
                            this.frame = 0;
                            ++this.roopneutral;
                            if (this.roopneutral >= 2 && this.parent.nowscene != SceneBattle.BATTLESCENE.end)
                            {
                                this.roopneutral = 0;
                                if (this.roopmove > this.moveroop && !this.badstatus[4])
                                {
                                    this.speed = 4;
                                    this.motion = DanBeetle.MOTION.attack;
                                    this.counterTiming = true;
                                }
                                else
                                {
                                    if (this.up && this.position.Y == 0)
                                        this.up = false;
                                    else if (!this.up && this.position.Y >= 2)
                                        this.up = true;
                                    if (this.up)
                                    {
                                        this.positionre = new Point(this.position.X, this.position.Y - 1);
                                        this.ymove = (float)(16.0 / (4 * this.speed) * -1.0);
                                    }
                                    else
                                    {
                                        this.positionre = new Point(this.position.X, this.position.Y + 1);
                                        this.ymove = 16f / (4 * this.speed);
                                    }
                                    if (this.Canmove(this.positionre, this.number) && this.roopmove <= 3 && !this.HeviSand)
                                    {
                                        this.motion = DanBeetle.MOTION.move;
                                        ++this.roopmove;
                                    }
                                    else
                                    {
                                        this.up = !this.up;
                                        this.positionre = this.position;
                                        this.ymove = 0.0f;
                                        this.motion = DanBeetle.MOTION.attack;
                                        this.counterTiming = true;
                                    }
                                }
                            }
                        }
                        break;
                    }
                    break;
                case DanBeetle.MOTION.move:
                    if (this.moveflame)
                    {
                        this.animationpoint = this.AnimeMove(this.frame);
                        if (this.frame == 7)
                        {
                            this.positionDirect = new Vector2((float)(position.X * 40.0 + 16.0), (float)(position.Y * 24.0 + 56.0));
                            this.motion = DanBeetle.MOTION.neutral;
                            this.frame = 0;
                        }
                    }
                    this.positionDirect.Y += this.ymove;
                    this.position.Y = this.Calcposition(this.positionDirect, 64, true).Y;
                    break;
                case DanBeetle.MOTION.attack:
                    this.animationpoint = this.AnimeAttack(this.frame);
                    if (this.moveflame)
                    {
                        if (this.frame == 9)
                        {
                            this.sound.PlaySE(SoundEffect.sword);
                            if ((uint)this.barrierType > 0U)
                            {
                                if (this.version == 0)
                                    this.parent.attacks.Add(new SonicBoomMini(this.sound, this.parent, this.position.X + this.UnionRebirth, this.position.Y, this.union, this.Power, 5, this.element, true));
                                else
                                    this.parent.attacks.Add(new SonicBoomMini(this.sound, this.parent, this.position.X + this.UnionRebirth, this.position.Y, this.union, this.Power, 1 + version * 2, this.element, true));
                            }
                            else
                                this.parent.attacks.Add(new KnifeAttack(this.sound, this.parent, this.position.X + this.UnionRebirth, this.position.Y, this.union, this.Power, this.speed, ChipBase.ELEMENT.normal, false));
                            this.counterTiming = false;
                        }
                        if (this.frame >= 13)
                        {
                            if (this.barrierType == CharacterBase.BARRIER.None && this.barrier)
                            {
                                if (this.version == 0)
                                {
                                    this.barrierType = CharacterBase.BARRIER.PowerAura;
                                    this.barierPower = 10;
                                    this.barierTime = -1;
                                }
                                else
                                {
                                    this.barrierType = CharacterBase.BARRIER.Barrier;
                                    this.barierPower = 1;
                                    this.barierTime = -1;
                                    this.barrier = false;
                                }
                            }
                            else
                                this.barrier = true;
                            this.frame = 0;
                            this.roopmove = 0;
                            this.roopneutral = 0;
                            this.speed = this.nspeed;
                            this.motion = DanBeetle.MOTION.neutral;
                        }
                        break;
                    }
                    break;
            }
            this.FlameControl();
            this.MoveAftar();
        }

        public override void Render(IRenderer dg)
        {
            this._position = new Vector2((int)this.positionDirect.X + 4 + 4 * this.UnionRebirth + this.Shake.X, (int)this.positionDirect.Y + this.Shake.Y + 24);
            this._rect = new Rectangle(this.animationpoint.X * this.wide, 6 * this.height, this.wide, this.height);
            dg.DrawImage(dg, this.picturename, this._rect, false, this._position, this.rebirth, this.color);
            this._position = new Vector2((int)this.positionDirect.X + 4 + 4 * this.UnionRebirth + this.Shake.X, (int)this.positionDirect.Y + this.Shake.Y);
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
                this._rect.Y = this.animationpoint.Y;
                dg.DrawImage(dg, this.picturename, this._rect, false, this._position, this.rebirth, this.color);
            }
            this.HPposition = new Vector2((int)this.positionDirect.X, (int)this.positionDirect.Y - this.height / 2 + 32);
            this.Nameprint(dg, this.printNumber);
        }

        private Point AnimeNeutral(int waitflame)
        {
            return this.Return(new int[1], new int[1], 1, waitflame);
        }

        private Point AnimeMove(int waitflame)
        {
            return this.Return(new int[4] { 0, 1, 2, 6 }, new int[4]
            {
        1,
        2,
        1,
        0
            }, 0, waitflame);
        }

        private Point AnimeAttack(int waitflame)
        {
            return this.Return(new int[10]
            {
        0,
        1,
        7,
        8,
        9,
        10,
        11,
        12,
        13,
        14
            }, new int[9] { 3, 4, 5, 6, 7, 8, 9, 10, 11 }, 0, waitflame);
        }

        public override void BarrierRend(IRenderer dg)
        {
            this.BarrierRender(dg, new Vector2(this.positionDirect.X, this.positionDirect.Y + 24f));
        }

        public override void BarrierPowerRend(IRenderer dg)
        {
            this.BarrierPowerRender(dg, new Vector2(this.positionDirect.X, this.positionDirect.Y + 24f));
        }

        private enum MOTION
        {
            neutral,
            move,
            attack,
        }
    }
}

