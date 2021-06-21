using System;
using System.Collections.Generic;
using System.Drawing;

namespace Common.OpenGL
{
    public class Text : IEquatable<Text>
    {
        private static Dictionary<Text, string> CachedTextKeys;

        public string Content { get; set; }
        public Point Position { get; set; }
        public LoadedFont Font { get; set; }
        public Color Color { get; set; }

        static Text()
        {
            Text.CachedTextKeys = new Dictionary<Text, string>();
        }

        public string GetCharKey(int i)
        {
            if (!Text.CachedTextKeys.TryGetValue(this, out var textKey))
            {
                textKey = $"{this.Font.Font.Name}:{this.Font.Font.SizeInPoints:0.##}";
                Text.CachedTextKeys[this] = textKey;
            }

            return this.Content[i] + textKey;
        }

        public bool Equals(Text other)
        {
            return other != null &&
                   Content == other.Content &&
                   EqualityComparer<Point>.Default.Equals(this.Position, other.Position) &&
                   EqualityComparer<Font>.Default.Equals(this.Font.Font, other.Font.Font) &&
                   EqualityComparer<byte[]>.Default.Equals(this.Font.Bytes, other.Font.Bytes) &&
                   EqualityComparer<Color>.Default.Equals(this.Color, other.Color);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Text);
        }

        public override int GetHashCode()
        {
            var hashCode = 175167428;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Content);
            hashCode = hashCode * -1521134295 + EqualityComparer<Point>.Default.GetHashCode(Position);
            hashCode = hashCode * -1521134295 + EqualityComparer<LoadedFont>.Default.GetHashCode(Font);
            hashCode = hashCode * -1521134295 + EqualityComparer<Color>.Default.GetHashCode(Color);
            return hashCode;
        }
    }
}
