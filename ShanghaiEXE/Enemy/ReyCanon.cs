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
using System.Drawing;

namespace NSEnemy
{
    internal class ReyCanon : EnemyBase
    {
        private ReyCanon.MOTION motion = ReyCanon.MOTION.neutral;
        private new bool rockon = false;
        private byte fire = 0;
        private Point targetpoint;

        public ReyCanon(IAudioEngine s, SceneBattle p, int pX, int pY, byte n, Panel.COLOR u, byte v)
          : base(s, p, pX, pY, n, u, v)
        {
            for (int index = 0; index < this.dropchips.Length; ++index)
                this.dropchips[index] = new ChipFolder(this.sound);
            this.name = ShanghaiEXE.Translate("Enemy.ReyCanonName1");
            this.printNumber = true;
            this.picturename = "reycanon";
            this.race = EnemyBase.ENEMY.virus;
            this.Flying = false;
            this.hp = 60 * version;
            this.hpmax = this.hp;
            this.power = 15 + 15 * ((version - 1) * 2);
            this.wide = 40;
            this.height = 56;
            this.version = v;
            this.speed = 9 - Math.Min(version * 2, 8);
            this.hpprint = this.hp;
            this.printhp = true;
            this.effecting = false;
            if (this.parent != null)
                this.roop = (byte)(parent.manyenemys - (uint)this.number);
            this.PositionDirectSet();
            switch (this.version)
            {
                case 0:
                    this.name = ShanghaiEXE.Translate("Enemy.ReyCanonName2");
                    this.hp = 1000;
                    this.hpmax = this.hp;
                    this.power = 100;
                    this.hpprint = this.hp;
                    this.speed = 4;
                    this.printNumber = false;
                    this.dropchips[0].chip = new Reygun(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new MegaReygun(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new GigaReygun(this.sound);
                    this.dropchips[2].codeNo = 2;
                    this.dropchips[3].chip = new Reygun(this.sound);
                    this.dropchips[3].codeNo = 3;
                    this.dropchips[4].chip = new MegaReygun(this.sound);
                    this.dropchips[4].codeNo = 3;
                    this.speed = 2;
                    this.havezenny = 1500;
                    break;
                case 1:
                    this.dropchips[0].chip = new Reygun(this.sound);
                    this.dropchips[0].codeNo = 2;
                    this.dropchips[1].chip = new Reygun(this.sound);
                    this.dropchips[1].codeNo = 2;
                    this.dropchips[2].chip = new Reygun(this.sound);
                    this.dropchips[2].codeNo = 1;
                    this.dropchips[3].chip = new Reygun(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new Reygun(this.sound);
                    if (this.Random.Next(4) < 3)
                        this.dropchips[4].codeNo = 0;
                    else
                        this.dropchips[4].codeNo = 3;
                    this.havezenny = 350;
                    break;
                case 2:
                    this.dropchips[0].chip = new MegaReygun(this.sound);
                    this.dropchips[0].codeNo = 2;
                    this.dropchips[1].chip = new MegaReygun(this.sound);
                    this.dropchips[1].codeNo = 2;
                    this.dropchips[2].chip = new MegaReygun(this.sound);
                    this.dropchips[2].codeNo = 1;
                    this.dropchips[3].chip = new MegaReygun(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new MegaReygun(this.sound);
                    if (this.Random.Next(4) < 3)
                        this.dropchips[4].codeNo = 0;
                    else
                        this.dropchips[4].codeNo = 3;
                    this.havezenny = 600;
                    break;
                case 3:
                    this.dropchips[0].chip = new GigaReygun(this.sound);
                    this.dropchips[0].codeNo = 2;
                    this.dropchips[1].chip = new GigaReygun(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new GigaReygun(this.sound);
                    this.dropchips[2].codeNo = 3;
                    this.dropchips[3].chip = new GigaReygun(this.sound);
                    this.dropchips[3].codeNo = 1;
                    this.dropchips[4].chip = new GigaReygun(this.sound);
                    this.dropchips[4].codeNo = 1;
                    this.havezenny = 800;
                    break;
                case 4:
                    this.name = ShanghaiEXE.Translate("Enemy.ReyCanonName3");
                    this.printNumber = false;
                    this.dropchips[0].chip = new Reygun(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new MegaReygun(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new GigaReygun(this.sound);
                    this.dropchips[2].codeNo = 2;
                    this.dropchips[3].chip = new Reygun(this.sound);
                    this.dropchips[3].codeNo = 3;
                    this.dropchips[4].chip = new MegaReygun(this.sound);
                    this.dropchips[4].codeNo = 3;
                    this.speed = 2;
                    this.havezenny = 1500;
                    break;
                default:
                    this.name = ShanghaiEXE.Translate("Enemy.ReyCanonName4") + (version - 3).ToString();
                    this.printNumber = false;
                    this.dropchips[0].chip = new Reygun(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new MegaReygun(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new GigaReygun(this.sound);
                    this.dropchips[2].codeNo = 2;
                    this.dropchips[3].chip = new Reygun(this.sound);
                    this.dropchips[3].codeNo = 3;
                    this.dropchips[4].chip = new MegaReygun(this.sound);
                    this.dropchips[4].codeNo = 3;
                    this.speed = 2;
                    this.havezenny = 1500;
                    break;
            }
            this.neutlal = true;
        }

        public override void PositionDirectSet()
        {
            this.positionDirect = new Vector2((float)(position.X * 40.0 + 20.0), (float)(position.Y * 24.0 + 60.0));
        }

        protected override void Moving()
        {
            this.neutlal = this.motion == ReyCanon.MOTION.neutral;
            switch (this.motion)
            {
                case ReyCanon.MOTION.neutral:
                    ++this.frame;
                    if (this.frame > 60 && this.parent.nowscene != SceneBattle.BATTLESCENE.end && !this.badstatus[4])
                    {
                        this.motion = ReyCanon.MOTION.search;
                        this.targetpoint = this.position;
                        this.targetpoint.X += this.union == Panel.COLOR.blue ? -1 : 1;
                        this.frame = 0;
                        break;
                    }
                    break;
                case ReyCanon.MOTION.search:
                    if (this.version > 0)
                    {
                        if (this.parent.player.position == this.targetpoint && this.union == Panel.COLOR.blue)
                            this.rockon = true;
                    }
                    else if (this.parent.player.position.X == this.targetpoint.X && this.union == Panel.COLOR.blue)
                        this.rockon = true;
                    foreach (EnemyBase enemy in this.parent.enemys)
                    {
                        if (this.version > 0)
                        {
                            if (enemy.position == this.targetpoint && this.union != enemy.union)
                                this.rockon = true;
                        }
                        else if (enemy.position.X == this.targetpoint.X && this.union != enemy.union)
                        {
                            this.rockon = true;
                            this.targetpoint.Y = enemy.position.Y;
                        }
                    }
                    foreach (ObjectBase objectBase in this.parent.objects)
                    {
                        if (this.version > 0)
                        {
                            if (objectBase.position == this.targetpoint && this.union != objectBase.union)
                                this.rockon = true;
                        }
                        else if (objectBase.position.X == this.targetpoint.X && this.union != objectBase.union)
                        {
                            this.rockon = true;
                            this.targetpoint.Y = this.position.Y;
                        }
                    }
                    if (this.frame == 3)
                    {
                        this.frame = 0;
                        if (this.rockon)
                        {
                            this.counterTiming = true;
                            this.sound.PlaySE(SoundEffect.rockon);
                            this.motion = ReyCanon.MOTION.attack;
                            this.fire = 0;
                        }
                        else
                        {
                            this.sound.PlaySE(SoundEffect.search);
                            this.targetpoint.X += this.union == Panel.COLOR.blue ? -1 : 1;
                            if (this.targetpoint.X >= 6 || this.targetpoint.X < 0)
                                this.motion = ReyCanon.MOTION.neutral;
                        }
                    }
                    this.FlameControl();
                    break;
                case ReyCanon.MOTION.attack:
                    if (this.frame < 4)
                        this.animationpoint.X = this.frame;
                    else
                        this.animationpoint.X = 0;
                    if (this.moveflame)
                    {
                        switch (this.frame)
                        {
                            case 1:
                                this.sound.PlaySE(SoundEffect.canon);
                                this.ShakeStart(5, 5);
                                this.parent.attacks.Add(new BombAttack(this.sound, this.parent, this.targetpoint.X, this.targetpoint.Y, this.union, this.Power, 1, this.element));
                                this.parent.effects.Add(new Bomber(this.sound, this.parent, this.targetpoint.X, this.targetpoint.Y, Bomber.BOMBERTYPE.bomber, 2));
                                if (this.version == 0)
                                {
                                    this.parent.attacks.Add(new BombAttack(this.sound, this.parent, this.targetpoint.X, this.targetpoint.Y - 1, this.union, this.Power, 1, this.element));
                                    this.parent.effects.Add(new Bomber(this.sound, this.parent, this.targetpoint.X, this.targetpoint.Y - 1, Bomber.BOMBERTYPE.bomber, 2));
                                    this.parent.attacks.Add(new BombAttack(this.sound, this.parent, this.targetpoint.X, this.targetpoint.Y + 1, this.union, this.Power, 1, this.element));
                                    this.parent.effects.Add(new Bomber(this.sound, this.parent, this.targetpoint.X, this.targetpoint.Y + 1, Bomber.BOMBERTYPE.bomber, 2));
                                }
                                this.parent.effects.Add(new BulletBigShells(this.sound, this.parent, this.position, this.positionDirect.X - 16 * this.UnionRebirth, this.positionDirect.Y - 16f, 32, this.union, 40 + this.Random.Next(20), 2, 0));
                                break;
                            case 4:
                                this.rockon = false;
                                this.motion = ReyCanon.MOTION.neutral;
                                this.counterTiming = false;
                                break;
                        }
                    }
                    ++this.fire;
                    this.FlameControl();
                    break;
            }
            this.MoveAftar();
        }

        public override void Render(IRenderer dg)
        {
            this._position = new Vector2((int)this.positionDirect.X + this.Shake.X, (int)this.positionDirect.Y + this.Shake.Y);
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
            this.HPposition = new Vector2(this.positionDirect.X, this.positionDirect.Y - 32f);
            if (this.motion == ReyCanon.MOTION.attack)
            {
                this._position = this.positionDirect;
                this._position.X += this.union == Panel.COLOR.blue ? -22f : 22f;
                this._position.Y -= 12f;
                this._rect = new Rectangle(fire / 3 * 64, 32, 64, 64);
                dg.DrawImage(dg, "shot", this._rect, false, this._position, this.union == Panel.COLOR.blue, Color.White);
            }
            if ((uint)this.motion > 0U)
            {
                this._position = new Vector2(this.targetpoint.X * 40 + 20, this.targetpoint.Y * 24 + 70);
                this._rect = this.motion == ReyCanon.MOTION.search ? new Rectangle(160 + this.frame * 32, 0, 32, 32) : new Rectangle(288, 0, 32, 32);
                dg.DrawImage(dg, this.picturename, this._rect, false, this._position, this.rebirth, this.color);
                if (this.version == 0)
                {
                    this._position = new Vector2(this.targetpoint.X * 40 + 20, (this.targetpoint.Y - 1) * 24 + 70);
                    this._rect = this.motion == ReyCanon.MOTION.search ? new Rectangle(160 + this.frame * 32, 0, 32, 32) : new Rectangle(288, 0, 32, 32);
                    dg.DrawImage(dg, this.picturename, this._rect, false, this._position, this.rebirth, this.color);
                    this._position = new Vector2(this.targetpoint.X * 40 + 20, (this.targetpoint.Y + 1) * 24 + 70);
                    this._rect = this.motion == ReyCanon.MOTION.search ? new Rectangle(160 + this.frame * 32, 0, 32, 32) : new Rectangle(288, 0, 32, 32);
                    dg.DrawImage(dg, this.picturename, this._rect, false, this._position, this.rebirth, this.color);
                }
            }
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
            search,
            attack,
        }
    }
}

