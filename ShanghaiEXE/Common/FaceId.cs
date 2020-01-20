using ExtensionMethods;

namespace Common
{
    public class FaceId
    {
        public FaceId(int sheet, byte index, bool mono)
        {
            this.Sheet = sheet;
            this.Index = index;
            this.Mono = mono;
        }

        public FaceId() : this(0, 0, false) { }

        public int Sheet { get; }
        public byte Index { get; }

        public bool Mono { get; }

        public override string ToString()
        {
            return this.ToFace().ToString();
        }
    }
}
