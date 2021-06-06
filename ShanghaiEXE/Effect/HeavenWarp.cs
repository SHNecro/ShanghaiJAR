using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;
using System.Collections.Generic;
using NSMap;
using NSMap.Character;

namespace NSEffect
{
    internal class HeavenWarp : EffectBase
    {
        private static readonly Dictionary<MapCharacterBase.ANGLE, int> AngleTextures = new Dictionary<MapCharacterBase.ANGLE, int>
        {
            { MapCharacterBase.ANGLE.DOWN, 48 * 0 },
            { MapCharacterBase.ANGLE.DOWNRIGHT, 48 * 1 },
            { MapCharacterBase.ANGLE.RIGHT, 48 * 2 },
            { MapCharacterBase.ANGLE.UPRIGHT, 48 * 3 },
            { MapCharacterBase.ANGLE.UP, 48 * 4 },
            { MapCharacterBase.ANGLE.UPLEFT, 48 * 5 },
            { MapCharacterBase.ANGLE.LEFT, 48 * 6 },
            { MapCharacterBase.ANGLE.DOWNLEFT, 48 * 7 },
        };

        private string textureFile;
        private Rectangle textureRect;

        private MapField field;
        private long ySpeed;
        private long[] yOffsets;

        private bool xFlipped;
        private bool warpIn;
        private bool hasShadow;

        public HeavenWarp(IAudioEngine s, Vector2 pd, Point posi, MapField field, bool warpIn)
          : this(s, pd, posi, field, warpIn, false, "charachip1", new Rectangle(0, AngleTextures[field.parent.Player.Angle], 32, 48))
        {
        }

        public HeavenWarp(IAudioEngine s, Vector2 pd, Point posi, MapField field, bool warpIn, bool xFlipped, string textureFile, Rectangle textureRect)
          : this(s, pd, posi, field, warpIn, xFlipped, textureFile, textureRect, true)
        {
        }

        public HeavenWarp(IAudioEngine s, Vector2 pd, Point posi, MapField field, bool warpIn, bool xFlipped, string textureFile, Rectangle textureRect, bool hasShadow)
          : base(s, null, posi.X, posi.Y)
        {
            this.positionDirect = new Vector2(pd.X, pd.Y - 20);

            this.textureFile = textureFile;
            this.textureRect = textureRect;

            this.field = field;
            this.ySpeed = 16;
            this.yOffsets = new long[this.textureRect.Width];

            this.xFlipped = xFlipped;
            this.warpIn = warpIn;
            this.hasShadow = hasShadow;
            if (this.warpIn)
            {
                for (int setupFrame = 0; setupFrame < this.textureRect.Width; setupFrame++)
                {
                    for (int i = 0; i < this.textureRect.Width; i++)
                    {
                        var iAdj = i % 2 == 0 ? (i / 2) : (this.textureRect.Width - ((i + 1) / 2));
                        var yOffsetSpeed = this.ySpeed >> iAdj;
                        yOffsets[i] = yOffsets[i] + yOffsetSpeed;
                    }
                    this.ySpeed = this.ySpeed * 2;
                }
                this.ySpeed /= 2;
            }
        }

        public override void Updata()
        {
            if (this.moveflame)
            {
                for (int i = 0; i < this.textureRect.Width; i++)
                {
                    var iAdj = i % 2 == 0 ? (i / 2) : (this.textureRect.Width - ((i + 1) / 2));
                    var yOffsetSpeed = (this.warpIn ? -1 : 1) * (this.ySpeed >> iAdj);

                    yOffsets[i] += yOffsetSpeed;
                }
                this.ySpeed = this.warpIn ? (this.ySpeed / 2) : (this.ySpeed * 2);
            }

            this.FlameControl(3);

            if (this.frame > this.textureRect.Width)
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

            var centerOffset = this.textureRect.Width / 2;
            if (this.hasShadow)
            {
                this._rect = new Rectangle(0, 48 * 8, 32, 48);
                this._position = new Vector2(this.positionDirect.X + Shake.X, this.positionDirect.Y + Shake.Y - 18);
                this.color = Color.White;
                dg.DrawImage(dg, "charachip1", this._rect, false, this._position, this.rebirth, this.color);
            }

            for (int i = 0; i < this.textureRect.Width; i++)
            {
                var x = !this.xFlipped ? i : (this.textureRect.Width - i - 1);

                var xOff = x - centerOffset + 0.5f;
                var yOff = this.yOffsets[i] + 18;

                this._rect = new Rectangle(this.textureRect.X + i, this.textureRect.Y, 1, this.textureRect.Height);
                this._position = new Vector2(this.positionDirect.X + Shake.X + xOff, this.positionDirect.Y + Shake.Y - yOff);
                this.color = Color.White;
                dg.DrawImage(dg, this.textureFile, this._rect, false, this._position, this.rebirth, this.color);
            }
        }
    }
}
