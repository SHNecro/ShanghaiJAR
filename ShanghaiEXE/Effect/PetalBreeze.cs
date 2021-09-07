using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;
using System;
using System.Collections.Generic;

namespace NSEffect
{
    internal class PetalBreeze : EffectBase
    {
        private static readonly Rectangle Petal1Sprite = new Rectangle(0, 600, 22, 23);
        private static readonly int Petal1Delay = 5;
        private static readonly int Petal1Frames = 13;
        private static readonly PetalInfo Petal1 = new PetalInfo(Petal1Sprite, Petal1Delay, Petal1Frames);

        private static readonly Rectangle Petal2Sprite = new Rectangle(0, 623, 12, 10);
        private static readonly int Petal2Delay = 5;
        private static readonly int Petal2Frames = 8;
        private static readonly PetalInfo Petal2 = new PetalInfo(Petal2Sprite, Petal2Delay, Petal2Frames);

        private static readonly double InitialBreezeStrength = 9;
        private static readonly double BreezeDecay = 0.99;

        private readonly List<Petal> petals = new List<Petal>();
        private double basePetalsToAdd;
        private double breezeStrength;

        public PetalBreeze(IAudioEngine s, Vector2 pd, Point posi)
          : base(s, null, posi.X, posi.Y)
        {
            this.positionDirect = pd;
            this.animationpoint.X = 0;
        }

        public override void Updata()
        {
            var timeToCrossScreen = 240 / InitialBreezeStrength;
            if (this.frame < timeToCrossScreen)
            {
                breezeStrength = InitialBreezeStrength * (this.frame / timeToCrossScreen);
            }
            else
            {
                breezeStrength *= BreezeDecay;
                if (petals.Count == 0)
                {
                    this.flag = false;
                }
            }

            var breezeX = breezeStrength;
            var gravityY = 2;

            var isPetalAddFrame = false;

            basePetalsToAdd = (breezeX > basePetalsToAdd) ? breezeX : (breezeX * 0.01 + basePetalsToAdd * 0.99);

            var petalsToAdd = Random.Next((isPetalAddFrame ? 1 : 0), (int)Math.Round(basePetalsToAdd / 2));

            for (int i = 0; i < petalsToAdd; i++)
            {
                isPetalAddFrame = false;
                var petalType = Random.Next(2) == 0 ? 1 : 2;
                var petalInfo = petalType == 1 ? Petal1 : Petal2;
                var petalInitialFrame = Random.Next(0, petalInfo.Frames);

                var newPos = Random.Next(-160, (int)(240 / Math.Max(1, basePetalsToAdd)));
                var xPos = newPos < 0 ? 0 : (newPos * basePetalsToAdd);
                var yPos = newPos < 0 ? -newPos : 0;

                var petalPos = new PointF((float)(xPos - petalInfo.Sprite.Width), (float)(yPos - petalInfo.Sprite.Height));
                var petalScale = (float)(Random.NextDouble() * (1.4 - 0.6) + 0.6);

               this.petals.Add(new Petal(petalPos, petalInitialFrame, petalType, petalScale));
            }

            foreach (var petal in this.petals)
            {
                var xOff = Random.NextDouble() - 0.5;
                var yOff = Random.NextDouble() - 0.5;
                petal.Position = new PointF((float)(petal.Position.X + xOff + breezeX), (float)(petal.Position.Y + yOff + gravityY));
            }

            this.petals.RemoveAll(p => p.Position.X > 240 || p.Position.Y > 160);
            this.FlameControl(1);
        }

        public override void Render(IRenderer dg)
        {
            this.color = Color.White;

            foreach (var petal in this.petals)
            {
                var petalInfo = petal.Type == 1 ? Petal1 : Petal2;
                var petalFrame = (petal.InitialFrame + (this.frame / petalInfo.Delay)) % petalInfo.Frames;

                this._rect = new Rectangle(petalInfo.Sprite.X + (petalInfo.Sprite.Width * petalFrame), petalInfo.Sprite.Y, petalInfo.Sprite.Width, petalInfo.Sprite.Height);
                this._position = new Vector2(petal.Position.X + Shake.X, petal.Position.Y + Shake.Y);
                dg.DrawImage(dg, "body25", this._rect, false, this._position, petal.Scale, 0, this.color);
            }
        }

        private class PetalInfo
        {
            public PetalInfo(Rectangle sprite, int delay, int frames)
            {
                this.Sprite = sprite;
                this.Delay = delay;
                this.Frames = frames;
            }

            public Rectangle Sprite { get; }
            public int Delay { get; }
            public int Frames { get; }
        }

        private class Petal
        {
            public Petal(PointF position, int frame, int type, float scale)
            {
                this.Position = position;
                this.InitialFrame = frame;
                this.Type = type;
                this.Scale = scale;
            }

            public PointF Position { get; set; }
            public int InitialFrame { get; }
            public int Type { get; }
            public float Scale { get; }
        }
    }
}
