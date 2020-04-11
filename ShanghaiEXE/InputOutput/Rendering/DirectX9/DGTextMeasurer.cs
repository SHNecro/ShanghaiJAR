using System;
using System.Drawing;

namespace NSShanghaiEXE.InputOutput.Rendering.DirectX9
{
    public class DGTextMeasurer : ITextMeasurer
    {
        private Font regularFont;
        private Font miniFont;
        private Font microFont;

        public DGTextMeasurer(Font regularFont, Font miniFont, Font microFont)
        {
            this.regularFont = regularFont;
            this.miniFont = miniFont;
            this.microFont = microFont;
        }

        public DGTextMeasurer()
        {
        }

        public Size MeasureRegularText(string text) => this.MeasureText(text, this.regularFont);

        public Size MeasureMiniText(string text) => this.MeasureText(text, this.miniFont);

        public Size MeasureMicroText(string text) => this.MeasureText(text, this.microFont);

        public Size MeasureText(string text, Font font, int roundIncrement)
        {
            var unroundedSize = this.MeasureText(text, font);
            return new Size((int)Math.Ceiling((double)unroundedSize.Width / roundIncrement) * roundIncrement, (int)Math.Ceiling((double)unroundedSize.Height / roundIncrement) * roundIncrement);
        }

        public Size MeasureText(string text, Font font)
        {
            if (font == null)
            {
                throw new ArgumentNullException();
            }

            if (text == string.Empty)
            {
                return new Size(0, 0);
            }

            using (var graphics = Graphics.FromHwnd(IntPtr.Zero))
            {
                using (var slimFont = new SlimFont(font))
                {
                    return slimFont.Font.MeasureString(null, text, SlimDX.Direct3D9.DrawTextFormat.Left).Size;
                }
            }
        }
    }
}
