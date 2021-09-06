using NSAttack;
using NSBattle;
using NSBattle.Character;
using NSChip;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEffect;
using NSGame;
using Common.Vectors;
using System.Drawing;

namespace NSEnemy
{
    internal class Youmu : NaviBase
    {
        private readonly int[] pattern = new int[12]
        {
      0,
      0,
      1,
      0,
      2,
      0,
      0,
      1,
      2,
      0,
      0,
      1
        };
        private readonly int[] powers = new int[4] { 50, 10, 30, 0 };
        private readonly int nspeed = 3;
        private Youmu.ATTACK attack;
        private readonly int moveroop;
        private int roopneutral;
        private int roopmove;
        private HalfSoul soul;
        private int action;
        private int atackroop;
        private bool atack;
        private int attackProcess;
        private Point target;

        public Youmu(IAudioEngine s, SceneBattle p, int pX, int pY, byte n, Panel.COLOR u, byte v)
          : base(s, p, pX, pY, n, u, v)
        {
            for (int index = 0; index < this.dropchips.Length; ++index)
                this.dropchips[index] = new ChipFolder(this.sound);
            this.element = ChipBase.ELEMENT.normal;
            switch (this.version)
            {
                case 1:
                    this.name = ShanghaiEXE.Translate("Enemy.YoumuName1");
                    this.power = 80;
                    this.hp = 2300;
                    this.moveroop = 3;
                    break;
                case 2:
                    this.name = ShanghaiEXE.Translate("Enemy.YoumuName2");
                    this.power = 100;
                    this.hp = 2400;
                    this.moveroop = 3;
                    break;
                case 3:
                    this.name = ShanghaiEXE.Translate("Enemy.YoumuName3");
                    this.power = 120;
                    this.hp = 2500;
                    this.moveroop = 2;
                    this.nspeed = 3;
                    break;
                case 4:
                    this.nspeed = 1;
                    this.name = ShanghaiEXE.Translate("Enemy.YoumuName4");
                    this.power = 150;
                    this.hp = 2600;
                    this.moveroop = 5;
                    this.nspeed = 2;
                    break;
                default:
                    this.nspeed = 1;
                    this.name = ShanghaiEXE.Translate("Enemy.YoumuName5") + (version - 3).ToString();
                    this.power = 180;
                    this.hp = 2600 + (version - 4) * 100;
                    this.moveroop = 4;
                    this.nspeed = 2;
                    break;
            }
            this.picturename = "youmu";
            this.guard = CharacterBase.GUARD.noDamage;
            this.armarCount = -1;
            this.race = EnemyBase.ENEMY.navi;
            this.Flying = false;
            this.wide = 104;
            this.height = 96;
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
                    this.dropchips[0].chip = new YoumuV1(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new YoumuV1(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new YoumuV1(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new YoumuV1(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new YoumuV1(this.sound);
                    this.dropchips[4].codeNo = 0;
                    this.havezenny = 0;
                    break;
                case 2:
                    this.dropchips[0].chip = new YoumuV2(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new YoumuV2(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new YoumuV2(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new YoumuV2(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new YoumuV2(this.sound);
                    this.dropchips[4].codeNo = 0;
                    this.havezenny = 0;
                    break;
                case 3:
                    this.dropchips[0].chip = new YoumuV1(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new YoumuV1(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new YoumuV2(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new YoumuV2(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new YoumuV3(this.sound);
                    this.dropchips[4].codeNo = 0;
                    this.havezenny = 800;
                    break;
                default:
                    this.dropchips[0].chip = new YoumuV1(this.sound);
                    this.dropchips[0].codeNo = 1;
                    this.dropchips[1].chip = new YoumuV2(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new YoumuV2(this.sound);
                    this.dropchips[2].codeNo = 1;
                    this.dropchips[3].chip = new YoumuV1(this.sound);
                    this.dropchips[3].codeNo = 2;
                    this.dropchips[4].chip = new YoumuV3(this.sound);
                    this.dropchips[4].codeNo = 1;
                    this.havezenny = 9000;
                    if (this.version < 8)
                        break;
                    this.dropchips[4].chip = new YoumuX(this.sound);
                    this.dropchips[4].codeNo = this.Random.Next(4);
                    this.havezenny = 18000;
                    break;
            }
        }

        public override void PositionDirectSet()
        {
            this.positionDirect = new Vector2((float)(position.X * 40.0 + 4.0), (float)(position.Y * 24.0 + 48.0));
        }

        public override void InitAfter()
        {
            if (this.parent == null)
                return;
            this.soul = new HalfSoul(this.sound, this.parent, this.position.X - this.UnionRebirth(this.union), this.position.Y, this, false);
            this.parent.enemys.Add(soul);
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
                        if (this.waittime >= 16 / version || this.atack)
                        {
                            this.waittime = 0;
                            ++this.roopneutral;
                            if (this.roopneutral >= 1 && this.parent.nowscene != SceneBattle.BATTLESCENE.end)
                            {
                                this.roopneutral = 0;
                                if (this.roopmove > this.moveroop && !this.badstatus[4])
                                {
                                    this.roopmove = this.version > 3 ? this.Random.Next(-1, 1) : 0;
                                    ++this.atackroop;
                                    if (this.parent.player.invincibility || this.parent.player.barrierType != CharacterBase.BARRIER.None || this.parent.player.nohit)
                                        this.atack = true;
                                    if (!this.atack)
                                    {
                                        this.attack = (Youmu.ATTACK)this.pattern[this.action];
                                        if (this.attack == Youmu.ATTACK.RoukanRenzan)
                                            this.powerPlus = this.powers[this.pattern[this.action]] - this.power;
                                        else
                                            this.powerPlus = this.powers[this.pattern[this.action]];
                                        ++this.action;
                                        if (this.action >= this.pattern.Length)
                                            this.action = 0;
                                        this.attackProcess = 0;
                                    }
                                    else
                                    {
                                        this.attack = Youmu.ATTACK.Hakurouken;
                                        this.powerPlus = this.powers[(int)this.attack];
                                        this.atack = false;
                                    }
                                    this.waittime = 0;
                                    this.Motion = NaviBase.MOTION.attack;
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
                                case Youmu.ATTACK.RoukanRenzan:
                                    switch (this.attackProcess)
                                    {
                                        case 0:
                                            this.animationpoint = this.AnimeSlashUP(0);
                                            switch (this.waittime)
                                            {
                                                case 1:
                                                    this.target = this.RandomTarget(this.union);
                                                    this.parent.attacks.Add(new Dummy(this.sound, this.parent, this.target.X, this.target.Y - 1, this.union, new Point(0, 2), 30, true));
                                                    break;
                                                case 4:
                                                    this.counterTiming = true;
                                                    break;
                                                case 8:
                                                    ++this.attackProcess;
                                                    this.waittime = 0;
                                                    break;
                                            }
                                            break;
                                        case 1:
                                            this.animationpoint = this.AnimeSlashUP(this.waittime);
                                            switch (this.waittime)
                                            {
                                                case 2:
                                                    this.sound.PlaySE(SoundEffect.sword);
                                                    SwordAttack swordAttack1 = new SwordAttack(this.sound, this.parent, this.target.X, this.target.Y, this.union, this.Power, 2, ChipBase.ELEMENT.normal, true, true)
                                                    {
                                                        invincibility = false
                                                    };
                                                    this.parent.attacks.Add(swordAttack1);
                                                    this.target = this.RandomTarget(this.union);
                                                    this.parent.attacks.Add(new Dummy(this.sound, this.parent, this.target.X, this.target.Y - 1, this.union, new Point(0, 2), 30, true));
                                                    break;
                                                case 4:
                                                    this.counterTiming = false;
                                                    break;
                                                case 8:
                                                    ++this.attackProcess;
                                                    this.waittime = 0;
                                                    break;
                                            }
                                            break;
                                        case 2:
                                            this.animationpoint = this.AnimeSlashDOWN(this.waittime);
                                            switch (this.waittime)
                                            {
                                                case 2:
                                                    this.sound.PlaySE(SoundEffect.sword);
                                                    SwordAttack swordAttack2 = new SwordAttack(this.sound, this.parent, this.target.X, this.target.Y, this.union, this.Power, 2, ChipBase.ELEMENT.normal, true, true)
                                                    {
                                                        invincibility = false
                                                    };
                                                    this.parent.attacks.Add(swordAttack2);
                                                    if (this.version > 1)
                                                    {
                                                        this.target = this.RandomTarget(this.union);
                                                        this.parent.attacks.Add(new Dummy(this.sound, this.parent, this.target.X, this.target.Y - 1, this.union, new Point(0, 2), 30, true));
                                                        break;
                                                    }
                                                    break;
                                                case 8:
                                                    if (this.version > 1)
                                                    {
                                                        ++this.attackProcess;
                                                        this.waittime = 0;
                                                        break;
                                                    }
                                                    this.speed = this.nspeed;
                                                    this.SoulAttack();
                                                    this.motion = NaviBase.MOTION.move;
                                                    this.waittime = 0;
                                                    break;
                                            }
                                            break;
                                        case 3:
                                            this.animationpoint = this.AnimeSlashUP(this.waittime);
                                            switch (this.waittime)
                                            {
                                                case 2:
                                                    this.sound.PlaySE(SoundEffect.sword);
                                                    SwordAttack swordAttack3 = new SwordAttack(this.sound, this.parent, this.target.X, this.target.Y, this.union, this.Power, 2, ChipBase.ELEMENT.normal, true, true)
                                                    {
                                                        invincibility = false
                                                    };
                                                    this.parent.attacks.Add(swordAttack3);
                                                    break;
                                                case 5:
                                                    this.speed = this.nspeed;
                                                    this.SoulAttack();
                                                    this.motion = NaviBase.MOTION.move;
                                                    this.waittime = 0;
                                                    break;
                                            }
                                            break;
                                    }
                                    break;
                                case Youmu.ATTACK.NishinIttaiken:
                                    this.animationpoint = this.AnimeSlashNishin(this.waittime);
                                    switch (this.waittime)
                                    {
                                        case 1:
                                            this.soul.LostFlag(true);
                                            this.guard = CharacterBase.GUARD.none;
                                            this.counterTiming = true;
                                            this.speed += 2;
                                            this.sound.PlaySE(SoundEffect.charge);
                                            break;
                                        case 10:
                                            this.counterTiming = false;
                                            this.sound.PlaySE(SoundEffect.sword);
                                            int s = 4;
                                            AttackBase attackBase1 = new BombWave(this.sound, this.parent, this.position.X + this.UnionRebirth(this.union), 0, this.union, this.Power, s, this.element);
                                            attackBase1.invincibility = false;
                                            this.parent.attacks.Add(attackBase1);
                                            AttackBase attackBase2 = new BombWave(this.sound, this.parent, this.position.X + this.UnionRebirth(this.union), 1, this.union, this.Power, s, this.element);
                                            attackBase2.invincibility = false;
                                            this.parent.attacks.Add(attackBase2);
                                            AttackBase attackBase3 = new BombWave(this.sound, this.parent, this.position.X + this.UnionRebirth(this.union), 2, this.union, this.Power, s, this.element);
                                            attackBase3.invincibility = false;
                                            this.parent.attacks.Add(attackBase3);
                                            break;
                                        case 25:
                                            this.speed = this.nspeed;
                                            this.guard = CharacterBase.GUARD.noDamage;
                                            this.motion = NaviBase.MOTION.move;
                                            this.waittime = 0;
                                            this.soul.LostFlag(false);
                                            break;
                                    }
                                    break;
                                case Youmu.ATTACK.IaiJuujizan:
                                    if (this.attackProcess == 0)
                                    {
                                        this.animationpoint = this.AnimeIai(this.waittime);
                                        switch (this.waittime)
                                        {
                                            case 1:
                                                this.counterTiming = true;
                                                this.parent.effects.Add(new Flash(this.sound, this.parent, this.positionDirect, this.position));
                                                this.sound.PlaySE(SoundEffect.pikin);
                                                this.speed += 2;
                                                break;
                                            case 2:
                                                this.target = this.RandomTarget(this.union);
                                                this.effecting = true;
                                                this.positionReserved = this.position;
                                                this.positionre = new Point(this.union == Panel.COLOR.blue ? 0 : 5, this.target.Y);
                                                this.position = this.positionre;
                                                this.PositionDirectSet();
                                                break;
                                            case 6:
                                                this.counterTiming = false;
                                                this.sound.PlaySE(SoundEffect.sword);
                                                SwordCloss swordCloss = new SwordCloss(this.sound, this.parent, this.target.X, this.target.Y, this.union, this.Power, 2, ChipBase.ELEMENT.normal, true)
                                                {
                                                    invincibility = false
                                                };
                                                this.parent.attacks.Add(swordCloss);
                                                break;
                                            case 25:
                                                this.positionReserved = null;
                                                this.speed = this.nspeed;
                                                this.motion = NaviBase.MOTION.move;
                                                this.effecting = false;
                                                this.speed = this.nspeed;
                                                this.waittime = 0;
                                                break;
                                        }
                                    }
                                    break;
                                case Youmu.ATTACK.Hakurouken:
                                    this.animationpoint = this.AnimeSlashHakurou(this.waittime);
                                    switch (this.waittime)
                                    {
                                        case 1:
                                            this.counterTiming = true;
                                            break;
                                        case 2:
                                            this.sound.PlaySE(SoundEffect.sword);
                                            foreach (CharacterBase characterBase in this.parent.AllChara())
                                            {
                                                if (characterBase.union == this.UnionEnemy && (characterBase.guard != CharacterBase.GUARD.none || characterBase.invincibilitytime > 0 || characterBase.nohit || (uint)characterBase.barrierType > 0U))
                                                {
                                                    characterBase.Hp -= this.Power;
                                                    characterBase.invincibilitytime = 0;
                                                    characterBase.barrierType = CharacterBase.BARRIER.None;
                                                    this.sound.PlaySE(SoundEffect.damageenemy);
                                                }
                                            }
                                            break;
                                        case 4:
                                            this.counterTiming = false;
                                            break;
                                        case 8:
                                            this.speed = this.nspeed;
                                            this.motion = NaviBase.MOTION.move;
                                            this.waittime = 0;
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
                            this.guard = CharacterBase.GUARD.noDamage;
                            if (this.soul.motion == HalfSoul.MOTION.lost)
                                this.soul.LostFlag(false);
                            this.NockMotion();
                            this.counterTiming = false;
                            this.effecting = false;
                            this.PositionDirectSet();
                            break;
                        case 3:
                            this.NockMotion();
                            break;
                        case 15:
                            this.animationpoint = new Point(0, 1);
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
            if (this.effecting)
                this.AttackMake(this.Power, 0, 0);
            this.FlameControl();
            this.MoveAftar();
        }

        private void SoulAttack()
        {
            if (this.hp > this.hpmax / 2)
                return;
            this.soul.attack = true;
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
            this._rect = new Rectangle(this.animationpoint.X * this.wide, 864 * (this.version < 4 ? 0 : 2) + this.animationpoint.Y * this.height, this.wide, this.height);
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
                this.Death(this._rect, new Rectangle(this.animationpoint.X * this.wide, 864 + this.animationpoint.Y * this.height, this.wide, this.height), this._position, this.picturename);
            }
            if (this.whitetime == 0)
            {
                dg.DrawImage(dg, this.picturename, this._rect, false, this._position, this.rebirth, this.color);
            }
            else
            {
                this._rect.Y = 864 + this.animationpoint.Y * this.height;
                dg.DrawImage(dg, this.picturename, this._rect, false, this._position, this.rebirth, this.color);
            }
            this.HPposition = new Vector2((int)this.positionDirect.X + 20, (int)this.positionDirect.Y + this.height / 2 - 6);
            this.Nameprint(dg, this.printNumber);
        }

        private Point AnimeNeutral(int waitflame)
        {
            return this.Return(new int[1], new int[1], 0, waitflame);
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
            }, 0, waitflame);
        }

        private Point AnimeSlashUP(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[5]
            {
        1,
        1,
        1,
        1,
        100
            }, new int[5] { 0, 1, 2, 3, 4 }, 1, waittime);
        }

        private Point AnimeSlashDOWN(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[5]
            {
        1,
        1,
        1,
        1,
        100
            }, new int[5] { 0, 1, 2, 3, 4 }, 3, waittime);
        }

        private Point AnimeSlashHakurou(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[5]
            {
        1,
        1,
        1,
        1,
        100
            }, new int[5] { 0, 1, 2, 3, 4 }, 2, waittime);
        }

        private Point AnimeSlashNishin(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[14]
            {
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
        1,
        1,
        1,
        100
            }, new int[14]
            {
        0,
        1,
        2,
        3,
        4,
        5,
        6,
        7,
        8,
        9,
        10,
        11,
        12,
        13
            }, 6, waittime);
        }

        private Point AnimeIai(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[4]
            {
        1,
        1,
        1,
        100
            }, new int[4] { 0, 1, 2, 3 }, 5, waittime);
        }

        private enum ATTACK
        {
            RoukanRenzan,
            NishinIttaiken,
            IaiJuujizan,
            Hakurouken,
        }
    }
}

