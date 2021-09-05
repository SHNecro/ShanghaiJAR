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

namespace NSEnemy
{
    internal class Lanster : EnemyBase
    {
        private Lanster.MOTION motion = Lanster.MOTION.neutral;
        private int roopneutral;
        private bool Xmove;
        private readonly int nspeed;
        private int plusX;
        private readonly int steel;
        private int attackroop;
        private bool bash;

        public Lanster(IAudioEngine s, SceneBattle p, int pX, int pY, byte n, Panel.COLOR u, byte v)
          : base(s, p, pX, pY, n, u, v)
        {
            this.wantedPosition.X = -24;
            for (int index = 0; index < this.dropchips.Length; ++index)
                this.dropchips[index] = new ChipFolder(this.sound);
            this.picturename = "lanster";
            this.race = EnemyBase.ENEMY.virus;
            this.Flying = false;
            this.wide = 72;
            this.height = 40;
            this.printNumber = false;
            this.steel = version;
            switch (this.version)
            {
                case 0:
                    this.power = 300;
                    this.hp = 500;
                    this.nspeed = 3;
                    this.name = ShanghaiEXE.Translate("Enemy.LansterName1");
                    break;
                case 1:
                    this.power = 50;
                    this.nspeed = 7;
                    this.hp = 100;
                    this.name = ShanghaiEXE.Translate("Enemy.LansterName2");
                    break;
                case 2:
                    this.power = 80;
                    this.nspeed = 6;
                    this.hp = 140;
                    this.name = ShanghaiEXE.Translate("Enemy.LansterName3");
                    break;
                case 3:
                    this.power = 120;
                    this.nspeed = 5;
                    this.hp = 230;
                    this.name = ShanghaiEXE.Translate("Enemy.LansterName4");
                    break;
                case 4:
                    this.power = 200;
                    this.nspeed = 4;
                    this.hp = 300;
                    this.name = ShanghaiEXE.Translate("Enemy.LansterName5");
                    break;
                default:
                    this.power = 200 + (version - 4) * 40;
                    this.nspeed = 4;
                    this.hp = 300 + (version - 4) * 40;
                    this.name = ShanghaiEXE.Translate("Enemy.LansterName6") + (version - 3).ToString();
                    break;
            }
            this.roop = this.number;
            this.hpmax = this.hp;
            this.speed = this.nspeed;
            this.hpprint = this.hp;
            this.printhp = true;
            this.effecting = false;
            this.PositionDirectSet();
            this.element = ChipBase.ELEMENT.normal;
            switch (this.version)
            {
                case 0:
                    this.dropchips[0].chip = new KnightLance(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new KnightLance(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new KnightLance(this.sound);
                    this.dropchips[2].codeNo = 2;
                    this.dropchips[3].chip = new KnightLance(this.sound);
                    this.dropchips[3].codeNo = 2;
                    this.dropchips[4].chip = new KnightLance(this.sound);
                    this.dropchips[4].codeNo = 3;
                    break;
                case 1:
                    this.dropchips[0].chip = new Lance(this.sound);
                    this.dropchips[0].codeNo = 1;
                    this.dropchips[1].chip = new Lance(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new Lance(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new Lance(this.sound);
                    this.dropchips[3].codeNo = 2;
                    this.dropchips[4].chip = new Lance(this.sound);
                    if (this.Random.Next(4) < 3)
                        this.dropchips[4].codeNo = 2;
                    else
                        this.dropchips[4].codeNo = 3;
                    this.havezenny = 300;
                    break;
                case 2:
                    this.dropchips[0].chip = new KnightLance(this.sound);
                    this.dropchips[0].codeNo = 1;
                    this.dropchips[1].chip = new KnightLance(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new KnightLance(this.sound);
                    this.dropchips[2].codeNo = 2;
                    this.dropchips[3].chip = new KnightLance(this.sound);
                    this.dropchips[3].codeNo = 2;
                    this.dropchips[4].chip = new KnightLance(this.sound);
                    this.dropchips[4].codeNo = 3;
                    this.havezenny = 600;
                    break;
                case 3:
                    this.dropchips[0].chip = new PaladinLance(this.sound);
                    this.dropchips[0].codeNo = 1;
                    this.dropchips[1].chip = new PaladinLance(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new PaladinLance(this.sound);
                    this.dropchips[2].codeNo = 3;
                    this.dropchips[3].chip = new PaladinLance(this.sound);
                    this.dropchips[3].codeNo = 2;
                    this.dropchips[4].chip = new PaladinLance(this.sound);
                    this.dropchips[4].codeNo = 0;
                    this.havezenny = 800;
                    break;
                default:
                    this.dropchips[0].chip = new Lance(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new KnightLance(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new KnightLance(this.sound);
                    this.dropchips[2].codeNo = 2;
                    this.dropchips[3].chip = new PaladinLance(this.sound);
                    this.dropchips[3].codeNo = 2;
                    this.dropchips[4].chip = new Lance(this.sound);
                    this.dropchips[4].codeNo = 3;
                    this.havezenny = 800;
                    break;
            }
            this.neutlal = true;
        }

        public override void PositionDirectSet()
        {
            this.positionDirect = new Vector2((float)(position.X * 40.0 + 16.0), (float)(position.Y * 24.0 + 68.0));
        }

        protected override void Moving()
        {
            this.neutlal = this.motion == Lanster.MOTION.neutral;
            if (this.moveflame)
            {
                this.positionre = this.position;
                switch (this.motion)
                {
                    case Lanster.MOTION.neutral:
                        if (this.moveflame)
                        {
                            this.animationpoint = this.AnimeNeutral(this.frame);
                            if (this.frame >= 7)
                            {
                                this.frame = 0;
                                ++this.roopneutral;
                            }
                            if (this.roopneutral >= 2 && this.parent.nowscene != SceneBattle.BATTLESCENE.end)
                            {
                                this.roopneutral = 0;
                                if (this.parent.panel[this.position.X + this.UnionRebirth, this.position.Y].color == this.UnionEnemy || this.HeviSand)
                                {
                                    if (this.EnemySearch(this.position.Y) && !this.badstatus[4])
                                    {
                                        this.speed /= 2;
                                        ++this.attackroop;
                                        this.motion = Lanster.MOTION.attack;
                                        this.counterTiming = true;
                                    }
                                    else
                                    {
                                        this.Xmove = false;
                                        this.motion = Lanster.MOTION.move;
                                    }
                                }
                                else
                                {
                                    this.Xmove = true;
                                    this.motion = Lanster.MOTION.move;
                                }
                            }
                            break;
                        }
                        break;
                    case Lanster.MOTION.move:
                        if (this.Xmove)
                        {
                            this.positionre = new Point(this.position.X + this.UnionRebirth, this.position.Y);
                            if (this.Canmove(this.positionre, this.number, this.union))
                            {
                                this.parent.effects.Add(new MoveEnemy(this.sound, this.parent, this.position.X, this.position.Y));
                                this.position = this.positionre;
                                this.PositionDirectSet();
                                this.frame = 0;
                            }
                            else
                            {
                                this.positionre = this.position;
                                this.Xmove = false;
                            }
                        }
                        if (!this.Xmove)
                        {
                            bool flag = false;
                            if (this.EnemySearch(this.position.Y - 2) && !flag)
                            {
                                this.positionre = new Point(this.position.X, this.position.Y - 1);
                                if (this.Canmove(this.positionre, this.number, this.union))
                                {
                                    flag = true;
                                    this.parent.effects.Add(new MoveEnemy(this.sound, this.parent, this.position.X, this.position.Y));
                                    this.position = this.positionre;
                                    this.PositionDirectSet();
                                    this.frame = 0;
                                }
                                else
                                    this.positionre = this.position;
                            }
                            if (this.EnemySearch(this.position.Y - 1) && !flag)
                            {
                                this.positionre = new Point(this.position.X, this.position.Y - 1);
                                if (this.Canmove(this.positionre, this.number, this.union))
                                {
                                    flag = true;
                                    this.parent.effects.Add(new MoveEnemy(this.sound, this.parent, this.position.X, this.position.Y));
                                    this.position = this.positionre;
                                    this.PositionDirectSet();
                                    this.frame = 0;
                                }
                                else
                                    this.positionre = this.position;
                            }
                            if (this.EnemySearch(this.position.Y + 1) && !flag)
                            {
                                this.positionre = new Point(this.position.X, this.position.Y + 1);
                                if (this.Canmove(this.positionre, this.number, this.union))
                                {
                                    flag = true;
                                    this.parent.effects.Add(new MoveEnemy(this.sound, this.parent, this.position.X, this.position.Y));
                                    this.position = this.positionre;
                                    this.PositionDirectSet();
                                    this.frame = 0;
                                }
                                else
                                    this.positionre = this.position;
                            }
                            if (this.EnemySearch(this.position.Y + 2) && !flag)
                            {
                                this.positionre = new Point(this.position.X, this.position.Y + 1);
                                if (this.Canmove(this.positionre, this.number, this.union))
                                {
                                    this.parent.effects.Add(new MoveEnemy(this.sound, this.parent, this.position.X, this.position.Y));
                                    this.position = this.positionre;
                                    this.PositionDirectSet();
                                    this.frame = 0;
                                }
                                else
                                    this.positionre = this.position;
                            }
                        }
                        this.motion = Lanster.MOTION.neutral;
                        break;
                    case Lanster.MOTION.attack:
                        this.animationpoint.X = this.AnimeAttack(this.frame).X;
                        if (this.frame == 4 + this.Attackinterval())
                        {
                            this.counterTiming = false;
                            this.sound.PlaySE(SoundEffect.lance);
                            this.parent.attacks.Add(new LanceAttack(this.sound, this.parent, this.position.X + this.UnionRebirth, this.position.Y, this.union, this.Power, this.speed, this.element, false));
                        }
                        if (this.frame >= 18 + this.Attackinterval())
                        {
                            this.plusX = 0;
                            this.speed = this.nspeed;
                            this.motion = Lanster.MOTION.neutral;
                            this.frame = 0;
                            if (this.attackroop >= 3 && !this.bash)
                            {
                                int pX = Eriabash.SteelX(this, this.parent);
                                if (pX != 99 && pX > 0 && pX < 5)
                                {
                                    for (int pY = 0; pY < this.parent.panel.GetLength(1); ++pY)
                                        this.parent.attacks.Add(new EriaSteel(this.sound, this.parent, pX, pY, this.union, 10, this.element));
                                }
                                if (!this.bash && this.version > 0)
                                    this.bash = true;
                                this.attackroop = 0;
                                if (this.version == 0)
                                {
                                    this.MoveRandom(false, false);
                                    this.parent.effects.Add(new MoveEnemy(this.sound, this.parent, this.position.X, this.position.Y));
                                    this.position = this.positionre;
                                    this.PositionDirectSet();
                                }
                            }
                            break;
                        }
                        break;
                }
            }
            if (this.motion == Lanster.MOTION.attack && (this.frame >= 3 + this.Attackinterval() && this.frame <= 6 + this.Attackinterval()))
            {
                this.plusX += 3 * (this.version > 0 ? version : 5) * this.UnionRebirth;
                switch (this.union)
                {
                    case Panel.COLOR.red:
                        if (this.plusX > 20)
                        {
                            this.plusX = 20;
                            break;
                        }
                        break;
                    case Panel.COLOR.blue:
                        if (this.plusX < -20)
                        {
                            this.plusX = -20;
                            break;
                        }
                        break;
                }
            }
            this.FlameControl();
            this.MoveAftar();
        }

        public override void Render(IRenderer dg)
        {
            if (this.animationpoint.X < 13)
            {
                int num1 = (int)this.positionDirect.X + 3 + 21 * this.UnionRebirth;
                Point shake = this.Shake;
                int x = shake.X;
                double num2 = num1 + x;
                int num3 = (int)this.positionDirect.Y - 2;
                shake = this.Shake;
                int y = shake.Y;
                double num4 = num3 + y;
                this._position = new Vector2((float)num2, (float)num4);
                this._rect = new Rectangle(this.animationpoint.X * this.wide, Math.Min(this.version, (byte)4) * this.height, this.wide, this.height);
                if (this.version == 0)
                    this._rect.Y = this.height * 5;
            }
            else
            {
                this._position = new Vector2((int)this.positionDirect.X + 18 * this.UnionRebirth + this.plusX + this.Shake.X, (int)this.positionDirect.Y - 2 + this.Shake.Y);
                this._rect = new Rectangle(this.animationpoint.X * this.wide, Math.Min(this.version, (byte)4) * this.height, 46, this.height);
                if (this.version == 0)
                    this._rect.Y = this.height * 5;
                dg.DrawImage(dg, this.picturename, this._rect, false, this._position, this.rebirth, this.color);
                int num1 = (int)this.positionDirect.X + 3 * this.UnionRebirth;
                Point shake = this.Shake;
                int x = shake.X;
                double num2 = num1 + x;
                int num3 = (int)this.positionDirect.Y - 2;
                shake = this.Shake;
                int y = shake.Y;
                double num4 = num3 + y;
                this._position = new Vector2((float)num2, (float)num4);
                this._rect = new Rectangle(this.animationpoint.X * this.wide + 46, Math.Min(this.version, (byte)4) * this.height, 26, this.height);
                if (this.version == 0)
                    this._rect.Y = this.height * 5;
            }
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
            if (this.animationpoint.X >= 13) { }
            this.HPposition = new Vector2(this.positionDirect.X + 6f, this.positionDirect.Y - 2f - this.height / 2);
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
            }, new int[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, 1, waitflame);
        }

        private Point AnimeAttack(int waitflame)
        {
            return this.ReturnKai(new int[8]
            {
        0,
        1,
        1,
        1,
        this.Attackinterval(),
        1,
        1,
        30
            }, new int[8] { 0, 8, 9, 10, 11, 12, 13, 14 }, 1, waitflame);
        }

        private int Attackinterval()
        {
            return 8 - Math.Min(4, (int)this.version);
        }

        private enum MOTION
        {
            neutral,
            move,
            attack,
        }
    }
}

