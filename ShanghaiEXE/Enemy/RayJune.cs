using NSAttack;
using NSBattle;
using NSChip;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEffect;
using NSGame;
using Common.Vectors;
using System;
using System.Drawing;

namespace NSEnemy
{
    internal class RayJune : EnemyBase
    {
        private RayJune.MOTION motion = RayJune.MOTION.neutral;
        private readonly int nspeed;
        private readonly int moveroop;
        private int roopneutral;
        private int roopmove;

        public RayJune(IAudioEngine s, SceneBattle p, int pX, int pY, byte n, Panel.COLOR u, byte v)
          : base(s, p, pX, pY, n, u, v)
        {
            for (int index = 0; index < this.dropchips.Length; ++index)
                this.dropchips[index] = new ChipFolder(this.sound);
            this.element = ChipBase.ELEMENT.eleki;
            this.printNumber = true;
            this.name = ShanghaiEXE.Translate("Enemy.RayJuneName1");
            this.roopmove = n * -3;
            switch (this.version)
            {
                case 0:
                    this.printNumber = false;
                    this.name = ShanghaiEXE.Translate("Enemy.RayJuneName2");
                    this.power = 180;
                    this.hp = 2000;
                    this.nspeed = 6;
                    this.roopmove = 0;
                    break;
                case 1:
                    this.power = 40;
                    this.hp = 90;
                    this.moveroop = 3;
                    this.nspeed = 6;
                    break;
                case 2:
                    this.power = 80;
                    this.hp = 140;
                    this.moveroop = 3;
                    this.nspeed = 5;
                    break;
                case 3:
                    this.power = 150;
                    this.hp = 210;
                    this.moveroop = 3;
                    this.nspeed = 4;
                    break;
                case 4:
                    this.printNumber = false;
                    this.name = ShanghaiEXE.Translate("Enemy.RayJuneName3");
                    this.power = 180;
                    this.hp = 240;
                    this.nspeed = 3;
                    break;
                default:
                    this.printNumber = false;
                    this.name = ShanghaiEXE.Translate("Enemy.RayJuneName4") + (version - 3).ToString();
                    this.power = 180 + (version - 4) * 20;
                    this.hp = 240 + (version - 4) * 40;
                    this.nspeed = 2;
                    break;
            }
            this.picturename = "raijun";
            this.race = EnemyBase.ENEMY.virus;
            this.Flying = false;
            this.wide = 72;
            this.height = 48;
            this.hpmax = this.hp;
            this.speed = this.nspeed;
            this.hpprint = this.hp;
            this.printhp = true;
            this.effecting = false;
            this.PositionDirectSet();
            switch (this.version)
            {
                case 1:
                    this.dropchips[0].chip = new ElekiFang1(this.sound);
                    this.dropchips[0].codeNo = 2;
                    this.dropchips[1].chip = new ElekiFang1(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new ElekiFang1(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new ElekiFang1(this.sound);
                    this.dropchips[3].codeNo = 1;
                    this.dropchips[4].chip = new ElekiFang1(this.sound);
                    if (this.Random.Next(4) < 3)
                        this.dropchips[4].codeNo = 1;
                    else
                        this.dropchips[4].codeNo = 3;
                    this.havezenny = 300;
                    break;
                case 2:
                    this.dropchips[0].chip = new ElekiFang2(this.sound);
                    this.dropchips[0].codeNo = 1;
                    this.dropchips[1].chip = new ElekiFang2(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new ElekiFang2(this.sound);
                    this.dropchips[2].codeNo = 3;
                    this.dropchips[3].chip = new ElekiFang2(this.sound);
                    this.dropchips[3].codeNo = 2;
                    this.dropchips[4].chip = new ElekiFang2(this.sound);
                    this.dropchips[4].codeNo = 0;
                    this.havezenny = 600;
                    break;
                case 3:
                    this.dropchips[0].chip = new ElekiFang3(this.sound);
                    this.dropchips[0].codeNo = 1;
                    this.dropchips[1].chip = new ElekiFang3(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new ElekiFang3(this.sound);
                    this.dropchips[2].codeNo = 3;
                    this.dropchips[3].chip = new ElekiFang3(this.sound);
                    this.dropchips[3].codeNo = 2;
                    this.dropchips[4].chip = new ElekiFang3(this.sound);
                    this.dropchips[4].codeNo = 0;
                    this.havezenny = 800;
                    break;
                default:
                    this.dropchips[0].chip = new ElekiFang1(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new ElekiFang2(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new ElekiFang2(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new ElekiFang3(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new ElekiFang1(this.sound);
                    this.dropchips[4].codeNo = 3;
                    this.havezenny = 1500;
                    break;
            }
        }

        public override void PositionDirectSet()
        {
            this.positionDirect = new Vector2((float)(position.X * 40.0 + 16.0), (float)(position.Y * 24.0 + 64.0));
        }

        protected override void Moving()
        {
            this.neutlal = this.motion == RayJune.MOTION.neutral;
            switch (this.motion)
            {
                case RayJune.MOTION.neutral:
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
                                if (this.roopmove >= this.moveroop && !this.badstatus[4])
                                {
                                    this.speed = 4;
                                    this.motion = RayJune.MOTION.attack;
                                    this.counterTiming = true;
                                }
                                else
                                    this.motion = RayJune.MOTION.move;
                            }
                        }
                        break;
                    }
                    break;
                case RayJune.MOTION.move:
                    ++this.roopmove;
                    this.motion = RayJune.MOTION.neutral;
                    this.MoveRandom(false, false);
                    if (this.position == this.positionre)
                    {
                        this.motion = RayJune.MOTION.neutral;
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
                case RayJune.MOTION.attack:
                    this.animationpoint = this.AnimeAttack(this.frame);
                    if (this.moveflame)
                    {
                        if (this.frame == 1)
                        {
                            this.parent.attacks.Add(new Dummy(this.sound, this.parent, this.position.X + this.UnionRebirth, this.position.Y - 1, this.union, new Point(6, 0), 20, true));
                            this.parent.attacks.Add(new Dummy(this.sound, this.parent, this.position.X + this.UnionRebirth, this.position.Y, this.union, new Point(0, 0), 20, true));
                            this.parent.attacks.Add(new Dummy(this.sound, this.parent, this.position.X + this.UnionRebirth, this.position.Y + 1, this.union, new Point(6, 0), 20, true));
                        }
                        if (this.frame == 11)
                        {
                            this.sound.PlaySE(SoundEffect.thunder);
                            ElekiFang elekiFang = new ElekiFang(this.sound, this.parent, this.position.X + this.UnionRebirth, this.position.Y, this.union, this.Power, 1, this.element, false);
                            elekiFang.positionDirect.Y += 10f;
                            this.parent.attacks.Add(elekiFang);
                            this.counterTiming = false;
                        }
                        if (this.version == 0)
                        {
                            int num = 6;
                            if (this.frame == 11 + num)
                            {
                                this.sound.PlaySE(SoundEffect.thunder);
                                ElekiFang elekiFang = new ElekiFang(this.sound, this.parent, this.position.X + this.UnionRebirth, this.position.Y, this.union, this.Power, 1, this.element, true);
                                elekiFang.positionDirect.Y += 10f;
                                this.parent.attacks.Add(elekiFang);
                            }
                            if (this.frame == 11 + num * 2)
                            {
                                this.sound.PlaySE(SoundEffect.thunder);
                                ElekiFang elekiFang = new ElekiFang(this.sound, this.parent, this.position.X + this.UnionRebirth, this.position.Y, this.union, this.Power, 1, this.element, false);
                                elekiFang.positionDirect.Y += 10f;
                                this.parent.attacks.Add(elekiFang);
                            }
                            if (this.frame == 11 + num * 3)
                            {
                                this.sound.PlaySE(SoundEffect.thunder);
                                ElekiFang elekiFang = new ElekiFang(this.sound, this.parent, this.position.X + this.UnionRebirth, this.position.Y, this.union, this.Power, 1, this.element, true);
                                elekiFang.positionDirect.Y += 10f;
                                this.parent.attacks.Add(elekiFang);
                            }
                            if (this.frame == 11 + num * 4)
                            {
                                this.sound.PlaySE(SoundEffect.thunder);
                                ElekiFang elekiFang = new ElekiFang(this.sound, this.parent, this.position.X + this.UnionRebirth, this.position.Y, this.union, this.Power, 1, this.element, false);
                                elekiFang.positionDirect.Y += 10f;
                                this.parent.attacks.Add(elekiFang);
                            }
                            if (this.frame == 11 + num * 5)
                            {
                                this.sound.PlaySE(SoundEffect.thunder);
                                ElekiFang elekiFang = new ElekiFang(this.sound, this.parent, this.position.X + this.UnionRebirth, this.position.Y, this.union, this.Power, 1, this.element, true);
                                elekiFang.positionDirect.Y += 10f;
                                this.parent.attacks.Add(elekiFang);
                            }
                            if (this.frame == 11 + num * 6)
                            {
                                this.sound.PlaySE(SoundEffect.thunder);
                                ElekiFang elekiFang = new ElekiFang(this.sound, this.parent, this.position.X + this.UnionRebirth, this.position.Y, this.union, this.Power, 1, this.element, false);
                                elekiFang.positionDirect.Y += 10f;
                                this.parent.attacks.Add(elekiFang);
                            }
                            if (this.frame == 11 + num * 5)
                            {
                                this.frame = 0;
                                this.roopmove = 0;
                                this.roopneutral = 0;
                                this.speed = 6;
                                this.motion = RayJune.MOTION.neutral;
                            }
                        }
                        if (this.frame >= 30 && this.version > 0)
                        {
                            this.frame = 0;
                            this.roopmove = 0;
                            this.roopneutral = 0;
                            this.speed = this.nspeed;
                            this.motion = RayJune.MOTION.move;
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
            this._position = new Vector2((int)this.positionDirect.X + this.Shake.X, (int)this.positionDirect.Y + this.Shake.Y);
            this._rect = new Rectangle(this.animationpoint.X * this.wide, Math.Min(this.version, (byte)4) * this.height, this.wide, this.height);
            if (this.version == 0)
                this._rect.Y = this.height * 5;
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
            this.HPposition = new Vector2((int)this.positionDirect.X, (int)this.positionDirect.Y - this.height / 2 - 3);
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

        private Point AnimeAttack(int waitflame)
        {
            return this.Return(new int[21]
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
        11,
        12,
        13,
        14,
        15,
        16,
        17,
        18,
        19,
        100
            }, new int[21]
            {
        0,
        4,
        5,
        4,
        5,
        4,
        5,
        4,
        5,
        4,
        5,
        6,
        7,
        8,
        9,
        10,
        11,
        12,
        13,
        14,
        4
            }, 0, waitflame);
        }

        private enum MOTION
        {
            neutral,
            move,
            attack,
        }
    }
}

