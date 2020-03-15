using System;
using System.Drawing;
using System.Linq;

namespace Common.OpenGL
{
    public class TextMeasurer
    {
        public Size MeasureText(string text, Font font, int roundIncrement)
        {
            var unroundedSize = this.MeasureText(text, font);
            return new Size((int)Math.Ceiling(((double)unroundedSize.Width / roundIncrement) + 1) * roundIncrement, (int)Math.Ceiling(((double)unroundedSize.Height / roundIncrement) + 1) * roundIncrement);
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

            var renderedText = new Text { Color = Color.Black, Content = text, Font = font, Position = new Point(0, 0) };
            var glyphMetrics = Enumerable.Range(0, text.Length).Select(i => FontGlyphs.GetOrSetGlyphMetrics(renderedText, i));
            return new Size((int)Math.Round(glyphMetrics.Sum(gm => gm.Metrics.HorizontalAdvance.ToDouble())), (int)Math.Round(glyphMetrics.Max(gm => gm.Metrics.Height.ToDouble())));
        }
    }
}
