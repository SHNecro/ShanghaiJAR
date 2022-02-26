using NSAttack;
using NSBattle;
using NSChip;
using NSEffect;
using NSGame;
using NSShanghaiEXE.Common;
using NSShanghaiEXE.ExtensionMethods;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace NSEnemy
{
    internal class DruidMan : NaviBase
	{
		private const int IdleFrames = 4;

		private static readonly Rectangle FullFrameRect = new Rectangle(0, 0, 104, 99);
		private static readonly Rectangle BodyRect = new Rectangle(47, 16, 48, 83);

		private static readonly Vector2 SpriteOffset = new Vector2(-BodyRect.Width / 2.0f, -8);
		private static readonly Vector2 HPOffset = new Vector2(24, BodyRect.Height / 2.0f + 8);

		private static readonly Animation[] WaveAnimation =
		{
			new Animation { Frame = 0, Delay = 4 },
			new Animation { Frame = 1, Delay = 2 },
			new Animation { Frame = 2, Delay = 4, IsCounter = true },
			new Animation { Frame = 3, Delay = 2, IsCounter = true },
			new Animation { Frame = 4, Delay = 4, IsCounter = true },
			new Animation { Frame = 5, Delay = 4, IsCounter = true },
			new Animation { Frame = 6, Delay = 2, IsCounter = true },
			new Animation { Frame = 7, Delay = 2, IsCounter = true },
			new Animation { Frame = 8, Delay = 4 },
			new Animation { Frame = 9, Delay = 2 },
			new Animation { Frame = 10, Delay = 4 },
			new Animation { Frame = 11, Delay = 4 },
		};
		private static readonly IEnumerable<Tuple<Animation, int>> WaveAnimationFrameTimings = WaveAnimation.ToFrameTimings();

        private static readonly Animation[] BurstAnimation =
        {
            new Animation { Frame = 0, Delay = 2, IsCounter = true },
            new Animation { Frame = 1, Delay = 4, IsCounter = true },
            new Animation { Frame = 2, Delay = 6, IsCounter = true },
            new Animation { Frame = 3, Delay = 3, IsCounter = true },
            new Animation { Frame = 4, Delay = 2, IsCounter = true },
            new Animation { Frame = 5, Delay = 2 },
            new Animation { Frame = 6, Delay = 3 },
            new Animation { Frame = 7, Delay = 3 },
            new Animation { Frame = 8, Delay = 3 },
            new Animation { Frame = 9, Delay = 4 }
        };
        private static readonly IEnumerable<Tuple<Animation, int>> BurstAnimationFrameTimings = BurstAnimation.ToFrameTimings();
        // var darkChips = new[] { 255, 256, 257, 258, 260, 261, 263 };
        private static readonly Tuple<int, int>[] DarkSummonChipVersions =
        {
            Tuple.Create(255, 1),
            Tuple.Create(256, 2),
            Tuple.Create(257, 3),
            Tuple.Create(258, 5),
            Tuple.Create(260, 3),
            Tuple.Create(261, 6),
            Tuple.Create(262, 3),
            Tuple.Create(263, 2),
        };

        private DruidManFlinch flinchEffect;

		private int idleDelay;
		private int idleDelayBase;
		private int idleDelayFuzz;

		private double attackChance;

		private int attackDelay;
		private int attackDelayBase;
		private int attackDelayFuzz;

		private int attackCooldown;
		private int attackCooldownBase;
		private int attackCooldownFuzz;

		private int knockbackInvisibility;
        private int maximumKnockbackTime;
        private double knockbackRetailiationChance;
        private int knockbackRetaliationBurstCount;
        private List<Tuple<Point, int, ChipBase.ELEMENT>> retaliationBurstWarnings;
        private int retaliationWaitTime;

        private int burstWeight;
        private int burstAttackInitialDelay;
        private int burstDuration;
        private int burstSpacing;
        private int burstWarning;
        private int burstPowerAdjust;
        private List<Tuple<Point, int, ChipBase.ELEMENT>> burstWarnings;
        private ChipBase.ELEMENT burstElement;

        private int waveWeight;
		private int waveAttackInitialDelay;
		private int waveSpeed;
		private int waveWarning;
		private int waveRest;
		private int wavesPerRepeat;
		private int waveRepeats;
        private int[] currentWaveRows;
        private int[] waveRows;
        private ChipBase.ELEMENT[] waveElements;
        private double waveEffectiveChance;

        private int slashWeight;
        private double slashWideChance;
        private int slashAttackInitialDelay;
        private int slashSpacing;
        private int slashWarning;
        private int slashCount;
        // Poison status does time/8 damage
        private int slashStatusTime;
        private double slashAreaGrabChance;
        private List<Tuple<Rectangle, int>> slashWarnings;

        private int darkSummonWeight;
        private int darkSummonAttackInitialDelay;
        private int darkSummonHealth;

        private bool isElemental;

        private AttackState attackMotion;
		private AttackType attackType;
        private int currentKnockbackTime;
        private int knockbackFrustration;

        public DruidMan(IAudioEngine s, SceneBattle p, int pX, int pY, byte n, Panel.COLOR u, byte v)
			: base(s, p, pX, pY, n, u, v)
		{
			for (int index = 0; index < this.dropchips.Length; ++index)
				this.dropchips[index] = new ChipFolder(this.sound);

            if (this.version == 0)
            {
                this.version = 4;
            }

			this.SetVersionStats();
			this.SetVersionDrops();
            this.SetDynamicAttackWeights();

            this.wide = BodyRect.Width;
			this.height = BodyRect.Height;
			this.race = EnemyBase.ENEMY.navi;
			this.Flying = false;
			this.hpmax = this.hp;
			this.speed = 2;
			this.hpprint = this.hp;
			this.printhp = true;
			this.effecting = false;
			
			this.animationpoint = new Point(0, 0);

            this.burstWarnings = new List<Tuple<Point, int, ChipBase.ELEMENT>>();
            this.slashWarnings = new List<Tuple<Rectangle, int>>();
            this.retaliationBurstWarnings = new List<Tuple<Point, int, ChipBase.ELEMENT>>();
        }

		private Vector2 SpritePositionDirect => this.positionDirect + SpriteOffset;
		private Vector2 HPPositionDirect => this.SpritePositionDirect + HPOffset;

		private AttackState AttackMotion
		{
			get
			{
				return this.attackMotion;
			}

			set
			{
				this.attackMotion = value;
				this.waittime = 0;
			}
		}

        public override MOTION Motion
        {
            get
            {
                return base.Motion;
            }
            set
            {
                if (this.Motion == MOTION.knockback && value == MOTION.knockback)
                {
                    this.currentKnockbackTime += this.waittime;
                }
                base.Motion = value;
            }
        }

        public override void InitAfter()
		{
			base.InitAfter();
			this.PositionDirectSet();
			this.IdleDelaySet();
		}

		public override void PositionDirectSet()
		{
			this.positionDirect = new Vector2(position.X * 40.0f + (BodyRect.Width / 2.0f), position.Y * 24.0f + (FullFrameRect.Height / 2.0f));
		}

		protected override void Moving()
		{
            this.SetDynamicAttackWeights();
            
            if (this.retaliationBurstWarnings.Any())
            {
                this.retaliationBurstWarnings.ForEach(bw => this.ProcessBurstWarnings(bw, this.retaliationWaitTime));
                this.retaliationWaitTime++;
            }

            switch (this.Motion)
			{
				case MOTION.neutral:
					this.animationpoint = new Point((this.waittime / 4) % 4, 0);

					if (this.waittime >= this.idleDelay)
					{
						this.Motion = this.Random.NextDouble() > this.attackChance ? MOTION.move : MOTION.attack;
						switch (this.Motion)
						{
							case MOTION.move:
								this.IdleDelaySet();
								break;
							case MOTION.attack:
								this.AttackMotion = AttackState.Idle;
								this.AttackDelaySet();
								break;
						}
					}
					break;
				case MOTION.move:
					this.parent.effects.Add(new StepShadow(
						this.sound,
						this.parent,
						new Rectangle(FrameCoordX(this.animationpoint.X), FrameCoordY(this.animationpoint.Y), FullFrameRect.Width, FullFrameRect.Height),
						this.SpritePositionDirect,
						this.picturename,
						this.rebirth,
						this.position));
					this.CommitMoveRandom();
					
					this.motion = MOTION.neutral;
					break;
				case MOTION.knockback:
                    if (this.currentKnockbackTime > this.maximumKnockbackTime)
                    {
                        this.invincibilitytime = this.knockbackInvisibility;
                        this.currentKnockbackTime = 0;
                    }
                    this.counterTiming = false;
                    this.burstWarnings.Clear();
                    this.slashWarnings.Clear();
                    if (this.waittime < 3)
					{
						this.rend = true;
						this.printhp = true;
						this.animationpoint = new Point(0, 4);
                        this.retaliationBurstWarnings.Clear();
                    }
					else if (this.waittime == 3)
					{
						this.animationpoint = new Point(1, 4);
					}
					else if (this.waittime == 6)
					{
						this.rend = false;
						if (this.flinchEffect == null || !this.flinchEffect.flag)
						{
							this.flinchEffect = new DruidManFlinch(
								this.sound,
								this.parent,
								this.SpritePositionDirect,
								this.rebirth,
								this.position);
							this.parent.effects.Add(this.flinchEffect);
						}
					}
					else if (this.waittime == 24)
					{
						this.printhp = false;
						this.CommitMoveRandom();

                        this.knockbackFrustration++;
                        var knockbackAggression = ((double)this.currentKnockbackTime / this.maximumKnockbackTime) * (1.0 - this.knockbackRetailiationChance);
                        var isRetaliating = this.currentKnockbackTime == 0 || Random.NextDouble() < this.knockbackRetailiationChance + knockbackAggression;
                        if (isRetaliating)
                        {
                            this.retaliationWaitTime = 0;
                            var allPanelCoords = Enumerable.Range(0, this.parent.panel.GetLength(0)).SelectMany(x => Enumerable.Range(0, this.parent.panel.GetLength(1)).Select(y => new Point(x, y)));
                            var allPanels = allPanelCoords.Select(p => Tuple.Create(p, this.parent.panel[p.X, p.Y]));
                            var potentialPanels = allPanels.Where(tup => tup.Item2.color == this.UnionEnemy && !tup.Item2.Hole)
                                .Select(tup => tup.Item1).ToList();
                            var burstCount = Math.Min(Math.Max(0, potentialPanels.Count - 1), this.knockbackRetaliationBurstCount);
                            var panels = new Point[burstCount];
                            for (var i = burstCount; i > 0; i--)
                            {
                                var selectedIndex = Random.Next(0, potentialPanels.Count);
                                panels[i - 1] = potentialPanels[selectedIndex];
                                potentialPanels.RemoveAt(selectedIndex);
                            }

                            var retaliationElement = ChipBase.ELEMENT.poison;
                            if (this.isElemental)
                            {
                                var target = this.RandomTarget();
                                var targetElem = this.parent.AllChara().Where(c => c.position == target).Select(c => c.element).DefaultIfEmpty(ChipBase.ELEMENT.normal).First();
                                var effectiveElements = GetEffectiveElements(targetElem);
                                retaliationElement = effectiveElements[this.Random.Next(effectiveElements.Length)];
                            }

                            foreach (var panel in panels)
                            {
                                var column = panel.X;
                                var row = panel.Y;
                                this.retaliationBurstWarnings.Add(Tuple.Create(new Point(column, row), this.burstWarning, retaliationElement));
                                this.parent.attacks.Add(new Dummy(
                                    this.sound,
                                    this.parent,
                                    column,
                                    row,
                                    this.union,
                                    new Point(0, 0),
                                    this.burstWarning,
                                    true));
                            }
                        }
					}
					else if (this.waittime >= this.knockbackInvisibility)
					{
						this.rend = true;
						this.printhp = true;
						this.Motion = MOTION.neutral;
                        this.currentKnockbackTime = 0;
					}
					break;
				case MOTION.attack:
					switch (this.AttackMotion)
					{
						case AttackState.Idle:
							if (this.waittime < this.attackDelay)
							{
								this.animationpoint = new Point((this.waittime / 4) % 4, 0);
							}
							else
                            {
                                this.knockbackFrustration--;
                                this.AttackMotion = AttackState.Attack;
                                var bins = new[]
                                {
                                    Tuple.Create(AttackType.Burst, this.burstWeight),
                                    Tuple.Create(AttackType.Wave, this.waveWeight),
                                    Tuple.Create(AttackType.Slash, this.slashWeight),
                                    Tuple.Create(AttackType.DarkSummon, this.darkSummonWeight)
                                };
                                var draw = this.Random.Next(bins.Sum(b => b.Item2));
                                this.attackType = bins.Select((b, i) => Tuple.Create(b.Item1, bins.Take(i + 1).Sum(bb => bb.Item2))).FirstOrDefault(b => b.Item2 > draw)?.Item1 ?? AttackType.Burst;
							}
							break;
						case AttackState.Attack:
                            switch (this.attackType)
                            {
                                case AttackType.Burst:
                                    var burstCurrentFrame = BurstAnimationFrameTimings.FirstOrDefault(t => t.Item2 > this.waittime)?.Item1 ?? BurstAnimation.Last();
                                    this.animationpoint = new Point(burstCurrentFrame.Frame, 2);

                                    this.counterTiming = burstCurrentFrame.IsCounter;
                                    if (this.waittime > this.burstAttackInitialDelay)
                                    {
                                        var burstAttackTime = this.waittime - this.burstAttackInitialDelay;
                                        this.burstWarnings.ForEach(bw => this.ProcessBurstWarnings(bw, burstAttackTime));
                                        if (burstAttackTime < this.burstDuration)
                                        {
                                            if (burstAttackTime % this.burstSpacing == 0)
                                            {
                                                var columnProgress = (burstAttackTime / this.burstSpacing) % 6;
                                                var column = this.rebirth ? columnProgress : (5 - columnProgress);
                                                var validRows = Enumerable.Range(0, 3).Where(r =>
                                                    !this.parent.panel[column, r].Hole).ToArray();
                                                var row = validRows.Length > 0 ? validRows[this.Random.Next(0, validRows.Length)] : -1;

                                                if (this.isElemental && columnProgress == 0)
                                                {
                                                    var effectiveElements = GetEffectiveElements(this.burstElement);
                                                    this.burstElement = effectiveElements[this.Random.Next(effectiveElements.Length)];
                                                }

                                                if (row != -1)
                                                {
                                                    this.burstWarnings.Add(Tuple.Create(new Point(column, row), burstAttackTime + this.burstWarning, this.burstElement));
                                                    this.parent.attacks.Add(new Dummy(
                                                        this.sound,
                                                        this.parent,
                                                        column,
                                                        row,
                                                        this.union,
                                                        new Point(0, 0),
                                                        this.burstWarning,
                                                        true));
                                                }
                                            }
                                        }
                                        else if (this.burstWarnings.All(bw => burstAttackTime > bw.Item2))
                                        {
                                            if (this.isElemental)
                                            {
                                                this.parent.effects.Add(new FlashFead(this.sound, this.parent, Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF), 15));
                                                this.sound.PlaySE(SoundEffect.eriasteal2);
                                                for (int pX = 0; pX < this.parent.panel.GetLength(0); ++pX)
                                                {
                                                    for (int pY = 0; pY < this.parent.panel.GetLength(1); ++pY)
                                                    {
                                                        if (this.parent.panel[pX, pY].State != Panel.PANEL._none && this.parent.panel[pX, pY].State != Panel.PANEL._un)
                                                        {
                                                            this.parent.panel[pX, pY].state = Panel.PANEL._nomal;
                                                            this.parent.effects.Add(new Smoke(this.sound, this.parent, pX, pY, this.element));
                                                        }
                                                    }
                                                }
                                            }

                                            this.burstWarnings.Clear();
                                            this.AttackMotion = AttackState.Cooldown;
                                            this.AttackCooldownSet();
                                        }
                                    }
                                    break;
                                case AttackType.Wave:
                                    var waveCurrentFrame = WaveAnimationFrameTimings.FirstOrDefault(t => t.Item2 > this.waittime)?.Item1 ?? WaveAnimation.Last();
                                    this.animationpoint = new Point(waveCurrentFrame.Frame, 6);

                                    this.counterTiming = waveCurrentFrame.IsCounter;
                                    if (this.waittime > this.waveAttackInitialDelay)
                                    {
                                        var waveSpacing = this.waveSpeed * 2;
                                        var waveLength = (waveSpacing * this.wavesPerRepeat) + (this.waveRest * this.waveRepeats);
                                        var waveAttackTime = this.waittime - this.waveAttackInitialDelay;
                                        if (waveAttackTime < waveLength * this.waveRepeats)
                                        {
                                            var waveTime = waveAttackTime % waveLength;
                                            if (waveTime < this.waveWarning)
                                            {
                                                if (this.waveRows == null)
                                                {
                                                    var row1 = this.Random.Next(3);
                                                    var row2Proposed = this.Random.Next(3);
                                                    var row2 = row2Proposed != row1 ? row2Proposed : ((row1 + 1) % 3);
                                                    this.waveRows = new[] { row1, row2 };
                                                    for (var i = 0; i < 2; i++)
                                                    {
                                                        var y = this.waveRows[i];
                                                        var cols = Enumerable.Range(0, 6);
                                                        if (!this.rebirth)
                                                        {
                                                            cols = cols.Reverse();
                                                        }
                                                        foreach (var c in cols)
                                                        {
                                                            if (this.parent.panel[c, y].Hole)
                                                            {
                                                                break;
                                                            }
                                                            this.parent.attacks.Add(new Dummy(
                                                                this.sound,
                                                                this.parent,
                                                                c,
                                                                y,
                                                                this.union,
                                                                new Point(0, 0),
                                                                this.waveWarning,
                                                                true));
                                                        }

                                                        if (this.isElemental)
                                                        {
                                                            var isEffective = this.Random.NextDouble() < this.waveEffectiveChance;

                                                            if (isEffective)
                                                            {
                                                                var target = this.RandomTarget();
                                                                var targetElem = this.parent.AllChara().Where(c => c.position == target).Select(c => c.element).DefaultIfEmpty(ChipBase.ELEMENT.normal).First();
                                                                var effectiveElements = GetEffectiveElements(targetElem);

                                                                this.waveElements[0] = effectiveElements[this.Random.Next(effectiveElements.Length)];
                                                                this.waveElements[1] = effectiveElements[this.Random.Next(effectiveElements.Length)];
                                                            }
                                                            else
                                                            {
                                                                var elements = GetEffectiveElements(ChipBase.ELEMENT.normal);

                                                                this.waveElements[0] = elements[this.Random.Next(elements.Length)];
                                                                this.waveElements[1] = elements[this.Random.Next(elements.Length)];
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            else if (waveTime < this.waveWarning + (waveSpacing * this.wavesPerRepeat))
                                            {
                                                if (waveTime == this.waveWarning)
                                                {
                                                    this.sound.PlaySE(SoundEffect.bombmiddle);
                                                    this.ShakeStart(4, 8);
                                                    this.currentWaveRows = this.waveRows;
                                                    this.waveRows = null;
                                                }
                                                for (var i = 0; i < 2; i++)
                                                {
                                                    var y = this.currentWaveRows[i];
                                                    if (waveTime % waveSpacing == 0)
                                                    {
                                                        var wavePower = this.waveElements[i] == ChipBase.ELEMENT.leaf ? (this.Power / this.wavesPerRepeat) : this.Power;
                                                        this.parent.attacks.Add(new DruidManWave(
                                                            this.sound,
                                                            this.parent,
                                                            this.rebirth ? 0 : 5,
                                                            y,
                                                            this.union,
                                                            wavePower,
                                                            this.waveSpeed,
                                                            this.waveElements[i]));
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                this.waveRows = null;
                                            }
                                        }
                                        else
                                        {
                                            this.AttackMotion = AttackState.Cooldown;
                                            this.AttackCooldownSet();
                                        }
                                    }
                                    break;
                                case AttackType.Slash:
                                    var slashCurrentFrame = default(Animation);
                                    var animationRow = 0;
                                    if (this.waittime < BurstAnimation.Sum(a => a.Delay))
                                    {
                                        slashCurrentFrame = BurstAnimationFrameTimings.FirstOrDefault(t => t.Item2 > this.waittime)?.Item1 ?? BurstAnimation.Last();
                                        animationRow = 2;
                                        if (slashCurrentFrame.Frame == 5)
                                        {
                                            this.sound.PlaySE(SoundEffect.dark);
                                        }
                                    }
                                    else
                                    {
                                        var slashAnimWaitTime = this.waittime - BurstAnimationFrameTimings.Last().Item2;
                                        slashCurrentFrame = WaveAnimationFrameTimings.FirstOrDefault(t => t.Item2 > slashAnimWaitTime)?.Item1 ?? WaveAnimation.Last();
                                        animationRow = 6;
                                    }
                                    this.animationpoint = new Point(slashCurrentFrame.Frame, animationRow);

                                    this.counterTiming = slashCurrentFrame.IsCounter;
                                    if (this.waittime > this.slashAttackInitialDelay)
                                    {
                                        var slashAttackTime = this.waittime - this.slashAttackInitialDelay;
                                        this.slashWarnings.ForEach(bw =>
                                        {
                                            if (bw.Item2 == slashAttackTime)
                                            {
                                                var slashElement = ChipBase.ELEMENT.poison;

                                                if (this.isElemental)
                                                {
                                                    var allElements = GetEffectiveElements(ChipBase.ELEMENT.normal);
                                                    slashElement = allElements[this.Random.Next(allElements.Length)];
                                                }

                                                this.sound.PlaySE(SoundEffect.breakObject);
                                                var isSlashSquare = (bw.Item1.Width == 1 && bw.Item1.Height == 1);
                                                var newSlash = isSlashSquare
                                                ? new Halberd(
                                                    this.sound,
                                                    this.parent,
                                                    bw.Item1.X,
                                                    bw.Item1.Y == 0 ? 0 : 2,
                                                    this.union,
                                                    this.power,
                                                    1,
                                                    slashElement,
                                                    false)
                                                : new Halberd(
                                                    this.sound,
                                                    this.parent,
                                                    bw.Item1.X,
                                                    1,
                                                    this.union,
                                                    this.power,
                                                    1,
                                                    slashElement,
                                                    false);

                                                switch (slashElement)
                                                {
                                                    case ChipBase.ELEMENT.heat:
                                                        newSlash.BadStatusSet(BADSTATUS.melt, this.slashStatusTime);
                                                        break;
                                                    case ChipBase.ELEMENT.aqua:
                                                        newSlash.BadStatusSet(BADSTATUS.slip, this.slashStatusTime);
                                                        break;
                                                    case ChipBase.ELEMENT.eleki:
                                                        newSlash.BadStatusSet(BADSTATUS.paralyze, 60);
                                                        break;
                                                    case ChipBase.ELEMENT.leaf:
                                                        newSlash.BadStatusSet(BADSTATUS.blind, this.slashStatusTime);
                                                        break;
                                                    case ChipBase.ELEMENT.poison:
                                                        newSlash.BadStatusSet(BADSTATUS.poison, this.slashStatusTime);
                                                        break;
                                                    case ChipBase.ELEMENT.earth:
                                                        newSlash.BadStatusSet(BADSTATUS.heavy, this.slashStatusTime);
                                                        break;
                                                }
                                                this.parent.attacks.Add(newSlash);
                                            }
                                        });
                                        if (slashAttackTime < this.slashCount * (this.slashWarning + this.slashSpacing))
                                        {
                                            if (slashAttackTime % (this.slashSpacing + this.slashWarning) == 0)
                                            {
                                                var targetPos = this.RandomTarget();
                                                var isSlashSquare = this.Random.NextDouble() > this.slashWideChance;
                                                if (isSlashSquare)
                                                {
                                                    var xOffset = this.Random.Next(2);
                                                    if (targetPos.X + 1 > 5 || this.parent.panel[targetPos.X + 1, targetPos.Y].color == this.union)
                                                    {
                                                        xOffset = 0;
                                                    }
                                                    else if (targetPos.X == 0)
                                                    {
                                                        xOffset = 1;
                                                    }

                                                    var yOffset = this.Random.Next(2) - 1;
                                                    if (targetPos.Y + 1 > 2 || this.parent.panel[targetPos.X, targetPos.Y + 1].color == this.union)
                                                    {
                                                        yOffset = -1;
                                                    }
                                                    else if (targetPos.Y - 1 < 0 || this.parent.panel[targetPos.X, targetPos.Y - 1].color == this.union)
                                                    {
                                                        yOffset = 0;
                                                    }
                                                    targetPos.Offset(xOffset, yOffset);

                                                    this.slashWarnings.Add(Tuple.Create(new Rectangle(targetPos, new Size(1, 1)), slashAttackTime + this.slashWarning));
                                                    this.parent.attacks.Add(new Dummy(
                                                        this.sound,
                                                        this.parent,
                                                        targetPos.X,
                                                        targetPos.Y,
                                                        this.union,
                                                        new Point(1, 1),
                                                        this.slashWarning,
                                                        true));
                                                }
                                                else
                                                {
                                                    this.slashWarnings.Add(Tuple.Create(new Rectangle(new Point(targetPos.X, 0), new Size(0, 2)), slashAttackTime + this.slashWarning));
                                                    this.parent.attacks.Add(new Dummy(
                                                        this.sound,
                                                        this.parent,
                                                        targetPos.X,
                                                        0,
                                                        this.union,
                                                        new Point(1, 2),
                                                        this.slashWarning,
                                                        true));
                                                }
                                            }
                                        }
                                        else if (this.slashWarnings.All(sw => slashAttackTime > sw.Item2))
                                        {
                                            int pX = Eriabash.SteelX(this, this.parent);
                                            if (pX != 99 && (pX > 2 || (pX == 2 && this.Random.NextDouble() < this.slashAreaGrabChance)))
                                            {
                                                for (int pY = 0; pY < this.parent.panel.GetLength(1); ++pY)
                                                    this.parent.attacks.Add(new EriaSteel(this.sound, this.parent, pX, pY, this.union, 10, ChipBase.ELEMENT.normal));
                                            }

                                            this.slashWarnings.Clear();
                                            this.AttackMotion = AttackState.Cooldown;
                                            this.AttackCooldownSet();
                                        }
                                    }
                                    break;
                                case AttackType.DarkSummon:
                                    var darkSummonCurrentFrame = BurstAnimationFrameTimings.FirstOrDefault(t => t.Item2 > this.waittime)?.Item1 ?? BurstAnimation.Last();
                                    this.animationpoint = new Point(darkSummonCurrentFrame.Frame, 2);

                                    this.counterTiming = darkSummonCurrentFrame.IsCounter;
                                    if (this.waittime >= this.darkSummonAttackInitialDelay)
                                    {
                                        var darkChips = DarkSummonChipVersions.Where(cv => cv.Item2 <= this.version).Select(cv => cv.Item1).ToArray();
                                        var spawnPanels = this.RandomMultiPanel(1, this.union, false);
                                        var spawnPanel = spawnPanels.Length == 0 ? null : new Point?(spawnPanels.First());
                                        if (spawnPanel != null)
                                        {
                                            this.sound.PlaySE(SoundEffect.dark);
                                            this.parent.effects.Add(new FlashFead(this.sound, this.parent, Color.FromArgb(0x80, 0x60, 0x40, 0x60), 15));
                                            var chip1 = darkChips[this.Random.Next(darkChips.Length)];
                                            var chip2 = darkChips[this.Random.Next(darkChips.Length)];
                                            var chip3 = darkChips[this.Random.Next(darkChips.Length)];
                                            var teacherName = ShanghaiEXE.Translate("Enemy.DruidManDarkSummonName");
                                            var darkTeacher = new NormalNavi(this.sound, this.parent, spawnPanel.Value.X, spawnPanel.Value.Y, 42, this.union, 11, this.darkSummonHealth, chip1, chip2, chip3, teacherName);
                                            this.parent.enemys.Add(darkTeacher);
                                            darkTeacher.Init();
                                            darkTeacher.InitAfter();
                                        }
                                        this.AttackMotion = AttackState.Cooldown;
                                        this.AttackCooldownSet();
                                    }
                                    break;
                            }
							break;
						case AttackState.Cooldown:
                            this.knockbackFrustration = 0;
                            if (this.waittime >= this.attackCooldown)
							{
								this.Motion = MOTION.move;
							}
							break;
					}
					break;
			}

			this.waittime++;
			this.MoveAftar();
		}

		public override void Updata()
		{
			base.Updata();

            this.FlameControl(1);
		}
		
		public override void Render(IRenderer dg)
		{
			this.SetOverlayColor();

            if (this.hpprint <= 0)
            {
                this.printhp = false;
            }

            if (this.Hp <= 0)
            {
                var flinchRect = new Rectangle(FrameCoordX(0), FrameCoordY(4), FullFrameRect.Width, FullFrameRect.Height);
                var flinchWhiteRect = new Rectangle(FrameCoordX(0), FrameCoordY(5), FullFrameRect.Width, FullFrameRect.Height);
                this.Death(flinchRect, flinchWhiteRect, this.SpritePositionDirect, this.picturename);
                return;
            }

            var spriteOffsetPosition = this.SpritePositionDirect + new Vector2(this.Shake.X, this.Shake.Y);
			this.HPposition = this.HPPositionDirect;

            var hitmarkedAnimationPoint = this.whitetime == 0 ? this.animationpoint : this.animationpoint.WithOffset(0, 1);
            dg.DrawImage(dg, this.picturename, new Rectangle(FrameCoordX(hitmarkedAnimationPoint.X), FrameCoordY(hitmarkedAnimationPoint.Y), FullFrameRect.Width, FullFrameRect.Height), false, spriteOffsetPosition, this.color);
			this.Nameprint(dg, this.printNumber);
		}

        private void ProcessBurstWarnings(Tuple<Point, int, ChipBase.ELEMENT> bw, int burstAttackTime)
        {
            if (bw.Item2 == burstAttackTime)
            {
                if (this.isElemental)
                {
                    var attack = (AttackBase)new Tower(
                        this.sound,
                        this.parent,
                        bw.Item1.X,
                        bw.Item1.Y,
                        this.union,
                        this.power + this.burstPowerAdjust,
                        -1,
                        bw.Item3);
                    var panelType = Panel.PANEL._nomal;
                    switch (bw.Item3)
                    {
                        case ChipBase.ELEMENT.heat:
                            panelType = Panel.PANEL._burner;
                            break;
                        case ChipBase.ELEMENT.aqua:
                            panelType = Panel.PANEL._ice;
                            break;
                        case ChipBase.ELEMENT.eleki:
                            panelType = Panel.PANEL._thunder;
                            attack = new CrackThunder(
                                this.sound,
                                this.parent,
                                bw.Item1.X,
                                bw.Item1.Y,
                                this.union,
                                this.power + this.burstPowerAdjust,
                                false);
                            break;
                        case ChipBase.ELEMENT.leaf:
                            panelType = Panel.PANEL._grass;
                            break;
                        case ChipBase.ELEMENT.poison:
                            panelType = Panel.PANEL._poison;
                            break;
                        case ChipBase.ELEMENT.earth:
                            // panelType = Panel.PANEL._sand;
                            // panelType = Panel.PANEL._crack;
                            panelType = Panel.PANEL._nomal;
                            attack = new SandHoleAttack(
                                this.sound,
                                this.parent,
                                bw.Item1.X,
                                bw.Item1.Y,
                                this.union,
                                this.power + this.burstPowerAdjust,
                                4,
                                0,
                                SandHoleAttack.MOTION.set,
                                ChipBase.ELEMENT.earth);
                            break;
                    }

                    this.parent.attacks.Add(attack);
                    this.parent.panel[bw.Item1.X, bw.Item1.Y].state = panelType;
                }
                else
                {
                    this.parent.attacks.Add(new Tower(
                        this.sound,
                        this.parent,
                        bw.Item1.X,
                        bw.Item1.Y,
                        this.union,
                        this.power + this.burstPowerAdjust,
                        -1,
                        bw.Item3));
                }
            }
        }

        private void SetDefaultVersionStats()
        {
            this.name = ShanghaiEXE.Translate("Enemy.DruidManName");
            this.power = 200;
            this.hp = 2600;
            this.picturename = "druidman";
            this.element = ChipBase.ELEMENT.poison;

            this.idleDelayBase = 30;
            this.idleDelayFuzz = 0;

            this.attackChance = 0.5;

            this.attackDelayBase = 8;
            this.attackDelayFuzz = 0;
            this.attackCooldownBase = 8;
            this.attackCooldownFuzz = 0;

            this.burstWeight = 5;
            this.burstAttackInitialDelay = 24;
            this.burstSpacing = 4;
            this.burstDuration = this.burstSpacing * 6 * 8;
            this.burstWarning = 40;
            this.burstPowerAdjust = -50;
            this.burstElement = ChipBase.ELEMENT.poison;

            this.waveWeight = 4;
            this.waveAttackInitialDelay = 24;
            this.waveSpeed = 4;
            this.waveWarning = 16;
            this.waveRest = 8;
            this.wavesPerRepeat = 3;
            this.waveRepeats = 4;
            this.waveElements = new[] { ChipBase.ELEMENT.poison, ChipBase.ELEMENT.poison };

            this.slashWeight = 0;
            this.slashWideChance = 0.25;
            this.slashAttackInitialDelay = 24;
            this.slashSpacing = 32;
            this.slashWarning = 32;
            this.slashCount = 4;
            this.slashStatusTime = 60 * 8;
            this.slashAreaGrabChance = 0.3;

            this.darkSummonWeight = 0;
            this.darkSummonAttackInitialDelay = 18;
            this.darkSummonHealth = 200;

            this.knockbackInvisibility = 120;
            this.maximumKnockbackTime = 150;
            this.knockbackRetaliationBurstCount = 4;
            this.knockbackRetailiationChance = 0.6;

            this.isElemental = false;
        }

		private void SetVersionStats()
		{
            this.SetDefaultVersionStats();

			switch (this.version)
            {
                case 1:
                    this.name = ShanghaiEXE.Translate("Enemy.DruidManName");
                    this.power = 200;
                    this.hp = 2600;
                    break;
                case 2:
                    this.name = ShanghaiEXE.Translate("Enemy.DruidManNameV2");
                    this.power = 220;
                    this.hp = 3000;
                    break;
                case 3:
                    this.name = ShanghaiEXE.Translate("Enemy.DruidManNameV3");
                    this.power = 240;
                    this.hp = 3400;
                    break;
                case 4:
                    this.name = ShanghaiEXE.Translate("Enemy.DruidManNameSP");
                    this.power = 250;
                    this.hp = 3600;
                    break;
            }

            if (this.version >= 2)
            {
                this.idleDelayBase = 28;
                this.attackChance = 0.55;
                this.attackDelayBase = 7;
                this.attackCooldownBase = 6;

                this.burstAttackInitialDelay = 20;
                this.burstSpacing = 3;
                this.burstWarning = 36;
                this.waveAttackInitialDelay = 20;
                this.waveSpeed = 3;
                this.wavesPerRepeat = 4;
                this.slashAttackInitialDelay = 20;
                this.slashWarning = 30;
                this.slashSpacing = 26;

                this.darkSummonHealth = 100;

                this.knockbackInvisibility = 90;
                this.maximumKnockbackTime = 120;
                this.knockbackRetailiationChance = 0.65;
            }

            if (this.version >= 3)
            {
                this.idleDelayBase = 26;
                this.attackChance = 0.6;
                this.attackCooldownBase = 8;
                this.attackDelayBase = 5;

                this.burstAttackInitialDelay = 16;
                this.burstWarning = 32;
                this.waveAttackInitialDelay = 16;
                this.wavesPerRepeat = 6;
                this.slashAttackInitialDelay = 16;
                this.slashWarning = 28;
                this.slashSpacing = 20;

                this.darkSummonHealth = 200;

                this.knockbackInvisibility = 60;
                this.maximumKnockbackTime = 90;
                this.knockbackRetaliationBurstCount = 5;
                this.knockbackRetailiationChance = 0.7;
            }

            if (this.version >= 4)
            {
                this.picturename = "druidmanSP";
                this.element = ChipBase.ELEMENT.normal;
                this.isElemental = true;

                this.burstSpacing = 5;

                this.waveEffectiveChance = 0.3;
                this.maximumKnockbackTime = 60;
                this.knockbackRetailiationChance = 0.75;
            }

            if (this.version >= 5)
            {
                var plusVersions = this.version - 4;
                this.name = ShanghaiEXE.Translate("Enemy.DruidManNameSPPlus") + (plusVersions + 1);
                this.power = 250 + 10 * plusVersions;
                this.hp = 3600 + 200 * plusVersions;

                // SP2 : 5
                // SP3 : 4
                // SP4 : 4
                // SP5 : 3
                this.burstSpacing = Math.Max(3, 5 - plusVersions / 2);

                // SP2 : 7
                // SP3 : 6
                // SP4 : 5
                // SP5 : 4
                this.waveRest = Math.Max(4, 8 - plusVersions);

                // SP2 : 28/22
                // SP3 : 26/20
                // SP4 : 24/18
                // SP5 : 22/16
                this.slashWarning = Math.Max(20, 28 - plusVersions * 2);
                this.slashSpacing = Math.Max(16, 24 - plusVersions * 2);

                // SP2 : 0.40
                // SP3 : 0.50
                // SP4 : 0.60
                // SP5 : 0.70
                // SP+ : 0.75
                this.waveEffectiveChance = Math.Min(0.75, 0.3 + plusVersions * 0.10);

                this.knockbackInvisibility = Math.Max(45, 60 - plusVersions * 5);
                this.knockbackRetailiationChance = Math.Max(0.9, 0.75 + plusVersions * 0.05);
            }
        }

        private void SetDefaultVersionDrops()
        {
            this.dropchips[0].chip = new DruidMnV1(this.sound);
            this.dropchips[0].codeNo = 1;
            this.dropchips[1].chip = new DruidMnV2(this.sound);
            this.dropchips[1].codeNo = 1;
            this.dropchips[2].chip = new DruidMnV2(this.sound);
            this.dropchips[2].codeNo = 1;
            this.dropchips[3].chip = new DruidMnV1(this.sound);
            this.dropchips[3].codeNo = 2;
            this.dropchips[4].chip = new DruidMnV3(this.sound);
            this.dropchips[4].codeNo = 1;
            this.havezenny = 2600 * this.version;
            if (this.version >= 8)
            {
                this.dropchips[4].chip = new DruidMnX(this.sound);
                this.dropchips[4].codeNo = this.Random.Next(4);
                this.havezenny = 20000;
            }
        }

		private void SetVersionDrops()
		{
            this.SetDefaultVersionDrops();

            switch (this.version)
            {
                case 1:
                    this.dropchips[0].chip = new DruidMnV1(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new DruidMnV1(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new DruidMnV1(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new DruidMnV1(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new DruidMnV1(this.sound);
                    this.dropchips[4].codeNo = 0;
                    this.havezenny = 0;
                    break;
                case 2:
                    this.dropchips[0].chip = new DruidMnV2(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new DruidMnV2(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new DruidMnV2(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new DruidMnV2(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new DruidMnV2(this.sound);
                    this.dropchips[4].codeNo = 0;
                    this.havezenny = 0;
                    break;
                case 3:
                    this.dropchips[0].chip = new DruidMnV1(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new DruidMnV1(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new DruidMnV2(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new DruidMnV2(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new DruidMnV3(this.sound);
                    this.dropchips[4].codeNo = 0;
                    this.havezenny = 2600;
                    break;
                default:
                    break;
            }
		}

        private void SetDefaultDynamicAttackWeights()
        {
            this.slashWeight = (this.Hp > this.HpMax / 2) ? 0 : 3;
        }

        private void SetDynamicAttackWeights()
        {
            this.SetDefaultDynamicAttackWeights();

            if (this.version >= 2)
            {
                this.slashWeight = (this.Hp > this.HpMax / 2) ? 1 : 3;
                this.darkSummonWeight = (this.Hp > this.HpMax / 2) ? 0 : 3;
            }

            if (this.version >= 3)
            {
                this.darkSummonWeight = (this.Hp > this.HpMax / 2) ? 1 : 3;
            }

            if (this.parent?.enemys == null || this.parent.enemys.Count >= 2)
            {
                this.darkSummonWeight = 0;
            }
        }

		private void CommitMoveRandom()
		{
			this.MoveRandom(false, false);
			this.position = this.positionre;
			this.PositionDirectSet();
		}

		private void IdleDelaySet()
		{
			this.idleDelay = this.idleDelayBase + this.Random.Next(-this.idleDelayFuzz, this.idleDelayFuzz);
            var knockbackFrustrationStages = 4;
            var adjustedKnockbackFrustration = knockbackFrustrationStages - (double)Math.Max(0, Math.Min(knockbackFrustrationStages, this.knockbackFrustration));
            this.idleDelay = (int)Math.Round(idleDelay * 0.3 + idleDelay * 0.7 * (adjustedKnockbackFrustration / knockbackFrustrationStages));
		}

		private void AttackDelaySet()
		{
			this.attackDelay = this.attackDelayBase + this.Random.Next(-this.attackDelayFuzz, this.attackDelayFuzz);
		}

		private void AttackCooldownSet()
		{
			this.attackCooldown = this.attackCooldownBase + this.Random.Next(-this.attackCooldownFuzz, this.attackCooldownFuzz);
		}

		private void SetOverlayColor()
		{
			if (this.alfha < byte.MaxValue)
			{
				this.color = Color.FromArgb(alfha, this.mastorcolor);
			}
			else
			{
				this.color = this.mastorcolor;
			}
		}

		private static int FrameCoordX(int frameNumber)
		{
			return FullFrameRect.Width * frameNumber;
		}
		private static int FrameCoordY(int frameNumber)
		{
			return FullFrameRect.Height * frameNumber;
		}

        private static ChipBase.ELEMENT[] GetEffectiveElements(ChipBase.ELEMENT elem)
        {
            switch (elem)
            {
                case ChipBase.ELEMENT.heat:
                    return new[] { ChipBase.ELEMENT.aqua, ChipBase.ELEMENT.earth };
                case ChipBase.ELEMENT.aqua:
                    return new[] { ChipBase.ELEMENT.eleki, ChipBase.ELEMENT.poison };
                case ChipBase.ELEMENT.eleki:
                    return new[] { ChipBase.ELEMENT.leaf, ChipBase.ELEMENT.earth };
                case ChipBase.ELEMENT.leaf:
                    return new[] { ChipBase.ELEMENT.heat, ChipBase.ELEMENT.poison };
                case ChipBase.ELEMENT.poison:
                    return new[] { ChipBase.ELEMENT.heat, ChipBase.ELEMENT.eleki };
                case ChipBase.ELEMENT.earth:
                    return new[] { ChipBase.ELEMENT.aqua, ChipBase.ELEMENT.leaf };
                default:
                    return Enum.GetValues(typeof(ChipBase.ELEMENT)).Cast<ChipBase.ELEMENT>().Except(new[] { ChipBase.ELEMENT.normal }).ToArray();
            }
        }

		private enum AttackState
		{
			Idle,
			Attack,
			Cooldown
		}

		private enum AttackType
		{
			Burst,
			Wave,
			Slash,
            DarkSummon
		}
	}
}
