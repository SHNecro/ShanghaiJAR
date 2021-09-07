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
using System.Drawing;
using System.Linq;

namespace NSEnemy
{
    internal class Kikuri : NaviBase
    {
        private readonly int[] pattern = new int[11]
        {
      1,
      2,
      0,
      3,
      0,
      1,
      0,
      2,
      1,
      0,
      3
        };
        private readonly int[] pattern2 = new int[11]
        {
      4,
      2,
      0,
      3,
      0,
      4,
      1,
      2,
      4,
      1,
      3
        };
        private readonly int[] powers = new int[5] { 0, 30, 0, 50, 150 };
        private readonly int[] drillY = new int[2];
        private readonly Point[,] targetRey = new Point[2, 12];
        private int action;
        private const int nspeed = 8;
        private readonly int moveroop;
        private Kikuri.ATTACK attack;
        private int roopneutral;
        private int roopmove;
        private bool UpDown;
        private int movetime;
        private readonly int movespeed;
        private int movelong;
        private readonly MetalDorill drillBreak;
        private readonly bool ballShot;
        private readonly int cancelCount;
        private readonly bool drilinvi;
        private int atackroop;
        private readonly bool atack;
        private readonly bool back;
        private readonly int attackCount;
        private Point target;
        private int attackProcess;
        private readonly int barierHP;
        private int aulaLevel;

        public Kikuri(IAudioEngine s, SceneBattle p, int pX, int pY, byte n, Panel.COLOR u, byte v)
          : base(s, p, pX, pY, n, u, v)
        {
            for (int index = 0; index < this.dropchips.Length; ++index)
                this.dropchips[index] = new ChipFolder(this.sound);
            this.element = ChipBase.ELEMENT.normal;
            this.noslip = true;
            this.superArmor = true;
            this.aulaLevel = 2;
            this.barrierEX = true;

            if (this.version > 1)
            {
                this.version = 0;
            }

            switch (this.version)
            {
                case 0:
                    this.name = ShanghaiEXE.Translate("Enemy.KikuriName1");
                    this.power = 200;
                    this.hp = 10000;
                    this.moveroop = 6;
                    this.movespeed = 2;
                    this.barierHP = 100;
                    break;
                case 1:
                    this.name = ShanghaiEXE.Translate("Enemy.KikuriName2");
                    this.power = 100;
                    this.hp = 6000;
                    this.moveroop = 6;
                    this.movespeed = 2;
                    this.barierHP = 50;
                    break;
            }
            this.barrierType = CharacterBase.BARRIER.Barrier;
            this.barierPower = this.barierHP;
            this.picturename = "kikuri";
            this.race = EnemyBase.ENEMY.navi;
            this.Flying = true;
            this.wide = 88;
            this.height = 168;
            this.hpmax = this.hp;
            this.speed = 8;
            this.hpprint = this.hp;
            this.printhp = true;
            this.effecting = false;
            this.roopmove = 0;
            this.PositionDirectSet();
            int version = this.version;
            this.dropchips[0].chip = new NSChip.Kikuri(this.sound);
            this.dropchips[0].codeNo = 0;
            this.dropchips[1].chip = new NSChip.Kikuri(this.sound);
            this.dropchips[1].codeNo = 0;
            this.dropchips[2].chip = new NSChip.Kikuri(this.sound);
            this.dropchips[2].codeNo = 0;
            this.dropchips[3].chip = new NSChip.Kikuri(this.sound);
            this.dropchips[3].codeNo = 0;
            this.dropchips[4].chip = new NSChip.Kikuri(this.sound);
            this.dropchips[4].codeNo = 0;
            this.havezenny = 9000;
        }

        public override void PositionDirectSet()
        {
            if (this.position.Y < 0)
                this.position.Y = 0;
            if (this.position.Y > 2)
                this.position.Y = 2;
            this.positionDirect = new Vector2((float)(position.X * 40.0 + 36.0), (float)(position.Y * 24.0 + 76.0));
        }

        public override void InitAfter()
        {
            this.parent.noFanfale = true;
            this.parent.mind.perfect = true;
            this.parent.player.busterPower += 5;

            this.ForcePanels();
            this.parent.effects.Add(new KikuriHalfBrokenPanels(this.sound, this.parent));
            this.animationpoint = new Point(6, 0);
        }

        private void ForcePanels()
        {
            var invisiblePanels = Enumerable.Range(4, 1).SelectMany(x => Enumerable.Range(0, 3).Select(y => new Point(x, y)));
            foreach (var panel in invisiblePanels)
            {
                this.parent.panel[panel.X, panel.Y].inviolability = true;
                this.parent.panel[panel.X, panel.Y].noRender = true;
                this.parent.panel[panel.X, panel.Y].state = Panel.PANEL._nomal;
            }
            var emptyPanels = Enumerable.Range(5, 1).SelectMany(x => Enumerable.Range(0, 3).Select(y => new Point(x, y)));
            foreach (var panel in emptyPanels)
            {
                this.parent.panel[panel.X, panel.Y].inviolability = true;
                this.parent.panel[panel.X, panel.Y].noRender = true;
                this.parent.panel[panel.X, panel.Y].state = Panel.PANEL._none;
            }
        }

        public override void DeleteBarier()
        {
            base.DeleteBarier();

            if (this.aulaLevel > 0 && (this.barierPower <= 0 || this.barrierType == CharacterBase.BARRIER.None))
            {
                --this.aulaLevel;
                if (this.aulaLevel > 0)
                {
                    this.barrierType = CharacterBase.BARRIER.Barrier;
                    this.barierPower = this.barierHP;
                }

                switch (this.aulaLevel)
                {
                    case 0:
                        this.animationpoint = this.AnimeNeutral(this.waittime % 3);
                        break;
                    case 1:
                        this.animationpoint = this.AnimeNeutralAura2(this.waittime % 3);
                        break;
                    case 2:
                        this.animationpoint = this.AnimeNeutralAura1(this.waittime % 3);
                        break;
                }
            }
        }

        protected override void Moving()
        {
            this.ForcePanels();
            this.neutlal = this.Motion == NaviBase.MOTION.neutral;
            switch (this.Motion)
            {
                case NaviBase.MOTION.neutral:
                    if (this.moveflame)
                        ++this.waittime;
                    ++this.movetime;
                    if (this.SlideMove(1f / movespeed, this.UpDown ? 2 : 3))
                    {
                        int num1 = this.UpDown ? -1 : 1;
                        Point position = this.position;
                        position.Y += num1;
                        ++this.roopmove;
                        if (!this.Canmove(position, this.number))
                            this.UpDown = !this.UpDown;
                        if (this.roopmove % 3 == 2)
                        {
                            this.powerPlus = this.powers[0] * (version == 0 ? 1 : 0);
                            this.sound.PlaySE(SoundEffect.dark);
                            int num2 = this.Random.Next(3);
                            for (int pY = 0; pY < 3; ++pY)
                            {
                                if (pY != num2)
                                {
                                    AttackBase attack = new Tower(this.sound, this.parent, this.union == Panel.COLOR.blue ? 3 : 2, pY, this.union, this.Power, 4, ChipBase.ELEMENT.poison);
                                    this.parent.effects.Add(new Soul(this.sound, this.parent, attack.position.X, attack.position.Y, 16, attack, this.version == 0 ? 30 : 60));
                                }
                            }
                        }
                    }
                    if (this.moveflame)
                    {
                        switch (this.aulaLevel)
                        {
                            case 0:
                                this.animationpoint = this.AnimeNeutral(this.waittime % 3);
                                break;
                            case 1:
                                this.animationpoint = this.AnimeNeutralAura2(this.waittime % 3);
                                break;
                            case 2:
                                this.animationpoint = this.AnimeNeutralAura1(this.waittime % 3);
                                break;
                        }
                    }
                    if (this.waittime >= 4 || this.atack)
                        this.waittime = 0;
                    if (this.parent.nowscene != SceneBattle.BATTLESCENE.end && ((this.roopmove > this.moveroop || this.HeviSand) && !this.badstatus[4]))
                    {
                        this.roopneutral = 0;
                        this.roopmove = 0;
                        if (!this.atack)
                        {
                            if (this.hp <= this.hpmax / 2)
                            {
                                this.attack = (Kikuri.ATTACK)this.pattern2[this.action];
                                this.powerPlus = this.powers[this.pattern2[this.action]];
                            }
                            else
                            {
                                this.attack = (Kikuri.ATTACK)this.pattern[this.action];
                                this.powerPlus = this.powers[this.pattern[this.action]];
                            }
                            ++this.action;
                            if (this.action >= this.pattern.Length)
                                this.action = 0;
                            this.frame = 0;
                        }
                        if ((uint)this.attack > 0U)
                        {
                            ++this.atackroop;
                            this.speed = this.version == 0 ? 3 : 4;
                            this.waittime = 0;
                            this.positionre = this.position;
                            this.PositionDirectSet();
                            this.attackProcess = 0;
                            this.barierPower = 0;
                            this.barrierType = CharacterBase.BARRIER.None;
                            this.aulaLevel = 0;
                            this.Motion = NaviBase.MOTION.attack;
                        }
                        else
                        {
                            this.powerPlus = this.powers[0] * (version == 0 ? 1 : 0);
                            this.sound.PlaySE(SoundEffect.dark);
                            int num = this.Random.Next(3);
                            for (int pY = 0; pY < 3; ++pY)
                            {
                                if (pY != num)
                                {
                                    AttackBase attack = new Tower(this.sound, this.parent, this.union == Panel.COLOR.blue ? 3 : 2, pY, this.union, this.Power, 4, ChipBase.ELEMENT.poison);
                                    this.parent.effects.Add(new Soul(this.sound, this.parent, attack.position.X, attack.position.Y, 16, attack, this.version == 0 ? 30 : 60));
                                }
                            }
                        }
                        break;
                    }
                    break;
                case NaviBase.MOTION.attack:
                    if (this.moveflame)
                    {
                        switch (this.attack)
                        {
                            case Kikuri.ATTACK.MeteorRey:
                                int num1 = 3;
                                if (this.moveflame)
                                {
                                    this.animationpoint = this.AnimeSphereFlash(this.waittime % 4);
                                    ++this.waittime;
                                }
                                switch (this.attackProcess)
                                {
                                    case 0:
                                        int num2 = 3;
                                        int num3 = 0;
                                        int y = 0;
                                        int num4 = 0;
                                        bool flag = false;
                                        for (int index = 0; index < this.targetRey.GetLength(1); ++index)
                                        {
                                            this.targetRey[0, index] = new Point(num2 + num4 * this.UnionRebirth(this.union), y);
                                            this.targetRey[1, index] = new Point(num3 - num4 * this.UnionRebirth(this.union), 2 - y);
                                            if (flag)
                                            {
                                                if (y <= 0)
                                                {
                                                    ++num4;
                                                    flag = !flag;
                                                }
                                                else
                                                    --y;
                                            }
                                            else if (y >= 2)
                                            {
                                                ++num4;
                                                flag = !flag;
                                            }
                                            else
                                                ++y;
                                        }
                                        this.parent.attacks.Add(new Dummy(this.sound, this.parent, this.targetRey[0, 0].X, this.targetRey[0, 0].Y, this.union, new Point(), 60, true));
                                        this.parent.attacks.Add(new Dummy(this.sound, this.parent, this.targetRey[1, 0].X, this.targetRey[1, 0].Y, this.union, new Point(), 60, true));
                                        ++this.attackProcess;
                                        break;
                                }
                                if (this.waittime % 4 == 2)
                                {
                                    if (this.attackProcess >= num1)
                                    {
                                        int index = this.attackProcess - num1;
                                        if (index < this.targetRey.GetLength(1))
                                        {
                                            this.sound.PlaySE(SoundEffect.beam);
                                            int s = this.version > 0 ? 2 : 3;
                                            this.parent.attacks.Add(new MeteorRay(this.sound, this.parent, this.targetRey[0, index].X, this.targetRey[0, index].Y, this.union, this.Power, s, ChipBase.ELEMENT.normal));
                                            this.parent.attacks.Add(new MeteorRay(this.sound, this.parent, this.targetRey[1, index].X, this.targetRey[1, index].Y, this.union, this.Power, s, ChipBase.ELEMENT.normal));
                                        }
                                    }
                                    ++this.attackProcess;
                                }
                                if (this.attackProcess > 15)
                                {
                                    this.AttackEnd();
                                    break;
                                }
                                break;
                            case Kikuri.ATTACK.BioWater:
                                if (this.moveflame)
                                {
                                    this.animationpoint = this.AnimeSphereFlash(this.waittime % 4);
                                    ++this.waittime;
                                }
                                if (this.attackProcess == 0) { }
                                if (this.waittime % 4 == 3)
                                {
                                    this.target = this.RandomTarget(this.union);
                                    int z = 80;
                                    this.sound.PlaySE(SoundEffect.water);
                                    Soul soul = new Soul(this.sound, this.parent, this.target.X, this.target.Y, z, null, 30);
                                    soul.attack = new ClossBomb(this.sound, this.parent, this.target.X, this.target.Y, this.union, this.Power, 2, new Vector2(soul.positionDirect.X, soul.positionDirect.Y - z), this.target, 120, ClossBomb.TYPE.single, true, ClossBomb.TYPE.single, true, true);
                                    this.parent.effects.Add(soul);
                                }
                                if (this.waittime >= 60)
                                {
                                    this.AttackEnd();
                                    break;
                                }
                                break;
                            case Kikuri.ATTACK.EyeEraser:
                                if (this.moveflame)
                                {
                                    this.animationpoint = this.AnimeEyeBeam(this.waittime);
                                    ++this.waittime;
                                }
                                switch (this.waittime)
                                {
                                    case 1:
                                        this.speed = 4;
                                        break;
                                    case 4:
                                        this.sound.PlaySE(SoundEffect.shoot);
                                        this.parent.effects.Add(new EyeBeam(this.sound, this.parent, this.position.X, this.position.Y, this.speed, this.version == 0));
                                        break;
                                    case 5:
                                        this.parent.panel[this.position.X + this.UnionRebirth(this.union), this.position.Y].State = Panel.PANEL._crack;
                                        break;
                                    case 6:
                                        this.parent.panel[this.position.X + 2 * this.UnionRebirth(this.union), this.position.Y].State = Panel.PANEL._crack;
                                        break;
                                    case 7:
                                        this.parent.panel[this.position.X + 3 * this.UnionRebirth(this.union), this.position.Y].State = Panel.PANEL._crack;
                                        break;
                                    case 8:
                                        this.parent.panel[this.position.X + 4 * this.UnionRebirth(this.union), this.position.Y].State = Panel.PANEL._crack;
                                        break;
                                    case 9:
                                        this.sound.PlaySE(SoundEffect.bombbig);
                                        this.parent.effects.Add(new ScreenFlash(this.sound, this.parent, Color.Red, 2));
                                        this.ShakeStart(4, 30 * this.speed);
                                        break;
                                    case 10:
                                    case 18:
                                    case 26:
                                        if (this.version > 0)
                                        {
                                            this.parent.attacks.Add(new Tower(this.sound, this.parent, this.position.X + this.UnionRebirth(this.union), 0, this.union, this.Power, 4, ChipBase.ELEMENT.heat));
                                            this.parent.attacks.Add(new Tower(this.sound, this.parent, this.position.X + this.UnionRebirth(this.union), 1, this.union, this.Power, 4, ChipBase.ELEMENT.heat));
                                            this.parent.attacks.Add(new Tower(this.sound, this.parent, this.position.X + this.UnionRebirth(this.union), 2, this.union, this.Power, 4, ChipBase.ELEMENT.heat));
                                            break;
                                        }
                                        this.parent.attacks.Add(new Tower(this.sound, this.parent, this.position.X + this.UnionRebirth(this.union), 0, this.union, this.Power, 4, ChipBase.ELEMENT.aqua));
                                        this.parent.attacks.Add(new Tower(this.sound, this.parent, this.position.X + this.UnionRebirth(this.union), 1, this.union, this.Power, 4, ChipBase.ELEMENT.heat));
                                        this.parent.attacks.Add(new Tower(this.sound, this.parent, this.position.X + this.UnionRebirth(this.union), 2, this.union, this.Power, 4, ChipBase.ELEMENT.leaf));
                                        break;
                                    case 60:
                                        this.AttackEnd();
                                        break;
                                }
                                break;
                            case Kikuri.ATTACK.MoonLight:
                                this.animationpoint = this.AnimeSatelliteBarn(this.waittime);
                                ++this.waittime;
                                switch (this.waittime)
                                {
                                    case 1:
                                        this.sound.PlaySE(SoundEffect.dark);
                                        break;
                                    case 5:
                                        this.parent.attacks.Add(new Dummy(this.sound, this.parent, this.position.X + this.UnionRebirth(this.union), 0, this.union, new Point(2, 2), 60, true));
                                        break;
                                    case 10:
                                        this.sound.PlaySE(SoundEffect.bombbig);
                                        this.parent.attacks.Add(new SatelliteBarn(this.sound, this.parent, this.position.X + this.UnionRebirth(this.union), 1, this.union, this.Power, 4, this.element));
                                        break;
                                    case 25:
                                        this.parent.attacks.Add(new Dummy(this.sound, this.parent, this.position.X + 3 * this.UnionRebirth(this.union), 0, this.union, new Point(2, 2), 60, true));
                                        break;
                                    case 30:
                                        this.parent.attacks.Add(new SatelliteBarn(this.sound, this.parent, this.position.X + 3 * this.UnionRebirth(this.union), 1, this.union, this.Power, 4, this.element));
                                        break;
                                    case 45:
                                        this.parent.attacks.Add(new Dummy(this.sound, this.parent, this.position.X + 2 * this.UnionRebirth(this.union), -1, this.union, new Point(2, 2), 60, true));
                                        break;
                                    case 50:
                                        this.parent.attacks.Add(new SatelliteBarn(this.sound, this.parent, this.position.X + 2 * this.UnionRebirth(this.union), 0, this.union, this.Power, 4, this.element));
                                        break;
                                    case 65:
                                        this.parent.attacks.Add(new Dummy(this.sound, this.parent, this.position.X + 2 * this.UnionRebirth(this.union), 1, this.union, new Point(2, 2), 60, true));
                                        break;
                                    case 70:
                                        this.parent.attacks.Add(new SatelliteBarn(this.sound, this.parent, this.position.X + 2 * this.UnionRebirth(this.union), 2, this.union, this.Power, 4, this.element));
                                        break;
                                    case 90:
                                        this.animationpoint.X = 12;
                                        break;
                                    case 92:
                                        this.animationpoint.X = 11;
                                        break;
                                    case 94:
                                        this.AttackEnd();
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
                    if (this.position.X != (this.union == Panel.COLOR.blue ? 4 : 1))
                    {
                        this.positionre.X = this.union == Panel.COLOR.blue ? 4 : 1;
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
                        case 2:
                            this.movelong = 24;
                            this.effecting = false;
                            this.counterTiming = false;
                            this.effecting = false;
                            this.PositionDirectSet();
                            break;
                        case 15:
                            this.PositionDirectSet();
                            break;
                        case 21:
                            this.waittime = 0;
                            if (!this.atack)
                                this.speed = 8;
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

        public override void NockMotion()
        {
        }

        public override void Render(IRenderer dg)
        {
            if (this.alfha < byte.MaxValue)
                this.color = Color.FromArgb(alfha, this.mastorcolor);
            else
                this.color = this.mastorcolor;
            this._position = new Vector2((int)this.positionDirect.X + this.Shake.X, (int)this.positionDirect.Y + this.Shake.Y);
            this._rect = new Rectangle(this.animationpoint.X * this.wide, (this.version > 0 ? 0 : 2) * this.height, this.wide, this.height);
            if (this.Hp <= 0)
            {
                this.rd = this._rect;
                this._rect = new Rectangle(this.animationpoint.X * this.wide, (this.version > 0 ? 0 : 2) * this.height, this.wide, this.height);
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
            this.HPposition = new Vector2((int)this.positionDirect.X, (int)this.positionDirect.Y + this.height / 2 - 72);
        }

        private void AttackEnd()
        {
            this.aulaLevel = 2;
            this.motion = NaviBase.MOTION.neutral;
            this.waittime = 0;
            this.frame = 0;
            this.barrierType = CharacterBase.BARRIER.Barrier;
            this.barierPower = this.barierHP;
            this.animationpoint = this.AnimeNeutralAura1(this.waittime % 3);
        }

        public override bool EnemyInitAction()
        {
            if (this.alfha < byte.MaxValue)
                ++this.alfha;
            if (this.moveflame)
            {
                this.animationpoint.X = this.AnimeInit(this.waittime).X;
                switch (this.waittime)
                {
                    case 0:
                        this.speed = 128;
                        this.sound.PlaySE(SoundEffect.dark);
                        break;
                    case 1:
                        this.speed = 16;
                        break;
                    case 4:
                        this.speed = 4;
                        this.alfha = byte.MaxValue;
                        this.sound.PlaySE(SoundEffect.flash);
                        break;
                    case 17:
                        this.namePrint = true;
                        return true;
                }
                ++this.waittime;
            }
            this.FlameControl();
            return false;
        }

        private Point AnimeInit(int waitflame)
        {
            return this.Return(new int[18]
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
        13,
        14,
        15,
        16,
        1000
            }, new int[18]
            {
        0,
        1,
        2,
        3,
        4,
        0,
        5,
        6,
        7,
        8,
        9,
        10,
        8,
        9,
        10,
        8,
        9,
        10
            }, 1, waitflame);
        }

        private Point AnimeNeutralAura1(int waitflame)
        {
            return this.Return(new int[3] { 0, 1, 2 }, new int[3]
            {
        8,
        9,
        10
            }, 1, waitflame);
        }

        private Point AnimeNeutralAura2(int waitflame)
        {
            return this.Return(new int[1], new int[1] { 7 }, 1, waitflame);
        }

        private Point AnimeNeutral(int waitflame)
        {
            return this.Return(new int[1], new int[1] { 6 }, 1, waitflame);
        }

        private Point AnimeSphereFlash(int waitflame)
        {
            return this.Return(new int[4] { 0, 1, 2, 100 }, new int[4]
            {
        6,
        18,
        19,
        18
            }, 0, waitflame);
        }

        private Point AnimeEyeBeam(int waitflame)
        {
            return this.Return(new int[5] { 0, 1, 2, 3, 1000 }, new int[5]
            {
        7,
        13,
        14,
        15,
        16
            }, 0, waitflame);
        }

        private Point AnimeSatelliteBarn(int waitflame)
        {
            return this.Return(new int[12]
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
        1000
            }, new int[12]
            {
        7,
        26,
        27,
        28,
        29,
        30,
        31,
        32,
        33,
        11,
        12,
        13
            }, 0, waitflame);
        }

        private enum ATTACK
        {
            SoulBomber,
            MeteorRey,
            BioWater,
            EyeEraser,
            MoonLight,
        }
    }
}

