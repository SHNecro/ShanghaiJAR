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
    internal class Ikary : EnemyBase
    {
        private readonly int[] zSpeed = new int[39]
        {
      0,
      0,
      1,
      0,
      1,
      0,
      1,
      1,
      1,
      1,
      2,
      2,
      2,
      2,
      4,
      4,
      4,
      4,
      4,
      4,
      4,
      4,
      4,
      4,
      4,
      2,
      2,
      2,
      2,
      1,
      1,
      1,
      1,
      0,
      1,
      0,
      1,
      0,
      0
        };
        private Ikary.MOTION motion = Ikary.MOTION.neutral;
        private Shadow shadow;
        private int z;
        private int zCount;
        private bool zdown;
        private new int roop;

        public Ikary(IAudioEngine s, SceneBattle p, int pX, int pY, byte n, Panel.COLOR u, byte v)
          : base(s, p, pX, pY, n, u, v)
        {
            this.helpPosition.X = -8;
            for (int index = 0; index < this.dropchips.Length; ++index)
                this.dropchips[index] = new ChipFolder(this.sound);
            this.picturename = "SquAnchor";
            this.race = EnemyBase.ENEMY.virus;
            this.Flying = false;
            this.wide = 48;
            this.height = 56;
            this.printNumber = false;
            this.element = ChipBase.ELEMENT.aqua;
            switch (this.version)
            {
                case 0:
                    this.name = ShanghaiEXE.Translate("Enemy.IkaryName1");
                    this.printNumber = false;
                    this.power = 180;
                    this.hp = 2100;
                    this.speed = 3;
                    break;
                case 1:
                    this.name = ShanghaiEXE.Translate("Enemy.IkaryName2");
                    this.power = 60;
                    this.hp = 120;
                    this.speed = 4;
                    break;
                case 2:
                    this.name = ShanghaiEXE.Translate("Enemy.IkaryName3");
                    this.power = 90;
                    this.hp = 160;
                    this.speed = 4;
                    break;
                case 3:
                    this.name = ShanghaiEXE.Translate("Enemy.IkaryName4");
                    this.power = 130;
                    this.hp = 200;
                    this.speed = 3;
                    break;
                case 4:
                    this.name = ShanghaiEXE.Translate("Enemy.IkaryName5");
                    this.printNumber = false;
                    this.power = 180;
                    this.hp = 250;
                    this.speed = 3;
                    break;
                default:
                    this.name = ShanghaiEXE.Translate("Enemy.IkaryName6") + (version - 3).ToString();
                    this.printNumber = false;
                    this.power = 180 + (version - 4) * 40;
                    this.hp = 300 + (version - 4) * 40;
                    this.speed = 3;
                    break;
            }
            this.roop = this.number;
            this.hpmax = this.hp;
            this.hpprint = this.hp;
            this.printhp = true;
            this.effecting = false;
            this.Flying = true;
            this.PositionDirectSet();
            switch (this.version)
            {
                case 1:
                    this.dropchips[0].chip = new HeavyAnchor1(this.sound);
                    this.dropchips[0].codeNo = 2;
                    this.dropchips[1].chip = new HeavyAnchor1(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new HeavyAnchor1(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new HeavyAnchor1(this.sound);
                    this.dropchips[3].codeNo = 1;
                    this.dropchips[4].chip = new HeavyAnchor1(this.sound);
                    if (this.Random.Next(4) < 3)
                        this.dropchips[4].codeNo = 1;
                    else
                        this.dropchips[4].codeNo = 3;
                    this.havezenny = 300;
                    break;
                case 2:
                    this.dropchips[0].chip = new HeavyAnchor2(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new HeavyAnchor2(this.sound);
                    this.dropchips[1].codeNo = 3;
                    this.dropchips[2].chip = new HeavyAnchor2(this.sound);
                    this.dropchips[2].codeNo = 3;
                    this.dropchips[3].chip = new HeavyAnchor2(this.sound);
                    this.dropchips[3].codeNo = 2;
                    this.dropchips[4].chip = new HeavyAnchor2(this.sound);
                    this.dropchips[4].codeNo = 1;
                    this.havezenny = 600;
                    break;
                case 3:
                    this.dropchips[0].chip = new HeavyAnchor3(this.sound);
                    this.dropchips[0].codeNo = 1;
                    this.dropchips[1].chip = new HeavyAnchor3(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new HeavyAnchor3(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new HeavyAnchor3(this.sound);
                    this.dropchips[3].codeNo = 3;
                    this.dropchips[4].chip = new HeavyAnchor3(this.sound);
                    this.dropchips[4].codeNo = 2;
                    this.havezenny = 800;
                    break;
                default:
                    this.dropchips[0].chip = new HeavyAnchor1(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new HeavyAnchor2(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new HeavyAnchor1(this.sound);
                    this.dropchips[2].codeNo = 2;
                    this.dropchips[3].chip = new HeavyAnchor2(this.sound);
                    this.dropchips[3].codeNo = 2;
                    this.dropchips[4].chip = new HeavyAnchor3(this.sound);
                    this.dropchips[4].codeNo = 3;
                    this.havezenny = 1600;
                    break;
            }
            this.neutlal = true;
        }

        public override void PositionDirectSet()
        {
            this.positionDirect = new Vector2((float)(position.X * 40.0 + 14.0), (float)(position.Y * 24.0 + 56.0));
        }

        public override void InitAfter()
        {
            this.shadow = new Shadow(this.sound, this.parent, this.position.X, this.position.Y, this);
            this.parent.effects.Add(shadow);
        }

        protected override void Moving()
        {
            this.neutlal = this.motion == Ikary.MOTION.neutral;
            this.shadow.position = this.position;
            this.shadow.PositionDirectSet(this.position);
            switch (this.motion)
            {
                case Ikary.MOTION.neutral:
                    this.z += this.zSpeed[this.zCount] * (!this.zdown ? -1 : 1);
                    ++this.zCount;
                    if (this.zCount >= this.zSpeed.Length)
                    {
                        this.zCount = 0;
                        this.zdown = !this.zdown;
                        if (!this.zdown)
                            ++this.roop;
                    }
                    this.nohit = this.z <= -40;
                    this.frame = 0;
                    this.MoveAftar();
                    break;
                case Ikary.MOTION.attack:
                    if (this.effecting && !this.nohit)
                    {
                        this.AttackMake(this.Power, 0, 0);
                        break;
                    }
                    break;
            }
            if (this.moveflame)
            {
                this.positionre = this.position;
                switch (this.motion)
                {
                    case Ikary.MOTION.neutral:
                        if ((this.roop > 3 || this.version == 0) && !this.badstatus[4] && this.parent.nowscene != SceneBattle.BATTLESCENE.end)
                        {
                            this.counterTiming = false;
                            this.frame = 0;
                            this.motion = Ikary.MOTION.attack;
                            this.effecting = true;
                            this.nohit = true;
                            this.z = -32;
                            this.height = 128;
                            this.parent.effects.Add(new MoveEnemy(this.sound, this.parent, this.position.X, this.position.Y));
                            this.position = this.RandomTarget();
                            this.PositionDirectSet();
                            this.animationpoint.X = 1;
                            break;
                        }
                        break;
                    case Ikary.MOTION.attack:
                        this.animationpoint.X = this.AnimeAttack(this.frame).X;
                        int sp = 4;
                        switch (this.frame)
                        {
                            case 12:
                                this.nohit = false;
                                this.Sound.PlaySE(SoundEffect.clincher);
                                this.counterTiming = true;
                                this.parent.effects.Add(new Water(this.sound, this.parent, this.position.X, this.position.Y, sp));
                                BombAttack bombAttack = new BombAttack(this.sound, this.parent, this.position.X, this.position.Y, this.union, this.Power, 1, this.element);
                                bombAttack.breaking = true;
                                this.parent.attacks.Add(bombAttack);
                                if (!this.StandPanel.Hole)
                                {
                                    this.ShakeStart(5, 30);
                                    if (this.version > 1 || this.version == 0)
                                    {
                                        this.parent.effects.Add(new Water(this.sound, this.parent, this.position.X, this.position.Y - 1, sp));
                                        this.parent.attacks.Add(new BombAttack(this.sound, this.parent, this.position.X, this.position.Y - 1, this.union, this.Power, 1, this.element));
                                        this.parent.effects.Add(new Water(this.sound, this.parent, this.position.X, this.position.Y + 1, sp));
                                        this.parent.attacks.Add(new BombAttack(this.sound, this.parent, this.position.X, this.position.Y + 1, this.union, this.Power, 1, this.element));
                                    }
                                    if (this.version > 2 || this.version == 0)
                                    {
                                        this.parent.effects.Add(new Water(this.sound, this.parent, this.position.X + 1, this.position.Y, sp));
                                        this.parent.attacks.Add(new BombAttack(this.sound, this.parent, this.position.X + 1, this.position.Y, this.union, this.Power, 1, this.element));
                                        this.parent.effects.Add(new Water(this.sound, this.parent, this.position.X - 1, this.position.Y, sp));
                                        this.parent.attacks.Add(new BombAttack(this.sound, this.parent, this.position.X - 1, this.position.Y, this.union, this.Power, 1, this.element));
                                    }
                                }
                                this.StandPanel.Crack();
                                break;
                            case 20:
                                this.counterTiming = false;
                                break;
                            case 25:
                                if (this.version == 0)
                                {
                                    this.counterTiming = false;
                                    this.frame = 0;
                                    this.motion = Ikary.MOTION.attack;
                                    this.effecting = true;
                                    this.nohit = true;
                                    this.z = -32;
                                    this.height = 128;
                                    this.parent.effects.Add(new MoveEnemy(this.sound, this.parent, this.position.X, this.position.Y));
                                    this.position = this.RandomTarget();
                                    this.PositionDirectSet();
                                    this.animationpoint.X = 1;
                                    break;
                                }
                                break;
                            case 40:
                                this.motion = Ikary.MOTION.neutral;
                                this.frame = 0;
                                this.effecting = false;
                                this.nohit = false;
                                this.z = 0;
                                this.height = 56;
                                this.roop = 0;
                                this.animationpoint.X = 0;
                                this.parent.effects.Add(new MoveEnemy(this.sound, this.parent, this.position.X, this.position.Y));
                                this.MoveRandom(false, false, this.union, false);
                                this.position = this.positionre;
                                this.PositionDirectSet();
                                break;
                        }
                        break;
                }
            }
            this.FlameControl();
        }

        public override void Render(IRenderer dg)
        {
            this._position = new Vector2((int)this.positionDirect.X + 6 + this.Shake.X, (int)this.positionDirect.Y + 2 + this.Shake.Y + this.z);
            this._rect = new Rectangle(this.animationpoint.X * this.wide, Math.Min(this.version, (byte)4) * 128, this.wide, this.height);
            if (this.version == 0)
                this._rect.Y = 640;
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
            this.HPposition = new Vector2(this.positionDirect.X + 6f, (float)(positionDirect.Y + 2.0 - this.height / 2 + 64.0));
            this.Nameprint(dg, this.printNumber);
        }

        private Point AnimeAttack(int waitflame)
        {
            return this.ReturnKai(new int[9]
            {
        0,
        5,
        1,
        1,
        1,
        1,
        1,
        1,
        100
            }, new int[9] { 1, 1, 2, 3, 4, 5, 6, 7, 8 }, 1, waitflame);
        }

        private enum MOTION
        {
            neutral,
            attack,
        }
    }
}

