using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;
using System;
using NSBattle;

namespace NSEffect
{
    internal class BarrierDamageBlob : EffectBase
    {
        private const float Speed = 1.5f;
        private static readonly Rectangle TextureRect = new Rectangle(200, 160, 9, 9);

        private Vector2 destination;
        private Action onArrivalAction;

        public BarrierDamageBlob(IAudioEngine s, SceneBattle parent, Vector2 pd, Point posi, Vector2 destination, Action onArrivalAction)
          : base(s, null, posi.X, posi.Y)
        {
            this.parent = parent;
            this.positionDirect = pd;
            this.destination = destination;
            this.onArrivalAction = onArrivalAction;

            this.animationpoint.X = 0;
        }

        public override void Updata()
        {
            if (this.positionDirect == this.destination)
            {
                var repairEffect = new Repair(this.sound, this.parent, this.positionDirect, 2, this.position);
                repairEffect.upprint = true;
                this.parent.effects.Add(repairEffect);
                this.onArrivalAction?.Invoke();

                this.flag = false;
            }

            // Moves 1px a tick
            var path = this.destination - this.positionDirect;
            var magnitude = (float)Math.Sqrt(path.X * path.X + path.Y * path.Y);
            if (magnitude > Speed)
            {
                this.positionDirect = new Vector2(this.positionDirect.X + (path.X / magnitude) * Speed, this.positionDirect.Y + (path.Y / magnitude) * Speed);
            }
            else
            {
                this.positionDirect = this.destination;
            }

            this.FlameControl(2);
            if (this.moveflame)
            {
                this.animationpoint.X = ((this.animationpoint.X + 1) % 4);
                this.waittime++;
            }
            this.color = Color.FromArgb((int)Math.Min(255, (this.waittime / 5.0) * 255), Color.White);
        }

        public override void Render(IRenderer dg)
        {
            this._rect = new Rectangle(TextureRect.X + (this.animationpoint.X * TextureRect.Width), TextureRect.Y, TextureRect.Width, TextureRect.Height);
            this._position = new Vector2(Shake.X + this.positionDirect.X, Shake.Y + this.positionDirect.Y);
            dg.DrawImage(dg, "heavenbarrier", this._rect, false, this._position, this.rebirth, this.color);
        }
    }
}
