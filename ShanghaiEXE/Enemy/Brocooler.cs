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
    internal class Brocooler : EnemyBase
    {
        private Brocooler.MOTION motion = Brocooler.MOTION.neutral;
        private readonly int nspeed;
        private Brocla brocla;
        private Brocla brocla2;
        private readonly NSAttack.PoisonShot.TYPE type;
        private bool wind;

        public Brocooler(IAudioEngine s, SceneBattle p, int pX, int pY, byte n, Panel.COLOR u, byte v)
          : base(s, p, pX, pY, n, u, v)
        {
            this.helpPosition.Y = -4;
            for (int index = 0; index < this.dropchips.Length; ++index)
                this.dropchips[index] = new ChipFolder(this.sound);
            this.picturename = nameof(Brocooler);
            this.race = EnemyBase.ENEMY.virus;
            this.Flying = false;
            this.wide = 56;
            this.height = 48;
            switch (this.version)
            {
                case 0:
                    this.name = ShanghaiEXE.Translate("Enemy.BrocoolerName1");
                    this.power = 200;
                    this.hp = 1700;
                    break;
                case 1:
                    this.name = ShanghaiEXE.Translate("Enemy.BrocoolerName2");
                    this.power = 100;
                    this.hp = 150;
                    break;
                case 2:
                    this.name = ShanghaiEXE.Translate("Enemy.BrocoolerName3");
                    this.power = 130;
                    this.hp = 200;
                    break;
                case 3:
                    this.name = ShanghaiEXE.Translate("Enemy.BrocoolerName4");
                    this.power = 150;
                    this.hp = 250;
                    break;
                case 4:
                    this.name = ShanghaiEXE.Translate("Enemy.BrocoolerName5");
                    this.power = 200;
                    this.hp = 300;
                    break;
                default:
                    this.name = ShanghaiEXE.Translate("Enemy.BrocoolerName6") + (version - 3).ToString();
                    this.power = 200 + (version - 4) * 20;
                    this.hp = 300 + (version - 4) * 50;
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
                    this.dropchips[0].chip = new BrocraLink1(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new BrocraLink1(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new BrocraLink1(this.sound);
                    this.dropchips[2].codeNo = 2;
                    this.dropchips[3].chip = new BrocraLink1(this.sound);
                    this.dropchips[3].codeNo = 2;
                    this.dropchips[4].chip = new BrocraLink1(this.sound);
                    this.dropchips[4].codeNo = 3;
                    this.havezenny = 600;
                    this.speed = 6;
                    this.nspeed = this.speed;
                    this.element = ChipBase.ELEMENT.leaf;
                    break;
                case 1:
                    this.dropchips[0].chip = new BrocraLink1(this.sound);
                    this.dropchips[0].codeNo = 1;
                    this.dropchips[1].chip = new BrocraLink1(this.sound);
                    this.dropchips[1].codeNo = 2;
                    this.dropchips[2].chip = new BrocraLink1(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new BrocraLink1(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new BrocraLink1(this.sound);
                    if (this.Random.Next(4) < 3)
                        this.dropchips[4].codeNo = 0;
                    else
                        this.dropchips[4].codeNo = 3;
                    this.havezenny = 300;
                    this.speed = 10;
                    this.element = ChipBase.ELEMENT.normal;
                    break;
                case 2:
                    this.dropchips[0].chip = new BrocraLink2(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new BrocraLink2(this.sound);
                    this.dropchips[1].codeNo = 2;
                    this.dropchips[2].chip = new BrocraLink2(this.sound);
                    this.dropchips[2].codeNo = 2;
                    this.dropchips[3].chip = new BrocraLink2(this.sound);
                    this.dropchips[3].codeNo = 3;
                    this.dropchips[4].chip = new BrocraLink2(this.sound);
                    this.dropchips[4].codeNo = 1;
                    this.havezenny = 600;
                    this.speed = 8;
                    this.element = ChipBase.ELEMENT.heat;
                    break;
                case 3:
                    this.dropchips[0].chip = new BrocraLink3(this.sound);
                    this.dropchips[0].codeNo = 2;
                    this.dropchips[1].chip = new BrocraLink3(this.sound);
                    this.dropchips[1].codeNo = 2;
                    this.dropchips[2].chip = new BrocraLink3(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new BrocraLink3(this.sound);
                    this.dropchips[3].codeNo = 3;
                    this.dropchips[4].chip = new BrocraLink3(this.sound);
                    this.dropchips[4].codeNo = 1;
                    this.havezenny = 800;
                    this.speed = 6;
                    this.element = ChipBase.ELEMENT.aqua;
                    break;
                case 8:
                    this.dropchips[0].chip = new BrocraLink1(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new BrocraLink2(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new BrocraLink2(this.sound);
                    this.dropchips[2].codeNo = 2;
                    this.dropchips[3].chip = new BrocraLink3(this.sound);
                    this.dropchips[3].codeNo = 2;
                    this.dropchips[4].chip = new BrocraLinkX(this.sound);
                    this.dropchips[4].codeNo = this.Random.Next(4);
                    this.havezenny = 8000;
                    break;
                default:
                    this.dropchips[0].chip = new BrocraLink1(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new BrocraLink2(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new BrocraLink2(this.sound);
                    this.dropchips[2].codeNo = 2;
                    this.dropchips[3].chip = new BrocraLink3(this.sound);
                    this.dropchips[3].codeNo = 2;
                    this.dropchips[4].chip = new BrocraLink1(this.sound);
                    this.dropchips[4].codeNo = 3;
                    this.havezenny = 800;
                    this.speed = 4;
                    this.element = ChipBase.ELEMENT.poison;
                    break;
            }
            this.neutlal = true;
        }

        public override void PositionDirectSet()
        {
            this.positionDirect = new Vector2((float)(position.X * 40.0 + 14.0), (float)(position.Y * 24.0 + 66.0));
        }

        protected override void Moving()
        {
            this.neutlal = this.motion == Brocooler.MOTION.neutral;
            if (this.moveflame)
            {
                this.positionre = this.position;
                switch (this.motion)
                {
                    case Brocooler.MOTION.neutral:
                        this.animationpoint = this.AnimeNeutral(this.frame);
                        switch (this.frame)
                        {
                            case 4:
                            case 10:
                            case 16:
                            case 22:
                                this.sound.PlaySE(SoundEffect.search);
                                break;
                            case 20:
                                this.counterTiming = true;
                                break;
                        }
                        if (this.frame == 16 && this.version == 0)
                        {
                            this.parent.effects.RemoveAll(e => e is NSEffect.BackWind);
                            this.parent.effects.RemoveAll(e => e is NSEffect.PushWind);
                        }
                        if (this.frame > 25)
                        {
                            if (!this.badstatus[4] && this.parent.nowscene != SceneBattle.BATTLESCENE.end)
                            {
                                if (this.parent.objects.Count <= 0 || this.version > 0)
                                {
                                    this.motion = Brocooler.MOTION.attack;
                                }
                                else
                                {
                                    this.waittime = 0;
                                    this.speed = 1;
                                    this.motion = Brocooler.MOTION.okataduke;
                                }
                            }
                            else
                                ++this.roop;
                            this.frame = 0;
                            break;
                        }
                        break;
                    case Brocooler.MOTION.attack:
                        if ((uint)this.frame <= 1U)
                        {
                            if (this.parent.nowscene != SceneBattle.BATTLESCENE.end)
                            {
                                this.MoveRandom(false, false, this.UnionEnemy, 1);
                                if (this.positionre != this.position)
                                {
                                    Panel.PANEL panel;
                                    switch (this.version)
                                    {
                                        case 0:
                                            panel = Panel.PANEL._grass;
                                            this.parent.effects.Add(new Smoke(this.sound, this.parent, this.positionre.X, this.positionre.Y, ChipBase.ELEMENT.normal));
                                            break;
                                        case 1:
                                            panel = Panel.PANEL._crack;
                                            this.parent.effects.Add(new Smoke(this.sound, this.parent, this.positionre.X, this.positionre.Y, ChipBase.ELEMENT.normal));
                                            break;
                                        case 2:
                                            panel = Panel.PANEL._burner;
                                            this.parent.effects.Add(new Smoke(this.sound, this.parent, this.positionre.X, this.positionre.Y, ChipBase.ELEMENT.heat));
                                            break;
                                        case 3:
                                            panel = Panel.PANEL._ice;
                                            this.parent.effects.Add(new Smoke(this.sound, this.parent, this.positionre.X, this.positionre.Y, ChipBase.ELEMENT.aqua));
                                            break;
                                        default:
                                            panel = Panel.PANEL._poison;
                                            this.parent.effects.Add(new Smoke(this.sound, this.parent, this.positionre.X, this.positionre.Y, ChipBase.ELEMENT.poison));
                                            break;
                                    }
                                    this.sound.PlaySE(SoundEffect.heat);
                                    if (this.version == 0)
                                    {
                                        this.sound.PlaySE(SoundEffect.shoot);
                                        if (!this.wind)
                                            this.parent.effects.Add(new NSEffect.PushWind(this.sound, this.parent, new Vector2(), new Point(), this.union));
                                        else
                                            this.parent.effects.Add(new NSEffect.BackWind(this.sound, this.parent, new Vector2(), new Point(), this.union));
                                        this.wind = !this.wind;
                                        if (this.brocla != null && this.brocla.flag)
                                            this.brocla.flag = false;
                                        this.brocla = new Brocla(this.sound, this.parent, this.positionre.X, this.positionre.Y, this.union, this.Power, panel, false, this.element);
                                        this.MoveRandom(false, false, this.UnionEnemy, 1);
                                        this.parent.attacks.Add(brocla);
                                        if (this.brocla2 != null && this.brocla2.flag)
                                            this.brocla2.flag = false;
                                        this.brocla2 = new Brocla(this.sound, this.parent, this.positionre.X, this.positionre.Y, this.union, this.Power, panel, false, this.element);
                                        this.parent.attacks.Add(brocla2);
                                    }
                                    else
                                    {
                                        if (this.brocla != null && this.brocla.flag)
                                            this.brocla.flag = false;
                                        this.brocla = new Brocla(this.sound, this.parent, this.positionre.X, this.positionre.Y, this.union, this.Power, panel, false, this.element);
                                        this.parent.attacks.Add(brocla);
                                    }
                                }
                                this.frame = 0;
                                this.motion = Brocooler.MOTION.neutral;
                                this.counterTiming = false;
                                break;
                            }
                            break;
                        }
                        this.frame = 0;
                        this.motion = Brocooler.MOTION.neutral;
                        this.counterTiming = false;
                        break;
                    case Brocooler.MOTION.okataduke:
                        if (this.BlackOut(
                            this,
                            this.parent,
                            ShanghaiEXE.Translate("Enemy.BrocoolerSpecial"),
                            ""))
                        {
                            if (this.waittime == 1)
                            {
                                this.sound.PlaySE(SoundEffect.eriasteal2);
                                for (int pX = 0; pX < this.parent.panel.GetLength(0); ++pX)
                                {
                                    for (int pY = 0; pY < this.parent.panel.GetLength(1); ++pY)
                                    {
                                        if (this.parent.panel[pX, pY].State != Panel.PANEL._none && this.parent.panel[pX, pY].color == this.union)
                                        {
                                            this.parent.panel[pX, pY].State = Panel.PANEL._nomal;
                                            this.parent.effects.Add(new Smoke(this.sound, this.parent, pX, pY, this.element));
                                        }
                                    }
                                }
                                foreach (ObjectBase objectBase in this.parent.objects)
                                    objectBase.Break();
                            }
                            if (this.waittime > 30 && this.BlackOutEnd(this, this.parent))
                            {
                                this.motion = Brocooler.MOTION.neutral;
                                this.frame = 0;
                                this.waittime = 0;
                                this.speed = this.nspeed;
                            }
                            ++this.waittime;
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

        public override void RenderUP(IRenderer dg)
        {
            this.BlackOutRender(dg, this.union);
        }

        private Point AnimeNeutral(int waitflame)
        {
            return this.ReturnKai(new int[14]
            {
        1,
        1,
        1,
        4,
        1,
        1,
        4,
        1,
        1,
        4,
        1,
        1,
        4,
        60
            }, new int[14]
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
        0,
        0
            }, 1, waitflame);
        }

        private enum MOTION
        {
            neutral,
            attack,
            okataduke,
        }
    }
}

