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
    internal class Mossa : EnemyBase
    {
        private Mossa.MOTION motion = Mossa.MOTION.neutral;
        private Point targetArea;
        private bool spMove;

        public Mossa(IAudioEngine s, SceneBattle p, int pX, int pY, byte n, Panel.COLOR u, byte v)
          : base(s, p, pX, pY, n, u, v)
        {
            this.helpPosition.X = -8;
            for (int index = 0; index < this.dropchips.Length; ++index)
                this.dropchips[index] = new ChipFolder(this.sound);
            this.picturename = "mossa";
            this.race = EnemyBase.ENEMY.virus;
            this.Flying = false;
            this.wide = 48;
            this.height = 40;
            this.printNumber = false;
            switch (this.version)
            {
                case 0:
                    this.power = 100;
                    this.hp = 1400;
                    this.name = ShanghaiEXE.Translate("Enemy.MossaName1");
                    this.element = ChipBase.ELEMENT.leaf;
                    this.speed = 4;
                    break;
                case 1:
                    this.power = 40;
                    this.hp = 100;
                    this.name = ShanghaiEXE.Translate("Enemy.MossaName2");
                    this.element = ChipBase.ELEMENT.leaf;
                    this.speed = 7;
                    break;
                case 2:
                    this.power = 60;
                    this.hp = 140;
                    this.name = ShanghaiEXE.Translate("Enemy.MossaName3");
                    this.element = ChipBase.ELEMENT.heat;
                    this.speed = 6;
                    break;
                case 3:
                    this.power = 100;
                    this.hp = 220;
                    this.name = ShanghaiEXE.Translate("Enemy.MossaName4");
                    this.element = ChipBase.ELEMENT.poison;
                    this.speed = 5;
                    break;
                case 4:
                    this.name = ShanghaiEXE.Translate("Enemy.MossaName5");
                    this.printNumber = false;
                    this.power = 120;
                    this.hp = 300;
                    this.element = ChipBase.ELEMENT.eleki;
                    this.speed = 4;
                    break;
                default:
                    this.name = ShanghaiEXE.Translate("Enemy.MossaName6") + (version - 3).ToString();
                    this.power = 120 + (version - 4) * 40;
                    this.hp = 300 + (version - 4) * 40;
                    this.element = ChipBase.ELEMENT.eleki;
                    this.speed = 3;
                    break;
            }
            this.roop = n;
            this.hpmax = this.hp;
            this.hpprint = this.hp;
            this.printhp = true;
            this.effecting = false;
            this.PositionDirectSet();
            switch (this.version)
            {
                case 1:
                    this.dropchips[0].chip = new TrapNet(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new TrapNet(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new TrapNet(this.sound);
                    this.dropchips[2].codeNo = 1;
                    this.dropchips[3].chip = new TrapNet(this.sound);
                    this.dropchips[3].codeNo = 2;
                    this.dropchips[4].chip = new TrapNet(this.sound);
                    if (this.Random.Next(4) < 3)
                        this.dropchips[4].codeNo = 2;
                    else
                        this.dropchips[4].codeNo = 3;
                    this.havezenny = 300;
                    break;
                case 2:
                    this.dropchips[0].chip = new FireNet(this.sound);
                    this.dropchips[0].codeNo = 3;
                    this.dropchips[1].chip = new FireNet(this.sound);
                    this.dropchips[1].codeNo = 3;
                    this.dropchips[2].chip = new FireNet(this.sound);
                    this.dropchips[2].codeNo = 1;
                    this.dropchips[3].chip = new FireNet(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new FireNet(this.sound);
                    this.dropchips[4].codeNo = 2;
                    this.havezenny = 600;
                    break;
                case 3:
                    this.dropchips[0].chip = new PoisonNet(this.sound);
                    this.dropchips[0].codeNo = 3;
                    this.dropchips[1].chip = new PoisonNet(this.sound);
                    this.dropchips[1].codeNo = 3;
                    this.dropchips[2].chip = new PoisonNet(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new PoisonNet(this.sound);
                    this.dropchips[3].codeNo = 2;
                    this.dropchips[4].chip = new PoisonNet(this.sound);
                    this.dropchips[4].codeNo = 1;
                    this.havezenny = 800;
                    break;
                default:
                    this.dropchips[0].chip = new ElekiNet(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new ElekiNet(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new ElekiNet(this.sound);
                    this.dropchips[2].codeNo = 3;
                    this.dropchips[3].chip = new ElekiNet(this.sound);
                    this.dropchips[3].codeNo = 2;
                    this.dropchips[4].chip = new ElekiNet(this.sound);
                    this.dropchips[4].codeNo = 1;
                    this.havezenny = 1600;
                    break;
            }
            this.neutlal = true;
        }

        public override void PositionDirectSet()
        {
            this.positionDirect = new Vector2((float)(position.X * 40.0 + 14.0) - 2 * this.UnionRebirth, (float)(position.Y * 24.0 + 68.0));
        }

        protected override void Moving()
        {
            this.neutlal = this.motion == Mossa.MOTION.neutral;
            if (this.moveflame)
            {
                this.positionre = this.position;
                switch (this.motion)
                {
                    case Mossa.MOTION.neutral:
                        if (this.frame > 3)
                        {
                            if (this.roop > 3 - version && !this.badstatus[4] && this.parent.nowscene != SceneBattle.BATTLESCENE.end)
                            {
                                this.frame = 0;
                                if (this.version == 0)
                                {
                                    this.motion = Mossa.MOTION.spMove;
                                }
                                else
                                {
                                    this.motion = Mossa.MOTION.attack;
                                    this.counterTiming = true;
                                }
                            }
                            else
                                ++this.roop;
                            this.frame = 0;
                            break;
                        }
                        break;
                    case Mossa.MOTION.move:
                        this.motion = Mossa.MOTION.neutral;
                        this.MoveRandom(false, false);
                        if (this.position == this.positionre)
                        {
                            this.frame = 0;
                            this.motion = Mossa.MOTION.neutral;
                            break;
                        }
                        this.parent.effects.Add(new MoveEnemy(this.sound, this.parent, this.position.X, this.position.Y));
                        this.position = this.positionre;
                        this.PositionDirectSet();
                        this.frame = 0;
                        break;
                    case Mossa.MOTION.attack:
                        this.animationpoint.X = this.AnimeAttack(this.frame).X;
                        switch (this.frame)
                        {
                            case 0:
                                this.counterTiming = true;
                                break;
                            case 8:
                                if (this.parent.nowscene != SceneBattle.BATTLESCENE.end)
                                {
                                    this.counterTiming = false;
                                    this.sound.PlaySE(SoundEffect.bound);
                                    Point end = this.RandomPanel(this.UnionEnemy);
                                    this.parent.attacks.Add(new WebBomb(this.sound, this.parent, this.positionre.X, this.positionre.Y, this.union, this.Power, 1, new Vector2(this.positionDirect.X + (this.union == Panel.COLOR.red ? 9f : -9f), this.positionDirect.Y - 9f), end, 40, 120, this.element));
                                    break;
                                }
                                break;
                            case 18:
                                this.animationpoint.X = 0;
                                this.frame = 0;
                                this.roop = 0;
                                this.motion = Mossa.MOTION.move;
                                break;
                        }
                        break;
                    case Mossa.MOTION.spMove:
                        switch (this.frame)
                        {
                            case 1:
                                if (this.spMove)
                                {
                                    this.targetArea = this.RandomTarget();
                                    if (this.targetArea.Y == this.position.Y)
                                    {
                                        this.targetArea.Y = this.position.Y;
                                        if (this.targetArea.X < this.position.X)
                                            this.targetArea.X = this.position.X - 1;
                                        else if (this.targetArea.X > this.position.X)
                                            this.targetArea.X = this.position.X + 1;
                                        else
                                            this.targetArea = this.position;
                                    }
                                    else
                                    {
                                        this.targetArea.X = this.position.X;
                                        if (this.targetArea.Y < this.position.Y)
                                            this.targetArea.Y = this.position.Y - 1;
                                        else if (this.targetArea.Y > this.position.Y)
                                            this.targetArea.Y = this.position.Y + 1;
                                        else
                                            this.targetArea = this.position;
                                    }
                                }
                                else
                                {
                                    this.targetArea = this.RandomTarget();
                                    this.spMove = true;
                                }
                                this.parent.attacks.Add(new Dummy(this.sound, this.parent, this.targetArea.X, this.targetArea.Y, this.union, new Point(), this.speed * 8, true));
                                break;
                            case 8:
                                this.sound.PlaySE(SoundEffect.lance);
                                this.parent.effects.Add(new MoveEnemy(this.sound, this.parent, this.position.X, this.position.Y));
                                if (this.position.X < this.targetArea.X)
                                    this.rebirth = true;
                                else
                                    this.rebirth = false;
                                this.position = this.targetArea;
                                this.parent.attacks.Add(new WebTrap(this.sound, this.parent, this.position.X, this.position.Y, this.union, this.Power, 1, 150, this.element));
                                this.PositionDirectSet();
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
            this.HPposition = new Vector2(this.positionDirect.X + 6f, this.positionDirect.Y + 2f - this.height / 2);
            this.Nameprint(dg, this.printNumber);
        }

        private Point AnimeAttack(int waitflame)
        {
            return this.Return(new int[12]
            {
        0,
        1,
        2,
        3,
        7,
        8,
        12,
        13,
        14,
        15,
        16,
        17
            }, new int[11] { 0, 1, 2, 3, 4, 5, 4, 3, 2, 1, 0 }, 1, waitflame);
        }

        private enum MOTION
        {
            neutral,
            move,
            attack,
            spMove,
        }
    }
}

