using NSBattle;
using NSBattle.Character;
using NSChip;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEffect;
using NSEnemy;
using NSObject;
using Common.Vectors;
using System.Drawing;
using System;
using System.Linq;

namespace NSAttack
{
    internal class NoiseBreakout : AttackBase
    {
        private const int FrameLifetime = 90;

        private readonly bool blocked;
        private readonly Vector2 initialPosition;
        private readonly Vector2 initialVelocity;
        
        // position -> positonDirect
        private Vector2 velocity;
        private Vector2 acceleration;
        private int framesToImpact;

        private bool isSecondaryAttack;

        public NoiseBreakout(
          IAudioEngine so,
          SceneBattle p,
          int pX,
          int pY,
          Panel.COLOR u,
          int po,
          int s,
          Vector2 startPos,
          float initialYVel,
          bool blocked,
          bool secondary = false)
          : base(so, p, pX, pY, u, po, ChipBase.ELEMENT.normal)
        {
            this.breaking = false;
            this.hitrange = new Point(0, 0);
            this.hitting = false;
            this.speed = s;
            this.initialPosition = startPos;
            this.positionDirect = this.initialPosition;

            var endPos = new Vector2((float)(pX * 40.0 + 21.0), (float)(pY * 24.0 + 78.0));
            var gravityPxPerFrameSq = (float)((32 / 1.0) * (1.0 / 1.0) * (9.8 / 1) * (1.0 / 60) * (1.0 / 60));
            var timeUntilImpact = this.CalculateImpactTime(initialYVel, startPos.Y, endPos.Y, gravityPxPerFrameSq);
            if (timeUntilImpact == null)
            {
                this.flag = false;
            }
            else
            {
                var constantXVel = (endPos.X - this.initialPosition.X) / timeUntilImpact.Value;
                this.initialVelocity = new Vector2((float)constantXVel, initialYVel);
                this.velocity = this.initialVelocity;
                this.acceleration = new Vector2(0, gravityPxPerFrameSq);
                this.framesToImpact = (int)Math.Ceiling(timeUntilImpact.Value);
                this.position = new Point(pX, pY);
                this.isSecondaryAttack = secondary;
            }

            this.rebirth = this.union == Panel.COLOR.red;
            this.blocked = blocked;
        }

        public override void Updata()
        {
            var noiseSpread = this.positionDirect + new Vector2(10 * 2 * (float)(this.Random.NextDouble() - 0.5), 10 * 2 * (float)(this.Random.NextDouble() - 0.5));
            var noiseTrail = new Noise(this.sound, noiseSpread, this.position, true);
            noiseTrail.blackOutObject = false;
            this.parent.effects.Add(noiseTrail);
            this.positionDirect += new Vector2(this.velocity.X, this.velocity.Y);
            this.velocity += this.acceleration;

            if (!this.blocked)
            {
                this.PanelBright();

                this.hitting = this.waittime >= this.framesToImpact - 5;
                if (this.waittime >= this.framesToImpact)
                {
                    if (!this.isSecondaryAttack)
                    {
                        // TODO: add damaging effects
                        foreach (var xOffset in new[] { -1, 1 })
                        {
                            if (this.position.X + xOffset >= 0 && this.position.X + xOffset < this.parent.panel.GetLength(0))
                            {
                                var noiseAttack = new NoiseBreakout(
                                    this.sound,
                                    this.parent,
                                    this.position.X + xOffset,
                                    this.position.Y,
                                    this.union,
                                    this.power,
                                    this.speed,
                                    this.positionDirect,
                                    -this.velocity.Y / 2,
                                    false,
                                    true);
                                this.parent.attacks.Add(noiseAttack);
                            }
                        }
                    }

                    this.sound.PlaySE(SoundEffect.noise);
                    this.flag = false;
                }
            }
            else
            {
                if (this.waittime >= this.framesToImpact / 2)
                {
                    var blockShield = new ReflShield(this.sound, this.positionDirect, this.position);
                    blockShield.blackOutObject = false;
                    this.parent.effects.Add(blockShield);

                    var shieldBlock = new Guard(this.sound, this.parent, noiseSpread, 2, this.position);
                    shieldBlock.blackOutObject = false;
                    this.parent.effects.Add(shieldBlock);
                    this.sound.PlaySE(SoundEffect.damagezero);

                    this.flag = false;
                }
            }
            this.waittime++;
        }

        public override void Render(IRenderer dg)
        {
        }

        public override bool HitCheck(Point charaposition, Panel.COLOR charaunion)
        {
            return base.HitCheck(charaposition, charaunion);
        }

        public override bool HitCheck(Point charaposition)
        {
            return base.HitCheck(charaposition);
        }

        public override bool HitEvent(Player p)
        {
            return base.HitEvent(p);
        }

        public override bool HitEvent(EnemyBase e)
        {
            return base.HitEvent(e);
        }

        public override bool HitEvent(ObjectBase o)
        {
            return base.HitEvent(o);
        }

        private double? CalculateImpactTime(double v0, double x0, double x, double a)
        {
            var root = Math.Sqrt((v0 * v0) - (4 * 0.5 * a * (x0 - x)));
            var roots = new[] { -1, 1 }.Select(r => (-v0 + r * root) / a);
            return roots.LastOrDefault(r => r > 0);
        }
    }
}
