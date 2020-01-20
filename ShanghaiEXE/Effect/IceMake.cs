﻿using NSBattle;
using NSShanghaiEXE.InputOutput;
using NSShanghaiEXE.InputOutput.Rendering.DirectX9;
using NSShanghaiEXE.InputOutput.Rendering;
using SlimDX;
using System.Drawing;

namespace NSEffect
{
    internal class IceMake : EffectBase
    {
        private readonly int[,] action = new int[2, 5]
        {
      {
        0,
        1,
        2,
        3,
        4
      },
      {
        0,
        1,
        2,
        3,
        4
      }
        };
        private int angle = 0;
        private const int interval = 4;
        private const int intervalLong = 16;
        private int f;

        private int Angle
        {
            get
            {
                return this.angle;
            }
            set
            {
                this.angle = value;
                if (this.angle >= 0)
                    return;
                this.angle = 7;
            }
        }

        public IceMake(MyAudio s, Vector2 pd, Point posi)
          : base(s, null, posi.X, posi.Y)
        {
            this.positionDirect = pd;
            this.animationpoint.X = 3;
        }

        public override void Updata()
        {
            this.FlameControl(4);
            if (this.moveflame && this.animationpoint.X > 0)
                --this.animationpoint.X;
            ++this.f;
        }

        public override void Render(IRenderer dg)
        {
            this._rect = new Rectangle(this.animationpoint.X * 56 + 128, 0, 56, 56);
            this._position = new Vector2(this.positionDirect.X + Shake.X, this.positionDirect.Y + Shake.Y);
            this.color = Color.White;
            dg.DrawImage(dg, "body7", this._rect, false, this._position, this.rebirth, this.color);
        }
    }
}
