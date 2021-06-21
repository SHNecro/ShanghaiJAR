using System;
using System.Collections.Generic;
using System.Drawing;

namespace Common.OpenGL
{
    public class LoadedFont
    {
        public LoadedFont(Font font)
        {
            this.Font = font;
        }

        public LoadedFont(Font font, byte[] fontBytes)
            : this(font)
        {
            this.Bytes = fontBytes;
        }

        public Font Font { get; }
        public byte[] Bytes { get; }

        public bool Equals(LoadedFont other)
        {
            return other != null &&
                   EqualityComparer<Font>.Default.Equals(this.Font, other.Font) &&
                   EqualityComparer<byte[]>.Default.Equals(this.Bytes, other.Bytes);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as LoadedFont);
        }

        public override int GetHashCode()
        {
            var hashCode = 175167428;
            hashCode = hashCode * -1521134295 + EqualityComparer<Font>.Default.GetHashCode(this.Font);
            hashCode = hashCode * -1521134295 + EqualityComparer<byte[]>.Default.GetHashCode(this.Bytes);
            return hashCode;
        }
    }
}
