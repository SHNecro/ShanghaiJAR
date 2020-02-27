using NSShanghaiEXE.InputOutput;
using NSShanghaiEXE.InputOutput.Rendering;
using SlimDX;
using System.Drawing;
using System;
using System.Collections.Generic;
using NSMap;
using NSMap.Character;

namespace NSEffect
{
    internal class ShanghaiHeavenWarp : EffectBase
    {
        private const int XOffset = 7;
        private const int Width = 24;

        private static readonly Dictionary<MapCharacterBase.ANGLE, Point> AngleTextures = new Dictionary<MapCharacterBase.ANGLE, Point>
        {
            { MapCharacterBase.ANGLE.DOWN, new Point(0, 48 * 0) },
            { MapCharacterBase.ANGLE.DOWNRIGHT, new Point(0, 48 * 1) },
            { MapCharacterBase.ANGLE.RIGHT, new Point(0, 48 * 2) },
            { MapCharacterBase.ANGLE.UPRIGHT, new Point(0, 48 * 3) },
            { MapCharacterBase.ANGLE.UP, new Point(0, 48 * 4) },
            { MapCharacterBase.ANGLE.UPLEFT, new Point(0, 48 * 5) },
            { MapCharacterBase.ANGLE.LEFT, new Point(0, 48 * 6) },
            { MapCharacterBase.ANGLE.DOWNLEFT, new Point(0, 48 * 7) },
        };

        private MapField field;
        private long ySpeed;
        private long[] yOffsets;

        private bool reversed;

        public ShanghaiHeavenWarp(MyAudio s, Vector2 pd, Point posi, MapField field, bool reversed)
          : base(s, null, posi.X, posi.Y)
        {
            this.positionDirect = new Vector2(pd.X, pd.Y - 20);

            this.field = field;
            this.ySpeed = 16;
            this.yOffsets = new long[32];

            this.reversed = reversed;
            if (this.reversed)
            {
                for (int setupFrame = 0; setupFrame < Width; setupFrame++)
                {
                    for (int i = XOffset; i < Width; i++)
                    {
                        var iAdj = i % 2 == 0 ? (i / 2) : (Width - ((i + 1) / 2));
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
                for (int i = XOffset; i < Width; i++)
                {
                    var iAdj = i % 2 == 0 ? (i / 2) : (Width - ((i + 1) / 2));
                    var yOffsetSpeed = (this.reversed ? -1 : 1) * (this.ySpeed >> iAdj);

                    yOffsets[i] += yOffsetSpeed;
                }
                this.ySpeed = this.reversed ? (this.ySpeed / 2) : (this.ySpeed * 2);
            }

            this.FlameControl(3);

            if (this.frame > Width)
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

            this._rect = new Rectangle(0, 48 * 8, 32, 48);
            this._position = new Vector2(this.positionDirect.X + Shake.X, this.positionDirect.Y + Shake.Y - 18);
            this.color = Color.White;
            dg.DrawImage(dg, "charachip1", this._rect, false, this._position, this.rebirth, this.color);

            var angleTexturePoint = AngleTextures[this.field.parent.Player.Angle];
            for (int i = XOffset; i < Width; i++)
            {
                var xOff = i - 15;
                var yOff = this.yOffsets[i] + 18;

                this._rect = new Rectangle(angleTexturePoint.X + i, angleTexturePoint.Y, 1, 48);
                this._position = new Vector2(this.positionDirect.X + Shake.X + xOff, this.positionDirect.Y + Shake.Y - yOff);
                this.color = Color.White;
                dg.DrawImage(dg, "charachip1", this._rect, false, this._position, this.rebirth, this.color);
            }
        }
    }
}
