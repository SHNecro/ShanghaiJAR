using System.Drawing;
using System.Windows.Forms;

namespace Common.OpenGL
{
    public interface IControl
    {
        Control GetControl();

        void SetSize(Size size);
    }
}
