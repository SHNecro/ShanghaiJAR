using NSShanghaiEXE.InputOutput;
using NSShanghaiEXE.InputOutput.Rendering;
using SlimDX;
using System.Drawing;
using System;
using System.Collections.Generic;

namespace NSEffect
{
    internal class ShanghaiHeavenWarp : EffectBase
    {
        private const int XOffset = 7;
        private const int Width = 24;
        private int ySpeed;
        private int[] yOffsets;

        public ShanghaiHeavenWarp(MyAudio s, Vector2 pd, Point posi)
          : base(s, null, posi.X, posi.Y)
        {
            this.positionDirect = new Vector2(pd.X, pd.Y - 20);

            this.ySpeed = 16;
            this.yOffsets = new int[32];
        }

        public override void Updata()
        {
            if (this.moveflame)
            {
                for (int i = XOffset; i < Width; i++)
                {
                    var iAdj = i % 2 == 0 ? (i / 2) : (Width - ((i + 1) / 2));
                    var yOffsetSpeed = this.ySpeed >> iAdj;
                    yOffsets[i] += yOffsetSpeed;
                }
                this.ySpeed *= 2;
            }

            this.FlameControl(3);

            if (this.frame > 60)
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

            for (int i = XOffset; i < Width; i++)
            {
                var xOff = i - 16;
                var yOff = this.yOffsets[i] + 16;

                this._rect = new Rectangle(i, 240, 1, 48);
                this._position = new Vector2(this.positionDirect.X + Shake.X + xOff, this.positionDirect.Y + Shake.Y - yOff);
                this.color = Color.White;
                dg.DrawImage(dg, "charachip1", this._rect, false, this._position, this.rebirth, this.color);
            }
        }
    }
}
