using NSAttack;
using NSBattle;
using NSBattle.Character;
using NSChip;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;
using Common.Vectors;
using System;
using System.Drawing;

namespace NSEnemy
{
    internal class Gelpark : EnemyBase
    {
        private readonly int waitlimit = 0;
        private int waitingtime = 0;
        private Gelpark.MOTION motion = Gelpark.MOTION.guard;
        private readonly ClossBomb.TYPE type;
        private JusticeBeam jb;

        public Gelpark(IAudioEngine s, SceneBattle p, int pX, int pY, byte n, Panel.COLOR u, byte v)
          : base(s, p, pX, pY, n, u, v)
        {
            this.helpPosition.X = -2;
            this.helpPosition.Y = -8;
            for (int index = 0; index < this.dropchips.Length; ++index)
                this.dropchips[index] = new ChipFolder(this.sound);
            this.picturename = "gelpark";
            this.race = EnemyBase.ENEMY.virus;
            this.Flying = false;
            this.wide = 56;
            this.height = 48;
            this.speed = 7 - Math.Min(5, (int)this.version);
            this.name = ShanghaiEXE.Translate("Enemy.GelparkName1");
            this.printNumber = true;
            switch (this.version)
            {
                case 0:
                    this.name = ShanghaiEXE.Translate("Enemy.GelparkName2");
                    this.printNumber = false;
                    this.power = 180;
                    this.hp = 1500;
                    this.speed = 4;
                    this.waitlimit = 20;
                    break;
                case 1:
                    this.power = 40;
                    this.hp = 60;
                    this.waitlimit = 40;
                    break;
                case 2:
                    this.power = 80;
                    this.hp = 100;
                    this.waitlimit = 30;
                    break;
                case 3:
                    this.power = 150;
                    this.hp = 150;
                    this.waitlimit = 20;
                    break;
                case 4:
                    this.name = ShanghaiEXE.Translate("Enemy.GelparkName3");
                    this.printNumber = false;
                    this.power = 180;
                    this.hp = 230;
                    this.waitlimit = 10;
                    break;
                default:
                    this.printNumber = false;
                    this.name = ShanghaiEXE.Translate("Enemy.GelparkName4") + (version - 3).ToString();
                    this.power = 180 + (version - 4) * 40;
                    this.hp = 230 + (version - 4) * 40;
                    this.waitlimit = 10;
                    break;
            }
            this.waitingtime = this.waitlimit;
            this.guard = CharacterBase.GUARD.guard;
            this.roop = this.number;
            this.hpmax = this.hp;
            this.hpprint = this.hp;
            this.printhp = true;
            this.effecting = false;
            this.PositionDirectSet();
            this.element = ChipBase.ELEMENT.eleki;
            switch (this.version)
            {
                case 1:
                    this.dropchips[0].chip = new Hakkero1(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new Hakkero1(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new Hakkero1(this.sound);
                    this.dropchips[2].codeNo = 2;
                    this.dropchips[3].chip = new Hakkero1(this.sound);
                    this.dropchips[3].codeNo = 2;
                    this.dropchips[4].chip = new Hakkero1(this.sound);
                    if (this.Random.Next(4) < 3)
                        this.dropchips[4].codeNo = 2;
                    else
                        this.dropchips[4].codeNo = 3;
                    this.havezenny = 300;
                    this.type = ClossBomb.TYPE.single;
                    break;
                case 2:
                    this.dropchips[0].chip = new Hakkero2(this.sound);
                    this.dropchips[0].codeNo = 3;
                    this.dropchips[1].chip = new Hakkero2(this.sound);
                    this.dropchips[1].codeNo = 2;
                    this.dropchips[2].chip = new Hakkero2(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new Hakkero2(this.sound);
                    this.dropchips[3].codeNo = 1;
                    this.dropchips[4].chip = new Hakkero2(this.sound);
                    this.dropchips[4].codeNo = 1;
                    this.havezenny = 600;
                    this.type = ClossBomb.TYPE.closs;
                    break;
                case 3:
                    this.dropchips[0].chip = new Hakkero3(this.sound);
                    this.dropchips[0].codeNo = 2;
                    this.dropchips[1].chip = new Hakkero3(this.sound);
                    this.dropchips[1].codeNo = 2;
                    this.dropchips[2].chip = new Hakkero3(this.sound);
                    this.dropchips[2].codeNo = 1;
                    this.dropchips[3].chip = new Hakkero3(this.sound);
                    this.dropchips[3].codeNo = 3;
                    this.dropchips[4].chip = new Hakkero3(this.sound);
                    this.dropchips[4].codeNo = 0;
                    this.havezenny = 800;
                    this.type = ClossBomb.TYPE.big;
                    break;
                default:
                    this.dropchips[0].chip = new Hakkero1(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new Hakkero1(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new Hakkero2(this.sound);
                    this.dropchips[2].codeNo = 2;
                    this.dropchips[3].chip = new Hakkero3(this.sound);
                    this.dropchips[3].codeNo = 2;
                    this.dropchips[4].chip = new Hakkero1(this.sound);
                    this.dropchips[4].codeNo = 3;
                    this.havezenny = 1600;
                    this.type = ClossBomb.TYPE.big;
                    break;
            }
            this.neutlal = true;
        }

        public override void PositionDirectSet()
        {
            this.positionDirect = new Vector2((float)(position.X * 40.0 + 12.0), (float)(position.Y * 24.0 + 68.0));
        }

        protected override void Moving()
        {
            if (this.moveflame)
            {
                this.positionre = this.position;
                switch (this.motion)
                {
                    case Gelpark.MOTION.guard:
                        this.animationpoint.X = 0;
                        bool flag1 = false;
                        if (this.version == 0)
                        {
                            flag1 = true;
                        }
                        else
                        {
                            foreach (CharacterBase characterBase in this.parent.AllChara())
                            {
                                if (characterBase.union == this.UnionEnemy && characterBase.position.Y == this.position.Y)
                                {
                                    flag1 = true;
                                    break;
                                }
                            }
                        }
                        if (flag1)
                        {
                            this.motion = Gelpark.MOTION.wake;
                            this.frame = 0;
                            break;
                        }
                        break;
                    case Gelpark.MOTION.wake:
                        this.animationpoint.X = this.AnimeWake(this.frame).X;
                        if (this.frame == 2)
                            this.guard = CharacterBase.GUARD.none;
                        if (this.frame >= 3)
                        {
                            this.motion = Gelpark.MOTION.wait;
                            this.frame = 0;
                            break;
                        }
                        break;
                    case Gelpark.MOTION.wait:
                        this.animationpoint.X = this.AnimeWait(this.frame).X;
                        bool flag2 = false;
                        if (this.frame >= 10)
                            this.frame = 0;
                        foreach (CharacterBase characterBase in this.parent.AllChara())
                        {
                            if (characterBase.union == this.UnionEnemy && characterBase.position.Y == this.position.Y)
                            {
                                flag2 = true;
                                break;
                            }
                        }
                        --this.waitingtime;
                        if (!flag2 && this.version > 0)
                        {
                            this.waitingtime = this.waitlimit;
                            this.motion = Gelpark.MOTION.close;
                            this.frame = 0;
                            break;
                        }
                        if (this.waitingtime <= 0)
                        {
                            this.waitingtime = this.waitlimit;
                            this.motion = Gelpark.MOTION.attack1;
                            this.frame = 0;
                            break;
                        }
                        break;
                    case Gelpark.MOTION.close:
                        this.animationpoint.X = this.AnimeDown(this.frame).X;
                        if (this.frame == 2)
                            this.guard = CharacterBase.GUARD.guard;
                        if (this.frame >= 3)
                        {
                            this.motion = Gelpark.MOTION.guard;
                            this.frame = 0;
                            break;
                        }
                        break;
                    case Gelpark.MOTION.attack1:
                        this.animationpoint.X = this.AnimeAttack1(this.frame).X;
                        Point point = new Point();
                        switch (this.frame)
                        {
                            case 3:
                                this.guard = CharacterBase.GUARD.guard;
                                break;
                            case 5:
                                this.counterTiming = true;
                                break;
                            case 12:
                                if (this.version == 0)
                                {
                                    if (this.version == 0)
                                        this.speed = 10;
                                    this.sound.PlaySE(SoundEffect.beamlong);
                                    this.jb = new JusticeBeam(this.sound, this.parent, this.position.X + this.UnionRebirth, this.position.Y, this.union, this.Power, 2);
                                    this.jb.positionDirect.Y += 6f;
                                    this.parent.attacks.Add(jb);
                                    break;
                                }
                                this.sound.PlaySE(SoundEffect.beam);
                                Beam beam = new Beam(this.sound, this.parent, this.position.X + this.UnionRebirth, this.position.Y, this.union, this.Power, 2, false);
                                beam.positionDirect.Y += 16f;
                                this.parent.attacks.Add(beam);
                                break;
                            case 14:
                                if (this.version == 0)
                                {
                                    point.X = this.union == Panel.COLOR.red ? 2 : 3;
                                    point.Y = 1;
                                    JusticeRing justiceRing = new JusticeRing(this.sound, this.parent, point.X, point.Y, this.union, this.Power, this.element);
                                    justiceRing.positionDirect.Y += 6f;
                                    this.parent.attacks.Add(justiceRing);
                                    break;
                                }
                                break;
                            case 16:
                                if (this.version == 0)
                                {
                                    point.X = this.union == Panel.COLOR.red ? 4 : 1;
                                    point.Y = 1;
                                    JusticeRing justiceRing = new JusticeRing(this.sound, this.parent, point.X, point.Y, this.union, this.Power, this.element);
                                    justiceRing.positionDirect.Y += 6f;
                                    this.parent.attacks.Add(justiceRing);
                                    break;
                                }
                                break;
                            case 18:
                                if (this.version == 0)
                                {
                                    point.X = this.union == Panel.COLOR.red ? 3 : 2;
                                    point.Y = 1;
                                    JusticeRing justiceRing = new JusticeRing(this.sound, this.parent, point.X, point.Y, this.union, this.Power, this.element);
                                    justiceRing.positionDirect.Y += 6f;
                                    this.parent.attacks.Add(justiceRing);
                                    break;
                                }
                                break;
                            case 20:
                                if (this.version == 0)
                                {
                                    point.X = this.union == Panel.COLOR.red ? 5 : 0;
                                    point.Y = 1;
                                    JusticeRing justiceRing = new JusticeRing(this.sound, this.parent, point.X, point.Y, this.union, this.Power, this.element);
                                    justiceRing.positionDirect.Y += 6f;
                                    this.parent.attacks.Add(justiceRing);
                                }
                                if (this.version == 0)
                                {
                                    this.jb.end = true;
                                    this.speed = 4;
                                }
                                this.motion = Gelpark.MOTION.attack2;
                                this.frame = 0;
                                this.counterTiming = false;
                                break;
                        }
                        break;
                    case Gelpark.MOTION.attack2:
                        this.animationpoint.X = this.AnimeAttack2(this.frame).X;
                        if (this.frame == 2)
                            this.guard = CharacterBase.GUARD.guard;
                        if (this.frame >= 3)
                        {
                            this.motion = Gelpark.MOTION.guard;
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

        private Point AnimeWake(int waitflame)
        {
            return this.Return(new int[4] { 0, 1, 2, 3 }, new int[4]
            {
        0,
        1,
        2,
        3
            }, 1, waitflame);
        }

        private Point AnimeWait(int waitflame)
        {
            return this.Return(new int[4] { 0, 8, 9, 10 }, new int[4]
            {
        3,
        3,
        4,
        3
            }, 1, waitflame);
        }

        private Point AnimeDown(int waitflame)
        {
            return this.Return(new int[4] { 0, 1, 2, 3 }, new int[4]
            {
        3,
        2,
        1,
        0
            }, 1, waitflame);
        }

        private Point AnimeAttack1(int waitflame)
        {
            return this.Return(new int[8]
            {
        0,
        1,
        2,
        3,
        4,
        5,
        10,
        80
            }, new int[8] { 3, 2, 1, 0, 6, 7, 8, 9 }, 1, waitflame);
        }

        private Point AnimeAttack2(int waitflame)
        {
            return this.Return(new int[4] { 0, 1, 2, 3 }, new int[4]
            {
        8,
        7,
        6,
        0
            }, 1, waitflame);
        }

        private enum MOTION
        {
            guard,
            wake,
            wait,
            close,
            attack1,
            attack2,
        }
    }
}

