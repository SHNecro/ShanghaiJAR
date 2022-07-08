using NSAttack;
using NSBattle;
using NSChip;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.Common;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;
using Common.Vectors;
using System.Drawing;
using System.Linq;
using System.Collections.Generic;
using NSEffect;
using System.Collections.Concurrent;
using System;
using NSObject;
using NSBattle.Character;

namespace NSEnemy
{
    internal class HeavenBarrier : EnemyBase
    {
        private static readonly Rectangle SparkleTextureRect = new Rectangle(450, 780, 5, 5);
        private static readonly Rectangle DamageBlobTextureRect = new Rectangle(200, 160, 9, 9);

        private readonly IDictionary<int, Sparkle> sparkles = new ConcurrentDictionary<int, Sparkle>();

        private HeavenBarrier controller;
        private List<HeavenBarrier> controlledBarriers;
        private List<Tuple<HeavenBarrier, ChipBase.ELEMENT, int>> unprocessedAttacks;
        private int totalHp;
        private Dictionary<ChipBase.ELEMENT, int> damageBuildup;
        private int rawDamageTaken;
        private int rawDamageTakenSinceLastUpdate;
        private int remainingRetaliation;
        private bool isPerfectKill;
        private Color textColor;
        private BarrierInfoPanel infoPanel;
        private ChipBase blackoutAdaptChip;
        // Inherent problem w/ timestop counter, ongoing effects continue (ex. masterspark)
        private int? preAdaptWaitTime;

        private MOTION state;
        private Color overlayColor;
        private int lastDamage;
        private int deathOrder;
        private bool blackoutSimulhitPossible;
        private bool blackoutBuildupInterrupted;
        private int blackoutWaittime;

        private Action retaliateTickAction;

        public HeavenBarrier(IAudioEngine s, SceneBattle p, int pX, int pY, byte n, Panel.COLOR u, byte v)
          : base(s, p, pX, pY, n, u, v)
        {
            for (int index = 0; index < this.dropchips.Length; ++index)
                this.dropchips[index] = new ChipFolder(this.sound);
            this.picturename = "heavenbarrier";
            this.race = EnemyBase.ENEMY.virus;
            this.element = ChipBase.ELEMENT.normal;
            this.Flying = true;
            this.power = 0;
            this.wide = 32;
            this.height = 80;
            this.version = v;
            this.frame = 0;
            this.speed = 7 - version;
            this.printhp = false;
            this.printNumber = false;
            this.effecting = false;
            this.noslip = true;
            if (this.parent != null)
                this.roop = (byte)(parent.manyenemys - (uint)this.number);
            this.PositionDirectSet();

            this.name = ShanghaiEXE.Translate("Enemy.HeavenBarrierName");
            if (this.version < 1)
            {
                this.version = 1;
            }
            this.hp = 800 + (200 * (this.version - 1));

            this.animationpoint = new Point(1, 0);
            this.printNumber = false;

            // No chip or zenny reward
            this.dropchips[0].chip = new Reygun(this.sound);
            this.dropchips[0].codeNo = 0;
            this.dropchips[1].chip = new Reygun(this.sound);
            this.dropchips[1].codeNo = 1;
            this.dropchips[2].chip = new Reygun(this.sound);
            this.dropchips[2].codeNo = 2;
            this.dropchips[3].chip = new Reygun(this.sound);
            this.dropchips[3].codeNo = 2;
            this.dropchips[4].chip = new Reygun(this.sound);
            this.dropchips[4].codeNo = 3;

            this.hpmax = Constants.ArbitraryLargeValue;
            this.hpprint = Constants.ArbitraryLargeValue;
            this.neutlal = true;
            this.badstatusresist = true;

            this.overlayColor = Color.Transparent;
            this.textColor = Color.White;
        }

        public override int Hp
        {
            get
            {
                return base.Hp;
            }

            set
            {
                var originalHp = this.Hp;

                base.Hp = value;
                if (this.state == MOTION.Absorbing)
                {
                    if (originalHp > this.Hp)
                    {
                        // Damage buildup handled by Dameged()
                        // Added Dameged() calls to MasterSpark, poison, etc.

                        // Keep track of ending damage to prevent overkill
                        this.lastDamage = originalHp - this.Hp;
                    }
                }
            }
        }

        public override void Dameged(AttackBase attack)
        {
            if (attack is Dummy)
            {
                return;
            }

            if (this.state == MOTION.Absorbing)
            {
                if (this.parent.blackOut && this.controller.blackoutAdaptChip == null && this.controller.preAdaptWaitTime == null)
                {
                    foreach (CharacterBase characterBase in this.parent.AllChara())
                    {
                        if (characterBase.number == parent.blackOutChips[0].userNum)
                            this.controller.preAdaptWaitTime = characterBase.waittime;
                    }

                    this.controller.blackoutAdaptChip = new DammyChip(this.sound);
                    var adaptText = ShanghaiEXE.Translate("Enemy.HeavenBarrierSpecial");
                    this.controller.blackoutAdaptChip.BlackOut(this, this.parent, adaptText, "");
                }

                if (this.parent.blackOut)
                {
                    if (!this.controller.controlledBarriers.Any(b => b.blackOutObject))
                    {
                        this.blackOutObject = true;

                        foreach (var b in this.controller.controlledBarriers)
                        {
                            if (b == this)
                            {
                                continue;
                            }

                            b.blackoutSimulhitPossible = true;
                        }
                    }
                }

                if (!this.parent.blackOut || this.blackOutObject || this.blackoutSimulhitPossible)
                {
                    var attackDamage = this.lastDamage;
                    if (attackDamage == -1)
                    {
                        attackDamage = attack.DamageMath(this);
                    }

                    var remainingDamage = this.controller.totalHp - this.controller.rawDamageTaken;
                    var cappedDamage = Math.Min(attackDamage, remainingDamage);
                    this.controller.rawDamageTakenSinceLastUpdate += cappedDamage;

                    this.controller.unprocessedAttacks.Add(Tuple.Create(this, attack.Element, cappedDamage));
                }
                else if (this.parent.blackOut && !this.blackoutBuildupInterrupted)
                {
                    this.sound.PlaySE(SoundEffect.bound);

                    var effectiveEffect = new Elementhit(this.sound, this.parent, this.positionDirect, 1, this.position, ChipBase.ELEMENT.eleki);
                    effectiveEffect.blackOutObject = true;
                    this.parent.effects.Add(effectiveEffect);
                    
                    this.invincibility = true;
                    this.invincibilitytime = 1;
                    // alpha increases by 15 per tick, if not exact then overflows and wraps around
                    this.alfha = byte.MaxValue - (15 * 8);

                    // TODO: Effects
                    this.blackoutBuildupInterrupted = true;
                }

                this.lastDamage = -1;
            }
        }

        public override void InitAfter()
        {
            base.InitAfter();

            var barriers = this.Parent.AllChara().Where(c => c.union == this.union).OfType<HeavenBarrier>();
            if (this.controller == null && this == barriers.OrderBy(b => b.position.X).ThenBy(b => b.position.Y).First())
            {
                this.controller = barriers.Select(b => b.controller).FirstOrDefault(c => c != null) ?? barriers.FirstOrDefault() ?? this;
                this.controller.controlledBarriers = barriers.ToList();

                var newDeathOrder = 0;
                this.controller.controlledBarriers.ForEach(c => 
                {
                    c.controller = this.controller;
                    this.controller.totalHp += c.Hp;
                    c.Hp = Constants.ArbitraryLargeValue;
                    c.deathOrder = newDeathOrder++;
                });
                
                this.controller.damageBuildup = new Dictionary<ChipBase.ELEMENT, int>
                {
                    { ChipBase.ELEMENT.normal, 0 },
                    { ChipBase.ELEMENT.heat, 0 },
                    { ChipBase.ELEMENT.aqua, 0 },
                    { ChipBase.ELEMENT.eleki, 0 },
                    { ChipBase.ELEMENT.leaf, 0 },
                    { ChipBase.ELEMENT.poison, 0 },
                    { ChipBase.ELEMENT.earth, 0 }
                };
                this.controller.unprocessedAttacks = new List<Tuple<HeavenBarrier, ChipBase.ELEMENT, int>>();

                var infoPositionX = !this.parent.panel[0, 1].Hole && this.parent.panel[0, 1].state != Panel.PANEL._un
                    ? 0 : 1;
                var infoPositionY = !this.parent.panel[0, 1].Hole && this.parent.panel[0, 1].state != Panel.PANEL._un
                    ? 1 : 2;
                this.controller.infoPanel = new BarrierInfoPanel(this.sound, this.parent, this.UnionEnemy, infoPositionX, infoPositionY, elem => this.controller.damageBuildup[elem]);
                this.controller.parent.objects.Add(this.controller.infoPanel);

                this.parent.custom.escapeV = Constants.ArbitraryLargeValue;
            }
        }

        public override void PositionDirectSet()
        {
            this.positionDirect = new Vector2((float)(position.X * 40.0 + 20.0) + 4 * this.UnionRebirth + 2, (float)(position.Y * 24.0 + 54.0) - 4);
        }

        public override void Updata()
        {
            if (!this.parent.blackOut)
            {
                base.Updata();

                if (this.controller == this)
                {
                    this.infoPanel.ForcedShowState = null;
                    this.blackoutAdaptChip = null;

                    foreach (var b in this.controlledBarriers)
                    {
                        b.blackOutObject = false;
                        b.blackoutBuildupInterrupted = false;
                        b.blackoutWaittime = 0;
                    }
                }
            }
            else
            {
                // redundant check since only flagged objs run during blackout
                // harmless, but explicitly should only run if it's the "main" one
                if (this.blackOutObject)
                {
                    this.controller.infoPanel.ForcedShowState = false;

                    if (this.controller.preAdaptWaitTime != null && parent.blackOutChips[0] != this.controller.blackoutAdaptChip)
                    {
                        foreach (CharacterBase characterBase in parent.AllChara())
                        {
                            if (characterBase.number == parent.blackOutChips[0].userNum)
                                characterBase.waittime = this.controller.preAdaptWaitTime.Value;
                        }
                        this.controller.preAdaptWaitTime = null;
                    }

                    foreach (var b in this.controller.controlledBarriers)
                    {
                        if (b.blackoutSimulhitPossible)
                        {
                            b.blackoutSimulhitPossible = false;
                        }
                    }
                }

                this.blackoutWaittime++;
            }
        }

        public override void RenderUP(IRenderer dg)
        {
            if (this.controller == this)
            {
                if (this.blackoutAdaptChip?.chipUseEnd == false)
                {
                    this.blackoutAdaptChip.BlackOutRender(dg, this.union);
                }
            }
        }


        protected override void Moving()
        {
            this.neutlal = true;

            switch (this.state)
            {
                case MOTION.Absorbing:
                    this.animationpoint = IdleAnimation(this.waittime);

                    this.invincibility = false;
                    this.invincibilitytime = 0;
                    var totalDamage = this.controller.rawDamageTaken;
                    if (totalDamage >= this.controller.totalHp)
                    {
                        this.state = MOTION.Breaking;
                        this.Hp = 0;
                        this.waittime = 0;
                    }
                    break;
                case MOTION.Breaking:
                    var deathDelay = (this.deathOrder + 1) * 90;
                    if (this.waittime >= deathDelay)
                    {
                        var fakeDeathPosition = new Vector2((int)this.positionDirect.X + this.Shake.X, (int)this.positionDirect.Y + this.Shake.Y);
                        var fakeDeathRect = new Rectangle(this.animationpoint.X * this.wide, this.animationpoint.Y * this.height, this.wide, this.height);

                        this.parent.effects.Add(new EnemyDeath(this.sound, this.parent, fakeDeathRect, new Rectangle(this.animationpoint.X * this.wide, 0, this.wide, this.height), fakeDeathPosition, this.picturename, this.rebirth, this.position));

                        this.animationpoint = new Point(this.Random.Next(1, 9 + 1), 1);
                        this.waittime = 0;
                        this.state = MOTION.Broken;
                    }
                    else
                    {
                        this.animationpoint = IdleAnimation(this.waittime);
                    }
                    break;
                case MOTION.Broken:
                    if (this.waittime % 8 == 0)
                    {
                        var nextBrokenAnimX = ((this.animationpoint.X + 1) % 9) + 1;
                        this.animationpoint = new Point(nextBrokenAnimX, 1);
                    }

                    if (this.controller == this)
                    {
                        if (this.controlledBarriers.All(c => c.state == MOTION.Broken))
                        {
                            this.controlledBarriers.ForEach(c =>
                            {
                                c.state = MOTION.RetaliatingChargeUp;
                                c.waittime = 0;
                            });
                            this.infoPanel.AllowedToBreak = true;
                            this.infoPanel.Break();

                            var cancelledElements = this.damageBuildup.ToDictionary(kvp => kvp.Key, kvp => 0);
                            foreach (var typeDamage in this.damageBuildup)
                            {
                                if (typeDamage.Key == ChipBase.ELEMENT.normal || typeDamage.Value <= 0)
                                {
                                    continue;
                                }

                                var effectiveTypes = GetEffectiveElements(typeDamage.Key);
                                if (effectiveTypes.All(t => this.damageBuildup[t] - cancelledElements[t] > 0))
                                {
                                    var cancelledElementTypes = effectiveTypes.ToList();
                                    cancelledElementTypes.Add(typeDamage.Key);
                                    var cancelledAmount = cancelledElementTypes.Select(t => this.damageBuildup[t] - cancelledElements[t]).Min();
                                    foreach (var cancelledElementType in cancelledElementTypes)
                                    {
                                        cancelledElements[cancelledElementType] += cancelledAmount;
                                    }
                                }
                            }
                            foreach (var kvp in cancelledElements)
                            {
                                this.damageBuildup[kvp.Key] -= kvp.Value;
                            }

                            this.remainingRetaliation = this.damageBuildup.Sum(kvp => kvp.Value);
                            this.isPerfectKill = this.remainingRetaliation == 0;
                        }
                    }
                    break;
                case MOTION.RetaliatingChargeUp:
                    this.animationpoint = new Point(1, 1);
                    if (this.waittime < 360)
                    {
                        if (this.controller == this)
                        {
                            if (this.waittime > 180 && this.waittime <= 200)
                            {
                                this.sound.PlaySE(SoundEffect.beamlong);
                            }
                            if (this.waittime == 200)
                            {
                                this.sound.BGMFadeStart(90, 25);
                            }

                            if (this.waittime == 290)
                            {
                                this.sound.BGMFadeStart(50, 0);
                            }
                            if (this.waittime == 340)
                            {
                                this.sound.SetBGM("heavenbackground");
                                this.sound.BGMFadeStart(20, 100);
                            }
                        }

                        if (this.waittime < 300)
                        {
                            var whiteoutProgress = 0.5 * (1 - Math.Cos((Math.Min(500, this.waittime * 1.5) / 500.0 / 2) * 2 * Math.PI));
                            this.overlayColor = Color.FromArgb((int)(whiteoutProgress * 255), Color.White);

                            var dissolveProgress = Math.Min(200, this.waittime) / 200.0;
                            switch (this.waittime % 3)
                            {
                                case 0:
                                case 1:
                                    var initialPosition = new Point((int)this.positionDirect.X + 1, (int)this.positionDirect.Y - 2 + (int)(whiteoutProgress * 34) + this.Random.Next(1, 4));
                                    var lifespan = 300;
                                    var t = 0;
                                    var randomPhaseShift = this.Random.NextDouble() * 2 * Math.PI;
                                    var outerRim = 4 * Math.Min(0.5, 1.0 / (1 + (400 - this.waittime) / 20.0));
                                    var movement = new Action<Sparkle>(s =>
                                    {
                                        t++;
                                        var xScale = Math.Min(outerRim, Math.Min(0.5, 1.0 / (1 + t / 20.0)));
                                        var x = 20 * xScale * Math.Cos(randomPhaseShift + (t / (400.0 / 5)) * 2 * Math.PI);
                                        var y = 20 * -0.0001 * t * t + 0.5 * Math.Sin(randomPhaseShift + (t / (400.0 / 5)) * 2 * Math.PI);
                                        s.Position = new Point(initialPosition.X + (int)Math.Round(x), initialPosition.Y + (int)Math.Round(y));
                                    });
                                    var newSparkle = new Sparkle { Lifespan = lifespan, RemainingLife = lifespan, Movement = movement };
                                    movement.Invoke(newSparkle);
                                    this.sparkles[this.sparkles.FirstOrDefault(kvp => !this.sparkles.ContainsKey(kvp.Key + 1)).Key + 1] = newSparkle;
                                    break;
                            }
                        }
                        else
                        {
                            var whiteoutProgress = (int)Math.Max(0, Math.Min(255, (this.waittime - 300) / 60.0 * 255));
                            this.overlayColor = Color.FromArgb(255 - whiteoutProgress, Color.White);
                            this.alfha = (byte)(255 - whiteoutProgress);
                        }

                        if (this.controller == this)
                        {
                            if (this.waittime < 225)
                            {
                                var textStrobeTime = 45;
                                if (this.waittime % textStrobeTime == 0)
                                {
                                    this.textColor = (this.waittime / textStrobeTime) % 2 == 0 ? Color.Red : Color.Transparent;
                                }
                            }
                            else
                            {
                                this.textColor = Color.Red;
                            }

                            // pre-mark panels for petals
                            if (this.waittime > 200)
                            {
                                if (this.waittime % 8 == 0)
                                {
                                    var untargetedPanels = Enumerable.Range(0, 6).SelectMany(c => Enumerable.Range(0, 3).Select(r => new Point(c, r))).Cast<Point?>()
                                        .Where(p => !this.parent.attacks.Any(ef => ef is Dummy && ef.position == p)
                                                && (!this.isPerfectKill || this.parent.panel[p.Value.X, p.Value.Y].color == this.union));
                                    var untargetedOrRandomPanel = untargetedPanels.ElementAtOrDefault(this.Random.Next(0, untargetedPanels.Count()))
                                        ?? new Point(this.Random.Next(!this.isPerfectKill ? 0 : 3, 6), this.Random.Next(0, 3));
                                    this.parent.attacks.Add(new Dummy(this.sound, this.parent, untargetedOrRandomPanel.X, untargetedOrRandomPanel.Y, this.union, new Point(0, 0), 120, true));
                                }
                            }

                            if (this.waittime == 300)
                            {
                                var petalBreeze = new PetalBreeze(this.sound, Vector2.Zero, Point.Empty);
                                petalBreeze.upprint = true;
                                this.parent.effects.Add(petalBreeze);

                                var enemies = this.parent.AllChara().Where(c => c.union == this.UnionEnemy);
                                foreach (var enemy in enemies)
                                {
                                    enemy.barierTime = Math.Min(enemy.barierTime, 60);
                                }
                            }
                        }
                    }
                    else
                    {
                        this.state = MOTION.Retaliating;
                        this.waittime = 0;

                        this.invincibility = true;
                        this.invincibilitytime = Constants.ArbitraryLargeValue;
                    }
                    break;
                case MOTION.Retaliating:
                    // Prevent fleeing after retaliation starts
                    // this.parent.custom.escapeV = -ArbitraryLargeValue;
                    this.animationpoint = new Point(1, 1);

                    if (this.controller == this)
                    {
                        var retaliationTime = !this.isPerfectKill ? 1000 : 200;
                        if (this.waittime < retaliationTime)
                        {
                            if (this.retaliateTickAction == null)
                            {
                                this.textColor = Color.Red;

                                var fullDamagePerTick = (double)this.remainingRetaliation / retaliationTime;
                                var damagePerTick = (int)fullDamagePerTick;
                                var remainderPerTick = fullDamagePerTick % 1.0;
                                var leftoverDamage = 0.0;
                                this.retaliateTickAction = () =>
                                {
                                    leftoverDamage += remainderPerTick;
                                    var leftoverRemainder = leftoverDamage % 1.0;
                                    var wholeLeftoverDamage = (int)(leftoverDamage - leftoverRemainder);
                                    leftoverDamage = leftoverRemainder;

                                    var damageThisTick = wholeLeftoverDamage + damagePerTick;
                                    this.remainingRetaliation -= damageThisTick;

                                    var enemies = this.parent.AllHitter().Where(c => c.union == this.UnionEnemy);
                                    foreach (var enemy in enemies)
                                    {
                                        enemy.Hp -= damageThisTick;
                                    }
                                };
                            }

                            this.retaliateTickAction.Invoke();

                            if (this.waittime % 120 == 0)
                            {
                                var petalBreeze = new PetalBreeze(this.sound, Vector2.Zero, Point.Empty);
                                petalBreeze.upprint = (this.waittime / 120) % 2 == 0;
                                this.parent.effects.Add(petalBreeze);
                            }

                            if (this.waittime % 4 == 0)
                            {
                                var untargetedPanels = Enumerable.Range(0, 6).SelectMany(c => Enumerable.Range(0, 3).Select(r => new Point(c, r))).Cast<Point?>()
                                    .Where(p => !this.parent.effects.Any(ef => ef is BadWater && ef.position == p)
                                                && (!this.isPerfectKill || this.parent.panel[p.Value.X, p.Value.Y].color == this.union));
                                var untargetedOrRandomPanel = untargetedPanels.ElementAtOrDefault(this.Random.Next(0, untargetedPanels.Count()))
                                    ?? new Point(this.Random.Next(!this.isPerfectKill ? 0 : 3, 6), this.Random.Next(0, 3));
                                this.parent.effects.Add(new BadWater(this.sound, this.parent, untargetedOrRandomPanel.X, untargetedOrRandomPanel.Y, 2, BadWater.TYPE.Pink));
                            }

                            if (this.waittime < 1000 - 120)
                            {
                                if (this.waittime % 8 == 0)
                                {
                                    var untargetedPanels = Enumerable.Range(0, 6).SelectMany(c => Enumerable.Range(0, 3).Select(r => new Point(c, r))).Cast<Point?>()
                                        .Where(p => !this.parent.attacks.Any(ef => ef is Dummy && ef.position == p)
                                                && (!this.isPerfectKill || this.parent.panel[p.Value.X, p.Value.Y].color == this.union));
                                    var untargetedOrRandomPanel = untargetedPanels.ElementAtOrDefault(this.Random.Next(0, untargetedPanels.Count()))
                                        ?? new Point(this.Random.Next(!this.isPerfectKill ? 0 : 3, 6), this.Random.Next(0, 3));
                                    this.parent.attacks.Add(new Dummy(this.sound, this.parent, untargetedOrRandomPanel.X, untargetedOrRandomPanel.Y, this.union, new Point(0, 0), 120, true));
                                }
                            }
                        }
                        else if (this.waittime < retaliationTime + 300)
                        {
                            if (this.remainingRetaliation > 0)
                            {
                                var enemies = this.parent.AllChara().Where(c => c.union == this.UnionEnemy);
                                foreach (var enemy in enemies)
                                {
                                    enemy.Hp -= this.remainingRetaliation;
                                }
                                this.remainingRetaliation = 0;
                            }

                            // settle
                            this.textColor = Color.Transparent;
                        }
                        else
                        {
                            if (this.controlledBarriers.All(c => c.state == MOTION.Retaliating))
                            {
                                this.controlledBarriers.ForEach(c =>
                                {
                                    c.state = MOTION.Ended;
                                    c.waittime = 0;
                                });
                            }
                        }
                    }
                    break;
                case MOTION.Ended:
                    this.animationpoint = new Point(1, 1);
                    break;
            }

            if (this.controller == this)
            {
                var barriersSimulHit = this.unprocessedAttacks.Select(a => a.Item1).Concat(this.controlledBarriers.Where(b => b.blackoutBuildupInterrupted));
                foreach (var attack in this.unprocessedAttacks)
                {
                    var hitBarrier = attack.Item1;
                    var hitElement = attack.Item2;
                    var hitAmount = attack.Item3;
                    foreach (var barrier in this.controlledBarriers)
                    {
                        var simulHit = barriersSimulHit.Contains(barrier);
                        if (simulHit)
                        {
                            // do nothing
                        }
                        else
                        {
                            var damageBlob = new BarrierDamageBlob(
                                this.sound,
                                this.parent,
                                hitBarrier.positionDirect,
                                hitBarrier.position,
                                barrier.positionDirect,
                                () => { this.damageBuildup[hitElement] += hitAmount; });
                            damageBlob.upprint = true;
                            this.parent.effects.Add(damageBlob);
                        }
                    }
                }
                this.unprocessedAttacks.Clear();

                if (this.rawDamageTakenSinceLastUpdate > 0)
                {
                    this.rawDamageTaken += this.rawDamageTakenSinceLastUpdate;
                    this.rawDamageTakenSinceLastUpdate = 0;
                }

                var panels = Enumerable.Range(0, this.parent.panel.GetLength(0))
                    .SelectMany(px => Enumerable.Range(0, this.parent.panel.GetLength(1)).Select(py => new Point(px, py)));
                foreach (var panelPos in panels)
                {
                    var panel = this.parent.panel[panelPos.X, panelPos.Y];
                    switch (panel.State)
                    {
                        case Panel.PANEL._sand:
                        case Panel.PANEL._poison:
                        case Panel.PANEL._burner:
                        case Panel.PANEL._thunder:
                            panel.State = Panel.PANEL._nomal;
                            this.parent.effects.Add(new Smoke(this.sound, this.parent, panelPos.X, panelPos.Y, panel.Element));
                            break;
                        case Panel.PANEL._break:
                            if (this.controlledBarriers.Any(c => c.position == panelPos))
                            {
                                panel.state = Panel.PANEL._crack;
                            }
                            break;
                        case Panel.PANEL._grass:
                        case Panel.PANEL._ice:
                        case Panel.PANEL._crack:
                        case Panel.PANEL._nomal:
                        case Panel.PANEL._none:
                        case Panel.PANEL._un:
                            break;
                    }
                }
                var forbiddenObjects = this.parent.AllObjects().Where(o => o is SuzuranWhite || o is BigSuzuran).Cast<ObjectBase>();
                foreach (var forbiddenObject in forbiddenObjects)
                {
                    forbiddenObject.Break();
                    this.parent.attacks.Add(new CrackThunder(this.sound, this.parent, forbiddenObject.position.X, forbiddenObject.position.Y, this.union, forbiddenObject.Hp, true));
                }

                if (this.controlledBarriers.Any(c => c.state == MOTION.Absorbing || c.state == MOTION.Breaking || c.state == MOTION.Broken))
                {
                    var decrementedElements = new List<ChipBase.ELEMENT>();
                    foreach (var typeDamage in this.damageBuildup)
                    {
                        if (typeDamage.Key == ChipBase.ELEMENT.normal || typeDamage.Value - decrementedElements.Count(e => e == typeDamage.Key) <= 0)
                        {
                            continue;
                        }

                        var effectiveTypes = GetEffectiveElements(typeDamage.Key);
                        if (effectiveTypes.All(t => this.damageBuildup[t] - decrementedElements.Count(e => e == t) > 0))
                        {
                            decrementedElements.Add(typeDamage.Key);
                            decrementedElements.AddRange(effectiveTypes);
                        }
                    }
                    foreach (var decrementedElement in decrementedElements)
                    {
                        this.damageBuildup[decrementedElement]--;
                    }
                    this.damageBuildup[ChipBase.ELEMENT.normal] -= Math.Min(decrementedElements.Count, this.damageBuildup[ChipBase.ELEMENT.normal]);

                    if (this.waittime % 2 == 0)
                    {
                        var xOffset = this.Random.Next(0, 50);
                        var yOffset = this.Random.Next(0, 70 - xOffset);
                        var noisePosition = new Vector2(250 - xOffset, -10 + yOffset);
                        var noiseEffect = new Noise(this.sound, noisePosition, Point.Empty);
                        noiseEffect.downprint = true;
                        noiseEffect.blackOutObject = false;
                        this.parent.effects.Insert(0, noiseEffect);
                    }
                    if (this.waittime % 40 == 0)
                    {
                        var xOffset = this.Random.Next(20, 50) + 10;
                        var yOffset = 70 - xOffset;
                        var shieldPosition = new Vector2(250 - xOffset, -10 + yOffset);
                        var shieldEffect = new ReflShield(this.sound, shieldPosition, Point.Empty);
                        shieldEffect.downprint = true;
                        shieldEffect.blackOutObject = false;
                        this.parent.effects.Add(shieldEffect);
                    }

                    if (this.waittime % 100 == 50 && this.Random.NextDouble() < 0.5)
                    {
                        var target = this.RandomPanel(this.UnionEnemy);
                        var xOffset = this.Random.Next(0, 50);
                        var yOffset = this.Random.Next(0, 70 - xOffset);
                        var noisePower = 100;
                        var noisePosition = new Vector2(260 - xOffset, -10 + yOffset);
                        var yVel = -(float)this.Random.NextDouble() * Math.Max(0, noisePosition.Y) / 5;
                        var breakthroughChance = 0.1 + (0.8 * this.rawDamageTaken / this.totalHp);
                        this.parent.attacks.Add(
                            new NoiseBreakout(
                                this.sound,
                                this.parent,
                                target.X, target.Y,
                                this.union,
                                noisePower,
                                1,
                                noisePosition,
                                yVel,
                                this.Random.NextDouble() >= breakthroughChance));
                    }
                }
                else if (this.controlledBarriers.Any(c => c.state == MOTION.RetaliatingChargeUp))
                {
                    var xOffset = this.Random.Next(0, 50);
                    var yOffset = this.Random.Next(0, 70 - xOffset);
                    var noisePosition = new Vector2(250 - xOffset, -10 + yOffset);

                    var noiseExpansion = 1 + (200 - Math.Abs(200 - Math.Min(360, this.waittime))) / 30;
                    var expansionPositions = new[]
                    {
                        noisePosition,
                        new Vector2(220, 20), new Vector2(204, 6), new Vector2(210, 40),
                        new Vector2(180, 18), new Vector2(170, 10), new Vector2(190, 30)
                    };
                    
                    if (this.waittime % 2 == 0)
                    {
                        for (var i = 0; i < noiseExpansion; i++)
                        {
                            if (i >= expansionPositions.Length) break;

                            var newNoisePosition = new Vector2(expansionPositions[i].X + this.Random.Next(-10, 9), expansionPositions[i].Y + this.Random.Next(-10, 9));
                            var noiseEffect = new Noise(this.sound, newNoisePosition, Point.Empty);
                            noiseEffect.downprint = true;
                            noiseEffect.blackOutObject = false;
                            this.parent.effects.Insert(0, noiseEffect);
                        }
                    }

                    if (this.waittime > 50 && this.waittime < 200)
                    {
                        if (this.waittime % 45 == 0)
                        {
                            this.sound.PlaySE(SoundEffect.noise);
                        }
                    }
                    else if (this.waittime > 200 && this.waittime < 400)
                    {
                        if (this.waittime % 30 == 0)
                        {
                            this.sound.PlaySE(SoundEffect.pikin);
                        }
                    }
                }
            }

            this.waittime++;
            this.UpdateSparkles();
            this.MoveAftar();
        }

        public override void Render(IRenderer dg)
        {
            this._position = new Vector2((int)this.positionDirect.X + this.Shake.X, (int)this.positionDirect.Y + this.Shake.Y);
            this._rect = new Rectangle(this.animationpoint.X * this.wide, this.animationpoint.Y * this.height, this.wide, this.height);
            if (this.Hp <= 0 && this.state == MOTION.Ended)
                this.Death(this._rect, new Rectangle(this.animationpoint.X * this.wide, this.height, this.wide, this.height), new Vector2(-100, -100)/* this._position */, this.picturename);
            if (this.alfha < byte.MaxValue)
                this.color = Color.FromArgb(alfha, byte.MaxValue, byte.MaxValue, byte.MaxValue);
            else
                this.color = this.mastorcolor;
            if (this.state != MOTION.Retaliating && this.state != MOTION.Ended)
            {
                if (this.whitetime == 0)
                {
                    dg.DrawImage(dg, this.picturename, this._rect, false, this._position, this.rebirth, this.color);
                }
                else
                {
                    this._rect.X = 0;
                    dg.DrawImage(dg, this.picturename, this._rect, false, this._position, this.rebirth, this.color);
                }

                this._rect.Offset(320 - this._rect.X, 0);
                dg.DrawImage(dg, this.picturename, this._rect, false, this._position, this.rebirth, this.overlayColor);
            }

            foreach (var sparkleEntry in this.sparkles)
            {
                var sparkle = sparkleEntry.Value;
                if (sparkle.RemainingLife <= 0) continue;
                this._rect = new Rectangle(SparkleTextureRect.X + (sparkle.Frame * SparkleTextureRect.Width), SparkleTextureRect.Y, SparkleTextureRect.Width, SparkleTextureRect.Height);
                this._position = new Vector2(Shake.X + sparkle.Position.X, Shake.Y + sparkle.Position.Y);
                this.color = Color.White;
                dg.DrawImage(dg, "body25", this._rect, false, this._position, this.rebirth, this.color);
            }

            HeavenBarrier activeBlackoutObject;
            if (this.parent.blackOut && ((activeBlackoutObject = this.controller.controlledBarriers.FirstOrDefault(b => b.blackOutObject)) != null)
                && activeBlackoutObject != this && !this.blackoutBuildupInterrupted)
            {
                var chargeFrame = (activeBlackoutObject.blackoutWaittime / 2) % 8;
                this._rect = new Rectangle(64 * chargeFrame, 0, 64, 64);
                this._position = new Vector2((int)this.positionDirect.X + this.Shake.X, (int)this.positionDirect.Y + this.Shake.Y);
                this.color = Color.White;
                dg.DrawImage(dg, "charge", this._rect, false, this._position, this.rebirth, this.color);
            }

            //this.HPposition = new Vector2(this.positionDirect.X + 4f, this.positionDirect.Y - 32f);
            this.Nameprint(dg, this.printNumber);

            if (this.controller == this)
            {
                if (this.textColor.A != 0)
                {
                    var remainingHp = this.totalHp - (this.rawDamageTaken + this.rawDamageTakenSinceLastUpdate);
                    if (remainingHp > 0 && this.controlledBarriers.All(c => c.state == MOTION.Absorbing))
                    {
                        DrawBlockCharacters(dg, this.Nametodata(remainingHp.ToString()), 88, new Vector2(164 + this.Shake.X, 20 + this.Shake.Y), Color.White, out var finalRect, out var finalPos);
                    }
                    else if (this.controlledBarriers.All(c => c.state == MOTION.RetaliatingChargeUp || c.state == MOTION.Retaliating) && !this.isPerfectKill)
                    {
                        DrawBlockCharacters(dg, this.Nametodata(this.remainingRetaliation.ToString()), 88, new Vector2(164 + this.Shake.X, 40 + this.Shake.Y), Color.Red, out var finalRect, out var finalPos);
                    }
                }
            }
        }

        private void UpdateSparkles()
        {
            foreach (var kvp in this.sparkles)
            {
                kvp.Value.RemainingLife -= 1;
            }

            var expiredSparkles = this.sparkles.Where(kvp => kvp.Value.RemainingLife <= 0).ToArray();

            foreach (var expired in expiredSparkles)
            {
                this.sparkles.Remove(expired);
            }

            foreach (var kvp in this.sparkles)
            {
                kvp.Value.Movement?.Invoke(kvp.Value);
            }
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

        private static Point IdleAnimation(int frame)
        {
            const int cyclelength = 18 - 1;
            var x = 1;
            var adjustedFrame = frame / 6;
            var cycleFrame = adjustedFrame % cyclelength;

            x = new[] { 1, 1, 1, 2, 3, 4, 5, 6, 7, 7, 6, 5, 4, 3, 2, 1, 1, 1 }[cycleFrame];

            return new Point(x, 0);
        }

        private class Sparkle
        {
            public int Lifespan { get; set; }
            public int RemainingLife { get; set; }

            public int Frame => 3 - (int)Math.Round(3.0 * this.RemainingLife / this.Lifespan);

            public Point Position { get; set; }
            public Action<Sparkle> Movement { get; set; }
        }

        private enum MOTION
        {
            Absorbing,
            Breaking,
            Broken,
            RetaliatingChargeUp,
            Retaliating,
            Ended
        }
    }
}

