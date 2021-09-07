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
using System.Linq;

namespace NSEnemy
{
    internal class Rieber : EnemyBase
    {
        private Rieber.MOTION motion = Rieber.MOTION.neutral;
        private bool print = true;
        private bool attacked;
        private bool inelea;
        private Point going;
        private SuzuranWhite suzuran;
        private DammyEnemy dammy;

        public Rieber(IAudioEngine s, SceneBattle p, int pX, int pY, byte n, Panel.COLOR u, byte v)
          : base(s, p, pX, pY, n, u, v)
        {
            this.helpPosition.X = -8;
            for (int index = 0; index < this.dropchips.Length; ++index)
                this.dropchips[index] = new ChipFolder(this.sound);
            this.picturename = "rieber";
            this.element = ChipBase.ELEMENT.earth;
            this.race = EnemyBase.ENEMY.virus;
            this.Flying = false;
            this.wide = 48;
            this.height = 32;
            switch (this.version)
            {
                case 0:
                    this.name = ShanghaiEXE.Translate("Enemy.RieberName1");
                    this.power = 200;
                    this.hp = 2000;
                    this.element = ChipBase.ELEMENT.leaf;
                    break;
                case 1:
                    this.name = ShanghaiEXE.Translate("Enemy.RieberName2");
                    this.power = 30;
                    this.hp = 80;
                    break;
                case 2:
                    this.name = ShanghaiEXE.Translate("Enemy.RieberName3");
                    this.power = 60;
                    this.hp = 160;
                    break;
                case 3:
                    this.name = ShanghaiEXE.Translate("Enemy.RieberName4");
                    this.power = 100;
                    this.hp = 240;
                    break;
                case 4:
                    this.name = ShanghaiEXE.Translate("Enemy.RieberName5");
                    this.power = 120;
                    this.hp = 240;
                    break;
                default:
                    this.name = ShanghaiEXE.Translate("Enemy.RieberName6") + (version - 3).ToString();
                    this.power = 120 + (version - 4) * 20;
                    this.hp = 240 + (version - 4) * 20;
                    break;
            }
            this.hpmax = this.hp;
            this.speed = 5;
            this.hpprint = this.hp;
            this.printhp = true;
            this.effecting = false;
            if (this.parent != null)
                this.roop = (byte)(parent.manyenemys - (uint)this.number);
            this.PositionDirectSet();
            this.frame = this.number * 3;
            switch (this.version)
            {
                case 1:
                    this.dropchips[0].chip = new SandHell1(this.sound);
                    this.dropchips[0].codeNo = 1;
                    this.dropchips[1].chip = new SandHell1(this.sound);
                    this.dropchips[1].codeNo = 2;
                    this.dropchips[2].chip = new SandHell1(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new SandHell1(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new SandHell1(this.sound);
                    if (this.Random.Next(4) < 3)
                        this.dropchips[4].codeNo = 0;
                    else
                        this.dropchips[4].codeNo = 3;
                    this.havezenny = 300;
                    break;
                case 2:
                    this.dropchips[0].chip = new SandHell2(this.sound);
                    this.dropchips[0].codeNo = 1;
                    this.dropchips[1].chip = new SandHell2(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new SandHell2(this.sound);
                    this.dropchips[2].codeNo = 2;
                    this.dropchips[3].chip = new SandHell2(this.sound);
                    this.dropchips[3].codeNo = 3;
                    this.dropchips[4].chip = new SandHell2(this.sound);
                    this.dropchips[4].codeNo = 0;
                    this.havezenny = 600;
                    break;
                case 3:
                    this.dropchips[0].chip = new SandHell3(this.sound);
                    this.dropchips[0].codeNo = 2;
                    this.dropchips[1].chip = new SandHell3(this.sound);
                    this.dropchips[1].codeNo = 3;
                    this.dropchips[2].chip = new SandHell3(this.sound);
                    this.dropchips[2].codeNo = 1;
                    this.dropchips[3].chip = new SandHell3(this.sound);
                    this.dropchips[3].codeNo = 1;
                    this.dropchips[4].chip = new SandHell3(this.sound);
                    this.dropchips[4].codeNo = 0;
                    this.havezenny = 800;
                    break;
                default:
                    this.dropchips[0].chip = new SandHell1(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new SandHell1(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new SandHell2(this.sound);
                    this.dropchips[2].codeNo = 2;
                    this.dropchips[3].chip = new SandHell3(this.sound);
                    this.dropchips[3].codeNo = 2;
                    this.dropchips[4].chip = new SandHell1(this.sound);
                    this.dropchips[4].codeNo = 3;
                    this.havezenny = 800;
                    break;
            }
            this.neutlal = true;
        }

        public override void PositionDirectSet()
        {
            this.positionDirect = new Vector2((float)(position.X * 40.0 + 14.0), (float)(position.Y * 24.0 + 70.0));
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
            this.parent.enemys.Add(dammy);
        }

        protected override void Moving()
        {
            this.neutlal = this.motion == Rieber.MOTION.neutral;
            if (this.moveflame)
            {
                this.animationpoint.X = this.frame % 4;
                this.positionre = this.position;
                switch (this.motion)
                {
                    case Rieber.MOTION.neutral:
                        if (this.frame == (this.inelea ? 10 : 20) / 2 && !this.badstatus[4] && this.parent.nowscene != SceneBattle.BATTLESCENE.end)
                            this.counterTiming = false;
                        if (this.frame >= (this.inelea ? 10 : 20) && !this.badstatus[4] && this.parent.nowscene != SceneBattle.BATTLESCENE.end)
                        {
                            if (this.attacked)
                            {
                                this.frame = 0;
                                this.counterTiming = false;
                                this.motion = Rieber.MOTION.move;
                            }
                            else
                            {
                                this.frame = 0;
                                this.motion = Rieber.MOTION.attack;
                                this.counterTiming = true;
                                this.attacked = true;
                            }
                            break;
                        }
                        break;
                    case Rieber.MOTION.attack:
                        if (this.version == 0)
                        {
                            if (this.inelea)
                            {
                                for (int seed = 0; seed < 9; ++seed)
                                {
                                    this.MoveRandom(false, false, this.union, seed);
                                    this.parent.attacks.Add(new SandHoleAttack(this.sound, this.parent, this.positionre.X, this.positionre.Y, this.union, this.Power, 45, 4, SandHoleAttack.MOTION.init, this.element));
                                }
                            }
                            else
                            {
                                this.dammy.flag = false;
                                for (int seed = 0; seed < 4; ++seed)
                                {
                                    this.MoveRandom(false, false, this.UnionEnemy, seed);
                                    this.parent.attacks.Add(new SandHoleAttack(this.sound, this.parent, this.positionre.X, this.positionre.Y, this.union, this.Power, 45, 4, SandHoleAttack.MOTION.init, this.element, true));
                                }
                            }
                        }
                        else if (this.inelea)
                        {
                            this.MoveRandom(false, false, this.union, false);
                            this.parent.attacks.Add(new SandHoleAttack(this.sound, this.parent, this.positionre.X, this.positionre.Y, this.union, this.Power, 300, Math.Min(version - 1, 3), SandHoleAttack.MOTION.init, this.element));
                        }
                        else
                        {
                            this.dammy.flag = false;
                            var maxPits = Math.Min(this.version, (byte)4);
                            var enemyPanels = Enumerable.Range(0, 3).SelectMany(y => Enumerable.Range(0, 6).Select(x => this.parent.panel[x, y])).ToArray();
                            var targets = this.RandomMultiPanel(enemyPanels.Length, this.UnionEnemy).Where(p => !this.parent.panel[p.X, p.Y].Hole).Take(maxPits);
                            foreach (var point in targets)
                            { 
                                this.positionre = point;
                                this.parent.attacks.Add(new SandHoleAttack(this.sound, this.parent, point.X, point.Y, this.union, this.Power, 30, Math.Min(version - 1, 3), SandHoleAttack.MOTION.init, this.element, true));
                            }
                        }
                        this.going = this.positionre;
                        this.positionre = this.position;
                        this.frame = 0;
                        this.motion = Rieber.MOTION.neutral;
                        break;
                    case Rieber.MOTION.move:
                        if (this.Canmove(this.going, this.number, this.inelea ? this.union : this.UnionEnemy))
                        {
                            bool flag = false;
                            foreach (AttackBase attack in this.parent.attacks)
                            {
                                if (attack.position == this.going && attack is SandHoleAttack)
                                    flag = true;
                            }
                            if (flag)
                            {
                                this.DammySet();
                                this.parent.effects.Add(new MoveEnemy(this.sound, this.parent, this.position.X, this.position.Y));
                                if (this.version > 0)
                                {
                                    this.parent.attacks.RemoveAll(a =>
                                   {
                                       if (a is SandHoleAttack)
                                           return a.position == this.position;
                                       return false;
                                   });
                                    this.parent.attacks.Add(new SandHoleAttack(this.sound, this.parent, this.position.X, this.position.Y, this.union, !this.badstatus[1] ? this.power : this.power / 2, 20, Math.Min(version - 1, 3), SandHoleAttack.MOTION.set, this.element));
                                }
                                this.position = this.going;
                                this.positionre = this.going;
                                this.positionDirect = new Vector2((float)(position.X * 40.0 + 14.0), (float)(position.Y * 24.0 + 70.0));
                            }
                        }
                        if (this.version == 0)
                        {
                            this.print = false;
                            if (this.inelea)
                            {
                                if (this.suzuran != null)
                                {
                                    if (!this.suzuran.flag)
                                    {
                                        Point point = this.RandomPanel(this.union);
                                        this.sound.PlaySE(SoundEffect.enterenemy);
                                        this.parent.effects.Add(new MoveEnemy(this.sound, this.parent, point.X, point.Y));
                                        this.suzuran = new SuzuranWhite(this.sound, this.parent, point.X, point.Y, this.union, 10, 120);
                                        this.parent.objects.Add(suzuran);
                                    }
                                }
                                else
                                {
                                    Point point = this.RandomPanel(this.union);
                                    this.sound.PlaySE(SoundEffect.enterenemy);
                                    this.parent.effects.Add(new MoveEnemy(this.sound, this.parent, point.X, point.Y));
                                    this.suzuran = new SuzuranWhite(this.sound, this.parent, point.X, point.Y, this.union, 10, 120);
                                    this.parent.objects.Add(suzuran);
                                }
                            }
                        }
                        this.inelea = !this.inelea;
                        this.attacked = false;
                        this.frame = 0;
                        this.motion = Rieber.MOTION.neutral;
                        break;
                }
            }
            this.MoveAftar();
            this.FlameControl();
        }

        public override void Render(IRenderer dg)
        {
            this._position = new Vector2((int)this.positionDirect.X + 6 + 6 * this.UnionRebirth + this.Shake.X, (int)this.positionDirect.Y + 10 + this.Shake.Y);
            this._rect = new Rectangle(this.animationpoint.X * this.wide, Math.Min(this.version, (byte)4) * this.height, this.wide, this.height);
            if (this.version == 0)
                this._rect.Y = this.height * 5;
            if (this.Hp <= 0)
                this.Death(this._rect, new Rectangle(this.animationpoint.X * this.wide, 0, this.wide, this.height), this._position, this.picturename);
            if (this.alfha < byte.MaxValue)
                this.color = Color.FromArgb(alfha, byte.MaxValue, byte.MaxValue, byte.MaxValue);
            else
                this.color = this.mastorcolor;
            if (this.print)
            {
                if (this.whitetime == 0)
                {
                    dg.DrawImage(dg, this.picturename, this._rect, false, this._position, this.rebirth, this.color);
                }
                else
                {
                    this._rect.Y = this.animationpoint.Y * this.height;
                    dg.DrawImage(dg, this.picturename, this._rect, false, this._position, this.rebirth, this.color);
                }
            }
            if (this.print)
                this.HPposition = new Vector2(this.positionDirect.X + 4f, this.positionDirect.Y + 10f + this.height / 2);
            this.Nameprint(dg, this.printNumber);
        }

        private enum MOTION
        {
            neutral,
            attack,
            move,
        }
    }
}

