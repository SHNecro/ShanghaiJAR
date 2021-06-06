
using Common.Vectors;
using Common.OpenGL;
using System.Drawing;

namespace NSShanghaiEXE.InputOutput.Rendering.OpenGL
{
    public class GLTextMeasurer : ITextMeasurer
    {
        private static TextMeasurer measurer;

        private Font regularFont;
        private Font miniFont;
        private Font microFont;

        static GLTextMeasurer()
        {
            GLTextMeasurer.measurer = new TextMeasurer();
        }

        public GLTextMeasurer(Font regularFont, Font miniFont, Font microFont)
        {
            this.regularFont = regularFont;
            this.miniFont = miniFont;
            this.microFont = microFont;
        }

        public Size MeasureRegularText(string text) => this.MeasureText(text, this.regularFont);

        public Size MeasureMiniText(string text) => this.MeasureText(text, this.miniFont);

        public Size MeasureMicroText(string text) => this.MeasureText(text, this.microFont);

        public Size MeasureText(string text, Font font, int roundIncrement)
        {
            return GLTextMeasurer.measurer.MeasureText(text, font, roundIncrement);
        }

        public Size MeasureText(string text, Font font)
        {
            return GLTextMeasurer.measurer.MeasureText(text, font);
        }
    }
}
