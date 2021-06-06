using NSBattle;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;
using System;

namespace NSEffect
{
    internal class FlashFead : EffectBase
    {
        private float alpha = byte.MaxValue;
        private readonly float minus;
        private new Color color;

        public FlashFead(IAudioEngine s, SceneBattle p, Color color, int endtime)
          : base(s, p, 0, 0)
        {
            this.positionDirect = new Vector2(0.0f, 0.0f);
            this.color = color;
            this.minus = this.alpha / endtime;
        }

        public override void Updata()
        {
            this.color = Color.FromArgb(Math.Max(0, (int)this.alpha), this.color);
            this.alpha -= this.minus;
            if (alpha > 0.0)
                return;
            this.flag = false;
        }

        public override void RenderDOWN(IRenderer dg)
        {
            this._rect = new Rectangle(0, 0, 240, 160);
            this._position = new Vector2(0.0f, 0.0f);
            dg.DrawImage(dg, "fadescreen", this._rect, true, this._position, this.color);
        }
    }
}
