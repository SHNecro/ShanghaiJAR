﻿using NSAttack;
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

        private int crossDiveWarningFrames;
        private int crossDiveInitialDelayFrames;
        private int crossDiveDiagonalPassFrames;
        private int crossDiveCircleBackDelayFrames;
        private int crossDiveEndFlightFrames;
        private bool crossDiveDirectionBottomUp;
        private int crossDiveCenterX;
        private bool crossDiveReverse;

        private int diveWeight;
        private int crossDiveWeight;
        private int iceCrashWeight;
        private int spinWeight;
        private int powerUpWeight;

        private AttackState attackMotion;
        private AttackType attackType;

        private int attackWaitTime;

        private bool detachedShadow;
        private Vector2 detachedShadowOffset;

        public CirnoBX(IAudioEngine s, SceneBattle p, int pX, int pY, byte n, Panel.COLOR u, byte v)
            : base(s, p, pX, pY, n, u, v)
        {
            for (int index = 0; index < this.dropchips.Length; ++index)
                this.dropchips[index] = new ChipFolder(this.sound);
            
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
                    break;
                case MOTION.knockback:
                    this.counterTiming = false;
                    this.nohit = false;
                    this.effecting = false;
                    this.printhp = true;
                    this.rend = true;
                    this.overMove = false;
                    this.detachedShadow = false;
                    this.superArmor = false;
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
                            var bins = new[]
                            {
                                    Tuple.Create(AttackType.Dive, this.diveWeight),
                                    Tuple.Create(AttackType.CrossDive, this.crossDiveWeight),
                                    Tuple.Create(AttackType.IceCrash, this.iceCrashWeight),
                                    Tuple.Create(AttackType.Spin, this.spinWeight),
                                    Tuple.Create(AttackType.PowerUp, this.powerUpWeight)
                            };
                            var draw = this.Random.Next(bins.Sum(b => b.Item2));
                            this.attackType = bins.Select((b, i) => Tuple.Create(b.Item1, bins.Take(i + 1).Sum(bb => bb.Item2))).FirstOrDefault(b => b.Item2 > draw)?.Item1 ?? AttackType.Dive;
                            this.attackWaitTime = 0;
                            break;
                        case AttackState.Attack:
                            switch (this.attackType)
                            {
                                case AttackType.Dive:
                                    // counter during 1st frame liftoff
                                    // maybe: unhittable (? lightning, pillars?) while in-air
                                    // reserve position once moving, use real position so swords can intercept
                                    // shadow handling underneath (edit for non-shadow) positionDirect Y++ etc.
                                    // extra effects? crack hit panel, damage in-transit panel (push to target?), hit next panel w/ dust/panel?
                                    // movement effect on return
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

                                        // Begin feather rain 
                                        this.parent.attacks.Add(new DiveFeatherSpawner(this.sound, this.parent, this.union, this.power / 4, this.diveFeatherHitTime, this.diveFeatherDelay, this.diveFeatherSets, this.diveFeatherCount, this.element));
                                    }
                                    else if (this.attackWaitTime == 23)
                                    {
                                        // main sprite dips down 1px
                                        this.positionDirect.Y += 1;
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
										}
										else if (this.attackWaitTime >= 38 + diveToTargetFrames + this.diveRestFrames)
										{
											this.parent.effects.Add(new MoveEnemy(this.sound, this.parent, this.position.X, this.position.Y));

											this.position = this.positionReserved.Value;
											this.positionReserved = null;
											this.detachedShadow = false;
											this.effecting = false;
											this.HitFlagReset();

											this.PositionDirectSet();
											this.AttackMotion = AttackState.Cooldown;

											this.AttackCooldownSet();
										}
                                    }
                                    break;
                                case AttackType.CrossDive:
                                    // movement effect on takeoff
                                    // reserve position
                                    // ? ice powerup? no visual effects

                                    // TODO: detach shadow
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
                                            ;
                                        }
                                        else if (this.attackWaitTime == this.crossDiveInitialDelayFrames)
                                        {
                                            var target = this.RandomTarget();
                                            this.crossDiveCenterX = target.X < 3 ? 1 : target.X;
                                            
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
                                                : Math.Atan(24.0 / 40.0)) );
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

                                            if (this.attackWaitTime > this.crossDiveInitialDelayFrames + this.crossDiveWarningFrames + this.crossDiveDiagonalPassFrames)
                                            {
                                                this.effecting = false;
                                            }
                                        }
                                        else
                                        {
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
                                                var xOffForY = (1 - y) * (tiltLeft  ? -1 : 1 );
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
                                        }
                                        else
                                        {
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
                                            break;
                                        case 60:
                                            this.AttackMotion = AttackState.Cooldown;
                                            this.AttackCooldownSet();
                                            break;
                                    }
                                    break;
                                case AttackType.Spin:
                                    switch (this.attackWaitTime)
                                    {
                                        case 0:
                                            this.animationpoint = new Point(0, 5);
                                            break;
                                        case 60:
                                            this.AttackMotion = AttackState.Cooldown;
                                            this.AttackCooldownSet();
                                            break;
                                    }
                                    break;
                                case AttackType.PowerUp:
                                    switch (this.attackWaitTime)
                                    {
                                        case 1:
                                            this.animationpoint = new Point(0, 1);
                                            break;
                                        case 60:
                                            this.AttackMotion = AttackState.Cooldown;
                                            this.AttackCooldownSet();
                                            break;
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
            var hitmarkedAnimationPoint = this.whitetime == 0 ? this.animationpoint : this.animationpoint.WithOffset(6, 0);
            dg.DrawImage(dg, this.picturename, new Rectangle(FrameCoordX(hitmarkedAnimationPoint.X), FrameCoordY(hitmarkedAnimationPoint.Y), FullFrameRect.Width, FullFrameRect.Height), false, spriteOffsetPosition, 1f, 0f, reversed, this.color);
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
            this.hp = 200;
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
            this.diveFeatherDelay = 45;
            this.diveFeatherCount = 3;
            this.diveFeatherSets = 2;

            this.crossDiveWarningFrames = 20;
            this.crossDiveInitialDelayFrames = 45;
            this.crossDiveDiagonalPassFrames = 15;
            this.crossDiveCircleBackDelayFrames = 5;
            this.crossDiveEndFlightFrames = 30;

            this.diveWeight = 0;
            this.crossDiveWeight = 1;
            this.iceCrashWeight = 0;
            this.spinWeight = 0;
            this.powerUpWeight = 0;
        }

        private void SetVersionStats()
        {
            this.SetDefaultVersionStats();

            if  (this.version == 1)
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
                //this.iceCrashWeight = (this.Hp > this.HpMax / 2) ? 1 : 3;
                //this.spinWeight = (this.Hp > this.HpMax / 2) ? 0 : 3;
            }
            else
            {
                //this.iceCrashWeight = (this.Hp > this.HpMax / 2) ? 1 : 3;
                //this.spinWeight = (this.Hp > this.HpMax / 2) ? 0 : 3;
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
            PowerUp
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
            private static readonly Rectangle FeatherFullFrameRect = new Rectangle(515, 711, 22, 10);
            private static readonly Rectangle FeatherShadowRect = new Rectangle(524, 734, 9, 4);
            private static readonly Vector2 PanelsOffset = new Vector2(20, 80);

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
	}
}
