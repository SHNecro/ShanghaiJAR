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
    internal class Bronzer : EnemyBase
    {
        private Bronzer.MOTION motion = Bronzer.MOTION.neutral;
        private readonly int manyshoot;
        private readonly NapalmBomb.TYPE type;

        public Bronzer(IAudioEngine s, SceneBattle p, int pX, int pY, byte n, Panel.COLOR u, byte v)
          : base(s, p, pX, pY, n, u, v)
        {
            for (int index = 0; index < this.dropchips.Length; ++index)
                this.dropchips[index] = new ChipFolder(this.sound);
            this.picturename = "bronzer";
            this.race = EnemyBase.ENEMY.virus;
            this.guard = CharacterBase.GUARD.guard;
            this.Flying = false;
            this.wide = 40;
            this.height = 32;
            this.printNumber = false;
            this.speed = 4;
            switch (this.version)
            {
                case 0:
                    this.manyshoot = 8;
                    this.name = ShanghaiEXE.Translate("Enemy.BronzerName1");
                    this.power = 150;
                    this.hp = 800;
                    this.speed = 1;
                    break;
                case 1:
                    this.manyshoot = 1;
                    this.name = ShanghaiEXE.Translate("Enemy.BronzerName2");
                    this.power = 20;
                    this.hp = 80;
                    break;
                case 2:
                    this.manyshoot = 2;
                    this.name = ShanghaiEXE.Translate("Enemy.BronzerName3");
                    this.power = 60;
                    this.hp = 100;
                    break;
                case 3:
                    this.manyshoot = 3;
                    this.name = ShanghaiEXE.Translate("Enemy.BronzerName4");
                    this.power = 120;
                    this.hp = 150;
                    break;
                case 4:
                    this.manyshoot = 4;
                    this.name = ShanghaiEXE.Translate("Enemy.BronzerName5");
                    this.power = 120;
                    this.hp = 180;
                    break;
                default:
                    this.manyshoot = 4;
                    this.power = 120 + (version - 4) * 20;
                    this.hp = 180 + (version - 4) * 40;
                    this.name = ShanghaiEXE.Translate("Enemy.BronzerName6") + (version - 3).ToString();
                    break;
            }
            this.type = NapalmBomb.TYPE.single;
            this.roop = this.number;
            this.hpmax = this.hp;
            this.hpprint = this.hp;
            this.printhp = true;
            this.effecting = false;
            this.PositionDirectSet();
            this.element = ChipBase.ELEMENT.normal;
            switch (this.version)
            {
                case 0:
                    this.dropchips[0].chip = new BronzeNapalm(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new BronzeNapalm(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new BronzeNapalm(this.sound);
                    this.dropchips[2].codeNo = 2;
                    this.dropchips[3].chip = new BronzeNapalm(this.sound);
                    this.dropchips[3].codeNo = 2;
                    this.dropchips[4].chip = new BronzeNapalm(this.sound);
                    if (this.Random.Next(4) < 3)
                        this.dropchips[4].codeNo = 0;
                    else
                        this.dropchips[4].codeNo = 3;
                    this.havezenny = 300;
                    break;
                case 1:
                    this.dropchips[0].chip = new BronzeNapalm(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new BronzeNapalm(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new BronzeNapalm(this.sound);
                    this.dropchips[2].codeNo = 2;
                    this.dropchips[3].chip = new BronzeNapalm(this.sound);
                    this.dropchips[3].codeNo = 2;
                    this.dropchips[4].chip = new BronzeNapalm(this.sound);
                    if (this.Random.Next(4) < 3)
                        this.dropchips[4].codeNo = 2;
                    else
                        this.dropchips[4].codeNo = 3;
                    this.havezenny = 300;
                    break;
                case 2:
                    this.dropchips[0].chip = new MetalNapalm(this.sound);
                    this.dropchips[0].codeNo = 1;
                    this.dropchips[1].chip = new MetalNapalm(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new MetalNapalm(this.sound);
                    this.dropchips[2].codeNo = 2;
                    this.dropchips[3].chip = new MetalNapalm(this.sound);
                    this.dropchips[3].codeNo = 2;
                    this.dropchips[4].chip = new MetalNapalm(this.sound);
                    this.dropchips[4].codeNo = 3;
                    this.havezenny = 600;
                    break;
                case 3:
                    this.dropchips[0].chip = new MithrillNapalm(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new MithrillNapalm(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new MithrillNapalm(this.sound);
                    this.dropchips[2].codeNo = 3;
                    this.dropchips[3].chip = new MithrillNapalm(this.sound);
                    this.dropchips[3].codeNo = 2;
                    this.dropchips[4].chip = new MithrillNapalm(this.sound);
                    this.dropchips[4].codeNo = 1;
                    this.havezenny = 800;
                    break;
                case 8:
                    this.dropchips[0].chip = new FuhathuNapalm(this.sound);
                    this.dropchips[0].codeNo = 3;
                    this.dropchips[1].chip = new FuhathuNapalm(this.sound);
                    this.dropchips[1].codeNo = 3;
                    this.dropchips[2].chip = new FuhathuNapalm(this.sound);
                    this.dropchips[2].codeNo = 2;
                    this.dropchips[3].chip = new FuhathuNapalm(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new MetalNapalmX(this.sound);
                    this.dropchips[4].codeNo = this.Random.Next(4);
                    this.havezenny = 8000;
                    break;
                default:
                    this.dropchips[0].chip = new FuhathuNapalm(this.sound);
                    this.dropchips[0].codeNo = 3;
                    this.dropchips[1].chip = new FuhathuNapalm(this.sound);
                    this.dropchips[1].codeNo = 3;
                    this.dropchips[2].chip = new FuhathuNapalm(this.sound);
                    this.dropchips[2].codeNo = 2;
                    this.dropchips[3].chip = new FuhathuNapalm(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new FuhathuNapalm(this.sound);
                    this.dropchips[4].codeNo = 1;
                    this.havezenny = 800;
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
            this.neutlal = this.motion == Bronzer.MOTION.neutral;
            if (this.moveflame)
            {
                this.positionre = this.position;
                switch (this.motion)
                {
                    case Bronzer.MOTION.neutral:
                        if (this.frame > 18)
                        {
                            if (this.roop > 3 - version && !this.badstatus[4] && this.parent.nowscene != SceneBattle.BATTLESCENE.end)
                            {
                                this.frame = 0;
                                this.motion = Bronzer.MOTION.attack;
                            }
                            else
                                ++this.roop;
                            this.frame = 0;
                            break;
                        }
                        break;
                    case Bronzer.MOTION.move:
                        this.animationpoint.X = this.AnimeMove(this.frame).X;
                        switch (this.frame)
                        {
                            case 4:
                                this.MoveRandom(false, false);
                                if (!(this.position == this.positionre))
                                {
                                    this.parent.effects.Add(new MoveEnemy(this.sound, this.parent, this.position.X, this.position.Y));
                                    this.position = this.positionre;
                                    this.PositionDirectSet();
                                    break;
                                }
                                break;
                            case 8:
                                this.motion = Bronzer.MOTION.neutral;
                                this.frame = 0;
                                break;
                        }
                        break;
                    case Bronzer.MOTION.attack:
                        this.animationpoint.X = this.AnimeAttack(this.frame).X;
                        switch (this.frame)
                        {
                            case 3:
                                this.guard = CharacterBase.GUARD.none;
                                break;
                            case 14:
                                this.counterTiming = true;
                                if (this.parent.nowscene != SceneBattle.BATTLESCENE.end)
                                {
                                    this.sound.PlaySE(SoundEffect.canon);
                                    Point end = this.RandomTarget();
                                    int heattime = 300;
                                    if (this.version == 0)
                                        heattime = 70;
                                    else if (this.version > 2)
                                        heattime = 100;
                                    Vector2 v = new Vector2(this.positionDirect.X + (this.union == Panel.COLOR.red ? 0.0f : 0.0f), this.positionDirect.Y - 8f);
                                    this.parent.attacks.Add(new NapalmBomb(this.sound, this.parent, this.positionre.X, this.positionre.Y, this.union, this.Power, 1, v, end, this.Random.Next(40, 60), this.type, heattime));
                                    for (int seed = 0; seed < (this.version != 0 ? this.Random.Next(this.manyshoot) : this.manyshoot); ++seed)
                                    {
                                        this.MoveRandom(false, false, this.union == Panel.COLOR.red ? Panel.COLOR.blue : Panel.COLOR.red, seed);
                                        Point positionre = this.positionre;
                                        this.parent.attacks.Add(new NapalmBomb(this.sound, this.parent, this.positionre.X, this.positionre.Y, this.union, this.Power, 1, v, positionre, this.Random.Next(40, 60), this.type, heattime));
                                    }
                                    break;
                                }
                                break;
                            case 24:
                                this.guard = CharacterBase.GUARD.guard;
                                this.animationpoint.X = 0;
                                this.frame = 0;
                                this.roop = 0;
                                this.motion = Bronzer.MOTION.move;
                                this.counterTiming = false;
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
            this._position = new Vector2((int)this.positionDirect.X + 10 + 4 * this.UnionRebirth + this.Shake.X, (int)this.positionDirect.Y + 2 + this.Shake.Y);
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

        private Point AnimeMove(int waitflame)
        {
            return CharacterAnimation.ReturnKai(new int[6]
            {
        1,
        1,
        2,
        2,
        1,
        1
            }, new int[6] { 0, 6, 7, 7, 6, 0 }, 1, waitflame);
        }

        private Point AnimeAttack(int waitflame)
        {
            return CharacterAnimation.ReturnKai(new int[9]
            {
        0,
        2,
        1,
        1,
        8,
        2,
        8,
        1,
        1
            }, new int[9] { 0, 1, 2, 3, 4, 5, 4, 1, 0 }, 1, waitflame);
        }

        private enum MOTION
        {
            neutral,
            move,
            attack,
        }
    }
}

