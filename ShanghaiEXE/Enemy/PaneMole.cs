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
    internal class PaneMole : EnemyBase
    {
        private PaneMole.MOTION motion = PaneMole.MOTION.neutral;
        private int animepls;
        private readonly int nspeed;
        private readonly int moveroop;
        private readonly bool attackanimation;
        private int roopneutral;
        private int roopmove;

        public PaneMole(IAudioEngine s, SceneBattle p, int pX, int pY, byte n, Panel.COLOR u, byte v)
          : base(s, p, pX, pY, n, u, v)
        {
            this.helpPosition.X = -16;
            this.wantedPosition.X = -8;
            this.wantedPosition.Y = -8;
            this.printNumber = true;
            this.name = ShanghaiEXE.Translate("Enemy.PaneMoleName1");
            for (int index = 0; index < this.dropchips.Length; ++index)
                this.dropchips[index] = new ChipFolder(this.sound);
            this.nspeed = 6 - Math.Min(5, (int)this.version);
            switch (this.version)
            {
                case 0:
                    this.name = ShanghaiEXE.Translate("Enemy.PaneMoleName2");
                    this.power = 70;
                    this.hp = 700;
                    this.nspeed = 1;
                    this.moveroop = 1;
                    this.printNumber = false;
                    break;
                case 1:
                    this.power = 30;
                    this.hp = 60;
                    this.moveroop = 2;
                    break;
                case 2:
                    this.power = 100;
                    this.hp = 150;
                    this.moveroop = 2;
                    break;
                case 3:
                    this.power = 150;
                    this.hp = 210;
                    this.moveroop = 1;
                    this.nspeed = 2;
                    break;
                case 4:
                    this.name = ShanghaiEXE.Translate("Enemy.PaneMoleName3");
                    this.power = 200;
                    this.hp = 260;
                    this.moveroop = 1;
                    this.printNumber = false;
                    break;
                default:
                    this.name = ShanghaiEXE.Translate("Enemy.PaneMoleName4") + (version - 3).ToString();
                    this.power = 300;
                    this.hp = 260 + (version - 4) * 40;
                    this.moveroop = 1;
                    this.printNumber = false;
                    break;
            }
            this.picturename = "panemole";
            this.race = EnemyBase.ENEMY.virus;
            this.element = ChipBase.ELEMENT.earth;
            this.Flying = false;
            this.wide = 56;
            this.height = 40;
            this.hpmax = this.hp;
            this.speed = this.nspeed;
            this.hpprint = this.hp;
            this.printhp = true;
            this.effecting = false;
            this.Noslip = true;
            this.PositionDirectSet();
            switch (this.version)
            {
                case 0:
                    this.nspeed = 1;
                    this.speed = this.nspeed;
                    this.dropchips[0].chip = new BioSpray1(this.sound);
                    this.dropchips[0].codeNo = 2;
                    this.dropchips[1].chip = new GraviBall2(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new GraviBall2(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new GraviBall2(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new GraviBall2(this.sound);
                    this.dropchips[4].codeNo = 3;
                    this.havezenny = 600;
                    break;
                case 1:
                    this.dropchips[0].chip = new PanelShoot1(this.sound);
                    this.dropchips[0].codeNo = 2;
                    this.dropchips[1].chip = new PanelShoot1(this.sound);
                    this.dropchips[1].codeNo = 2;
                    this.dropchips[2].chip = new PanelShoot1(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new PanelShoot1(this.sound);
                    this.dropchips[3].codeNo = 1;
                    this.dropchips[4].chip = new PanelShoot1(this.sound);
                    this.dropchips[4].codeNo = 3;
                    this.havezenny = 300;
                    break;
                case 2:
                    this.dropchips[0].chip = new PanelShoot2(this.sound);
                    this.dropchips[0].codeNo = 3;
                    this.dropchips[1].chip = new PanelShoot2(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new PanelShoot2(this.sound);
                    this.dropchips[2].codeNo = 1;
                    this.dropchips[3].chip = new PanelShoot2(this.sound);
                    this.dropchips[3].codeNo = 1;
                    this.dropchips[4].chip = new PanelShoot2(this.sound);
                    this.dropchips[4].codeNo = 2;
                    this.havezenny = 600;
                    break;
                case 3:
                    this.dropchips[0].chip = new PanelShoot3(this.sound);
                    this.dropchips[0].codeNo = 1;
                    this.dropchips[1].chip = new PanelShoot3(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new PanelShoot3(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new PanelShoot3(this.sound);
                    this.dropchips[3].codeNo = 2;
                    this.dropchips[4].chip = new PanelShoot3(this.sound);
                    this.dropchips[4].codeNo = 3;
                    this.havezenny = 800;
                    break;
                default:
                    this.dropchips[0].chip = new PanelShoot1(this.sound);
                    this.dropchips[0].codeNo = 2;
                    this.dropchips[1].chip = new PanelShoot1(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new PanelShoot2(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new PanelShoot3(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new PanelShoot1(this.sound);
                    this.dropchips[4].codeNo = 3;
                    this.havezenny = 800;
                    if (this.version < 8)
                        break;
                    this.dropchips[4].chip = new PanelShootX(this.sound);
                    this.dropchips[4].codeNo = this.Random.Next(4);
                    this.havezenny = 8000;
                    break;
            }
        }

        public override void PositionDirectSet()
        {
            this.positionDirect = new Vector2((float)(position.X * 40.0 + 6.0), (float)(position.Y * 24.0 + 74.0));
        }

        protected override void Moving()
        {
            this.neutlal = this.motion == PaneMole.MOTION.neutral;
            switch (this.motion)
            {
                case PaneMole.MOTION.neutral:
                    if (this.moveflame && this.frame >= 4)
                    {
                        this.frame = 0;
                        ++this.roopneutral;
                        if (this.roopneutral >= 2 && this.parent.nowscene != SceneBattle.BATTLESCENE.end)
                        {
                            this.roopneutral = 0;
                            if (this.roopmove > this.moveroop && !this.badstatus[4])
                            {
                                this.motion = PaneMole.MOTION.attack;
                                this.counterTiming = true;
                            }
                            else
                                this.motion = PaneMole.MOTION.move;
                        }
                        break;
                    }
                    break;
                case PaneMole.MOTION.move:
                    if (this.moveflame)
                    {
                        this.animationpoint = this.AnimeMove(this.frame);
                        switch (this.frame)
                        {
                            case 5:
                                this.nohit = true;
                                break;
                            case 10:
                                this.MoveRandom(false, false);
                                this.position = this.positionre;
                                this.PositionDirectSet();
                                break;
                            case 14:
                                this.nohit = false;
                                break;
                            case 25:
                                ++this.roopmove;
                                this.frame = 0;
                                this.speed = this.nspeed;
                                this.motion = PaneMole.MOTION.neutral;
                                break;
                        }
                        break;
                    }
                    break;
                case PaneMole.MOTION.attack:
                    if (this.moveflame)
                    {
                        this.animationpoint.X = this.AnimeAttack(this.frame).X;
                        if (this.frame == 1 && this.version >= 3)
                        {
                            if (this.parent.panel[this.position.X + this.UnionRebirth, this.position.Y].state == Panel.PANEL._break)
                            {
                                this.parent.effects.Add(new Smoke(this.sound, this.parent, this.position.X + this.UnionRebirth, this.position.Y, ChipBase.ELEMENT.normal));
                                this.parent.panel[this.position.X + this.UnionRebirth, this.position.Y].state = Panel.PANEL._nomal;
                            }
                        }
                        else if (this.frame == 1 && this.version == 0)
                        {
                            this.parent.effects.Add(new Smoke(this.sound, this.parent, this.position.X + this.UnionRebirth, this.position.Y, ChipBase.ELEMENT.normal));
                            this.parent.panel[this.position.X + this.UnionRebirth, this.position.Y].state = (Panel.PANEL)this.Random.Next(3, 9);
                        }
                        if (this.frame == 4)
                        {
                            this.counterTiming = false;
                            this.animepls = 1;
                        }
                        if (this.frame == 7)
                        {
                            switch (this.version)
                            {
                                case 0:
                                    PanelShoot panelShoot = new PanelShoot(this.sound, this.parent, this.position.X + this.UnionRebirth, this.position.Y, this.union, this.Power, ChipBase.ELEMENT.normal)
                                    {
                                        breaking = true
                                    };
                                    this.parent.attacks.Add(panelShoot);
                                    break;
                                case 1:
                                    this.parent.attacks.Add(new PanelShoot(this.sound, this.parent, this.position.X + this.UnionRebirth, this.position.Y, this.union, this.Power, ChipBase.ELEMENT.normal));
                                    break;
                                case 2:
                                    this.parent.attacks.Add(new PanelShoot(this.sound, this.parent, this.position.X + this.UnionRebirth, this.position.Y, this.union, this.Power, ChipBase.ELEMENT.normal));
                                    break;
                                default:
                                    this.parent.attacks.Add(new PanelShoot(this.sound, this.parent, this.position.X + this.UnionRebirth, this.position.Y, this.union, this.Power, ChipBase.ELEMENT.normal));
                                    break;
                            }
                        }
                        if (this.frame == 10)
                        {
                            this.animationpoint.X = 0;
                            this.animepls = 0;
                            this.frame = 0;
                            this.roopmove = 0;
                            this.roopneutral = 0;
                            this.speed = this.nspeed;
                            this.motion = PaneMole.MOTION.move;
                        }
                        break;
                    }
                    break;
            }
            if (this.motion != PaneMole.MOTION.attack) { }
            this.FlameControl();
            this.MoveAftar();
        }

        public override void Render(IRenderer dg)
        {
            int num1 = (int)this.positionDirect.X + (this.union == Panel.COLOR.blue ? 5 : 22);
            Point shake = this.Shake;
            int x = shake.X;
            double num2 = num1 + x;
            int y1 = (int)this.positionDirect.Y;
            shake = this.Shake;
            int y2 = shake.Y;
            double num3 = y1 + y2;
            this._position = new Vector2((float)num2, (float)num3);
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
            this.HPposition = new Vector2((int)this.positionDirect.X + 16, (int)this.positionDirect.Y - this.height / 2 + 40);
            this.Nameprint(dg, this.printNumber);
        }

        private Point AnimeMove(int waitflame)
        {
            return this.ReturnKai(new int[20]
            {
        1,
        1,
        1,
        1,
        1,
        1,
        1,
        1,
        1,
        1,
        1,
        1,
        1,
        1,
        1,
        1,
        1,
        1,
        1,
        1
            }, new int[20]
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
        9,
        8,
        7,
        6,
        5,
        4,
        3,
        2,
        1,
        0
            }, 1, waitflame);
        }

        private Point AnimeNeutral(int waitflame)
        {
            return this.Return(new int[1], new int[1], 0, waitflame);
        }

        private Point AnimeAttack(int waitflame)
        {
            return this.ReturnKai(new int[8]
            {
        1,
        1,
        1,
        3,
        1,
        1,
        1,
        100
            }, new int[8] { 0, 10, 11, 12, 13, 14, 15, 15 }, 0, waitflame);
        }

        private enum MOTION
        {
            neutral,
            move,
            attack,
        }
    }
}

