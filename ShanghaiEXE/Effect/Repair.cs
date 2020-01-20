﻿using NSBattle;
using NSShanghaiEXE.InputOutput;
using NSShanghaiEXE.InputOutput.Rendering.DirectX9;
using NSShanghaiEXE.InputOutput.Rendering;
using SlimDX;
using System.Drawing;

namespace NSEffect
{
    internal class Repair : EffectBase
    {
        public Repair(MyAudio s, SceneBattle p, Vector2 pd, int sp, Point posi)
          : base(s, p, posi.X, posi.Y)
        {
            this.speed = sp;
            this.positionDirect = pd;
        }

        public Repair(MyAudio s, SceneBattle p, int pX, int pY, int sp)
          : base(s, p, pX, pY)
        {
            this.speed = sp;
            this.positionDirect = new Vector2(pX * 40 + 20, pY * 24 + 80);
        }

        public override void Updata()
        {
            this.animationpoint.X = this.frame;
            if (this.frame >= 6)
                this.flag = false;
            this.FlameControl();
        }

        public override void Render(IRenderer dg)
        {
            this._rect = new Rectangle(40 * this.animationpoint.X, 144, 40, 72);
            this._position = new Vector2(this.positionDirect.X + Shake.X, this.positionDirect.Y + Shake.Y);
            this.color = Color.White;
            dg.DrawImage(dg, "bomber", this._rect, false, this._position, this.rebirth, this.color);
        }
    }
}
