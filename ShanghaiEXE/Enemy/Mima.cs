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
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace NSEnemy
{
    internal class Mima : NaviBase
    {
        private readonly int[] powers = new int[6]
        {
      0,
      0,
      50,
      200,
      100,
      0
        };
        private readonly int[] pattern = new int[12]
        {
      0,
      1,
      2,
      3,
      1,
      1,
      0,
      2,
      4,
      3,
      3,
      5
        };
        private readonly int[] pattern2 = new int[12]
        {
      0,
      1,
      5,
      3,
      4,
      4,
      0,
      2,
      4,
      3,
      3,
      5
        };
        private readonly int nspeed = 4;
        private List<Point> targetPanels = new List<Point>();
        private int action;
        private readonly int moveroop;
        private Mima.ATTACK attack;
        private int roopneutral;
        private int roopmove;
        private int atackroop;
        private readonly bool atack;
        private int attackProcess;
        private MimaCharge mimacharge;
        private bool godmode;
        private bool godmodeinit;

        public Mima(IAudioEngine s, SceneBattle p, int pX, int pY, byte n, Panel.COLOR u, byte v)
          : base(s, p, pX, pY, n, u, v)
        {
            for (int index = 0; index < this.dropchips.Length; ++index)
                this.dropchips[index] = new ChipFolder(this.sound);
            this.element = ChipBase.ELEMENT.poison;
            if (this.version != 1)
            {
                this.version = 8;
            }

            switch (this.version)
			{
				case 1:
                    this.name = ShanghaiEXE.Translate("Enemy.MimaName2");
                    this.power = 100;
                    this.hp = 3000;
                    this.moveroop = 1;
                    break;
                default:
                    this.nspeed = 1;
                    this.name = ShanghaiEXE.Translate("Enemy.MimaName1");
                    this.power = 200;
                    this.hp = 3000 + (version - 4) * 500;
                    this.moveroop = 3;
                    break;
            }
            this.picturename = "mima";
            this.race = EnemyBase.ENEMY.navi;
            this.Flying = false;
            this.wide = 80;
            this.height = 80;
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
                    this.dropchips[0].chip = new MarisaV1(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new MarisaV1(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new MarisaV1(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new MarisaV1(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new MarisaV1(this.sound);
                    this.dropchips[4].codeNo = 0;
                    this.havezenny = 0;
                    break;
                case 2:
                    this.dropchips[0].chip = new MarisaV2(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new MarisaV2(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new MarisaV2(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new MarisaV2(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new MarisaV2(this.sound);
                    this.dropchips[4].codeNo = 0;
                    this.havezenny = 0;
                    break;
                case 3:
                    this.dropchips[0].chip = new MarisaV1(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new MarisaV1(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new MarisaV2(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new MarisaV2(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new MarisaV3(this.sound);
                    this.dropchips[4].codeNo = 0;
                    this.havezenny = 800;
                    break;
                default:
                    this.dropchips[0].chip = new MarisaV1(this.sound);
                    this.dropchips[0].codeNo = 1;
                    this.dropchips[1].chip = new MarisaV2(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new MarisaV2(this.sound);
                    this.dropchips[2].codeNo = 1;
                    this.dropchips[3].chip = new MarisaV1(this.sound);
                    this.dropchips[3].codeNo = 2;
                    this.dropchips[4].chip = new MarisaV3(this.sound);
                    this.dropchips[4].codeNo = 1;
                    this.havezenny = 9000;
                    if (this.version < 8)
                        break;
                    this.dropchips[4].chip = new MarisaX(this.sound);
                    this.dropchips[4].codeNo = this.Random.Next(4);
                    this.havezenny = 18000;
                    break;
            }
        }

        public override void PositionDirectSet()
        {
            this.positionDirect = new Vector2((float)(position.X * 40.0 + 20.0), (float)(position.Y * 24.0 + 50.0));
        }

        protected override void Moving()
        {
            if (!this.godmode && this.Hp <= this.HpMax / 2)
            {
                this.GodMode();
            }
            else
            {
                this.neutlal = this.Motion == NaviBase.MOTION.neutral;
                switch (this.Motion)
                {
                    case NaviBase.MOTION.neutral:
                        if (this.moveflame)
                        {
                            this.animationpoint = this.AnimeNeutral(this.waittime % 3);
                            ++this.waittime;
                        }
                        if (this.moveflame && (this.waittime >= 8 / (version == 0 ? 5 : version) || this.atack))
                        {
                            this.waittime = 0;
                            ++this.roopneutral;
                            if (this.roopneutral >= 1 && this.parent.nowscene != SceneBattle.BATTLESCENE.end)
                            {
                                this.roopneutral = 0;
                                if (this.roopmove > this.moveroop && !this.badstatus[4])
                                {
                                    this.roopmove = this.version > 3 ? this.Random.Next(-1, this.moveroop + 1) : 0;
                                    ++this.atackroop;
                                    if (!this.atack)
                                    {
                                        if (this.hp <= this.hpmax / 2)
                                        {
                                            this.attack = (Mima.ATTACK)this.pattern2[this.action];
                                            this.powerPlus = this.powers[this.pattern2[this.action]];
                                        }
                                        else
                                        {
                                            this.attack = (Mima.ATTACK)this.pattern[this.action];
                                            this.powerPlus = this.powers[this.pattern[this.action]];
                                        }
                                        ++this.action;
                                        if (this.action >= this.pattern.Length)
                                            this.action = 0;
                                        switch (this.attack)
                                        {
                                            case Mima.ATTACK.IllProminence:
                                                this.counterTiming = true;
                                                this.sound.PlaySE(SoundEffect.pikin);
                                                this.speed = 4;
                                                this.targetPanels = ((IEnumerable<Point>)this.RandomMultiPanel(4, this.UnionEnemy)).ToList<Point>();
                                                for (int index = 0; index < this.targetPanels.Count; ++index)
                                                {
                                                    List<AttackBase> attacks = this.parent.attacks;
                                                    IAudioEngine sound = this.sound;
                                                    SceneBattle parent = this.parent;
                                                    Point point = this.targetPanels[index];
                                                    int x = point.X;
                                                    point = this.targetPanels[index];
                                                    int y = point.Y;
                                                    int union = (int)this.union;
                                                    point = new Point();
                                                    Point hitrange = point;
                                                    Dummy dummy = new Dummy(sound, parent, x, y, (Panel.COLOR)union, hitrange, 25, true);
                                                    attacks.Add(dummy);
                                                }
                                                break;
                                            case Mima.ATTACK.DarkWave:
                                                this.sound.PlaySE(SoundEffect.sand);
                                                this.speed = 3;
                                                break;
                                            case Mima.ATTACK.GrandSpear:
                                                this.sound.PlaySE(SoundEffect.quake);
                                                this.ShakeStart(1, 30);
                                                this.speed = 4;
                                                break;
                                            case Mima.ATTACK.SoulFlame:
                                                this.speed = 3;
                                                break;
                                            case Mima.ATTACK.CrescentCharge:
                                                this.speed = 3;
                                                this.parent.attacks.Add(new Dummy(this.sound, this.parent, this.union == Panel.COLOR.blue ? 5 : 0, 1, this.union, new Point(6, 0), 30, true));
                                                break;
                                            case Mima.ATTACK.Reincarnation:
                                                this.sound.PlaySE(SoundEffect.charge);
                                                this.speed = 3;
                                                break;
                                        }
                                        this.attackProcess = 0;
                                    }
                                    this.waittime = 0;
                                    this.Motion = NaviBase.MOTION.attack;
                                }
                                else
                                {
                                    this.speed = 4;
                                    this.waittime = 0;
                                    if (this.atack)
                                        this.roopmove = this.moveroop + 1;
                                    this.Motion = NaviBase.MOTION.move;
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
                                    case Mima.ATTACK.IllProminence:
                                        if (this.attackProcess == 0)
                                        {
                                            this.animationpoint = this.AnimeIllProminence1(this.waittime);
                                            switch (this.waittime)
                                            {
                                                case 2:
                                                    this.counterTiming = false;
                                                    ++this.attackProcess;
                                                    this.waittime = 0;
                                                    break;
                                            }
                                        }
                                        else
                                        {
                                            this.animationpoint = this.AnimeIllProminence2(this.waittime);
                                            switch (this.waittime)
                                            {
                                                case 3:
                                                    this.sound.PlaySE(SoundEffect.bombmiddle);
                                                    Point point;
                                                    for (int index = 0; index < this.targetPanels.Count; ++index)
                                                    {
                                                        this.ShakeStart(2, 8);
                                                        List<AttackBase> attacks = this.parent.attacks;
                                                        IAudioEngine sound1 = this.sound;
                                                        SceneBattle parent1 = this.parent;
                                                        point = this.targetPanels[index];
                                                        int x1 = point.X;
                                                        point = this.targetPanels[index];
                                                        int y1 = point.Y;
                                                        int union = (int)this.union;
                                                        int power = this.Power;
                                                        BombAttack bombAttack = new BombAttack(sound1, parent1, x1, y1, (Panel.COLOR)union, power, 1, ChipBase.ELEMENT.poison);
                                                        attacks.Add(bombAttack);
                                                        List<EffectBase> effects = this.parent.effects;
                                                        IAudioEngine sound2 = this.sound;
                                                        SceneBattle parent2 = this.parent;
                                                        point = this.targetPanels[index];
                                                        int x2 = point.X;
                                                        point = this.targetPanels[index];
                                                        int y2 = point.Y;
                                                        Bomber bomber = new Bomber(sound2, parent2, x2, y2, Bomber.BOMBERTYPE.poison, 3);
                                                        effects.Add(bomber);
                                                    }
                                                    if (this.attackProcess < 2)
                                                    {
                                                        this.targetPanels = ((IEnumerable<Point>)this.RandomMultiPanel(4, this.UnionEnemy)).ToList<Point>();
                                                        for (int index = 0; index < this.targetPanels.Count; ++index)
                                                        {
                                                            List<AttackBase> attacks = this.parent.attacks;
                                                            IAudioEngine sound = this.sound;
                                                            SceneBattle parent = this.parent;
                                                            point = this.targetPanels[index];
                                                            int x = point.X;
                                                            point = this.targetPanels[index];
                                                            int y = point.Y;
                                                            int union = (int)this.union;
                                                            point = new Point();
                                                            Point hitrange = point;
                                                            Dummy dummy = new Dummy(sound, parent, x, y, (Panel.COLOR)union, hitrange, 25, true);
                                                            attacks.Add(dummy);
                                                        }
                                                        break;
                                                    }
                                                    break;
                                                case 8:
                                                    if (this.attackProcess >= 2)
                                                    {
                                                        this.waittime = 0;
                                                        this.motion = NaviBase.MOTION.move;
                                                        break;
                                                    }
                                                    ++this.attackProcess;
                                                    this.waittime = 0;
                                                    break;
                                            }
                                        }
                                        break;
                                    case Mima.ATTACK.DarkWave:
                                        switch (this.attackProcess)
                                        {
                                            case 0:
                                                this.animationpoint = this.AnimeDarkWave1(this.waittime);
                                                switch (this.waittime)
                                                {
                                                    case 1:
                                                        this.effecting = true;
                                                        this.positionre = this.RandomTarget(this.union);
                                                        this.positionre.X -= this.UnionRebirth(this.union);
                                                        if (!this.NoObject(this.positionre, this.number))
                                                        {
                                                            this.nohit = false;
                                                            this.Motion = NaviBase.MOTION.move;
                                                            this.frame = 0;
                                                            this.waittime = 0;
                                                            this.roopneutral = 0;
                                                            ++this.roopmove;
                                                            break;
                                                        }
                                                        break;
                                                    case 6:
                                                        this.counterTiming = true;
                                                        this.position = this.positionre;
                                                        this.PositionDirectSet();
                                                        break;
                                                    case 9:
                                                        ++this.attackProcess;
                                                        this.waittime = 0;
                                                        break;
                                                }
                                                break;
                                            case 1:
                                                this.animationpoint = this.AnimeDarkWave2(this.waittime);
                                                switch (this.waittime)
                                                {
                                                    case 3:
                                                        this.counterTiming = false;
                                                        this.sound.PlaySE(SoundEffect.wave);
                                                        this.parent.effects.Add(new MimaWaveLong(this.sound, this.parent, this.position.X + this.UnionRebirth(this.union), this.position.Y, 3));
                                                        AttackBase attackBase1 = new LanceAttack(this.sound, this.parent, this.position.X + this.UnionRebirth(this.union), this.position.Y, this.union, this.Power, 2, ChipBase.ELEMENT.aqua, false);
                                                        attackBase1.invincibility = false;
                                                        this.parent.attacks.Add(attackBase1);
                                                        break;
                                                    case 7:
                                                        ++this.attackProcess;
                                                        this.waittime = 0;
                                                        break;
                                                }
                                                break;
                                            case 2:
                                                this.animationpoint = this.AnimeDarkWave3(this.waittime);
                                                switch (this.waittime)
                                                {
                                                    case 3:
                                                        this.sound.PlaySE(SoundEffect.wave);
                                                        this.parent.effects.Add(new MimaWaveDream(this.sound, this.parent, this.position.X + this.UnionRebirth(this.union), this.position.Y, 3));
                                                        AttackBase attackBase2 = new Halberd(this.sound, this.parent, this.position.X, this.position.Y, this.union, this.Power, 2, ChipBase.ELEMENT.aqua, true);
                                                        attackBase2.invincibility = false;
                                                        this.parent.attacks.Add(attackBase2);
                                                        break;
                                                    case 6:
                                                        this.waittime = 0;
                                                        this.effecting = false;
                                                        this.motion = NaviBase.MOTION.move;
                                                        break;
                                                }
                                                break;
                                        }
                                        break;
                                    case Mima.ATTACK.GrandSpear:
                                        switch (this.attackProcess)
                                        {
                                            case 0:
                                                this.animationpoint = this.AnimeMove(this.waittime);
                                                if (this.moveflame)
                                                {
                                                    switch (this.waittime)
                                                    {
                                                        case 1:
                                                            this.MoveRandom(true, false);
                                                            break;
                                                        case 5:
                                                            this.counterTiming = true;
                                                            this.position = this.positionre;
                                                            this.PositionDirectSet();
                                                            break;
                                                        case 10:
                                                            this.nohit = false;
                                                            ++this.attackProcess;
                                                            this.waittime = 0;
                                                            break;
                                                    }
                                                    break;
                                                }
                                                break;
                                            case 1:
                                                this.animationpoint = this.AnimeGrandSpear(this.waittime);
                                                if (this.moveflame)
                                                {
                                                    switch (this.waittime)
                                                    {
                                                        case 2:
                                                            this.counterTiming = false;
                                                            this.parent.attacks.Add(new MimaRockTower(this.sound, this.parent, this.position.X + this.UnionRebirth(this.union), this.position.Y, this.union, this.Power, MimaRockTower.MOTION.init));
                                                            break;
                                                        case 4:
                                                            this.parent.attacks.Add(new MimaRockTower(this.sound, this.parent, this.position.X + 2 * this.UnionRebirth(this.union), this.position.Y - 1, this.union, this.Power, MimaRockTower.MOTION.init));
                                                            this.parent.attacks.Add(new MimaRockTower(this.sound, this.parent, this.position.X + 2 * this.UnionRebirth(this.union), this.position.Y, this.union, this.Power, MimaRockTower.MOTION.init));
                                                            this.parent.attacks.Add(new MimaRockTower(this.sound, this.parent, this.position.X + 2 * this.UnionRebirth(this.union), this.position.Y + 1, this.union, this.Power, MimaRockTower.MOTION.init));
                                                            break;
                                                        case 6:
                                                            this.parent.attacks.Add(new MimaRockTower(this.sound, this.parent, this.position.X + 3 * this.UnionRebirth(this.union), this.position.Y - 1, this.union, this.Power, MimaRockTower.MOTION.init));
                                                            this.parent.attacks.Add(new MimaRockTower(this.sound, this.parent, this.position.X + 3 * this.UnionRebirth(this.union), this.position.Y, this.union, this.Power, MimaRockTower.MOTION.init));
                                                            this.parent.attacks.Add(new MimaRockTower(this.sound, this.parent, this.position.X + 3 * this.UnionRebirth(this.union), this.position.Y + 1, this.union, this.Power, MimaRockTower.MOTION.init));
                                                            this.waittime = 0;
                                                            this.motion = NaviBase.MOTION.move;
                                                            break;
                                                    }
                                                    break;
                                                }
                                                break;
                                        }
                                        break;
                                    case Mima.ATTACK.SoulFlame:
                                        if (this.position.X != 1)
                                        {
                                            this.sound.PlaySE(SoundEffect.dark);
                                            this.parent.attacks.Add(new MimaFrame(this.sound, this.parent, this.position.X, this.position.Y, this.union, this.Power, 2, new Vector2(this.positionDirect.X, this.positionDirect.Y + 16f), ChipBase.ELEMENT.heat, 2));
                                        }
                                        this.waittime = 0;
                                        this.motion = NaviBase.MOTION.move;
                                        break;
                                    case Mima.ATTACK.CrescentCharge:
                                        if (this.moveflame)
                                        {
                                            switch (this.attackProcess)
                                            {
                                                case 0:
                                                    if (this.waittime < 5)
                                                        this.animationpoint = this.AnimeMove(this.waittime);
                                                    else
                                                        this.animationpoint.X = -1;
                                                    this.nohit = true;
                                                    switch (this.waittime)
                                                    {
                                                        case 10:
                                                            this.sound.PlaySE(SoundEffect.shoot);
                                                            this.mimacharge = new MimaCharge(this.sound, this.parent, this.union == Panel.COLOR.blue ? 5 : 0, 1, this.union, this.Power, 1, this.positionDirect, this.element, 8);
                                                            this.parent.attacks.Add(mimacharge);
                                                            this.waittime = 0;
                                                            ++this.attackProcess;
                                                            break;
                                                    }
                                                    break;
                                                case 1:
                                                    this.animationpoint.X = -1;
                                                    if (!this.mimacharge.flag)
                                                    {
                                                        this.waittime = 0;
                                                        ++this.attackProcess;
                                                        break;
                                                    }
                                                    break;
                                                case 2:
                                                    this.animationpoint = this.AnimeMove(this.waittime + 5);
                                                    switch (this.waittime)
                                                    {
                                                        case 5:
                                                            this.nohit = false;
                                                            this.waittime = 0;
                                                            this.motion = NaviBase.MOTION.neutral;
                                                            this.animationpoint.X = 0;
                                                            break;
                                                    }
                                                    break;
                                            }
                                            break;
                                        }
                                        break;
                                    case Mima.ATTACK.Reincarnation:
                                        if (this.attackProcess == 1)
                                            this.counterTiming = true;
                                        switch (this.attackProcess)
                                        {
                                            case 0:
                                            case 1:
                                            case 2:
                                                this.animationpoint = this.AnimeReincarnation(this.waittime);
                                                if (this.waittime >= 6)
                                                {
                                                    ++this.attackProcess;
                                                    this.waittime = 0;
                                                    break;
                                                }
                                                break;
                                            case 3:
                                                this.counterTiming = false;
                                                this.MoveRandom(true, false);
                                                this.sound.PlaySE(SoundEffect.dark);
                                                this.parent.objects.Add(new MimaNavi(this.sound, this.parent, this.positionre.X, this.positionre.Y, this.union));
                                                this.waittime = 0;
                                                this.roopmove = -4;
                                                this.motion = NaviBase.MOTION.move;
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
                                        this.nohit = false;
                                        this.Motion = NaviBase.MOTION.neutral;
                                        this.frame = 0;
                                        this.roopneutral = 0;
                                        ++this.roopmove;
                                        break;
                                    }
                                    break;
                                case 3:
                                    this.nohit = true;
                                    break;
                                case 5:
                                    this.position = this.positionre;
                                    this.PositionDirectSet();
                                    break;
                                case 8:
                                    this.nohit = false;
                                    break;
                                case 10:
                                    this.nohit = false;
                                    this.Motion = NaviBase.MOTION.neutral;
                                    this.speed = this.nspeed;
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
                                this.nohit = false;
                                this.counterTiming = false;
                                this.effecting = false;
                                this.PositionDirectSet();
                                break;
                            case 3:
                                this.NockMotion();
                                break;
                            case 15:
                                this.animationpoint = new Point(0, 2);
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
                ShanghaiEXE.Translate("Enemy.MimaSpecial"),
                ""))
                return;
            this.animationpoint = this.AnimeReincarnation(this.waittime % 6);
            switch (this.waittime)
            {
                case 1:
                    this.sound.PlaySE(SoundEffect.thunder);
                    this.parent.effects.Add(new FlashFead(this.sound, this.parent, Color.White, 15));
                    break;
                case 15:
                    this.sound.PlaySE(SoundEffect.thunder);
                    this.parent.effects.Add(new FlashFead(this.sound, this.parent, Color.White, 15));
                    break;
                case 30:
                    this.sound.PlaySE(SoundEffect.bombbig);
                    this.sound.PlaySE(SoundEffect.damageplayer);
                    this.parent.effects.Add(new FlashFead(this.sound, this.parent, Color.White, 90));
                    this.ShakeStart(4, 60);
                    break;
                case 31:
                    this.parent.player.HPhalf();
                    break;
                case 90:
                    this.animationpoint.X = 0;
                    this.nohit = false;
                    break;
            }
            if (this.waittime > 120 && this.BlackOutEnd(this, this.parent))
            {
                this.motion = NaviBase.MOTION.neutral;
                this.animationpoint = new Point();
                this.godmode = true;
            }
            ++this.waittime;
        }

        public override void NockMotion()
        {
            this.animationpoint = new Point(5, 4);
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
            this._rect = new Rectangle(this.animationpoint.X * this.wide, 480 * (this.version != 0 && version < 5 ? 0 : 2) + this.animationpoint.Y * this.height, this.wide, this.height);
            if (this.Hp <= 0)
            {
                this.rd = this._rect;
                this.NockMotion();
                int num3 = this.animationpoint.X * this.wide;
                shake = this.Shake;
                int x1 = shake.X;
                int x2 = num3 + x1;
                int y3 = (this.version == 1 ? this.animationpoint.Y : this.animationpoint.Y + 12) * this.height;
                int wide = this.wide;
                int height1 = this.height;
                shake = this.Shake;
                int y4 = shake.Y;
                int height2 = height1 + y4;
                this._rect = new Rectangle(x2, y3, wide, height2);
                var deathAnimRect = new Rectangle(5 * this.wide, (4 + 6) * this.height, this.wide, this.height);
                this.Death(this._rect, deathAnimRect, this._position, this.picturename);
            }
            if (this.whitetime == 0)
            {
                dg.DrawImage(dg, this.picturename, this._rect, false, this._position, this.rebirth, this.color);
            }
            else
            {
                this._rect.Y = 480 + this.animationpoint.Y * this.height;
                dg.DrawImage(dg, this.picturename, this._rect, false, this._position, this.rebirth, this.color);
            }
            if (!this.nohit)
                this.HPposition = new Vector2((int)this.positionDirect.X + 8, (int)this.positionDirect.Y + this.height / 2);
            else
                this.HPposition = new Vector2(-180f, (int)this.positionDirect.Y + this.height / 2);
            this.Nameprint(dg, this.printNumber);
        }

        public override void RenderUP(IRenderer dg)
        {
            this.BlackOutRender(dg, this.union);
        }

        private Point AnimeNeutral(int waitflame)
        {
            return CharacterAnimation.ReturnKai(new int[19]
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
        1,
        100
            }, new int[18]
            {
        0,
        1,
        2,
        0,
        1,
        2,
        0,
        1,
        2,
        0,
        1,
        2,
        0,
        1,
        2,
        0,
        1,
        2
            }, 0, this.waittime);
        }

        private Point AnimeMove(int waitflame)
        {
            return this.Return(new int[10]
            {
        0,
        1,
        2,
        3,
        4,
        6,
        7,
        8,
        9,
        10
            }, new int[11] { 0, 3, 4, 5, 6, 7, 6, 5, 4, 3, 0 }, 0, waitflame);
        }

        private Point AnimeIllProminence1(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[3]
            {
        1,
        1,
        100
            }, new int[3] { 0, 1, 2 }, 1, waittime);
        }

        private Point AnimeIllProminence2(int waittime)
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
        100
            }, new int[9] { 1, 2, 3, 4, 5, 6, 1, 5, 6 }, 1, waittime);
        }

        private Point AnimeDarkWave1(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[9]
            {
        1,
        1,
        1,
        1,
        1,
        2,
        1,
        1,
        1
            }, new int[9] { 0, 3, 4, 5, 6, 7, 6, 5, 4 }, 0, waittime);
        }

        private Point AnimeDarkWave2(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[7]
            {
        1,
        1,
        1,
        1,
        1,
        1,
        100
            }, new int[7] { 0, 1, 2, 3, 4, 5, 6 }, 3, waittime);
        }

        private Point AnimeDarkWave3(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[5]
            {
        1,
        1,
        1,
        1,
        100
            }, new int[5] { 0, 1, 2, 3, 4 }, 4, waittime);
        }

        private Point AnimeGrandSpear(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[6]
            {
        1,
        1,
        1,
        1,
        1,
        100
            }, new int[6] { 0, 1, 2, 3, 4, 5 }, 2, waittime);
        }

        private Point AnimeReincarnation(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[6]
            {
        1,
        1,
        1,
        1,
        1,
        100
            }, new int[6] { 0, 1, 2, 3, 4, 5 }, 5, waittime);
        }

        private enum ATTACK
        {
            IllProminence,
            DarkWave,
            GrandSpear,
            SoulFlame,
            CrescentCharge,
            Reincarnation,
        }
    }
}

