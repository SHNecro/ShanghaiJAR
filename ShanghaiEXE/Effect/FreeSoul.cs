using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using static NSMap.Character.MapCharacterBase;

namespace NSEffect
{
    internal class FreeSoul : EffectBase
    {
        private static readonly Rectangle GhostDissolveTextureRectOrigin = new Rectangle(360, 680, 30, 50);
        private static readonly Rectangle AliveDissolveTextureRectOrigin = new Rectangle(360 + (30 * 6), 680, 30, 50);
        private static readonly Rectangle SpriteDissolveTextureRectOrigin = new Rectangle(1410, 680, 30, 50);
        private static readonly Rectangle SoulTextureRect = new Rectangle(360, 780, 30, 20);
        private static readonly Rectangle SparkleTextureRect = new Rectangle(450, 780, 5, 5);

        private readonly Point characterOffset;
        private readonly ANGLE angle;
        private readonly DissolveCharacter character;

        private readonly Rectangle dissolveTextureRect;
        private readonly bool noBody;

        private IDictionary<Point, int> sparkles = new ConcurrentDictionary<Point, int>();

        private Point soulPosition = new Point(0, -4);

        public FreeSoul(IAudioEngine s, Vector2 pd, Point posi, ANGLE angle, DissolveCharacter character)
          : base(s, null, posi.X, posi.Y)
        {
            this.positionDirect = new Vector2(pd.X, pd.Y);

            this.angle = angle;
            this.character = character;
            if (this.angle == ANGLE.UPRIGHT
                || this.angle == ANGLE.DOWNLEFT)
            {
                this.rebirth = true;
                this.characterOffset = new Point(2, -6);
            }
            else
            {
                this.characterOffset = new Point(-2, -6);
            }

            var dissolveTextureRectOrigin = new Rectangle();
            switch (this.character)
            {
                case DissolveCharacter.Ghost:
                    dissolveTextureRectOrigin = GhostDissolveTextureRectOrigin;
                    break;
                case DissolveCharacter.Alive:
                    dissolveTextureRectOrigin = AliveDissolveTextureRectOrigin;
                    break;
                case DissolveCharacter.Sprite:
                    dissolveTextureRectOrigin = SpriteDissolveTextureRectOrigin;
                    break;
                case DissolveCharacter.NoBody:
                    this.noBody = true;
                    break;
            }

            var dissolveAdjustmentX = 0;
            var dissolveAdjustmentY = 0;

            if (this.angle == ANGLE.UPLEFT
                || this.angle == ANGLE.UPRIGHT)
            {
                dissolveAdjustmentY = dissolveTextureRectOrigin.Height;
            }

            var adjustedRect = new Rectangle(
                dissolveTextureRectOrigin.X + dissolveAdjustmentX,
                dissolveTextureRectOrigin.Y + dissolveAdjustmentY,
                dissolveTextureRectOrigin.Width,
                dissolveTextureRectOrigin.Height);
            this.dissolveTextureRect = adjustedRect;
        }

        public override void Updata()
        {
            if (this.moveflame)
            {
                foreach (var kvp in this.sparkles)
                {
                    this.sparkles[kvp.Key] -= 1;
                }

                var expiredSparkles = this.sparkles.Where(kvp => kvp.Value <= 0).ToArray();

                foreach (var expired in expiredSparkles)
                {
                    this.sparkles.Remove(expired);
                }

                this.soulPosition.Offset(0, -(1 + (int)(this.frame / 5)));

                var randomDist = Random.NextDouble() * 5 + 8;
                var randomAngle = Random.NextDouble() * 2 * Math.PI;
                var newSparklePoint = new Point(this.soulPosition.X + (int)(randomDist * Math.Cos(randomAngle)), this.soulPosition.Y + (int)(randomDist * Math.Sin(randomAngle)));
                this.sparkles[newSparklePoint] = 3;
            }

            this.FlameControl(3);

            if (this.frame >= 30)
            {
                this.flag = false;
            }
        }

        public override void Render(IRenderer dg)
        {
            if (!this.flag)
            {
                return;
            }

            var soulFrame = Math.Abs(((this.frame / 2) % 4) - 2);

            // Soul
            this._rect = new Rectangle(SoulTextureRect.X + (soulFrame * SoulTextureRect.Width), SoulTextureRect.Y, SoulTextureRect.Width, SoulTextureRect.Height);
            this._position = new Vector2(this.positionDirect.X + Shake.X + this.soulPosition.X, this.positionDirect.Y + Shake.Y + this.soulPosition.Y);
            this.color = Color.White;
            dg.DrawImage(dg, "body25", this._rect, false, this._position, this.rebirth, this.color);

            foreach (var sparkle in this.sparkles)
            {
                // Sparkles
                this._rect = new Rectangle(SparkleTextureRect.X + ((3 - sparkle.Value) * SparkleTextureRect.Width), SparkleTextureRect.Y, SparkleTextureRect.Width, SparkleTextureRect.Height);
                this._position = new Vector2(this.positionDirect.X + Shake.X + sparkle.Key.X, this.positionDirect.Y + Shake.Y + sparkle.Key.Y);
                this.color = Color.White;
                dg.DrawImage(dg, "body25", this._rect, false, this._position, this.rebirth, this.color);
            }

            var dissolveFrame = this.frame / 2;
            if (!this.noBody && dissolveFrame < 5)
            {
                // Shadow
                this._rect = new Rectangle(32 * ((dissolveFrame / 2) + 1), 48 * 8, 32, 48);
                this._position = new Vector2(this.positionDirect.X + Shake.X, this.positionDirect.Y + Shake.Y - 1);
                this.color = Color.White;
                dg.DrawImage(dg, "charachip1", this._rect, false, this._position, this.rebirth, this.color);

                // Dissolving
                this._rect = new Rectangle(this.dissolveTextureRect.X + (dissolveFrame * this.dissolveTextureRect.Width), this.dissolveTextureRect.Y, this.dissolveTextureRect.Width, this.dissolveTextureRect.Height);
                this._position = new Vector2(this.positionDirect.X + Shake.X + this.characterOffset.X, this.positionDirect.Y + Shake.Y + this.characterOffset.Y);
                this.color = Color.White;
                dg.DrawImage(dg, "body25", this._rect, false, this._position, this.rebirth, this.color);
            }
        }

        public enum DissolveCharacter
        {
            Ghost = 0,
            Alive,
            NoBody,
            Sprite,
        }
    }
}
