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

using NSEffect;

using NSObject;

using System.Collections.Generic;

using NSNet;


namespace NSEnemy
{
    internal class Flandre : NaviBase
    {
        private readonly int[] powers = new int[4] { 20, 10, 30, 0 };
        private readonly int nspeed = 2;
        private readonly int moveroop;
        private Flandre.ATTACK attack;
        private int attackspeed;
        private int roopneutral;
        private int roopmove;
        private int atackroop;
        private int preX;
        private readonly bool atack;
        private int knifeTrue = 0;
        private int knifeNum = 0;
        //private Point[] knifePos;
        int[] knifeX = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        int[] knifeY = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        //AttackBase levStart = new AttackBase();
        private readonly AttackBase[] levStart = new AttackBase[2];

        public Flandre(IAudioEngine s, SceneBattle p, int pX, int pY, byte n, Panel.COLOR u, byte v)
          : base(s, p, pX, pY, n, u, v)
        {
            for (int index = 0; index < this.dropchips.Length; ++index)
                this.dropchips[index] = new ChipFolder(this.sound);
            this.element = ChipBase.ELEMENT.normal;
            attackspeed = 3;
            switch (this.version)
            {
                case 1:
                    this.name = ShanghaiEXE.Translate("Enemy.FlandreName1");
                    this.power = 120;
                    this.hp = 2400;
                    this.moveroop = 1;
                    break;
                case 2:
                    this.name = ShanghaiEXE.Translate("Enemy.FlandreName2");
                    this.power = 150;
                    this.hp = 2800;
                    this.moveroop = 1;
                    attackspeed = 2;
                    break;
                case 3:
                    this.name = ShanghaiEXE.Translate("Enemy.FlandreName3");
                    this.power = 180;
                    this.hp = 3200;
                    this.moveroop = 2;
                    attackspeed = 2;
                    break;
                case 4:
                    this.nspeed = 1;
                    this.name = ShanghaiEXE.Translate("Enemy.FlandreName4");
                    this.power = 210;
                    this.hp = 3600;
                    this.moveroop = 2;
                    attackspeed = 1;
                    break;
                default:
                    this.nspeed = 1;
                    this.name = ShanghaiEXE.Translate("Enemy.FlandreName4") + (version - 3).ToString();
                    this.power = 250;
                    this.hp = 3000 + (version - 4) * 350;
                    this.moveroop = 3;
                    attackspeed = 1;
                    break;
            }
            this.picturename = "flandre";
            this.race = EnemyBase.ENEMY.navi;
            this.Flying = false;
            this.wide = 78;
            this.height = 78;
            this.hpmax = this.hp;
            this.speed = this.nspeed;
            this.hpprint = this.hp;
            this.printhp = true;
            this.effecting = false;
            this.Flying = false;
            //Point[] knifePos = new Point[version-1];
            this.roopmove = n * 3;
            this.PositionDirectSet();
            switch (this.version)
            {
                case 1:
                    this.dropchips[0].chip = new FlandreV1(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new FlandreV1(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new FlandreV1(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new FlandreV1(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new FlandreV1(this.sound);
                    this.dropchips[4].codeNo = 0;
                    this.havezenny = 0;
                    break;
                case 2:
                    this.dropchips[0].chip = new FlandreV2(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new FlandreV2(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new FlandreV2(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new FlandreV2(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new FlandreV2(this.sound);
                    this.dropchips[4].codeNo = 0;
                    this.havezenny = 0;
                    break;
                case 3:
                    this.dropchips[0].chip = new FlandreV1(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new FlandreV1(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new FlandreV2(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new FlandreV2(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new FlandreV3(this.sound);
                    this.dropchips[4].codeNo = 0;
                    this.havezenny = 800;
                    break;
                default:
                    this.dropchips[0].chip = new FlandreV1(this.sound);
                    this.dropchips[0].codeNo = 1;
                    this.dropchips[1].chip = new FlandreV2(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new FlandreV2(this.sound);
                    this.dropchips[2].codeNo = 1;
                    this.dropchips[3].chip = new FlandreV1(this.sound);
                    this.dropchips[3].codeNo = 2;
                    this.dropchips[4].chip = new FlandreV3(this.sound);
                    this.dropchips[4].codeNo = 1;
                    this.havezenny = 9000;
                    if (this.version < 8)
                        break;
                    this.dropchips[4].chip = new FlandreDS(this.sound);
                    this.dropchips[4].codeNo = 0;
                    this.havezenny = 18000;
                    break;
            }
        }

        public override void PositionDirectSet()
        {
            this.positionDirect = new Vector2((float)(position.X * 40.0 + 16.0), (float)(position.Y * 24.0 + 58.0));
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
                        if (this.waittime >= 8 / version || this.atack)
                        {
                            this.waittime = 0;
                            ++this.roopneutral;
                            if (this.roopneutral >= 1 && this.parent.nowscene != SceneBattle.BATTLESCENE.end)
                            {
                                this.roopneutral = 0;
                                if (this.roopmove > this.moveroop && !this.badstatus[4])
                                {
                                    this.roopmove = this.version > 0 ? this.Random.Next(-1, this.moveroop + 1) : 0;
                                    ++this.atackroop;
                                    this.speed = 1;
                                    if (!this.atack)
                                    {
                                        //int index = this.Random.Next(this.version > 0 ? 3 : 1);
                                        int randMax = 4;
                                        /*
                                        if (this.Hp <= this.HpMax / 2) {
                                            randMax = 5;
                                        }*/
                                        int index = this.Random.Next(randMax);

                                        //if (index == 5) { index = 4; }
                                        /*
                                        if (index != 4)
                                        {
                                            this.attack = (Flandre.ATTACK)index;
                                        }
                                        else
                                        {
                                            this.attack = Flandre.ATTACK.earthBreaker;
                                        }*/
                                        this.attack = (Flandre.ATTACK)index;
                                        //this.attack = Flandre.ATTACK.laevateinn;
                                        this.powerPlus = this.powers[index];
                                    }
                                    this.waittime = 0;
                                    this.Motion = NaviBase.MOTION.attack;
                                    this.counterTiming = true;
                                    knifeTrue = 0;
                                    knifeNum = Math.Max(version - 1, 3);
                                    //Point[] knifePos = new Point[knifeNum];
                                }
                                else
                                {
                                    this.waittime = 0;
                                    if (this.atack)
                                        this.roopmove = this.moveroop + 1;
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
                                case Flandre.ATTACK.flanGear:
                                    this.animationpoint = this.AnimeSpark(this.waittime);
                                    int gearNum = 1;
                                    //if (version > 1) { gearNum = 2; }
                                    for (int gearA = 0; gearA < gearNum; gearA++)
                                    {
                                        switch (this.waittime)
                                        {
                                            case 2:
                                                this.MoveRandom(false, false, this.union == Panel.COLOR.red ? Panel.COLOR.blue : Panel.COLOR.red, 0);
                                                Point point1 = this.positionre;
                                                this.positionre = this.position;
                                                knifeX[0] = point1.X;
                                                knifeY[0] = point1.Y;
                                                int gTime = 60 + 22;
                                                this.parent.attacks.Add(new Dummy(this.sound, this.parent, point1.X, point1.Y, this.union, new Point(), gTime, true));
                                                this.parent.attacks.Add(new Dummy(this.sound, this.parent, point1.X - 1, point1.Y, this.union, new Point(), gTime, true));
                                                this.parent.attacks.Add(new Dummy(this.sound, this.parent, point1.X + 1, point1.Y, this.union, new Point(), gTime, true));
                                                this.parent.attacks.Add(new Dummy(this.sound, this.parent, point1.X, point1.Y - 1, this.union, new Point(), gTime, true));
                                                this.parent.attacks.Add(new Dummy(this.sound, this.parent, point1.X, point1.Y + 1, this.union, new Point(), gTime, true));

                                                break;
                                            case 34:
                                                this.counterTiming = false;
                                                //this.sound.PlaySE(SoundEffect.beam);
                                                //Point point1 = this.RandomTarget();

                                                //this.parent.attacks.Add(new Beam(this.sound, this.parent, this.position.X + this.UnionRebirth(this.union), this.position.Y, this.union, this.Power, 2, false));
                                                this.parent.attacks.Add(new FlanGear(this.sound, this.parent, knifeX[0], knifeY[0], this.union, this.Power, 0));
                                                break;
                                            case 64:
                                                this.roopneutral = 0;
                                                this.Motion = NaviBase.MOTION.neutral;
                                                if (!this.atack)
                                                {
                                                    this.speed = this.nspeed;
                                                    break;
                                                }
                                                break;
                                        }
                                    }
                                    if (this.waittime != 24)
                                        break;
                                    break;
                                case Flandre.ATTACK.randomSword:
                                    this.animationpoint = this.AnimeBomb(this.waittime);
                                    //KnifeAttack[] knifeArr = new KnifeAttack[3];
                                    //int knifeTrue = 0;
                                    
                                    int knifeNum = Math.Max(version-1,3);
                                    //Point[] knifePos = new Point[knifeNum];
                                    //int[] knifeX = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                                    //int[] knifeY = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

                                    switch (this.waittime)
                                    {
                                        case 1:
                                            //this.counterTiming = false;
                                            //this.sound.PlaySE(SoundEffect.throw_);
                                            Point point1 = this.RandomTarget();
                                            knifeX[0] = point1.X;
                                            knifeY[0] = point1.Y;
                                            this.parent.attacks.Add(new Dummy(this.sound, this.parent, point1.X, point1.Y, this.union, new Point(), 30, true));
                                            knifeTrue += 1;

                                            this.RandomTarget();
                                            Vector2 v = new Vector2(this.positionDirect.X + 8 * this.UnionRebirth(this.union), this.positionDirect.Y - 8f);
                                            //KnifeAttack[] knifeArr = new KnifeAttack[3];
                                            for (int seed = 1; seed < knifeNum; ++seed)
                                            {
                                                this.MoveRandom(false, false, this.union == Panel.COLOR.red ? Panel.COLOR.blue : Panel.COLOR.red, seed);
                                                Point positionre = this.positionre;
                                                knifeX[seed] = this.positionre.X;
                                                knifeY[seed] = this.positionre.Y;
                                                //knifePos[knifeTrue] = this.positionre;
                                                //this.parent.attacks.Add(new ClossBomb(this.sound, this.parent, this.positionre.X, this.positionre.Y, this.union, this.Power, 1, v, positionre, 40, ClossBomb.TYPE.closs, false, ClossBomb.TYPE.big, false, false));

                                                //this.sound.PlaySE(SoundEffect.sword);
                                                /*
                                                KnifeAttack knifeAttack = new KnifeAttack(this.sound, this.parent, this.positionre.X, this.positionre.Y, this.union, this.Power, 2, this.element, false)
                                                {
                                                    invincibility = false
                                                };
                                                knifeArr[knifeTrue] = knifeAttack;*/
                                                knifeTrue +=1;
                                                this.parent.attacks.Add(new Dummy(this.sound, this.parent, this.positionre.X, this.positionre.Y, this.union, new Point(), 30, true));
                                                //this.parent.attacks.Add(knifeAttack);

                                            }
                                            this.positionre = this.position;
                                            break;

                                        case 36:
                                            //this.sound.PlaySE(SoundEffect.sword);
                                            this.counterTiming = false;
                                            for (int sva = 0; sva < knifeTrue; sva++)
                                            {
                                                
                                                this.sound.PlaySE(SoundEffect.sword);
                                                KnifeAttack knifeAttack = new KnifeAttack(this.sound, this.parent, knifeX[sva], knifeY[sva], this.union, this.Power, 2, ChipBase.ELEMENT.heat, false)
                                                {
                                                    invincibility = false
                                                };
                                                this.parent.attacks.Add(knifeAttack);
                                                
                                            }
                                            break;

                                        case 46:
                                            this.roopneutral = 0;
                                            this.Motion = NaviBase.MOTION.neutral;
                                            if (!this.atack)
                                            {
                                                this.speed = this.nspeed;
                                                break;
                                            }
                                            break;
                                    }
                                    break;

                                case Flandre.ATTACK.bunshin:
                                    this.animationpoint = this.bunshin(this.waittime);

                                    switch (this.waittime)
                                    {
                                        case 1:
                                            
                                            //this.sound.PlaySE(SoundEffect.charge); // need to change this to use a different SE than Earth Breaker, playtest issue
                                            //this.speed = this.attackspeed;
                                            break;
                                        case 4:
                                            this.counterTiming = true;
                                            break;
                                        case 20:
                                            this.counterTiming = false;
                                            this.sound.PlaySE(SoundEffect.enterenemy);
                                            this.MoveRandom(false, false);
                                            Point posRef = this.positionre;
                                            this.positionre = this.position;
                                            //this.parent.effects.Add(new MoveEnemy(this.sound, this.parent, posRef.X, posRef.Y));
                                            flanBunshin bunshinAttack = new flanBunshin(this.sound, this.parent, posRef.X, posRef.Y, this.union, this.Power);
                                            
                                            this.parent.attacks.Add(bunshinAttack);

                                            this.roopneutral = 0;
                                            this.Motion = NaviBase.MOTION.neutral;
                                            if (!this.atack)
                                            {
                                                this.speed = this.nspeed;
                                                break;
                                            }
                                            break;
                                    }
                                    break;
                                case Flandre.ATTACK.laevateinn:
                                    switch (this.waittime)
                                    {
                                        case 1:
                                            this.counterTiming = false;
                                            this.animationpoint.X = -1;
                                            this.levStart[0] = new flanLev(this.sound, this.parent, this.position.X, this.position.Y, this.union, this.Power, this.speed);
                                            //this.levStart = levStart;
                                            this.parent.attacks.Add(levStart[0]);

                                            break;
                                        case 17:
                                            this.MoveRandom(true, true);

                                            this.Shadow();
                                            this.positionReserved = this.position;
                                            this.position = this.positionre;
                                            this.PositionDirectSet();

                                            levStart[0].position = this.position;
                                            break;
                                        case 26:
                                            flanLevF levEnd = new flanLevF(this.sound, this.parent, this.position.X, this.position.Y, this.union, this.Power, this.speed);

                                            this.parent.attacks.Add(levEnd);
                                            break;
                                        case 40:

                                            this.roopneutral = 0;
                                            this.Motion = NaviBase.MOTION.neutral;
                                            if (!this.atack)
                                            {
                                                this.speed = this.nspeed;
                                                break;
                                            }

                                            break;
                                        
                                    }
                                    break;
                                case Flandre.ATTACK.earthBreaker:
                                    this.animationpoint = this.AnimeMissile(this.waittime);
                                    //this.animationpoint.X = this.AnimeSlash4(this.frame).X;
                                    switch (this.waittime)
                                    {
                                        case 1:
                                            this.counterTiming = false;
                                            //this.chargeanime = 0;
                                            this.sound.PlaySE(SoundEffect.charge);
                                            this.speed = this.attackspeed;
                                            //this.chargeEffect = 1;
                                            break;
                                        case 20:
                                            this.counterTiming = true;
                                            this.sound.PlaySE(SoundEffect.chargemax);
                                            //this.chargeEffect = 2;
                                            
                                            break;
                                        case 35:
                                            this.speed = this.nspeed;
                                            //this.chargeEffect = 0;
                                            this.preX = this.position.X;
                                            this.positionReserved = this.position;
                                            this.position.X = this.TargetX(this, this.parent);
                                            if (this.position.X < 0)
                                            {
                                                this.position.X = 0;
                                            }
                                            if (this.position.X > 5)
                                            {
                                                this.position.X = 5;
                                            }

                                            this.PositionDirectSet();
                                            break;
                                        case 38:
                                            //this.effecting = true;
                                            //this.Shadow();
                                            /*
                                            int x = this.union == Panel.COLOR.blue ? 2 : 3;
                                            if (!this.HeviSand)
                                                this.position = new Point(x, 1);
                                            this.PositionDirectSet();*/

                                            

                                            break;
                                        case 45:
                                            this.counterTiming = false;
                                            this.speed = this.attackspeed;
                                            this.sound.PlaySE(SoundEffect.bombmiddle);
                                            base.ShakeStart(2, 16);
                                            for (int j = 0; j < 3; j++)
                                            {
                                                AttackBase attackBase = new BombAttack(this.sound, this.parent, this.position.X + (1 + 0) * base.UnionRebirth(this.union), j, this.union, this.Power*2, 4, this.element);
                                                //AttackBase attackBase = new BombAttack(this.sound, this.parent, this.position.X + (1 + 0) * base.UnionRebirth(this.union), j, this.union, this.Power * 2, 1, 1, new Point(), this.element);

                                                attackBase.breaking = true;
                                                this.parent.attacks.Add(attackBase);
                                                //BombAttack bombAttack = new BombAttack(this.sound, this.parent, this.position.X + (1 + index) * this.UnionRebirth(this.union), pY, this.union, this.Power, 1, 1, new Point(), ChipBase.ELEMENT.normal);

                                                if (!this.parent.panel[this.position.X + (1 + 0) * base.UnionRebirth(this.union), j].OnCharaCheck())
                                                {
                                                    this.parent.panel[this.position.X + (1 + 0) * base.UnionRebirth(this.union), j].Break();
                                                }
                                                else
                                                {
                                                    this.parent.panel[this.position.X + (1 + 0) * base.UnionRebirth(this.union), j].Crack();
                                                }


                                                attackBase.invincibility = true;

                                                //this.parent.attacks.Add(base.Paralyze(attackBase));
                                                this.parent.effects.Add(new Shock(this.sound, this.parent, attackBase.position.X, attackBase.position.Y, 2, Panel.COLOR.red));
                                            }
                                            break;
                                        case 50:
                                            this.roopneutral = 0;
                                            this.Motion = NaviBase.MOTION.neutral;
                                            this.positionReserved = null;
                                            this.position.X = this.preX;
                                            this.PositionDirectSet();
                                            if (!this.atack)
                                            {
                                                this.speed = this.nspeed;
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
                                this.MoveRandom(false, false);
                                if (this.position == this.positionre)
                                {
                                    this.Motion = NaviBase.MOTION.neutral;
                                    this.frame = 0;
                                    this.roopneutral = 0;
                                    ++this.roopmove;
                                    break;
                                }
                                break;
                            case 3:
                                this.positionReserved = null;
                                this.position = this.positionre;
                                this.PositionDirectSet();
                                break;
                            case 5:
                                this.Motion = NaviBase.MOTION.neutral;
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
                            this.counterTiming = false;
                            this.effecting = false;
                            this.PositionDirectSet();
                            break;
                        case 3:
                            this.NockMotion();
                            break;
                        case 15:
                            this.animationpoint = new Point(20, 0);
                            this.PositionDirectSet();
                            break;
                        case 21:
                            this.animationpoint = new Point(0, 0);
                            this.waittime = 0;
                            this.Motion = NaviBase.MOTION.neutral;
                            break;
                    }
                    if (this.waittime >= 2 && this.waittime <= 6)
                        this.positionDirect.X -= this.UnionRebirth(this.union);
                    ++this.waittime;
                    break;
            }
            this.FlameControl();
            this.MoveAftar();
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

            int xOff = 4;
            int yOff = -12;

            this._position = new Vector2((float)num1+xOff, (float)num2+yOff);
            //this._position = new Vector2((int)this.positionDirect.X + xOff + this.Shake.X, (int)this.positionDirect.Y + yOff + this.Shake.Y);
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
            this.HPposition = new Vector2((int)this.positionDirect.X + 8, (int)this.positionDirect.Y - 8 - this.height / 2 + 8);
            this.Nameprint(dg, this.printNumber);
        }

        private Point AnimeNeutral(int waitflame)
        {
            return this.Return(new int[1], new int[1], 1, waitflame);
        }

        private Point AnimeMove(int waitflame)
        {
            return this.Return(new int[5] { 0, 1, 2, 4, 5 }, new int[5]
            {
        0,
        1,
        2,
        1,
        0
            }, 1, waitflame);
        }

        private Point AnimeSpark(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[10]
            {
        4,
        4,
        4,
        4,
        4,
        4,
        60,
        4,
        4,
        4
            }, new int[11] { 0, 4, 5, 6, 7, 8, 11, 5, 4, 0, 0 }, 0, waittime);
        }

        private Point AnimeBomb(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[6]
            {
        8,
        8,
        4,
        4,
        4,
        4
            }, new int[6] { 0, 15, 16, 17, 18, 19 }, 0, waittime);
        }

        private Point AnimeMissile(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[10]
            {
        4,
        4,
        8,
        18,
        4,
        3,
        4,
        4,
        4,
        60
            }, new int[10] { 0, 20, 21, 22, 23, 24, 25, 26, 27, 28 }, 0, waittime);
        }

        private Point bunshin(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[4]
            {
        4,
        4,
        8,
        4
            }, new int[4] { 0, 20, 21, 22 }, 0, waittime);
        }

        private Point AnimeSlash4(int waitflame)
        {
            int[] array = new int[]
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
                1,
                100
            };
            int[] xpoint = new int[]
            {
                20,
                21,
                22,
                0,
                1,
                2,
                23,
                24,
                25,
                26,
                27,
                28
            };
            int y = 0;
            return CharacterAnimation.Return(array, xpoint, y, waitflame);
        }

        private enum ATTACK
        {
            flanGear,
            randomSword,
            bunshin, earthBreaker,
            laevateinn,
            //earthBreaker,
        }

        protected int TargetX(CharacterBase character, SceneBattle battle)
        {
            List<CharacterBase> characterBaseList = new List<CharacterBase>();
            foreach (CharacterBase characterBase in battle.AllHitter())
            {
                if (characterBase is EnemyBase)
                {
                    if (characterBase.union == character.UnionEnemy)
                        characterBaseList.Add(characterBase);
                }
                else if (characterBase is Player)
                {
                    if (characterBase.union == character.UnionEnemy)
                        characterBaseList.Add(characterBase);
                }
                else if (characterBase is ObjectBase)
                {
                    ObjectBase objectBase = (ObjectBase)characterBase;
                    if ((objectBase.unionhit || objectBase.union == character.union) && character.UnionEnemy == objectBase.StandPanel.color)
                        characterBaseList.Add(characterBase);
                }
            }
            bool flag = false;
            int num = character.union == Panel.COLOR.red ? 6 : -1;
            foreach (CharacterBase characterBase in characterBaseList)
            {
                if (characterBase.position.Y == character.position.Y)
                {
                    flag = true;
                    if (character.union == Panel.COLOR.red)
                    {
                        if (num > characterBase.position.X)
                            num = characterBase.position.X;
                    }
                    else if (num < characterBase.position.X)
                        num = characterBase.position.X;
                }
            }
            if (flag)
                return num - this.UnionRebirth(character.union);
            foreach (CharacterBase characterBase in characterBaseList)
            {
                if (characterBase.position.Y != character.position.Y)
                {
                    if (character.union == Panel.COLOR.red)
                    {
                        if (num > characterBase.position.X)
                            num = characterBase.position.X;
                    }
                    else if (num < characterBase.position.X)
                        num = characterBase.position.X;
                }
            }
            return num - this.UnionRebirth(character.union);
        }


    }
}

