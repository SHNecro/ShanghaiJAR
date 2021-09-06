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
    internal class HakutakuMan : NaviBase
    {
        private readonly int[] powers = new int[4] { 20, 20, 0, 10 };
        private int nspeed = 2;
        private readonly int moveroop;
        private HakutakuMan.ATTACK attack;
        private int roopneutral;
        private int roopmove;
        private bool attackFlag;
        private bool beast;
        private bool beastOutEnd;
        private readonly int movespeed;
        private int atackroop;
        private readonly bool atack;
        private int attackCombo;
        private bool spinUP;
        private bool spinGo;
        private DammyEnemy dammy;
        private bool godmodeinit;

        public HakutakuMan(IAudioEngine s, SceneBattle p, int pX, int pY, byte n, Panel.COLOR u, byte v)
          : base(s, p, pX, pY, n, u, v)
        {
            for (int index = 0; index < this.dropchips.Length; ++index)
                this.dropchips[index] = new ChipFolder(this.sound);
            this.element = ChipBase.ELEMENT.leaf;
            switch (this.version)
            {
                case 1:
                    this.movespeed = 10;
                    this.nspeed = 3;
                    this.name = ShanghaiEXE.Translate("Enemy.HakutakuManName1");
                    this.power = 80;
                    this.hp = 1100;
                    this.moveroop = 3;
                    break;
                case 2:
                    this.movespeed = 13;
                    this.nspeed = 3;
                    this.name = ShanghaiEXE.Translate("Enemy.HakutakuManName2");
                    this.power = 100;
                    this.hp = 1300;
                    this.moveroop = 3;
                    break;
                case 3:
                    this.movespeed = 16;
                    this.nspeed = 2;
                    this.name = ShanghaiEXE.Translate("Enemy.HakutakuManName3");
                    this.power = 120;
                    this.hp = 1500;
                    this.moveroop = 2;
                    break;
                case 4:
                    this.movespeed = 16;
                    this.nspeed = 2;
                    this.name = ShanghaiEXE.Translate("Enemy.HakutakuManName4");
                    this.power = 200;
                    this.hp = 1600;
                    this.moveroop = 2;
                    this.element = ChipBase.ELEMENT.heat;
                    break;
                default:
                    this.movespeed = 16;
                    this.nspeed = 2;
                    this.name = ShanghaiEXE.Translate("Enemy.HakutakuManName5") + (version - 3).ToString();
                    this.power = 200;
                    this.hp = 1800 + (version - 4) * 500;
                    this.moveroop = 3;
                    this.element = ChipBase.ELEMENT.heat;
                    break;
            }
            this.picturename = "hakutaku";
            this.race = EnemyBase.ENEMY.navi;
            this.Flying = false;
            this.wide = 88;
            this.height = 88;
            this.hpmax = this.hp;
            this.speed = this.nspeed;
            this.hpprint = this.hp;
            this.printhp = true;
            this.effecting = false;
            this.Flying = false;
            this.roopmove = n * 3;
            this.PositionDirectSet();
            switch (this.version)
            {
                case 1:
                    this.dropchips[0].chip = new HakutakuManV1(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new HakutakuManV1(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new HakutakuManV1(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new HakutakuManV1(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new HakutakuManV1(this.sound);
                    this.dropchips[4].codeNo = 0;
                    this.havezenny = 0;
                    break;
                case 2:
                    this.dropchips[0].chip = new HakutakuManV2(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new HakutakuManV2(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new HakutakuManV2(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new HakutakuManV2(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new HakutakuManV2(this.sound);
                    this.dropchips[4].codeNo = 0;
                    this.havezenny = 0;
                    break;
                case 3:
                    this.dropchips[0].chip = new HakutakuManV1(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new HakutakuManV1(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new HakutakuManV2(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new HakutakuManV2(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new HakutakuManV3(this.sound);
                    this.dropchips[4].codeNo = 0;
                    this.havezenny = 800;
                    break;
                default:
                    this.dropchips[0].chip = new HakutakuManV1(this.sound);
                    this.dropchips[0].codeNo = 1;
                    this.dropchips[1].chip = new HakutakuManV2(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new HakutakuManV2(this.sound);
                    this.dropchips[2].codeNo = 1;
                    this.dropchips[3].chip = new HakutakuManV1(this.sound);
                    this.dropchips[3].codeNo = 2;
                    this.dropchips[4].chip = new HakutakuManV3(this.sound);
                    this.dropchips[4].codeNo = 1;
                    this.havezenny = 9000;
                    if (this.version >= 8)
                    {
                        this.dropchips[4].chip = new HakutakuManX(this.sound);
                        this.dropchips[4].codeNo = this.Random.Next(4);
                        this.havezenny = 18000;
                        break;
                    }
                    break;
            }
            this.motion = NaviBase.MOTION.move;
        }

        public override void PositionDirectSet()
        {
            this.positionDirect = new Vector2((float)(position.X * 40.0 + 16.0), (float)(position.Y * 24.0 + 50.0));
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

        public override void Updata()
        {
            if (this.slipping && (this.neutlal || this.knockslip))
                this.Slip(this.height);
            else if (!this.badstatus[3])
            {
                if (!this.beastOutEnd && this.Hp <= this.HpMax / 2 && (!this.parent.blackOut || this.blackOutObject))
                    this.GodMode();
                else if (!this.neutlal || !this.badstatus[7] || this.badstatus[6])
                    this.Moving();
            }
            if (this.union == Panel.COLOR.red && this.parent.turn == this.samontarn + 3)
            {
                this.parent.effects.Add(new MoveEnemy(this.sound, this.parent, this.positionDirect, this.position));
                this.sound.PlaySE(SoundEffect.enterenemy);
                this.flag = false;
            }
            if (this.alfha < byte.MaxValue)
                this.alfha += 15;
            this.BaseUpdata();
        }

        protected override void Moving()
        {
            this.neutlal = this.Motion == NaviBase.MOTION.neutral;
            switch (this.Motion)
            {
                case NaviBase.MOTION.neutral:
                    if (this.moveflame)
                        ++this.waittime;
                    if (this.moveflame)
                    {
                        this.animationpoint = this.AnimeNeutral(this.waittime);
                        if (this.waittime >= (this.beast ? 2 : 8 - Math.Min(6, (int)this.version)))
                        {
                            this.waittime = 0;
                            ++this.roopneutral;
                            if (this.roopneutral >= 1 && this.parent.nowscene != SceneBattle.BATTLESCENE.end)
                            {
                                this.roopneutral = 0;
                                if (this.roopmove > this.moveroop && !this.badstatus[4])
                                {
                                    this.roopmove = 0;
                                    this.roopmove = this.version > 3 ? this.Random.Next(-1, this.moveroop + 1) : 0;
                                    ++this.atackroop;
                                    if (!this.atack)
                                    {
                                        int index = this.Random.Next(4);
                                        this.attack = (HakutakuMan.ATTACK)index;
                                        this.powerPlus = this.powers[index];
                                    }
                                    this.waittime = 0;
                                    this.attackFlag = true;
                                    this.Motion = NaviBase.MOTION.move;
                                    this.counterTiming = true;
                                }
                                else
                                {
                                    this.waittime = 0;
                                    this.Motion = NaviBase.MOTION.move;
                                }
                            }
                        }
                        break;
                    }
                    break;
                case NaviBase.MOTION.attack:
                    if (this.moveflame)
                    {
                        ++this.waittime;
                        if (this.moveflame)
                        {
                            switch (this.attack)
                            {
                                case HakutakuMan.ATTACK.WideCrow:
                                    if (!this.beast)
                                    {
                                        this.animationpoint = this.AnimeWide(this.waittime);
                                        switch (this.waittime)
                                        {
                                            case 6:
                                                this.counterTiming = false;
                                                this.sound.PlaySE(SoundEffect.shotwave);
                                                this.parent.attacks.Add(new SwordAttack(this.sound, this.parent, this.position.X + this.UnionRebirth(this.union), this.position.Y, this.union, this.Power, 3, this.element, false, false));
                                                break;
                                            case 10:
                                                this.motion = NaviBase.MOTION.move;
                                                this.frame = 0;
                                                this.speed = this.nspeed;
                                                this.roopneutral = 0;
                                                this.waittime = 0;
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        if (this.attackCombo == 0)
                                        {
                                            this.animationpoint = this.AnimeWideBO(this.waittime);
                                            switch (this.waittime)
                                            {
                                                case 6:
                                                    this.counterTiming = false;
                                                    this.sound.PlaySE(SoundEffect.shotwave);
                                                    AttackBase attackBase = new SwordAttack(this.sound, this.parent, this.position.X + this.UnionRebirth(this.union), this.position.Y, this.union, this.Power, 3, this.element, false, false);
                                                    attackBase.invincibility = false;
                                                    this.parent.attacks.Add(attackBase);
                                                    break;
                                                case 8:
                                                    ++this.attackCombo;
                                                    this.frame = 0;
                                                    this.speed = this.nspeed;
                                                    this.roopneutral = 0;
                                                    this.waittime = 0;
                                                    break;
                                            }
                                        }
                                        else
                                        {
                                            this.animationpoint = this.AnimeLongBO(this.waittime);
                                            switch (this.waittime)
                                            {
                                                case 6:
                                                    this.counterTiming = false;
                                                    this.sound.PlaySE(SoundEffect.shotwave);
                                                    this.parent.attacks.Add(new LanceAttack(this.sound, this.parent, this.position.X + this.UnionRebirth(this.union), this.position.Y, this.union, this.Power, 3, this.element, true));
                                                    break;
                                                case 10:
                                                    this.attackCombo = 0;
                                                    this.motion = NaviBase.MOTION.move;
                                                    this.frame = 0;
                                                    this.speed = this.nspeed;
                                                    this.roopneutral = 0;
                                                    this.waittime = 0;
                                                    break;
                                            }
                                        }
                                        break;
                                    }
                                    break;
                                case HakutakuMan.ATTACK.LongCrow:
                                    if (!this.beast)
                                    {
                                        this.animationpoint = this.AnimeLong(this.waittime);
                                        switch (this.waittime)
                                        {
                                            case 6:
                                                this.counterTiming = false;
                                                this.sound.PlaySE(SoundEffect.shotwave);
                                                this.parent.attacks.Add(new LanceAttack(this.sound, this.parent, this.position.X + this.UnionRebirth(this.union), this.position.Y, this.union, this.Power, 3, this.element, true));
                                                break;
                                            case 10:
                                                this.motion = NaviBase.MOTION.move;
                                                this.frame = 0;
                                                this.speed = this.nspeed;
                                                this.roopneutral = 0;
                                                this.waittime = 0;
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        if (this.attackCombo == 0)
                                        {
                                            this.animationpoint = this.AnimeLongBO(this.waittime);
                                            switch (this.waittime)
                                            {
                                                case 6:
                                                    this.counterTiming = false;
                                                    this.sound.PlaySE(SoundEffect.shotwave);
                                                    AttackBase attackBase = new LanceAttack(this.sound, this.parent, this.position.X + this.UnionRebirth(this.union), this.position.Y, this.union, this.Power, 3, this.element, true);
                                                    attackBase.invincibility = false;
                                                    this.parent.attacks.Add(attackBase);
                                                    break;
                                                case 8:
                                                    ++this.attackCombo;
                                                    this.frame = 0;
                                                    this.speed = this.nspeed;
                                                    this.roopneutral = 0;
                                                    this.waittime = 0;
                                                    break;
                                            }
                                        }
                                        else
                                        {
                                            this.animationpoint = this.AnimeWideBO(this.waittime);
                                            switch (this.waittime)
                                            {
                                                case 6:
                                                    this.counterTiming = false;
                                                    this.sound.PlaySE(SoundEffect.shotwave);
                                                    this.parent.attacks.Add(new SwordAttack(this.sound, this.parent, this.position.X + this.UnionRebirth(this.union), this.position.Y, this.union, this.Power, 3, this.element, false, false));
                                                    break;
                                                case 10:
                                                    this.attackCombo = 0;
                                                    this.motion = NaviBase.MOTION.move;
                                                    this.frame = 0;
                                                    this.speed = this.nspeed;
                                                    this.roopneutral = 0;
                                                    this.waittime = 0;
                                                    break;
                                            }
                                        }
                                        break;
                                    }
                                    break;
                                case HakutakuMan.ATTACK.Miss:
                                    if (!this.beast)
                                    {
                                        this.animationpoint = this.AnimeMiss(this.waittime);
                                        switch (this.waittime)
                                        {
                                            case 7:
                                            case 9:
                                                this.counterTiming = false;
                                                this.sound.PlaySE(SoundEffect.lance);
                                                break;
                                            case 13:
                                                this.sound.PlaySE(SoundEffect.canon);
                                                this.sound.PlaySE(SoundEffect.damageenemy);
                                                this.whitetime = 4;
                                                this.hp -= 20 * version;
                                                break;
                                            case 20:
                                                this.motion = NaviBase.MOTION.move;
                                                this.frame = 0;
                                                this.roopneutral = 0;
                                                this.waittime = 0;
                                                this.speed = this.nspeed;
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        this.animationpoint = this.AnimeCrossBO(this.waittime);
                                        switch (this.waittime)
                                        {
                                            case 6:
                                                this.sound.PlaySE(SoundEffect.shotwave);
                                                this.parent.attacks.Add(new SwordCloss(this.sound, this.parent, this.position.X + this.UnionRebirth(this.union), this.position.Y, this.union, this.Power, 3, this.element, false));
                                                break;
                                            case 10:
                                                this.motion = NaviBase.MOTION.move;
                                                this.frame = 0;
                                                this.speed = this.nspeed;
                                                this.roopneutral = 0;
                                                this.waittime = 0;
                                                break;
                                        }
                                        break;
                                    }
                                    break;
                                case HakutakuMan.ATTACK.Hadouken:
                                    if (!this.beast)
                                    {
                                        this.animationpoint = this.AnimeHadou(this.waittime);
                                        switch (this.waittime)
                                        {
                                            case 8:
                                                this.counterTiming = false;
                                                int x = this.RandomTarget().X;
                                                int num1 = 1;
                                                int num2 = 8;
                                                AttackBase attackBase = new FireBreath(this.sound, this.parent, this.position.X + num1 * this.UnionRebirth(this.union), this.position.Y, this.union, this.Power, 2, this.element, x);
                                                attackBase.positionDirect.Y += num2;
                                                this.parent.attacks.Add(attackBase);
                                                break;
                                            case 16:
                                                this.motion = NaviBase.MOTION.move;
                                                this.frame = 0;
                                                this.speed = this.nspeed;
                                                this.roopneutral = 0;
                                                this.waittime = 0;
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        switch (this.attackCombo)
                                        {
                                            case 0:
                                                this.animationpoint = this.AnimeSpin1BO(this.waittime);
                                                if (this.waittime == 4)
                                                {
                                                    this.counterTiming = false;
                                                    this.guard = CharacterBase.GUARD.guard;
                                                    switch (this.position.Y)
                                                    {
                                                        case 0:
                                                            this.spinUP = false;
                                                            this.spinGo = true;
                                                            break;
                                                        case 2:
                                                            this.spinUP = true;
                                                            this.spinGo = true;
                                                            break;
                                                        default:
                                                            this.spinGo = false;
                                                            break;
                                                    }
                                                    this.PositionDirectSet();
                                                    this.HitFlagReset();
                                                    this.sound.PlaySE(SoundEffect.knife);
                                                    this.effecting = true;
                                                    ++this.attackCombo;
                                                    this.frame = 0;
                                                    this.speed = this.nspeed;
                                                    this.roopneutral = 0;
                                                    this.waittime = 0;
                                                    this.DammySet();
                                                    break;
                                                }
                                                break;
                                            case 1:
                                                this.animationpoint = this.AnimeSpin2BO(this.waittime % 4);
                                                if (this.spinGo)
                                                {
                                                    if (this.SlideMove(movespeed, 0))
                                                    {
                                                        this.SlideMoveEnd();
                                                        this.PositionDirectSet();
                                                        this.spinGo = false;
                                                        break;
                                                    }
                                                    break;
                                                }
                                                if (this.spinUP)
                                                {
                                                    if (this.SlideMove(movespeed, 2))
                                                    {
                                                        this.SlideMoveEnd();
                                                        Point position = this.position;
                                                        --position.Y;
                                                        if (!this.InAreaCheck(position))
                                                        {
                                                            this.spinUP = !this.spinUP;
                                                            position = this.position;
                                                            position.X += this.UnionRebirth(this.union);
                                                            if (!this.InAreaCheck(position))
                                                            {
                                                                ++this.attackCombo;
                                                                this.frame = 0;
                                                                this.speed = this.nspeed;
                                                                this.roopneutral = 0;
                                                                this.waittime = 0;
                                                            }
                                                            this.PositionDirectSet();
                                                            this.spinGo = true;
                                                        }
                                                    }
                                                }
                                                else if (this.SlideMove(movespeed, 3))
                                                {
                                                    this.SlideMoveEnd();
                                                    Point position = this.position;
                                                    ++position.Y;
                                                    if (!this.InAreaCheck(position))
                                                    {
                                                        this.spinUP = !this.spinUP;
                                                        position = this.position;
                                                        position.X += this.UnionRebirth(this.union);
                                                        if (!this.InAreaCheck(position))
                                                        {
                                                            this.HitFlagReset();
                                                            ++this.attackCombo;
                                                            this.frame = 0;
                                                            this.speed = this.nspeed;
                                                            this.roopneutral = 0;
                                                            this.waittime = 0;
                                                        }
                                                        this.PositionDirectSet();
                                                        this.spinGo = true;
                                                    }
                                                }
                                                break;
                                            case 2:
                                                this.animationpoint = this.AnimeSpin2BO(this.waittime % 3);
                                                if (this.SlideMove(movespeed, 1))
                                                {
                                                    Point position = this.position;
                                                    position.X -= this.UnionRebirth(this.union);
                                                    if (!this.InAreaCheck(position))
                                                    {
                                                        this.guard = CharacterBase.GUARD.none;
                                                        this.effecting = false;
                                                        ++this.attackCombo;
                                                        this.frame = 0;
                                                        this.speed = this.nspeed;
                                                        this.roopneutral = 0;
                                                        this.waittime = 0;
                                                    }
                                                    break;
                                                }
                                                break;
                                            default:
                                                this.animationpoint = this.AnimeSpin3BO(this.waittime);
                                                if (this.waittime == 4)
                                                {
                                                    this.effecting = false;
                                                    this.attackCombo = 0;
                                                    this.motion = NaviBase.MOTION.move;
                                                    this.frame = 0;
                                                    this.speed = this.nspeed;
                                                    this.roopneutral = 0;
                                                    this.waittime = 0;
                                                    break;
                                                }
                                                break;
                                        }
                                        break;
                                    }
                                    break;
                            }
                        }
                        break;
                    }
                    break;
                case NaviBase.MOTION.move:
                    this.animationpoint = this.AnimeMove(this.waittime);
                    if (this.moveflame)
                    {
                        switch (this.waittime)
                        {
                            case 0:
                                if (!this.attackFlag || this.attack == HakutakuMan.ATTACK.Hadouken)
                                {
                                    this.MoveRandom(false, false);
                                }
                                else
                                {
                                    Point point = this.RandomTarget();
                                    if (this.Canmove(new Point(point.X - this.UnionRebirth(this.union), point.Y), this.number, this.union == Panel.COLOR.blue ? Panel.COLOR.red : Panel.COLOR.blue) && !this.HeviSand)
                                        this.positionre = new Point(point.X - this.UnionRebirth(this.union), point.Y);
                                    else
                                        this.MoveRandom(true, false);
                                }
                                if (this.position == this.positionre)
                                {
                                    if (this.attackFlag)
                                    {
                                        this.Motion = NaviBase.MOTION.attack;
                                        if (this.beast)
                                            this.speed *= 2;
                                        this.attackFlag = false;
                                        switch (this.attack)
                                        {
                                            case HakutakuMan.ATTACK.WideCrow:
                                                this.parent.attacks.Add(new Dummy(this.sound, this.parent, this.position.X + this.UnionRebirth(this.union), this.position.Y - 1, this.union, new Point(0, 2), 60, true));
                                                break;
                                            case HakutakuMan.ATTACK.LongCrow:
                                                this.parent.attacks.Add(new Dummy(this.sound, this.parent, this.position.X + this.UnionRebirth(this.union), this.position.Y, this.union, new Point(1, 0), 60, true));
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        this.dammy.flag = false;
                                        this.Motion = NaviBase.MOTION.neutral;
                                    }
                                    this.frame = 0;
                                    this.roopneutral = 0;
                                    ++this.roopmove;
                                    break;
                                }
                                break;
                            case 4:
                                var originalPosition = this.position;
                                this.position = this.positionre;
                                this.positionReserved = null;
                                if (this.attackFlag)
                                {
                                    this.positionReserved = originalPosition;
                                    switch (this.attack)
                                    {
                                        case HakutakuMan.ATTACK.WideCrow:
                                            this.parent.attacks.Add(new Dummy(this.sound, this.parent, this.position.X + this.UnionRebirth(this.union), this.position.Y - 1, this.union, new Point(0, 2), 60, true));
                                            break;
                                        case HakutakuMan.ATTACK.LongCrow:
                                            this.parent.attacks.Add(new Dummy(this.sound, this.parent, this.position.X + this.UnionRebirth(this.union), this.position.Y, this.union, new Point(1, 0), 60, true));
                                            break;
                                        case HakutakuMan.ATTACK.Miss:
                                            if (this.beast)
                                            {
                                                this.parent.attacks.Add(new Dummy(this.sound, this.parent, this.position.X + this.UnionRebirth(this.union), this.position.Y, this.union, new Point(0, 0), 60, true));
                                                this.parent.attacks.Add(new Dummy(this.sound, this.parent, this.position.X + 2 * this.UnionRebirth(this.union), this.position.Y - 1, this.union, new Point(0, 0), 60, true));
                                                this.parent.attacks.Add(new Dummy(this.sound, this.parent, this.position.X, this.position.Y - 1, this.union, new Point(0, 0), 60, true));
                                                this.parent.attacks.Add(new Dummy(this.sound, this.parent, this.position.X + 2 * this.UnionRebirth(this.union), this.position.Y + 1, this.union, new Point(0, 0), 60, true));
                                                this.parent.attacks.Add(new Dummy(this.sound, this.parent, this.position.X, this.position.Y + 1, this.union, new Point(0, 0), 60, true));
                                                break;
                                            }
                                            break;
                                    }
                                }
                                this.PositionDirectSet();
                                break;
                            case 7:
                                if (this.attackFlag)
                                {
                                    this.Motion = NaviBase.MOTION.attack;
                                    this.attackFlag = false;
                                    this.speed *= 2;
                                }
                                else
                                {
                                    this.dammy.flag = false;
                                    this.Motion = NaviBase.MOTION.neutral;
                                }
                                this.frame = 0;
                                this.roopneutral = 0;
                                ++this.roopmove;
                                break;
                        }
                        ++this.waittime;
                        break;
                    }
                    break;
                case NaviBase.MOTION.knockback:
                    switch (this.waittime)
                    {
                        case 2:
                            this.NockMotion();
                            this.attackCombo = 0;
                            this.speed = this.nspeed;
                            this.guard = CharacterBase.GUARD.none;
                            this.counterTiming = false;
                            this.effecting = false;
                            this.attackFlag = false;
                            this.PositionDirectSet();
                            break;
                        case 3:
                            this.NockMotion();
                            break;
                        case 15:
                            this.animationpoint = new Point(5, 0);
                            this.PositionDirectSet();
                            break;
                        case 21:
                            this.animationpoint = new Point(0, 0);
                            this.waittime = 0;
                            this.dammy.flag = false;
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
            this.FlameControl();
            this.MoveAftar();
        }

        public override void NockMotion()
        {
            this.animationpoint = new Point(1, 0);
        }

        public override void Render(IRenderer dg)
        {
            if (this.alfha < byte.MaxValue)
                this.color = Color.FromArgb(alfha, this.mastorcolor);
            else
                this.color = this.mastorcolor;
            this._position = new Vector2((int)this.positionDirect.X + this.Shake.X, (int)this.positionDirect.Y + this.Shake.Y);
            this._rect = new Rectangle(this.animationpoint.X * this.wide, (this.version < 4 ? 0 : 2) * this.height + (this.beast ? this.height * 3 : 0), this.wide, this.height);
            if (this.Hp <= 0)
            {
                this.rd = this._rect;
                this.NockMotion();
                this._rect = new Rectangle(this.animationpoint.X * this.wide, (this.version < 4 ? 0 : 2) * this.height + (this.beast ? this.height * 3 : 0), this.wide, this.height);
                this.Death(this._rect, new Rectangle(this.animationpoint.X * this.wide, this.height + (this.beast ? this.height * 3 : 0), this.wide, this.height), this._position, this.picturename);
            }
            if (this.whitetime == 0)
            {
                dg.DrawImage(dg, this.picturename, this._rect, false, this._position, this.rebirth, this.color);
            }
            else
            {
                this._rect.Y = this.height + (this.beast ? this.height * 3 : 0);
                dg.DrawImage(dg, this.picturename, this._rect, false, this._position, this.rebirth, this.color);
            }
            this.HPposition = new Vector2((int)this.positionDirect.X + 8, (int)this.positionDirect.Y + this.height / 2);
            this.Nameprint(dg, this.printNumber);
        }

        private void GodMode()
        {
            if (!this.godmodeinit)
            {
                this.nohit = true;
                this.godmodeinit = true;
            }
            if (!this.BlackOut(
                this,
                this.parent,
                ShanghaiEXE.Translate("Enemy.HakutakuManSpecial"),
                ""))
                return;
            switch (this.waittime)
            {
                case 1:
                    this.motion = NaviBase.MOTION.neutral;
                    this.nohit = true;
                    break;
                case 30:
                    this.animationpoint.X = 4;
                    this.sound.PlaySE(SoundEffect.bombbig);
                    this.sound.PlaySE(SoundEffect.dragonVoice);
                    this.ShakeStart(4, 60);
                    break;
                case 90:
                    this.parent.backscreen = 100;
                    this.beast = true;
                    if (this.nspeed > 2)
                        --this.nspeed;
                    this.speed = this.nspeed;
                    this.animationpoint.X = 0;
                    this.nohit = false;
                    this.sound.PlaySE(SoundEffect.docking);
                    this.whitetime = 4;
                    break;
            }
            if (this.waittime > 120)
            {
                if (this.BlackOutEnd(this, this.parent))
                {
                    this.beastOutEnd = true;
                    this.waittime = 0;
                    return;
                }
            }
            else if (this.waittime > 30 && this.waittime < 90)
            {
                if (this.parent.backscreen < byte.MaxValue)
                {
                    this.parent.backscreen += 10;
                    if (this.parent.backscreen > byte.MaxValue)
                        this.parent.backscreen = byte.MaxValue;
                }
                this.animationpoint = this.AnimeBeastOut(this.waittime % 4);
            }
            ++this.waittime;
        }

        public override void RenderUP(IRenderer dg)
        {
            this.BlackOutRender(dg, this.union);
        }

        private Point AnimeNeutral(int waitflame)
        {
            return this.Return(new int[1], new int[1], 1, waitflame);
        }

        private Point AnimeMove(int waitflame)
        {
            return this.Return(new int[7] { 0, 1, 2, 3, 5, 6, 7 }, new int[7]
            {
        0,
        2,
        3,
        4,
        3,
        2,
        0
            }, 1, waitflame);
        }

        private Point AnimeWide(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[4]
            {
        1,
        4,
        1,
        100
            }, new int[4] { 0, 5, 6, 7 }, 0, waittime);
        }

        private Point AnimeLong(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[4]
            {
        1,
        4,
        1,
        100
            }, new int[4] { 0, 8, 9, 10 }, 0, waittime);
        }

        private Point AnimeHadou(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[7]
            {
        1,
        1,
        1,
        2,
        1,
        1,
        100
            }, new int[7] { 0, 11, 12, 13, 14, 15, 16 }, 0, waittime);
        }

        private Point AnimeMiss(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[9]
            {
        1,
        4,
        1,
        1,
        1,
        1,
        1,
        1,
        100
            }, new int[9] { 0, 17, 18, 19, 20, 19, 20, 21, 22 }, 0, waittime);
        }

        private Point AnimeBeastOut(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[4]
            {
        1,
        1,
        1,
        1
            }, new int[4] { 23, 24, 25, 26 }, 0, waittime);
        }

        private Point AnimeWideBO(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[4]
            {
        4,
        1,
        1,
        100
            }, new int[4] { 5, 6, 7, 8 }, 0, waittime);
        }

        private Point AnimeLongBO(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[4]
            {
        4,
        1,
        1,
        100
            }, new int[4] { 9, 10, 11, 12 }, 0, waittime);
        }

        private Point AnimeCrossBO(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[5]
            {
        2,
        1,
        1,
        1,
        100
            }, new int[5] { 17, 13, 14, 15, 16 }, 0, waittime);
        }

        private Point AnimeSpin1BO(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[4]
            {
        2,
        1,
        1,
        100
            }, new int[4] { 17, 18, 19, 20 }, 0, waittime);
        }

        private Point AnimeSpin2BO(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[3]
            {
        1,
        1,
        100
            }, new int[3] { 20, 21, 22 }, 0, waittime);
        }

        private Point AnimeSpin3BO(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[4]
            {
        1,
        1,
        1,
        100
            }, new int[4] { 20, 19, 18, 17 }, 0, waittime);
        }

        private enum ATTACK
        {
            WideCrow,
            LongCrow,
            Miss,
            Hadouken,
        }
    }
}

