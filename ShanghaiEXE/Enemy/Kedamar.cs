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
    internal class Kedamar : EnemyBase
    {
        private Kedamar.MOTION motion = Kedamar.MOTION.neutral;
        private float ymove;
        private float xmove;
        private readonly byte wavespeed;
        private float xplus;

        public Kedamar(IAudioEngine s, SceneBattle p, int pX, int pY, byte n, Panel.COLOR u, byte v)
          : base(s, p, pX, pY, n, u, v)
        {
            for (int index = 0; index < this.dropchips.Length; ++index)
                this.dropchips[index] = new ChipFolder(this.sound);
            this.name = ShanghaiEXE.Translate("Enemy.KedamarName1");
            this.picturename = "kedamar";
            this.race = EnemyBase.ENEMY.virus;
            this.Flying = false;
            this.wide = 34;
            this.height = 34;
            this.hp = 40 * version;
            this.hpmax = this.hp;
            this.power = 10 + 30 * (version - 1);
            this.speed = 5 - version;
            this.hpprint = this.hp;
            this.printhp = true;
            this.printNumber = true;
            this.effecting = false;
            this.roop = (byte)this.number;
            this.PositionDirectSet();
            this.wavespeed = Math.Max((byte)(7 - (version - 1) * 3), (byte)1);
            switch (this.version)
            {
                case 0:
                    this.name = ShanghaiEXE.Translate("Enemy.KedamarName2");
                    this.printNumber = false;
                    this.hp = 800;
                    this.hpmax = this.hp;
                    this.hpprint = this.hp;
                    this.power = 160;
                    this.speed = 3;
                    this.wavespeed = 2;
                    this.dropchips[0].chip = new GroundWave(this.sound);
                    this.dropchips[0].codeNo = 2;
                    this.dropchips[1].chip = new GroundWave(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new GroundWave(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new GroundWave(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new GroundWave(this.sound);
                    this.dropchips[4].codeNo = 3;
                    this.havezenny = 800;
                    break;
                case 1:
                    this.speed = 4;
                    this.dropchips[0].chip = new ShotWave(this.sound);
                    this.dropchips[0].codeNo = 2;
                    this.dropchips[1].chip = new ShotWave(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new ShotWave(this.sound);
                    this.dropchips[2].codeNo = 1;
                    this.dropchips[3].chip = new ShotWave(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new ShotWave(this.sound);
                    if (this.Random.Next(4) < 3)
                        this.dropchips[4].codeNo = 0;
                    else
                        this.dropchips[4].codeNo = 3;
                    this.havezenny = 300;
                    break;
                case 2:
                    this.speed = 3;
                    this.dropchips[0].chip = new ShootWave(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new ShootWave(this.sound);
                    this.dropchips[1].codeNo = 3;
                    this.dropchips[2].chip = new ShootWave(this.sound);
                    this.dropchips[2].codeNo = 1;
                    this.dropchips[3].chip = new ShootWave(this.sound);
                    this.dropchips[3].codeNo = 1;
                    this.dropchips[4].chip = new ShootWave(this.sound);
                    this.dropchips[4].codeNo = 2;
                    this.havezenny = 600;
                    break;
                case 3:
                    this.speed = 3;
                    this.wavespeed = 2;
                    this.dropchips[0].chip = new GroundWave(this.sound);
                    this.dropchips[0].codeNo = 2;
                    this.dropchips[1].chip = new GroundWave(this.sound);
                    this.dropchips[1].codeNo = 3;
                    this.dropchips[2].chip = new GroundWave(this.sound);
                    this.dropchips[2].codeNo = 3;
                    this.dropchips[3].chip = new GroundWave(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new GroundWave(this.sound);
                    this.dropchips[4].codeNo = 1;
                    this.havezenny = 800;
                    break;
                case 4:
                    this.name = ShanghaiEXE.Translate("Enemy.KedamarName3");
                    this.hp = 300;
                    this.hpmax = this.hp;
                    this.hpprint = this.hp;
                    this.printNumber = false;
                    this.wavespeed = 1;
                    this.speed = 2;
                    this.dropchips[0].chip = new GigantWave(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new GigantWave(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new GigantWave(this.sound);
                    this.dropchips[2].codeNo = 3;
                    this.dropchips[3].chip = new GigantWave(this.sound);
                    this.dropchips[3].codeNo = 2;
                    this.dropchips[4].chip = new GigantWave(this.sound);
                    this.dropchips[4].codeNo = 1;
                    this.havezenny = 800;
                    break;
                default:
                    this.name = ShanghaiEXE.Translate("Enemy.KedamarName4") + (version - 3).ToString();
                    this.hp = 300 + (version - 4) * 20;
                    this.hpmax = this.hp;
                    this.hpprint = this.hp;
                    this.printNumber = false;
                    this.wavespeed = 1;
                    this.speed = 2;
                    this.dropchips[0].chip = new GigantWave(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new GigantWave(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new GigantWave(this.sound);
                    this.dropchips[2].codeNo = 3;
                    this.dropchips[3].chip = new GigantWave(this.sound);
                    this.dropchips[3].codeNo = 2;
                    this.dropchips[4].chip = new GigantWave(this.sound);
                    this.dropchips[4].codeNo = 1;
                    this.havezenny = 800;
                    break;
            }
        }

        public override void PositionDirectSet()
        {
            this.positionDirect = new Vector2((float)(position.X * 40.0 + 20.0), (float)(position.Y * 24.0 + 72.0));
        }

        protected override void Moving()
        {
            this.neutlal = this.motion == Kedamar.MOTION.neutral;
            switch (this.motion)
            {
                case Kedamar.MOTION.neutral:
                    if (this.moveflame)
                    {
                        this.animationpoint = this.AnimeNeutral(this.frame);
                        if (this.frame == 8)
                        {
                            this.frame = 0;
                            this.roop += (byte)this.Random.Next(1, 2);
                            if (this.parent.nowscene != SceneBattle.BATTLESCENE.end && !this.badstatus[4])
                            {
                                if (this.roop > parent.manyenemys + 1 || this.version == 0 || this.union == Panel.COLOR.red)
                                {
                                    this.roop = 0;
                                    bool flag = false;
                                    foreach (CharacterBase characterBase in this.parent.AllChara())
                                    {
                                        if (characterBase.union != this.union && characterBase.position.Y == this.position.Y)
                                        {
                                            flag = true;
                                            break;
                                        }
                                    }
                                    if (flag)
                                    {
                                        this.motion = Kedamar.MOTION.attack;
                                        this.counterTiming = true;
                                    }
                                    else
                                    {
                                        if (this.position.Y > this.RandomTarget().Y)
                                        {
                                            this.positionre = new Point(this.position.X, this.position.Y - 1);
                                            this.ymove = (float)(24.0 / (7 * this.speed) * -1.0);
                                        }
                                        else
                                        {
                                            this.positionre = new Point(this.position.X, this.position.Y + 1);
                                            this.ymove = 24f / (7 * this.speed);
                                        }
                                        if (this.Canmove(this.positionre, this.number) && !this.HeviSand)
                                        {
                                            this.motion = Kedamar.MOTION.move;
                                        }
                                        else
                                        {
                                            this.positionre = this.position;
                                            this.ymove = 0.0f;
                                            this.motion = Kedamar.MOTION.attack;
                                            this.counterTiming = true;
                                        }
                                    }
                                }
                            }
                            else
                                this.roop = 0;
                        }
                        break;
                    }
                    break;
                case Kedamar.MOTION.move:
                    if (this.moveflame)
                    {
                        this.animationpoint = this.AnimeMove(this.frame);
                        if (this.frame == 7)
                        {
                            if (this.version == 0)
                                this.parent.attacks.Add(new WaveAttsck(this.sound, this.parent, this.union == Panel.COLOR.blue ? this.position.X - 1 : this.position.X + 1, this.position.Y, this.union, !this.badstatus[1] ? this.power : this.power / 2, wavespeed, this.version >= 3 || this.version == 0 ? 1 : 0, this.element));
                            this.motion = Kedamar.MOTION.neutral;
                            this.frame = 0;
                        }
                    }
                    this.positionDirect.Y += this.ymove;
                    this.position = this.Calcposition(this.positionDirect, 34, true);
                    break;
                case Kedamar.MOTION.attack:
                    if (this.moveflame)
                    {
                        if (this.frame <= 6)
                        {
                            this.xmove = 24f / (6 * this.speed);
                            if (this.union == Panel.COLOR.blue)
                                this.xmove *= -1f;
                        }
                        else if (this.frame > 17)
                        {
                            this.xmove = 24f / (8 * this.speed);
                            this.xmove *= -1f;
                            if (this.union == Panel.COLOR.blue)
                                this.xmove *= -1f;
                        }
                        else
                            this.xmove = 0.0f;
                        this.animationpoint = this.AnimeAttack(this.frame);
                        if (this.frame == 10)
                        {
                            this.parent.attacks.Add(new WaveAttsck(this.sound, this.parent, this.union == Panel.COLOR.blue ? this.position.X - 1 : this.position.X + 1, this.position.Y, this.union, !this.badstatus[1] ? this.power : this.power / 2, wavespeed, this.version >= 3 || this.version == 0 ? 1 : 0, this.element));
                            this.counterTiming = false;
                        }
                        if (this.frame == 25)
                        {
                            this.motion = Kedamar.MOTION.neutral;
                            this.frame = 0;
                        }
                    }
                    if (this.motion == Kedamar.MOTION.attack)
                    {
                        this.xplus += this.xmove;
                        break;
                    }
                    this.xplus = 0.0f;
                    break;
            }
            this.FlameControl();
            this.MoveAftar();
        }

        public override void Render(IRenderer dg)
        {
            this._position = new Vector2((int)this.positionDirect.X + (int)this.xplus + this.Shake.X, (int)this.positionDirect.Y + this.Shake.Y);
            this._rect = new Rectangle(this.animationpoint.X * this.wide, this.animationpoint.Y * this.height + Math.Min(this.version, (byte)4) * 68, this.wide, this.height);
            if (this.version == 0)
                this._rect.Y = this.animationpoint.Y * this.height + 340;
            if (this.Hp <= 0)
                this.Death(this._rect, new Rectangle(this.animationpoint.X * this.wide, this.animationpoint.Y * this.height, this.wide, this.height), this._position, this.picturename);
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
                this._rect.Y = this.animationpoint.Y * 34;
                dg.DrawImage(dg, this.picturename, this._rect, false, this._position, this.rebirth, this.color);
            }
            this.HPposition = new Vector2((int)this.positionDirect.X + (int)this.xplus, (int)this.positionDirect.Y + this.height / 2);
            this.Nameprint(dg, this.printNumber);
        }

        private Point AnimeNeutral(int waitflame)
        {
            return this.Return(new int[5] { 0, 2, 4, 6, 8 }, new int[5]
            {
        0,
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
            move,
            attack,
        }
    }
}

