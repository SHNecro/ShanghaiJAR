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
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace NSEnemy
{
    internal class ScissorMan : NaviBase
    {
        private readonly int[] pattern = new int[18]
        {
      2,
      0,
      0,
      1,
      0,
      2,
      1,
      0,
      1,
      2,
      0,
      0,
      1,
      0,
      2,
      1,
      0,
      1
        };
        private readonly int[] pattern2 = new int[18]
        {
      2,
      0,
      3,
      1,
      0,
      2,
      1,
      3,
      1,
      3,
      3,
      1,
      0,
      2,
      0,
      0,
      1,
      1
        };
        private readonly int[] powers = new int[5] { 40, -30, 0, 0, 0 };
        private readonly int nspeed = 2;
        private readonly int aspeed = 4;
        private readonly Point[,] target = new Point[2, 9];
        private int action;
        private bool ready;
        private int attackCount;
        private readonly int moveroop;
        private ScissorMan.ATTACK attack;
        private int roopneutral;
        private int roopmove;
        private bool KillMode;
        private int time;
        private Vector2 endposition;
        private float movex;
        private float movey;
        private float plusy;
        private float speedy;
        private float plusing;
        private const int startspeed = 4;
        private int flyflame;
        private readonly int animaHP;
        private int attackroop;
        private bool NextAttack;
        private Point[] posis;
        private int attackMode;
        private AnimaBall ball;
        private bool distracted;
        private bool originalReverse;

        private int Action
        {
            get
            {
                return this.action;
            }
            set
            {
                this.action = value;
                if (this.action < this.pattern.Length)
                    return;
                this.action = 0;
            }
        }

        public ScissorMan(IAudioEngine s, SceneBattle p, int pX, int pY, byte n, Panel.COLOR u, byte v)
          : base(s, p, pX, pY, n, u, v)
        {
            for (int index = 0; index < this.dropchips.Length; ++index)
                this.dropchips[index] = new ChipFolder(this.sound);
            this.element = ChipBase.ELEMENT.leaf;
            switch (this.version)
            {
                case 0:
                    this.name = ShanghaiEXE.Translate("Enemy.ScissorManName1");
                    this.power = 80;
                    this.hp = 2100;
                    this.moveroop = 2;
                    this.nspeed = 4;
                    this.animaHP = 350;
                    this.version = 1;
                    break;
                case 1:
                    this.name = ShanghaiEXE.Translate("Enemy.ScissorManName2");
                    this.power = 80;
                    this.hp = 2000;
                    this.moveroop = 2;
                    this.nspeed = 4;
                    this.animaHP = 300;
                    break;
                case 2:
                    this.name = ShanghaiEXE.Translate("Enemy.ScissorManName3");
                    this.power = 80;
                    this.hp = 2200;
                    this.moveroop = 2;
                    this.nspeed = 4;
                    this.animaHP = 350;
                    break;
                case 3:
                    this.name = ShanghaiEXE.Translate("Enemy.ScissorManName4");
                    this.power = 100;
                    this.hp = 2400;
                    this.moveroop = 3;
                    this.nspeed = 4;
                    this.animaHP = 400;
                    break;
                case 4:
                    this.name = ShanghaiEXE.Translate("Enemy.ScissorManName5");
                    this.power = 200;
                    this.hp = 2600;
                    this.moveroop = 3;
                    this.nspeed = 4;
                    this.aspeed = 3;
                    this.animaHP = 500;
                    break;
                default:
                    this.name = ShanghaiEXE.Translate("Enemy.ScissorManName6") + (version - 3).ToString();
                    this.power = 200;
                    this.hp = 2800 + (version - 4) * 400;
                    this.animaHP = 700;
                    this.moveroop = 3;
                    this.nspeed = 4;
                    this.aspeed = 3;
                    break;
            }
            this.speed = this.nspeed;
            this.picturename = nameof(ScissorMan);
            this.race = EnemyBase.ENEMY.navi;
            this.wide = 160;
            this.height = 144;
            this.hpmax = this.hp;
            this.hpprint = this.hp;
            this.printhp = true;
            this.effecting = false;
            this.Flying = false;
            this.roopmove = 0;
            this.PositionDirectSet();
            switch (this.version)
            {
                case 1:
                    this.dropchips[0].chip = new ScissorManV1(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new ScissorManV1(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new ScissorManV1(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new ScissorManV1(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new ScissorManV1(this.sound);
                    this.dropchips[4].codeNo = 0;
                    this.havezenny = 0;
                    break;
                case 2:
                    this.dropchips[0].chip = new ScissorManV2(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new ScissorManV2(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new ScissorManV2(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new ScissorManV2(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new ScissorManV2(this.sound);
                    this.dropchips[4].codeNo = 0;
                    this.havezenny = 0;
                    break;
                case 3:
                    this.dropchips[0].chip = new ScissorManV1(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new ScissorManV1(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new ScissorManV2(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new ScissorManV2(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new ScissorManV3(this.sound);
                    this.dropchips[4].codeNo = 0;
                    this.havezenny = 8800;
                    break;
                default:
                    this.dropchips[0].chip = new ScissorManV1(this.sound);
                    this.dropchips[0].codeNo = 1;
                    this.dropchips[1].chip = new ScissorManV2(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new ScissorManV2(this.sound);
                    this.dropchips[2].codeNo = 1;
                    this.dropchips[3].chip = new ScissorManV1(this.sound);
                    this.dropchips[3].codeNo = 2;
                    this.dropchips[4].chip = new ScissorManV3(this.sound);
                    this.dropchips[4].codeNo = 1;
                    this.havezenny = 9000;
                    if (this.version >= 8)
                    {
                        this.dropchips[4].chip = new ScissorManX(this.sound);
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
            this.positionDirect = new Vector2((float)(position.X * 40.0 + 4.0), (float)(position.Y * 24.0 + 48.0));
        }

        protected override void Moving()
        {
            this.neutlal = this.Motion == NaviBase.MOTION.neutral;
            switch (this.Motion)
            {
                case NaviBase.MOTION.neutral:
                    this.animationpoint.X = 0;
                    if (this.moveflame)
                        ++this.waittime;
                    if (this.moveflame && this.waittime >= 8 / version)
                    {
                        this.waittime = 0;
                        ++this.roopneutral;
                        this.animationpoint = this.AnimeNeutral(this.waittime);
                        if (this.roopneutral >= 1 && this.parent.nowscene != SceneBattle.BATTLESCENE.end)
                        {
                            this.roopneutral = 0;
                            if (this.roopmove > this.moveroop && !this.badstatus[4])
                            {
                                this.roopmove = 0;
                                this.speed = 1;
                                ++this.attackroop;
                                this.waittime = 0;
                                this.ready = false;
                                this.NextAttack = false;
                                this.powerPlus = this.powers[(int)this.attack];
                                this.Motion = NaviBase.MOTION.attack;
                            }
                            else
                            {
                                this.Badstatusresist = false;
                                this.superArmor = false;
                                if (this.hp <= this.HpMax / 2)
                                {
                                    this.attack = (ScissorMan.ATTACK)this.pattern2[this.action];
                                    if (this.parent.player.Hp <= this.animaHP && this.Random.Next(10) >= 5)
                                    {
                                        this.attack = ScissorMan.ATTACK.AnimaDeleater;
                                        this.Badstatusresist = true;
                                        this.superArmor = true;
                                    }
                                    this.NextAttack = true;
                                    ++this.action;
                                    if (this.action >= this.pattern.Length)
                                        this.action = 0;
                                }
                                else
                                {
                                    this.attack = (ScissorMan.ATTACK)this.pattern[this.action];
                                    if (this.parent.player.Hp <= this.animaHP && this.Random.Next(10) >= 5)
                                    {
                                        this.attack = ScissorMan.ATTACK.AnimaDeleater;
                                        this.Badstatusresist = true;
                                        this.superArmor = true;
                                    }
                                    this.NextAttack = true;
                                    ++this.action;
                                    if (this.action >= this.pattern2.Length)
                                        this.action = 0;
                                }
                                this.powerPlus = this.powers[this.pattern[this.action]];
                                this.attackMode = 0;
                                this.waittime = 0;
                                ++this.roopmove;
                                this.Motion = NaviBase.MOTION.move;
                            }
                        }
                        break;
                    }
                    break;
                case NaviBase.MOTION.attack:
                    switch (this.attack)
                    {
                        case ScissorMan.ATTACK.CrossScissor:
                            if (this.moveflame)
                                ++this.waittime;
                            if (this.moveflame)
                            {
                                this.animationpoint = this.AnimeCross(this.waittime);
                                switch (this.waittime)
                                {
                                    case 1:
                                        this.MoveRandom(true, true);
                                        this.parent.attacks.Add(new Dummy(this.sound, this.parent, this.positionre.X + this.UnionRebirth(this.union), this.positionre.Y, this.union, new Point(), 15, true));
                                        this.parent.attacks.Add(new Dummy(this.sound, this.parent, this.positionre.X + this.UnionRebirth(this.union) - 1, this.positionre.Y - 1, this.union, new Point(), 15, true));
                                        this.parent.attacks.Add(new Dummy(this.sound, this.parent, this.positionre.X + this.UnionRebirth(this.union) + 1, this.positionre.Y - 1, this.union, new Point(), 15, true));
                                        this.parent.attacks.Add(new Dummy(this.sound, this.parent, this.positionre.X + this.UnionRebirth(this.union) - 1, this.positionre.Y + 1, this.union, new Point(), 15, true));
                                        this.parent.attacks.Add(new Dummy(this.sound, this.parent, this.positionre.X + this.UnionRebirth(this.union) + 1, this.positionre.Y + 1, this.union, new Point(), 15, true));
                                        break;
                                    case 5:
                                        this.counterTiming = true;
                                        this.effecting = true;
                                        if (!this.HeviSand)
                                        {
                                            this.parent.effects.Add(new StepShadow(this.sound, this.parent, new Rectangle(this.animationpoint.X * this.wide, (this.version < 4 ? 0 : 2) * this.height, this.wide, this.height), this.positionDirect, this.picturename, this.rebirth, this.position));
                                            this.position = this.positionre;
                                            this.PositionDirectSet();
                                        }
                                        this.PositionDirectSet();
                                        break;
                                    case 7:
                                        this.counterTiming = false;
                                        this.sound.PlaySE(SoundEffect.breakObject);
                                        this.ShakeStart(5, 5);
                                        this.parent.attacks.Add(new SwordCloss(this.sound, this.parent, this.position.X + this.UnionRebirth(this.union), this.position.Y, this.union, this.Power, 2, this.element, false));
                                        break;
                                    case 12:
                                        this.effecting = false;
                                        this.attackCount = 0;
                                        this.roopneutral = 0;
                                        this.Motion = NaviBase.MOTION.move;
                                        this.speed = this.nspeed;
                                        break;
                                }
                                break;
                            }
                            break;
                        case ScissorMan.ATTACK.SlashRush:
                            if (this.moveflame)
                            {
                                ++this.waittime;
                                if (this.attackMode == 0)
                                {
                                    this.counterTiming = true;
                                    this.animationpoint = this.AnimeSlashRush1(this.waittime);
                                    switch (this.waittime)
                                    {
                                        case 1:
                                            this.parent.attacks.Add(new Dummy(this.sound, this.parent, this.posis[1].X, this.posis[1].Y, this.union, new Point(), 30, true));
                                            break;
                                        case 3:
                                            this.waittime = 0;
                                            this.counterTiming = false;
                                            ++this.attackMode;
                                            this.parent.attacks.Add(new Dummy(this.sound, this.parent, this.posis[2].X, this.posis[2].Y, this.union, new Point(), 30, true));
                                            break;
                                    }
                                }
                                else
                                {
                                    if (this.attackMode % 2 == 1)
                                        this.animationpoint = this.AnimeSlashRush2(this.waittime);
                                    else
                                        this.animationpoint = this.AnimeSlashRush3(this.waittime);
                                    int index1 = this.attackMode % this.posis.Length;
                                    switch (this.waittime)
                                    {
                                        case 2:
                                            this.sound.PlaySE(SoundEffect.sword);
                                            KnifeAttack knifeAttack = new KnifeAttack(this.sound, this.parent, this.posis[index1].X, this.posis[index1].Y, this.union, this.Power, 2, this.element, false)
                                            {
                                                invincibility = false
                                            };
                                            this.parent.attacks.Add(knifeAttack);
                                            break;
                                        case 4:
                                            ++this.attackMode;
                                            if (this.attackMode < 9 + version * 4 - 1)
                                            {
                                                int index2 = (this.attackMode + 1) % this.posis.Length;
                                                this.parent.attacks.Add(new Dummy(this.sound, this.parent, this.posis[index2].X, this.posis[index2].Y, this.union, new Point(), 30, true));
                                            }
                                            this.waittime = 0;
                                            if (this.attackMode >= 9 + version * 4)
                                            {
                                                this.roopneutral = 0;
                                                this.Motion = NaviBase.MOTION.move;
                                                this.speed = this.nspeed;
                                                break;
                                            }
                                            break;
                                    }
                                }
                                break;
                            }
                            break;
                        case ScissorMan.ATTACK.MeteorSickle:
                            switch (this.attackMode)
                            {
                                case 0:
                                    if (this.moveflame)
                                    {
                                        ++this.waittime;
                                        this.animationpoint = this.AnimeMeteorSickle1(this.waittime);
                                        switch (this.waittime)
                                        {
                                            case 1:
                                                this.posis = new Point[2];
                                                this.posis[0].X = this.union == Panel.COLOR.blue ? 0 : 5;
                                                this.posis[0].Y = this.Random.Next(3);
                                                this.parent.attacks.Add(new Dummy(this.sound, this.parent, this.posis[0].X, this.posis[0].Y, this.union, new Point(), 30, true));
                                                break;
                                            case 3:
                                                this.waittime = 0;
                                                this.counterTiming = false;
                                                this.nohit = true;
                                                ++this.attackMode;
                                                this.Throw();
                                                break;
                                        }
                                        break;
                                    }
                                    break;
                                case 1:
                                    if (this.moveflame)
                                    {
                                        this.animationpoint = this.AnimeMeteorSickle2(this.waittime % 2 + 1);
                                        ++this.waittime;
                                    }
                                    if (this.flyflame == this.time / 3)
                                    {
                                        this.sound.PlaySE(SoundEffect.knife);
                                        this.parent.attacks.Add(new DelayScissor(this.sound, this.parent, this.posis[0].X, this.posis[0].Y, this.union, this.Power, 16, this.element, new Vector2(this.positionDirect.X + 10 * this.UnionRebirth(this.union), this.positionDirect.Y - 10f + this.plusy), this.version > 3, 10));
                                        this.posis[1] = this.RandomTarget();
                                        this.parent.attacks.Add(new Dummy(this.sound, this.parent, this.posis[1].X, this.posis[1].Y, this.union, new Point(), 30, true));
                                    }
                                    if (this.flyflame == this.time / 3 * 2)
                                    {
                                        this.sound.PlaySE(SoundEffect.knife);
                                        this.parent.attacks.Add(new DelayScissor(this.sound, this.parent, this.posis[1].X, this.posis[1].Y, this.union, this.Power, 16, this.element, new Vector2(this.positionDirect.X + 10 * this.UnionRebirth(this.union), this.positionDirect.Y - 10f + this.plusy), this.version > 3, 10));
                                    }
                                    if (this.flyflame == this.time)
                                    {
                                        this.nohit = false;
                                        this.roopneutral = 0;
                                        this.Motion = NaviBase.MOTION.neutral;
                                        this.animationpoint.X = 22;
                                        this.speed = this.nspeed;
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
                            break;
                        case ScissorMan.ATTACK.DeathWiper:
                            if (this.moveflame)
                                ++this.waittime;
                            if (this.moveflame)
                            {
                                this.animationpoint = this.AnimeDeathWiper(this.waittime);
                                switch (this.waittime)
                                {
                                    case 1:
                                        this.MoveRandom(true, true);
                                        this.parent.attacks.Add(new Dummy(this.sound, this.parent, this.position.X + this.UnionRebirth(this.union), this.position.Y - 1, this.union, new Point(1, 0), 15, true));
                                        this.parent.attacks.Add(new Dummy(this.sound, this.parent, this.position.X + this.UnionRebirth(this.union), this.position.Y, this.union, new Point(1, 0), 15, true));
                                        this.parent.attacks.Add(new Dummy(this.sound, this.parent, this.position.X + this.UnionRebirth(this.union), this.position.Y + 1, this.union, new Point(1, 0), 15, true));
                                        break;
                                    case 5:
                                        this.counterTiming = true;
                                        this.sound.PlaySE(SoundEffect.sword);
                                        this.parent.attacks.Add(new Halberd(this.sound, this.parent, this.position.X + this.UnionRebirth(this.union), this.position.Y, this.union, this.Power, 2, this.element, false));
                                        break;
                                    case 7:
                                        this.counterTiming = false;
                                        break;
                                    case 10:
                                        this.effecting = false;
                                        this.attackCount = 0;
                                        this.roopneutral = 0;
                                        this.Motion = NaviBase.MOTION.move;
                                        this.speed = this.nspeed;
                                        break;
                                }
                                break;
                            }
                            break;
                        case ScissorMan.ATTACK.AnimaDeleater:
                            switch (this.attackMode)
                            {
                                case 0:
                                    this.animationpoint = this.AnimeDeleater1(this.waittime);
                                    if (this.moveflame)
                                    {
                                        ++this.waittime;
                                        switch (this.waittime)
                                        {
                                            case 0:
                                            case 1:
                                            case 2:
                                            case 3:
                                            case 4:
                                            case 5:
                                            case 7:
                                            case 8:
                                                break;
                                            case 6:
                                                this.ball = new AnimaBall(this.sound, this.parent, this.position.X, this.position.Y, this.union, 0, 1, new Vector2(this.positionDirect.X, this.positionDirect.Y + 24f), this.element, 8);
                                                this.parent.attacks.Add(ball);
                                                goto case 0;
                                            default:
                                                bool flag = false;
                                                if (this.ball != null)
                                                {
                                                    if (this.ball.hit)
                                                    {
                                                        this.waittime = 0;
                                                        ++this.attackMode;
                                                    }
                                                    else if (!this.ball.flag)
                                                        flag = true;
                                                }
                                                else
                                                {
                                                    flag = true;
                                                    this.nohit = true;
                                                    this.animationpoint.X = 0;
                                                }
                                                if (flag)
                                                {
                                                    this.Badstatusresist = false;
                                                    this.superArmor = false;
                                                    this.roopneutral = 0;
                                                    this.Motion = NaviBase.MOTION.neutral;
                                                    this.animationpoint.X = 0;
                                                    this.speed = this.nspeed;
                                                    goto case 0;
                                                }
                                                else
                                                    goto case 0;
                                        }
                                    }
                                    break;
                                case 1:
                                    this.parent.effects.Add(new StepShadow(this.sound, this.parent, new Rectangle(this.animationpoint.X * this.wide, (this.version < 4 ? 0 : 2) * this.height, this.wide, this.height), this.positionDirect, this.picturename, this.rebirth, this.position));
                                    this.position = new Point(this.parent.player.position.X - 2 * this.UnionRebirth(this.union), this.parent.player.position.Y);
                                    this.PositionDirectSet();
                                    ++this.attackMode;
                                    break;
                                case 2:
                                    if (!this.distracted)
                                    {
                                        this.animationpoint = this.AnimeDeleater2(this.waittime);
                                    }
                                    if (this.moveflame)
                                    {
                                        ++this.waittime;
                                        switch (this.waittime)
                                        {
                                            case 4:
                                                this.sound.PlaySE(SoundEffect.damageplayer);
                                                this.parent.player.Hp /= 2;
                                                break;
                                            case 10:
                                                this.sound.PlaySE(SoundEffect.damageplayer);
                                                this.parent.player.Hp /= 2;
                                                break;
                                            case 16:
                                                this.parent.effects.Add(new ScreenFlash(this.sound, this.parent, Color.Red, 0));
                                                this.ball.black = true;
                                                this.KillMode = true;
                                                this.parent.player.Hp = 0;
                                                break;
                                            default:
                                                if (this.parent.player.Hp > 0)
                                                {
                                                    if (this.waittime >= 30)
                                                    {
                                                        if (!this.distracted && this.waittime < 60)
                                                        {
                                                            this.MoveRandom(false, false);
                                                            this.position = this.positionre;
                                                            this.PositionDirectSet();
                                                            this.distracted = true;
                                                            this.originalReverse = this.rebirth;
                                                            this.rebirth ^= true;
                                                        }

                                                        if (this.waittime < 35)
                                                        {
                                                            this.animationpoint = this.AnimeSlashRush1(this.waittime % 5);
                                                        }
                                                        else
                                                        {
                                                            this.animationpoint = (this.waittime % 10 < 5) ? this.AnimeSlashRush2(this.waittime % 5) : this.AnimeSlashRush3(this.waittime % 5);

                                                            if (this.waittime % 5 == 0)
                                                            {
                                                                this.rebirth ^= true;
                                                            }
                                                            if (this.waittime % 10 == 0)
                                                            {
                                                                this.MoveRandom(false, false);
                                                                this.position = this.positionre;
                                                                this.PositionDirectSet();
                                                            }
                                                        }
                                                    }

                                                    if (this.waittime >= 55 && this.waittime < 240)
                                                    {
                                                        this.parent.effects.RemoveAll(e => e is ScreenFlash);
                                                        this.ball.black = false;
                                                        this.ball.hit = false;
                                                        this.KillMode = false;
                                                        this.parent.player.printplayer = true;
                                                    }
                                                    if (this.waittime >= 240)
                                                    {
                                                        this.Motion = MOTION.knockback;
                                                    }
                                                }
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
                    this.animationpoint.X = 0;
                    if (this.moveflame)
                        ++this.waittime;
                    ++this.roopmove;
                    this.Motion = NaviBase.MOTION.neutral;
                    if (this.NextAttack)
                    {
                        switch (this.attack)
                        {
                            case ScissorMan.ATTACK.SlashRush:
                                List<Point> source = new List<Point>();
                                for (int x = 0; x < this.parent.panel.GetLength(0); ++x)
                                {
                                    for (int y = 0; y < this.parent.panel.GetLength(1); ++y)
                                    {
                                        if (this.parent.panel[x, y].color == this.UnionEnemy)
                                            source.Add(new Point(x, y));
                                    }
                                }
                                this.posis = source.OrderBy<Point, Guid>(i => Guid.NewGuid()).ToArray<Point>();
                                this.MoveRandom(false, false);
                                break;
                            case ScissorMan.ATTACK.DeathWiper:
                                this.MoveRandom(true, true);
                                break;
                            case ScissorMan.ATTACK.AnimaDeleater:
                                this.MoveRandom(true, true);
                                break;
                            default:
                                this.MoveRandom(false, false);
                                break;
                        }
                    }
                    else
                        this.MoveRandom(false, false);
                    this.speed = this.nspeed;
                    if (this.attackroop > 4)
                        this.attackroop = 0;
                    if (this.position == this.positionre)
                    {
                        this.frame = 0;
                        this.roopneutral = 0;
                        break;
                    }
                    this.parent.effects.Add(new StepShadow(this.sound, this.parent, new Rectangle(this.animationpoint.X * this.wide, (this.version < 4 ? 0 : 2) * this.height, this.wide, this.height), this.positionDirect, this.picturename, this.rebirth, this.position));
                    this.position = this.positionre;
                    this.PositionDirectSet();
                    this.frame = 0;
                    this.roopneutral = 0;
                    break;
                case NaviBase.MOTION.knockback:
                    if (this.distracted)
                    {
                        this.distracted = false;
                        this.parent.effects.RemoveAll(e => e is ScreenFlash);
                        this.ball.black = false;
                        this.ball.hit = false;
                        this.KillMode = false;
                        this.parent.player.printplayer = true;
                        this.rebirth = this.originalReverse;
                    }
                    switch (this.waittime)
                    {
                        case 2:
                            this.Badstatusresist = false;
                            this.superArmor = false;
                            this.rebirth = this.union == Panel.COLOR.red;
                            this.ready = false;
                            this.attackCount = 0;
                            this.speed = this.nspeed;
                            this.effecting = false;
                            this.NockMotion();
                            this.counterTiming = false;
                            this.effecting = false;
                            this.PositionDirectSet();
                            break;
                        case 3:
                            this.NockMotion();
                            break;
                        case 15:
                            this.animationpoint = new Point(9, 0);
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
            if (this.motion == NaviBase.MOTION.attack)
                this.FlameControl(this.aspeed);
            else
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
            double num1 = (int)this.positionDirect.X + this.Shake.X + (this.rebirth ? 40 : 0);
            int y1 = (int)this.positionDirect.Y;
            Point shake = this.Shake;
            int y2 = shake.Y;
            double num2 = y1 + y2 + (double)this.plusy;
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
                if (!this.KillMode)
                    dg.DrawImage(dg, this.picturename, this._rect, false, this._position, this.rebirth, this.color);
                else
                    dg.DrawImage(dg, this.picturename, this._rect, false, this._position, this.rebirth, Color.Black);
            }
            else
            {
                this._rect.Y = this.height;
                dg.DrawImage(dg, this.picturename, this._rect, false, this._position, this.rebirth, this.color);
            }
            this.HPposition = new Vector2((int)this.positionDirect.X + 20, (int)this.positionDirect.Y + this.height / 2 - 32);
            this.Nameprint(dg, this.printNumber);
        }

        private Point AnimeNeutral(int waitflame)
        {
            return this.Return(new int[1], new int[1], 1, waitflame);
        }

        private void Throw()
        {
            this.animationpoint.X = 9;
            this.waittime = 0;
            this.time = 60;
            this.flyflame = 0;
            ++this.roopmove;
            this.powerPlus = this.powers[3];
            Point position = this.position;
            this.Noslip = true;
            this.MoveRandom(false, false);
            this.endposition = new Vector2((float)(positionre.X * 40.0 + 4.0), (float)(positionre.Y * 24.0 + 48.0));
            this.movex = (this.positionDirect.X - this.endposition.X) / time;
            this.movey = (this.positionDirect.Y - this.endposition.Y) / time;
            this.plusy = 0.0f;
            this.speedy = 4f;
            this.plusing = this.speedy / (this.time / 2);
            this.flyflame = 0;
        }

        private Point AnimeCross(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[9]
            {
        1,
        1,
        1,
        1,
        2,
        1,
        1,
        1,
        100
            }, new int[9] { 0, 2, 3, 14, 15, 16, 17, 18, 19 }, 0, waittime);
        }

        private Point AnimeDeathWiper(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[5]
            {
        2,
        1,
        1,
        1,
        100
            }, new int[5] { 9, 10, 11, 12, 13 }, 0, waittime);
        }

        private Point AnimeSlashRush1(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[2]
            {
        1,
        10
            }, new int[2] { 2, 3 }, 0, waittime);
        }

        private Point AnimeSlashRush2(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[4]
            {
        1,
        1,
        1,
        10
            }, new int[4] { 4, 5, 6, 7 }, 0, waittime);
        }

        private Point AnimeSlashRush3(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[4]
            {
        1,
        1,
        1,
        10
            }, new int[4] { 10, 11, 12, 13 }, 0, waittime);
        }

        private Point AnimeMeteorSickle1(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[3]
            {
        1,
        1,
        10
            }, new int[3] { 20, 21, 22 }, 0, waittime);
        }

        private Point AnimeMeteorSickle2(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[2]
            {
        1,
        1
            }, new int[2] { 23, 24 }, 0, waittime);
        }

        private Point AnimeDeleater1(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[6]
            {
        1,
        1,
        3,
        1,
        1,
        100
            }, new int[6] { 2, 3, 4, 5, 6, 7 }, 0, waittime);
        }

        private Point AnimeDeleater2(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[18]
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
        1,
        1,
        1,
        1,
        100
            }, new int[18]
            {
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
        13,
        14,
        15,
        16,
        17,
        18,
        19
            }, 0, waittime);
        }

        private enum ATTACK
        {
            CrossScissor,
            SlashRush,
            MeteorSickle,
            DeathWiper,
            AnimaDeleater,
        }
    }
}

