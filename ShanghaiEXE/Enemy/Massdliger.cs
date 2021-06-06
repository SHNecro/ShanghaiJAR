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
    internal class Massdliger : EnemyBase
    {
        private Massdliger.MOTION motion = Massdliger.MOTION.neutral;
        private int HPold;
        private int targetX;
        private int roopneutral;

        public Massdliger(IAudioEngine s, SceneBattle p, int pX, int pY, byte n, Panel.COLOR u, byte v)
          : base(s, p, pX, pY, n, u, v)
        {
            this.helpPosition.X = -8;
            this.wantedPosition.X = -4;
            for (int index = 0; index < this.dropchips.Length; ++index)
                this.dropchips[index] = new ChipFolder(this.sound);
            this.picturename = "massdliger";
            this.race = EnemyBase.ENEMY.virus;
            this.Flying = false;
            this.wide = 56;
            this.height = 48;
            this.printNumber = true;
            this.name = ShanghaiEXE.Translate("Enemy.MassdligerName1");
            switch (this.version)
            {
                case 0:
                    this.name = ShanghaiEXE.Translate("Enemy.MassdligerName2");
                    this.power = 200;
                    this.hp = 1300;
                    this.speed = 7;
                    break;
                case 1:
                    this.power = 50;
                    this.hp = 100;
                    this.speed = 7;
                    break;
                case 2:
                    this.power = 80;
                    this.hp = 120;
                    this.speed = 5;
                    break;
                case 3:
                    this.power = 120;
                    this.hp = 200;
                    this.speed = 4;
                    break;
                case 4:
                    this.printNumber = false;
                    this.name = ShanghaiEXE.Translate("Enemy.MassdligerName3");
                    this.power = 150;
                    this.hp = 280;
                    this.speed = 3;
                    break;
                default:
                    this.printNumber = false;
                    this.name = ShanghaiEXE.Translate("Enemy.MassdligerName4") + (version - 3).ToString();
                    this.power = 150 + (version - 4) * 20;
                    this.hp = 260 + (version - 4) * 20;
                    this.speed = 3;
                    break;
            }
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
                    this.dropchips[0].chip = new Railgun1(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new Railgun1(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new Railgun1(this.sound);
                    this.dropchips[2].codeNo = 2;
                    this.dropchips[3].chip = new Railgun1(this.sound);
                    this.dropchips[3].codeNo = 1;
                    this.dropchips[4].chip = new Railgun1(this.sound);
                    if (this.Random.Next(4) < 3)
                        this.dropchips[4].codeNo = 1;
                    else
                        this.dropchips[4].codeNo = 3;
                    this.havezenny = 300;
                    break;
                case 2:
                    this.dropchips[0].chip = new Railgun2(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new Railgun2(this.sound);
                    this.dropchips[1].codeNo = 3;
                    this.dropchips[2].chip = new Railgun2(this.sound);
                    this.dropchips[2].codeNo = 1;
                    this.dropchips[3].chip = new Railgun2(this.sound);
                    this.dropchips[3].codeNo = 1;
                    this.dropchips[4].chip = new Railgun2(this.sound);
                    this.dropchips[4].codeNo = 2;
                    this.havezenny = 600;
                    break;
                case 3:
                    this.dropchips[0].chip = new Railgun3(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new Railgun3(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new Railgun3(this.sound);
                    this.dropchips[2].codeNo = 1;
                    this.dropchips[3].chip = new Railgun3(this.sound);
                    this.dropchips[3].codeNo = 2;
                    this.dropchips[4].chip = new Railgun3(this.sound);
                    this.dropchips[4].codeNo = 3;
                    this.havezenny = 800;
                    break;
                default:
                    this.dropchips[0].chip = new Railgun1(this.sound);
                    this.dropchips[0].codeNo = 1;
                    this.dropchips[1].chip = new Railgun2(this.sound);
                    this.dropchips[1].codeNo = 2;
                    this.dropchips[2].chip = new Railgun2(this.sound);
                    this.dropchips[2].codeNo = 2;
                    this.dropchips[3].chip = new Railgun3(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new Railgun1(this.sound);
                    this.dropchips[4].codeNo = 3;
                    this.havezenny = 800;
                    if (this.version >= 8)
                    {
                        this.dropchips[4].chip = new RailgunX(this.sound);
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
            this.positionDirect = new Vector2((float)(position.X * 40.0 + 14.0) - 4 * this.UnionRebirth, (float)(position.Y * 24.0 + 68.0));
        }

        protected override void Moving()
        {
            this.neutlal = this.motion == Massdliger.MOTION.neutral;
            if (this.moveflame)
            {
                this.positionre = this.position;
                switch (this.motion)
                {
                    case Massdliger.MOTION.neutral:
                        if (this.moveflame)
                        {
                            if (this.HPold < this.Hp)
                                this.HPold = this.Hp;
                            this.animationpoint = this.AnimeNeutral(this.frame);
                            if ((this.Hp < this.HPold || this.version == 0) && this.parent.nowscene != SceneBattle.BATTLESCENE.end)
                            {
                                this.roopneutral = 0;
                                if (!this.badstatus[4])
                                {
                                    this.frame = 0;
                                    this.sound.PlaySE(SoundEffect.rockon);
                                    this.counterTiming = true;
                                    this.motion = Massdliger.MOTION.attack;
                                }
                            }
                            break;
                        }
                        break;
                    case Massdliger.MOTION.move:
                        this.motion = Massdliger.MOTION.neutral;
                        this.MoveRandom(false, false);
                        if (this.position == this.positionre)
                        {
                            this.frame = 0;
                            this.motion = Massdliger.MOTION.neutral;
                            this.roopneutral = 0;
                            break;
                        }
                        this.parent.effects.Add(new MoveEnemy(this.sound, this.parent, this.position.X, this.position.Y));
                        this.position = this.positionre;
                        this.PositionDirectSet();
                        this.frame = 0;
                        this.roopneutral = 0;
                        break;
                    case Massdliger.MOTION.attack:
                        this.animationpoint.X = this.AnimeAttack(this.frame).X;
                        switch (this.frame)
                        {
                            case 3:
                                this.sound.PlaySE(SoundEffect.canon);
                                this.ShakeStart(5, 5);
                                this.parent.attacks.Add(new BustorShot(this.sound, this.parent, this.position.X + this.UnionRebirth, this.position.Y, this.union, this.Power, BustorShot.SHOT.railgun, ChipBase.ELEMENT.eleki, false, 0));
                                break;
                            case 5:
                                this.targetX += this.union == Panel.COLOR.red ? 1 : -1;
                                this.frame = 0;
                                this.counterTiming = false;
                                if (this.targetX < 0 || this.targetX > 5)
                                {
                                    this.animationpoint.X = 0;
                                    this.roop = 0;
                                    this.HPold = this.Hp;
                                    this.motion = Massdliger.MOTION.move;
                                    this.animationpoint = this.AnimeNeutral(this.frame);
                                    break;
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

        private Point AnimeNeutral(int waitflame)
        {
            return this.Return(new int[1], new int[1], 1, waitflame);
        }

        private Point AnimeAttack(int waitflame)
        {
            return this.Return(new int[6] { 0, 1, 2, 3, 4, 5 }, new int[6]
            {
        0,
        1,
        2,
        3,
        4,
        5
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

