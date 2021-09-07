using NSBattle;
using NSChip;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSEffect
{
    internal class ScreenBlack : EffectBase
    {
        private readonly bool print = true;
        private int alpha = 0;
        private readonly int alphaPlus = 25;
        private readonly Color color_ = Color.Black;
        private new readonly bool rebirth;
        private int endAlpha = byte.MaxValue;
        public bool end;

        public ScreenBlack(
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
            this.downprint = true;
            this.speed = _speed;
            this.positionDirect = pd;
            this.animationpoint.X = 3;
            this.color = Color.FromArgb(0, Color.Black);
        }

        public ScreenBlack(
          IAudioEngine s,
          SceneBattle p,
          Vector2 pd,
          Point posi,
          ChipBase.ELEMENT ele,
          int _speed,
          bool rebirth,
          Color color,
          int alphaPlus)
          : base(s, p, posi.X, posi.Y)
        {
            this.rebirth = rebirth;
            this.element = ele;
            this.downprint = true;
            this.speed = _speed;
            this.positionDirect = pd;
            this.animationpoint.X = 3;
            this.alphaPlus = alphaPlus;
            this.endAlpha = color.A;
            this.color_ = Color.FromArgb(0, color);
            this.color = Color.FromArgb(0, Color.Black);
        }

        public override void Updata()
        {
            if (!this.end)
            {
                if (this.alpha < endAlpha)
                {
                    this.alpha += this.alphaPlus;
                    if (this.alpha > endAlpha)
                        this.alpha = endAlpha;
                }
                this.color = Color.FromArgb(this.alpha, this.color_);
            }
            else
            {
                this.alpha -= this.alphaPlus;
                if (this.alpha < 0)
                    this.alpha = 0;
                this.color = Color.FromArgb(this.alpha, this.color_);
                if (this.alpha == 0)
                    this.flag = false;
            }
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
