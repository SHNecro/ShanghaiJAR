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
using NSBattle.Character;
using NSObject;

namespace NSEnemy
{
    internal class CirnoBX : NaviBase
    {
        private static readonly Rectangle FullFrameRect = new Rectangle(0, 0, 96, 128);
        private static readonly Vector2 SpriteOffset = new Vector2(20, 52);

        private static readonly Vector2 HPOffset = new Vector2(0, -48);

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

        private Point diveTargetPosition;
        private int diveFramesPerPanel;
        private int diveRestFrames;
        private int diveFeatherHitTime;
        private int diveFeatherDelay;
        private int diveFeatherCount;
        private int diveFeatherSets;
        private bool diveAborted;
        private int powerDiveSmallIceCount;
        private int powerDiveIcicleHitTime;
        private int powerDiveSmallIceLifetime;
        private int powerDiveSmallIceSpawnDelay;
        private int powerDiveSmallIceHp;

        private int crossDiveWarningFrames;
        private int crossDiveInitialDelayFrames;
        private int crossDiveDiagonalPassFrames;
        private int crossDiveCircleBackDelayFrames;
        private int crossDiveEndFlightFrames;
        private int crossDiveEntryFramesBeforeCounter;
        private int crossDiveCounterFrames;
        private bool crossDiveDirectionBottomUp;
        private int crossDiveCenterX;
        private bool crossDiveReverse;

        private int iceCrashDuration;
        private int iceCrashLargeIceHitTime;
        private int iceCrashLargeIceLifetime;
        private int iceCrashLargeIceHp;
        private int iceCrashLargeIceRemnantHp;
        private int iceCrashSmallIceHitTime;
        private int iceCrashSmallIceSpawnDelay;
        private int iceCrashBlockerHitTime;
        private int iceCrashBlockerLifetime;
        private int iceCrashBlockerHp;
        private int powerIceCrashAdditionalLargeIceSpawnDelay;
        private bool iceCrashPowerupFollowup;

        // Swapped with "superspin", which is now regular spin
        private int spinPatternLength;
        private int spinFeatherPatternStayWeight;
        private int spinFeatherPatternMoveWeight;
        private int spinSpinupTime;
        private int spinDuration;
        private int spinFeatherInitialDelay;
        private int spinFeatherMinimumDelay;
        private int spinFeatherPerPanelTime;
        private List<int> spinPattern;
        private int spinPowerupTornadoCounter;
        private int spinPowerupTornadoMoverEvaluationNumber;

        private int superDiveHorizontalDiveSpeed;
        private int superDiveHorizontalDiveMaxTime;
        private int superDiveSwoopCount;
        private int superDiveSwoopMaxTime;
        private int superDiveIceTrailRockDelay;
        private int superDiveIceTrailRockLife;
        private int superDiveIceTrailRockHp;
        private int superDiveIceTrailSpikeDelay;
        private int superDiveSwoopSpeed;
        private double superDiveSwoopIncomingAngleSpeed;
        private double superDiveSwoopOutgoingAngleSpeed;
        private int superDiveSwoopIncomingFrames;
        private int superDiveSwoopOutgoingFrames;
        private int superDiveSwoopFeatherHitTime;
        private int superDiveSwoopFeatherDelay;
        private int superDiveSwoopFeatherSets;
        private int superDiveSwoopFeatherCount;
        private int superDiveImpactMaxTime;
        private Point superDiveTargetPosition;
        private bool superDiveImpactAborted;
        private bool[] superDiveRowHolesHit;

        private int superSpinInitialTrackingMoveTime;
        private int superSpinFinalTrackingMoveTime;
        private int superSpinInitialFeatherPerPanelTime;
        private int superSpinFinalFeatherPerPanelTime;
        private int superSpinPowerupFinalFeatherPerPanelTime;
        private int superSpinInitialFeatherDelay;
        private int superSpinFinalFeatherDelay;
        private double superSpinMinFeatherIntervalFactor;
        private double superSpinMaxFeatherIntervalFactor;
        private int superSpinSpinupTime;
        private int superSpinDuration;
        private DIRECTION superSpinDirection;
        private List<SpinFeather> superSpinFeathers;
        private double superSpinCurrentFeatherIntervalFactor;
        private int superSpinPowerupTornadoDelay;
        private int superSpinPowerupTornadoLifetime;

        private bool isPoweredUp;

        private Dictionary<AttackType, int> standardAttackWeights;
        private Dictionary<AttackType, int> poweredAttackWeights;

        private AttackState attackMotion;
        private AttackType attackType;

        private int attackWaitTime;

        private bool detachedShadow;
        private Vector2 detachedShadowOffset;

        private Point? underAnimationPoint;
        private Vector2 underAnimationOffset;

        public CirnoBX(IAudioEngine s, SceneBattle p, int pX, int pY, byte n, Panel.COLOR u, byte v)
            : base(s, p, pX, pY, n, u, v)
        {
            for (int index = 0; index < this.dropchips.Length; ++index)
                this.dropchips[index] = new ChipFolder(this.sound);

            this.spinPattern = new List<int>();
            this.superSpinFeathers = new List<SpinFeather>();
            this.standardAttackWeights = new Dictionary<AttackType, int>();
            this.poweredAttackWeights = new Dictionary<AttackType, int>();

            this.SetVersionStats();
            this.SetVersionDrops();
            this.SetDynamicAttackWeights();

            this.wide = FullFrameRect.Width;
            this.height = FullFrameRect.Height;
            this.race = EnemyBase.ENEMY.navi;
            this.Flying = true;
            this.hpmax = this.hp;
            this.speed = 2;
            this.hpprint = this.hp;
            this.printhp = true;
            this.effecting = false;

            this.picturename = "cirnobx";

            this.animationpoint = new Point(1, 0);
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

        public override void InitAfter()
        {
            base.InitAfter();
            this.PositionDirectSet();
            this.IdleDelaySet();
        }

        public override void PositionDirectSet()
        {
            this.positionDirect = new Vector2(position.X * 40.0f + 0, position.Y * 24.0f + 0);
        }

        protected override void Moving()
        {
            this.SetDynamicAttackWeights();

            switch (this.Motion)
            {
                case MOTION.neutral:
                    if (this.animationpoint == new Point(2, 0) && this.waittime < 4)
                    {
                        // Allow a movement effect to persist without affecting idle delay
                    }
                    else
                    {
                        this.animationpoint = new Point(1, 0);
                    }
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
                    {
                        var initialPosition = this.position;
                        var initialPositionDirect = this.SpritePositionDirect;
                        this.CommitMoveRandom();
                        this.animationpoint = new Point(2, 0);
                        if (initialPosition != this.position)
                        {
                            this.parent.effects.Add(new StepShadowYuyu(
                                this.sound,
                                this.parent,
                                new Rectangle(FrameCoordX(3), FrameCoordY(0), FullFrameRect.Width, FullFrameRect.Height),
                                initialPositionDirect,
                                this.picturename,
                                this.rebirth,
                                initialPosition,
                                255, 255, 255));
                        }

                        this.Motion = MOTION.neutral;
                    }
                    break;
                case MOTION.knockback:
                    this.ShakeEnd();
                    this.counterTiming = false;
                    this.nohit = false;
                    this.effecting = false;
                    this.printhp = true;
                    this.rend = true;
                    this.overMove = false;
                    this.detachedShadow = false;
                    this.detachedShadowOffset = Vector2.Zero;
                    this.underAnimationPoint = null;
                    this.underAnimationOffset = Vector2.Zero;
                    this.superArmor = false;
                    this.guard = GUARD.none;
                    this.isPoweredUp = false;
                    if (this.positionReserved != null)
                    {
                        this.position = this.positionReserved.Value;
                        this.positionReserved = null;
                    }

                    switch (this.waittime)
                    {
                        case 0:
                            this.animationpoint = new Point(0, 0);
                            this.PositionDirectSet();
                            break;
                        case 9:
                            this.animationpoint = new Point(0, 0);
                            this.positionDirect.Y -= 1;
                            break;
                        case 21:

                            this.Motion = MOTION.neutral;
                            break;
                    }

                    break;
                case MOTION.attack:
                    switch (this.AttackMotion)
                    {
                        case AttackState.Idle:
                            this.AttackMotion = AttackState.Attack;
                            var bins = !this.isPoweredUp ? this.standardAttackWeights : this.poweredAttackWeights;
                            var draw = this.Random.Next(bins.Sum(b => b.Value));
                            this.attackType = bins.Select((b, i) => Tuple.Create(b.Key, bins.Take(i + 1).Sum(bb => bb.Value))).FirstOrDefault(b => b.Item2 > draw)?.Item1 ?? AttackType.Dive;
                            this.attackWaitTime = 0;
                            break;
                        case AttackState.Attack:
                            switch (this.attackType)
                            {
                                case AttackType.Dive:
                                    if (this.attackWaitTime == 0)
                                    {
                                        // idle shuttered
                                        this.animationpoint = new Point(2, 0);
                                    }
                                    else if (this.attackWaitTime == 4)
                                    {
                                        // takeoff shuttered
                                        this.animationpoint = new Point(0, 3);
                                    }
                                    else if (this.attackWaitTime == 8)
                                    {
                                        // takeoff
                                        this.animationpoint = new Point(1, 3);
                                        this.counterTiming = true;
                                    }
                                    else if (this.attackWaitTime == 13)
                                    {
                                        // leg motion
                                        this.animationpoint = new Point(2, 3);
                                    }
                                    else if (this.attackWaitTime == 18)
                                    {
                                        this.counterTiming = false;
                                        // leg out, detach shadow for bobbing
                                        this.animationpoint = new Point(3, 3);
                                        this.detachedShadow = true;
                                        this.detachedShadowOffset = new Vector2(0, 0);
                                    }
                                    else if (this.attackWaitTime == 23)
                                    {
                                        // main sprite dips down 1px
                                        this.positionDirect.Y += 1;

                                        this.parent.attacks.Add(new DiveFeatherSpawner(this.sound, this.parent, this.union, this.power / 4, this.diveFeatherHitTime, this.diveFeatherDelay, this.diveFeatherSets, this.diveFeatherCount, this.element));
                                    }
                                    else if (this.attackWaitTime == 28)
                                    {
                                        // main sprite returns 1px
                                        this.positionDirect.Y -= 1;
                                    }
                                    else if (this.attackWaitTime == 33)
                                    {
                                        // main sprite dips down 1px
                                        this.positionDirect.Y += 1;
                                    }
                                    else if (this.attackWaitTime == 38)
                                    {
                                        // dive begins, sprite offset resets
                                        this.animationpoint = new Point(4, 3);
                                        this.PositionDirectSet();
                                        this.positionReserved = this.position;

                                        var targetedEnemyPosition = this.RandomTarget();
                                        var targets = this.parent.AllChara().Where(c => c.union != this.union);
                                        if (targets.Any())
                                        {
                                            var selectedEnemyIndex = Random.Next(0, targets.Count());
                                            targetedEnemyPosition = targets.Skip(selectedEnemyIndex).First().position;
                                        }

                                        if (this.union == Panel.COLOR.blue && targetedEnemyPosition.X >= this.position.X)
                                        {
                                            targetedEnemyPosition = new Point(0, this.position.Y);
                                        }
                                        else if (this.union == Panel.COLOR.red && targetedEnemyPosition.X <= this.position.X)
                                        {
                                            targetedEnemyPosition = new Point(5, this.position.Y);
                                        }

                                        // extrapolate to edge of field
                                        var offsetX = (double)(targetedEnemyPosition.X - this.position.X);
                                        var offsetY = (double)(targetedEnemyPosition.Y - this.position.Y);
                                        var xRepeatsUntilBorderHit = this.union == Panel.COLOR.blue
                                            ? (0 - this.position.X) / offsetX
                                            : (5 - this.position.X) / offsetX;
                                        var yRepeatsUntilBorderHit = offsetY < 0
                                            ? (0 - this.position.Y) / offsetY
                                            : (2 - this.position.Y) / offsetY;
                                        xRepeatsUntilBorderHit = double.IsNaN(xRepeatsUntilBorderHit) ? double.PositiveInfinity : xRepeatsUntilBorderHit;
                                        yRepeatsUntilBorderHit = double.IsNaN(yRepeatsUntilBorderHit) ? double.PositiveInfinity : yRepeatsUntilBorderHit;
                                        var repeatsUntilBorderHit = Math.Min(xRepeatsUntilBorderHit, yRepeatsUntilBorderHit);
                                        var edgeRoundingFunc = this.union == Panel.COLOR.blue ? (Func<double, double>)Math.Floor : Math.Ceiling;
                                        this.diveTargetPosition = new Point(
                                            (int)edgeRoundingFunc(this.position.X + offsetX * repeatsUntilBorderHit),
                                            (int)Math.Round(this.position.Y + offsetY * repeatsUntilBorderHit));

                                        // Mark target position? too short?
                                        this.parent.attacks.Add(new Dummy(this.sound, this.parent, this.diveTargetPosition.X, this.diveTargetPosition.Y, this.union, new Point(0, 0), this.CalculateDiveToTargetFrames(), true));

                                        // Turn on hit for on-path panels
                                        this.effecting = true;
                                    }
                                    else if (this.attackWaitTime > 38)
                                    {
                                        var diveToTargetFrames = this.CalculateDiveToTargetFrames();

                                        // Dive handling
                                        if (this.attackWaitTime < 38 + diveToTargetFrames)
                                        {
                                            var t = (this.attackWaitTime - 38.0) / diveToTargetFrames;
                                            var diveProgress = (float)(t * t); // still ranges [0, 1]
                                            var diveStartPositionDirect = new Vector2(this.positionReserved.Value.X * 40.0f + 0, this.positionReserved.Value.Y * 24.0f + 0);
                                            var panelEdgeAdjustment = (32.0f * this.UnionRebirth(this.UnionEnemy));
                                            var endPositionDirect = new Vector2(this.diveTargetPosition.X * 40.0f + panelEdgeAdjustment, this.diveTargetPosition.Y * 24.0f + 32.0f);

                                            this.positionDirect = diveStartPositionDirect + (endPositionDirect - diveStartPositionDirect) * diveProgress;

                                            var groundOffset = 32 * diveProgress;
                                            this.detachedShadowOffset = new Vector2(0, -groundOffset);

                                            var panelPosition = new Vector2((this.positionDirect.X - panelEdgeAdjustment) / 40, (this.positionDirect.Y - groundOffset) / 24);
                                            var roundingFunc = this.union == Panel.COLOR.red ? (Func<double, double>)Math.Floor : Math.Ceiling;
                                            this.position = new Point((int)roundingFunc(panelPosition.X), (int)Math.Round(panelPosition.Y));

                                            var dir = default(DIRECTION?);
                                            const double PanelDragAllowance = 0.45;
                                            var xDiff = this.position.X - panelPosition.X;
                                            var yDiff = this.position.Y - panelPosition.Y;
                                            if (Math.Sqrt(xDiff * xDiff + yDiff * yDiff) < PanelDragAllowance)
                                            {
                                                // simple calculation, only drag if on main path
                                                var xDiffEnd = this.diveTargetPosition.X - this.positionReserved.Value.X;
                                                var yDiffEnd = this.diveTargetPosition.Y - this.positionReserved.Value.Y;
                                                var xDiffAbs = Math.Abs(xDiffEnd);
                                                var yDiffAbs = Math.Abs(yDiffEnd);
                                                if (xDiffAbs < yDiffAbs)
                                                {
                                                    dir = yDiff > 0 ? DIRECTION.down : DIRECTION.up;
                                                }
                                                else // include exact diagonal, default to push
                                                {
                                                    var direction = this.union != Panel.COLOR.blue ? DIRECTION.right : DIRECTION.left;
                                                    dir = direction;
                                                }
                                            }

                                            if (dir != null)
                                            {
                                                this.DiveDragAttackMake(this.Power / 4, dir.Value);
                                            }
                                        }
                                        else if (this.attackWaitTime == 38 + diveToTargetFrames)
                                        {
                                            var panelEdgeAdjustment = (32.0f * this.UnionRebirth(this.UnionEnemy));
                                            var endPositionDirect = new Vector2(this.diveTargetPosition.X * 40.0f + panelEdgeAdjustment, this.diveTargetPosition.Y * 24.0f + 32.0f);

                                            this.positionDirect = endPositionDirect;
                                            var roundingFunc = this.union == Panel.COLOR.red ? (Func<double, double>)Math.Floor : Math.Ceiling;
                                            this.position = this.diveTargetPosition.WithOffset(this.UnionRebirth(this.UnionEnemy), 0);

                                            if (this.parent.panel[this.position.X, this.position.Y].Hole)
                                            {
                                                this.diveAborted = true;
                                            }
                                            else
                                            {
                                                this.sound.PlaySE(SoundEffect.bomb);

                                                this.detachedShadowOffset = new Vector2(0, -32);

                                                this.DiveDragAttackMake(this.Power / 4, this.union == Panel.COLOR.blue ? DIRECTION.left : DIRECTION.right);
                                                this.effecting = false;

                                                var slamAttack = new BombAttack(this.sound, this.parent, this.diveTargetPosition.X, this.diveTargetPosition.Y, this.union, this.Power, 1, this.element)
                                                {
                                                    invincibility = false
                                                };
                                                slamAttack.BadStatusSet(BADSTATUS.paralyze, 45);
                                                slamAttack.BadStatusSet(BADSTATUS.stop, 45);
                                                this.parent.attacks.Add(slamAttack);

                                                this.parent.effects.Add(new DiveBomber(this.sound, this.parent, this.diveTargetPosition));

                                                if (this.isPoweredUp)
                                                {
                                                    var targets = this.RandomMultiPanel(this.powerDiveSmallIceCount, this.UnionEnemy);

                                                    var smallIceIcicleSpawner = new AttackSpawner(
                                                        this.sound,
                                                        this.parent,
                                                        this.union,
                                                        f => f >= this.powerDiveSmallIceCount * this.powerDiveSmallIceSpawnDelay,
                                                        f =>
                                                        {
                                                            if (f % this.powerDiveSmallIceSpawnDelay == 0)
                                                            {
                                                                var target = targets[f / this.powerDiveSmallIceSpawnDelay];
                                                                var icicle = new IceSpikeBX(this.sound, this.parent, target.X, target.Y, this.union, this.power, this.powerDiveIcicleHitTime, this.element, true, this.powerDiveSmallIceLifetime, this.powerDiveSmallIceHp);
                                                                this.parent.attacks.Add(icicle);
                                                            }
                                                        });

                                                    this.parent.attacks.Add(smallIceIcicleSpawner);

                                                    this.parent.ShakeStart(8, 8);
                                                    this.isPoweredUp = false;
                                                }
                                            }
                                        }
                                        else if (this.attackWaitTime >= 38 + diveToTargetFrames + this.diveRestFrames || this.diveAborted)
                                        {
                                            this.parent.effects.Add(new MoveEnemy(this.sound, this.parent, this.position.X, this.position.Y));

                                            this.position = this.positionReserved.Value;
                                            this.positionReserved = null;
                                            this.detachedShadow = false;
                                            this.effecting = false;
                                            this.HitFlagReset();
                                            this.diveAborted = false;

                                            this.PositionDirectSet();
                                            this.AttackMotion = AttackState.Cooldown;

                                            this.AttackCooldownSet();
                                        }
                                    }
                                    break;
                                case AttackType.CrossDive:
                                    if (!this.crossDiveReverse)
                                    {
                                        if (this.attackWaitTime == 0)
                                        {
                                            this.printhp = false;
                                            this.rend = false;
                                            this.nohit = true;
                                            this.overMove = true;
                                            this.superArmor = true;

                                            this.positionReserved = this.position;

                                            this.parent.effects.Add(new MoveEnemy(this.sound, this.parent, this.position.X, this.position.Y));
                                        }
                                        else if (this.attackWaitTime < this.crossDiveInitialDelayFrames)
                                        {
                                            // Wait before 1st pass
                                        }
                                        else if (this.attackWaitTime == this.crossDiveInitialDelayFrames)
                                        {
                                            // Set initial 1st pass position (next steps work by offset)
                                            var target = this.RandomTarget();

                                            if (this.union == Panel.COLOR.blue)
                                            {
                                                this.crossDiveCenterX = target.X < 3 ? 1 : target.X;
                                            }
                                            else
                                            {
                                                this.crossDiveCenterX = target.X > 2 ? 4 : target.X;
                                            }

                                            var bottomUpIfEligible = Random.Next() % 2 == 0;
                                            for (var y = 0; y < 3; y++)
                                            {
                                                var tiltLeft = bottomUpIfEligible ^ this.union == Panel.COLOR.blue;
                                                var xOffForY = (1 - y) * (tiltLeft ? 1 : -1);
                                                var targetMissed = true;
                                                for (var xOff = -1; xOff < 2; xOff++)
                                                {
                                                    var point = new Point(this.crossDiveCenterX + xOff + xOffForY, y);
                                                    if (point == target)
                                                    {
                                                        targetMissed = false;
                                                        break;
                                                    }
                                                }
                                                bottomUpIfEligible ^= targetMissed;
                                            }

                                            this.crossDiveDirectionBottomUp = bottomUpIfEligible;
                                            this.animationpoint = this.crossDiveDirectionBottomUp ? new Point(2, 2) : new Point(3, 2);


                                            var targetPositionDirect = new Vector2(40 * this.crossDiveCenterX, 24 * 1);
                                            var angle = (float)((this.union == Panel.COLOR.blue
                                                ? Math.PI - Math.Atan(24.0 / 40.0)
                                                : Math.Atan(24.0 / 40.0)));
                                            if (!this.crossDiveDirectionBottomUp)
                                            {
                                                angle = (float)(Math.PI * 2 - angle);
                                            }

                                            var diagonalPassPixels = Math.Sqrt((24 * 3 * 24 * 3) + (40 * 3 * 40 * 3));
                                            var pixelsPerFrame = diagonalPassPixels / this.crossDiveDiagonalPassFrames;
                                            var pixelsFromTargetPosition = pixelsPerFrame * (this.crossDiveDiagonalPassFrames * 0.5 + this.crossDiveWarningFrames);
                                            this.positionDirect = new Vector2(
                                                (float)(targetPositionDirect.X + Math.Cos(angle) * -pixelsFromTargetPosition),
                                                (float)(targetPositionDirect.Y + Math.Sin(angle) * pixelsFromTargetPosition));

                                            // create warning flashes
                                            for (var y = 0; y < 3; y++)
                                            {
                                                var tiltLeft = this.crossDiveDirectionBottomUp ^ this.union == Panel.COLOR.blue;
                                                var xOffForY = (1 - y) * (tiltLeft ? 1 : -1);
                                                for (var xOff = -1; xOff < 2; xOff++)
                                                {
                                                    var point = new Point(this.crossDiveCenterX + xOff + xOffForY, y);
                                                    if (this.PositionOver(point) || this.parent.panel[point.X, point.Y].color == this.union)
                                                    {
                                                        continue;
                                                    }

                                                    this.parent.attacks.Add(new Dummy(this.sound, this.parent, point.X, point.Y, this.union, Point.Empty, this.crossDiveWarningFrames, true));
                                                }
                                            }

                                            this.printhp = true;
                                            this.rend = true;
                                        }
                                        else if (this.attackWaitTime < this.crossDiveInitialDelayFrames + this.crossDiveWarningFrames + this.crossDiveDiagonalPassFrames + this.crossDiveCircleBackDelayFrames)
                                        {
                                            // Offset position
                                            var angle = (float)((this.union == Panel.COLOR.blue
                                                ? Math.PI - Math.Atan(24.0 / 40.0)
                                                : Math.Atan(24.0 / 40.0)));
                                            if (!this.crossDiveDirectionBottomUp)
                                            {
                                                angle = (float)(Math.PI * 2 - angle);
                                            }

                                            var diagonalPassPixels = Math.Sqrt((24 * 3 * 24 * 3) + (40 * 3 * 40 * 3));
                                            var pixelsPerFrame = diagonalPassPixels / this.crossDiveDiagonalPassFrames;
                                            this.positionDirect += new Vector2(
                                                (float)(Math.Cos(angle) * pixelsPerFrame),
                                                (float)(Math.Sin(angle) * -pixelsPerFrame));
                                            var yPosition = (int)Math.Round(this.positionDirect.Y / 24);
                                            var tiltLeft = this.crossDiveDirectionBottomUp ^ this.union == Panel.COLOR.blue;
                                            var xOffForY = (1 - yPosition) * (tiltLeft ? 1 : -1);
                                            this.position = new Point(this.crossDiveCenterX + xOffForY, yPosition);
                                            this.effecting = true;

                                            this.nohit = (this.positionDirect.Y < 0 || this.positionDirect.Y > 24 * 3);
                                            this.detachedShadow = !this.nohit;
                                            this.detachedShadowOffset = new Vector2(0, 0);

                                            var diagonalPassStart = this.crossDiveInitialDelayFrames + this.crossDiveWarningFrames;

                                            // counter ignores already-present attacks, must hit just after entering
                                            this.counterTiming = this.attackWaitTime > diagonalPassStart + this.crossDiveEntryFramesBeforeCounter
                                                && this.attackWaitTime <= diagonalPassStart + this.crossDiveEntryFramesBeforeCounter + this.crossDiveCounterFrames;

                                            if (this.attackWaitTime > this.crossDiveInitialDelayFrames + this.crossDiveWarningFrames + this.crossDiveDiagonalPassFrames)
                                            {
                                                this.effecting = false;
                                            }

                                            var previousYPositionDirect = this.positionDirect.Y - (float)(Math.Sin(angle) * -pixelsPerFrame);
                                            var previousYPosition = (int)Math.Round(previousYPositionDirect / 24);

                                            if (previousYPosition == 1 && yPosition != 1)
                                            {
                                                this.sound.PlaySE(SoundEffect.futon);
                                            }

                                            if (this.isPoweredUp)
                                            {
                                                if (previousYPosition == 1 && yPosition != 1)
                                                {
                                                    Func<BouzuTornado, Point> centerTargeting = t =>
                                                    {
                                                        t.flag = t.waittime++ < 8;
                                                        return new Point(this.crossDiveCenterX, 1);
                                                    };
                                                    var centerTornado = new BouzuTornado(this.sound, this.parent, this.crossDiveCenterX, 1, this.union, this.power, this.element, -1, 1, 0, false, centerTargeting);
                                                    centerTornado.InitAfter();
                                                    
                                                    var leftTornado = new BouzuTornado(this.sound, this.parent, this.crossDiveCenterX, 1, this.union, this.power, this.element, -1, 5, 0, false, this.CreateTornadoOrbitTargetingFunc(!this.crossDiveDirectionBottomUp, true, new Point(this.crossDiveCenterX, 1), t => t.waittime++ < 9 && centerTornado.flag));
                                                    leftTornado.InitAfter();
                                                    
                                                    var rightTornado = new BouzuTornado(this.sound, this.parent, this.crossDiveCenterX, 1, this.union, this.power, this.element, -1, 5, 0, false, this.CreateTornadoOrbitTargetingFunc(!this.crossDiveDirectionBottomUp, false, new Point(this.crossDiveCenterX, 1), t => t.waittime++ < 9 && centerTornado.flag));
                                                    rightTornado.InitAfter();

                                                    this.parent.attacks.Add(centerTornado);
                                                    this.parent.attacks.Add(leftTornado);
                                                    this.parent.attacks.Add(rightTornado);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            // Set initial position for 2nd pass, flip for reverse
                                            var targetPositionDirect = new Vector2(40 * this.crossDiveCenterX, 24 * 1);
                                            var angle = (float)((this.union == Panel.COLOR.blue
                                                ? Math.Atan(24.0 / 40.0)
                                                : Math.PI - Math.Atan(24.0 / 40.0)));
                                            if (!this.crossDiveDirectionBottomUp)
                                            {
                                                angle = (float)(Math.PI * 2 - angle);
                                            }

                                            var diagonalPassPixels = Math.Sqrt((24 * 3 * 24 * 3) + (40 * 3 * 40 * 3));
                                            var pixelsPerFrame = diagonalPassPixels / this.crossDiveDiagonalPassFrames;
                                            var pixelsFromTargetPosition = pixelsPerFrame * (this.crossDiveDiagonalPassFrames * 0.5 + this.crossDiveWarningFrames);
                                            this.positionDirect = new Vector2(
                                                (float)(targetPositionDirect.X + Math.Cos(angle) * -pixelsFromTargetPosition),
                                                (float)(targetPositionDirect.Y + Math.Sin(angle) * pixelsFromTargetPosition));

                                            // create warning flashes
                                            for (var y = 0; y < 3; y++)
                                            {
                                                var tiltLeft = this.crossDiveDirectionBottomUp ^ this.union == Panel.COLOR.blue;
                                                var xOffForY = (1 - y) * (tiltLeft ? -1 : 1);
                                                for (var xOff = -1; xOff < 2; xOff++)
                                                {
                                                    var point = new Point(this.crossDiveCenterX + xOff + xOffForY, y);
                                                    if (this.PositionOver(point) || this.parent.panel[point.X, point.Y].color == this.union)
                                                    {
                                                        continue;
                                                    }

                                                    this.parent.attacks.Add(new Dummy(this.sound, this.parent, point.X, point.Y, this.union, Point.Empty, this.crossDiveWarningFrames, true));
                                                }
                                            }

                                            this.crossDiveReverse = true;
                                            this.attackWaitTime = 0;
                                            this.HitFlagReset();
                                        }
                                    }
                                    else
                                    {
                                        // starts at warningframes
                                        if (this.attackWaitTime < this.crossDiveWarningFrames + this.crossDiveDiagonalPassFrames + this.crossDiveEndFlightFrames)
                                        {
                                            // Offset position from 2nd pass initial position
                                            var angle = (float)((this.union == Panel.COLOR.blue
                                                ? Math.Atan(24.0 / 40.0)
                                                : Math.PI - Math.Atan(24.0 / 40.0)));
                                            if (!this.crossDiveDirectionBottomUp)
                                            {
                                                angle = (float)(Math.PI * 2 - angle);
                                            }

                                            var diagonalPassPixels = Math.Sqrt((24 * 3 * 24 * 3) + (40 * 3 * 40 * 3));
                                            var pixelsPerFrame = diagonalPassPixels / this.crossDiveDiagonalPassFrames;
                                            this.positionDirect += new Vector2(
                                                (float)(Math.Cos(angle) * pixelsPerFrame),
                                                (float)(Math.Sin(angle) * -pixelsPerFrame));
                                            var yPosition = (int)Math.Round(this.positionDirect.Y / 24);
                                            var tiltLeft = this.crossDiveDirectionBottomUp ^ this.union == Panel.COLOR.blue;
                                            var xOffForY = (1 - yPosition) * (tiltLeft ? -1 : 1);
                                            this.position = new Point(this.crossDiveCenterX + xOffForY, yPosition);
                                            this.effecting = true;

                                            this.nohit = (this.positionDirect.Y < 0 || this.positionDirect.Y > 24 * 3);
                                            this.detachedShadow = !this.nohit;
                                            this.detachedShadowOffset = new Vector2(0, 0);

                                            var previousYPositionDirect = this.positionDirect.Y - (float)(Math.Sin(angle) * -pixelsPerFrame);
                                            var previousYPosition = (int)Math.Round(previousYPositionDirect / 24);
                                            if (previousYPosition == 1 && yPosition != 1)
                                            {
                                                this.sound.PlaySE(SoundEffect.futon);
                                            }

                                            if (this.isPoweredUp)
                                            {
                                                // arm rotation constant, handled from initialization, only handle clearing powerup
                                                if (previousYPosition == 1 && yPosition != 1)
                                                {
                                                    this.isPoweredUp = false;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            // Return to position, reset flags
                                            this.parent.effects.Add(new MoveEnemy(this.sound, this.parent, this.positionDirect + SpriteOffset, new Point(0, 0)));

                                            this.nohit = false;
                                            this.effecting = false;
                                            this.overMove = false;
                                            this.superArmor = false;
                                            this.position = this.positionReserved.Value;
                                            this.positionReserved = null;
                                            this.AttackMotion = AttackState.Cooldown;
                                            this.AttackCooldownSet();
                                            this.crossDiveReverse = false;
                                            this.isPoweredUp = false;
                                            this.PositionDirectSet();
                                            this.HitFlagReset();
                                        }
                                    }

                                    if (this.effecting)
                                    {
                                        var trailingX = this.crossDiveReverse ^ this.union == Panel.COLOR.red ? -1 : 1;
                                        var trailingY = this.crossDiveDirectionBottomUp ? 1 : -1;
                                        var offsets = new List<Point>
                                        {
                                            new Point(0, 0),
                                            new Point(trailingX, 0),
                                            new Point(0, trailingY),
                                            new Point(trailingX, trailingY)
                                        };

                                        foreach (var offset in offsets)
                                        {
                                            var point = this.position.WithOffset(offset);
                                            if (this.PositionOver(point) || this.parent.panel[point.X, point.Y].color == this.union)
                                            {
                                                continue;
                                            }

                                            this.AttackMake(this.Power, offset.X, offset.Y, true);
                                        }
                                    }

                                    break;
                                case AttackType.IceCrash:
                                    switch (this.attackWaitTime)
                                    {
                                        case 0:
                                            this.animationpoint = new Point(0, 4);
                                            this.positionDirect.Y += 1;
                                            this.detachedShadowOffset = new Vector2(0, -1);
                                            this.detachedShadow = true;
                                            this.counterTiming = true;
                                            break;
                                        case 5:
                                            this.positionDirect.Y -= 1;
                                            this.detachedShadowOffset.Y += 1;
                                            break;
                                        case 10:
                                            this.positionDirect.Y += 1;
                                            this.detachedShadowOffset.Y -= 1;
                                            break;
                                        case 15:
                                            this.positionDirect.Y -= 1;
                                            this.detachedShadowOffset.Y += 1;
                                            break;
                                        case 20:
                                            this.animationpoint = new Point(1, 4);
                                            this.detachedShadow = false;
                                            break;
                                        case 24:
                                            this.animationpoint = new Point(2, 4);
                                            this.counterTiming = false;
                                            
                                            this.parent.attacks.Add(new IceCrashSpawner(
                                                this.sound,
                                                this.parent,
                                                this.union,
                                                this.power,
                                                this.element,
                                                this.iceCrashDuration,
                                                this.iceCrashLargeIceHitTime,
                                                this.iceCrashLargeIceLifetime,
                                                this.iceCrashLargeIceHp,
                                                this.iceCrashLargeIceRemnantHp,
                                                this.iceCrashSmallIceHitTime,
                                                this.iceCrashSmallIceSpawnDelay,
                                                !this.isPoweredUp ? -1 : this.powerIceCrashAdditionalLargeIceSpawnDelay));
                                            this.iceCrashPowerupFollowup = !this.isPoweredUp;
                                            this.isPoweredUp = false;
                                            break;
                                        case 60:
                                        case 90:
                                        case 120:
                                        case 150:
                                            {
                                                var initialPosition = this.position;
                                                var initialPositionDirect = this.SpritePositionDirect;

                                                this.MoveRandom(false, false);
                                                for (var i_retry = 0; i_retry < 6; i_retry++)
                                                {
                                                    if ((this.union == Panel.COLOR.red || this.positionre.X > 3)
                                                        && (this.union == Panel.COLOR.blue || this.positionre.X < 2))
                                                    {
                                                        break;
                                                    }
                                                    this.MoveRandom(false, false);
                                                }
                                                this.position = this.positionre;
                                                this.PositionDirectSet();

                                                this.animationpoint = new Point(2, 0);
                                                if (initialPosition != this.position)
                                                {
                                                    this.parent.effects.Add(new StepShadowYuyu(
                                                        this.sound,
                                                        this.parent,
                                                        new Rectangle(FrameCoordX(3), FrameCoordY(0), FullFrameRect.Width, FullFrameRect.Height),
                                                        initialPositionDirect,
                                                        this.picturename,
                                                        this.rebirth,
                                                        initialPosition,
                                                        255, 255, 255));
                                                }
                                            }
                                            break;
                                        case 64:
                                        case 94:
                                        case 124:
                                        case 154:
                                            this.animationpoint = new Point(1, 0);
                                            break;
                                        case 65:
                                        case 85:
                                        case 105:
                                            {
                                                var blockerRow = (this.attackWaitTime - 65) / 20;
                                                var blockerColumn = this.union == Panel.COLOR.blue ? 3 : 2;
                                                this.parent.attacks.Add(new IceSpikeBX(this.sound, this.parent, blockerColumn, blockerRow, this.union, this.power, this.iceCrashBlockerHitTime, this.element, true, this.iceCrashBlockerLifetime, this.iceCrashBlockerHp));
                                            }
                                            break;
                                        case 180:
                                            if (this.iceCrashPowerupFollowup)
                                            {
                                                this.attackType = AttackType.PowerUp;
                                                this.attackWaitTime = 0;
                                            }
                                            else
                                            {
                                                this.AttackMotion = AttackState.Cooldown;
                                                this.AttackCooldownSet();
                                            }
                                            break;
                                    }
                                    break;
                                case AttackType.Spin:
                                    {
                                        const int spinPoseFrames = 6;
                                        const int spinSwipeFrames = 4;
                                        if (this.attackWaitTime < this.superSpinSpinupTime)
                                        {
                                            // Move to center, animate and create pattern
                                            if (this.attackWaitTime <= 10 + spinPoseFrames + spinSwipeFrames)
                                            {
                                                switch (this.attackWaitTime)
                                                {
                                                    case 0:
                                                        {
                                                            var initialPosition = this.position;
                                                            var initialPositionDirect = this.SpritePositionDirect;

                                                            var spinPosition = this.union == Panel.COLOR.blue ? new Point(4, 1) : new Point(1, 1);
                                                            if (this.parent.panel[spinPosition.X, spinPosition.Y].color != this.union || this.parent.panel[spinPosition.X, spinPosition.Y].Hole)
                                                            {
                                                                spinPosition = this.union == Panel.COLOR.blue ? new Point(5, 1) : new Point(0, 1);
                                                            }

                                                            if (this.parent.panel[spinPosition.X, spinPosition.Y].color != this.union || this.parent.panel[spinPosition.X, spinPosition.Y].Hole)
                                                            {
                                                                this.MoveRandom(false, false);
                                                                for (var i_retry = 0; i_retry < 6; i_retry++)
                                                                {
                                                                    if ((this.union == Panel.COLOR.red || this.positionre.X > 3)
                                                                        && (this.union == Panel.COLOR.blue || this.positionre.X < 2)
                                                                        && !this.parent.panel[this.positionre.X, this.positionre.Y].Hole)
                                                                    {
                                                                        spinPosition = this.positionre;
                                                                        break;
                                                                    }
                                                                    this.MoveRandom(false, false);
                                                                }
                                                            }
                                                            this.positionre = spinPosition;

                                                            this.position = this.positionre;
                                                            this.positionReserved = this.position;
                                                            this.PositionDirectSet();

                                                            this.animationpoint = new Point(2, 0);
                                                            if (initialPosition != this.position)
                                                            {
                                                                this.parent.effects.Add(new StepShadowYuyu(
                                                                    this.sound,
                                                                    this.parent,
                                                                    new Rectangle(FrameCoordX(3), FrameCoordY(0), FullFrameRect.Width, FullFrameRect.Height),
                                                                    initialPositionDirect,
                                                                    this.picturename,
                                                                    this.rebirth,
                                                                    initialPosition,
                                                                    255, 255, 255));
                                                            }
                                                            break;
                                                        }
                                                    case 4:
                                                        this.animationpoint = new Point(1, 0);
                                                        break;
                                                    case 10:
                                                        this.animationpoint = new Point(0, 5);
                                                        this.counterTiming = true;
                                                        break;
                                                    case 10 + spinPoseFrames:
                                                        this.animationpoint = new Point(1, 5);
                                                        break;
                                                    case 10 + spinSwipeFrames + spinPoseFrames:
                                                        this.animationpoint = new Point(2, 5);
                                                        this.counterTiming = false;
                                                        this.guard = GUARD.guard;
                                                        this.superSpinDirection = DIRECTION.up;
                                                        this.superSpinFeathers.Clear();

                                                        this.superSpinCurrentFeatherIntervalFactor = this.superSpinMinFeatherIntervalFactor + this.Random.NextDouble() * (this.superSpinMaxFeatherIntervalFactor - this.superSpinMinFeatherIntervalFactor);
                                                        break;
                                                }
                                            }
                                            else
                                            {
                                                // Start with slow spin, accelerate
                                                var spinStartTime = this.attackWaitTime - (10 + spinPoseFrames + spinSwipeFrames);

                                                if (this.isPoweredUp && spinStartTime == this.superSpinPowerupTornadoDelay)
                                                {
                                                    var tornadoRow = this.Random.Next() % 2 == 0 ? 0 : 2;
                                                    var tornadoStartColumn = this.union == Panel.COLOR.red ? 0 : 5;
                                                    var tornadoEndColumn = this.union == Panel.COLOR.red ? 3 : 2;
                                                    var tornadoLifetime = this.superSpinPowerupTornadoLifetime;
                                                    Func<BouzuTornado, Point> tornadoTargeting = t =>
                                                    {
                                                        t.flag = t.waittime++ < tornadoLifetime;
                                                        return t.waittime < tornadoLifetime / 2 ? new Point(tornadoEndColumn, tornadoRow) : new Point(tornadoStartColumn, tornadoRow);
                                                    };
                                                    var deflectingTornado = new BouzuTornado(this.sound, this.parent, tornadoStartColumn, tornadoRow, this.union, this.Power, this.element, -1, 5, 1, false, tornadoTargeting);

                                                    this.parent.attacks.Add(deflectingTornado);
                                                }

                                                const int minFrameDelay = 2;
                                                const int initialFrameDelay = 6;
                                                Func<int, int, int, int> interpolate = (initial, final, t) => (int)(Math.Round(initial - (initial - final) * ((double)t / this.superSpinSpinupTime)));
                                                Func<int, int> calculateFrameDelay = t => interpolate(initialFrameDelay, minFrameDelay, t);

                                                var animProgress = 0;
                                                while (animProgress < spinStartTime)
                                                {
                                                    animProgress += calculateFrameDelay(animProgress);
                                                }

                                                if (animProgress == spinStartTime)
                                                {
                                                    var spinFrame = ((this.animationpoint.X - 2) + 1) % 3;
                                                    this.animationpoint = new Point(spinFrame + 2, 5);
                                                }

                                                var roundingFunc = this.superSpinDirection == DIRECTION.up ? (Func<double, double>)Math.Ceiling : Math.Floor;
                                                var settledRow = (int)Math.Max(0, Math.Min(2, roundingFunc(this.positionDirect.Y / 24.0)));

                                                var acceleratingMoveSpeed = interpolate(this.superSpinInitialTrackingMoveTime, this.superSpinFinalTrackingMoveTime, spinStartTime);

                                                var spriteOffset = 24.0f / acceleratingMoveSpeed * (this.superSpinDirection == DIRECTION.up ? -1 : 1);
                                                var newPosition = new Point((int)Math.Round(this.positionDirect.X / 40), (int)Math.Round(this.positionDirect.Y / 24));
                                                this.positionDirect.Y += spriteOffset;
                                                this.position = newPosition;

                                                foreach (var feather in this.superSpinFeathers)
                                                {
                                                    var freshlyMoving = !feather.HasShadow && feather.IsThrown;
                                                    if (!feather.IsThrown)
                                                    {
                                                        feather.positionDirect.Y += spriteOffset;
                                                        feather.position = newPosition;
                                                    }
                                                    else if (freshlyMoving)
                                                    {
                                                        feather.positionDirect = new Vector2(feather.position.X * 40.0f + 0, feather.position.Y * 24.0f + 0);
                                                        feather.HasShadow = true;
                                                    }

                                                    if (feather.DeflectDirection == null && this.parent.AllObjects().OfType<BouzuTornado>().Any(bt => bt.position == feather.position && bt.union == feather.union))
                                                    {
                                                        feather.DeflectDirection = feather.position.Y == 0 ? DIRECTION.down : DIRECTION.up;
                                                        this.sound.PlaySE(SoundEffect.chain);
                                                    }
                                                }

                                                var nextRow = settledRow + (this.superSpinDirection == DIRECTION.up ? -1 : 1);
                                                if (nextRow < 0 || nextRow >= 3
                                                    || this.parent.panel[this.position.X, nextRow].Hole
                                                    || (this.parent.OnPanelCheck(this.position.X, nextRow, false) && this.position.Y != nextRow))
                                                {
                                                    this.superSpinDirection = this.superSpinDirection == DIRECTION.up ? DIRECTION.down : DIRECTION.up;
                                                }

                                                var featherInterval = (int)Math.Round(this.superSpinInitialTrackingMoveTime * this.superSpinCurrentFeatherIntervalFactor);
                                                if (spinStartTime % featherInterval == 0)
                                                {
                                                    var perPanelTime = interpolate(this.superSpinInitialFeatherPerPanelTime, this.isPoweredUp ? this.superSpinPowerupFinalFeatherPerPanelTime : this.superSpinFinalFeatherPerPanelTime, spinStartTime);
                                                    var delayTime = interpolate(this.superSpinInitialFeatherDelay, this.superSpinFinalFeatherDelay, spinStartTime);
                                                    var newFeather = new SpinFeather(this.sound, this.parent, this.union, this.Power, this.position.X, this.position.Y, delayTime, perPanelTime, this.element);
                                                    newFeather.HasShadow = false;

                                                    var yOff = this.Random.Next(-18, 3);
                                                    var xOffRange = Math.Max(4, Math.Abs(yOff));
                                                    var xOff = this.Random.Next(-xOffRange, xOffRange);
                                                    newFeather.positionDirect.X += xOff;
                                                    newFeather.positionDirect.Y += yOff;

                                                    this.parent.attacks.Add(newFeather);
                                                    this.superSpinFeathers.Add(newFeather);
                                                }
                                            }
                                        }
                                        else if (attackWaitTime < this.superSpinSpinupTime + this.superSpinDuration)
                                        {
                                            // Full-speed
                                            if (this.attackWaitTime % 2 == 0)
                                            {
                                                var spinFrame = ((this.animationpoint.X - 2) + 1) % 3;
                                                this.animationpoint = new Point(spinFrame + 2, 5);
                                            }

                                            var roundingFunc = this.superSpinDirection == DIRECTION.up ? (Func<double, double>)Math.Ceiling : Math.Floor;
                                            var settledRow = (int)Math.Max(0, Math.Min(2, roundingFunc(this.positionDirect.Y / 24.0)));

                                            var spriteOffset = 24.0f / this.superSpinFinalTrackingMoveTime * (this.superSpinDirection == DIRECTION.up ? -1 : 1);
                                            var newPosition = new Point((int)Math.Round(this.positionDirect.X / 40), (int)Math.Round(this.positionDirect.Y / 24));
                                            this.positionDirect.Y += spriteOffset;
                                            this.position = newPosition;

                                            foreach (var feather in this.superSpinFeathers)
                                            {
                                                var freshlyMoving = !feather.HasShadow && feather.IsThrown;
                                                if (!feather.IsThrown)
                                                {
                                                    feather.positionDirect.Y += spriteOffset;
                                                    feather.position = newPosition;
                                                }
                                                else if (freshlyMoving)
                                                {
                                                    feather.positionDirect = new Vector2(feather.position.X * 40.0f + 0, feather.position.Y * 24.0f + 0);
                                                    feather.HasShadow = true;
                                                }

                                                if (feather.DeflectDirection == null && this.parent.AllObjects().OfType<BouzuTornado>().Any(bt => bt.position == feather.position && bt.union == feather.union))
                                                {
                                                    feather.DeflectDirection = feather.position.Y == 0 ? DIRECTION.down : DIRECTION.up;
                                                }
                                            }

                                            var nextRow = settledRow + (this.superSpinDirection == DIRECTION.up ? -1 : 1);
                                            if (nextRow < 0 || nextRow >= 3
                                                || this.parent.panel[this.position.X, nextRow].Hole
                                                || (this.parent.OnPanelCheck(this.position.X, nextRow, false) && this.position.Y != nextRow))
                                            {
                                                this.superSpinDirection = this.superSpinDirection == DIRECTION.up ? DIRECTION.down : DIRECTION.up;
                                            }

                                            var featherInterval = (int)Math.Round(this.superSpinFinalTrackingMoveTime * this.superSpinCurrentFeatherIntervalFactor);
                                            if (this.attackWaitTime % featherInterval == 0)
                                            {
                                                var perPanelTime = this.isPoweredUp ? this.superSpinPowerupFinalFeatherPerPanelTime : this.superSpinFinalFeatherPerPanelTime;
                                                var delayTime = this.superSpinFinalFeatherDelay;
                                                var newFeather = new SpinFeather(this.sound, this.parent, this.union, this.Power, this.position.X, this.position.Y, delayTime, perPanelTime, this.element);
                                                newFeather.HasShadow = false;

                                                var yOff = this.Random.Next(-18, 6);
                                                var xOffRange = Math.Max(6, Math.Abs(yOff));
                                                var xOff = this.Random.Next(-xOffRange, xOffRange);
                                                newFeather.positionDirect.X += xOff;
                                                newFeather.positionDirect.Y += yOff;

                                                this.parent.attacks.Add(newFeather);
                                                this.superSpinFeathers.Add(newFeather);
                                            }
                                        }
                                        else
                                        {
                                            this.isPoweredUp = false;
                                            this.guard = GUARD.none;
                                            this.AttackMotion = AttackState.Cooldown;
                                            this.AttackCooldownSet();

                                            this.position = this.positionReserved.Value;
                                            this.positionReserved = null;
                                            this.PositionDirectSet();
                                        }
                                    }
                                    break;
                                case AttackType.PowerUp:
                                    switch (this.attackWaitTime)
                                    {
                                        case 1:
                                            this.animationpoint = new Point(0, 1);
                                            this.positionDirect.Y += 1;
                                            this.detachedShadowOffset = new Vector2(0, -1);
                                            this.detachedShadow = true;
                                            this.counterTiming = true;
                                            break;
                                        case 5:
                                            this.positionDirect.Y -= 1;
                                            this.detachedShadowOffset.Y += 1;
                                            break;
                                        case 10:
                                            this.positionDirect.Y += 1;
                                            this.detachedShadowOffset.Y -= 1;
                                            break;
                                        case 15:
                                            this.positionDirect.Y -= 1;
                                            this.detachedShadowOffset.Y += 1;
                                            break;
                                        case 20:
                                            this.animationpoint = new Point(1, 1);
                                            this.underAnimationPoint = new Point(3, 1);
                                            this.detachedShadow = false;
                                            this.ShakeStart(4, 3);

                                            this.sound.PlaySE(SoundEffect.charge);
                                            this.isPoweredUp = true;
                                            break;
                                        case 23:
                                            this.underAnimationPoint = new Point(4, 1);
                                            break;
                                        case 24:
                                            this.animationpoint = new Point(2, 1);
                                            this.counterTiming = false;
                                            break;
                                        case 26:
                                            this.underAnimationPoint = null;
                                            break;
                                        case 60:
                                            this.AttackMotion = AttackState.Cooldown;
                                            this.AttackCooldownSet();
                                            break;
                                    }
                                    break;
                                case AttackType.SuperDive:
                                    switch (this.attackWaitTime)
                                    {
                                        case 1:
                                            // idle shuttered
                                            this.animationpoint = new Point(2, 0);
                                            break;
                                        case 5:
                                            // takeoff shuttered
                                            this.animationpoint = new Point(0, 3);
                                            break;
                                        case 9:
                                            // takeoff
                                            this.animationpoint = new Point(1, 3);
                                            this.counterTiming = true;
                                            break;
                                        case 14:
                                            // leg motion
                                            this.animationpoint = new Point(2, 3);
                                            break;
                                        case 18:
                                            this.counterTiming = false;
                                            // leg out, detach shadow for bobbing
                                            this.animationpoint = new Point(3, 3);
                                            this.detachedShadow = true;
                                            this.detachedShadowOffset = new Vector2(0, 0);

                                            this.superArmor = true;
                                            break;
                                        case 24:
                                            this.ShakeSingleStart(1, 36);
                                            break;
                                        case 48:
                                            break;
                                        case 60:
                                            this.positionReserved = this.position;
                                            this.superDiveRowHolesHit = new[] { false, false, false };
                                            // further logic timing-dependent, in if statement
                                            break;
                                    }

                                    if (this.attackWaitTime < 60)
                                    {
                                        // handled by switch case
                                    }
                                    else if (this.attackWaitTime == 60)
                                    {
                                        this.sound.PlaySE(SoundEffect.wave);
                                    }
                                    else if (attackWaitTime < 60 + this.superDiveHorizontalDiveMaxTime)
                                    {
                                        var offset = this.superDiveHorizontalDiveSpeed * this.UnionRebirth(this.union);
                                        this.positionDirect.X += offset;

                                        if (attackWaitTime % 5 == 0)
                                        {
                                            this.parent.effects.Add(new StepShadowYuyu(
                                                this.sound,
                                                this.parent,
                                                new Rectangle(FrameCoordX(9), FrameCoordY(3), FullFrameRect.Width, FullFrameRect.Height),
                                                this.SpritePositionDirect - new Vector2(offset, 0),
                                                this.picturename,
                                                this.rebirth,
                                                this.position,
                                                255, 255, 255));
                                        }

                                        var panelEdgeAdjustment = (32.0f * this.UnionRebirth(this.UnionEnemy));

                                        var panelPosition = new Vector2((this.positionDirect.X - panelEdgeAdjustment) / 40, this.positionDirect.Y / 24);
                                        var roundingFunc = this.union == Panel.COLOR.red ? (Func<double, double>)Math.Floor : Math.Ceiling;
                                        var newPosition = new Point((int)roundingFunc(panelPosition.X), (int)Math.Round(panelPosition.Y));
                                        var positionChanged = newPosition != this.position;
                                        this.position = newPosition;

                                        if (this.InArea)
                                        {
                                            this.DiveDragAttackMake(this.Power / 4, this.union == Panel.COLOR.blue ? DIRECTION.left : DIRECTION.right);
                                            this.HitFlagReset();
                                            this.effecting = true;

                                            if (positionChanged)
                                            {
                                                var currentPosition = this.position;
                                                var iceTrailSpawner = new AttackSpawner(
                                                    this.sound,
                                                    this.parent,
                                                    this.union,
                                                    f => f > this.superDiveIceTrailRockDelay + this.superDiveIceTrailSpikeDelay,
                                                    f =>
                                                    {
                                                        if (f == this.superDiveIceTrailRockDelay)
                                                        {
                                                            foreach (var y in new[] { currentPosition.Y - 1, currentPosition.Y + 1 })
                                                            {
                                                                if (y < 0 || y >= 3)
                                                                {
                                                                    continue;
                                                                }

                                                                if (this.superDiveRowHolesHit[y] || this.parent.panel[currentPosition.X, y].Hole)
                                                                {
                                                                    this.superDiveRowHolesHit[y] = true;
                                                                    continue;
                                                                }

                                                                var direction = currentPosition.Y - y > 0 ? DIRECTION.down : DIRECTION.up;

                                                                var rockCenterPush = new DragEnemyHit(this.sound, this.parent, currentPosition.X, y, this.union, this.Power, this.element, this, direction)
                                                                {
                                                                    breaking = true,
                                                                    invincibility = false
                                                                };
                                                                this.parent.attacks.Add(rockCenterPush);
                                                                this.parent.objects.Add(new IceRocks(this.sound, this.parent, currentPosition.X, y, this.union, this.superDiveIceTrailRockLife, this.superDiveIceTrailRockHp));
                                                            }
                                                        }
                                                        else if (f == this.superDiveIceTrailRockDelay + this.superDiveIceTrailSpikeDelay)
                                                        {
                                                            for (var y = 0; y < 3; y++)
                                                            {
                                                                if (y == currentPosition.Y + 1 || y == currentPosition.Y - 1)
                                                                {
                                                                    continue;
                                                                }

                                                                if (this.superDiveRowHolesHit[y] || this.parent.panel[currentPosition.X, y].Hole)
                                                                {
                                                                    this.superDiveRowHolesHit[y] = true;
                                                                    continue;
                                                                }

                                                                this.parent.attacks.Add(new Tower(this.sound, this.parent, currentPosition.X, y, this.union, this.Power, -1, this.Element));
                                                            }
                                                        }
                                                    });

                                                this.parent.attacks.Add(iceTrailSpawner);
                                            }
                                        }
                                        else
                                        {
                                            this.position = this.positionReserved.Value;
                                        }
                                    }
                                    else if (this.attackWaitTime < 60 + this.superDiveHorizontalDiveMaxTime + (this.superDiveSwoopCount * this.superDiveSwoopMaxTime))
                                    {
                                        var swoopFrame = (attackWaitTime - (60 + this.superDiveHorizontalDiveMaxTime)) % this.superDiveSwoopMaxTime;
                                        if (swoopFrame == 0)
                                        {
                                            this.nohit = true;

                                            var diveTarget = this.RandomTarget();
                                            // diveTarget.X += this.UnionRebirth(this.UnionEnemy);
                                            this.position = diveTarget;

                                            this.detachedShadow = false;
                                            this.detachedShadowOffset = new Vector2(0, -12);
                                            
                                            this.positionDirect = new Vector2(diveTarget.X * 40 + 6, diveTarget.Y * 24 + 12);
                                            for (var i = 1; i <= this.superDiveSwoopIncomingFrames; i++)
                                            {
                                                var angle = i * this.superDiveSwoopIncomingAngleSpeed;
                                                angle = Math.Min(Math.PI / 2, angle);
                                                this.positionDirect.X += this.superDiveSwoopSpeed * (float)Math.Cos(angle) * this.UnionRebirth(this.UnionEnemy);
                                                var yOffset = this.superDiveSwoopSpeed * (float)Math.Sin(angle);
                                                this.positionDirect.Y -= yOffset;
                                                this.detachedShadowOffset.Y += yOffset;
                                            }

                                           this.parent.attacks.Add(new DiveFeatherSpawner(this.sound, this.parent, this.union, this.power / 4, this.superDiveSwoopFeatherHitTime, this.superDiveSwoopFeatherDelay, this.superDiveSwoopFeatherSets, this.superDiveSwoopFeatherCount, this.element));
                                        }
                                        else if (swoopFrame < this.superDiveSwoopIncomingFrames)
                                        {
                                            this.detachedShadow = this.detachedShadowOffset.Y < 16;

                                            var angle = (this.superDiveSwoopIncomingFrames - swoopFrame) * this.superDiveSwoopIncomingAngleSpeed;
                                            angle = Math.Min(Math.PI / 2, angle);
                                            this.positionDirect.X -= this.superDiveSwoopSpeed * (float)Math.Cos(angle) * this.UnionRebirth(this.UnionEnemy);
                                            var yOffset = this.superDiveSwoopSpeed * (float)Math.Sin(angle);
                                            this.positionDirect.Y += yOffset;
                                            this.detachedShadowOffset.Y -= yOffset;
                                        }
                                        else if (swoopFrame < this.superDiveSwoopIncomingFrames + this.superDiveSwoopOutgoingFrames)
                                        {
                                            if (swoopFrame == this.superDiveSwoopIncomingFrames)
                                            {
                                                this.sound.PlaySE(SoundEffect.futon);
                                            }

                                            this.detachedShadow = this.detachedShadowOffset.Y < 16;

                                            var angle = (swoopFrame - this.superDiveSwoopIncomingFrames) * this.superDiveSwoopOutgoingAngleSpeed;
                                            angle = Math.Min(Math.PI / 2, angle);
                                            this.positionDirect.X -= this.superDiveSwoopSpeed * (float)Math.Cos(angle) * this.UnionRebirth(this.UnionEnemy);
                                            var yOffset = this.superDiveSwoopSpeed * (float)Math.Sin(angle);
                                            this.positionDirect.Y -= yOffset;
                                            this.detachedShadowOffset.Y += yOffset;
                                        }

                                        if (attackWaitTime % 5 == 0)
                                        {
                                            this.parent.effects.Add(new StepShadowYuyu(
                                                this.sound,
                                                this.parent,
                                                new Rectangle(FrameCoordX(9), FrameCoordY(3), FullFrameRect.Width, FullFrameRect.Height),
                                                this.SpritePositionDirect,
                                                this.picturename,
                                                this.rebirth,
                                                this.position,
                                                255, 255, 255));
                                        }

                                        var panelEdgeAdjustment = (32.0f * this.UnionRebirth(this.UnionEnemy));
                                        var panelPosition = new Vector2((this.positionDirect.X - panelEdgeAdjustment) / 40, this.positionDirect.Y / 24);
                                        var roundingFunc = this.union == Panel.COLOR.red ? (Func<double, double>)Math.Floor : Math.Ceiling;
                                        var newPosition = new Point((int)roundingFunc(panelPosition.X), (int)Math.Round(panelPosition.Y));
                                        if ((this.position.Y * 24) - this.positionDirect.Y < 24)
                                        {
                                            this.position = new Point(newPosition.X, this.position.Y);
                                            this.nohit = false;
                                            this.effecting = true;
                                            this.DiveDragAttackMake(this.Power / 4, this.union == Panel.COLOR.blue ? DIRECTION.left : DIRECTION.right);
                                            this.HitFlagReset();
                                        }
                                        else
                                        {
                                            this.effecting = false;
                                            this.nohit = true;
                                            this.position = new Point(this.positionReserved.Value.X, this.position.Y);
                                        }
                                    }
                                    else if (this.attackWaitTime < 60 + this.superDiveHorizontalDiveMaxTime + (this.superDiveSwoopCount * this.superDiveSwoopMaxTime) + this.superDiveImpactMaxTime
                                        && !this.superDiveImpactAborted)
                                    {
                                        var diveTime = this.attackWaitTime - (60 + this.superDiveHorizontalDiveMaxTime + (this.superDiveSwoopCount * this.superDiveSwoopMaxTime));
                                        var diveTimeFunc = new Func<float, float>(time => 12 * time / this.superDiveImpactMaxTime);
                                        var t = Math.Min(1f, diveTimeFunc(diveTime));
                                        var diveImpactTime = Enumerable.Range(0, this.superDiveImpactMaxTime).First(time => diveTimeFunc(time) > 1);

                                        var startingPosition = new Point(this.union == Panel.COLOR.blue ? 4 : 1, 1);
                                        var startingPositionDirect = new Vector2(280, -20);


                                        if (diveTime == 0)
                                        {
                                            this.superDiveTargetPosition = new Point(this.union == Panel.COLOR.blue ? 1 : 4, 1);

                                            var allPanels = Enumerable.Range(0, this.parent.panel.GetLength(0)).SelectMany(x => Enumerable.Range(0, this.parent.panel.GetLength(1)).Select(y => new Point(x, y)));
                                            var allEnemyPanels = allPanels.Where(p => this.parent.panel[p.X, p.Y].color != this.union);

                                            var columnHitPanels = new[] { -1, 0, 1 }.Select(yOff => this.superDiveTargetPosition.WithOffset(0, yOff));
                                            if (!allEnemyPanels.Except(columnHitPanels).Any())
                                            {
                                                this.superDiveTargetPosition.Offset(this.union == Panel.COLOR.blue ? 1 : -1, 0);
                                            }
                                        }

                                        var panelEdgeAdjustment = (32.0f * this.UnionRebirth(this.UnionEnemy));
                                        var endPositionDirect = new Vector2(this.superDiveTargetPosition.X * 40 + panelEdgeAdjustment, this.superDiveTargetPosition.Y * 24 + 32);

                                        this.positionDirect = startingPositionDirect + Vector2.Multiply(endPositionDirect - startingPositionDirect, t);
                                        
                                        var panelPosition = new Vector2((this.positionDirect.X - panelEdgeAdjustment) / 40, this.positionDirect.Y / 24);
                                        var roundingFunc = this.union == Panel.COLOR.red ? (Func<double, double>)Math.Floor : Math.Ceiling;
                                        var newPosition = new Point((int)roundingFunc(panelPosition.X), (int)Math.Round(panelPosition.Y));
                                        this.position = newPosition;

                                        var impactTime = diveTime - diveImpactTime;

                                        if (impactTime < 0 && (this.superDiveTargetPosition.Y * 24 + 32) - this.positionDirect.Y < 16)
                                        {
                                            this.nohit = false;

                                            var dragPower = this.Power / 4;
                                            var dir = this.union == Panel.COLOR.blue ? DIRECTION.left : DIRECTION.right;

                                            var dragPathX = Enumerable.Range(Math.Min(newPosition.X, this.superDiveTargetPosition.X), Math.Abs(newPosition.X - this.superDiveTargetPosition.X));
                                            foreach (var pathX in dragPathX)
                                            {
                                                var enemyHit = new DragEnemyHit(this.sound, this.parent, pathX, this.superDiveTargetPosition.Y, this.union, dragPower, this.element, this, dir)
                                                {
                                                    breaking = true,
                                                    invincibility = false
                                                };
                                                this.parent.attacks.Add(enemyHit);
                                            }

                                            this.HitFlagReset();
                                        }
                                        else if (impactTime > 0)
                                        {
                                            EnemyHit enemyHit = new EnemyHit(this.sound, this.parent, this.position.X - this.UnionRebirth(this.union), this.position.Y, this.union, this.Power / 4, this.element, this)
                                            {
                                                breaking = false
                                            };
                                            this.parent.attacks.Add(enemyHit);
                                            this.HitFlagReset();
                                        }

                                        switch (impactTime)
                                        {
                                            case 0:
                                                {
                                                    if (this.parent.panel[this.superDiveTargetPosition.X, this.superDiveTargetPosition.Y].Hole)
                                                    {
                                                        this.superDiveImpactAborted = true;
                                                        break;
                                                    }

                                                    this.sound.PlaySE(SoundEffect.bombmiddle);
                                                    var slamAttack = new BombAttack(this.sound, this.parent, this.superDiveTargetPosition.X, this.superDiveTargetPosition.Y, this.union, this.Power, 1, this.element)
                                                    {
                                                        invincibility = false
                                                    };
                                                    slamAttack.BadStatusSet(BADSTATUS.paralyze, 45);
                                                    slamAttack.BadStatusSet(BADSTATUS.stop, 45);
                                                    this.parent.attacks.Add(slamAttack);

                                                    this.parent.effects.Add(new DiveBomber(this.sound, this.parent, this.superDiveTargetPosition));

                                                    this.parent.panel[this.superDiveTargetPosition.X, this.superDiveTargetPosition.Y].Crack();
                                                    if (this.parent.panel[this.superDiveTargetPosition.X, this.superDiveTargetPosition.Y].State == Panel.PANEL._break)
                                                    {
                                                        this.parent.panel[this.superDiveTargetPosition.X, this.superDiveTargetPosition.Y].State = Panel.PANEL._crack;
                                                    }

                                                    this.ShakeStart(12, 8);

                                                    this.isPoweredUp = false;
                                                    break;
                                                }
                                            case 15:
                                                {
                                                    var offsets = new[,] { { -1, 0 }, { 1, 0 }, { 0, -1 }, { 0, 1 } };

                                                    for (var i = 0; i < offsets.GetLength(0); i++)
                                                    {
                                                        var offsetPosition = this.superDiveTargetPosition.WithOffset(offsets[i, 0], offsets[i, 1]);
                                                        if (this.InAreaCheck(offsetPosition) && !this.parent.panel[offsetPosition.X, offsetPosition.Y].Hole)
                                                        {
                                                            var slamAttack = new BombAttack(this.sound, this.parent, offsetPosition.X, offsetPosition.Y, this.union, this.Power, 1, this.element)
                                                            {
                                                                invincibility = true
                                                            };
                                                            this.parent.attacks.Add(slamAttack);

                                                            this.parent.effects.Add(new DiveBomber(this.sound, this.parent, offsetPosition));
                                                            this.ShakeStart(3, 5);
                                                        }
                                                    }
                                                    
                                                    break;
                                                }
                                            case 30:
                                                {
                                                    var offsets = new[,] { { -1, -1 }, { -1, 1 }, { 1, -1 }, { 1, 1 } };

                                                    for (var i = 0; i < offsets.GetLength(0); i++)
                                                    {
                                                        var offsetPosition = this.superDiveTargetPosition.WithOffset(offsets[i, 0], offsets[i, 1]);
                                                        if (this.InAreaCheck(offsetPosition) && !this.parent.panel[offsetPosition.X, offsetPosition.Y].Hole)
                                                        {
                                                            var slamAttack = new BombAttack(this.sound, this.parent, offsetPosition.X, offsetPosition.Y, this.union, this.Power, 1, this.element)
                                                            {
                                                                invincibility = true
                                                            };
                                                            this.parent.attacks.Add(slamAttack);

                                                            this.parent.effects.Add(new DiveBomber(this.sound, this.parent, offsetPosition));
                                                            this.ShakeStart(2);
                                                        }
                                                    }

                                                    break;
                                                }
                                        }
                                    }
                                    else
                                    {
                                        this.ShakeEnd();
                                        this.AttackMotion = AttackState.Cooldown;
                                        this.AttackCooldownSet();

                                        this.detachedShadow = false;
                                        this.detachedShadowOffset = Vector2.Zero;
                                        this.underAnimationPoint = null;
                                        this.underAnimationOffset = Vector2.Zero;
                                        this.isPoweredUp = false;
                                        this.nohit = false;
                                        this.superArmor = false;
                                        this.effecting = false;
                                        this.superDiveImpactAborted = false;

                                        this.parent.effects.Add(new MoveEnemy(this.sound, this.parent, this.position.X, this.position.Y));
                                        if (this.parent.panel[this.superDiveTargetPosition.X, this.superDiveTargetPosition.Y].State == Panel.PANEL._crack)
                                        {
                                            this.parent.panel[this.superDiveTargetPosition.X, this.superDiveTargetPosition.Y].State = Panel.PANEL._break;
                                        }

                                        this.position = this.positionReserved.Value;
                                        this.PositionDirectSet();
                                        this.positionReserved = null;
                                    }
                                    break;
                                case AttackType.SuperSpin:
                                    {
                                        const int spinPoseFrames = 6;
                                        const int spinSwipeFrames = 4;
                                        if (this.attackWaitTime < this.spinSpinupTime)
                                        {
                                            // Move to center, animate and create pattern
                                            if (this.attackWaitTime <= 10 + spinPoseFrames + spinSwipeFrames)
                                            {
                                                switch (this.attackWaitTime)
                                                {
                                                    case 0:
                                                        {
                                                            var initialPosition = this.position;
                                                            var initialPositionDirect = this.SpritePositionDirect;

                                                            var spinPosition = this.union == Panel.COLOR.blue ? new Point(4, 1) : new Point(1, 1);
                                                            if (this.parent.panel[spinPosition.X, spinPosition.Y].color != this.union || this.parent.panel[spinPosition.X, spinPosition.Y].Hole)
                                                            {
                                                                spinPosition = this.union == Panel.COLOR.blue ? new Point(5, 1) : new Point(0, 1);
                                                            }

                                                            if (this.parent.panel[spinPosition.X, spinPosition.Y].color != this.union || this.parent.panel[spinPosition.X, spinPosition.Y].Hole)
                                                            {
                                                                this.MoveRandom(false, false);
                                                                for (var i_retry = 0; i_retry < 6; i_retry++)
                                                                {
                                                                    if ((this.union == Panel.COLOR.red || this.positionre.X > 3)
                                                                        && (this.union == Panel.COLOR.blue || this.positionre.X < 2)
                                                                        && !this.parent.panel[this.positionre.X, this.positionre.Y].Hole)
                                                                    {
                                                                        spinPosition = this.positionre;
                                                                        break;
                                                                    }
                                                                    this.MoveRandom(false, false);
                                                                }
                                                            }
                                                            this.positionre = spinPosition;

                                                            this.position = this.positionre;
                                                            this.PositionDirectSet();

                                                            this.animationpoint = new Point(2, 0);
                                                            if (initialPosition != this.position)
                                                            {
                                                                this.parent.effects.Add(new StepShadowYuyu(
                                                                    this.sound,
                                                                    this.parent,
                                                                    new Rectangle(FrameCoordX(3), FrameCoordY(0), FullFrameRect.Width, FullFrameRect.Height),
                                                                    initialPositionDirect,
                                                                    this.picturename,
                                                                    this.rebirth,
                                                                    initialPosition,
                                                                    255, 255, 255));
                                                            }
                                                            break;
                                                        }
                                                    case 4:
                                                        this.animationpoint = new Point(1, 0);
                                                        break;
                                                    case 10:
                                                        this.animationpoint = new Point(0, 5);
                                                        this.counterTiming = true;
                                                        break;
                                                    case 10 + spinPoseFrames:
                                                        this.animationpoint = new Point(1, 5);
                                                        break;
                                                    case 10 + spinSwipeFrames + spinPoseFrames:
                                                        this.animationpoint = new Point(2, 5);
                                                        this.counterTiming = false;
                                                        this.guard = GUARD.guard;

                                                        this.spinPattern.Clear();
                                                        var initialRow = this.Random.Next(3);
                                                        this.spinPattern.Add(initialRow);
                                                        var potentialMovements = new List<int>();

                                                        for (var i = 0; i < this.spinPatternLength - 1; i++)
                                                        {
                                                            var currentRow = this.spinPattern.Last();
                                                            var distanceFromCycleStart = currentRow - initialRow;

                                                            var remainingRows = this.spinPatternLength - 1 - i;
                                                            if (Math.Abs(distanceFromCycleStart) >= remainingRows)
                                                            {
                                                                this.spinPattern.Add(currentRow + (distanceFromCycleStart > 0 ? -1 : 1));
                                                            }
                                                            else
                                                            {
                                                                potentialMovements.Clear();
                                                                potentialMovements.AddRange(Enumerable.Repeat(0, this.spinFeatherPatternStayWeight));
                                                                if (currentRow != 0)
                                                                {
                                                                    potentialMovements.AddRange(Enumerable.Repeat(-1, this.spinFeatherPatternMoveWeight));
                                                                }
                                                                if (currentRow != 2)
                                                                {
                                                                    potentialMovements.AddRange(Enumerable.Repeat(1, this.spinFeatherPatternMoveWeight));
                                                                }

                                                                this.spinPattern.Add(currentRow + potentialMovements[this.Random.Next(potentialMovements.Count)]);
                                                            }
                                                        }

                                                        break;
                                                }
                                            }
                                            else
                                            {
                                                // Start with slow spin, accelerate
                                                var spinStartTime = this.attackWaitTime - (10 + spinPoseFrames + spinSwipeFrames);

                                                const int minFrameDelay = 2;
                                                const int initialFrameDelay = 6;
                                                Func<int, int> calculateFrameDelay = t => (int)(Math.Round(initialFrameDelay - (initialFrameDelay - minFrameDelay) * ((double)t / this.spinSpinupTime)));

                                                var animProgress = 0;
                                                while (animProgress < spinStartTime)
                                                {
                                                    animProgress += calculateFrameDelay(animProgress);
                                                }

                                                if (animProgress == spinStartTime)
                                                {
                                                    var spinFrame = ((this.animationpoint.X - 2) + 1) % 3;
                                                    this.animationpoint = new Point(spinFrame + 2, 5);
                                                }

                                                Func<int, int> calculateFeatherDelay = t => (int)(Math.Round(this.spinFeatherInitialDelay - (this.spinFeatherInitialDelay - this.spinFeatherMinimumDelay) * ((double)t / this.spinSpinupTime)));

                                                var featherProgress = 0;
                                                while (featherProgress < spinStartTime)
                                                {
                                                    featherProgress += calculateFeatherDelay(featherProgress);
                                                }

                                                // Spawn row attacks on similar acceleration (to establish pattern)
                                                if (featherProgress == spinStartTime)
                                                {
                                                    // Spawn row attacks at max speed in same pattern
                                                    var gap = this.spinPattern[0];
                                                    this.spinPattern.RemoveAt(0);
                                                    this.spinPattern.Add(gap);

                                                    var randomDelayOrder = Enumerable.Range(0, 3).ToArray();//.OrderBy(x => this.Random.Next(-1, 2)).ToArray();
                                                    var px = 3;
                                                    for (var i = 0; i < randomDelayOrder.Length; i++)
                                                    {
                                                        var py = randomDelayOrder[i];
                                                        if (py != gap)
                                                        {
                                                            var delayTime = 10 + 2 * i;
                                                            this.parent.attacks.Add(new SpinFeather(this.sound, this.parent, this.union, this.Power, px, py, delayTime, this.spinFeatherPerPanelTime, this.element));
                                                        }
                                                    }
                                                    
                                                    if (isPoweredUp)
                                                    {
                                                        const int tornadoInterval = 4;
                                                        if (this.spinPowerupTornadoCounter % tornadoInterval == 0)
                                                        {
                                                            var tornadoLifetime = this.Random.Next(12, 15);
                                                            var tornadoTargeting = this.CreateTornadoOrbitTargetingFunc(false, this.spinPowerupTornadoCounter % (tornadoInterval * 2) == 0, this.position, t => t.waittime++ < tornadoLifetime);
                                                            var tornado = new BouzuTornado(this.sound, this.parent, this.position.X, this.position.Y, this.union, this.power, this.element, -1, 1, 0, false, tornadoTargeting);
                                                            tornado.InitAfter();
                                                            this.parent.attacks.Add(tornado);

                                                            var currentPosition = this.position;
                                                            Func<BouzuTornado, Point> centerTornadoTargeting = t => { t.flag = t.waittime++ < tornadoLifetime; return currentPosition; };
                                                            var centerTornado = new BouzuTornado(this.sound, this.parent, this.position.X, this.position.Y, this.union, this.power, this.element, -1, 1, 0, false, centerTornadoTargeting);
                                                            centerTornado.InitAfter();
                                                            this.parent.attacks.Add(centerTornado);
                                                        }

                                                        this.spinPowerupTornadoCounter++;
                                                    }
                                                }
                                            }
                                        }
                                        else if (this.attackWaitTime < this.spinSpinupTime + this.spinDuration)
                                        {
                                            // Full-speed
                                            if (this.attackWaitTime % 2 == 0)
                                            {
                                                var spinFrame = ((this.animationpoint.X - 2) + 1) % 3;
                                                this.animationpoint = new Point(spinFrame + 2, 5);
                                            }

                                            const int subdivisions = 3;

                                            var fullSpinTime = this.attackWaitTime - this.spinSpinupTime;
                                            var delayDivision = this.spinFeatherMinimumDelay / subdivisions;
                                            var switchTime = this.spinSpinupTime % delayDivision;
                                            if (fullSpinTime % delayDivision == switchTime)
                                            {
                                                var fullDelayHit = fullSpinTime % (delayDivision * subdivisions) == switchTime;
                                                var isFullSpeed = fullSpinTime > this.spinFeatherMinimumDelay * this.spinPatternLength / subdivisions;

                                                if (isFullSpeed && this.spinPattern.Count == this.spinPatternLength)
                                                {
                                                    // Convert gaps to bits 0, 1, 2, add middle with both gaps between
                                                    var repeatedPatternBits = this.spinPattern.Select(g => Enumerable.Repeat(1 << g, subdivisions - 1).ToArray())
                                                        .Aggregate((i, j) => i.Concat(Enumerable.Repeat(i[i.Length - 1] | j[0], subdivisions - 1)).Concat(j).ToArray()).ToList();
                                                    repeatedPatternBits.AddRange(Enumerable.Repeat(repeatedPatternBits[repeatedPatternBits.Count - 1] | repeatedPatternBits[0], subdivisions - 1));
                                                    this.spinPattern.Clear();
                                                    this.spinPattern.AddRange(repeatedPatternBits);
                                                }

                                                // Spawn row attacks at max speed in same pattern
                                                var gapBits = isFullSpeed ? this.spinPattern[0] : (1 << this.spinPattern[0]);

                                                if (isFullSpeed || fullDelayHit)
                                                {
                                                    var originalHead = this.spinPattern[0];
                                                    this.spinPattern.RemoveAt(0);
                                                    this.spinPattern.Add(originalHead);

                                                    var randomDelayOrder = Enumerable.Range(0, 3).ToArray();//.OrderBy(x => this.Random.Next(-1, 2)).ToArray();
                                                    var px = 3;
                                                    for (var i = 0; i < randomDelayOrder.Length; i++)
                                                    {
                                                        var py = randomDelayOrder[i];
                                                        if (((1 << py) & gapBits) == 0)
                                                        {
                                                            var delayTime = 10 + 2 * i;
                                                            this.parent.attacks.Add(new SpinFeather(this.sound, this.parent, this.union, this.Power, px, py, delayTime, this.spinFeatherPerPanelTime, this.element));
                                                        }
                                                    }
                                                    
                                                    if (isPoweredUp)
                                                    {
                                                        const int tornadoInterval = 12;
                                                        if (this.spinPowerupTornadoCounter % tornadoInterval == 0)
                                                        {
                                                            var tornadoLifetime = this.Random.Next(9, 12);
                                                            var tornadoOrbitTargeting = this.CreateTornadoOrbitTargetingFunc(false, this.spinPowerupTornadoCounter % (tornadoInterval * 2) == 0, this.position, t => t.waittime++ < tornadoLifetime);
                                                            var tornadoTargeting = this.CreateTornadoMoverFunc(tornadoOrbitTargeting, () => tornadoLifetime += 5);
                                                            var tornado = new BouzuTornado(this.sound, this.parent, this.position.X, this.position.Y, this.union, this.power, this.element, -1, 1, 0, false, tornadoTargeting);
                                                            tornado.InitAfter();
                                                            this.parent.attacks.Add(tornado);

                                                            var currentPosition = this.position;
                                                            var centerTornadoLifetime = tornadoLifetime;
                                                            Func<BouzuTornado, Point> centerTornadoTargeting = t => { t.flag = t.waittime++ < centerTornadoLifetime; return currentPosition; };
                                                            var centerTornadoMoverTargeting = this.CreateTornadoMoverFunc(centerTornadoTargeting, () => centerTornadoLifetime += 5);
                                                            var centerTornado = new BouzuTornado(this.sound, this.parent, this.position.X, this.position.Y, this.union, this.power, this.element, -1, 1, 0, false, centerTornadoMoverTargeting);
                                                            centerTornado.InitAfter();
                                                            this.parent.attacks.Add(centerTornado);
                                                        }

                                                        this.spinPowerupTornadoCounter++;
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            this.spinPowerupTornadoCounter = 0;
                                            this.spinPowerupTornadoMoverEvaluationNumber = 0;
                                            this.isPoweredUp = false;
                                            this.guard = GUARD.none;
                                            this.AttackMotion = AttackState.Cooldown;
                                            this.AttackCooldownSet();
                                        }
                                    }
                                    break;
                            }
                            this.attackWaitTime++;
                            break;
                        case AttackState.Cooldown:
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
                var flinchRect = new Rectangle(FrameCoordX(0), FrameCoordY(0), FullFrameRect.Width, FullFrameRect.Height);
                var flinchWhiteRect = new Rectangle(FrameCoordX(0 + 6), FrameCoordY(0), FullFrameRect.Width, FullFrameRect.Height);
                this.Death(flinchRect, flinchWhiteRect, this.SpritePositionDirect, this.picturename);
                return;
            }

            this.HPposition = this.HPPositionDirect;
            var spriteOffsetPosition = this.SpritePositionDirect + new Vector2(this.Shake.X, this.Shake.Y);

            if (this.detachedShadow)
            {
                var shadowFramePoint = this.whitetime == 0 ? new Point(4, 0) : new Point(10, 0);
                var detachedShadowPositionDirect = spriteOffsetPosition + this.detachedShadowOffset;
                dg.DrawImage(dg, this.picturename, new Rectangle(FrameCoordX(shadowFramePoint.X), FrameCoordY(shadowFramePoint.Y), FullFrameRect.Width, FullFrameRect.Height), false, detachedShadowPositionDirect, this.color);
            }

            var reversed = this.crossDiveReverse;

            if (this.underAnimationPoint != null)
            {
                var hitmarkedUnderAnimationPoint = this.whitetime == 0 ? this.underAnimationPoint.Value : this.underAnimationPoint.Value.WithOffset(6, 0);
                dg.DrawImage(dg, this.picturename, new Rectangle(FrameCoordX(hitmarkedUnderAnimationPoint.X), FrameCoordY(hitmarkedUnderAnimationPoint.Y), FullFrameRect.Width, FullFrameRect.Height), false, spriteOffsetPosition + this.underAnimationOffset, 1f, 0f, reversed, this.color);
            }

            var whiteAnimationPoint = this.animationpoint.WithOffset(6, 0);
            var hitmarkedAnimationPoint = this.whitetime == 0 ? this.animationpoint : whiteAnimationPoint;
            dg.DrawImage(dg, this.picturename, new Rectangle(FrameCoordX(hitmarkedAnimationPoint.X), FrameCoordY(hitmarkedAnimationPoint.Y), FullFrameRect.Width, FullFrameRect.Height), false, spriteOffsetPosition, 1f, 0f, reversed, this.color);

            if (this.isPoweredUp)
            {
                const double period = 20;
                var strobeProgress = this.frame / period;
                var opacity = 120 * 0.5 * (1 - Math.Cos(strobeProgress * (2 * Math.PI)));
                var roundedOpacity = (int)Math.Round(opacity);

                var strobeColor = Color.FromArgb(roundedOpacity, Color.White);
                dg.DrawImage(dg, this.picturename, new Rectangle(FrameCoordX(whiteAnimationPoint.X), FrameCoordY(whiteAnimationPoint.Y), FullFrameRect.Width, FullFrameRect.Height), false, spriteOffsetPosition, 1f, 0f, reversed, strobeColor);
            }

            this.Nameprint(dg, this.printNumber);
        }

        private void DiveDragAttackMake(int dragPower, DIRECTION dir)
        {
            if (!this.effecting)
                return;
            var enemyHit = new DragEnemyHit(this.sound, this.parent, this.position.X, this.position.Y, this.union, dragPower, this.element, this, dir)
            {
                breaking = true,
                invincibility = false
            };
            this.parent.attacks.Add(enemyHit);
        }

        private Func<BouzuTornado, Point> CreateTornadoOrbitTargetingFunc(bool isClockwise, bool isLeft, Point center, Func<BouzuTornado, bool> flagFunc)
        {
            return t =>
            {
                int xOff = 0, yOff = 0;
                var offsets = new[,] { { -1, 0 }, { -1, -1 }, { 0, -1 }, { 1, -1 }, { 1, 0 }, { 1, 1 }, { 0, 1 }, { -1, 1 } };
                if (t.waittime != 0)
                {
                    var offsetIndex = (t.waittime - 1) % offsets.GetLength(0);
                    xOff = offsets[offsetIndex, 0] * (isLeft ? -1 : 1);
                    yOff = offsets[offsetIndex, 1] * (isLeft ? -1 : 1) * (!isClockwise ? -1 : 1);
                }
                t.flag = flagFunc(t);
                return new Point(center.X + xOff, center.Y + yOff);
            };
        }

        private Func<BouzuTornado, Point> CreateTornadoMoverFunc(Func<BouzuTornado, Point> originalTargeting, Action lifetimeExtension)
        {
            var spinComplete = false;
            var isMover = false;
            var moverEvaluationCount = 0;

            return t =>
            {
                if (!isMover)
                {
                    if (!spinComplete && !this.isPoweredUp)
                    {
                        spinComplete = true;
                        lifetimeExtension.Invoke();
                    }

                    if (spinComplete)
                    {
                        if (moverEvaluationCount == this.spinPowerupTornadoMoverEvaluationNumber)
                        {
                            const int tornadoMoverDelayCount = 3;
                            
                            if (t.position.Y == this.RandomTarget().Y)
                            {
                                isMover = true;
                                t.waittime = 0;
                                this.spinPowerupTornadoMoverEvaluationNumber += tornadoMoverDelayCount;
                            }
                        }

                        moverEvaluationCount++;
                    }
                }

                if (isMover)
                {
                    var endPositionX = t.union == Panel.COLOR.blue ? 0 : 5;
                    if (t.position.X == endPositionX)
                    {
                        t.waittime++;
                    }

                    t.flag = t.waittime < 2;

                    return new Point(endPositionX, t.position.Y);
                }

                return originalTargeting.Invoke(t);
            };
        }

        public class DragEnemyHit : EnemyHit
        {
            private DIRECTION dir;

            public DragEnemyHit(
              IAudioEngine so,
              SceneBattle p,
              int pX,
              int pY,
              Panel.COLOR u,
              int po,
              ChipBase.ELEMENT ele,
              CharacterBase chara,
              DIRECTION dir)
              : base(so, p, pX, pY, u, po, ele, chara)
            {
                this.dir = dir;
            }

            public override bool HitEvent(Player p)
            {
                p.Knockbuck(dir, false, this.union);
                return base.HitEvent(p);
            }

            public override bool HitEvent(EnemyBase e)
            {
                e.Knockbuck(dir, false, this.union);
                return base.HitEvent(e);
            }

            public override bool HitEvent(ObjectBase o)
            {
                o.Knockbuck(dir, false, this.union);
                return base.HitEvent(o);
            }
        }

        private int CalculateDiveToTargetFrames()
        {
            if (this.positionReserved == null)
            {
                return Constants.ArbitraryLargeValue;
            }

            var xDiff = this.diveTargetPosition.X - this.positionReserved.Value.X;
            var yDiff = this.diveTargetPosition.Y - this.positionReserved.Value.Y;
            var framesToTravelDistance = this.diveFramesPerPanel * Math.Sqrt(xDiff * xDiff + yDiff * yDiff);
            var adjustedTime = (int)Math.Ceiling(framesToTravelDistance);
            return adjustedTime;
        }
        
        private void SetDefaultVersionStats()
        {
            this.name = ShanghaiEXE.Translate("Enemy.CirnoBXName");
            this.power = 100;
            this.hp = 3000;
            this.picturename = "CirnoBX";
            this.element = ChipBase.ELEMENT.aqua;

            this.idleDelayBase = 30;
            this.idleDelayFuzz = 0;

            this.attackChance = 0.5;

            this.attackDelayBase = 8;
            this.attackDelayFuzz = 0;
            this.attackCooldownBase = 24;
            this.attackCooldownFuzz = 0;

            this.diveFramesPerPanel = 4;
            this.diveRestFrames = 40;
            this.diveFeatherHitTime = 20;
            this.diveFeatherDelay = 60;
            this.diveFeatherCount = 3;
            this.diveFeatherSets = 2;
            this.powerDiveSmallIceCount = 3;
            this.powerDiveIcicleHitTime = 90;
            this.powerDiveSmallIceLifetime = 180;
            this.powerDiveSmallIceSpawnDelay = 15;
            this.powerDiveSmallIceHp = 50;

            this.crossDiveWarningFrames = 20;
            this.crossDiveInitialDelayFrames = 45;
            this.crossDiveDiagonalPassFrames = 15;
            this.crossDiveCircleBackDelayFrames = 5;
            this.crossDiveEndFlightFrames = 30;
            this.crossDiveEntryFramesBeforeCounter = 1;
            this.crossDiveCounterFrames = 15;

            this.iceCrashDuration = 280;
            this.iceCrashLargeIceHitTime = 60;
            this.iceCrashLargeIceLifetime = 200;
            this.iceCrashLargeIceHp = 500;
            this.iceCrashSmallIceHitTime = 45;
            this.iceCrashSmallIceSpawnDelay = 20;
            this.iceCrashLargeIceRemnantHp = 200;
            this.iceCrashBlockerHitTime = 45;
            this.iceCrashBlockerLifetime = 150;
            this.iceCrashBlockerHp = 200;
            this.powerIceCrashAdditionalLargeIceSpawnDelay = 160;

            this.spinPatternLength = 5;
            this.spinFeatherPatternStayWeight = 2;
            this.spinFeatherPatternMoveWeight = 5;
            this.spinSpinupTime = 120;
            this.spinDuration = 240;
            this.spinFeatherInitialDelay = 45;
            this.spinFeatherMinimumDelay = 16;
            this.spinFeatherPerPanelTime = 8;

            this.superDiveHorizontalDiveSpeed = 10;
            this.superDiveHorizontalDiveMaxTime = 90;
            this.superDiveSwoopCount = 3;
            this.superDiveSwoopMaxTime = 90;
            this.superDiveIceTrailRockDelay = 15;
            this.superDiveIceTrailRockLife = 30;
            this.superDiveIceTrailRockHp = 30;
            this.superDiveIceTrailSpikeDelay = 45;
            this.superDiveSwoopSpeed = 12;
            this.superDiveSwoopIncomingAngleSpeed = 3 * 2 * Math.PI / 360;
            this.superDiveSwoopOutgoingAngleSpeed = 7 * 2 * Math.PI / 360;
            this.superDiveSwoopIncomingFrames = 45;
            this.superDiveSwoopOutgoingFrames = 32;
            this.superDiveSwoopFeatherHitTime = 20;
            this.superDiveSwoopFeatherDelay = 40;
            this.superDiveSwoopFeatherSets = 2;
            this.superDiveSwoopFeatherCount = 2;
            this.superDiveImpactMaxTime = 120;

            this.superSpinInitialTrackingMoveTime = 40;
            this.superSpinFinalTrackingMoveTime = 10;
            this.superSpinInitialFeatherPerPanelTime = 8;
            this.superSpinFinalFeatherPerPanelTime = 4;
            this.superSpinPowerupFinalFeatherPerPanelTime = 8;
            this.superSpinInitialFeatherDelay = 60;
            this.superSpinFinalFeatherDelay = 15;
            this.superSpinMinFeatherIntervalFactor = 1.8;
            this.superSpinMaxFeatherIntervalFactor = 2.4;
            this.superSpinSpinupTime = 240;
            this.superSpinDuration = 240;
            this.superSpinPowerupTornadoDelay = 180;
            this.superSpinPowerupTornadoLifetime = 10;

            this.standardAttackWeights[AttackType.Dive] = 4;
            this.standardAttackWeights[AttackType.CrossDive] = 4;
            this.standardAttackWeights[AttackType.IceCrash] = 2;
            this.standardAttackWeights[AttackType.Spin] = 3;
            this.standardAttackWeights[AttackType.PowerUp] = 2;
            this.standardAttackWeights[AttackType.SuperDive] = 0;
            this.standardAttackWeights[AttackType.SuperSpin] = 0;

            this.poweredAttackWeights[AttackType.Dive] = 3;
            this.poweredAttackWeights[AttackType.CrossDive] = 3;
            this.poweredAttackWeights[AttackType.IceCrash] = 3;
            this.poweredAttackWeights[AttackType.Spin] = 3;
            this.poweredAttackWeights[AttackType.PowerUp] = 0;
            this.poweredAttackWeights[AttackType.SuperDive] = 2;
            this.poweredAttackWeights[AttackType.SuperSpin] = 2;
        }

        private void SetVersionStats()
        {
            this.SetDefaultVersionStats();

            if (this.version == 1)
            {
                // Use defaults
            }
            else
            {
                this.name = ShanghaiEXE.Translate("Enemy.CirnoDXName");
                this.power = 200;
                this.hp = 2600;
            }
        }

        // TODO:
        private void SetVersionDrops()
        {
            if (this.version == 1)
            {
                this.dropchips[0].chip = new CirnoV1(this.sound);
                this.dropchips[0].codeNo = 0;
                this.dropchips[1].chip = new CirnoV1(this.sound);
                this.dropchips[1].codeNo = 0;
                this.dropchips[2].chip = new CirnoV1(this.sound);
                this.dropchips[2].codeNo = 0;
                this.dropchips[3].chip = new CirnoV1(this.sound);
                this.dropchips[3].codeNo = 0;
                this.dropchips[4].chip = new CirnoV1(this.sound);
                this.dropchips[4].codeNo = 0;
                this.havezenny = 2600;
            }
            else
            {
                this.dropchips[0].chip = new CirnoV1(this.sound);
                this.dropchips[0].codeNo = 0;
                this.dropchips[1].chip = new CirnoV1(this.sound);
                this.dropchips[1].codeNo = 0;
                this.dropchips[2].chip = new CirnoV1(this.sound);
                this.dropchips[2].codeNo = 0;
                this.dropchips[3].chip = new CirnoV1(this.sound);
                this.dropchips[3].codeNo = 0;
                this.dropchips[4].chip = new CirnoV1(this.sound);
                this.dropchips[4].codeNo = 0;
                this.havezenny = 2600;
            }
        }
        
        private void SetDynamicAttackWeights()
        {
            if (this.version == 1)
            {
                if (this.Hp < this.HpMax / 3)
                {
                    this.standardAttackWeights[AttackType.Dive] = 2;
                    this.standardAttackWeights[AttackType.CrossDive] = 2;
                    this.standardAttackWeights[AttackType.IceCrash] = 2;
                    this.standardAttackWeights[AttackType.Spin] = 2;
                    this.standardAttackWeights[AttackType.PowerUp] = 2;
                    this.standardAttackWeights[AttackType.SuperDive] = 0;
                    this.standardAttackWeights[AttackType.SuperSpin] = 0;

                    this.poweredAttackWeights[AttackType.Dive] = 2;
                    this.poweredAttackWeights[AttackType.CrossDive] = 2;
                    this.poweredAttackWeights[AttackType.IceCrash] = 2;
                    this.poweredAttackWeights[AttackType.Spin] = 2;
                    this.poweredAttackWeights[AttackType.PowerUp] = 0;
                    this.poweredAttackWeights[AttackType.SuperDive] = 2;
                    this.poweredAttackWeights[AttackType.SuperSpin] = 2;
                }
            }
            else
            {
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
        }

        private void AttackDelaySet()
        {
            this.attackDelay = this.attackDelayBase + this.Random.Next(-this.attackDelayFuzz, this.attackDelayFuzz);
        }

        private void AttackCooldownSet()
        {
            this.waittime = 0;
            this.animationpoint = new Point(1, 0);
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

        private enum AttackState
        {
            Idle,
            Attack,
            Cooldown
        }

        private enum AttackType
        {
            Dive,
            CrossDive,
            IceCrash,
            Spin,
            PowerUp,
            SuperDive,
            SuperSpin
        }

        private class DiveFeatherSpawner : AttackBase
        {
            private int hittime;
            private int hitdelay;
            private int sets;
            private int count;

            private CharacterBase targetedEnemy;

            public DiveFeatherSpawner(
                IAudioEngine so,
                SceneBattle p,
                Panel.COLOR u,
                int po,
                int hittime,
                int hitdelay,
                int sets,
                int count,
                ChipBase.ELEMENT ele)
                : base(so, p, 0, 0, u, po, ele)
            {
                this.hittime = hittime;
                this.hitdelay = hitdelay;
                this.sets = sets;
                this.count = count;

                var targets = p.AllChara().Where(c => c.union != this.union);
                if (targets.Any())
                {
                    var selectedEnemyIndex = Random.Next(0, targets.Count());
                    this.targetedEnemy = targets.Skip(selectedEnemyIndex).First();
                }
            }

            public override void Updata()
            {
                if (this.frame / this.hitdelay >= sets)
                {
                    this.flag = false;
                    return;
                }

                if (this.frame % this.hitdelay == 0)
                {
                    var randomTargets = this.RandomMultiPanel(this.count, this.UnionEnemy);
                    var targetedPosition = new[] { targetedEnemy?.position ?? new Point(-1, -1) }.Where(p => p.X != -1);
                    var targets = targetedPosition.Concat(randomTargets.Where(p => p != targetedEnemy?.position)).Take(this.count);

                    var sharedAngle = DiveFeather.GenerateRandomAngle(Random, this.union);
                    foreach (var target in targets)
                    {
                        this.parent.attacks.Add(new DiveFeather(this.sound, this.parent, target.X, target.Y, this.union, this.power, this.hittime, this.element, sharedAngle));
                    }
                }

                this.FlameControl();
            }
        }

        private class DiveFeather : AttackBase
        {
            public static readonly Rectangle FeatherFullFrameRect = new Rectangle(515, 711, 22, 10);
            public static readonly Rectangle FeatherShadowRect = new Rectangle(524, 734, 9, 4);
            public static readonly Vector2 PanelsOffset = new Vector2(20, 80);

            private readonly Vector2 originPosition;
            private readonly Vector2 movementPerFrame;
            private readonly Vector2 targetPosition;
            private readonly IList<Feather> individualFeathers;
            private readonly float spriteRotation;
            private readonly Vector2 shadowOffset;

            private int hitTime;
            private bool impacted;

            public DiveFeather(
                IAudioEngine so,
                SceneBattle p,
                int pX,
                int pY,
                Panel.COLOR u,
                int po,
                int hittime,
                ChipBase.ELEMENT ele,
                float? angleDegrees = null)
                : base(so, p, pX, pY, u, po, ele)
            {
                this.picturename = "cirnobx";
                this.invincibility = false;
                this.hitrange = new Point(0, 0);
                this.breaking = false;

                this.hitting = false;

                this.hitTime = hittime;

                this.targetPosition = new Vector2(position.X * 40.0f + 5 * (this.union == Panel.COLOR.blue ? 1 : -1), position.Y * 24.0f + -4);

                this.spriteRotation = angleDegrees ?? GenerateRandomAngle(Random, u);
                var angle = this.spriteRotation * (Math.PI / 180.0);
                var distance = (float)(Math.Min(hittime, 36) * 12); // if shorter than 36, will move faster but still guarantee starting offscreen
                this.originPosition = this.targetPosition + (new Vector2((float)Math.Cos(angle), -(float)Math.Sin(angle)) * distance);
                this.movementPerFrame = (this.targetPosition - this.originPosition) / this.hitTime;
                this.shadowOffset = new Vector2((float)(11 * -Math.Cos(angle) + 1), (float)(11 * Math.Sin(angle)));

                this.individualFeathers = new List<Feather>();

                var tangentAngle = angle + Math.PI / 2;
                var withinPanelSpreadHypotenuse = 40 / 2;
                var adjacentSpreadMax = withinPanelSpreadHypotenuse * Math.Cos(tangentAngle);
                var spread = (float)adjacentSpreadMax;

                var gaussianRandom = new Random();

                var featherCount = Random.Next(12, 18);
                for (var i = 0; i < featherCount; i++)
                {
                    var tangentMagnitude = (float)NextGaussian(gaussianRandom, 0, 1 / 2.5) * spread;
                    var tangentOffset = new Vector2((float)Math.Cos(tangentAngle), -(float)Math.Sin(tangentAngle)) * tangentMagnitude;

                    var offCenter = ((2.5 - Math.Abs(tangentMagnitude / spread)) / 2.5);
                    var yOffset = (float)NextGaussian(gaussianRandom, 0, offCenter) * 24 / 8;

                    var finalPosition = this.originPosition + tangentOffset;
                    this.individualFeathers.Add(new Feather
                    {
                        HitTime = (this.targetPosition.Y - finalPosition.Y) / this.movementPerFrame.Y,
                        PositionDirect = finalPosition + new Vector2(0, yOffset)
                    });
                }

                this.individualFeathers = this.individualFeathers.OrderBy(f => f.PositionDirect.Y).ToList();
            }

            public override void Updata()
            {
                if (this.over)
                    return;

                for (int i = 0; i < this.individualFeathers.Count; i++)
                {
                    var proposedPosition = this.individualFeathers[i].PositionDirect + this.movementPerFrame;
                    if (this.frame > this.individualFeathers[i].HitTime)
                    {
                        continue;
                    }
                    else if (this.frame + 1 > this.individualFeathers[i].HitTime)
                    {
                        var partialMovement = this.movementPerFrame * (float)(this.individualFeathers[i].HitTime - this.frame);
                        this.individualFeathers[i].PositionDirect += partialMovement;
                    }
                    else
                    {
                        this.individualFeathers[i].PositionDirect = proposedPosition;
                    }
                }

                if (this.impacted)
                {
                    this.hitting = false;
                    if (this.StandPanel.Hole || this.frame > this.hitTime + 24)
                    {
                        this.flag = false;
                    }
                    else if (this.frame >= this.hitTime + 12)
                    {
                        this.rend = !this.rend;
                    }
                }
                else
                {
                    if (this.frame % 5 == 0)
                        this.bright = !this.bright;
                    if (this.bright)
                        this.PanelBright();

                    if (this.frame >= this.hitTime)
                    {
                        this.hitting = true;
                        this.impacted = true;

                        this.sound.PlaySE(SoundEffect.shotwave);
                    }
                }
                this.FlameControl();
            }

            public override void Render(IRenderer dg)
            {
                if (this.over || !this.flag)
                    return;

                foreach (var feather in this.individualFeathers)
                {
                    var yRemaining = Math.Max(0, this.movementPerFrame.Y * (feather.HitTime - this.frame));

                    if (yRemaining < 36)
                    {
                        var shadowPosition = new Vector2(feather.PositionDirect.X, feather.PositionDirect.Y + (float)yRemaining);
                        var spriteOffsetPosition = shadowPosition + this.shadowOffset + DiveFeather.PanelsOffset + new Vector2(this.Shake.X, this.Shake.Y);

                        dg.DrawImage(
                            dg,
                            this.picturename,
                            DiveFeather.FeatherShadowRect,
                            false,
                            spriteOffsetPosition,
                            1.0f,
                            0,
                            this.color);
                    }
                }

                foreach (var feather in this.individualFeathers)
                {
                    var spriteOffsetPosition = feather.PositionDirect + DiveFeather.PanelsOffset + new Vector2(this.Shake.X, this.Shake.Y);

                    dg.DrawImage(
                        dg,
                        this.picturename,
                        DiveFeather.FeatherFullFrameRect,
                        false,
                        spriteOffsetPosition,
                        1.0f,
                        this.spriteRotation,
                        this.color);
                }
            }

            public override bool HitCheck(Point charaposition, Panel.COLOR charaunion)
            {
                if (!base.HitCheck(charaposition, charaunion))
                    return false;
                this.flag = false;
                return true;
            }

            public override bool HitCheck(Point charaposition)
            {
                if (!base.HitCheck(charaposition))
                    return false;
                this.flag = false;
                return true;
            }

            public static float GenerateRandomAngle(Random rand, Panel.COLOR union)
            {
                const double MinAngle = 30;
                const double MaxAngle = 60;

                var randomAngle = rand.NextDouble() * (MaxAngle - MinAngle) + MinAngle;

                return (float)(union == Panel.COLOR.blue ? randomAngle : 90 - randomAngle);
            }

            private static double NextGaussian(Random r, double mean = 0, double stddev = 1)
            {
                var u1 = r.NextDouble();
                var u2 = r.NextDouble();

                var rand_std_normal = Math.Sqrt(-2.0 * Math.Log(u1)) *
                                    Math.Sin(2.0 * Math.PI * u2);

                var rand_normal = mean + stddev * rand_std_normal;

                return rand_normal;
            }

            private class Feather
            {
                public double HitTime { get; set; }
                public Vector2 PositionDirect { get; set; }
            }
        }

        private class DiveBomber : EffectBase
        {
            public DiveBomber(IAudioEngine s, SceneBattle p, Point posi)
              : base(s, p, posi.X, posi.Y)
            {
                this.speed = 5;

                this.picturename = "cirnobx";
                this.animationpoint = new Point(0, 6);
                this.positionDirect = new Vector2(position.X * 40.0f + 0, position.Y * 24.0f + 0);
            }

            private Vector2 SpritePositionDirect => this.positionDirect + CirnoBX.SpriteOffset;

            public override void PositionDirectSet()
            {
                this.positionDirect = new Vector2(position.X * 40.0f + 0, position.Y * 24.0f + 0);
            }

            public override void Updata()
            {
                this.animationpoint.X = this.frame;
                this.PositionDirectSet();
                if (this.frame >= 5)
                    this.flag = false;
                this.FlameControl();
            }

            public override void Render(IRenderer dg)
            {
                if (!this.flag)
                {
                    return;
                }

                var spriteOffsetPosition = this.SpritePositionDirect + new Vector2(this.Shake.X, this.Shake.Y);
                dg.DrawImage(
                    dg,
                    this.picturename,
                    new Rectangle(FrameCoordX(this.animationpoint.X), FrameCoordY(this.animationpoint.Y), FullFrameRect.Width, FullFrameRect.Height),
                    false,
                    spriteOffsetPosition,
                    this.color);
            }
        }

        private class IceCrashSpawner : AttackBase
        {
            private readonly int duration;
            private readonly int largeIceHitTime;
            private readonly int largeIceLifetime;
            private readonly int largeIceHp;
            private readonly int largeIceRemnantHp;
            private readonly int smallIceHitTime;
            private readonly int smallIceSpawnDelay;
            private readonly int additionalLargeIceSpawnDelay;

            public IceCrashSpawner(
                IAudioEngine so,
                SceneBattle p,
                Panel.COLOR u,
                int po,
                ChipBase.ELEMENT ele,
                int duration,
                int largeIceHitTime,
                int largeIceLifetime,
                int largeIceHp,
                int largeIceRemnantHp,
                int smallIceHitTime,
                int smallIceSpawnTime,
                int additionalLargeIceSpawnDelay)
                : base(so, p, 0, 0, u, po, ele)
            {
                this.duration = duration;
                this.largeIceHitTime = largeIceHitTime;
                this.largeIceLifetime = largeIceLifetime;
                this.largeIceHp = largeIceHp;

                this.smallIceHitTime = smallIceHitTime;
                this.smallIceSpawnDelay = smallIceSpawnTime;
                this.largeIceRemnantHp = largeIceRemnantHp;
                this.additionalLargeIceSpawnDelay = additionalLargeIceSpawnDelay;
            }

            public override void Updata()
            {
                if (this.frame > this.duration)
                {
                    this.flag = false;
                    return;
                }

                var additionalLargeSpawn = this.frame != 0 && this.additionalLargeIceSpawnDelay != -1 && this.frame % this.additionalLargeIceSpawnDelay == 0;
                if (this.frame == 0 || additionalLargeSpawn)
                {
                    var allPanelPositions = Enumerable.Range(0, this.parent.panel.GetLength(0)).SelectMany(x => Enumerable.Range(0, this.parent.panel.GetLength(1)).Select(y => new Point(x, y))).ToArray();
                    var enemyPanels = allPanelPositions.Where(p => this.parent.panel[p.X, p.Y].color != this.union).ToArray();

                    var validTargets = allPanelPositions.Where(pos =>
                    {
                        var attackPositions = Enumerable.Range(0, 2).SelectMany(xOff => Enumerable.Range(0, 2).Select(yOff => new Point(pos.X + xOff, pos.Y + yOff))).ToArray();

                        var isOutOfBounds = attackPositions.Any(p => p.X >= this.parent.panel.GetLength(0) || p.Y >= this.parent.panel.GetLength(1) || this.parent.panel[p.X, p.Y].State == Panel.PANEL._un);
                        var hitsEnemyArea = attackPositions.Any(enemyPanels.Contains);
                        var freeSpaces = enemyPanels.Where(p => this.parent.panel[p.X, p.Y].State != Panel.PANEL._un).Except(attackPositions);
                        var willBlockArea = freeSpaces.Count() < 2;
                        if (additionalLargeSpawn)
                        {
                            var occupiedPanels = this.parent.AllHitter().OfType<IceRockLarge>().SelectMany(o => new[] { "00", "01", "10", "11" }.Select(off => new Point(o.position.X + (off[0] == '0' ? 0 : 1), o.position.Y + (off[1] == '0' ? 0 : 1))));
                            willBlockArea |= freeSpaces.Except(attackPositions).Except(occupiedPanels).Count() <= 2;
                            hitsEnemyArea &= attackPositions.Except(occupiedPanels).Any();
                        }

                        return hitsEnemyArea && !isOutOfBounds && !willBlockArea;
                    }).ToList();

                    if (validTargets.Any())
                    {
                        // Occupy the max number of enemy panels
                        var maxPanelsHit = validTargets.Max(pos =>
                        {
                            var attackPositions = Enumerable.Range(0, 2).SelectMany(xOff => Enumerable.Range(0, 2).Select(yOff => new Point(pos.X + xOff, pos.Y + yOff))).ToArray();
                            return enemyPanels.Intersect(attackPositions).Count();
                        });

                        validTargets.RemoveAll(pos =>
                        {
                            var attackPositions = Enumerable.Range(0, 2).SelectMany(xOff => Enumerable.Range(0, 2).Select(yOff => new Point(pos.X + xOff, pos.Y + yOff))).ToArray();
                            return enemyPanels.Intersect(attackPositions).Count() < maxPanelsHit;
                        });
                    }
                    else
                    {
                        // Block enemy panels without locking into 1 space
                        var frontmostPanels = enemyPanels.GroupBy(p => p.Y).Select(gr => gr.OrderBy(p => this.union == Panel.COLOR.blue ? -p.X : p.X).First());

                        validTargets.AddRange(frontmostPanels.Select(p => p.WithOffset(this.union == Panel.COLOR.blue ? 1 : -1, 0)).Where(pos =>
                        {
                            var attackPositions = Enumerable.Range(0, 2).SelectMany(xOff => Enumerable.Range(0, 2).Select(yOff => new Point(pos.X + xOff, pos.Y + yOff))).ToArray();

                            var isOutOfBounds = attackPositions.Any(p => p.X >= this.parent.panel.GetLength(0) || p.Y >= this.parent.panel.GetLength(1) || this.parent.panel[p.X, p.Y].State == Panel.PANEL._un);

                            return !isOutOfBounds;
                        }));
                    }

                    var targetedPosition = validTargets[this.Random.Next(0, validTargets.Count)];

                    this.parent.attacks.Add(new IceCrashLarge(this.sound, this.parent, targetedPosition.X, targetedPosition.Y, this.union, this.power, this.largeIceHitTime, this.element, this.largeIceLifetime, this.largeIceRemnantHp, this.largeIceHp));
                }
                else if (this.frame % this.smallIceSpawnDelay == 0)
                {
                    var allPanelPositions = Enumerable.Range(0, this.parent.panel.GetLength(0)).SelectMany(x => Enumerable.Range(0, this.parent.panel.GetLength(1)).Select(y => new Point(x, y))).ToArray();
                    var enemyPanels = allPanelPositions.Where(p => this.parent.panel[p.X, p.Y].color != this.union).ToArray();

                    var targetPanels = enemyPanels.Where(pos =>
                    {
                        var existingIcePresent = this.parent.AllHitter().Any(o => (o is IceRockLarge || o is DummyObject || o is IceRocks) && o.position == pos);
                        var iceIncoming = this.parent.attacks.Any(a =>
                        {
                            var attackPositions = Enumerable.Range(0, 2).SelectMany(xOff => Enumerable.Range(0, 2).Select(yOff => new Point(a.position.X + xOff, a.position.Y + yOff)));
                            return a is IceCrashLarge && attackPositions.Contains(pos);
                        });
                        return !existingIcePresent && !iceIncoming;
                    }).ToArray();
                    var targetPos = targetPanels[this.Random.Next(0, targetPanels.Length)];

                    this.parent.attacks.Add(new IceSpikeBX(this.sound, this.parent, targetPos.X, targetPos.Y, this.union, this.power, this.smallIceHitTime, this.element, false, 0, 0));
                }

                this.FlameControl();
            }
        }

        private class IceCrashLarge : AttackBase
        {
            private static readonly float DropAcceleration = 9.8f / 60;
            private static readonly int MinHeight = 160;

            private readonly int lifetime;
            private readonly int hitTime;
            private readonly Vector2 initialPosition;
            private readonly float initialVelocity;

            private readonly int smallIceHp;
            private readonly int largeIceHp;

            private bool hasShadow;

            public IceCrashLarge(
                IAudioEngine so,
                SceneBattle p,
                int pX,
                int pY,
                Panel.COLOR u,
                int po,
                int hittime,
                ChipBase.ELEMENT ele,
                int lifetime,
                int smallIceHp,
                int largeIceHp)
                : base(so, p, pX, pY, u, po, ele)
            {
                this.picturename = "cirnobx";
                this.invincibility = true;
                this.hitrange = new Point(1, 1);
                this.breaking = true;

                this.hitting = false;

                this.lifetime = lifetime;
                this.smallIceHp = smallIceHp;
                this.largeIceHp = largeIceHp;

                this.hitTime = hittime;

                this.initialVelocity = (float)((160 - (0.5 * DropAcceleration * this.hitTime * this.hitTime)) / this.hitTime);

                this.initialPosition = new Vector2(40 * this.position.X + 20, 24 * this.position.Y + 12);
                this.initialPosition.Y -= MinHeight;
                this.positionDirect = new Vector2(this.initialPosition.X, this.initialPosition.Y);
            }

            public override void Updata()
            {
                if (this.over)
                    return;

                this.hasShadow = !this.parent.AllHitter().OfType<IceRockLarge>().Any(c => c.position == this.position);

                if (this.frame == 0)
                {
                    for (var xOff = 0; xOff <= 1; xOff++)
                    {
                        for (var yOff = 0; yOff <= 1; yOff++)
                        {
                            var point = new Point(this.position.X + xOff, this.position.Y + yOff);
                            this.parent.attacks.Add(new Dummy(this.sound, this.parent, point.X, point.Y, this.union, Point.Empty, this.hitTime, true));
                        }
                    }
                }

                if (this.frame > this.hitTime)
                {
                    this.flag = false;

                    var targetedPanels = new List<Point>();
                    for (var xOff = 0; xOff <= 1; xOff++)
                    {
                        for (var yOff = 0; yOff <= 1; yOff++)
                        {
                            var point = new Point(this.position.X + xOff, this.position.Y + yOff);
                            targetedPanels.Add(point);

                            var dir = xOff == 0 ? DIRECTION.left : DIRECTION.right;
                            var enemyHit = this.StateCopy(new DragEnemyHit(this.sound, this.parent, point.X, point.Y, this.union, this.power, this.element, this, dir));
                            this.parent.attacks.Add(enemyHit);
                        }
                    }

                    var hitObjects = this.parent.AllHitter().Where(c => targetedPanels.Contains(c.position) || (c.positionReserved != null && targetedPanels.Contains(c.positionReserved.Value)));
                    if (hitObjects.Any(o => !(o is IceRockLarge || o is DummyObject || o is IceRocks)))
                    {
                        var emptySpaces = targetedPanels.Except(hitObjects.Select(e => e.position));
                        var isPinned = hitObjects.Select(e => e.position).Any(p =>
                            new[] { new Point(0, 1), new Point(0, -1), new Point(1, 0), new Point(-1, 0) }.Select(off => p.WithOffset(off.X, off.Y))
                            .All(adj => !(adj.X >= 0 && adj.X < 6 && adj.Y >= 0 && adj.Y < 3) || this.parent.panel[adj.X, adj.Y].color == this.union || this.parent.OnPanelCheck(adj.X, adj.Y, true) || emptySpaces.Contains(adj))
                        );
                        foreach (var space in emptySpaces)
                        {
                            this.parent.objects.Add(new IceRocks(this.sound, this.parent, space.X, space.Y, this.union, this.lifetime / (isPinned ? 2 : 1), this.smallIceHp));
                        }

                        // create debris
                        var pdX = this.positionDirect.X + CirnoBX.SpriteOffset.X;
                        var pdY = this.positionDirect.Y + CirnoBX.SpriteOffset.Y;
                        var factory = BreakIceRock.MakeOffsetFactory(this.sound, this.parent, this.position, this.union);

                        var fragmentStages = new[]
                        {
                            Tuple.Create(1, new[] { BreakIceRock.DebrisType.LargeLeft, BreakIceRock.DebrisType.LargeRight }),
                            Tuple.Create(4, new[] { BreakIceRock.DebrisType.FragLumpy, BreakIceRock.DebrisType.FragRound, BreakIceRock.DebrisType.FragSharp }),
                            Tuple.Create(8, new[] { BreakIceRock.DebrisType.ClodFlatLeft, BreakIceRock.DebrisType.ClodFlatRight, BreakIceRock.DebrisType.ClodRound, BreakIceRock.DebrisType.ClodSharp }),
                            Tuple.Create(16, new[] { BreakIceRock.DebrisType.ChipRound, BreakIceRock.DebrisType.ChipScale, BreakIceRock.DebrisType.ChipSharp, BreakIceRock.DebrisType.ChunkSharp, BreakIceRock.DebrisType.ChunkTall }),
                        };

                        var breakOffsets = new[] { new Vector2(12, -32), new Vector2(-5, -14), new Vector2(8, -56) };
                        foreach (var offset in breakOffsets)
                        {
                            foreach (var stage in fragmentStages)
                            {
                                var fragmentCount = this.Random.Next(stage.Item1 / 4, stage.Item1);
                                for (int i = 0; i < fragmentCount; i++)
                                {
                                    var isRight = Random.Next() % 2 == 0;
                                    this.parent.effects.Add(factory.Invoke(
                                        pdX + offset.X + Random.Next(-5, 6),
                                        pdY + offset.Y + Random.Next(-5, 6),
                                        Random.Next(-32, 32),
                                        Random.Next(16, 48),
                                        Random.Next(20, 48),
                                        stage.Item2[Random.Next(stage.Item2.Length)]));
                                }
                            }
                        }
                    }
                    else
                    {
                        var collidingIceRocks = this.parent.AllHitter().OfType<ObjectBase>().Where(o => new Rectangle(this.position, new Size(2, 2)).IntersectsWith(new Rectangle(o.position, o is IceRockLarge ? new Size(2, 2) : new Size(1, 1))));
                        foreach (var rock in collidingIceRocks)
                        {
                            rock.Break();
                        }

                        this.sound.PlaySE(SoundEffect.bomb);
                        this.parent.objects.Add(new IceRockLarge(this.sound, this.parent, this.position.X, this.position.Y, this.union, this.lifetime, this.largeIceHp));
                    }
                }

                var t = this.frame;
                this.positionDirect = this.initialPosition + new Vector2(0, (float)((this.initialVelocity * t) + (0.5 * DropAcceleration * t * t)));

                this.FlameControl();
            }

            public override void Render(IRenderer dg)
            {
                if (this.over || !this.flag)
                    return;

                if (this.hasShadow)
                {
                    var shadowRect = new Rectangle(FrameCoordX(5), FrameCoordY(6), FullFrameRect.Width, FullFrameRect.Height);
                    var shadowPosition = new Vector2(this.initialPosition.X, this.initialPosition.Y + MinHeight);
                    var shadowOffsetPosition = shadowPosition + CirnoBX.SpriteOffset + new Vector2(this.Shake.X, this.Shake.Y);

                    dg.DrawImage(
                        dg,
                        this.picturename,
                        shadowRect,
                        false,
                        shadowOffsetPosition,
                        1.0f,
                        0,
                        this.color);
                }

                var rockRect = new Rectangle(FrameCoordX(3), FrameCoordY(4), FullFrameRect.Width, FullFrameRect.Height);
                var spriteOffsetPosition = this.positionDirect + CirnoBX.SpriteOffset + new Vector2(this.Shake.X, this.Shake.Y);

                dg.DrawImage(
                    dg,
                    this.picturename,
                    rockRect,
                    false,
                    spriteOffsetPosition,
                    1.0f,
                    0,
                    this.color);
            }
        }

        internal class IceSpikeBX : AttackBase
        {
            private static readonly float DropAcceleration = 9.8f / 60;
            private static readonly int InitialHeight = 160;

            private readonly Shadow shadow;
            private readonly int hitTime;
            private readonly Vector2 initialPosition;
            private readonly bool createObject;
            private readonly float initialVelocity;
            private readonly int smallIceLifetime;
            private readonly int smallIceHp; 

            private bool isEnding;

            public IceSpikeBX(
                IAudioEngine so,
                SceneBattle p,
                int pX,
                int pY,
                Panel.COLOR u,
                int po,
                int hittime,
                ChipBase.ELEMENT ele,
                bool createObject,
                int lifetime,
                int hp)
                : base(so, p, pX, pY, u, po, ele)
            {
                this.invincibility = false;
                this.breaking = false;

                this.hitting = false;
                this.hitTime = hittime;

                this.initialVelocity = (float)((160 - (0.5 * DropAcceleration * this.hitTime * this.hitTime)) / this.hitTime);

                this.initialPosition = new Vector2(40 * this.position.X, 24 * this.position.Y);
                this.initialPosition.Y -= InitialHeight;
                this.positionDirect = new Vector2(this.initialPosition.X, this.initialPosition.Y);

                this.createObject = createObject;
                this.smallIceLifetime = lifetime;
                this.smallIceHp = hp;

                this.shadow = new Shadow(this.sound, this.parent, this.position.X, this.position.Y, this);
                this.parent.effects.Add(shadow);
            }

            public override void Updata()
            {
                if (this.over)
                    return;
                
                if (!this.isEnding && this.frame > this.hitTime)
                {
                    this.isEnding = true;

                    //this.parent.attacks.Add(this.StateCopy(new BombAttack(this.sound, this.parent, this.position.X, this.position.Y, this.union, this.power, 1, this.element)));
                    this.hitting = true;

                    this.shadow.rend = !this.StandPanel.Hole && !this.isEnding;

                    if (!this.StandPanel.Hole)
                    {
                        if (this.createObject)
                        {
                            var attackHits = this.parent.AllHitter().Where(c => this.position.Equals(c.position) || (c.positionReserved != null && this.position.Equals(c.positionReserved.Value))).ToArray();
                            if (attackHits.Any(c => !(c is IceRocks)))
                            {
                                // do nothing, spike hits person/obj and disppears
                            }
                            else
                            {
                                // create object
                                foreach (var c in attackHits.OfType<IceRocks>())
                                {
                                    c.Break();
                                }

                                this.sound.PlaySE(SoundEffect.knock);
                                this.parent.objects.Add(new IceRocks(this.sound, this.parent, this.position.X, this.position.Y, this.union, this.smallIceLifetime, this.smallIceHp));
                            }
                        }
                        else
                        {
                            // create break effect
                            var pdX = this.positionDirect.X + CirnoBX.SpriteOffset.X;
                            var pdY = this.positionDirect.Y + CirnoBX.SpriteOffset.Y;
                            var factory = BreakIceRock.MakeLeftRightFactory(this.sound, this.parent, this.position, this.union);

                            this.sound.PlaySE(SoundEffect.clincher);

                            var fragmentTypes = new[] { BreakIceRock.DebrisType.ChunkTall, BreakIceRock.DebrisType.ChipSharp, BreakIceRock.DebrisType.ClodSharp, BreakIceRock.DebrisType.ChunkSharp, BreakIceRock.DebrisType.ClodFlatLeft, BreakIceRock.DebrisType.ClodFlatRight, BreakIceRock.DebrisType.FragLumpy };

                            this.parent.effects.Add(factory.Invoke(
                                pdX + Random.Next(-8, 16),
                                pdY + Random.Next(8, 14),
                                Random.Next(16, 20),
                                Random.Next(20, 28),
                                false,
                                fragmentTypes[Random.Next(fragmentTypes.Length)]));
                            this.parent.effects.Add(factory.Invoke(
                                pdX + Random.Next(-16, 8),
                                pdY + Random.Next(8, 14),
                                Random.Next(16, 20),
                                Random.Next(20, 24),
                                true,
                                fragmentTypes[Random.Next(fragmentTypes.Length)]));
                            
                        }
                    }
                    else
                    {
                        // create disappear effect
                        // or do nothing
                    }
                }

                if (this.isEnding)
                {
                    if (!this.createObject)
                    {
                        this.flag = false;
                        this.shadow.flag = false;
                    }
                    else
                    {
                        // Hard cutoff line covered up by spawning object
                        var pxAboveGroundLevel = 60 - (this.positionDirect.Y - (this.initialPosition.Y + InitialHeight));
                        if (pxAboveGroundLevel < 0)
                        {
                            this.flag = false;
                            this.shadow.flag = false;
                        }
                    }
                }

                var t = this.frame;
                this.positionDirect = this.initialPosition + new Vector2(0, (float)((this.initialVelocity * t) + (0.5 * DropAcceleration * t * t)));

                this.FlameControl();
            }

            public override void Render(IRenderer dg)
            {
                if (this.over || !this.flag)
                    return;

                var spriteOffsetPosition = this.positionDirect + CirnoBX.SpriteOffset + new Vector2(this.Shake.X, this.Shake.Y);
                this._rect = new Rectangle(0, 392, 32, 48);
                this._position = spriteOffsetPosition;

                if (this.isEnding)
                {
                    var pxAboveGroundLevel = 60 - (this.positionDirect.Y - (this.initialPosition.Y + InitialHeight));
                    var pxRemoved = 60 - pxAboveGroundLevel;
                    var centerAdjustedHeight = pxAboveGroundLevel - pxRemoved;
                    this._rect.Height = Math.Max(0, (int)Math.Round(centerAdjustedHeight));
                }
                dg.DrawImage(dg, "towers", this._rect, false, this._position, this.rebirth, Color.White);
            }
        }

        private class IceRocks : ObjectBase
        {
            private readonly int lifetime;
            private bool breaked;

            public IceRocks(IAudioEngine s, SceneBattle p, int pX, int pY, Panel.COLOR union, int lifetime, int hp)
              : base(s, p, pX, pY, union)
            {
                this.height = 128;
                this.wide = 96;
                this.hp = hp;
                this.hitPower = 200;
                this.hpmax = this.hp;
                this.unionhit = false;
                this.overslip = false;
                this.lifetime = lifetime;

                this.positionDirect = new Vector2(position.X * 40.0f + 0, position.Y * 24.0f + 0);
                this.animationpoint = new Point(6, 6);

                this.picturename = "cirnobx";
            }

            public override void Updata()
            {
                base.Updata();

                var animFrame = this.frame / 5;
                if (animFrame < 4)
                {
                    this.animationpoint.X = 6 + animFrame;
                }

                if (this.frame > this.lifetime)
                {
                    this.Break();
                }

                this.FlameControl();
            }

            public override void Break()
            {
                if (!this.breaked || this.StandPanel.Hole)
                {
                    this.sound.PlaySE(SoundEffect.breakObject);
                    this.breaked = true;

                    var pdX = this.positionDirect.X + CirnoBX.SpriteOffset.X;
                    var pdY = this.positionDirect.Y + CirnoBX.SpriteOffset.Y;
                    var fuzzFactory = new Func<float, float, int, int, bool, BreakIceRock.DebrisType, BreakIceRock>(
                        (pX, pY, pZ, time, isRight, type) =>
                        {
                            Func<int> fuzz = () => this.Random.Next(-3, 4);
                            return BreakIceRock.MakeLeftRightFactory(this.sound, this.parent, this.position, this.union).Invoke(pX + fuzz(), pY + fuzz(), pZ + fuzz(), time + fuzz(), isRight, type);
                        });

                    this.parent.effects.Add(fuzzFactory.Invoke(pdX, pdY + 16, 12, 20, false, BreakIceRock.DebrisType.ChunkTall));
                    this.parent.effects.Add(fuzzFactory.Invoke(pdX, pdY + 16, 12, 20, true, BreakIceRock.DebrisType.ChunkSharp));
                    this.parent.effects.Add(fuzzFactory.Invoke(pdX, pdY + 20, 20, 10, false, BreakIceRock.DebrisType.FragSharp));
                    this.parent.effects.Add(fuzzFactory.Invoke(pdX, pdY + 14, 8, 24, true, BreakIceRock.DebrisType.ClodFlatRight));
                    this.parent.effects.Add(fuzzFactory.Invoke(pdX, pdY + 18, 16, 16, true, BreakIceRock.DebrisType.ClodSharp));
                    this.parent.effects.Add(fuzzFactory.Invoke(pdX, pdY + 8, 8, 10, false, BreakIceRock.DebrisType.ChipRound));
                    this.parent.effects.Add(fuzzFactory.Invoke(pdX, pdY + 12, 16, 8, true, BreakIceRock.DebrisType.ChipSharp));
                }
                this.flag = false;
            }

            public override void Render(IRenderer dg)
            {
                if (this.whitetime <= 0)
                    this._rect = new Rectangle(FrameCoordX(this.animationpoint.X), FrameCoordY(this.animationpoint.Y), this.wide, this.height);
                else
                    this._rect = new Rectangle(FrameCoordX(this.animationpoint.X), FrameCoordY(this.animationpoint.Y + 1), this.wide, this.height);

                var spriteOffsetPosition = this.positionDirect + CirnoBX.SpriteOffset + new Vector2(this.Shake.X, this.Shake.Y);
                this._position = spriteOffsetPosition;
                dg.DrawImage(dg, this.picturename, this._rect, false, this._position, this.rebirth, Color.White);
            }
        }

        private class IceRockLarge : ObjectBase
        {
            private const int BreakdownSteps = 3;
            private readonly List<DummyObject> dummyObjects;

            private readonly int lifetime;
            private bool breaked;
            private int shockwaveFrame;
            private int breakdowns;

            public IceRockLarge(IAudioEngine s, SceneBattle p, int pX, int pY, Panel.COLOR union, int lifetime, int hp)
              : base(s, p, pX, pY, union)
            {
                this.height = 128;
                this.wide = 96;
                this.hp = hp;
                this.hitPower = 200;
                this.hpmax = this.hp;
                this.unionhit = false;
                this.overslip = false;
                this.lifetime = lifetime;

                this.dummyObjects = new List<DummyObject>();
                for (var xOff = 0; xOff <= 1; xOff++)
                {
                    for (var yOff = 0; yOff <= 1; yOff++)
                    {
                        var dummy = new DummyObject(this.sound, this.parent, pX + xOff, pY + yOff, this.union, this);
                        this.parent.objects.Add(dummy);
                    }
                }

                this.positionDirect = new Vector2(40 * this.position.X + 20, 24 * this.position.Y + 12);
                this.animationpoint = new Point(4, 4);
                this.whitetime = 3;

                this.picturename = "cirnobx";
            }

            public override void Updata()
            {
                base.Updata();

                var animFrame = this.frame;
                switch (animFrame)
                {
                    case 0:
                        this.ShakeStart(4, 3);
                        this.animationpoint = new Point(4, 4);
                        break;
                    case 3:
                        this.animationpoint = new Point(3, 4);
                        break;
                    case 6:
                        this.shockwaveFrame = 1;
                        break;
                    case 9:
                        this.shockwaveFrame = 2;
                        break;
                    case 12:
                        this.shockwaveFrame = 0;
                        break;
                }

                for (int i = 0; i < BreakdownSteps; i++)
                {
                    if (this.breakdowns < i + 1)
                    {
                        if (this.frame > this.lifetime * (i + 1.0) / BreakdownSteps)
                        {
                            this.Hp -= (int)(this.HpMax * 1.0 / BreakdownSteps) + 1;
                        }

                        if (this.HpMax - this.Hp > this.HpMax * (i + 1.0) / BreakdownSteps)
                        {
                            this.whitetime = 3;
                            this.ShakeSingleStart(3, 3);
                            this.breakdowns++;

                            // create partial debris
                            var pdX = this.positionDirect.X + CirnoBX.SpriteOffset.X;
                            var pdY = this.positionDirect.Y + CirnoBX.SpriteOffset.Y;
                            var factory = BreakIceRock.MakeOffsetFactory(this.sound, this.parent, this.position, this.union);

                            var fragmentStages = new[]
                            {
                                Tuple.Create(1, new[] { BreakIceRock.DebrisType.LargeLeft, BreakIceRock.DebrisType.LargeRight }),
                                Tuple.Create(4, new[] { BreakIceRock.DebrisType.FragLumpy, BreakIceRock.DebrisType.FragRound, BreakIceRock.DebrisType.FragSharp }),
                                Tuple.Create(8, new[] { BreakIceRock.DebrisType.ClodFlatLeft, BreakIceRock.DebrisType.ClodFlatRight, BreakIceRock.DebrisType.ClodRound, BreakIceRock.DebrisType.ClodSharp }),
                                Tuple.Create(16, new[] { BreakIceRock.DebrisType.ChipRound, BreakIceRock.DebrisType.ChipScale, BreakIceRock.DebrisType.ChipSharp, BreakIceRock.DebrisType.ChunkSharp, BreakIceRock.DebrisType.ChunkTall }),
                            };

                            var breakOffsets = new[] { new Vector2(12, -32), new Vector2(-5, -14), new Vector2(8, -56) };

                            var offset = breakOffsets[i % breakOffsets.Length];
                            foreach (var stage in fragmentStages)
                            {
                                var fragmentCount = this.Random.Next(stage.Item1 / 4, stage.Item1);
                                for (int ii = 0; ii < fragmentCount; ii++)
                                {
                                    var isRight = Random.Next() % 2 == 0;
                                    this.parent.effects.Add(factory.Invoke(
                                        pdX + offset.X + Random.Next(-5, 6),
                                        pdY + offset.Y + Random.Next(-5, 6),
                                        Random.Next(-32, 32),
                                        Random.Next(16, 48),
                                        Random.Next(20, 48),
                                        stage.Item2[Random.Next(stage.Item2.Length)]));
                                }
                            }
                        }
                    }
                }
                if (this.Hp <= 0 || this.frame > this.lifetime)
                {
                    this.Break();
                }

                this.FlameControl();
            }

            public override void Break()
            {
                if (!this.breaked || this.StandPanel.Hole)
                {
                    this.sound.PlaySE(SoundEffect.breakObject);
                    this.breaked = true;

                    var pdX = this.positionDirect.X + CirnoBX.SpriteOffset.X;
                    var pdY = this.positionDirect.Y + CirnoBX.SpriteOffset.Y;
                    var factory = BreakIceRock.MakeOffsetFactory(this.sound, this.parent, this.position, this.union);

                    var fragmentStages = new[]
                    {
                        Tuple.Create(1, new[] { BreakIceRock.DebrisType.LargeLeft, BreakIceRock.DebrisType.LargeRight }),
                        Tuple.Create(3, new[] { BreakIceRock.DebrisType.FragLumpy, BreakIceRock.DebrisType.FragRound, BreakIceRock.DebrisType.FragSharp }),
                        Tuple.Create(9, new[] { BreakIceRock.DebrisType.ClodFlatLeft, BreakIceRock.DebrisType.ClodFlatRight, BreakIceRock.DebrisType.ClodRound, BreakIceRock.DebrisType.ClodSharp }),
                        Tuple.Create(27, new[] { BreakIceRock.DebrisType.ChipRound, BreakIceRock.DebrisType.ChipScale, BreakIceRock.DebrisType.ChipSharp, BreakIceRock.DebrisType.ChunkSharp, BreakIceRock.DebrisType.ChunkTall }),
                    };

                    var breakOffsets = new[] { new Vector2(12, -32), new Vector2(-5, -14), new Vector2(8, -56) };
                    var breaks = this.breakdowns % breakOffsets.Length;
                    for (var i = 0; i < breakOffsets.Length; i++)
                    {
                        var offset = breakOffsets[i];
                        offset.Y += 48;

                        var countMultiplier = 1.0;
                        if (i < breaks)
                        {
                            countMultiplier = 0.5;
                        }

                        foreach (var stage in fragmentStages)
                        {
                            var maxFragments = (int)Math.Round(stage.Item1 * countMultiplier);
                            var fragmentCount = this.Random.Next(maxFragments / 4, maxFragments);
                            for (int ii = 0; ii < fragmentCount; ii++)
                            {
                                var isRight = Random.Next() % 2 == 0;
                                this.parent.effects.Add(factory.Invoke(
                                    pdX + offset.X + Random.Next(-5, 6),
                                    pdY + offset.Y + Random.Next(-5, 6),
                                    Random.Next(-32, 32),
                                    Random.Next(16, 48),
                                    Random.Next(20, 48),
                                    stage.Item2[Random.Next(stage.Item2.Length)]));
                            }
                        }
                    }
                }
                this.flag = false;
                
                foreach (var dummy in this.dummyObjects)
                {
                    dummy.Break();
                    dummy.flag = false;
                }
            }

            public override void Render(IRenderer dg)
            {
                if (!this.flag)
                    return;

                var shadowAnimationPoint = new Point(5, 6);
                if (this.whitetime > 0)
                {
                    shadowAnimationPoint.Y += 1;
                }

                var shadowRect = new Rectangle(FrameCoordX(5), FrameCoordY(6), FullFrameRect.Width, FullFrameRect.Height);
                var shadowOffsetPosition = this.positionDirect + CirnoBX.SpriteOffset + new Vector2(this.Shake.X, this.Shake.Y);

                dg.DrawImage(
                    dg,
                    this.picturename,
                    shadowRect,
                    false,
                    shadowOffsetPosition,
                    1.0f,
                    0,
                    this.color);

                var shockwaveRect = new Rectangle?();
                switch (this.shockwaveFrame)
                {
                    case 1:
                        shockwaveRect = new Rectangle(FrameCoordX(3), FrameCoordY(1), FullFrameRect.Width, FullFrameRect.Height);
                        break;
                    case 2:
                        shockwaveRect = new Rectangle(FrameCoordX(4), FrameCoordY(1), FullFrameRect.Width, FullFrameRect.Height);
                        break;
                }
                if (shockwaveRect != null)
                {
                    var shockwavePosition = this.positionDirect + CirnoBX.SpriteOffset + new Vector2(this.Shake.X, this.Shake.Y);

                    dg.DrawImage(
                        dg,
                        this.picturename,
                        shockwaveRect.Value,
                        false,
                        shockwavePosition,
                        1.0f,
                        0,
                        this.color);
                }

                var adjustedAnimationPoint = new Point(this.animationpoint.X, this.animationpoint.Y);
                if (this.whitetime > 0)
                {
                    adjustedAnimationPoint.X += 6;
                }

                var rockRect = new Rectangle(FrameCoordX(adjustedAnimationPoint.X), FrameCoordY(adjustedAnimationPoint.Y), FullFrameRect.Width, FullFrameRect.Height);
                var spriteOffsetPosition = this.positionDirect + CirnoBX.SpriteOffset + new Vector2(this.Shake.X, this.Shake.Y);

                if (this.animationpoint.X == 3)
                {
                    rockRect.Height -= 30;
                    spriteOffsetPosition.Y += -15;
                }

                dg.DrawImage(
                    dg,
                    this.picturename,
                    rockRect,
                    false,
                    spriteOffsetPosition,
                    1.0f,
                    0,
                    this.color);
            }
        }

        private class DummyObject : ObjectBase
        {
            private ObjectBase parentObject;

            public DummyObject(IAudioEngine s, SceneBattle p, int pX, int pY, Panel.COLOR union, ObjectBase parentObject)
                : base(s, p, pX, pY, union)
            {
                this.hpmax = 100000;
                this.hp = 100000;
                this.noslip = true;

                this.parentObject = parentObject;
            }

            public override void Dameged(AttackBase attack)
            {
                if (attack is Dummy)
                {
                    return;
                }

                this.parentObject.Hp -= 100000 - this.hp;
                this.hp = 100000;
                this.parentObject.whitetime = this.whitetime;
                base.Dameged(attack);
            }

            public override void Updata()
            {
                if (this.hp < 100000)
                {
                    this.parentObject.Hp -= 100000 - this.hp;
                    this.hp = 100000;
                }

                this.counterTiming = this.parentObject.counterTiming;
                this.element = this.parentObject.Element;
                this.guard = this.parentObject.guard;
                this.badstatus = this.parentObject.badstatus;
                this.badstatustime = this.parentObject.badstatustime;
                this.flag = this.parentObject.flag;
            }

            public override void PositionDirectSet()
            {
            }

            public override void Render(IRenderer dg)
            {
            }
        }

        private class BreakIceRock : EffectBase
        {
            private const byte _speed = 1;

            private static readonly Dictionary<DebrisType, Rectangle> Sprites = new Dictionary<DebrisType, Rectangle>
            {
                { DebrisType.LargeLeft, new Rectangle(960, 768, 32, 32) },
                { DebrisType.LargeRight, new Rectangle(960 + 32, 768, 32, 32) },
                { DebrisType.ChunkTall, new Rectangle(960, 768 + 32 + 16, 16, 32) },
                { DebrisType.ChunkSharp, new Rectangle(960 + 16, 768 + 32 + 16, 32, 32) },
                { DebrisType.FragLumpy, new Rectangle(960 + 32 + 32, 768, 16, 16) },
                { DebrisType.FragRound, new Rectangle(960 + 32 + 32 + 16, 768, 16, 16) },
                { DebrisType.FragSharp, new Rectangle(960 + 32 + 32, 768 + 16 + 16, 16, 16) },
                { DebrisType.ClodRound, new Rectangle(960 + 32 + 32 + 16, 768 + 16, 16, 16) },
                { DebrisType.ClodFlatLeft, new Rectangle(960, 768 + 32, 16, 16) },
                { DebrisType.ClodFlatRight, new Rectangle(960 + 32 + 32 + 16, 768 + 32, 16, 16) },
                { DebrisType.ClodSharp, new Rectangle(960 + 16, 768 + 32, 16, 16) },
                { DebrisType.ChipRound, new Rectangle(960 + 32 + 32, 768 + 16, 16, 16) },
                { DebrisType.ChipSharp, new Rectangle(960 + 32, 768 + 32, 16, 16) },
                { DebrisType.ChipScale, new Rectangle(960 + 32 + 16, 768 + 32, 16, 16) }
            };

            private readonly int pZ;
            private readonly int time;
            private readonly Bound bound;
            private readonly DebrisType type;

            public BreakIceRock(
              IAudioEngine s,
              SceneBattle p,
              Point position,
              float pX,
              float pY,
              int pZ,
              Panel.COLOR union,
              int time,
              bool isRight,
              DebrisType type)
              : this(s, p, position, pX, pY, -32 * (!isRight ? -1 : 1), pZ, union, time, type)
            {
            }

            public BreakIceRock(
              IAudioEngine s,
              SceneBattle p,
              Point position,
              float pX,
              float pY,
              float dX,
              float dY,
              Panel.COLOR union,
              int time,
              DebrisType type)
              : base(s, p, position.X, position.Y)
            {
                this.type = type;
                this.time = time;
                this.union = union;
                this.speed = 1;
                this.positionDirect = new Vector2(pX, pY);
                if (union == Panel.COLOR.red)
                    this.rebirth = true;
                var endOffset = new Vector2(dX, dY);
                this.bound = new Bound(new Vector2(pX, pY), new Vector2(pX, pY) + endOffset, time);
            }

            public override void Updata()
            {
                this.positionDirect = this.bound.Update(this.positionDirect);
                if (this.frame > this.time)
                    this.flag = false;
                this.FlameControl();
            }

            public override void Render(IRenderer dg)
            {
                this._rect = Sprites[this.type];
                this._position = this.positionDirect;
                this._position.Y -= this.bound.plusy;
                dg.DrawImage(dg, "cirnobx", this._rect, false, this._position, !this.rebirth, Color.White);
            }

            public static Func<float, float, float, float, int, DebrisType, BreakIceRock> MakeOffsetFactory(IAudioEngine s, SceneBattle p, Point position, Panel.COLOR union)
            {
                return (pX, pY, dX, dY, time, type) => new BreakIceRock(s, p, position, pX, pY, dX, dY, union, time, type);
            }

            public static Func<float, float, int, int, bool, DebrisType, BreakIceRock> MakeLeftRightFactory(IAudioEngine s, SceneBattle p, Point position, Panel.COLOR union)
            {
                return (pX, pY, pZ, time, isRight, type) => new BreakIceRock(s, p, position, pX, pY, pZ, union, time, isRight, type);
            }

            public enum DebrisType
            {
                LargeLeft,
                LargeRight,
                ChunkTall,
                ChunkSharp,
                FragLumpy,
                FragRound,
                FragSharp,
                ClodRound,
                ClodFlatRight,
                ClodFlatLeft,
                ClodSharp,
                ChipRound,
                ChipSharp,
                ChipScale
            }
        }

        private class SpinFeather : AttackBase
        {
            private static readonly double panelAngle = Math.Atan(24.0 / 40.0);

            private readonly int throwDelay;
            private readonly int perPanelMoveTime;

            private readonly int initialX;
            private readonly float moveSpeed;

            public SpinFeather(
                IAudioEngine so,
                SceneBattle p,
                Panel.COLOR u,
                int po,
                int pX,
                int pY,
                int throwDelay,
                int perPanelMoveTime,
                ChipBase.ELEMENT ele)
                : base(so, p, pX, pY, u, po, ele)
            {
                this.throwDelay = throwDelay;
                this.perPanelMoveTime = perPanelMoveTime;

                this.moveSpeed = 40.0f / perPanelMoveTime;
                this.initialX = this.position.X;

                this.picturename = "cirnobx";
                this.animationpoint = new Point(5, 5);
                this.wide = 96;
                this.height = 128;
                this.positionDirect = new Vector2(position.X * 40.0f + 0, position.Y * 24.0f + 0);
                this.hitting = true;

                this.HasShadow = true;
            }

            public bool HasShadow { get; set; }

            public DIRECTION? DeflectDirection { get; set; }

            public bool IsThrown => this.waittime >= this.throwDelay;

            public override void Updata()
            {
                if (this.IsThrown)
                {
                    if (this.waittime == this.throwDelay)
                    {
                        this.sound.PlaySE(SoundEffect.knife);
                    }

                    var xDirectionMult = (this.union == Panel.COLOR.blue ? -1 : 1);
                    var yDirectionMult = this.DeflectDirection == DIRECTION.down ? 1 : -1;

                    var angle = this.DeflectDirection != null ? panelAngle : 0;
                    var xMoveSpeed = this.moveSpeed * Math.Cos(angle);
                    var yMoveSpeed = this.moveSpeed * Math.Sin(angle);

                    this.positionDirect.X += xDirectionMult * (float)xMoveSpeed;
                    this.positionDirect.Y += yDirectionMult * (float)yMoveSpeed;

                    Func<double, int> roundingFunc = x => (int)(this.union == Panel.COLOR.blue ? Math.Floor(x) : Math.Ceiling(x));
                    var centerOffset = xDirectionMult * -20;
                    this.position.X = roundingFunc((this.positionDirect.X + centerOffset) / 40); // this.initialX + directionMult * (int)Math.Floor((double)(this.waittime - this.throwDelay) / this.perPanelMoveTime);
                    if (this.DeflectDirection != null)
                    {
                        this.position.Y = (int)Math.Round(this.positionDirect.Y / 24);
                    }

                    this.PanelBright();
                    
                    if (this.positionDirect.X < 0 - 13 || this.positionDirect.X > 240 + 13)
                    {
                        this.flag = false;
                    }
                }

                this.waittime++;


            }

            public override void Render(IRenderer dg)
            {
                var adjustedPositionDirect = this.positionDirect + new Vector2(0, -18);

                if (this.HasShadow)
                {
                    var shadowPosition = new Vector2(adjustedPositionDirect.X, adjustedPositionDirect.Y + 24);
                    var shadowOffsetPosition = shadowPosition + DiveFeather.PanelsOffset + new Vector2(this.Shake.X, this.Shake.Y);

                    dg.DrawImage(
                        dg,
                        this.picturename,
                        DiveFeather.FeatherShadowRect,
                        false,
                        shadowOffsetPosition,
                        1.0f,
                        0,
                        this.color);
                }

                var spriteOffsetPosition = adjustedPositionDirect + DiveFeather.PanelsOffset + new Vector2(this.Shake.X, this.Shake.Y);

                var rotation = this.union == Panel.COLOR.blue ? 0f : 180f;
                rotation += (float)((this.DeflectDirection != null ? panelAngle : 0) * (this.union == Panel.COLOR.blue ^ this.DeflectDirection == DIRECTION.down ? -1 : 1) * (180f / Math.PI));
                dg.DrawImage(
                    dg,
                    this.picturename,
                    DiveFeather.FeatherFullFrameRect,
                    false,
                    spriteOffsetPosition,
                    1.0f,
                    rotation,
                    this.color);
            }

            public override bool HitEvent(Player p)
            {
                if (!base.HitEvent(p))
                    return false;
                this.flag = false;
                return true;
            }

            public override bool HitEvent(EnemyBase e)
            {
                if (!base.HitEvent(e))
                    return false;
                this.flag = false;
                return true;
            }

            public override bool HitEvent(ObjectBase o)
            {
                if (!base.HitEvent(o))
                    return false;
                this.flag = false;
                return true;
            }
        }

        private class AttackSpawner : AttackBase
        {
            private Func<int, bool> endPredicate;
            private Action<int> updateFunc;

            public AttackSpawner(
                IAudioEngine so,
                SceneBattle p,
                Panel.COLOR u,
                Func<int, bool> endPredicate,
                Action<int> updateFunc)
                : base(so, p, 0, 0, u, 0, ChipBase.ELEMENT.normal)
            {
                this.endPredicate = endPredicate;
                this.updateFunc = updateFunc;
            }

            public override void Updata()
            {
                if (this.endPredicate(this.frame))
                {
                    this.flag = false;
                    return;
                }

                this.updateFunc(this.frame);

                this.FlameControl();
            }
        }
    }
}
