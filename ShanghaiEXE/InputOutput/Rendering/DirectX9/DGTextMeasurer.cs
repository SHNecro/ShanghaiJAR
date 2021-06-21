using System;
using System.Drawing;
using Common.OpenGL;

namespace NSShanghaiEXE.InputOutput.Rendering.DirectX9
{
    public class DGTextMeasurer : ITextMeasurer
    {
        private LoadedFont regularFont;
        private LoadedFont miniFont;
        private LoadedFont microFont;

        public DGTextMeasurer(LoadedFont regularFont, LoadedFont miniFont, LoadedFont microFont)
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

        public Size MeasureText(string text, LoadedFont font, int roundIncrement)
        {
            var unroundedSize = this.MeasureText(text, font);
            return new Size((int)Math.Ceiling((double)unroundedSize.Width / roundIncrement) * roundIncrement, (int)Math.Ceiling((double)unroundedSize.Height / roundIncrement) * roundIncrement);
        }

        public Size MeasureText(string text, LoadedFont font)
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
                using (var slimFont = new SlimFont(font.Font))
                {
                    return slimFont.Font.MeasureString(null, text, SlimDX.Direct3D9.DrawTextFormat.Left).Size;
                }
            }
        }
    }
}
