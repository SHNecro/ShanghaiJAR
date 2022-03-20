using NSBattle;
using NSChip;
using NSShanghaiEXE.InputOutput.Audio;
using System.Drawing;
using System.Linq;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using NSEffect;
using NSAttack;
using NSGame;

namespace NSEnemy
{
    internal class Madman : ChipUsingNaviBase
    {
        private static readonly Rectangle SoulTextureRect = new Rectangle(360, 780, 30, 20);
        private static readonly Rectangle SparkleTextureRect = new Rectangle(450, 780, 5, 5);

        private static readonly Vector2 SoulOffset = new Vector2(2, 16);

        private static readonly IDictionary<Stage, StageSummon> StageSummons = new Dictionary<Stage, StageSummon>
        {
                { Stage.ShellArmor, new StageSummon {
                    EnrageStage = (Stage?)Stage.Invis,
                    Summon1 = new SummonDefinition { Id = 9, X = 3, Y = 1, Rank = 3 },
                    Summon2 = new SummonDefinition { Id = 9, X = 5, Y = 0, Rank = 2 }
                } },
                { Stage.Invis, new StageSummon {
                    EnrageStage = (Stage?) Stage.Aura,
                    Summon1 = new SummonDefinition { Id = 41, X = 3, Y = 0, Rank = 3 },
                    Summon2 = new SummonDefinition { Id = 41, X = 5, Y = 2, Rank = 2 }
                } },
                { Stage.Aura, new StageSummon {
                    EnrageStage = (Stage?)Stage.Barriers,
                    Summon1 = new SummonDefinition { Id = 37, X = 3, Y = 0, Rank = 3 },
                    Summon2 = new SummonDefinition { Id = 37, X = 4, Y = 1, Rank = 2 }
                }},
                { Stage.Barriers, new StageSummon {
                    EnrageStage = (Stage?)Stage.Guard,
                    Summon1 = new SummonDefinition { Id = 18, X = 5, Y = 2, Rank = 3 },
                    Summon2 = new SummonDefinition { Id = 18, X = 4, Y = 0, Rank = 2 }
                }},
                { Stage.Guard, new StageSummon {
                    EnrageStage = (Stage?)null,
                    Summon1 = new SummonDefinition { Id = 24, X = 3, Y = 0, Rank = 3 },
                    Summon2 = new SummonDefinition { Id = 24, X = 3, Y = 2, Rank = 2 }
                }},
        };

        private readonly IDictionary<int, Sparkle> sparkles = new ConcurrentDictionary<int, Sparkle>();

        private readonly IDictionary<Point, DelayedAction> delayedPanelActions = new Dictionary<Point, DelayedAction>();

        private Vector2 soulPosition;

        private Stage stage = Stage.ShellArmor;
        private Stage enrageStage;
        private bool isActivated;
        private int enabledChips;

        private bool stageInitialized;
        private int prevSummonCount;
        private EnemyBase summon1;
        private EnemyBase summon2;

        private NSObject.Rock shellArmorRock;
        private bool isFloatingOverCube;
        
        private bool beastOutEnd;
        private Color soulOverlayColor = Color.FromArgb(0);

        public Madman(
          IAudioEngine s,
          SceneBattle p,
          int pX,
          int pY,
          byte n,
          Panel.COLOR u,
          byte v)
          : base(s, p, pX, pY, n, u, v, 800, NSGame.ShanghaiEXE.Translate("Enemy.MadmanName"), "none")
        {
            this.hp = 1;
            this.hpprint = 1;
            this.SetDroppedChips();
            this.Flying = true;
        }

        public override int Hp
        {
            get
            {
                return base.Hp;
            }

            set
            {
                if ((this.barrierType == BARRIER.None &&
                    this.guard == GUARD.none &&
                    !this.invincibility &&
                    !this.isFloatingOverCube &&
                    this.stageInitialized &&
                    (this.stage == Stage.Vulnerable || this.enrageStage == Stage.Vulnerable))
                    || (this.stage == Stage.Enraged && value > base.Hp))
                {
                    base.Hp = value;
                }
            }
        }

        private IList<EnemyBase> Summons => new[] { this.summon1, this.summon2 }.Where(eb => eb != null).ToList();

        public override void Updata()
        {
            if (!this.stageInitialized)
            {
                this.InitializeStage();
            }

            if (this.stageInitialized)
            {
                this.UpdateStage(this.stage);
            }

            this.CheckSpecialTriggers();

            this.waittime++;

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

            if (this.waittime % 8 == 0)
            {
                var lifespan = 36;
                var randomDist = Random.NextDouble() * 5 + 8;
                var randomAngle = Random.NextDouble() * 2 * Math.PI;
                var pos = new Point((int)this.soulPosition.X + (int)(randomDist * Math.Cos(randomAngle)), (int)this.soulPosition.Y + (int)(randomDist * Math.Sin(randomAngle)));

                var newSparkle = new Sparkle { Lifespan = lifespan, RemainingLife = lifespan, Position = pos, Movement = null };
                this.sparkles[this.sparkles.FirstOrDefault(kvp => !this.sparkles.ContainsKey(kvp.Key + 1)).Key + 1] = newSparkle;
            }
        }

        public override void Render(IRenderer dg)
        {
            // Shadow
            if (!this.isFloatingOverCube)
            {
                this._rect = new Rectangle(32, 48 * 8, 32, 48);
                this._position = new Vector2(this.positionDirect.X + Shake.X + SoulOffset.X - 1, this.positionDirect.Y + Shake.Y - 1 + SoulOffset.Y + 8);
                this.color = Color.White;
                dg.DrawImage(dg, "charachip1", this._rect, false, this._position, this.rebirth, this.color);
            }

            var soulFrame = Math.Abs(((this.mastorflame / 8) % 4) - 2);

            this.soulPosition = this.GetSoulPosition();
            if (this.Hp <= 0)
            {
                this.rd = this._rect;
                this.animationpoint = new Point(5, 2);
                this._rect = new Rectangle(this.animationpoint.X * this.wide, this.animationpoint.Y * this.height, this.wide, this.height);
                this._position = this.soulPosition;
                this.Death(this._rect, new Rectangle(SoulTextureRect.X + SoulTextureRect.Width * soulFrame, SoulTextureRect.Y, SoulTextureRect.Width, SoulTextureRect.Height), this._position, this.picturename);
            }
            if (this.alfha < byte.MaxValue)
                this.color = Color.FromArgb(alfha, byte.MaxValue, byte.MaxValue, byte.MaxValue);
            else
                this.color = this.mastorcolor;
            this.soulPosition = this.GetSoulPosition();
            this._rect = new Rectangle(SoulTextureRect.X + SoulTextureRect.Width * soulFrame, SoulTextureRect.Y, SoulTextureRect.Width, SoulTextureRect.Height);
            this._position = this.soulPosition;
            if (this.whitetime == 0)
            {
                dg.DrawImage(dg, "body25", this._rect, false, this._position, this.rebirth, this.color);
                if (!this.soulOverlayColor.IsEmpty)
                {
                    this._rect.X += 110;
                    dg.DrawImage(dg, "body25", this._rect, false, this._position, this.rebirth, this.soulOverlayColor);
                }
            }
            else
            {
                this._rect.X += 110;
                dg.DrawImage(dg, "body25", this._rect, false, this._position, this.rebirth, this.color);
            }

            foreach (var sparkleEntry in this.sparkles)
            {
                var sparkle = sparkleEntry.Value;
                if (sparkle.RemainingLife <= 0) continue;
                this._rect = new Rectangle(SparkleTextureRect.X + (sparkle.Frame * SparkleTextureRect.Width), SparkleTextureRect.Y, SparkleTextureRect.Width, SparkleTextureRect.Height);
                this._position = new Vector2(Shake.X + sparkle.Position.X, Shake.Y + sparkle.Position.Y);
                this.color = Color.White;
                dg.DrawImage(dg, "body25", this._rect, false, this._position, this.rebirth, this.color);
                if (!this.soulOverlayColor.IsEmpty)
                {
                    this._rect.X += 110;
                    dg.DrawImage(dg, "body25", this._rect, false, this._position, this.rebirth, this.soulOverlayColor);
                }
            }

            if (!this.chips[this.usechip].chip.chipUseEnd && (this.motion == NaviBase.MOTION.attack || this.motion == NaviBase.MOTION.knockback))
                this.chips[this.usechip].chip.Render(dg, this);
            this.HPposition = new Vector2(this.positionDirect.X - 8, this.positionDirect.Y + 48f - this.height / 2);
            this.Nameprint(dg, this.printNumber);
        }

        public override void Dameged(AttackBase attack)
        {
            if (attack is Dummy)
            {
                return;
            }

            if (this.invincibility && this.invincibilitytime != 0
                && attack.breakinvi)
            {
                this.invincibility = false;
                this.invincibilitytime = 0;
            }
            if ((this.barrierType == BARRIER.None &&
                    !this.invincibility &&
                    !this.isFloatingOverCube &&
                    this.stageInitialized) &&
                this.guard == GUARD.guard && attack.breaking)
            {
                this.guard = GUARD.none;
            }
        }

        public override void RenderUP(IRenderer dg)
        {
            this.BlackOutRender(dg, this.union);
        }

        protected override void InitializeChips()
        {
            this.SetDroppedChips();

            ChipFolder chrgCan = new ChipFolder(this.sound);
            chrgCan.SettingChip(9);
            ChipFolder fireArm = new ChipFolder(this.sound);
            fireArm.SettingChip(16);
            ChipFolder sqAnchr = new ChipFolder(this.sound);
            sqAnchr.SettingChip(48);
            ChipFolder seedCan = new ChipFolder(this.sound);
            seedCan.SettingChip(6);
            this.chips = new[] { chrgCan, fireArm, sqAnchr, seedCan };
        }

        protected override void SetUsedChip()
        {
            base.SetUsedChip();

            // Equal chance to use any enabled?
            var chipWeights = Enumerable.Repeat(1, this.enabledChips).ToArray();
            var num = this.Random.Next(chipWeights.Sum());
            for (var i = 0; i < chipWeights.Length; i++)
            {
                num -= chipWeights[i];
                if (num < 0)
                {
                    this.usechip = i;
                    break;
                }
            }
        }

        protected override void OnAttack()
        {
            base.OnAttack();
        }

        protected override void MovementIdle()
        {
            base.MovementIdle();
        }

        protected override void MovementEffect()
        {
            base.MovementEffect();
        }

        private void InitializeStage()
        {
            var activeStage = this.stage;
            var isStandardInitialization = true;
            switch (activeStage)
            {
                case Stage.Vulnerable:
                case Stage.Enraged:
                    isStandardInitialization = false;
                    break;
            }
            if (isStandardInitialization && this.waittime < 105)
            {
                this.nohit = true;
                if (this.waittime < 80)
                {
                    for (var i = 0; i < 2; i++)
                    {
                        var lifespan = 24;
                        var initialSoulPosition = this.soulPosition;
                        var randomDist = Random.NextDouble() * 3 + 24;
                        var randomAngle = Random.NextDouble() * 2 * Math.PI;
                        var circleX = Math.Cos(randomAngle);
                        var circleY = Math.Sin(randomAngle);
                        var pos = new Point((int)initialSoulPosition.X + (int)(randomDist * circleX), (int)initialSoulPosition.Y + (int)(randomDist * circleY));
                        var velocity = new Vector2((float)-circleY, (float)circleX);
                        var movement = new Action<Sparkle>(s =>
                        {
                            var progress = 0.01f * s.RemainingLife / s.Lifespan;
                            velocity += new Vector2(progress * (initialSoulPosition.X - s.Position.X), progress * (initialSoulPosition.Y - s.Position.Y));

                            s.Position = new Point(s.Position.X + (int)Math.Round(velocity.X), s.Position.Y + (int)Math.Round(velocity.Y));
                        });

                        var newSparkle = new Sparkle { Lifespan = lifespan, RemainingLife = lifespan, Position = pos, Movement = movement };
                        this.sparkles[this.sparkles.FirstOrDefault(kvp => !this.sparkles.ContainsKey(kvp.Key + 1)).Key + 1] = newSparkle;
                    }
                }
                else if (this.waittime > 100)
                {
                    for (var i = 0; i < 20; i++)
                    {
                        var lifespan = 32;
                        var initialSoulPosition = this.soulPosition;
                        var randomAngle = Random.NextDouble() * 2 * Math.PI;
                        var circleX = Math.Cos(randomAngle);
                        var circleY = Math.Sin(randomAngle);
                        var pos = new Point((int)initialSoulPosition.X, (int)initialSoulPosition.Y);
                        var velocity = new Vector2((float)(10 * circleX), (float)(10 * circleY));
                        var movement = new Action<Sparkle>(s =>
                        {
                            velocity += new Vector2(0.1f * -velocity.X, 0.1f * -velocity.Y);

                            s.Position = new Point(s.Position.X + (int)Math.Round(velocity.X), s.Position.Y + (int)Math.Round(velocity.Y));
                        });
                        var newSparkle = new Sparkle { Lifespan = lifespan, RemainingLife = lifespan, Position = pos, Movement = movement };
                        this.sparkles[this.sparkles.FirstOrDefault(kvp => !this.sparkles.ContainsKey(kvp.Key + 1)).Key + 1] = newSparkle;
                    }
                }
                this.BaseUpdata();
            }
            else
            {
                if (activeStage == Stage.Vulnerable && this.waittime <= 90)
                {
                    if (this.waittime != 0 && this.waittime % 30 == 0)
                    {
                        for (var i = 0; i < 20; i++)
                        {
                            var lifespan = 32;
                            var initialSoulPosition = this.soulPosition;
                            var randomAngle = Random.NextDouble() * 2 * Math.PI;
                            var circleX = Math.Cos(randomAngle);
                            var circleY = Math.Sin(randomAngle);
                            var pos = new Point((int)initialSoulPosition.X, (int)initialSoulPosition.Y);
                            var velocity = new Vector2((float)(10 * circleX), (float)(10 * circleY));
                            var movement = new Action<Sparkle>(s =>
                            {
                                velocity += new Vector2(0.1f * -velocity.X, 0.1f * -velocity.Y);

                                s.Position = new Point(s.Position.X + (int)Math.Round(velocity.X), s.Position.Y + (int)Math.Round(velocity.Y));
                            });
                            var newSparkle = new Sparkle { Lifespan = lifespan, RemainingLife = lifespan, Position = pos, Movement = movement };
                            this.sparkles[this.sparkles.FirstOrDefault(kvp => !this.sparkles.ContainsKey(kvp.Key + 1)).Key + 1] = newSparkle;
                        }
                    }
                    this.BaseUpdata();
                }
                else if (activeStage == Stage.Enraged
                    && !this.beastOutEnd && (!this.parent.blackOut || this.blackOutObject))
                {
                    this.Motion = MOTION.neutral;
                    this.nohit = true;
                    if (!this.BlackOut(
                            this,
                            this.parent,
                            ShanghaiEXE.Translate("Enemy.MadmanSpecial"),
                            ""))
                    {
                        return;
                    }
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
                    else if (this.waittime > 30)
                    {
                        this.Hp = this.HpMax;
                        var lifespan = 90;
                        var initialSoulPosition = new Vector2(this.soulPosition.X + Random.Next(-4, 5), this.soulPosition.Y);
                        var pos = new Point((int)initialSoulPosition.X, (int)initialSoulPosition.Y - 160);
                        var offset = 160 - Random.Next(16);
                        var movement = new Action<Sparkle>(s =>
                        {
                            var t = 1 - ((double)s.RemainingLife / s.Lifespan);
                            var newY = (int)Math.Round(pos.Y + Math.Sin(t * Math.PI / 2) * offset);

                            s.Position = new Point(s.Position.X, newY);
                        });
                        var newSparkle = new Sparkle { Lifespan = lifespan, RemainingLife = lifespan, Position = pos, Movement = movement };
                        this.sparkles[this.sparkles.FirstOrDefault(kvp => !this.sparkles.ContainsKey(kvp.Key + 1)).Key + 1] = newSparkle;

                        if (this.waittime < 90)
                        {
                            if (this.parent.backscreen < byte.MaxValue)
                            {
                                this.parent.backscreen += 10;
                                if (this.parent.backscreen > byte.MaxValue)
                                    this.parent.backscreen = byte.MaxValue;
                            }
                            var fadeColor = (byte)((this.waittime - 30) * 1.5);
                            this.soulOverlayColor = Color.FromArgb(fadeColor, byte.MaxValue, 0, 0);
                        }
                    }

                    this.BaseUpdata();
                }
                else
                {
                    if (this.stage != Stage.Vulnerable)
                    {
                        this.Sound.PlaySE(SoundEffect.barrier);
                    }
                    else
                    {
                        this.Sound.PlaySE(SoundEffect.bright);
                    }
                    this.SummonStageEnemies(this.stage == Stage.Enraged ? this.enrageStage : this.stage, false);
                    this.nohit = false;
                    this.stageInitialized = true;
                }
            }
        }

        private void SummonStageEnemies(Stage summonStage, bool secondarySummon)
        {
            if (StageSummons.ContainsKey(summonStage))
            {
                var summon = StageSummons[summonStage];
                if (!secondarySummon || (summon.EnrageStage != null && StageSummons.TryGetValue(summon.EnrageStage.Value, out summon)))
                {
                    if (this.summon1 == null || !this.summon1.flag)
                    {
                        this.SummonEnemy(summon.Summon1.Id, summon.Summon1.X, summon.Summon1.Y, (byte)summon.Summon1.Rank);
                    }
                    if (this.summon2 == null || !this.summon2.flag)
                    {
                        this.SummonEnemy(summon.Summon2.Id, summon.Summon2.X, summon.Summon2.Y, (byte)summon.Summon2.Rank);
                    }
                }
            }
        }

        private void UpdateStage(Stage activeStage)
        {
            switch (activeStage)
            {
                case Stage.ShellArmor:
                    {
                        var endStage = false;
                        switch (this.CheckSummonChange(activeStage))
                        {
                            case 2:
                                this.badstatus[8] = true;
                                this.badstatustime[8] = -1;
                                this.isFloatingOverCube = true;
                                this.shellArmorRock = new NSObject.Rock(this.sound, this.parent, positionre.X, positionre.Y, this.union);
                                this.nohit = true;
                                this.parent.objects.Add(this.shellArmorRock);
                                this.parent.effects.Add(new MoveEnemy(this.sound, this.parent, positionre.X, positionre.Y));
                                break;
                            case 1:
                                this.shellArmorRock.whitetime = 8;
                                this.sound.PlaySE(SoundEffect.breakObject);
                                break;
                            case 0:
                                endStage = true;
                                break;
                        }

                        if (endStage || (!this.shellArmorRock?.flag ?? true))
                        {
                            this.badstatus[8] = false;
                            this.badstatustime[8] = 0;
                            this.shellArmorRock?.Break();
                            this.nohit = false;
                            this.isFloatingOverCube = false;
                            this.EndStage(activeStage);
                            activeStage = Stage.Invis;
                        }

                        // Prevent rock from being broken or pushed
                        // this.shellArmorRock.HPset(99999);
                        // this.shellArmorRock.guard = GUARD.noDamage;
                        if (this.shellArmorRock != null)
                        {
                            this.shellArmorRock.slipping = true;
                            this.shellArmorRock.noslip = true;
                        }

                    }
                    break;
                case Stage.Invis:
                    {
                        var endStage = false;
                        switch (this.CheckSummonChange(activeStage))
                        {
                            case 2:
                                this.invincibility = true;
                                this.invincibilitytime = -1;
                                this.enabledChips = 1;
                                this.isActivated = true;
                                break;
                            case 1:
                                break;
                            case 0:
                                endStage = true;
                                break;
                        }

                        if (endStage || !this.invincibility && this.invincibilitytime == 0)
                        {
                            this.invincibility = false;
                            this.invincibilitytime = 0;
                            this.EndStage(activeStage);
                            activeStage = Stage.Aura;
                        }
                    }
                    break;
                case Stage.Aura:
                    {
                        var endStage = false;
                        switch (this.CheckSummonChange(activeStage))
                        {
                            case 2:
                                this.barierPower = 600;
                                this.barierTime = -1;
                                this.barrierType = BARRIER.PowerAura;
                                this.enabledChips = 2;
                                break;
                            case 1:
                                this.barierPower = 300;
                                break;
                            case 0:
                                endStage = true;
                                break;
                        }

                        if (endStage || this.barrierType == BARRIER.None)
                        {
                            this.EndStage(activeStage);
                            activeStage = Stage.Barriers;
                        }
                    }
                    break;
                case Stage.Barriers:
                    {
                        var endStage = false;
                        switch (this.CheckSummonChange(activeStage))
                        {
                            case 2:
                                this.barierPower = 800;
                                this.barierTime = -1;
                                this.barrierType = BARRIER.Barrier;
                                this.enabledChips = 3;
                                break;
                            case 1:
                                if (this.barierPower <= 300)
                                {
                                    endStage = true;
                                }
                                else
                                {
                                    this.barierPower -= 300;
                                }
                                break;
                            case 0:
                                endStage = true;
                                break;
                        }

                        if (endStage || this.barrierType == BARRIER.None)
                        {
                            this.barierPower = 0;
                            this.barierTime = 0;
                            this.barrierType = BARRIER.None;
                            this.EndStage(activeStage);
                            activeStage = Stage.Guard;
                        }
                    }
                    break;
                case Stage.Guard:
                    {
                        var endStage = false;
                        switch (this.CheckSummonChange(activeStage))
                        {
                            case 2:
                                this.guard = GUARD.guard;
                                this.enabledChips = 4;
                                break;
                            case 1:
                                break;
                            case 0:
                                endStage = true;
                                break;
                        }

                        if (endStage || this.guard == GUARD.none)
                        {
                            this.guard = GUARD.none;
                            this.EndStage(activeStage);
                            activeStage = Stage.Vulnerable;
                        }

                        for (var i = 0; i < 10; i++)
                        {
                            var lifespan = 24;
                            var initialSoulPosition = this.soulPosition;
                            var randomDist = Random.NextDouble() * 3 + 24;
                            var randomAngle = Random.NextDouble() * 2 * Math.PI;
                            var circleX = Math.Cos(randomAngle);
                            var circleY = Math.Sin(randomAngle);
                            var pos = new Point((int)initialSoulPosition.X + (int)(randomDist * circleX), (int)initialSoulPosition.Y + (int)(randomDist * circleY));
                            var movement = new Action<Sparkle>(s =>
                            {
                                var offset = initialSoulPosition - new Vector2(s.Position.X, s.Position.Y);
                                var velocity = Vector2.Multiply(new Vector2(-offset.Y, offset.X), 0.1f);
                                if (activeStage == Stage.Vulnerable)
                                {
                                    velocity -= Vector2.Multiply(new Vector2(offset.X, offset.Y), 0.05f);
                                }
                                else
                                {
                                    velocity += Vector2.Multiply(new Vector2(offset.X, offset.Y), 0.01f);
                                }

                                s.Position = new Point(s.Position.X + (int)Math.Round(velocity.X), s.Position.Y + (int)Math.Round(velocity.Y));
                            });

                            var newSparkle = new Sparkle { Lifespan = lifespan, RemainingLife = lifespan, Position = pos, Movement = movement };
                            this.sparkles[this.sparkles.FirstOrDefault(kvp => !this.sparkles.ContainsKey(kvp.Key + 1)).Key + 1] = newSparkle;
                        }
                    }
                    break;
                case Stage.Vulnerable:
                    this.speed = 1;
                    if (this.waittime % 210 == 0)
                    {
                        for (var i = 0; i < 20; i++)
                        {
                            var lifespan = 32;
                            var initialSoulPosition = this.soulPosition;
                            var randomAngle = Random.NextDouble() * 2 * Math.PI;
                            var circleX = Math.Cos(randomAngle);
                            var circleY = Math.Sin(randomAngle);
                            var pos = new Point((int)initialSoulPosition.X, (int)initialSoulPosition.Y);
                            var velocity = new Vector2((float)(10 * circleX), (float)(10 * circleY));
                            var movement = new Action<Sparkle>(s =>
                            {
                                velocity += new Vector2(0.1f * -velocity.X, 0.1f * -velocity.Y);

                                s.Position = new Point(s.Position.X + (int)Math.Round(velocity.X), s.Position.Y + (int)Math.Round(velocity.Y));
                            });
                            var newSparkle = new Sparkle { Lifespan = lifespan, RemainingLife = lifespan, Position = pos, Movement = movement };
                            this.sparkles[this.sparkles.FirstOrDefault(kvp => !this.sparkles.ContainsKey(kvp.Key + 1)).Key + 1] = newSparkle;
                        }
                    }
                    break;
                case Stage.Enraged:
                    this.SummonStageEnemies(this.enrageStage, true);
                    this.UpdateStage(this.enrageStage);
                    this.enabledChips = 4;
                    this.isActivated = true;
                    return;
            }

            if (this.stage != Stage.Enraged)
            {
                this.stage = activeStage;
            }
            else
            {
                this.enrageStage = activeStage;
            }

            if (this.isActivated)
            {
                base.Updata();
            }
            else
            {
                this.BaseUpdata();
            }
        }

        private void CheckSpecialTriggers()
        {
            if (this.parent.player.mind.MindNow == MindWindow.MIND.smile)
            {
                this.parent.player.mind.MindNow = MindWindow.MIND.normal;
            }
            if (this.stageInitialized && this.parent.player.Hp > 5 && this.stage != Stage.Enraged)
            {
                this.enrageStage = this.stage;
                this.stage = Stage.Enraged;
                this.EndStage(null);
            }

            var panels = Enumerable.Range(0, this.parent.panel.GetLength(0))
                .SelectMany(px => Enumerable.Range(0, this.parent.panel.GetLength(1)).Select(py => new Point(px, py)));
            foreach (var panelPos in panels)
            {
                var panel = this.parent.panel[panelPos.X, panelPos.Y];
                var delay = 60;
                var reverseColor = panel.color == Panel.COLOR.blue ? Panel.COLOR.red : Panel.COLOR.blue;
                switch (panel.State)
                {
                    case Panel.PANEL._grass:
                        if (!this.delayedPanelActions.ContainsKey(panelPos))
                        {
                            this.parent.attacks.Add(new Dummy(this.sound, this.parent, panelPos.X, panelPos.Y, reverseColor, Point.Empty, delay, true));
                            this.delayedPanelActions[panelPos] = new DelayedAction(delay, () =>
                            {
                                var burnTower = new Tower(this.sound, this.parent, panelPos.X, panelPos.Y, reverseColor, 50, -1, ChipBase.ELEMENT.heat);
                                this.parent.attacks.Add(burnTower);
                                panel.state = Panel.PANEL._nomal;
                            });
                        }
                        break;
                    case Panel.PANEL._poison:
                        if (!this.delayedPanelActions.ContainsKey(panelPos))
                        {
                            this.parent.attacks.Add(new Dummy(this.sound, this.parent, panelPos.X, panelPos.Y, reverseColor, Point.Empty, delay, true));
                            this.delayedPanelActions[panelPos] = new DelayedAction(delay, () =>
                            {
                                var poisTower = new Tower(this.sound, this.parent, panelPos.X, panelPos.Y, reverseColor, 50, -1, ChipBase.ELEMENT.poison);
                                this.parent.attacks.Add(poisTower);
                                panel.state = Panel.PANEL._nomal;
                            });
                        }
                        break;
                }
            }

            foreach (var kvp in this.delayedPanelActions.ToList())
            {
                if (kvp.Value.RemainingLife != 0)
                {
                    kvp.Value.RemainingLife--;
                }
                else
                {
                    kvp.Value.Action.Invoke();
                    this.delayedPanelActions.Remove(kvp.Key);
                }
            }
        }

        private Vector2 GetSoulPosition()
        {
            var basePosition = new Vector2((int)this.positionDirect.X + this.Shake.X + SoulOffset.X, (int)this.positionDirect.Y + this.Shake.Y + SoulOffset.Y);

            if (this.isFloatingOverCube)
            {
                return new Vector2(basePosition.X, basePosition.Y - 16);
            }

            return basePosition;
        }

        private void SetDroppedChips()
        {
            ChipFolder chipFolder1 = new ChipFolder(this.sound);
            chipFolder1.SettingChip(1);
            chipFolder1.codeNo = this.Random.Next(4);
            this.dropchips[0] = chipFolder1;
            this.dropchips[1] = chipFolder1;
            this.dropchips[2] = chipFolder1;
            this.dropchips[3] = chipFolder1;
            this.dropchips[4] = chipFolder1;
        }

        private void SummonEnemy(int enemyId, int x, int y, byte rank)
        {
            var openSummonSlot = this.summon1 == null || !this.summon1.flag
                ? (value => this.summon1 = value)
                : (this.summon2 == null || !this.summon2.flag
                    ? (value => this.summon2 = value)
                    : (Action<EnemyBase>)null);

            if (openSummonSlot == null)
            {
                return;
            }

            var closestValidPanel = Enumerable.Range(0, this.parent.panel.GetLength(0))
                .SelectMany(px => Enumerable.Range(0, this.parent.panel.GetLength(1)).Select<int, Point?>(py => new Point(px, py)))
                .Where(p => p != null && this.CheckValidSummonPosition(p.Value.X, p.Value.Y))
                .OrderBy(p => Math.Abs(p.Value.X - x) + Math.Abs(p.Value.Y - y))
                .FirstOrDefault();
            var adjX = closestValidPanel?.X ?? x;
            var adjY = closestValidPanel?.Y ?? y;

            var enemyBase = new EnemyBase(this.sound, this.parent, adjX, adjY, (byte)enemyId, this.union, rank);
            var enemy = EnemyBase.EnemyMake(enemyId, enemyBase, false);
            enemy.Init();
            this.parent.enemys.Add(enemy);
            openSummonSlot.Invoke(enemy);
            enemy.InitAfter();
        }

        private bool CheckValidSummonPosition(int x, int y)
        {
            var notOccupied = !this.parent.OnPanelCheck(x, y, false);
            var alliedSpace = this.parent.panel[x, y].color == this.union;

            return notOccupied && alliedSpace;
        }

        private void EndStage(Stage? activeStage)
        {
            this.Motion = MOTION.neutral;
            this.waittime = 0;
            this.stageInitialized = false;

            if (activeStage != null)
            {
                var activeStageValue = activeStage.Value;
                foreach (var summon in this.Summons)
                {
                    if (StageSummons.ContainsKey(activeStageValue) &&
                        (summon.number == StageSummons[activeStageValue].Summon1.Id ||
                         summon.number == StageSummons[activeStageValue].Summon2.Id))
                    {
                        summon.Hp = 0;
                    }
                }
                this.CheckSummonChange(activeStageValue);
            }
        }

        private int CheckSummonChange(Stage activeStage)
        {
            var liveDeadSummons = this.Summons.Where(s =>
            {
                if (!StageSummons.ContainsKey(activeStage))
                {
                    return false;
                }
                return s.number == StageSummons[activeStage].Summon1.Id || s.number == StageSummons[activeStage].Summon2.Id;
            }).GroupBy(s => s.flag && s.Hp != 0);
            var currentSummonCount = liveDeadSummons.Sum(gr => gr.Key ? gr.Count() : 0);

            if (this.prevSummonCount != currentSummonCount)
            {
                this.prevSummonCount = currentSummonCount;

                var deadSummons = liveDeadSummons.SelectMany(gr => !gr.Key ? (IEnumerable<EnemyBase>)gr : new EnemyBase[0]);
                foreach (var deadSummon in deadSummons)
                {
                    this.AddSummonDeathSparkles(deadSummon.positionDirect);

                    if (deadSummon == this.summon1)
                    {
                        this.summon1 = null;
                    }

                    if (deadSummon == this.summon2)
                    {
                        this.summon2 = null;
                    }
                }

                return currentSummonCount;
            }

            return -1;
        }

        private void AddSummonDeathSparkles(Vector2 summonPosition)
        {
            for (var i = 0; i < 20; i++)
            {
                var lifespan = 32;
                var initialSoulPosition = this.soulPosition;
                var randomAngle = Random.NextDouble() * 2 * Math.PI;
                var circleX = Math.Cos(randomAngle);
                var circleY = Math.Sin(randomAngle);
                var burstSpeed = Random.NextDouble() * 3 + 2;
                var pos = new Point((int)summonPosition.X, (int)summonPosition.Y);
                var velocity = new Vector2((float)(burstSpeed * circleX), (float)(burstSpeed * circleY));
                var movement = new Action<Sparkle>(s =>
                {
                    var progress = (float)s.RemainingLife / s.Lifespan;
                    var burstVelocity = Vector2.Multiply(velocity, (float)Math.Cos(progress * Math.PI));
                    var toSoulVelocity = new Vector2(0.1f * progress * (initialSoulPosition.X - s.Position.X), 0.1f * progress * (initialSoulPosition.Y - s.Position.Y));
                    var finalVelocity = burstVelocity + toSoulVelocity;

                    s.Position = new Point(s.Position.X + (int)Math.Round(finalVelocity.X), s.Position.Y + (int)Math.Round(finalVelocity.Y));
                });
                var newSparkle = new Sparkle { Lifespan = lifespan, RemainingLife = lifespan, Position = pos, Movement = movement };
                this.sparkles[this.sparkles.FirstOrDefault(kvp => !this.sparkles.ContainsKey(kvp.Key + 1)).Key + 1] = newSparkle;
            }
        }

        private enum Stage
        {
            ShellArmor,
            Invis,
            Aura,
            Barriers,
            Guard,
            Vulnerable,
            Enraged
        }

        private class Sparkle
        {
            public int Lifespan { get; set; }
            public int RemainingLife { get; set; }

            public int Frame => 3 - (int)Math.Round(3.0 * this.RemainingLife / this.Lifespan);

            public Point Position { get; set; }
            public Action<Sparkle> Movement { get; set; }
        }

        private class DelayedAction
        {
            public int RemainingLife { get; set; }
            public Action Action { get; set; }

            public DelayedAction(int lifespan, Action action)
            {
                this.RemainingLife = lifespan;
                this.Action = action;
            }
        }

        private class StageSummon
        {
            public Stage? EnrageStage;
            public SummonDefinition Summon1;
            public SummonDefinition Summon2;
        }

        private class SummonDefinition
        {
            public int Id;
            public int X;
            public int Y;
            public byte Rank;
        }
    }
}
