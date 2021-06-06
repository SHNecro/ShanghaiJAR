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
    internal class Bouzu : EnemyBase
    {
        private Bouzu.MOTION motion = Bouzu.MOTION.neutral;
        private BouzuTornado tornado;
        private bool spSecond;

        public Bouzu(IAudioEngine s, SceneBattle p, int pX, int pY, byte n, Panel.COLOR u, byte v)
          : base(s, p, pX, pY, n, u, v)
        {
            this.helpPosition.X = -8;
            for (int index = 0; index < this.dropchips.Length; ++index)
                this.dropchips[index] = new ChipFolder(this.sound);
            this.picturename = "bouzu";
            this.race = EnemyBase.ENEMY.virus;
            this.Flying = false;
            this.wide = 64;
            this.height = 48;
            this.printNumber = false;
            switch (this.version)
            {
                case 0:
                    this.element = ChipBase.ELEMENT.poison;
                    this.name = ShanghaiEXE.Translate("Enemy.BouzuName1");
                    this.power = 250;
                    this.hp = 1800;
                    break;
                case 1:
                    this.element = ChipBase.ELEMENT.leaf;
                    this.name = ShanghaiEXE.Translate("Enemy.BouzuName2");
                    this.power = 60;
                    this.hp = 180;
                    break;
                case 2:
                    this.element = ChipBase.ELEMENT.aqua;
                    this.name = ShanghaiEXE.Translate("Enemy.BouzuName3");
                    this.power = 90;
                    this.hp = 240;
                    break;
                case 3:
                    this.element = ChipBase.ELEMENT.earth;
                    this.name = ShanghaiEXE.Translate("Enemy.BouzuName4");
                    this.power = 130;
                    this.hp = 300;
                    break;
                case 4:
                    this.element = ChipBase.ELEMENT.aqua;
                    this.name = ShanghaiEXE.Translate("Enemy.BouzuName5");
                    this.printNumber = false;
                    this.power = 180;
                    this.hp = 380;
                    break;
                default:
                    this.element = ChipBase.ELEMENT.aqua;
                    this.name = ShanghaiEXE.Translate("Enemy.BouzuName6") + (version - 3).ToString();
                    this.printNumber = false;
                    this.power = 180 + (version - 4) * 40;
                    this.hp = 400 + (version - 4) * 40;
                    break;
            }
            this.roop = this.number;
            this.hpmax = this.hp;
            this.speed = 4;
            this.hpprint = this.hp;
            this.printhp = true;
            this.effecting = false;
            this.PositionDirectSet();
            switch (this.version)
            {
                case 1:
                    this.dropchips[0].chip = new LeafStorm(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new LeafStorm(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new LeafStorm(this.sound);
                    this.dropchips[2].codeNo = 2;
                    this.dropchips[3].chip = new LeafStorm(this.sound);
                    this.dropchips[3].codeNo = 2;
                    this.dropchips[4].chip = new LeafStorm(this.sound);
                    if (this.Random.Next(4) < 3)
                        this.dropchips[4].codeNo = 0;
                    else
                        this.dropchips[4].codeNo = 3;
                    this.havezenny = 300;
                    break;
                case 2:
                    this.dropchips[0].chip = new AquaStorm(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new AquaStorm(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new AquaStorm(this.sound);
                    this.dropchips[2].codeNo = 2;
                    this.dropchips[3].chip = new AquaStorm(this.sound);
                    this.dropchips[3].codeNo = 2;
                    this.dropchips[4].chip = new AquaStorm(this.sound);
                    this.dropchips[4].codeNo = 3;
                    this.havezenny = 600;
                    break;
                case 3:
                    this.dropchips[0].chip = new SandStorm(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new SandStorm(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new SandStorm(this.sound);
                    this.dropchips[2].codeNo = 2;
                    this.dropchips[3].chip = new SandStorm(this.sound);
                    this.dropchips[3].codeNo = 2;
                    this.dropchips[4].chip = new SandStorm(this.sound);
                    this.dropchips[4].codeNo = 3;
                    this.havezenny = 800;
                    break;
                default:
                    this.dropchips[0].chip = new LeafStorm(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new AquaStorm(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new LeafStorm(this.sound);
                    this.dropchips[2].codeNo = 2;
                    this.dropchips[3].chip = new AquaStorm(this.sound);
                    this.dropchips[3].codeNo = 2;
                    this.dropchips[4].chip = new SandStorm(this.sound);
                    this.dropchips[4].codeNo = 3;
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
            this.neutlal = this.motion == Bouzu.MOTION.neutral;
            if (this.moveflame)
            {
                this.positionre = this.position;
                switch (this.motion)
                {
                    case Bouzu.MOTION.neutral:
                        if (this.frame > 3)
                        {
                            if (this.roop > 6 - version && !this.badstatus[4] && this.parent.nowscene != SceneBattle.BATTLESCENE.end)
                            {
                                this.counterTiming = false;
                                this.sound.PlaySE(SoundEffect.shoot);
                                if (this.version == 0)
                                {
                                    this.spSecond = !this.spSecond;
                                    this.positionre = new Point(this.spSecond ? 5 : 0, this.Random.Next(3));
                                    if (this.position != this.positionre && this.NoObject(this.positionre))
                                    {
                                        this.parent.effects.Add(new MoveEnemy(this.sound, this.parent, this.position.X, this.position.Y));
                                        this.position = this.positionre;
                                        this.PositionDirectSet();
                                    }
                                    this.tornado = new BouzuTornado(this.sound, this.parent, this.position.X, this.position.Y, this.union, this.Power, this.element, 2, 8, 1);
                                }
                                else
                                    this.tornado = new BouzuTornado(this.sound, this.parent, this.position.X, this.position.Y, this.union, this.Power, this.element, 2, 2 * version, 8 / version);
                                this.parent.attacks.Add(tornado);
                                this.frame = 0;
                                this.motion = Bouzu.MOTION.attack;
                            }
                            else
                            {
                                ++this.roop;
                                if (this.roop == 6 - version)
                                    this.counterTiming = true;
                            }
                            this.frame = 0;
                            break;
                        }
                        break;
                    case Bouzu.MOTION.attack:
                        if (this.tornado != null)
                        {
                            if (!this.tornado.flag)
                            {
                                this.roop = 0;
                                this.frame = 0;
                                this.motion = Bouzu.MOTION.neutral;
                                this.animationpoint.X = 0;
                            }
                            else
                            {
                                if (this.frame == 4)
                                    this.frame = 0;
                                this.animationpoint.X = this.AnimeAttack(this.frame).X;
                            }
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
            {
                if (this.tornado != null)
                    this.tornado.flag = false;
                this.Death(this._rect, new Rectangle(this.animationpoint.X * this.wide, 0, this.wide, this.height), this._position, this.picturename);
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
            this.HPposition = new Vector2(this.positionDirect.X + 6f, this.positionDirect.Y + 2f - this.height / 2);
            this.Nameprint(dg, this.printNumber);
        }

        private Point AnimeAttack(int waitflame)
        {
            return this.Return(new int[4] { 0, 1, 2, 3 }, new int[4]
            {
        1,
        2,
        3,
        4
            }, 1, waitflame);
        }

        private enum MOTION
        {
            neutral,
            attack,
        }
    }
}

