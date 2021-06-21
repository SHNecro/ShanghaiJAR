
using Common.Vectors;
using Common.OpenGL;
using System.Drawing;

namespace NSShanghaiEXE.InputOutput.Rendering.OpenGL
{
    public class GLTextMeasurer : ITextMeasurer
    {
        private static TextMeasurer measurer;

        private LoadedFont regularFont;
        private LoadedFont miniFont;
        private LoadedFont microFont;

        static GLTextMeasurer()
        {
            GLTextMeasurer.measurer = new TextMeasurer();
        }

        public GLTextMeasurer(LoadedFont regularFont, LoadedFont miniFont, LoadedFont microFont)
        {
            this.regularFont = regularFont;
            this.miniFont = miniFont;
            this.microFont = microFont;
        }

        public Size MeasureRegularText(string text) => this.MeasureText(text, this.regularFont);

        public Size MeasureMiniText(string text) => this.MeasureText(text, this.miniFont);

        public Size MeasureMicroText(string text) => this.MeasureText(text, this.microFont);

        public Size MeasureText(string text, LoadedFont font, int roundIncrement)
        {
            return GLTextMeasurer.measurer.MeasureText(text, font, roundIncrement);
        }

        public Size MeasureText(string text, LoadedFont font)
        {
            return GLTextMeasurer.measurer.MeasureText(text, font);
        }
    }
}
