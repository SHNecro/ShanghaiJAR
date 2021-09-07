using Common.Vectors;
using NSBattle;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using System;
using System.Drawing;

namespace NSEffect
{
    public class RiveradarCrosshair : EffectBase
    {
        private int version;

        public RiveradarCrosshair(IAudioEngine s, SceneBattle p, int pX, int pY, string picturename, int version)
            : base(s, p, pX, pY)
        {
            this.height = 40;
            this.version = version;
            this.picturename = picturename;
        }

        public int ManualFrame { get; set; }

        public override void Render(IRenderer dg)
        {
            this._position = new Vector2(this.position.X * 40 + 20, this.position.Y * 24 + 70);
            this._rect = new Rectangle(608 + this.ManualFrame * 32, Math.Min(this.version, (byte)4) * this.height, 32, 32);
            if (this.version == 0)
                this._rect.Y = 5 * this.height;
            dg.DrawImage(dg, this.picturename, this._rect, false, this._position, this.rebirth, this.color);
        }
    }
}
