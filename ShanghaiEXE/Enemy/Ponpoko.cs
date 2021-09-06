using NSAttack;
using NSBattle;
using NSBattle.Character;
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
    internal class Ponpoko : EnemyBase
    {
        private Ponpoko.MOTION motion = Ponpoko.MOTION.neutral;
        private readonly int nspeed;
        private int attackcount;
        private readonly int moveroop;
        private readonly bool attackanimation;
        private int roopneutral;
        private int roopmove;
        private Jizou jizou1;
        private Jizou jizou2;
        private bool tenbathu;

        public Ponpoko(IAudioEngine s, SceneBattle p, int pX, int pY, byte n, Panel.COLOR u, byte v)
          : base(s, p, pX, pY, n, u, v)
        {
            for (int index = 0; index < this.dropchips.Length; ++index)
                this.dropchips[index] = new ChipFolder(this.sound);
            this.picturename = "ponpoko";
            this.race = EnemyBase.ENEMY.virus;
            this.element = ChipBase.ELEMENT.leaf;
            this.wide = 40;
            this.height = 40;
            this.nspeed = 8 - version * 2;
            this.speed = this.nspeed;
            this.printhp = true;
            this.printNumber = true;
            this.name = ShanghaiEXE.Translate("Enemy.PonpokoName1");
            switch (this.version)
            {
                case 0:
                    this.power = 1000;
                    this.hp = 1500;
                    this.invincibility = true;
                    this.invincibilitytime = 9999999;
                    this.name = ShanghaiEXE.Translate("Enemy.PonpokoName2");
                    this.printNumber = false;
                    break;
                case 1:
                    this.power = 100;
                    this.hp = 130;
                    this.moveroop = 2;
                    break;
                case 2:
                    this.power = 140;
                    this.hp = 170;
                    this.moveroop = 2;
                    break;
                case 3:
                    this.power = 180;
                    this.hp = 200;
                    this.moveroop = 3;
                    break;
                case 4:
                    this.power = 220;
                    this.hp = 250;
                    this.moveroop = 3;
                    this.name = ShanghaiEXE.Translate("Enemy.PonpokoName3");
                    this.printNumber = false;
                    break;
                default:
                    this.power = 220 + (version - 4) * 40;
                    this.hp = 250 + (version - 4) * 50;
                    this.moveroop = 3;
                    this.name = ShanghaiEXE.Translate("Enemy.PonpokoName4") + (version - 3).ToString();
                    this.printNumber = false;
                    break;
            }
            this.hpmax = this.hp;
            this.hpprint = this.hp;
            this.effecting = false;
            this.Flying = false;
            this.roopmove = n * 3;
            this.PositionDirectSet();
            switch (this.version)
            {
                case 1:
                    this.dropchips[0].chip = new PonpocoJizou1(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new PonpocoJizou1(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new PonpocoJizou1(this.sound);
                    this.dropchips[2].codeNo = 1;
                    this.dropchips[3].chip = new PonpocoJizou1(this.sound);
                    this.dropchips[3].codeNo = 2;
                    this.dropchips[4].chip = new PonpocoJizou1(this.sound);
                    if (this.Random.Next(4) < 3)
                        this.dropchips[4].codeNo = 2;
                    else
                        this.dropchips[4].codeNo = 3;
                    this.havezenny = 300;
                    break;
                case 2:
                    this.dropchips[0].chip = new PonpocoJizou2(this.sound);
                    this.dropchips[0].codeNo = 2;
                    this.dropchips[1].chip = new PonpocoJizou2(this.sound);
                    this.dropchips[1].codeNo = 2;
                    this.dropchips[2].chip = new PonpocoJizou2(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new PonpocoJizou2(this.sound);
                    this.dropchips[3].codeNo = 3;
                    this.dropchips[4].chip = new PonpocoJizou2(this.sound);
                    this.dropchips[4].codeNo = 1;
                    this.havezenny = 600;
                    break;
                case 3:
                    this.dropchips[0].chip = new PonpocoJizou3(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new PonpocoJizou3(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new PonpocoJizou3(this.sound);
                    this.dropchips[2].codeNo = 3;
                    this.dropchips[3].chip = new PonpocoJizou3(this.sound);
                    this.dropchips[3].codeNo = 2;
                    this.dropchips[4].chip = new PonpocoJizou3(this.sound);
                    this.dropchips[4].codeNo = 1;
                    this.havezenny = 800;
                    break;
                default:
                    this.dropchips[0].chip = new PonpocoJizou1(this.sound);
                    this.dropchips[0].codeNo = 2;
                    this.dropchips[1].chip = new PonpocoJizou2(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new PonpocoJizou2(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new PonpocoJizou3(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new PonpocoJizou1(this.sound);
                    this.dropchips[4].codeNo = 3;
                    this.havezenny = 800;
                    if (this.version < 8)
                        break;
                    this.dropchips[4].chip = new PonpocoJizouX(this.sound);
                    this.dropchips[4].codeNo = this.Random.Next(4);
                    this.havezenny = 8000;
                    break;
            }
        }

        public override void PositionDirectSet()
        {
            this.positionDirect = new Vector2((float)(position.X * 40.0 + 20.0) + 8 * this.UnionRebirth, (float)(position.Y * 24.0 + 68.0));
        }

        protected override void Moving()
        {
            this.neutlal = this.motion == Ponpoko.MOTION.neutral;
            switch (this.motion)
            {
                case Ponpoko.MOTION.neutral:
                    if (this.moveflame)
                    {
                        this.animationpoint = this.AnimeNeutral(this.frame);
                        if (this.frame >= 7)
                        {
                            this.frame = 0;
                            ++this.roopneutral;
                            if (this.roopmove == this.moveroop - 1 && this.roopneutral == 1)
                                this.counterTiming = true;
                            if (this.roopneutral >= 2 && this.parent.nowscene != SceneBattle.BATTLESCENE.end)
                            {
                                this.roopneutral = 0;
                                this.motion = Ponpoko.MOTION.move;
                            }
                        }
                        break;
                    }
                    break;
                case Ponpoko.MOTION.move:
                    ++this.roopmove;
                    this.MoveRandom(false, false);
                    if (this.position == this.positionre)
                    {
                        this.frame = 0;
                        this.roopneutral = 0;
                    }
                    else
                    {
                        this.parent.effects.Add(new MoveEnemy(this.sound, this.parent, this.position.X, this.position.Y));
                        this.position = this.positionre;
                        this.PositionDirectSet();
                        this.frame = 0;
                        this.roopneutral = 0;
                    }
                    if (this.roopmove >= this.moveroop && !this.badstatus[4])
                    {
                        this.attackcount = 1;
                        this.speed = 3;
                        this.sound.PlaySE(SoundEffect.bomb);
                        this.parent.effects.Add(new Smoke(this.sound, this.parent, this.position.X, this.position.Y, ChipBase.ELEMENT.normal));
                        if (this.version == 0)
                        {
                            this.invincibility = false;
                            this.invincibilitytime = 0;
                            this.MoveRandom(false, false);
                            Point positionre1 = this.positionre;
                            if (positionre1 != this.position)
                            {
                                this.parent.effects.Add(new Smoke(this.sound, this.parent, positionre1.X, positionre1.Y, ChipBase.ELEMENT.normal));
                                this.jizou1 = new Jizou(this.sound, this.parent, positionre1.X, positionre1.Y, this.union, 5, this.Power);
                                this.parent.objects.Add(jizou1);
                                this.jizou1.PositionDirectSet();
                            }
                            this.MoveRandom(false, false);
                            Point positionre2 = this.positionre;
                            if (positionre2 != this.position)
                            {
                                this.parent.effects.Add(new Smoke(this.sound, this.parent, positionre2.X, positionre2.Y, ChipBase.ELEMENT.normal));
                                this.jizou2 = new Jizou(this.sound, this.parent, positionre2.X, positionre2.Y, this.union, 5, this.Power);
                                this.parent.objects.Add(jizou2);
                                this.jizou2.PositionDirectSet();
                            }
                        }
                        else
                            this.guard = CharacterBase.GUARD.guard;
                        this.motion = Ponpoko.MOTION.attack;
                        this.counterTiming = false;
                        break;
                    }
                    this.motion = Ponpoko.MOTION.neutral;
                    break;
                case Ponpoko.MOTION.attack:
                    if (this.moveflame)
                    {
                        this.animationpoint.X = 7;
                        if (this.frame >= 45)
                        {
                            this.frame = 0;
                            this.roopmove = 0;
                            this.roopneutral = 0;
                            this.speed = this.nspeed;
                            if (this.version == 0)
                            {
                                this.invincibility = true;
                                this.invincibilitytime = 9999999;
                                if (this.jizou1 != null)
                                    this.jizou1.flag = false;
                                if (this.jizou2 != null)
                                    this.jizou2.flag = false;
                            }
                            this.motion = Ponpoko.MOTION.neutral;
                            this.guard = CharacterBase.GUARD.none;
                        }
                        else if (this.tenbathu)
                        {
                            this.tenbathu = false;
                            this.motion = Ponpoko.MOTION.attacked;
                            this.frame = 0;
                            foreach (CharacterBase characterBase in this.parent.AllChara())
                            {
                                if (characterBase.union == this.UnionEnemy)
                                {
                                    Point position = characterBase.position;
                                    CrackThunder crackThunder = new CrackThunder(this.sound, this.parent, position.X, position.Y, this.union, this.Power, false)
                                    {
                                        breaking = true,
                                        breakinvi = true
                                    };
                                    this.parent.attacks.Add(crackThunder);
                                }
                            }
                        }
                        break;
                    }
                    break;
                case Ponpoko.MOTION.attacked:
                    if (this.moveflame)
                    {
                        this.animationpoint = this.AnimeAttack(this.frame % 4);
                        if (this.frame >= 45)
                        {
                            this.frame = 0;
                            this.roopmove = 0;
                            this.roopneutral = 0;
                            this.speed = this.nspeed;
                            this.motion = Ponpoko.MOTION.neutral;
                            this.guard = CharacterBase.GUARD.none;
                        }
                        break;
                    }
                    break;
            }
            this.FlameControl();
            this.MoveAftar();
        }

        public override void NoDameged(AttackBase attack)
        {
            if (this.version <= 0)
                return;
            this.tenbathu = true;
        }

        public override void Render(IRenderer dg)
        {
            int num1 = (int)this.positionDirect.X + (this.union == Panel.COLOR.blue ? 5 : -5);
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
            if (this.version == 0 && this.motion == Ponpoko.MOTION.attack)
                this.HPposition = new Vector2(999f, 999f);
            else
                this.HPposition = new Vector2((int)this.positionDirect.X, (int)this.positionDirect.Y - this.height / 2 - 8);
            this.Nameprint(dg, this.printNumber);
        }

        private Point AnimeNeutral(int waitflame)
        {
            return this.Return(new int[7] { 0, 1, 2, 3, 4, 5, 6 }, new int[7]
            {
        0,
        1,
        2,
        3,
        4,
        5,
        6
            }, 1, waitflame);
        }

        private Point AnimeAttack(int waitflame)
        {
            return this.Return(new int[3] { 1, 2, 3 }, new int[3]
            {
        8,
        9,
        10
            }, 0, waitflame);
        }

        private enum MOTION
        {
            neutral,
            move,
            attack,
            attacked,
        }
    }
}

