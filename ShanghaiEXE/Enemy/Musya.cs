using NSAttack;
using NSBattle;
using NSBattle.Character;
using NSChip;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEffect;
using NSGame;
using Common.Vectors;
using System;
using System.Drawing;
using System.Linq;
using NSObject;

namespace NSEnemy
{
    internal class Musya : EnemyBase
    {
        private Musya.MOTION motion = Musya.MOTION.neutral;
        private readonly Brocla brocla;
        private readonly NSAttack.PoisonShot.TYPE type;
        private bool up;
        private bool slide;
        private readonly int slideSpeed;
        private Point potisionNeutral;
        private DammyEnemy dammy;

        public Musya(IAudioEngine s, SceneBattle p, int pX, int pY, byte n, Panel.COLOR u, byte v)
          : base(s, p, pX, pY, n, u, v)
        {
            this.helpPosition.Y = -4;
            this.wantedPosition.X = -16;
            for (int index = 0; index < this.dropchips.Length; ++index)
                this.dropchips[index] = new ChipFolder(this.sound);
            this.picturename = "musya";
            this.race = EnemyBase.ENEMY.virus;
            this.Flying = false;
            this.wide = 56;
            this.height = 64;
            switch (this.version)
            {
                case 0:
                    this.name = ShanghaiEXE.Translate("Enemy.MusyaName1");
                    this.power = 200;
                    this.hp = 1200;
                    this.slideSpeed = 14;
                    break;
                case 1:
                    this.name = ShanghaiEXE.Translate("Enemy.MusyaName2");
                    this.power = 80;
                    this.hp = 150;
                    this.slideSpeed = 10;
                    break;
                case 2:
                    this.name = ShanghaiEXE.Translate("Enemy.MusyaName3");
                    this.power = 100;
                    this.hp = 200;
                    this.slideSpeed = 12;
                    break;
                case 3:
                    this.name = ShanghaiEXE.Translate("Enemy.MusyaName4");
                    this.power = 120;
                    this.hp = 250;
                    this.slideSpeed = 14;
                    break;
                case 4:
                    this.name = ShanghaiEXE.Translate("Enemy.MusyaName5");
                    this.power = 180;
                    this.hp = 300;
                    this.slideSpeed = 16;
                    break;
                default:
                    this.name = ShanghaiEXE.Translate("Enemy.MusyaName6") + (version - 3).ToString();
                    this.power = 180 + (version - 4) * 20;
                    this.hp = 300 + (version - 4) * 50;
                    this.slideSpeed = 16;
                    break;
            }
            this.Noslip = false;
            this.roop = this.number;
            this.hpmax = this.hp;
            this.hpprint = this.hp;
            this.printhp = true;
            this.effecting = false;
            this.PositionDirectSet();
            switch (this.version)
            {
                case 0:
                    this.dropchips[0].chip = new BraveSword(this.sound);
                    this.dropchips[0].codeNo = 1;
                    this.dropchips[1].chip = new BraveSword(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new BraveSword(this.sound);
                    this.dropchips[2].codeNo = 3;
                    this.dropchips[3].chip = new BraveSword(this.sound);
                    this.dropchips[3].codeNo = 3;
                    this.dropchips[4].chip = new BraveSword(this.sound);
                    this.dropchips[4].codeNo = 2;
                    this.havezenny = 600;
                    this.speed = 1;
                    this.element = ChipBase.ELEMENT.eleki;
                    break;
                case 1:
                    this.dropchips[0].chip = new HeatSword(this.sound);
                    this.dropchips[0].codeNo = 1;
                    this.dropchips[1].chip = new HeatSword(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new HeatSword(this.sound);
                    this.dropchips[2].codeNo = 3;
                    this.dropchips[3].chip = new HeatSword(this.sound);
                    this.dropchips[3].codeNo = 3;
                    this.dropchips[4].chip = new HeatSword(this.sound);
                    if (this.Random.Next(4) < 3)
                        this.dropchips[4].codeNo = 2;
                    else
                        this.dropchips[4].codeNo = 3;
                    this.havezenny = 300;
                    this.speed = 8;
                    this.element = ChipBase.ELEMENT.heat;
                    break;
                case 2:
                    this.dropchips[0].chip = new LeafSword(this.sound);
                    this.dropchips[0].codeNo = 1;
                    this.dropchips[1].chip = new LeafSword(this.sound);
                    this.dropchips[1].codeNo = 3;
                    this.dropchips[2].chip = new LeafSword(this.sound);
                    this.dropchips[2].codeNo = 2;
                    this.dropchips[3].chip = new LeafSword(this.sound);
                    this.dropchips[3].codeNo = 2;
                    this.dropchips[4].chip = new LeafSword(this.sound);
                    this.dropchips[4].codeNo = 0;
                    this.havezenny = 600;
                    this.speed = 6;
                    this.element = ChipBase.ELEMENT.leaf;
                    break;
                case 3:
                    this.dropchips[0].chip = new IceSword(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new IceSword(this.sound);
                    this.dropchips[1].codeNo = 2;
                    this.dropchips[2].chip = new IceSword(this.sound);
                    this.dropchips[2].codeNo = 3;
                    this.dropchips[3].chip = new IceSword(this.sound);
                    this.dropchips[3].codeNo = 3;
                    this.dropchips[4].chip = new IceSword(this.sound);
                    this.dropchips[4].codeNo = 1;
                    this.havezenny = 800;
                    this.speed = 5;
                    this.element = ChipBase.ELEMENT.aqua;
                    break;
                default:
                    this.dropchips[0].chip = new BraveSword(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new BraveSword(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new BraveSword(this.sound);
                    this.dropchips[2].codeNo = 1;
                    this.dropchips[3].chip = new BraveSword(this.sound);
                    this.dropchips[3].codeNo = 2;
                    this.dropchips[4].chip = new BraveSword(this.sound);
                    this.dropchips[4].codeNo = 3;
                    this.havezenny = 800;
                    this.speed = 4;
                    this.element = ChipBase.ELEMENT.normal;
                    if (this.version >= 8)
                    {
                        this.dropchips[4].chip = new OmegaSaber(this.sound);
                        this.dropchips[4].codeNo = this.Random.Next(4);
                        this.havezenny = 8000;
                        break;
                    }
                    break;
            }
            this.neutlal = true;
        }

        public override void Init()
        {
            this.dammy = new DammyEnemy(this.sound, this.parent, this.position.X, this.position.Y, this, true)
            {
                nohit = true
            };
        }

        private void DammySet()
        {
            this.dammy.position = this.position;
            this.dammy.flag = true;
            this.dammy.nomove = true;
            this.dammy.effecting = true;
            if (!this.parent.enemys.Contains(this.dammy))
            {
                this.parent.enemys.Add(dammy);
            }
        }

        public override void PositionDirectSet()
        {
            this.positionDirect = new Vector2((float)(position.X * 40.0 + 14.0) + 10 * this.UnionRebirth, (float)(position.Y * 24.0 + 68.0));
        }

        protected override void Moving()
        {
            this.neutlal = this.motion == Musya.MOTION.neutral;
            if (this.moveflame)
            {
                this.positionre = this.position;
                switch (this.motion)
                {
                    case Musya.MOTION.neutral:
                        this.animationpoint = this.AnimeNeutral(this.frame);
                        switch (this.frame)
                        {
                            case 4:
                                this.dammy.flag = false;
                                break;
                            case 5:
                                this.frame = 0;
                                if (this.EnemySearch(this.position.Y))
                                {
                                    this.slide = true;
                                    this.potisionNeutral = this.position;
                                    this.DammySet();
                                    this.motion = Musya.MOTION.attack;
                                    this.sound.PlaySE(SoundEffect.shoot);
                                    this.effecting = true;
                                    break;
                                }
                                this.motion = Musya.MOTION.move;
                                break;
                        }
                        break;
                    case Musya.MOTION.move:
                        if (this.up)
                        {
                            if (this.Canmove(new Point(this.position.X, this.position.Y - 1), this.number))
                            {
                                this.parent.effects.Add(new MoveEnemy(this.sound, this.parent, this.position.X, this.position.Y));
                                --this.position.Y;
                                this.PositionDirectSet();
                                this.motion = Musya.MOTION.neutral;
                                break;
                            }
                            this.up = false;
                            this.motion = Musya.MOTION.neutral;
                            break;
                        }
                        if (this.Canmove(new Point(this.position.X, this.position.Y + 1), this.number))
                        {
                            this.parent.effects.Add(new MoveEnemy(this.sound, this.parent, this.position.X, this.position.Y));
                            ++this.position.Y;
                            this.PositionDirectSet();
                            this.motion = Musya.MOTION.neutral;
                        }
                        else
                        {
                            this.up = true;
                            this.motion = Musya.MOTION.neutral;
                        }
                        break;
                    case Musya.MOTION.attack:
                        if (!this.slide)
                        {
                            this.animationpoint = this.AnimeAttack(this.frame);
                            this.PositionDirectSet();
                            switch (this.frame)
                            {
                                case 3:
                                    this.sound.PlaySE(SoundEffect.sword);
                                    AttackBase attackBase;
                                    if (this.version > 0)
                                    {
                                        attackBase = new KnifeAttack(this.sound, this.parent, this.position.X + this.UnionRebirth, this.position.Y, this.union, this.Power, 4, this.element, false);
                                    }
                                    else
                                    {
                                        attackBase = new KnifeAttack(this.sound, this.parent, this.position.X + this.UnionRebirth, this.position.Y, this.union, this.Power, 4, this.element, false);
                                        attackBase.breaking = true;
                                    }
                                    this.parent.attacks.Add(attackBase);
                                    break;
                                case 4:
                                    this.counterTiming = false;
                                    break;
                                case 12:
                                    this.parent.effects.Add(new MoveEnemy(this.sound, this.parent, this.position.X, this.position.Y));
                                    this.frame = 0;
                                    this.animationpoint = this.AnimeNeutral(this.frame);
                                    this.motion = Musya.MOTION.move;
                                    this.position = this.potisionNeutral;
                                    this.PositionDirectSet();
                                    this.effecting = false;
                                    break;
                            }
                            break;
                        }
                        break;
                }
            }
            if (this.motion == Musya.MOTION.attack && this.slide)
            {
                this.animationpoint = this.AnimeMove(this.frame);
                if (!this.InAreaCheck(new Point(this.position.X + this.UnionRebirth, this.position.Y)))
                {
                    this.slide = false;
                    this.effecting = false;
                    this.frame = 0;
                    this.counterTiming = true;
                }
                else if (this.parent.AllHitter().Any(o => (o is ObjectBase || this.UnionEnemy == o.union) && o.position == new Point(this.position.X + this.UnionRebirth, this.position.Y))
                    || this.parent.panel[this.position.X + this.UnionRebirth, this.position.Y].Hole)
                {
                    this.slide = false;
                    this.frame = 0;
                    this.counterTiming = true;
                    this.effecting = false;
                }
                else
                {
                    if (this.SlideMove(slideSpeed, 0))
                    {
                        this.SlideMoveEnd();
                    }
                }
            }
            this.AttackMake(this.Power, 0, 0);
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
            {
                this.Death(this._rect, new Rectangle(this.animationpoint.X * this.wide, 0, this.wide, this.height), this._position, this.picturename);
                this.dammy.flag = false;
            }
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
            this.HPposition = new Vector2(this.positionDirect.X + 10f - 10 * this.UnionRebirth, (float)(positionDirect.Y + 2.0 - this.height / 2 + 8.0));
            this.Nameprint(dg, this.printNumber);
        }

        private bool EnemySearch(int Y)
        {
            foreach (CharacterBase characterBase in this.parent.AllChara())
            {
                if (characterBase.union == this.UnionEnemy && characterBase.position.Y == Y)
                    return true;
            }
            return false;
        }

        private Point AnimeNeutral(int waitflame)
        {
            return this.ReturnKai(new int[5] { 1, 1, 1, 1, 1 }, new int[5]
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
            return this.ReturnKai(new int[1] { 1 }, new int[1] { 3 }, 1, waitflame);
        }

        private Point AnimeAttack(int waitflame)
        {
            return this.ReturnKai(new int[5] { 1, 1, 1, 1, 100 }, new int[5]
            {
        3,
        4,
        5,
        6,
        7
            }, 1, waitflame);
        }

        private enum MOTION
        {
            neutral,
            move,
            attack,
        }
    }
}

