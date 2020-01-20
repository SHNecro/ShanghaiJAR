using System;
using System.Windows.Forms;

namespace Common.OpenGL
{
    public interface IMouseInteractionControl : IControl
    {
        event MouseEventHandler ScaledMouseDown;
        event MouseEventHandler ScaledMouseMove;
        event MouseEventHandler ScaledMouseUp;
        event EventHandler ScaledMouseEnter;
        event EventHandler ScaledMouseHover;
        event EventHandler ScaledMouseLeave;
    }
}
