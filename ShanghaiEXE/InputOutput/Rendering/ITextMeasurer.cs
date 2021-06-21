using System.Drawing;
using Common.OpenGL;

namespace NSShanghaiEXE.InputOutput.Rendering
{
    public interface ITextMeasurer
    {
        Size MeasureRegularText(string text);

        Size MeasureMiniText(string text);

        Size MeasureMicroText(string text);

        Size MeasureText(string text, LoadedFont font, int roundIncrement);

        Size MeasureText(string text, LoadedFont font);
    }
}
