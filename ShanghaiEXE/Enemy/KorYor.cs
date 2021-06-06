using NSAttack;
using NSBattle;
using NSChip;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;
using Common.Vectors;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace NSEnemy
{
    internal class KorYor : EnemyBase
    {
        private KorYor.MOTION motion = KorYor.MOTION.neutral;
        private readonly List<Point> target = new List<Point>();
        private bool targetset;
        private int count;

        public KorYor(IAudioEngine s, SceneBattle p, int pX, int pY, byte n, Panel.COLOR u, byte v)
          : base(s, p, pX, pY, n, u, v)
        {
            this.helpPosition.X = -8;
            for (int index = 0; index < this.dropchips.Length; ++index)
                this.dropchips[index] = new ChipFolder(this.sound);
            this.picturename = nameof(KorYor);
            this.race = EnemyBase.ENEMY.virus;
            this.Flying = false;
            this.wide = 40;
            this.height = 32;
            this.name = ShanghaiEXE.Translate("Enemy.KorYorName1");
            this.printNumber = true;
            switch (this.version)
            {
                case 0:
                    this.name = ShanghaiEXE.Translate("Enemy.KorYorName2");
                    this.printNumber = false;
                    this.power = 120;
                    this.hp = 1500;
                    this.speed = 1;
                    break;
                case 1:
                    this.power = 60;
                    this.hp = 120;
                    this.speed = 6;
                    break;
                case 2:
                    this.power = 80;
                    this.hp = 140;
                    this.speed = 4;
                    break;
                case 3:
                    this.power = 100;
                    this.hp = 250;
                    this.speed = 3;
                    break;
                case 4:
                    this.name = ShanghaiEXE.Translate("Enemy.KorYorName3");
                    this.printNumber = false;
                    this.power = 120;
                    this.hp = 270;
                    this.speed = 2;
                    break;
                default:
                    this.name = ShanghaiEXE.Translate("Enemy.KorYorName4") + (version - 3).ToString();
                    this.printNumber = false;
                    this.power = 120 + (version - 4) * 40;
                    this.hp = 270 + (version - 4) * 40;
                    this.speed = 2;
                    break;
            }
            this.roop = this.number;
            this.element = ChipBase.ELEMENT.leaf;
            this.hpmax = this.hp;
            this.hpprint = this.hp;
            this.printhp = true;
            this.effecting = false;
            this.PositionDirectSet();
            switch (this.version)
            {
                case 1:
                    this.dropchips[0].chip = new KarehaWave1(this.sound);
                    this.dropchips[0].codeNo = 2;
                    this.dropchips[1].chip = new KarehaWave1(this.sound);
                    this.dropchips[1].codeNo = 2;
                    this.dropchips[2].chip = new KarehaWave1(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new KarehaWave1(this.sound);
                    this.dropchips[3].codeNo = 1;
                    this.dropchips[4].chip = new KarehaWave1(this.sound);
                    if (this.Random.Next(4) < 3)
                        this.dropchips[4].codeNo = 1;
                    else
                        this.dropchips[4].codeNo = 3;
                    this.havezenny = 300;
                    break;
                case 2:
                    this.dropchips[0].chip = new KarehaWave2(this.sound);
                    this.dropchips[0].codeNo = 1;
                    this.dropchips[1].chip = new KarehaWave2(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new KarehaWave2(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new KarehaWave2(this.sound);
                    this.dropchips[3].codeNo = 2;
                    this.dropchips[4].chip = new KarehaWave2(this.sound);
                    this.dropchips[4].codeNo = 3;
                    this.havezenny = 600;
                    break;
                case 3:
                    this.dropchips[0].chip = new KarehaWave3(this.sound);
                    this.dropchips[0].codeNo = 1;
                    this.dropchips[1].chip = new KarehaWave3(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new KarehaWave3(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new KarehaWave3(this.sound);
                    this.dropchips[3].codeNo = 3;
                    this.dropchips[4].chip = new KarehaWave3(this.sound);
                    this.dropchips[4].codeNo = 2;
                    this.havezenny = 800;
                    break;
                default:
                    this.dropchips[0].chip = new KarehaWave1(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new KarehaWave1(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new KarehaWave2(this.sound);
                    this.dropchips[2].codeNo = 2;
                    this.dropchips[3].chip = new KarehaWave3(this.sound);
                    this.dropchips[3].codeNo = 2;
                    this.dropchips[4].chip = new KarehaWave1(this.sound);
                    this.dropchips[4].codeNo = 3;
                    this.havezenny = 1600;
                    break;
            }
            this.neutlal = true;
        }

        public override void PositionDirectSet()
        {
            this.positionDirect = new Vector2((float)(position.X * 40.0 + 14.0), (float)(position.Y * 24.0 + 72.0));
        }

        protected override void Moving()
        {
            this.neutlal = this.motion == KorYor.MOTION.neutral;
            if (this.moveflame)
            {
                this.positionre = this.position;
                switch (this.motion)
                {
                    case KorYor.MOTION.neutral:
                        if (this.frame > 3)
                        {
                            if (this.version != 0) { }
                            if (this.roop > 3 - version && !this.badstatus[4] && this.parent.nowscene != SceneBattle.BATTLESCENE.end)
                            {
                                this.frame = 0;
                                this.counterTiming = true;
                                this.motion = KorYor.MOTION.attack;
                            }
                            else
                                ++this.roop;
                            this.frame = 0;
                            break;
                        }
                        break;
                    case KorYor.MOTION.attack:
                        if (this.animationpoint.X < 3)
                            ++this.animationpoint.X;
                        if (!this.targetset)
                        {
                            int x = this.position.X;
                            int y = this.position.Y;
                            int num1 = 0;
                            this.target.Clear();
                            bool flag = false;
                            int num2 = 0;
                            while (this.InAreaCheck(new Point(x + num1 * this.UnionRebirth, y)))
                            {
                                this.target.Add(new Point(x + num1 * this.UnionRebirth, y));
                                if (flag)
                                {
                                    if (y <= 0)
                                    {
                                        ++num1;
                                        flag = !flag;
                                    }
                                    else
                                        --y;
                                }
                                else if (y >= 2)
                                {
                                    ++num1;
                                    flag = !flag;
                                }
                                else
                                    ++y;
                                ++num2;
                            }
                            this.targetset = true;
                        }
                        if (this.count >= 3)
                            this.counterTiming = false;
                        if (this.count >= this.target.Count)
                        {
                            this.count = 0;
                            this.animationpoint.X = 0;
                            this.targetset = false;
                            this.motion = KorYor.MOTION.neutral;
                            this.frame = 0;
                            this.roop = 0;
                            break;
                        }
                        if (this.frame > 1)
                        {
                            this.sound.PlaySE(SoundEffect.lance);
                            if (this.version == 0)
                                this.parent.attacks.Add(new LeafWave(this.sound, this.parent, this.target[this.count].X, this.target[this.count].Y, this.union, this.Power, 1, 5));
                            else
                                this.parent.attacks.Add(new LeafWave(this.sound, this.parent, this.target[this.count].X, this.target[this.count].Y, this.union, this.Power, 1, 30 / version));
                            ++this.count;
                            if (this.count >= this.target.Count)
                            {
                                this.count = 0;
                                this.animationpoint.X = 0;
                                this.motion = KorYor.MOTION.neutral;
                                this.targetset = false;
                                this.roop = 0;
                            }
                            this.frame = 0;
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
            this.HPposition = new Vector2(this.positionDirect.X + 6f, (float)(positionDirect.Y + 2.0 - this.height / 2 + 32.0));
            this.Nameprint(dg, this.printNumber);
        }

        private Point AnimeAttack(int waitflame)
        {
            return this.Return(new int[4] { 0, 1, 2, 3 }, new int[4]
            {
        0,
        1,
        2,
        3
            }, 1, waitflame);
        }

        private enum MOTION
        {
            neutral,
            attack,
        }
    }
}

