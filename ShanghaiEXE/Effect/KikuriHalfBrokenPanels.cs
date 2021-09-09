using NSBattle;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSEffect
{
    internal class KikuriHalfBrokenPanels : EffectBase
    {
        public KikuriHalfBrokenPanels(IAudioEngine s, SceneBattle p)
          : base(s, p, 0, 0)
        {
            this.downprint = true;
        }

        public override void Updata()
        {
        }

        public override void Render(IRenderer dg)
        {
            for (var row = 0; row < 3; row++)
            {
                this._position = new Vector2(160 + this.Shake.X, 70 + (24 * row) + this.Shake.Y);
                this._rect = new Rectangle(1296, 120, 40, 32);
                dg.DrawImage(dg, "kikuriAttack", this._rect, true, this._position, this.rebirth, Color.White);
            }
        }
    }
}
