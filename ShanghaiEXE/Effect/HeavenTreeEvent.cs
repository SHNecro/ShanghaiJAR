using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace NSEffect
{
    internal class HeavenTreeEvent : EffectBase
    {
        private const int PetalGlowLength = 45;
        private const int PetalSparkleLength = 45;
        private const int PeachAppearingLength = 60;
        private const int PeachCreatedLength = 45;
        private const int PeachFloatDownLength = 180;
        private const int PeachDisappearLength = 90;

        // xy pos, width/height, type of petal
        private static readonly Rectangle[] Flowers = AdjustToMapPos(new[]
        {
            new Rectangle(147, -6, 10, 0),
            new Rectangle(138, -35, 10, 0),
            new Rectangle(123, -16, 10, 3),
            new Rectangle(139, -6, 10, 7),
            new Rectangle(156, -20, 10, 8),
            new Rectangle(147, 0, 10, 9),
            new Rectangle(161, -14, 10, 11),
            new Rectangle(132, -16, 12, 0),
            new Rectangle(171, 3, 12, 2),
            new Rectangle(170, -24, 12, 2),
            new Rectangle(125, -2, 12, 0),
            new Rectangle(136, -1, 10, 11),
            new Rectangle(153, -34, 10, 0),
            new Rectangle(159, -33, 10, 4),
            new Rectangle(160, -27, 12, 0),
            new Rectangle(178, -7, 12, 3),
            new Rectangle(171, -10, 10, 12),
            new Rectangle(176, -13, 12, 1),
            new Rectangle(161, 2, 10, 11),
            new Rectangle(160, -4, 10, 3),
            new Rectangle(161, -8, 10, 8),
            new Rectangle(143, -25, 10, 2),
            new Rectangle(147, -15, 10, 0),
            new Rectangle(137, -21, 10, 9),
            new Rectangle(160, 10, 10, 10),
            new Rectangle(164, 18, 12, 0),
            new Rectangle(157, 14, 12, 2),
            new Rectangle(139, 7, 10, 7),
            new Rectangle(141, 13, 10, 4),
            new Rectangle(135, 10, 10, 3),
        });
        private static Point MapOffset = new Point(50, -7);
        private static readonly Rectangle SparkleTextureRect = new Rectangle(450, 780, 5, 5);

        private Color? background;
        private Color? foreground;

        private double petalOpacity;

        private IList<Sparkle> sparkles = new List<Sparkle>();
        private Point? peachPosition;
        private double peachOpacity;
        private bool peachSprite;

        private bool sparklesOverPeach;

        private EventState state = EventState.PetalGlow;

        public HeavenTreeEvent(IAudioEngine s, Vector2 pd, Point posi)
          : base(s, null, posi.X, posi.Y)
        {
            this.positionDirect = new Vector2(pd.X, pd.Y - 20);
        }

        public override void Updata()
        {
            if (this.moveflame)
            {
                this.UpdateSparkles();

                switch (this.state)
                {
                    case EventState.PetalGlow:
                        if (this.frame > PetalGlowLength)
                        {
                            this.frame = 0;
                            this.state = EventState.PetalSparkle;
                        }
                        else
                        {
                            this.petalOpacity = (double)this.frame / PetalGlowLength;
                        }
                        break;
                    case EventState.PetalSparkle:
                        if (this.frame > PetalSparkleLength + PeachAppearingLength + 60)
                        {
                            this.background = null;
                            this.frame = 0;
                            this.state = EventState.PeachCreated;
                        }
                        else
                        {
                            var sparkleFreq = Math.Max(4, 8 - (int)Math.Round(this.frame / 45.0 * 4));
                            var gatherFrame = Math.Min(PeachAppearingLength, this.frame - PetalSparkleLength);
                            if (this.frame % sparkleFreq == 0)
                            {
                                foreach (var flower in Flowers)
                                {
                                    var screenPos = this.FlowerToScreenPos(flower.Location);

                                    var randomDist = Random.NextDouble() * flower.Width / 2;
                                    var randomAngle = Random.NextDouble() * 2 * Math.PI;
                                    var sparkleX = screenPos.X + (int)(randomDist * Math.Cos(randomAngle));
                                    var sparkleY = screenPos.Y + (int)(randomDist * Math.Sin(randomAngle));

                                    var currentFrame = this.frame;

                                    this.AddSparkle(
                                        3 * Random.Next(4, 13),
                                        new Point(sparkleX, sparkleY),
                                        s =>
                                        {
                                            var floatUpVel = (3 - s.Frame) / 3.0f;
                                            var velocity = new Vector2(0, floatUpVel);

                                            if (currentFrame >= PetalSparkleLength)
                                            {
                                                var gatherPos = new Vector2(0, -15);
                                                var drawInXDist = gatherPos.X - s.Position.X;
                                                var drawInYDist = gatherPos.Y - s.Position.Y;
                                                var drawStrength = gatherFrame / (float)PeachAppearingLength;
                                                velocity += new Vector2(drawStrength * (drawInXDist / 10.0f), -drawStrength * (drawInYDist / 10.0f));
                                            }

                                            s.Position = new Point(s.Position.X + (int)Math.Round(velocity.X), s.Position.Y - (int)Math.Round(velocity.Y));
                                        });
                                }
                            }
                            if (this.frame >= PetalSparkleLength)
                            {
                                this.peachPosition = new Point(0, -25);
                                this.peachOpacity = (double)gatherFrame / PeachAppearingLength;
                                this.petalOpacity = Math.Max(0.2, 1 - this.peachOpacity);
                                this.background = Color.FromArgb((int)Math.Min(255, this.peachOpacity * 96), Color.Black);
                            }
                        }
                        break;
                    case EventState.PeachCreated:
                        if (this.frame > PeachCreatedLength)
                        {
                            this.frame = 0;
                            this.state = EventState.PeachFloatDown;
                        }
                        else
                        {
                            if (this.frame < 8)
                            {
                                this.foreground = Color.FromArgb((int)Math.Min(255, (this.frame + 1) / 8.0 * 255), Color.White);
                            }
                            else if (this.frame < 24)
                            {
                                this.sparklesOverPeach = true;
                                this.peachSprite = true;
                                this.background = null;
                                this.foreground = Color.FromArgb((int)Math.Min(255, (24 - (this.frame + 1)) / 24.0 * 255), Color.White);
                            }
                            else if (this.frame % 5 == 0)
                            {
                                var randomDist = Random.NextDouble() * 20 / 2;
                                var randomAngle = Random.NextDouble() * 2 * Math.PI;
                                var sparkleX = this.peachPosition.Value.X + (int)(randomDist * Math.Cos(randomAngle));
                                var sparkleY = this.peachPosition.Value.Y + (int)(randomDist * Math.Sin(randomAngle));

                                var currentFrame = this.frame;

                                this.AddSparkle(
                                    3 * Random.Next(4, 13),
                                    new Point(sparkleX, sparkleY),
                                    s =>
                                    {
                                        var floatUpVel = (3 - s.Frame) / 3.0f;
                                        var velocity = new Vector2(0, floatUpVel);

                                        s.Position = new Point(s.Position.X + (int)Math.Round(velocity.X), s.Position.Y - (int)Math.Round(velocity.Y));
                                    });
                            }
                        }
                        break;
                    case EventState.PeachFloatDown:
                        if (this.frame > PeachFloatDownLength)
                        {
                            this.frame = 0;
                            this.state = EventState.PeachDisappear;
                        }
                        else
                        {
                            var t = (double)this.frame / PeachFloatDownLength;
                            var floatDownProgress = t - (Math.Sin(2 * Math.PI * t) / (2 * Math.PI));
                            this.peachPosition = new Point(0, (int)(-25 + 90 * floatDownProgress));

                            if (this.frame % 3 == 0)
                            {
                                var randomDist = Random.NextDouble() * 20 / 2;
                                var randomAngle = Random.NextDouble() * 2 * Math.PI;
                                var sparkleX = this.peachPosition.Value.X + (int)(randomDist * Math.Cos(randomAngle));
                                var sparkleY = this.peachPosition.Value.Y + (int)(randomDist * Math.Sin(randomAngle));

                                var currentFrame = this.frame;

                                this.AddSparkle(
                                    Random.Next(4, 13),
                                    new Point(sparkleX, sparkleY),
                                    s =>
                                    {
                                        var floatUpVel = (3 - s.Frame);
                                        var velocity = new Vector2(0, floatUpVel);

                                        s.Position = new Point(s.Position.X + (int)Math.Round(velocity.X), s.Position.Y - (int)Math.Round(velocity.Y));
                                    }
                                );
                            }
                        }
                        break;
                    case EventState.PeachDisappear:
                        if (this.frame > PeachDisappearLength)
                        {
                            this.flag = false;
                        }
                        else
                        {
                            if (this.frame % 3 == 0)
                            {
                                var randomDist = Random.NextDouble() * 20 / 2;
                                var randomAngle = Random.NextDouble() * 2 * Math.PI;
                                var sparkleX = this.peachPosition.Value.X + (int)(randomDist * Math.Cos(randomAngle));
                                var sparkleY = this.peachPosition.Value.Y + (int)(randomDist * Math.Sin(randomAngle));

                                var currentFrame = this.frame;

                                this.AddSparkle(
                                    Random.Next(4, 13),
                                    new Point(sparkleX, sparkleY),
                                    s =>
                                    {
                                        var floatUpVel = (3 - s.Frame);
                                        var velocity = new Vector2(0, floatUpVel);

                                        s.Position = new Point(s.Position.X + (int)Math.Round(velocity.X), s.Position.Y - (int)Math.Round(velocity.Y));
                                    }
                                );
                            }
                        }
                        break;
                }
            }

            this.FlameControl(1);
        }

        public override void SkipUpdate()
        {
            this.flag = false;
        }

        public override void Render(IRenderer dg)
        {
            if (!this.flag)
            {
                return;
            }

            if (this.background != null)
            {
                this._rect = new Rectangle(0, 0, 240, 160);
                this._position = new Vector2(120, 80);
                this.color = this.background.Value;
                dg.DrawImage(dg, "fadescreen", this._rect, false, this._position, this.rebirth, this.color);
            }

            this.DrawGlowingPetals(dg);

            if (!this.sparklesOverPeach)
            {
                this.DrawSparkles(dg);
            }

            if (this.peachPosition != null)
            {
                this._rect = new Rectangle(this.peachSprite ? 130 : 110, 330, 20, 20);
                this._position = new Vector2(
                    this.positionDirect.X + this.peachPosition.Value.X + Shake.X,
                    this.positionDirect.Y + this.peachPosition.Value.Y + Shake.Y);
                this.color = Color.FromArgb((int)Math.Min(255, this.peachOpacity * 255), Color.White);
                dg.DrawImage(dg, "body26", this._rect, false, this._position, this.rebirth, this.color);
            }

            if (this.sparklesOverPeach)
            {
                this.DrawSparkles(dg);
            }

            if (this.foreground != null)
            {
                this._rect = new Rectangle(0, 0, 240, 160);
                this._position = new Vector2(120, 80);
                this.color = this.foreground.Value;
                dg.DrawImage(dg, "fadescreen", this._rect, false, this._position, this.rebirth, this.color);
            }
        }

        private void DrawGlowingPetals(IRenderer dg)
        {
            this.color = Color.FromArgb((int)Math.Min(255, this.petalOpacity * 255), Color.White);

            foreach (var flower in Flowers)
            {
                var graphicYOffset = flower.Width == 10 ? 330 : 460;
                var graphicFlowerYOffset = flower.Width * flower.Height;
                this._rect = new Rectangle(0, graphicYOffset + graphicFlowerYOffset, flower.Width, flower.Width);

                var screenPos = this.FlowerToScreenPos(flower.Location);
                this._position = new Vector2(this.positionDirect.X + screenPos.X + Shake.X, this.positionDirect.Y + screenPos.Y + Shake.Y);
                dg.DrawImage(dg, "body26", this._rect, false, this._position, this.rebirth, this.color);
            }
        }

        private void DrawSparkles(IRenderer dg)
        {
            var sparklesSnapshot = this.sparkles.ToArray();
            foreach (var sparkle in sparklesSnapshot)
            {
                if (sparkle.RemainingLife <= 0) continue;

                // Sparkles
                this._rect = new Rectangle(SparkleTextureRect.X + (sparkle.Frame * SparkleTextureRect.Width), SparkleTextureRect.Y, SparkleTextureRect.Width, SparkleTextureRect.Height);
                this._position = new Vector2(this.positionDirect.X + Shake.X + sparkle.Position.X, this.positionDirect.Y + Shake.Y + sparkle.Position.Y);
                this.color = Color.White;
                dg.DrawImage(dg, "body25", this._rect, false, this._position, this.rebirth, this.color);
            }
        }

        private void AddSparkle(int lifespan, Point pos, Action<Sparkle> movement = null)
        {
            this.sparkles.Add(new Sparkle { Lifespan = lifespan, RemainingLife = lifespan, Position = pos, Movement = movement });
        }

        private void UpdateSparkles()
        {
            foreach (var sparkle in this.sparkles)
            {
                sparkle.RemainingLife--;
            }

            var expiredSparkles = this.sparkles.Where(s => s.RemainingLife <= 0).ToArray();
            foreach (var sparkle in expiredSparkles)
            {
                this.sparkles.Remove(sparkle);
            }

            foreach (var sparkle in this.sparkles)
            {
                sparkle.Movement?.Invoke(sparkle);
            }
        }

        private Point FlowerToScreenPos(Point flowerPos)
        {
            var screenX = 2 * (flowerPos.X - flowerPos.Y);
            var screenY = flowerPos.X + flowerPos.Y;

            return new Point(screenX + MapOffset.X, screenY + MapOffset.Y);
        }

        private enum EventState
        {
            PetalGlow,
            PetalSparkle,
            PeachCreated,
            PeachFloatDown,
            PeachDisappear
        }

        private class Sparkle
        {
            public int Lifespan { get; set; }
            public int RemainingLife { get; set; }

            public int Frame => 3 - (int)Math.Round(3.0 * this.RemainingLife / this.Lifespan);

            public Point Position { get; set; }
            public Action<Sparkle> Movement { get; set; }
        }

        private static Rectangle[] AdjustToMapPos(Rectangle[] mapFlowerPositions)
        {
            var topLeft = mapFlowerPositions.First().Location;
            foreach (var pos in mapFlowerPositions)
            {
                if (topLeft.X < pos.X || (topLeft.X == pos.X && topLeft.Y < pos.Y))
                {
                    topLeft = pos.Location;
                }
            }

            return mapFlowerPositions.Select(r => new Rectangle(r.X - topLeft.X, r.Y - topLeft.Y, r.Width, r.Height)).ToArray();
        }
    }
}
