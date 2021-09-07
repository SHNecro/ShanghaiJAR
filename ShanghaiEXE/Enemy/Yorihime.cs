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
using System.Collections.Generic;
using System.Drawing;

namespace NSEnemy
{
    internal class Yorihime : NaviBase
    {
        private readonly int[] powers = new int[9]
        {
      0,
      0,
      0,
      0,
      0,
      0,
      0,
      60,
      0
        };
        private readonly int nspeed = 8;
        private ChipBase.ELEMENT swordElement = ChipBase.ELEMENT.normal;
        private Yorihime.COMBO combo;
        private Yorihime.ATTACK[] attacks;
        private int atackNo;
        private readonly int shieldwait;
        private bool godmode;
        private readonly int moveroop;
        private Yorihime.ATTACK attack;
        private int roopneutral;
        private int roopmove;
        private readonly bool UpDown;
        private int movetime;
        private SwordShield swordshield;
        private int attackspeed;
        private int chargeEffect;
        private int chargeanime;
        private readonly int gurd;
        private bool godmodeinit;

        public Yorihime(IAudioEngine s, SceneBattle p, int pX, int pY, byte n, Panel.COLOR u, byte v)
          : base(s, p, pX, pY, n, u, v)
        {
            for (int index = 0; index < this.dropchips.Length; ++index)
                this.dropchips[index] = new ChipFolder(this.sound);
            this.element = ChipBase.ELEMENT.normal;
            switch (this.version)
            {
                case 1:
                    this.name = ShanghaiEXE.Translate("Enemy.YorihimeName1");
                    this.power = 30;
                    this.hp = 1500;
                    this.moveroop = 4;
                    this.shieldwait = 50;
                    this.attackspeed = 3;
                    this.nspeed = 3;
                    break;
                case 2:
                    this.name = ShanghaiEXE.Translate("Enemy.YorihimeName2");
                    this.power = 50;
                    this.hp = 1600;
                    this.moveroop = 3;
                    this.shieldwait = 40;
                    this.attackspeed = 3;
                    this.nspeed = 2;
                    break;
                case 3:
                    this.name = ShanghaiEXE.Translate("Enemy.YorihimeName3");
                    this.power = 70;
                    this.hp = 1800;
                    this.moveroop = 2;
                    this.shieldwait = 30;
                    this.attackspeed = 2;
                    this.nspeed = 2;
                    break;
                case 4:
                    this.name = ShanghaiEXE.Translate("Enemy.YorihimeName4");
                    this.power = 100;
                    this.hp = 2400;
                    this.moveroop = 3;
                    this.shieldwait = 20;
                    this.attackspeed = 2;
                    this.nspeed = 2;
                    break;
                default:
                    int num = version - 3;
                    this.name = ShanghaiEXE.Translate("Enemy.YorihimeName5") + num.ToString();
                    this.power = 120;
                    this.hp = 2400 + (version - 4) * 100;
                    this.moveroop = 4;
                    this.shieldwait = 20;
                    this.attackspeed = 2;
                    this.nspeed = 2;
                    {
                        num = version - 3;
                        break;
                    }
            }
            this.picturename = "yorihime";
            this.race = EnemyBase.ENEMY.navi;
            this.Flying = false;
            this.wide = 96;
            this.height = 96;
            this.hpmax = this.hp;
            this.speed = this.nspeed;
            this.hpprint = this.hp;
            this.printhp = true;
            this.effecting = false;
            this.roopmove = 0;
            this.PositionDirectSet();
            switch (this.version)
            {
                case 1:
                    this.dropchips[0].chip = new YorihimeV1(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new YorihimeV1(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new YorihimeV1(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new YorihimeV1(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new YorihimeV1(this.sound);
                    this.dropchips[4].codeNo = 0;
                    this.havezenny = 0;
                    break;
                case 2:
                    this.dropchips[0].chip = new YorihimeV2(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new YorihimeV2(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new YorihimeV2(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new YorihimeV2(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new YorihimeV2(this.sound);
                    this.dropchips[4].codeNo = 0;
                    this.havezenny = 0;
                    break;
                case 3:
                    this.dropchips[0].chip = new YorihimeV1(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new YorihimeV1(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new YorihimeV2(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new YorihimeV2(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new YorihimeV3(this.sound);
                    this.dropchips[4].codeNo = 0;
                    this.havezenny = 2500;
                    break;
                default:
                    this.dropchips[0].chip = new YorihimeV1(this.sound);
                    this.dropchips[0].codeNo = 1;
                    this.dropchips[1].chip = new YorihimeV2(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new YorihimeV2(this.sound);
                    this.dropchips[2].codeNo = 1;
                    this.dropchips[3].chip = new YorihimeV1(this.sound);
                    this.dropchips[3].codeNo = 2;
                    this.dropchips[4].chip = new YorihimeV3(this.sound);
                    this.dropchips[4].codeNo = 1;
                    this.havezenny = 9000;
                    if (this.version >= 8)
                    {
                        this.dropchips[4].chip = new YorihimeX(this.sound);
                        this.dropchips[4].codeNo = this.Random.Next(4);
                        this.havezenny = 18000;
                        break;
                    }
                    break;
            }
            this.speed = 1;
            this.Motion = NaviBase.MOTION.move;
        }

        public override void PositionDirectSet()
        {
            this.positionDirect = new Vector2((float)(position.X * 40.0 - 6.0), (float)(position.Y * 24.0 + 40.0));
        }

        public override void Updata()
        {
            if (this.slipping && (this.neutlal || this.knockslip))
                this.Slip(this.height);
            else if (!this.badstatus[3])
            {
                if (!this.godmode && this.Hp <= this.HpMax / 2)
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
            if (!this.godmode && this.Hp <= this.HpMax / 2)
            {
                this.GodMode();
            }
            else
            {
                if (this.swordshield == null)
                {
                    this.swordshield = new SwordShield(this.sound, this.parent, this.position.X + this.UnionRebirth(this.union), this.position.Y, this.shieldwait, this.union)
                    {
                        hitPower = 20 * version,
                        wave = this.version >= 3
                    };
                    this.parent.objects.Add(swordshield);
                    int pX = CharacterBase.SteelX(this, this.parent);
                    if (pX != 99)
                    {
                        for (int pY = 0; pY < this.parent.panel.GetLength(1); ++pY)
                        {
                            this.sound.PlaySE(SoundEffect.eriasteal2);
                            this.parent.effects.Add(new AfterSteal(this.sound, this.parent, pX, pY));
                            this.parent.panel[pX, pY].inviolability = true;
                        }
                    }
                }
                if (this.godmode)
                {
                    this.attackspeed = 2;
                    this.swordshield.flag = false;
                }
                else if (this.motion == NaviBase.MOTION.attack)
                {
                    this.swordshield.position = new Point(50, this.position.Y);
                }
                else
                {
                    this.swordshield.position = new Point(this.position.X + this.UnionRebirth(this.union), this.position.Y);
                    this.swordshield.PotisionSet();
                }
                this.neutlal = this.Motion == NaviBase.MOTION.neutral;
                switch (this.Motion)
                {
                    case NaviBase.MOTION.neutral:
                        if (this.moveflame)
                        {
                            ++this.waittime;
                            if (this.waittime >= 30 - Math.Min((int)this.version, 7) * 4 || this.godmode)
                            {
                                this.waittime = 0;
                                ++this.roopneutral;
                                if (this.roopneutral >= 1 && this.parent.nowscene != SceneBattle.BATTLESCENE.end)
                                {
                                    if (this.roopmove > this.moveroop && !this.badstatus[4])
                                    {
                                        this.roopneutral = 0;
                                        this.roopmove = 0;
                                        this.speed = this.attackspeed;
                                        int index = !this.godmode ? this.Random.Next(1, 4) : this.Random.Next(4, 8);
                                        this.combo = (Yorihime.COMBO)index;
                                        this.atackNo = 0;
                                        this.ComboMaker();
                                        this.attack = this.attacks[0];
                                        this.powerPlus = this.powers[index];
                                        this.waittime = 0;
                                        this.Motion = NaviBase.MOTION.attack;
                                        this.counterTiming = true;
                                    }
                                    else
                                    {
                                        this.roopneutral = 0;
                                        this.waittime = 0;
                                        this.speed = 1;
                                        this.Motion = NaviBase.MOTION.move;
                                    }
                                }
                            }
                        }
                        ++this.movetime;
                        break;
                    case NaviBase.MOTION.attack:
                        if (this.chargeEffect > 0)
                            ++this.chargeanime;
                        if (this.moveflame)
                            ++this.waittime;
                        if (this.moveflame)
                        {
                            switch (this.attack)
                            {
                                case Yorihime.ATTACK.SonicBoom:
                                    this.animationpoint = this.AnimeSlash1(this.waittime);
                                    switch (this.waittime)
                                    {
                                        case 6:
                                            this.counterTiming = false;
                                            this.sound.PlaySE(SoundEffect.sword);
                                            AttackBase attackBase1 = new SonicBoomMini(this.sound, this.parent, this.position.X + this.UnionRebirth(this.union), this.position.Y, this.union, this.Power, 4 + 2 * (version - 1), this.swordElement, false);
                                            if (this.atackNo < this.attacks.Length - 1)
                                                attackBase1.invincibility = false;
                                            this.parent.attacks.Add(attackBase1);
                                            break;
                                        case 10:
                                            this.NextCombo();
                                            break;
                                        case 20:
                                            this.ComboEnd();
                                            break;
                                    }
                                    break;
                                case Yorihime.ATTACK.FighterSword:
                                    this.animationpoint = this.AnimeSlash2(this.waittime);
                                    switch (this.waittime)
                                    {
                                        case 6:
                                            this.counterTiming = false;
                                            this.sound.PlaySE(SoundEffect.sword);
                                            AttackBase attackBase2 = new FighterSword(this.sound, this.parent, this.position.X + this.UnionRebirth(this.union), this.position.Y, this.union, this.Power, 4, this.swordElement);
                                            if (this.atackNo < this.attacks.Length - 1)
                                                attackBase2.invincibility = false;
                                            this.parent.attacks.Add(attackBase2);
                                            break;
                                        case 10:
                                            this.NextCombo();
                                            break;
                                        case 20:
                                            this.ComboEnd();
                                            break;
                                    }
                                    break;
                                case Yorihime.ATTACK.WindSword:
                                    this.animationpoint = this.AnimeSlash3(this.waittime);
                                    switch (this.waittime)
                                    {
                                        case 6:
                                            this.counterTiming = false;
                                            this.sound.PlaySE(SoundEffect.shoot);
                                            for (int pY = 0; pY < 3; ++pY)
                                                this.parent.attacks.Add(new Wind(this.sound, this.parent, this.position.X + this.UnionRebirth(this.union), pY, this.union, true));
                                            break;
                                        case 10:
                                            this.NextCombo();
                                            break;
                                        case 20:
                                            this.ComboEnd();
                                            break;
                                    }
                                    break;
                                case Yorihime.ATTACK.WideSword:
                                    this.animationpoint = this.AnimeSlash1(this.waittime);
                                    switch (this.waittime)
                                    {
                                        case 6:
                                            this.counterTiming = false;
                                            this.sound.PlaySE(SoundEffect.sword);
                                            AttackBase attackBase3 = new SwordAttack(this.sound, this.parent, this.position.X + this.UnionRebirth(this.union), this.position.Y, this.union, this.Power, 4, this.swordElement, false, false);
                                            if (this.atackNo < this.attacks.Length - 1)
                                                attackBase3.invincibility = false;
                                            this.parent.attacks.Add(attackBase3);
                                            break;
                                        case 10:
                                            this.NextCombo();
                                            break;
                                        case 20:
                                            this.ComboEnd();
                                            break;
                                    }
                                    break;
                                case Yorihime.ATTACK.CrossSword:
                                    this.animationpoint = this.AnimeSlash3(this.waittime);
                                    switch (this.waittime)
                                    {
                                        case 6:
                                            this.counterTiming = false;
                                            this.sound.PlaySE(SoundEffect.sword);
                                            AttackBase attackBase4 = new SwordCloss(this.sound, this.parent, this.position.X + this.UnionRebirth(this.union), this.position.Y, this.union, this.Power, 4, this.swordElement, false);
                                            if (this.atackNo < this.attacks.Length - 1)
                                                attackBase4.invincibility = false;
                                            this.parent.attacks.Add(attackBase4);
                                            break;
                                        case 10:
                                            this.NextCombo();
                                            break;
                                        case 20:
                                            this.ComboEnd();
                                            break;
                                    }
                                    break;
                                case Yorihime.ATTACK.MegaSonic:
                                    this.animationpoint = this.AnimeSlash5(this.waittime);
                                    switch (this.waittime)
                                    {
                                        case 1:
                                            this.counterTiming = false;
                                            this.chargeanime = 0;
                                            this.sound.PlaySE(SoundEffect.charge);
                                            this.speed = this.attackspeed;
                                            this.chargeEffect = 1;
                                            break;
                                        case 8:
                                            this.counterTiming = true;
                                            this.sound.PlaySE(SoundEffect.chargemax);
                                            this.chargeEffect = 2;
                                            break;
                                        case 15:
                                            this.speed = this.nspeed;
                                            this.chargeEffect = 0;
                                            break;
                                        case 16:
                                            this.counterTiming = false;
                                            this.sound.PlaySE(SoundEffect.sword);
                                            SonicBoom sonicBoom1 = new SonicBoom(this.sound, this.parent, this.position.X + this.UnionRebirth(this.union), this.position.Y, this.union, this.Power / 3, 4 + 2 * (version - 1), ChipBase.ELEMENT.leaf, false)
                                            {
                                                invincibility = false
                                            };
                                            this.parent.attacks.Add(sonicBoom1);
                                            break;
                                        case 18:
                                            this.sound.PlaySE(SoundEffect.sword);
                                            SonicBoom sonicBoom2 = new SonicBoom(this.sound, this.parent, this.position.X + this.UnionRebirth(this.union), this.position.Y, this.union, this.Power / 3, 4 + 2 * (version - 1), ChipBase.ELEMENT.eleki, false)
                                            {
                                                invincibility = false
                                            };
                                            this.parent.attacks.Add(sonicBoom2);
                                            break;
                                        case 20:
                                            this.sound.PlaySE(SoundEffect.sword);
                                            this.parent.attacks.Add(new SonicBoom(this.sound, this.parent, this.position.X + this.UnionRebirth(this.union), this.position.Y, this.union, this.Power / 3, 4 + 2 * (version - 1), ChipBase.ELEMENT.heat, false));
                                            this.NextCombo();
                                            break;
                                        case 30:
                                            this.ComboEnd();
                                            break;
                                    }
                                    break;
                                case Yorihime.ATTACK.GrandBreaker:
                                    this.animationpoint = this.AnimeSlash4(this.waittime);
                                    switch (this.waittime)
                                    {
                                        case 1:
                                            this.counterTiming = false;
                                            this.chargeanime = 0;
                                            this.sound.PlaySE(SoundEffect.charge);
                                            this.speed = this.attackspeed;
                                            this.chargeEffect = 1;
                                            break;
                                        case 20:
                                            this.counterTiming = true;
                                            this.sound.PlaySE(SoundEffect.chargemax);
                                            this.chargeEffect = 2;
                                            break;
                                        case 35:
                                            this.speed = this.nspeed;
                                            this.chargeEffect = 0;
                                            break;
                                        case 38:
                                            this.effecting = true;
                                            this.Shadow();
                                            int x = this.union == Panel.COLOR.blue ? 2 : 3;
                                            this.positionReserved = this.position;
                                            if (!this.HeviSand)
                                                this.position = new Point(x, 1);
                                            this.PositionDirectSet();
                                            break;
                                        case 41:
                                            this.counterTiming = false;
                                            this.speed = this.attackspeed;
                                            this.ShakeStart(2, 16);
                                            this.sound.PlaySE(SoundEffect.bombmiddle);
                                            for (int index = 0; index < 2; ++index)
                                            {
                                                for (int pY = 0; pY < 3; ++pY)
                                                {
                                                    BombAttack bombAttack = new BombAttack(this.sound, this.parent, this.position.X + (1 + index) * this.UnionRebirth(this.union), pY, this.union, this.Power, 1, 1, new Point(), ChipBase.ELEMENT.normal)
                                                    {
                                                        breaking = true
                                                    };
                                                    this.parent.attacks.Add(bombAttack);
                                                    this.parent.effects.Add(new Shock(this.sound, this.parent, bombAttack.position.X, bombAttack.position.Y, 2, Panel.COLOR.blue));
                                                }
                                            }
                                            break;
                                        case 48:
                                            this.NextCombo();
                                            break;
                                        case 58:
                                            this.ComboEnd();
                                            break;
                                    }
                                    break;
                                case Yorihime.ATTACK.DreamSword:
                                    this.animationpoint = this.AnimeSlash3(this.waittime);
                                    switch (this.waittime)
                                    {
                                        case 6:
                                            this.counterTiming = false;
                                            this.sound.PlaySE(SoundEffect.sword);
                                            AttackBase attackBase5 = new Halberd(this.sound, this.parent, this.position.X + this.UnionRebirth(this.union), this.position.Y, this.union, this.Power, 4, this.swordElement, false);
                                            if (this.atackNo < this.attacks.Length - 1)
                                                attackBase5.invincibility = false;
                                            this.parent.attacks.Add(attackBase5);
                                            break;
                                        case 10:
                                            this.NextCombo();
                                            break;
                                        case 20:
                                            this.ComboEnd();
                                            break;
                                    }
                                    break;
                                case Yorihime.ATTACK.SonicBoom2:
                                    this.animationpoint = this.AnimeSlash2(this.waittime);
                                    switch (this.waittime)
                                    {
                                        case 6:
                                            this.counterTiming = false;
                                            this.sound.PlaySE(SoundEffect.sword);
                                            AttackBase attackBase6 = new SonicBoomMini(this.sound, this.parent, this.position.X + this.UnionRebirth(this.union), this.position.Y, this.union, this.Power, 4 + 2 * (version - 1), this.swordElement, false);
                                            if (this.atackNo < this.attacks.Length - 1)
                                                attackBase6.invincibility = false;
                                            this.parent.attacks.Add(attackBase6);
                                            break;
                                        case 10:
                                            this.NextCombo();
                                            break;
                                        case 20:
                                            this.ComboEnd();
                                            break;
                                    }
                                    break;
                                case Yorihime.ATTACK.MoveFront:
                                    this.animationpoint = this.AnimeMove(this.waittime);
                                    if (this.moveflame)
                                    {
                                        switch (this.waittime)
                                        {
                                            case 1:
                                                this.counterTiming = false;
                                                this.speed = this.attackspeed;
                                                this.MoveRandom(true, true);
                                                break;
                                            case 2:
                                                this.Shadow();
                                                this.positionReserved = this.position;
                                                this.position = this.positionre;
                                                this.PositionDirectSet();
                                                break;
                                            case 6:
                                                this.NextCombo();
                                                break;
                                            case 8:
                                                this.ComboEnd();
                                                break;
                                        }
                                        break;
                                    }
                                    break;
                                case Yorihime.ATTACK.MoveTouch:
                                    this.animationpoint = this.AnimeMove(this.waittime);
                                    if (this.moveflame)
                                    {
                                        switch (this.waittime)
                                        {
                                            case 1:
                                                this.counterTiming = false;
                                                this.effecting = true;
                                                this.speed = this.attackspeed;
                                                if (!this.HeviSand)
                                                {
                                                    this.positionre = new Point(this.parent.player.position.X + 1, this.parent.player.position.Y);
                                                    break;
                                                }
                                                break;
                                            case 2:
                                                this.Shadow();
                                                this.positionReserved = this.position;
                                                this.position = this.positionre;
                                                this.PositionDirectSet();
                                                break;
                                            case 6:
                                                this.NextCombo();
                                                break;
                                            case 8:
                                                this.ComboEnd();
                                                break;
                                        }
                                        break;
                                    }
                                    break;
                                case Yorihime.ATTACK.MoveSide:
                                    this.animationpoint = this.AnimeMove(this.waittime);
                                    if (this.moveflame)
                                    {
                                        switch (this.waittime)
                                        {
                                            case 1:
                                                this.counterTiming = false;
                                                this.speed = this.attackspeed;
                                                this.MoveRandom(true, true, this.union, true);
                                                break;
                                            case 2:
                                                this.Shadow();
                                                this.positionReserved = this.position;
                                                this.position = this.positionre;
                                                this.PositionDirectSet();
                                                break;
                                            case 6:
                                                this.NextCombo();
                                                break;
                                            case 8:
                                                this.ComboEnd();
                                                break;
                                        }
                                        break;
                                    }
                                    break;
                            }
                            break;
                        }
                        break;
                    case NaviBase.MOTION.move:
                        this.positionReserved = null;
                        this.animationpoint = this.AnimeMove(this.waittime);
                        if (this.moveflame)
                        {
                            switch (this.waittime)
                            {
                                case 0:
                                    this.MoveSet();
                                    if (this.position == this.positionre)
                                    {
                                        this.Motion = NaviBase.MOTION.neutral;
                                        this.frame = 0;
                                        this.roopneutral = 0;
                                        break;
                                    }
                                    break;
                                case 2:
                                    this.Shadow();
                                    this.position = this.positionre;
                                    this.PositionDirectSet();
                                    break;
                                case 6:
                                    this.Motion = NaviBase.MOTION.neutral;
                                    this.frame = 0;
                                    this.roopneutral = 0;
                                    this.speed = this.nspeed;
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
                                this.chargeEffect = 0;
                                this.chargeanime = 0;
                                this.effecting = false;
                                this.HitFlagReset();
                                this.animationpoint = new Point(3, 0);
                                this.counterTiming = false;
                                this.effecting = false;
                                this.PositionDirectSet();
                                break;
                            case 3:
                                this.animationpoint = new Point(3, 0);
                                break;
                            case 15:
                                this.animationpoint = new Point(11, 0);
                                this.PositionDirectSet();
                                break;
                            case 21:
                                this.animationpoint = new Point(0, 0);
                                this.waittime = 0;
                                this.speed = this.nspeed;
                                if (!this.godmode)
                                {
                                    this.roopneutral = 0;
                                    this.roopmove = 0;
                                    this.speed = 2;
                                    this.atackNo = 0;
                                    this.combo = Yorihime.COMBO.CounterSword;
                                    this.ComboMaker();
                                    this.attack = this.attacks[0];
                                    this.powerPlus = this.powers[(int)this.attack];
                                    this.waittime = 0;
                                    this.Motion = NaviBase.MOTION.attack;
                                    this.counterTiming = true;
                                    break;
                                }
                                this.animationpoint = new Point(0, 0);
                                this.waittime = 0;
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
                ShanghaiEXE.Translate("Enemy.YorihimeSpecial"),
                ""))
                return;
            switch (this.waittime)
            {
                case 1:
                    this.motion = NaviBase.MOTION.neutral;
                    break;
                case 30:
                    this.animationpoint.X = 4;
                    this.sound.PlaySE(SoundEffect.bombbig);
                    this.ShakeStart(4, 60);
                    this.parent.effects.Add(new Shock(this.sound, this.parent, this.position.X - 1, this.position.Y, 4, Panel.COLOR.blue));
                    Shock shock = new Shock(this.sound, this.parent, this.position.X + 1, this.position.Y, 4, Panel.COLOR.blue);
                    shock.rebirth = !shock.rebirth;
                    this.parent.effects.Add(shock);
                    break;
                case 90:
                    this.animationpoint.X = 0;
                    this.sound.PlaySE(SoundEffect.docking);
                    this.whitetime = 4;
                    this.ElementSet();
                    this.nohit = false;
                    break;
            }
            if (this.waittime > 120 && this.BlackOutEnd(this, this.parent))
            {
                this.godmode = true;
                int pX = CharacterBase.SteelX(this, this.parent);
                if (pX != 99)
                {
                    for (int pY = 0; pY < this.parent.panel.GetLength(1); ++pY)
                    {
                        this.sound.PlaySE(SoundEffect.eriasteal2);
                        this.parent.effects.Add(new AfterSteal(this.sound, this.parent, pX, pY));
                        this.parent.panel[pX, pY].inviolability = true;
                    }
                }
            }
            ++this.waittime;
        }

        private void ElementSet()
        {
            switch (this.parent.player.Element)
            {
                case ChipBase.ELEMENT.normal:
                    this.swordElement = ChipBase.ELEMENT.normal;
                    break;
                case ChipBase.ELEMENT.heat:
                    this.swordElement = ChipBase.ELEMENT.aqua;
                    break;
                case ChipBase.ELEMENT.aqua:
                    this.swordElement = ChipBase.ELEMENT.poison;
                    break;
                case ChipBase.ELEMENT.eleki:
                    this.swordElement = ChipBase.ELEMENT.earth;
                    break;
                case ChipBase.ELEMENT.leaf:
                    this.swordElement = ChipBase.ELEMENT.heat;
                    break;
                case ChipBase.ELEMENT.poison:
                    this.swordElement = ChipBase.ELEMENT.eleki;
                    break;
                case ChipBase.ELEMENT.earth:
                    this.swordElement = ChipBase.ELEMENT.leaf;
                    break;
            }
        }

        private void ComboMaker()
        {
            List<Yorihime.ATTACK> attackList = new List<Yorihime.ATTACK>();
            switch (this.combo)
            {
                case Yorihime.COMBO.CounterSword:
                    attackList.Add(Yorihime.ATTACK.SonicBoom);
                    attackList.Add(Yorihime.ATTACK.MoveTouch);
                    attackList.Add(Yorihime.ATTACK.FighterSword);
                    break;
                case Yorihime.COMBO.FighterSword:
                    attackList.Add(Yorihime.ATTACK.MoveFront);
                    attackList.Add(Yorihime.ATTACK.FighterSword);
                    attackList.Add(Yorihime.ATTACK.MoveFront);
                    attackList.Add(Yorihime.ATTACK.FighterSword);
                    attackList.Add(Yorihime.ATTACK.MoveFront);
                    attackList.Add(Yorihime.ATTACK.FighterSword);
                    break;
                case Yorihime.COMBO.DubleSword:
                    attackList.Add(Yorihime.ATTACK.WindSword);
                    attackList.Add(Yorihime.ATTACK.MoveTouch);
                    attackList.Add(Yorihime.ATTACK.CrossSword);
                    break;
                case Yorihime.COMBO.SwordCombo:
                    attackList.Add(Yorihime.ATTACK.SonicBoom);
                    attackList.Add(Yorihime.ATTACK.SonicBoom2);
                    attackList.Add(Yorihime.ATTACK.SonicBoom);
                    attackList.Add(Yorihime.ATTACK.MoveFront);
                    attackList.Add(Yorihime.ATTACK.DreamSword);
                    break;
                case Yorihime.COMBO.TwinBlade:
                    attackList.Add(Yorihime.ATTACK.SonicBoom);
                    attackList.Add(Yorihime.ATTACK.MoveFront);
                    attackList.Add(Yorihime.ATTACK.WideSword);
                    attackList.Add(Yorihime.ATTACK.CrossSword);
                    attackList.Add(Yorihime.ATTACK.DreamSword);
                    break;
                case Yorihime.COMBO.TryBlade:
                    attackList.Add(Yorihime.ATTACK.WindSword);
                    attackList.Add(Yorihime.ATTACK.MoveTouch);
                    attackList.Add(Yorihime.ATTACK.FighterSword);
                    attackList.Add(Yorihime.ATTACK.WideSword);
                    attackList.Add(Yorihime.ATTACK.CrossSword);
                    break;
                case Yorihime.COMBO.MegaSonic:
                    attackList.Add(Yorihime.ATTACK.MoveSide);
                    attackList.Add(Yorihime.ATTACK.MegaSonic);
                    break;
                case Yorihime.COMBO.GlandBreaker:
                    attackList.Add(Yorihime.ATTACK.GrandBreaker);
                    break;
            }
            this.attacks = attackList.ToArray();
        }

        private void NextCombo()
        {
            ++this.atackNo;
            if (this.atackNo >= this.attacks.Length)
                return;
            this.waittime = 0;
            this.HitFlagReset();
            this.attack = this.attacks[this.atackNo];
        }

        private void ComboEnd()
        {
            this.animationpoint = new Point(0, 0);
            this.speed = this.nspeed;
            this.effecting = false;
            this.HitFlagReset();
            this.speed = 1;
            this.Motion = NaviBase.MOTION.move;
        }

        private void MoveSet()
        {
            ++this.roopmove;
            this.positionre = this.position;
            if (this.HeviSand)
                return;
            this.positionre.X = this.union == Panel.COLOR.blue ? 5 : 0;
            this.positionre.Y = this.Random.Next(3);
            if (!this.Canmove(this.positionre, this.number))
            {
                this.positionre.Y = 0;
                if (!this.Canmove(this.positionre, this.number))
                {
                    this.positionre.Y = 1;
                    if (!this.Canmove(this.positionre, this.number))
                    {
                        this.positionre.Y = 2;
                        if (!this.Canmove(this.positionre, this.number))
                        {
                            this.positionre = this.position;
                            this.Motion = NaviBase.MOTION.neutral;
                        }
                    }
                }
            }
        }

        public override void NockMotion()
        {
            this.animationpoint = new Point(3, 0);
        }

        public override void Render(IRenderer dg)
        {
            if (this.alfha < byte.MaxValue)
                this.color = Color.FromArgb(alfha, this.mastorcolor);
            else
                this.color = this.mastorcolor;
            double num1 = (int)this.positionDirect.X + this.Shake.X;
            int y1 = (int)this.positionDirect.Y;
            Point shake = this.Shake;
            int y2 = shake.Y;
            double num2 = y1 + y2;
            this._position = new Vector2((float)num1, (float)num2);
            this._rect = new Rectangle(this.animationpoint.X * this.wide, (this.version < 4 ? 0 : 2) * this.height, this.wide, this.height);
            if (this.Hp <= 0)
            {
                this.rd = this._rect;
                this.NockMotion();
                int num3 = this.animationpoint.X * this.wide;
                shake = this.Shake;
                int x1 = shake.X;
                int x2 = num3 + x1;
                int y3 = (this.version < 4 ? 0 : 2) * this.height;
                int wide = this.wide;
                int height1 = this.height;
                shake = this.Shake;
                int y4 = shake.Y;
                int height2 = height1 + y4;
                this._rect = new Rectangle(x2, y3, wide, height2);
                this.Death(this._rect, new Rectangle(this.animationpoint.X * this.wide, this.height, this.wide, this.height), this._position, this.picturename);
            }
            if (this.whitetime == 0)
            {
                dg.DrawImage(dg, this.picturename, this._rect, false, this._position, this.rebirth, this.color);
            }
            else
            {
                this._rect.Y = this.height;
                dg.DrawImage(dg, this.picturename, this._rect, false, this._position, this.rebirth, this.color);
            }
            if (this.chargeEffect == 1)
            {
                this._rect = new Rectangle(this.chargeanime % 16 / 2 * 64, 0, 64, 64);
                double num3 = position.X * 40.0 + 16.0 + 8.0;
                shake = this.Shake;
                double x = shake.X;
                double num4 = num3 + x;
                double num5 = position.Y * 24.0 + 58.0;
                shake = this.Shake;
                double y3 = shake.Y;
                double num6 = num5 + y3;
                this._position = new Vector2((float)num4, (float)num6);
                dg.DrawImage(dg, "charge", this._rect, false, this._position, Color.FromArgb(200, byte.MaxValue, byte.MaxValue, byte.MaxValue));
            }
            else if (this.chargeEffect == 2)
            {
                this._rect = new Rectangle(this.chargeanime % 16 / 2 * 64, 64, 64, 64);
                double num3 = position.X * 40.0 + 16.0 + 8.0;
                shake = this.Shake;
                double x = shake.X;
                double num4 = num3 + x;
                double num5 = position.Y * 24.0 + 58.0;
                shake = this.Shake;
                double y3 = shake.Y;
                double num6 = num5 + y3;
                this._position = new Vector2((float)num4, (float)num6);
                dg.DrawImage(dg, "charge", this._rect, false, this._position, Color.FromArgb(200, byte.MaxValue, byte.MaxValue, byte.MaxValue));
            }
            this.HPposition = new Vector2((int)this.positionDirect.X + 28, (int)this.positionDirect.Y - this.height / 2 + 96);
            this.Nameprint(dg, this.printNumber);
        }

        public override void RenderUP(IRenderer dg)
        {
            this.BlackOutRender(dg, this.union);
        }

        private Point AnimeNeutral(int waitflame)
        {
            return this.Return(new int[1], new int[1], 1, waitflame);
        }

        private Point AnimeSlash1(int waitflame)
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
            }, new int[7] { 5, 6, 7, 8, 9, 10, 11 }, 0, waitflame);
        }

        private Point AnimeSlash2(int waitflame)
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
            }, new int[7] { 12, 13, 14, 15, 16, 17, 18 }, 0, waitflame);
        }

        private Point AnimeSlash3(int waitflame)
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
            }, new int[7] { 19, 20, 21, 22, 23, 24, 25 }, 0, waitflame);
        }

        private Point AnimeSlash4(int waitflame)
        {
            return this.ReturnKai(new int[11]
            {
        35,
        1,
        1,
        1,
        1,
        1,
        1,
        1,
        1,
        1,
        100
            }, new int[11]
            {
        26,
        27,
        28,
        29,
        30,
        31,
        32,
        8,
        9,
        10,
        11
            }, 0, waitflame);
        }

        private Point AnimeSlash5(int waitflame)
        {
            return this.Return(new int[7]
            {
        15,
        16,
        17,
        18,
        19,
        20,
        100
            }, new int[7] { 5, 6, 7, 8, 9, 10, 11 }, 0, waitflame);
        }

        private Point AnimeMove(int waitflame)
        {
            return this.Return(new int[7] { 0, 1, 2, 3, 5, 6, 7 }, new int[6]
            {
        0,
        1,
        2,
        2,
        1,
        0
            }, 1, waitflame);
        }

        private enum ATTACK
        {
            SonicBoom,
            FighterSword,
            WindSword,
            WideSword,
            CrossSword,
            MegaSonic,
            GrandBreaker,
            DreamSword,
            SonicBoom2,
            MoveFront,
            MoveTouch,
            MoveSide,
        }

        private enum COMBO
        {
            CounterSword,
            FighterSword,
            DubleSword,
            SwordCombo,
            TwinBlade,
            TryBlade,
            MegaSonic,
            GlandBreaker,
        }
    }
}

