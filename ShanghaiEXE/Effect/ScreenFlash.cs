using NSBattle;
using NSChip;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSEffect
{
    internal class ScreenFlash : EffectBase
    {
        private readonly bool print = true;
        private int alpha = byte.MaxValue;
        private readonly int fadespeed = 25;
        private new readonly bool rebirth;

        public ScreenFlash(
          IAudioEngine s,
          SceneBattle p,
          Vector2 pd,
          Point posi,
          ChipBase.ELEMENT ele,
          int _speed,
          bool rebirth)
          : base(s, p, posi.X, posi.Y)
        {
            this.rebirth = rebirth;
            this.element = ele;
            this.speed = _speed;
            this.positionDirect = pd;
            this.upprint = true;
            this.animationpoint.X = 3;
            this.color = Color.FromArgb(byte.MaxValue, Color.White);
        }

        public ScreenFlash(IAudioEngine s, SceneBattle p)
          : base(s, p, 0, 0)
        {
            this.upprint = true;
            this.animationpoint.X = 3;
            this.color = Color.FromArgb(byte.MaxValue, Color.White);
        }

        public ScreenFlash(IAudioEngine s, SceneBattle p, Color color, int fadespeed)
          : base(s, p, 0, 0)
        {
            this.fadespeed = fadespeed;
            this.alpha = fadespeed != byte.MaxValue ? byte.MaxValue : 0;
            this.downprint = true;
            this.animationpoint.X = 3;
            this.color = Color.FromArgb(this.alpha, color);
        }

        public override void Updata()
        {
            if (this.fadespeed <= 0)
                return;
            this.animationpoint.X = this.frame;
            this.FlameControl();
            this.alpha -= this.fadespeed;
            if (this.alpha < 0)
                this.alpha = 0;
            this.color = Color.FromArgb(this.alpha, this.color);
            if (this.moveflame && this.frame > 10)
                this.flag = false;
        }

        public override void Render(IRenderer dg)
        {
            if (!this.print)
                return;
            this._position = new Vector2(0.0f, 0.0f);
            this._rect = new Rectangle(0, 0, 240, 160);
            dg.DrawImage(dg, "fadescreen", this._rect, true, this._position, this.rebirth, this.color);
        }
    }
}
