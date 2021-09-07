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
using System.Collections.Generic;
using System.Linq;

namespace NSEnemy
{
    internal class Juraigon : EnemyBase
    {
        private Juraigon.MOTION motion = Juraigon.MOTION.neutral;
        private readonly DammyEnemy[] dammyEnemy = new DammyEnemy[2];
        private bool dammyInit;
        private int roopneutral;
        private readonly int roopmove;
        private readonly int moveroop;
        private readonly int nspeed;
        private bool breathMode;
        private int breathCount;
        private int wideBreathTicks;
        private List<ElementFire> fireBreath;

        public Juraigon(IAudioEngine s, SceneBattle p, int pX, int pY, byte n, Panel.COLOR u, byte v)
          : base(s, p, pX, pY, n, u, v)
        {
            this.wantedPosition.Y = -16;
            this.helpPosition.X = -16;
            this.helpPosition.Y = -8;
            for (int index = 0; index < this.dropchips.Length; ++index)
                this.dropchips[index] = new ChipFolder(this.sound);
            this.picturename = "juraigon";
            this.race = EnemyBase.ENEMY.virus;
            this.Flying = false;
            this.wide = 120;
            this.height = 64;
            this.printNumber = true;
            this.noslip = true;
            this.name = ShanghaiEXE.Translate("Enemy.JuraigonName1");
            switch (this.version)
            {
                case 0:
                    this.printNumber = false;
                    this.name = ShanghaiEXE.Translate("Enemy.JuraigonName2");
                    this.power = 250;
                    this.hp = 5000;
                    this.moveroop = 2;
                    this.nspeed = 4;
                    break;
                case 1:
                    this.power = 80;
                    this.hp = 400;
                    this.moveroop = 2;
                    this.nspeed = 8;
                    break;
                case 2:
                    this.power = 120;
                    this.hp = 600;
                    this.moveroop = 2;
                    this.nspeed = 7;
                    break;
                case 3:
                    this.power = 180;
                    this.hp = 1000;
                    this.moveroop = 2;
                    this.nspeed = 6;
                    break;
                case 4:
                    this.printNumber = false;
                    this.name = ShanghaiEXE.Translate("Enemy.JuraigonName3");
                    this.power = 200;
                    this.hp = 1500;
                    this.moveroop = 2;
                    this.nspeed = 5;
                    break;
                default:
                    this.power = 200 + (version - 4) * 20;
                    this.hp = 1500 + (version - 4) * 100;
                    this.printNumber = false;
                    int num = version - 3;
                    this.name = ShanghaiEXE.Translate("Enemy.JuraigonName4") + num.ToString();
                    this.moveroop = 2;
                    this.nspeed = 5;
                    {
                        num = version - 3;
                        break;
                    }
            }
            this.speed = this.nspeed;
            this.roop = this.number;
            this.hpmax = this.hp;
            this.hpprint = this.hp;
            this.printhp = true;
            this.effecting = false;
            this.PositionDirectSet();
            this.element = ChipBase.ELEMENT.heat;
            this.fireBreath = new List<ElementFire>();
            switch (this.version)
            {
                case 0:
                    this.dropchips[0].chip = new DragnoBreath3(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new DragnoBreath3(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new DragnoBreath3(this.sound);
                    this.dropchips[2].codeNo = 2;
                    this.dropchips[3].chip = new DragnoBreath3(this.sound);
                    this.dropchips[3].codeNo = 2;
                    this.dropchips[4].chip = new DragnoBreath3(this.sound);
                    if (this.Random.Next(4) < 3)
                        this.dropchips[4].codeNo = 0;
                    else
                        this.dropchips[4].codeNo = 3;
                    this.havezenny = 300;
                    break;
                case 1:
                    this.dropchips[0].chip = new DragnoBreath1(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new DragnoBreath1(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new DragnoBreath1(this.sound);
                    this.dropchips[2].codeNo = 2;
                    this.dropchips[3].chip = new DragnoBreath1(this.sound);
                    this.dropchips[3].codeNo = 1;
                    this.dropchips[4].chip = new DragnoBreath1(this.sound);
                    if (this.Random.Next(4) < 3)
                        this.dropchips[4].codeNo = 1;
                    else
                        this.dropchips[4].codeNo = 3;
                    this.havezenny = 300;
                    break;
                case 2:
                    this.dropchips[0].chip = new DragnoBreath2(this.sound);
                    this.dropchips[0].codeNo = 3;
                    this.dropchips[1].chip = new DragnoBreath2(this.sound);
                    this.dropchips[1].codeNo = 3;
                    this.dropchips[2].chip = new DragnoBreath2(this.sound);
                    this.dropchips[2].codeNo = 1;
                    this.dropchips[3].chip = new DragnoBreath2(this.sound);
                    this.dropchips[3].codeNo = 2;
                    this.dropchips[4].chip = new DragnoBreath2(this.sound);
                    this.dropchips[4].codeNo = 0;
                    this.havezenny = 600;
                    break;
                case 3:
                    this.dropchips[0].chip = new DragnoBreath3(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new DragnoBreath3(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new DragnoBreath3(this.sound);
                    this.dropchips[2].codeNo = 1;
                    this.dropchips[3].chip = new DragnoBreath3(this.sound);
                    this.dropchips[3].codeNo = 3;
                    this.dropchips[4].chip = new DragnoBreath3(this.sound);
                    this.dropchips[4].codeNo = 2;
                    this.havezenny = 800;
                    break;
                default:
                    this.dropchips[0].chip = new DragnoBreath1(this.sound);
                    this.dropchips[0].codeNo = 3;
                    this.dropchips[1].chip = new DragnoBreath1(this.sound);
                    this.dropchips[1].codeNo = 3;
                    this.dropchips[2].chip = new DragnoBreath3(this.sound);
                    this.dropchips[2].codeNo = 2;
                    this.dropchips[3].chip = new DragnoBreath3(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new DragnoBreath1(this.sound);
                    this.dropchips[4].codeNo = 3;
                    this.havezenny = 800;
                    break;
            }
            this.neutlal = true;
        }

        public override void Init()
        {
            if (this.parent == null)
                return;
            this.parent.effects.Add(new EnemyShadow(this.sound, this.parent, this, this.union == Panel.COLOR.red));
        }

        public override void PositionDirectSet()
        {
            this.positionDirect = new Vector2((float)(position.X * 40.0 + 6.0) - 12 * this.UnionRebirth, (float)(position.Y * 24.0 + 56.0));
        }

        protected override void Moving()
        {
            if (!this.dammyInit)
            {
                this.dammyInit = true;
                this.dammyEnemy[0] = new DammyEnemy(this.sound, this.parent, this.UnionRebirth, 0, this, true);
                this.dammyEnemy[1] = new DammyEnemy(this.sound, this.parent, -1 * this.UnionRebirth, 0, this, false);
                this.parent.enemys.Add(this.dammyEnemy[0]);
                this.parent.enemys.Add(this.dammyEnemy[1]);
                this.dammyEnemy[0].nohit = true;
                this.dammyEnemy[0].noslip = true;
                this.dammyEnemy[1].noslip = true;
            }
            this.neutlal = this.motion == Juraigon.MOTION.neutral;
            if (this.moveflame)
            {
                this.positionre = this.position;
                switch (this.motion)
                {
                    case Juraigon.MOTION.neutral:
                        this.animationpoint = this.AnimeNeutral(this.frame);
                        if (this.frame >= 7)
                        {
                            this.frame = 0;
                            ++this.roopneutral;
                            if (this.roopneutral >= 2 && !this.badstatus[4] && this.parent.nowscene != SceneBattle.BATTLESCENE.end)
                            {
                                this.frame = 0;
                                this.counterTiming = true;
                                if (this.version > 0)
                                {
                                    this.motion = Juraigon.MOTION.attack;
                                }
                                else
                                {
                                    int num = this.Random.Next(3);
                                    this.speed = 3;
                                    switch (num)
                                    {
                                        case 0:
                                            this.motion = Juraigon.MOTION.attack;
                                            break;
                                        case 1:
                                            this.breathMode = true;
                                            this.sound.PlaySE(SoundEffect.dragonVoice);
                                            this.motion = Juraigon.MOTION.attack3;
                                            break;
                                        case 2:
                                            this.motion = Juraigon.MOTION.attack4;
                                            break;
                                    }
                                }
                            }
                            else
                                ++this.roop;
                            this.frame = 0;
                            break;
                        }
                        break;
                    case Juraigon.MOTION.move:
                        this.animationpoint = this.AnimeNeutral(this.frame);
                        this.MoveRandom(false, this.breathMode);
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
                        this.motion = !this.breathMode ? Juraigon.MOTION.neutral : Juraigon.MOTION.attack3;
                        break;
                    case Juraigon.MOTION.attack:
                        this.animationpoint.X = this.AnimeAttack1(this.frame).X;
                        if (this.frame == 4)
                        {
                            this.frame = 0;
                            this.speed = 4;
                            this.guard = CharacterBase.GUARD.none;
                            this.counterTiming = false;
                            this.motion = Juraigon.MOTION.attack2;
                            break;
                        }
                        break;
                    case Juraigon.MOTION.attack2:
                        this.animationpoint.X = this.AnimeAttack2(this.frame % 2).X;
                        int num1 = 8;
                        this.wideBreathTicks++;
                        switch (this.wideBreathTicks)
                        {
                            case 1:
                                this.counterTiming = false;
                                this.dammyEnemy[0].nohit = false;
                                this.dammyEnemy[0].effecting = false;
                                break;
                            case 8:
                                this.sound.PlaySE(SoundEffect.quake);
                                var attackBase1 = new ElementFire(this.sound, this.parent, this.position.X + 2 * this.UnionRebirth, this.position.Y, this.union, this.Power, 18, this.element, false, 1);
                                attackBase1.positionDirect.Y += num1;
                                this.parent.attacks.Add(attackBase1);
                                this.fireBreath.Add(attackBase1);
                                break;
                            case 11:
                                this.sound.PlaySE(SoundEffect.quake);
                                int num2 = 3;
                                var attackBase2 = new ElementFire(this.sound, this.parent, this.position.X + num2 * this.UnionRebirth, this.position.Y - 1, this.union, this.Power, 18, this.element, false, 1);

                                attackBase2.positionDirect.Y += num1;
                                this.parent.attacks.Add(attackBase2);
                                var attackBase3 = new ElementFire(this.sound, this.parent, this.position.X + num2 * this.UnionRebirth, this.position.Y, this.union, this.Power, 18, this.element, false, 1);
                                attackBase3.positionDirect.Y += num1;
                                this.parent.attacks.Add(attackBase3);
                                var attackBase4 = new ElementFire(this.sound, this.parent, this.position.X + num2 * this.UnionRebirth, this.position.Y + 1, this.union, this.Power, 18, this.element, false, 1);
                                attackBase4.positionDirect.Y += num1;
                                this.parent.attacks.Add(attackBase4);
                                this.fireBreath.Add(attackBase2);
                                this.fireBreath.Add(attackBase3);
                                this.fireBreath.Add(attackBase4);
                                break;
                            case 14:
                                this.sound.PlaySE(SoundEffect.quake);
                                int num3 = 4;
                                var attackBase5 = new ElementFire(this.sound, this.parent, this.position.X + num3 * this.UnionRebirth, this.position.Y - 1, this.union, this.Power, 18, this.element, false, 1);
                                attackBase5.positionDirect.Y += num1;
                                this.parent.attacks.Add(attackBase5);
                                var attackBase6 = new ElementFire(this.sound, this.parent, this.position.X + num3 * this.UnionRebirth, this.position.Y, this.union, this.Power, 18, this.element, false, 1);
                                attackBase6.positionDirect.Y += num1;
                                this.parent.attacks.Add(attackBase6);
                                var attackBase7 = new ElementFire(this.sound, this.parent, this.position.X + num3 * this.UnionRebirth, this.position.Y + 1, this.union, this.Power, 18, this.element, false, 1);
                                attackBase7.positionDirect.Y += num1;
                                this.parent.attacks.Add(attackBase7);
                                this.fireBreath.Add(attackBase5);
                                this.fireBreath.Add(attackBase6);
                                this.fireBreath.Add(attackBase7);
                                break;
                        }
                        if (this.frame >= 60)
                        {
                            this.wideBreathTicks = 0;
                            this.frame = 0;
                            this.dammyEnemy[0].nohit = true;
                            this.dammyEnemy[0].effecting = true;
                            this.motion = Juraigon.MOTION.move;
                            this.speed = this.nspeed;
                            this.fireBreath.Clear();
                            this.waittime = 0;
                        }
                        break;
                    case Juraigon.MOTION.attack3:
                        Point point1;
                        if (this.frame < 6)
                        {
                            ref Point local = ref this.animationpoint;
                            point1 = this.AnimeAttack3(this.frame);
                            int x = point1.X;
                            local.X = x;
                        }
                        else
                            this.animationpoint.X = this.AnimeAttack2(this.frame % 2).X;
                        switch (this.frame)
                        {
                            case 4:
                                this.counterTiming = false;
                                this.dammyEnemy[0].nohit = false;
                                this.dammyEnemy[0].effecting = false;
                                break;
                            case 12:
                                point1 = this.RandomTarget();
                                int x1 = point1.X;
                                int num4 = 2;
                                int num5 = 8;
                                AttackBase attackBase8 = new FireBreath(this.sound, this.parent, this.position.X + num4 * this.UnionRebirth, this.position.Y, this.union, this.Power, 2, this.element, x1);
                                attackBase8.positionDirect.Y += num5;
                                this.parent.attacks.Add(attackBase8);
                                break;
                            case 16:
                                if (this.breathCount < 3)
                                {
                                    this.frame = 0;
                                    this.dammyEnemy[0].nohit = true;
                                    this.dammyEnemy[0].effecting = true;
                                    ++this.breathCount;
                                    this.motion = Juraigon.MOTION.move;
                                    break;
                                }
                                break;
                            case 40:
                                this.motion = Juraigon.MOTION.move;
                                this.frame = 0;
                                this.dammyEnemy[0].nohit = true;
                                this.dammyEnemy[0].effecting = true;
                                this.breathCount = 0;
                                this.breathMode = false;
                                break;
                        }
                        break;
                    case Juraigon.MOTION.attack4:
                        if (this.frame < 6)
                            this.animationpoint.X = this.AnimeAttack3(this.frame).X;
                        else
                            this.animationpoint.X = this.AnimeAttack2(this.frame % 2).X;
                        switch (this.frame)
                        {
                            case 4:
                                this.counterTiming = false;
                                this.dammyEnemy[0].nohit = false;
                                this.dammyEnemy[0].effecting = false;
                                this.sound.PlaySE(SoundEffect.dragonVoice);
                                this.ShakeStart(4, 180);
                                for (int index = 0; index < 6; ++index)
                                {
                                    Point point2 = this.RandomPanel(this.UnionEnemy);
                                    this.parent.attacks.Add(new FallStone(this.sound, this.parent, point2.X, point2.Y, this.union, this.Power, 1, 20 * index, ChipBase.ELEMENT.normal));
                                }
                                break;
                            case 60:
                                this.motion = Juraigon.MOTION.neutral;
                                this.frame = 0;
                                this.dammyEnemy[0].nohit = true;
                                this.dammyEnemy[0].effecting = true;
                                this.breathCount = 0;
                                this.breathMode = false;
                                break;
                        }
                        break;
                }
            }
            this.MoveAftar();
            this.FlameControl();
        }

        public override void Updata()
        {
            base.Updata();
            if (this.badstatus[3] && this.badstatustime[3] != 0)
            {
                this.waittime++;
                this.wideBreathTicks = 0;
                if (this.waittime % (this.speed * 4) == 0)
                {
                    var oldestBreathLifetime = this.fireBreath.Where(ef => ef.flag).OrderByDescending(ef => ef.count).FirstOrDefault()?.count ?? -1;
                    var oldestBreaths = this.fireBreath.Where(ef => ef.flag && ef.count == oldestBreathLifetime).ToArray();
                    foreach (var oldestBreath in oldestBreaths)
                    {
                        oldestBreath.count = 20;
                        this.fireBreath.Remove(oldestBreath);
                    }

                    if (oldestBreathLifetime == -1)
                    {
                        this.waittime = 0;
                    }
                }
            }
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
            this.HPposition = new Vector2(this.positionDirect.X + 16f + 16 * this.UnionRebirth, this.positionDirect.Y + 2f - this.height / 2);
            this.Nameprint(dg, this.printNumber);
        }

        private Point AnimeNeutral(int waitflame)
        {
            return CharacterAnimation.ReturnKai(new int[9]
            {
        1,
        1,
        1,
        1,
        1,
        1,
        1,
        1,
        1
            }, new int[9] { 0, 1, 2, 3, 4, 5, 6, 7, 0 }, 1, waitflame);
        }

        private Point AnimeAttack1(int waitflame)
        {
            return CharacterAnimation.ReturnKai(new int[3]
            {
        1,
        1,
        2
            }, new int[3] { 0, 8, 9 }, 1, waitflame);
        }

        private Point AnimeAttack2(int waitflame)
        {
            return CharacterAnimation.ReturnKai(new int[2]
            {
        0,
        1
            }, new int[2] { 10, 11 }, 1, waitflame);
        }

        private Point AnimeAttack3(int waitflame)
        {
            return CharacterAnimation.ReturnKai(new int[5]
            {
        1,
        1,
        1,
        1,
        1
            }, new int[5] { 0, 8, 9, 10, 11 }, 1, waitflame);
        }

        private enum MOTION
        {
            neutral,
            move,
            attack,
            attack2,
            attack3,
            attack4,
        }
    }
}

