using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSEffect
{
    internal class AliceBed : EffectBase
    {
        private readonly int[,] action = new int[2, 9]
        {
      {
        0,
        1,
        2,
        3,
        5,
        6,
        7,
        8,
        9
      },
      {
        0,
        1,
        2,
        3,
        4,
        3,
        4,
        3,
        2
      }
        };
        private readonly float y = 0.0f;
        private const int interval = 6;
        private readonly int jumpflame;
        private bool se;

        public AliceBed(IAudioEngine s, Vector2 pd, Point posi, bool mute = false)
          : base(s, null, posi.X, posi.Y)
        {
            this.positionDirect = pd;
            this.animationpoint.X = 0;
            this.se = mute;
        }

        public override void Updata()
        {
            if (!this.se)
            {
                this.sound.PlaySE(SoundEffect.futon);
                this.se = true;
            }
            this.FlameControl(6);
            if (!this.moveflame)
                return;
            for (int index = 0; index < this.action.GetLength(1); ++index)
            {
                if (this.frame == this.action[0, index])
                {
                    this.animationpoint.X = this.action[1, index];
                    break;
                }
            }
        }

        public override void Render(IRenderer dg)
        {
            this._rect = new Rectangle(this.animationpoint.X * 64, 224, 64, 64);
            this._position = new Vector2(this.positionDirect.X + Shake.X, (float)(positionDirect.Y + (double)this.Shake.Y - 6.0) - (int)this.y);
            this.color = Color.White;
            dg.DrawImage(dg, "body1", this._rect, false, this._position, this.rebirth, this.color);
        }
    }
}
