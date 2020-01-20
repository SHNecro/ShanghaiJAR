using System.Drawing;

namespace Common.OpenGL
{
    public class OriginChangedEventArgs
    {
        public Point PreviousOrigin { get; set; }
        public Point NewOrigin { get; set; }
    }
}
