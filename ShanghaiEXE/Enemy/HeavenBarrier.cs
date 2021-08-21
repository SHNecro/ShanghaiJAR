using NSAttack;
using NSBattle;
using NSChip;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;
using Common.Vectors;
using System.Drawing;
using System.Linq;
using System.Collections.Generic;
using NSEffect;
using System.Collections.Concurrent;
using System;
using NSBattle.Character;

namespace NSEnemy
{
    internal class HeavenBarrier : EnemyBase
    {
        private static readonly Rectangle SparkleTextureRect = new Rectangle(450, 780, 5, 5);

        private readonly IDictionary<int, Sparkle> sparkles = new ConcurrentDictionary<int, Sparkle>();

        private HeavenBarrier controller;
        private List<HeavenBarrier> controlledBarriers;
        private Dictionary<ChipBase.ELEMENT, int> damageBuildup;

        private MOTION state;
        private Color overlayColor;

        private int lastDamage;

        public HeavenBarrier(IAudioEngine s, SceneBattle p, int pX, int pY, byte n, Panel.COLOR u, byte v)
          : base(s, p, pX, pY, n, u, v)
        {
            for (int index = 0; index < this.dropchips.Length; ++index)
                this.dropchips[index] = new ChipFolder(this.sound);
            this.name = ShanghaiEXE.Translate("Enemy.HeavenBarrierName1");
            this.picturename = "heavenbarrier";
            this.race = EnemyBase.ENEMY.virus;
            this.element = ChipBase.ELEMENT.normal;
            this.Flying = false;
            this.power = 0;
            this.wide = 32;
            this.height = 80;
            this.version = v;
            this.frame = 0;
            this.speed = 7 - version;
            this.printhp = true;
            this.printNumber = true;
            this.effecting = false;
            if (this.parent != null)
                this.roop = (byte)(parent.manyenemys - (uint)this.number);
            this.PositionDirectSet();

            if (this.version <= 0 || this.version > 3)
            {
                this.version = (byte)((this.version % 3) + 1);
            }

            switch (this.version)
            {
                case 1:
                    this.name = ShanghaiEXE.Translate("Enemy.HeavenBarrierName1");
                    this.hp = 600;
                    break;
                case 2:
                    this.name = ShanghaiEXE.Translate("Enemy.HeavenBarrierName2");
                    this.hp = 800;
                    break;
                case 3:
                    this.name = ShanghaiEXE.Translate("Enemy.HeavenBarrierName3");
                    this.hp = 1000;
                    break;
            }
            this.animationpoint = new Point(this.version, 0);
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

            this.hpmax = this.hp;
            this.hpprint = this.hp;
            this.neutlal = true;
            this.badstatusresist = true;

            this.overlayColor = Color.Transparent;
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
                    if (this.Hp <= 0)
                    {
                        this.state = MOTION.Breaking;
                        this.printhp = false;
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

            if (this.state == MOTION.Absorbing || this.state == MOTION.Breaking)
            {
                var attackDamage = this.lastDamage;
                if (attackDamage == -1)
                {
                    int num = 1;
                    if (this.standbarrier)
                        num *= 2;
                    if (this.guard == CharacterBase.GUARD.armar && attack.breaking)
                        num *= 2;
                    attackDamage = attack.DamageMath(this) / num;
                }

                // TODO: Handle damage cancelling, effects
                controller.damageBuildup[attack.Element] += attackDamage;
                this.lastDamage = -1;
            }
        }

        public override void InitAfter()
        {
            base.InitAfter();

            if (this.controller == null)
            {
                var barriers = this.Parent.AllChara().Where(c => c.union == this.union).OfType<HeavenBarrier>();
                this.controller = barriers.Select(b => b.controller).FirstOrDefault(c => c != null) ?? barriers.FirstOrDefault() ?? this;
                this.controlledBarriers = barriers.ToList();

                this.controlledBarriers.ForEach(c => c.controller = this.controller);

                this.damageBuildup = new Dictionary<ChipBase.ELEMENT, int>
                {
                    { ChipBase.ELEMENT.normal, 0 },
                    { ChipBase.ELEMENT.heat, 0 },
                    { ChipBase.ELEMENT.aqua, 0 },
                    { ChipBase.ELEMENT.eleki, 0 },
                    { ChipBase.ELEMENT.leaf, 0 },
                    { ChipBase.ELEMENT.poison, 0 },
                    { ChipBase.ELEMENT.earth, 0 }
                };
            }
        }

        public override void PositionDirectSet()
        {
            this.positionDirect = new Vector2((float)(position.X * 40.0 + 20.0) + 4 * this.UnionRebirth + 2, (float)(position.Y * 24.0 + 54.0) - 4);
        }

        protected override void Moving()
        {
            this.neutlal = true;

            switch (this.state)
            {
                case MOTION.Absorbing:
                    this.animationpoint = new Point(this.version, 0);
                    break;
                case MOTION.Breaking:
                    var fakeDeathPosition = new Vector2((int)this.positionDirect.X + this.Shake.X, (int)this.positionDirect.Y + this.Shake.Y);
                    var fakeDeathRect = new Rectangle(this.animationpoint.X * this.wide, this.animationpoint.Y * this.height, this.wide, this.height);
                    
                    this.parent.effects.Add(new EnemyDeath(this.sound, this.parent, fakeDeathRect, new Rectangle(this.animationpoint.X * this.wide, 0, this.wide, this.height), fakeDeathPosition, this.picturename, this.rebirth, this.position));

                    this.animationpoint = new Point(this.Random.Next(1, 9 + 1), 1);
                    this.waittime = 0;
                    this.state = MOTION.Broken;
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
                            //this.sound.PlaySE(SoundEffect.charge);
                            //this.sound.BGMFadeStart(300, 25);
                        }
                    }
                    break;
                case MOTION.RetaliatingChargeUp:
                    this.animationpoint = new Point(1, 1);
                    if (this.waittime < 660)
                    {
                        //if (this.controller == this)
                        //{
                        //    if (this.waittime == 300)
                        //    {
                        //        this.sound.BGMFadeStart(50, 0);
                        //    }
                        //    if (this.waittime == 350)
                        //    {
                        //        this.sound.SetBGM("heavenbackground");
                        //        this.sound.BGMFadeStart(20, 100);
                        //    }
                        //}
                        if (this.waittime < 400)
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
                            var whiteoutProgress = (int)Math.Min(255, (this.waittime - 400) / 150.0 * 255);
                            this.overlayColor = Color.FromArgb(255 - whiteoutProgress, Color.White);
                            this.alfha = (byte)(255 - whiteoutProgress);
                        }

                        if (this.controller == this)
                        {
                            if (this.waittime > 100 && this.waittime < 300)
                            {
                                if (this.waittime % 30 == 0)
                                {
                                    this.sound.PlaySE(SoundEffect.pikin);
                                }

                                if (this.waittime == 200)
                                {
                                    this.parent.attacks.Add(new Dummy(this.sound, this.parent, 1, 1, this.union, new Point(0, 0), 100, true));
                                }
                            }
                            else if (this.waittime == 300)
                            {
                                this.parent.attacks.Add(new Dummy(this.sound, this.parent, 1, 1, this.union, new Point(0, 0), 600, false));
                                this.parent.attacks.Add(new Dummy(this.sound, this.parent, 2, 0, this.union, new Point(3, 3), 150, true));
                                this.sound.PlaySE(SoundEffect.wave);
                            }
                            else if (this.waittime == 450)
                            {
                                this.parent.attacks.Add(new Dummy(this.sound, this.parent, 2, 0, this.union, new Point(3, 3), 600, false));
                            }
                            else if (this.waittime > 450)
                            {
                                this.RetaliateTick();
                                this.sound.PlaySE(SoundEffect.beamlong);
                            }

                            var numberOfSparkles = 8 * Math.Pow(this.waittime / 660.0, 2);
                            for (var i = 1; i < numberOfSparkles; i++)
                            {
                                // Pillar starts offscreen, goes to 60, 105 (middle of area)
                                var initialPosition = new Point(60, -8 + this.Random.Next(1, 4));
                                var lifespan = 720;
                                var t = 0;
                                var randomPhaseShift = this.Random.NextDouble() * 2 * Math.PI;
                                
                                var spinSpeedFactor = 1 + 4 * ((double)this.waittime / lifespan);
                                var rotations = 5;

                                var downwardsVelocity = 2 * (this.waittime / 660.0);
                                var radiusExpansion = (this.Random.NextDouble() + 0.5) * (this.waittime / 660.0);
                                var movement = new Action<Sparkle>(s =>
                                {
                                    t++;
                                    var radian = randomPhaseShift + (spinSpeedFactor * (t * rotations / (360 / (2 * Math.PI))));

                                    var radius = 5 + ((double)t / lifespan) * 50 * 0.5 * (1 - Math.Cos(Math.Min(Math.PI, radiusExpansion * (t / (360 / (2 * Math.PI))))));
                                    var xSpin = radius * Math.Cos(radian);
                                    var ySpin = 0.5 * radius * Math.Sin(radian);

                                    var yOffset = 115 * 0.5 * (1 - Math.Cos(Math.Min(Math.PI, downwardsVelocity * (t / (360 / (2 * Math.PI))))));

                                    var x = 0.0 + xSpin;
                                    var y = yOffset + ySpin;
                                    s.Position = new Point(initialPosition.X + (int)Math.Round(x), initialPosition.Y + (int)Math.Round(y));
                                });
                                var newSparkle = new Sparkle { Lifespan = lifespan, RemainingLife = lifespan, Movement = movement };
                                movement.Invoke(newSparkle);
                                this.sparkles[this.sparkles.FirstOrDefault(kvp => !this.sparkles.ContainsKey(kvp.Key + 1)).Key + 1] = newSparkle;
                            }
                        }
                    }
                    else if (this.waittime >= 660)
                    {
                        this.state = MOTION.Retaliating;
                    }
                    break;
                case MOTION.Retaliating:
                    this.animationpoint = new Point(1, 1);

                    if (this.controller == this)
                    {
                        if (this.waittime < 1000)
                        {
                            this.RetaliateTick();
                        }
                        else if (this.waittime >= 1000)
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
                if (this.controlledBarriers.Any(c => c.state == MOTION.Absorbing))
                {
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
                        var breakthroughChance = 1.0 - (double)this.controlledBarriers.Count(c => c.state == MOTION.Absorbing) / (this.controlledBarriers.Count + 1);
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

                this._rect.Offset(-this._rect.X, 0);
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

            this.HPposition = new Vector2(this.positionDirect.X + 4f, this.positionDirect.Y - 32f);
            this.Nameprint(dg, this.printNumber);
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

        private void RetaliateTick()
        {
            var enemies = this.parent.AllChara().Where(c => c.union == this.UnionEnemy);
            foreach (var enemy in enemies)
            {
                // TODO:
                enemy.Hp -= 1;
            }
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

