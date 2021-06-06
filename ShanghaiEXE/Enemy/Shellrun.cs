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
    internal class Shellrun : EnemyBase
    {
        private const int InternalHpRatio = 40;

        private Shellrun.MOTION motion = Shellrun.MOTION.neutral;
        private int count;
        private readonly int attackCount;
        private int flyflame;
        private Shadow shadow;
        private readonly int time;
        private Vector2 endposition;
        private float movex;
        private float movey;
        private float plusy;
        private float speedy;
        private float plusing;
        private const int startspeed = 6;
        private DammyEnemy dammy;

        public Shellrun(IAudioEngine s, SceneBattle p, int pX, int pY, byte n, Panel.COLOR u, byte v)
          : base(s, p, pX, pY, n, u, v)
        {
            this.wantedPosition.X = -8;
            this.attackCount = this.Random.Next(2, 5);
            for (int index = 0; index < this.dropchips.Length; ++index)
                this.dropchips[index] = new ChipFolder(this.sound);
            this.name = ShanghaiEXE.Translate("Enemy.ShellrunName1");
            this.picturename = "shelln";
            this.element = ChipBase.ELEMENT.aqua;
            this.race = EnemyBase.ENEMY.virus;
            this.Flying = false;
            this.Noslip = true;
            this.wide = 40;
            this.height = 40;
            this.hp = InternalHpRatio * 3 * version;
            this.hpmax = this.hp;
            this.power = 5 + 15 * (version - 1);
            this.speed = 5 - version;
            this.time = 60 - 10 * version;
            this.frame = -10 * (n - 1);
            this.printhp = true;
            this.printNumber = true;
            this.effecting = false;
            this.PositionDirectSet();
            switch (this.version)
            {
                case 0:
                    this.name = ShanghaiEXE.Translate("Enemy.ShellrunName2");
                    this.printNumber = false;
                    this.power = 150;
                    this.hp = 7 * InternalHpRatio;
                    this.hpmax = this.hp;
                    this.speed = 1;
                    this.time = 30;
                    this.dropchips[0].chip = new ShellArmor1(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new ShellArmor1(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new ShellArmor1(this.sound);
                    this.dropchips[2].codeNo = 2;
                    this.dropchips[3].chip = new ShellArmor1(this.sound);
                    this.dropchips[3].codeNo = 2;
                    this.dropchips[4].chip = new ShellArmor1(this.sound);
                    this.dropchips[4].codeNo = 3;
                    this.havezenny = 300;
                    break;
                case 1:
                    this.power = 15;
                    this.dropchips[0].chip = new ShellArmor1(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new ShellArmor1(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new ShellArmor1(this.sound);
                    this.dropchips[2].codeNo = 1;
                    this.dropchips[3].chip = new ShellArmor1(this.sound);
                    this.dropchips[3].codeNo = 2;
                    this.dropchips[4].chip = new ShellArmor1(this.sound);
                    if (this.Random.Next(4) < 3)
                        this.dropchips[4].codeNo = 2;
                    else
                        this.dropchips[4].codeNo = 3;
                    this.havezenny = 300;
                    break;
                case 2:
                    this.power = 60;
                    this.dropchips[0].chip = new ShellArmor2(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new ShellArmor2(this.sound);
                    this.dropchips[1].codeNo = 3;
                    this.dropchips[2].chip = new ShellArmor2(this.sound);
                    this.dropchips[2].codeNo = 1;
                    this.dropchips[3].chip = new ShellArmor2(this.sound);
                    this.dropchips[3].codeNo = 1;
                    this.dropchips[4].chip = new ShellArmor2(this.sound);
                    this.dropchips[4].codeNo = 2;
                    this.havezenny = 600;
                    break;
                case 3:
                    this.power = 90;
                    this.dropchips[0].chip = new ShellArmor3(this.sound);
                    this.dropchips[0].codeNo = 2;
                    this.dropchips[1].chip = new ShellArmor3(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new ShellArmor3(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new ShellArmor3(this.sound);
                    this.dropchips[3].codeNo = 1;
                    this.dropchips[4].chip = new ShellArmor3(this.sound);
                    this.dropchips[4].codeNo = 3;
                    this.havezenny = 800;
                    break;
                case 4:
                    this.power = 120;
                    this.name = ShanghaiEXE.Translate("Enemy.ShellrunName3");
                    this.speed = 1;
                    this.time = 20;
                    this.printNumber = false;
                    this.dropchips[0].chip = new ShellArmor3(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new ShellArmor3(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new ShellArmor3(this.sound);
                    this.dropchips[2].codeNo = 2;
                    this.dropchips[3].chip = new ShellArmor3(this.sound);
                    this.dropchips[3].codeNo = 2;
                    this.dropchips[4].chip = new ShellArmor3(this.sound);
                    this.dropchips[4].codeNo = 3;
                    this.havezenny = 800;
                    break;
                default:
                    this.power = 120 + 30 * (version - 3);
                    int num = version - 3;
                    this.name = ShanghaiEXE.Translate("Enemy.ShellrunName4") + num.ToString();
                    {
                        num = version - 3;
                    }
                    this.speed = 1;
                    this.time = 20;
                    this.printNumber = false;
                    this.dropchips[0].chip = new ShellArmor2(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new ShellArmor1(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new ShellArmor2(this.sound);
                    this.dropchips[2].codeNo = 2;
                    this.dropchips[3].chip = new ShellArmor3(this.sound);
                    this.dropchips[3].codeNo = 2;
                    this.dropchips[4].chip = new ShellArmor1(this.sound);
                    this.dropchips[4].codeNo = 3;
                    this.havezenny = 800;
                    if (this.version >= 8)
                    {
                        this.dropchips[4].chip = new ShellArmorX(this.sound);
                        this.dropchips[4].codeNo = this.Random.Next(4);
                        this.havezenny = 8000;
                        break;
                    }
                    break;
            }
            this.hpprint = this.hp;
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

        public override void InitAfter()
        {
            if (this.parent == null)
                return;
            this.shadow = new Shadow(this.sound, this.parent, this.position.X, this.position.Y, this);
            this.shadow.slide.X = this.union == Panel.COLOR.red ? -8 : 0;
            this.parent.effects.Add(shadow);
            this.shadow.PositionDirectSet(this.position);
            if (this.version > 0)
            {
                this.hp /= InternalHpRatio;
                this.hpprint = this.hp;
                this.hpmax = this.hp;
                this.guard = CharacterBase.GUARD.Sarmar;
            }
        }

        protected override void Moving()
        {
            if (this.shadow != null)
                this.shadow.positionDirect = new Vector2(this.positionDirect.X, this.positionDirect.Y + 8f);
            switch (this.motion)
            {
                case Shellrun.MOTION.neutral:
                    if ((uint)this.animationpoint.X > 0U)
                        this.animationpoint.X = 0;
                    if (this.moveflame && (this.frame > 30 || this.version == 0))
                    {
                        this.frame = 0;
                        if (!this.badstatus[4] && this.parent.nowscene != SceneBattle.BATTLESCENE.end && !this.HeviSand)
                        {
                            this.motion = Shellrun.MOTION.hop;
                            Point position = this.position;
                            this.Noslip = true;
                            this.counterTiming = true;
                            if (this.version > 0)
                                this.positionre = this.position;
                            else
                                this.MoveRandom(false, false);
                            this.animationpoint.X = 1;
                            this.Throw();
                            ++this.count;
                        }
                        break;
                    }
                    break;
                case Shellrun.MOTION.hop:
                    if (this.flyflame == this.time)
                    {
                        this.counterTiming = false;
                        this.animationpoint.X = 0;
                        this.sound.PlaySE(SoundEffect.quake);
                        this.ShakeStart(1, 10);
                        for (int index = 0; index < (this.version == 0 ? 1 : version); ++index)
                        {
                            Point point = this.RandomPanel(this.UnionEnemy);
                            this.parent.attacks.Add(new FallStone(this.sound, this.parent, point.X, point.Y, this.union, this.Power, 1, 20 * index, ChipBase.ELEMENT.normal));
                        }
                        this.motion = Shellrun.MOTION.neutral;
                        this.plusy = 0.0f;
                        this.frame = 0;
                        this.dammy.flag = false;
                    }
                    else
                    {
                        this.positionDirect.X -= this.movex;
                        this.positionDirect.Y -= this.movey;
                        this.plusy -= this.speedy;
                        this.speedy -= this.plusing;
                        this.nohit = speedy * (double)this.speedy < 25.0;
                        if (speedy < 0.0)
                            this.position = this.positionre;
                    }
                    ++this.flyflame;
                    break;
            }
            if (this.effecting && !this.nohit)
                this.AttackMake(this.Power, 0, 0);
            this.FlameControl();
            this.MoveAftar();
        }

        private void Throw()
        {
            this.endposition = new Vector2((float)(positionre.X * 40.0 + 26.0) + 6 * this.UnionRebirth, (float)(positionre.Y * 24.0 + 76.0));
            this.movex = (this.positionDirect.X - this.endposition.X) / time;
            this.movey = (this.positionDirect.Y - this.endposition.Y) / time;
            this.plusy = 0.0f;
            this.speedy = 6f;
            this.plusing = this.speedy / (this.time / 2);
            this.flyflame = 0;
        }

        public override void PositionDirectSet()
        {
            this.positionDirect = new Vector2((float)(position.X * 40.0 + 26.0) + 6 * this.UnionRebirth, (float)(position.Y * 24.0 + 76.0));
        }

        public override void Render(IRenderer dg)
        {
            this._position = new Vector2((int)this.positionDirect.X + this.Shake.X, (int)this.positionDirect.Y + this.plusy + Shake.Y);
            this._rect = new Rectangle(this.animationpoint.X * this.wide, Math.Min(this.version, (byte)4) * this.height, this.wide, this.height);
            if (this.version == 0)
                this._rect.Y = 5 * this.height;
            Rectangle rect = this._rect;
            rect.Y += this.height;
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
                rect = this._rect;
                rect.Y += this.height;
                dg.DrawImage(dg, this.picturename, this._rect, false, this._position, this.rebirth, this.color);
            }
            this.HPposition = new Vector2((int)this.positionDirect.X + 2, (int)this.positionDirect.Y + this.height / 2 - 40);
            this.Nameprint(dg, this.printNumber);
        }

        private enum MOTION
        {
            neutral,
            hop,
            attack,
        }
    }
}

