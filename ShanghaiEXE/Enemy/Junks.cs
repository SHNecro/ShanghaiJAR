using NSAttack;
using NSBattle;
using NSChip;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;
using Common.Vectors;
using System;
using System.Drawing;

namespace NSEnemy
{
    internal class Junks : EnemyBase
    {
        private Junks.MOTION motion = Junks.MOTION.neutral;
        private readonly ClossBomb.TYPE type;

        public Junks(IAudioEngine s, SceneBattle p, int pX, int pY, byte n, Panel.COLOR u, byte v)
          : base(s, p, pX, pY, n, u, v)
        {
            this.helpPosition.X = -8;
            for (int index = 0; index < this.dropchips.Length; ++index)
                this.dropchips[index] = new ChipFolder(this.sound);
            this.picturename = "junks";
            this.race = EnemyBase.ENEMY.virus;
            this.Flying = false;
            this.wide = 40;
            this.height = 40;
            this.name = ShanghaiEXE.Translate("Enemy.JunksName1");
            this.printNumber = true;
            switch (this.version)
            {
                case 0:
                    this.name = ShanghaiEXE.Translate("Enemy.JunksName2");
                    this.printNumber = false;
                    this.power = 200;
                    this.hp = 1300;
                    this.type = ClossBomb.TYPE.big;
                    break;
                case 1:
                    this.power = 20;
                    this.hp = 70;
                    break;
                case 2:
                    this.power = 60;
                    this.hp = 140;
                    break;
                case 3:
                    this.power = 100;
                    this.hp = 180;
                    break;
                case 4:
                    this.name = ShanghaiEXE.Translate("Enemy.JunksName3");
                    this.printNumber = false;
                    this.power = 120;
                    this.hp = 230;
                    break;
                default:
                    this.name = ShanghaiEXE.Translate("Enemy.JunksName4") + (version - 3).ToString();
                    this.printNumber = false;
                    this.power = 120 + (version - 4) * 40;
                    this.hp = 230 + (version - 4) * 40;
                    break;
            }
            this.roop = this.number;
            this.hpmax = this.hp;
            this.speed = 7;
            this.hpprint = this.hp;
            this.printhp = true;
            this.effecting = false;
            this.PositionDirectSet();
            this.element = ChipBase.ELEMENT.earth;
            switch (this.version)
            {
                case 0:
                    this.dropchips[0].chip = new DustBomb(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new DustBomb(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new MagicBomb(this.sound);
                    this.dropchips[2].codeNo = 2;
                    this.dropchips[3].chip = new StarBomb(this.sound);
                    this.dropchips[3].codeNo = 2;
                    this.dropchips[4].chip = new DustBomb(this.sound);
                    this.dropchips[4].codeNo = 3;
                    this.havezenny = 1600;
                    this.speed = 5;
                    this.type = ClossBomb.TYPE.big;
                    break;
                case 1:
                    this.dropchips[0].chip = new DustBomb(this.sound);
                    this.dropchips[0].codeNo = 2;
                    this.dropchips[1].chip = new DustBomb(this.sound);
                    this.dropchips[1].codeNo = 2;
                    this.dropchips[2].chip = new DustBomb(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new DustBomb(this.sound);
                    this.dropchips[3].codeNo = 1;
                    this.dropchips[4].chip = new DustBomb(this.sound);
                    if (this.Random.Next(4) < 3)
                        this.dropchips[4].codeNo = 1;
                    else
                        this.dropchips[4].codeNo = 3;
                    this.havezenny = 300;
                    this.type = ClossBomb.TYPE.single;
                    break;
                case 2:
                    this.dropchips[0].chip = new MagicBomb(this.sound);
                    this.dropchips[0].codeNo = 3;
                    this.dropchips[1].chip = new MagicBomb(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new MagicBomb(this.sound);
                    this.dropchips[2].codeNo = 1;
                    this.dropchips[3].chip = new MagicBomb(this.sound);
                    this.dropchips[3].codeNo = 2;
                    this.dropchips[4].chip = new MagicBomb(this.sound);
                    this.dropchips[4].codeNo = 0;
                    this.havezenny = 600;
                    this.type = ClossBomb.TYPE.line;
                    break;
                case 3:
                    this.dropchips[0].chip = new StarBomb(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new StarBomb(this.sound);
                    this.dropchips[1].codeNo = 2;
                    this.dropchips[2].chip = new StarBomb(this.sound);
                    this.dropchips[2].codeNo = 2;
                    this.dropchips[3].chip = new StarBomb(this.sound);
                    this.dropchips[3].codeNo = 1;
                    this.dropchips[4].chip = new StarBomb(this.sound);
                    this.dropchips[4].codeNo = 3;
                    this.havezenny = 800;
                    this.type = ClossBomb.TYPE.closs;
                    break;
                default:
                    this.dropchips[0].chip = new DustBomb(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new DustBomb(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new MagicBomb(this.sound);
                    this.dropchips[2].codeNo = 2;
                    this.dropchips[3].chip = new StarBomb(this.sound);
                    this.dropchips[3].codeNo = 2;
                    this.dropchips[4].chip = new DustBomb(this.sound);
                    this.dropchips[4].codeNo = 3;
                    this.havezenny = 1600;
                    this.type = ClossBomb.TYPE.closs;
                    if (this.version >= 8)
                    {
                        this.dropchips[4].chip = new MagicBombX(this.sound);
                        this.dropchips[4].codeNo = this.Random.Next(4);
                        this.havezenny = 8000;
                        break;
                    }
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
            this.neutlal = this.motion == Junks.MOTION.neutral;
            if (this.moveflame)
            {
                this.positionre = this.position;
                switch (this.motion)
                {
                    case Junks.MOTION.neutral:
                        if (this.frame > 3)
                        {
                            int num = 3 - version;
                            if (this.version == 0)
                                num = 1;
                            if (this.roop > num && !this.badstatus[4] && this.parent.nowscene != SceneBattle.BATTLESCENE.end)
                            {
                                this.frame = 0;
                                this.motion = Junks.MOTION.attack;
                            }
                            else
                                ++this.roop;
                            this.frame = 0;
                            break;
                        }
                        break;
                    case Junks.MOTION.attack:
                        this.animationpoint.X = this.AnimeAttack(this.frame).X;
                        switch (this.frame)
                        {
                            case 3:
                                this.counterTiming = true;
                                break;
                            case 5:
                                if (this.parent.nowscene != SceneBattle.BATTLESCENE.end)
                                {
                                    this.sound.PlaySE(SoundEffect.canon);
                                    Point end = this.RandomTarget();
                                    this.parent.attacks.Add(new ClossBomb(this.sound, this.parent, this.positionre.X, this.positionre.Y, this.union, this.Power, 1, new Vector2(this.positionDirect.X + (this.union == Panel.COLOR.red ? 9f : -9f), this.positionDirect.Y - 9f), end, 40, this.type, false, ClossBomb.TYPE.big, false, false));
                                    break;
                                }
                                break;
                            case 6:
                                this.animationpoint.X = 0;
                                this.frame = 0;
                                this.roop = 0;
                                this.motion = Junks.MOTION.neutral;
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
            return this.Return(new int[4] { 0, 1, 4, 5 }, new int[4]
            {
        0,
        1,
        2,
        3
            }, 1, waitflame);
        }

        private enum MOTION
        {
            neutral,
            attack,
        }
    }
}

