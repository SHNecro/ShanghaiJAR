using NSBattle;
using NSChip;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;
using System;

namespace NSEffect
{
    internal class ScreenObjectFade : EffectBase
    {
        private readonly bool print = true;
        private int alpha = 0;
        private readonly int alphaPlus = 25;
        private readonly Func<Color> finalColorGetter;
        private new readonly bool rebirth;
        private int endAlpha = byte.MaxValue;

        private bool isText;
        private Func<string> textGetter;

        private string spriteSheet;
        private Func<Rectangle> spriteTextureGetter;

        public bool end;
        
        public ScreenObjectFade(
          IAudioEngine s,
          SceneBattle p,
          Func<string> textGetter,
          Vector2 pd,
          Point posi,
          int _speed,
          bool rebirth,
          Func<Color> colorGetter,
          int alphaPlus)
          : base(s, p, posi.X, posi.Y)
        {
            this.rebirth = rebirth;
            this.textGetter = textGetter;
            this.upprint = true;
            this.speed = _speed;
            this.positionDirect = pd;
            this.animationpoint.X = 3;
            this.alphaPlus = alphaPlus;
            this.endAlpha = colorGetter().A;
            this.finalColorGetter = colorGetter;
            this.color = Color.FromArgb(0, Color.Black);

            this.isText = true;
            this.blackOutObject = true;
        }

        public ScreenObjectFade(
          IAudioEngine s,
          SceneBattle p,
          string spriteSheet,
          Func<Rectangle> spriteTextureGetter,
          Vector2 pd,
          Point posi,
          int _speed,
          bool rebirth,
          Func<Color> colorGetter,
          int alphaPlus)
          : base(s, p, posi.X, posi.Y)
        {
            this.rebirth = rebirth;
            this.spriteSheet = spriteSheet;
            this.spriteTextureGetter = spriteTextureGetter;
            this.upprint = true;
            this.speed = _speed;
            this.positionDirect = pd;
            this.animationpoint.X = 3;
            this.alphaPlus = alphaPlus;
            this.endAlpha = colorGetter().A;
            this.finalColorGetter = colorGetter;
            this.color = Color.FromArgb(0, Color.Black);

            this.isText = false;
            this.blackOutObject = true;
        }

        public ScreenObjectFade(
          IAudioEngine s,
          SceneBattle p,
          Func<string> textGetter,
          Vector2 pd,
          Point posi,
          int _speed,
          bool rebirth,
          Color color,
          int alphaPlus)
          : this(s, p, textGetter, pd, posi, _speed, rebirth, () => color, alphaPlus) { }

        public ScreenObjectFade(
          IAudioEngine s,
          SceneBattle p,
          string text,
          Vector2 pd,
          Point posi,
          int _speed,
          bool rebirth,
          Func<Color> colorGetter,
          int alphaPlus)
          : this(s, p, () => text, pd, posi, _speed, rebirth, colorGetter, alphaPlus) { }

        public ScreenObjectFade(
          IAudioEngine s,
          SceneBattle p,
          string text,
          Vector2 pd,
          Point posi,
          int _speed,
          bool rebirth,
          Color color,
          int alphaPlus)
          : this(s, p, () => text, pd, posi, _speed, rebirth, () => color, alphaPlus) { }

        public ScreenObjectFade(
          IAudioEngine s,
          SceneBattle p,
          string spriteSheet,
          Func<Rectangle> spriteTextureGetter,
          Vector2 pd,
          Point posi,
          int _speed,
          bool rebirth,
          Color color,
          int alphaPlus)
          : this(s, p, spriteSheet, spriteTextureGetter, pd, posi, _speed, rebirth, () => color, alphaPlus)
        { }

        public ScreenObjectFade(
          IAudioEngine s,
          SceneBattle p,
          string spriteSheet,
          Rectangle spriteTexture,
          Vector2 pd,
          Point posi,
          int _speed,
          bool rebirth,
          Func<Color> colorGetter,
          int alphaPlus)
          : this(s, p, spriteSheet, () => spriteTexture, pd, posi, _speed, rebirth, colorGetter, alphaPlus)
        { }

        public ScreenObjectFade(
          IAudioEngine s,
          SceneBattle p,
          string spriteSheet,
          Rectangle spriteTexture,
          Vector2 pd,
          Point posi,
          int _speed,
          bool rebirth,
          Color color,
          int alphaPlus)
          : this(s, p, spriteSheet, () => spriteTexture, pd, posi, _speed, rebirth, () => color, alphaPlus)
        { }

        public override void Updata()
        {
            this.endAlpha = this.finalColorGetter().A;
            if (!this.end)
            {
                if (this.alpha < endAlpha)
                {
                    this.alpha += this.alphaPlus;
                }
                if (this.alpha > endAlpha)
                    this.alpha = endAlpha;
                this.color = Color.FromArgb(this.alpha, this.finalColorGetter());
            }
            else
            {
                this.alpha -= this.alphaPlus;
                if (this.alpha < 0)
                    this.alpha = 0;
                this.color = Color.FromArgb(this.alpha, this.finalColorGetter());
                if (this.alpha == 0)
                    this.flag = false;
            }
        }

        public override void Render(IRenderer dg)
        {
            if (!this.print)
                return;
            this._position = new Vector2(this.positionDirect.X + Shake.X, (float)(positionDirect.Y + (double)this.Shake.Y));
            if (this.isText)
            {
                DrawBlockCharacters(dg, this.Nametodata(this.textGetter()), 88, this._position, this.color, out var finalRect, out var finalPos);
            }
            else
            {
                this._rect = this.spriteTextureGetter();
                dg.DrawImage(dg, this.spriteSheet, this._rect, true, this._position, this.rebirth, this.color);
            }
        }
    }
}
