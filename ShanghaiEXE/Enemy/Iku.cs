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
    internal class Iku : NaviBase
    {
        private readonly int[] powers = new int[6]
        {
      20,
      10,
      0,
      40,
      40,
      0
        };
        private readonly int[] drillY = new int[2];
        private const int nspeed = 8;
        private readonly int moveroop;
        private Iku.ATTACK attack;
        private int roopneutral;
        private int roopmove;
        private bool UpDown;
        private int movetime;
        private readonly int movespeed;
        private int movelong;
        private MetalDorill drillBreak;
        private readonly bool ballShot;
        private int cancelCount;
        private bool drilinvi;
        private int atackroop;
        private readonly bool atack;
        private readonly bool back;
        private int attackCount;
        private Point target;

        public Iku(IAudioEngine s, SceneBattle p, int pX, int pY, byte n, Panel.COLOR u, byte v)
          : base(s, p, pX, pY, n, u, v)
        {
            for (int index = 0; index < this.dropchips.Length; ++index)
                this.dropchips[index] = new ChipFolder(this.sound);
            this.element = ChipBase.ELEMENT.eleki;
            switch (this.version)
            {
                case 1:
                    this.name = ShanghaiEXE.Translate("Enemy.IkuName1");
                    this.power = 30;
                    this.hp = 1000;
                    this.moveroop = 2;
                    this.movespeed = 3;
                    break;
                case 2:
                    this.name = ShanghaiEXE.Translate("Enemy.IkuName2");
                    this.power = 60;
                    this.hp = 1200;
                    this.moveroop = 2;
                    this.movespeed = 2;
                    break;
                case 3:
                    this.name = ShanghaiEXE.Translate("Enemy.IkuName3");
                    this.power = 90;
                    this.hp = 1400;
                    this.moveroop = 2;
                    this.movespeed = 2;
                    break;
                case 4:
                    this.name = ShanghaiEXE.Translate("Enemy.IkuName4");
                    this.power = 120;
                    this.hp = 1900;
                    this.moveroop = 2;
                    this.movespeed = 1;
                    break;
                default:
                    this.name = ShanghaiEXE.Translate("Enemy.IkuName5") + (version - 3).ToString();
                    this.power = 150;
                    this.hp = 2000 + (version - 4) * 500;
                    this.moveroop = 2;
                    this.movespeed = 1;
                    break;
            }
            this.picturename = "iku";
            this.race = EnemyBase.ENEMY.navi;
            this.Flying = true;
            this.wide = 80;
            this.height = 72;
            this.hpmax = this.hp;
            this.speed = 8;
            this.hpprint = this.hp;
            this.printhp = true;
            this.effecting = false;
            this.roopmove = 0;
            this.PositionDirectSet();
            switch (this.version)
            {
                case 1:
                    this.dropchips[0].chip = new IkuV1(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new IkuV1(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new IkuV1(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new IkuV1(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new IkuV1(this.sound);
                    this.dropchips[4].codeNo = 0;
                    this.havezenny = 0;
                    break;
                case 2:
                    this.dropchips[0].chip = new IkuV2(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new IkuV2(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new IkuV2(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new IkuV2(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new IkuV2(this.sound);
                    this.dropchips[4].codeNo = 0;
                    this.havezenny = 0;
                    break;
                case 3:
                    this.dropchips[0].chip = new IkuV1(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new IkuV1(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new IkuV2(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new IkuV2(this.sound);
                    this.dropchips[3].codeNo = 0;
                    if (this.Random.Next(4) < 3)
                    {
                        this.dropchips[4].chip = new IkuV3(this.sound);
                        this.dropchips[4].codeNo = 0;
                    }
                    else
                    {
                        this.dropchips[4].chip = new ElekiDrill(this.sound);
                        this.dropchips[4].codeNo = 0;
                    }
                    this.havezenny = 2500;
                    break;
                default:
                    this.dropchips[0].chip = new IkuV1(this.sound);
                    this.dropchips[0].codeNo = 1;
                    this.dropchips[1].chip = new IkuV2(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new IkuV2(this.sound);
                    this.dropchips[2].codeNo = 1;
                    this.dropchips[3].chip = new IkuV1(this.sound);
                    this.dropchips[3].codeNo = 2;
                    this.dropchips[4].chip = new IkuV3(this.sound);
                    this.dropchips[4].codeNo = 1;
                    this.havezenny = 9000;
                    if (this.version < 8)
                        break;
                    this.dropchips[4].chip = new IkuX(this.sound);
                    this.dropchips[4].codeNo = this.Random.Next(4);
                    this.havezenny = 18000;
                    break;
            }
        }

        public override void PositionDirectSet()
        {
            if (this.position.Y < 0)
                this.position.Y = 0;
            if (this.position.Y > 2)
                this.position.Y = 2;
            if (!(this.motion == NaviBase.MOTION.attack && this.attack == Iku.ATTACK.drillBreak))
            {
                this.positionDirect = new Vector2((float)(position.X * 40.0 + 20.0), (float)(position.Y * 24.0 + 54.0));
            }
        }

        protected override void Moving()
        {
            this.neutlal = this.Motion == NaviBase.MOTION.neutral;
            switch (this.Motion)
            {
                case NaviBase.MOTION.neutral:
                    if (this.moveflame)
                        ++this.waittime;
                    ++this.movetime;
                    if (!this.HeviSand && this.SlideMove(1f / movespeed, this.UpDown ? 2 : 3))
                    {
                        int num = this.UpDown ? -1 : 1;
                        Point position = this.position;
                        position.Y += num;
                        ++this.roopmove;
                        if (!this.Canmove(position, this.number))
                            this.UpDown = !this.UpDown;
                        if (this.roopmove == this.moveroop && (this.ballShot || this.Hp >= this.HpMax / 2))
                        {
                            this.powerPlus = this.powers[5] * version;
                            this.sound.PlaySE(SoundEffect.eriasteal2);
                            this.parent.attacks.Add(new SlowThunder(this.sound, this.parent, this.position.X + this.UnionRebirth(this.union), this.position.Y, this.union, this.Power, ChipBase.ELEMENT.eleki, 1, 1, true));
                        }
                    }
                    if (this.moveflame)
                        this.animationpoint = this.AnimeNeutral(this.waittime);
                    if (this.waittime >= 4 || this.atack)
                        this.waittime = 0;
                    if (this.parent.nowscene != SceneBattle.BATTLESCENE.end && ((this.roopmove > this.moveroop || this.HeviSand) && !this.badstatus[4]))
                    {
                        ++this.atackroop;
                        this.roopneutral = 0;
                        this.roopmove = 0;
                        this.speed = Math.Max(2, 4 - version / 2);
                        if (!this.atack)
                        {
                            int index = this.Random.Next(this.Hp >= this.HpMax / 2 ? 2 : 8);
                            if (index >= 2)
                                index = 3;
                            if (index == 3)
                                this.speed = Math.Max(2, 3);
                            this.frame = 0;
                            this.attack = (Iku.ATTACK)index;
                            this.powerPlus = this.powers[index];
                        }
                        this.waittime = 0;
                        this.positionre = this.position;
                        this.PositionDirectSet();
                        this.Motion = NaviBase.MOTION.attack;
                        this.counterTiming = true;
                        break;
                    }
                    break;
                case NaviBase.MOTION.attack:
                    if (this.moveflame)
                        ++this.waittime;
                    if (this.moveflame)
                    {
                        switch (this.attack)
                        {
                            case Iku.ATTACK.lineThunder:
                                this.animationpoint = this.AnimeFever(this.waittime);
                                switch (this.waittime)
                                {
                                    case 1:
                                        this.target = this.RandomTarget();
                                        this.target.Y = 0;
                                        this.parent.attacks.Add(new Dummy(this.sound, this.parent, this.target.X, 0, this.union, new Point(), 13, true));
                                        this.parent.attacks.Add(new Dummy(this.sound, this.parent, this.target.X, 1, this.union, new Point(), 16, true));
                                        this.parent.attacks.Add(new Dummy(this.sound, this.parent, this.target.X, 2, this.union, new Point(), 19, true));
                                        break;
                                    case 6:
                                        this.counterTiming = false;
                                        if (7 - (version - 1) * 3 <= 0)
                                            break;
                                        break;
                                    case 13:
                                        this.target.Y = 0;
                                        this.parent.attacks.Add(new CrackThunder(this.sound, this.parent, this.target.X, this.target.Y, this.union, this.Power, false));
                                        break;
                                    case 16:
                                        this.target.Y = 1;
                                        this.parent.attacks.Add(new CrackThunder(this.sound, this.parent, this.target.X, this.target.Y, this.union, this.Power, false));
                                        break;
                                    case 19:
                                        this.target.Y = 2;
                                        this.parent.attacks.Add(new CrackThunder(this.sound, this.parent, this.target.X, this.target.Y, this.union, this.Power, false));
                                        break;
                                    case 20:
                                        this.roopneutral = 0;
                                        this.DorillChange();
                                        break;
                                }
                                break;
                            case Iku.ATTACK.searchThunder:
                                this.animationpoint = this.AnimeThunder(this.waittime);
                                switch (this.waittime)
                                {
                                    case 1:
                                        this.attackCount = 0;
                                        break;
                                    case 6:
                                        this.counterTiming = false;
                                        break;
                                }
                                if (this.waittime >= 5 && this.waittime % 5 == 0)
                                {
                                    if (this.attackCount > 0)
                                        this.parent.attacks.Add(new CrackThunder(this.sound, this.parent, this.target.X, this.target.Y, this.union, this.Power, false));
                                    if (this.attackCount < Math.Min(version + 2, 5))
                                    {
                                        this.target = this.RandomTarget();
                                        this.parent.attacks.Add(new Dummy(this.sound, this.parent, this.target.X, this.target.Y, this.union, new Point(), 13, true));
                                    }
                                    else
                                    {
                                        this.roopneutral = 0;
                                        this.parent.attacks.Add(new CrackThunder(this.sound, this.parent, this.target.X, this.target.Y, this.union, this.Power, false));
                                        this.DorillChange();
                                    }
                                    ++this.attackCount;
                                    break;
                                }
                                break;
                            case Iku.ATTACK.drillArm:
                                this.animationpoint = this.AnimeThunder(this.waittime);
                                switch (this.waittime)
                                {
                                    case 7:
                                        DrillAttack drillAttack1 = new DrillAttack(this.sound, this.parent, this.position.X + this.UnionRebirth(this.union), this.position.Y, this.union, this.Power, 1, ChipBase.ELEMENT.eleki)
                                        {
                                            invincibility = false,
                                            breaking = true
                                        };
                                        this.parent.attacks.Add(drillAttack1);
                                        this.sound.PlaySE(SoundEffect.drill2);
                                        break;
                                    case 10:
                                        DrillAttack drillAttack2 = new DrillAttack(this.sound, this.parent, this.position.X + this.UnionRebirth(this.union), this.position.Y, this.union, this.Power, 1, ChipBase.ELEMENT.eleki)
                                        {
                                            invincibility = false,
                                            breaking = true
                                        };
                                        this.parent.attacks.Add(drillAttack2);
                                        break;
                                    case 13:
                                        DrillAttack drillAttack3 = new DrillAttack(this.sound, this.parent, this.position.X + this.UnionRebirth(this.union), this.position.Y, this.union, this.Power, 1, ChipBase.ELEMENT.eleki)
                                        {
                                            invincibility = false,
                                            breaking = true
                                        };
                                        this.parent.attacks.Add(drillAttack3);
                                        break;
                                    case 16:
                                        this.waittime = 0;
                                        this.speed = 8;
                                        this.motion = NaviBase.MOTION.move;
                                        break;
                                }
                                break;
                            case Iku.ATTACK.drillBreak:
                                this.animationpoint.X = 7;
                                int speed = 5;
                                if (this.version >= 3)
                                {
                                    speed = 10;
                                    this.speed = 3;
                                }
                                else
                                    this.speed = 6;
                                switch (this.frame)
                                {
                                    case 1:
                                        this.Noslip = true;
                                        this.drilinvi = true;
                                        this.nohit = true;
                                        this.parent.effects.Add(new MoveEnemy(this.sound, this.parent, this.position.X, this.position.Y));
                                        switch (this.position.Y)
                                        {
                                            case 0:
                                                this.drillY[0] = 1;
                                                this.drillY[1] = 2;
                                                break;
                                            case 1:
                                                this.drillY[0] = 2;
                                                this.drillY[1] = 0;
                                                break;
                                            case 2:
                                                this.drillY[0] = 1;
                                                this.drillY[1] = 0;
                                                break;
                                        }
                                        this.sound.PlaySE(SoundEffect.drill1);
                                        this.parent.objects.Add(new MetalDorill(this.sound, this.parent, this.position.X, this.drillY[0], this.Power, this.version <= 3, speed, this.union));
                                        break;
                                    case 6:
                                        this.sound.PlaySE(SoundEffect.drill1);
                                        this.parent.objects.Add(new MetalDorill(this.sound, this.parent, this.position.X, this.drillY[1], this.Power, this.version <= 3, speed, this.union));
                                        break;
                                    case 11:
                                        this.drilinvi = false;
                                        this.nohit = false;
                                        this.sound.PlaySE(SoundEffect.drill1);
                                        this.drillBreak = new MetalDorill(this.sound, this.parent, this.position.X + this.UnionRebirth(this.union), this.position.Y, this.Power, this.version <= 3, speed, this.union);
                                        this.drillBreak.positionDirect.X -= 16 * this.UnionRebirth(this.union);
                                        this.drillBreak.positionDirect.Y -= 8f;
                                        this.parent.objects.Add(drillBreak);
                                        this.speed = 8;
                                        this.effecting = true;
                                        this.counterTiming = false;
                                        break;
                                }
                                break;
                        }
                        break;
                    }
                    break;
                case NaviBase.MOTION.move:
                    if (this.moveflame)
                        ++this.waittime;
                    ++this.roopmove;
                    this.Motion = NaviBase.MOTION.neutral;
                    this.positionre = this.position;
                    if (this.position.X != (this.union == Panel.COLOR.blue ? 5 : 0))
                    {
                        this.positionre.X = this.union == Panel.COLOR.blue ? 5 : 0;
                        if (this.Canmove(this.positionre))
                        {
                            this.parent.effects.Add(new MoveEnemy(this.sound, this.parent, this.position.X, this.position.Y));
                            this.position = this.positionre;
                            this.PositionDirectSet();
                            this.frame = 0;
                            this.roopneutral = 0;
                        }
                        else
                        {
                            this.positionre.Y = 0;
                            if (this.Canmove(this.positionre, this.number))
                            {
                                this.parent.effects.Add(new MoveEnemy(this.sound, this.parent, this.position.X, this.position.Y));
                                this.position = this.positionre;
                                this.PositionDirectSet();
                            }
                            else
                            {
                                this.positionre.Y = 1;
                                if (this.Canmove(this.positionre, this.number))
                                {
                                    this.parent.effects.Add(new MoveEnemy(this.sound, this.parent, this.position.X, this.position.Y));
                                    this.position = this.positionre;
                                    this.PositionDirectSet();
                                }
                                else
                                {
                                    this.positionre.Y = 2;
                                    if (this.Canmove(this.positionre, this.number))
                                    {
                                        this.parent.effects.Add(new MoveEnemy(this.sound, this.parent, this.position.X, this.position.Y));
                                        this.position = this.positionre;
                                        this.PositionDirectSet();
                                    }
                                    else
                                    {
                                        this.Motion = NaviBase.MOTION.neutral;
                                        this.PositionDirectSet();
                                    }
                                }
                            }
                        }
                        this.frame = 0;
                        this.roopneutral = 0;
                        this.roopmove = 0;
                    }
                    if (this.position.Y == 0)
                    {
                        this.UpDown = false;
                        break;
                    }
                    if (this.position.Y == 2)
                    {
                        this.UpDown = true;
                        break;
                    }
                    break;
                case NaviBase.MOTION.knockback:
                    switch (this.waittime)
                    {
                        case 1:
                            if (this.drillBreak != null)
                            {
                                this.drillBreak.flag = false;
                                break;
                            }
                            break;
                        case 2:
                            this.movelong = 24;
                            this.effecting = false;
                            this.animationpoint = new Point(3, 0);
                            this.counterTiming = false;
                            this.effecting = false;
                            this.PositionDirectSet();
                            this.Noslip = false;
                            break;
                        case 3:
                            this.animationpoint = new Point(3, 0);
                            break;
                        case 15:
                            this.animationpoint = new Point(8, 0);
                            this.PositionDirectSet();
                            break;
                        case 21:
                            this.animationpoint = new Point(0, 0);
                            this.waittime = 0;
                            if (!this.atack)
                                this.speed = 8;
                            this.Motion = NaviBase.MOTION.move;
                            break;
                    }
                    if (this.waittime >= 2 && this.waittime <= 6)
                        this.positionDirect.X -= this.UnionRebirth(this.union);
                    ++this.waittime;
                    break;
            }
            if (this.effecting && !this.nohit)
                this.AttackMake(this.Power, 0, 0);
            if (this.motion == NaviBase.MOTION.attack && this.attack == Iku.ATTACK.drillBreak && this.frame >= 11 && this.drillBreak != null)
            {
                if (!this.drillBreak.flag && this.drillBreak.speednow == 0)
                    this.drillBreak.speednow = 3;
                if (this.drillBreak.flag)
                    this.positionDirect.X = this.drillBreak.positionDirect.X - 24 * this.UnionRebirth(this.union);
                else
                    this.positionDirect.X += this.drillBreak.speednow * this.UnionRebirth(this.union);
                this.position.X = this.Calcposition(this.positionDirect, 72, false).X;
                if (this.union == Panel.COLOR.blue && this.position.X <= 0 && !this.drillBreak.flag || this.union == Panel.COLOR.red && this.position.X >= 6 && !this.drillBreak.flag)
                {
                    this.Noslip = false;
                    this.speed = 8;
                    this.motion = NaviBase.MOTION.move;
                    this.effecting = false;
                }
            }
            this.FlameControl();
            this.MoveAftar();
        }

        private void DorillChange()
        {
            if (this.DorillSearch())
            {
                this.parent.effects.Add(new MoveEnemy(this.sound, this.parent, this.position.X, this.position.Y));
                this.position = this.positionre;
                this.PositionDirectSet();
                this.Motion = NaviBase.MOTION.attack;
                this.waittime = 0;
                int index = 2;
                this.speed = Math.Max(3, 4 - version / 2);
                this.attack = (Iku.ATTACK)index;
                this.powerPlus = this.powers[index];
            }
            else
            {
                ++this.cancelCount;
                if (this.cancelCount >= 5)
                {
                    int pX = Eriabash.SteelX(this, this.parent);
                    if (pX != 99)
                    {
                        for (int pY = 0; pY < this.parent.panel.GetLength(1); ++pY)
                            this.parent.attacks.Add(new EriaSteel(this.sound, this.parent, pX, pY, this.union, 10, this.element));
                    }
                    this.cancelCount = 0;
                }
                this.motion = NaviBase.MOTION.move;
                this.waittime = 0;
                if (!this.atack)
                    this.speed = 8;
            }
        }

        private bool DorillSearch()
        {
            if (this.parent.panel[0, 0].state == Panel.PANEL._un)
            {
                this.cancelCount = 0;
                return false;
            }
            bool flag = false;
            int x = 0;
            int y = 0;
            for (int index1 = 0; index1 < this.parent.panel.GetLength(1); ++index1)
            {
                if (this.union == Panel.COLOR.blue)
                {
                    for (int index2 = 0; index2 < this.parent.panel.GetLength(0); ++index2)
                    {
                        if (this.parent.panel[index2, index1].color == this.union)
                        {
                            x = index2;
                            break;
                        }
                    }
                }
                else
                {
                    for (int length = this.parent.panel.GetLength(0); length >= 0; --length)
                    {
                        if (this.parent.panel[length, index1].color == this.union)
                        {
                            x = length;
                            break;
                        }
                    }
                }
                foreach (CharacterBase characterBase in this.parent.AllChara())
                {
                    if (this.union == Panel.COLOR.blue)
                    {
                        if (characterBase.position.Y == index1 && characterBase.position.X >= x - 2 && characterBase.position.X < x)
                        {
                            flag = true;
                            y = index1;
                            break;
                        }
                    }
                    else if (characterBase.position.Y == index1 && characterBase.position.X <= x + 2 && characterBase.position.X > x)
                    {
                        flag = true;
                        y = index1;
                        break;
                    }
                }
            }
            if (flag)
            {
                if (this.Canmove(new Point(x, y), this.number))
                {
                    if (!this.HeviSand)
                        this.positionre = new Point(x, y);
                    else
                        this.positionre = this.position;
                }
                else
                    flag = false;
            }
            return flag;
        }

        public override void NockMotion()
        {
            this.animationpoint = new Point(3, 0);
        }

        public override void Render(IRenderer dg)
        {
            if (!this.drilinvi)
            {
                if (this.alfha < byte.MaxValue)
                    this.color = Color.FromArgb(alfha, this.mastorcolor);
                else
                    this.color = this.mastorcolor;
                this._position = new Vector2((int)this.positionDirect.X + this.Shake.X, (int)this.positionDirect.Y + this.Shake.Y);
                this._rect = new Rectangle(this.animationpoint.X * this.wide, (this.version < 4 ? 0 : 2) * this.height, this.wide, this.height);
                if (this.Hp <= 0)
                {
                    this.rd = this._rect;
                    this.NockMotion();
                    this._rect = new Rectangle(this.animationpoint.X * this.wide + this.Shake.X, (this.version < 4 ? 0 : 2) * this.height, this.wide, this.height + this.Shake.Y);
                    this.Death(this._rect, new Rectangle(this.animationpoint.X * this.wide, this.height, this.wide, this.height), this._position, this.picturename);
                }
                if (this.whitetime == 0)
                {
                    dg.DrawImage(dg, this.picturename, this._rect, false, this._position, this.rebirth, this.color);
                    if (this.motion == NaviBase.MOTION.attack && this.attack == Iku.ATTACK.drillArm && this.waittime >= 7 && this.waittime < 20)
                    {
                        int num = this.waittime % 3;
                        this._position = new Vector2((int)this.positionDirect.X + 44 * this.UnionRebirth(this.union) + this.Shake.X, (int)this.positionDirect.Y + this.Shake.Y);
                        this._rect = new Rectangle(1600 + num * this.Wide, (this.version < 4 ? 0 : 2) * this.height, this.wide, this.height);
                        dg.DrawImage(dg, this.picturename, this._rect, false, this._position, this.rebirth, this.color);
                    }
                }
                else
                {
                    this._rect.Y = this.height;
                    dg.DrawImage(dg, this.picturename, this._rect, false, this._position, this.rebirth, this.color);
                    if (this.motion == NaviBase.MOTION.attack && this.attack == Iku.ATTACK.drillArm && this.waittime >= 7 && this.waittime < 20)
                    {
                        int num = this.waittime % 3;
                        this._position = new Vector2((int)this.positionDirect.X + 44 * this.UnionRebirth(this.union) + this.Shake.X, (int)this.positionDirect.Y + this.Shake.Y);
                        this._rect = new Rectangle(1600 + num * this.Wide, (this.version < 4 ? 0 : 2) * this.height, this.wide, this.height)
                        {
                            Y = this.height
                        };
                        dg.DrawImage(dg, this.picturename, this._rect, false, this._position, this.rebirth, this.color);
                    }
                }
                this.HPposition = new Vector2((int)this.positionDirect.X, (int)this.positionDirect.Y - this.height / 2 - 3);
            }
            else
                this.HPposition.X = 400f;
            this.Nameprint(dg, this.printNumber);
        }

        private Point AnimeNeutral(int waitflame)
        {
            return this.Return(new int[5] { 0, 1, 2, 3, 4 }, new int[5]
            {
        0,
        1,
        2,
        1,
        0
            }, 1, waitflame);
        }

        private Point AnimeFever(int waitflame)
        {
            return this.Return(new int[4] { 5, 6, 7, 100 }, new int[4]
            {
        8,
        9,
        10,
        10
            }, 0, waitflame);
        }

        private Point AnimeThunder(int waitflame)
        {
            return this.Return(new int[7]
            {
        5,
        6,
        7,
        8,
        9,
        10,
        100
            }, new int[7] { 8, 11, 12, 13, 14, 15, 16 }, 0, waitflame);
        }

        private Point AnimeDorill(int waitflame)
        {
            return this.Return(new int[7] { 2, 3, 4, 5, 6, 7, 100 }, new int[7]
            {
        8,
        11,
        12,
        13,
        14,
        15,
        16
            }, 0, waitflame);
        }

        private Point AnimeDorillBreak(int waitflame)
        {
            return this.Return(new int[5] { 18, 19, 20, 21, 100 }, new int[5]
            {
        8,
        4,
        5,
        6,
        7
            }, 0, waitflame);
        }

        private enum ATTACK
        {
            lineThunder,
            searchThunder,
            drillArm,
            drillBreak,
            drillBreakUP,
            ThunderBall,
        }
    }
}

