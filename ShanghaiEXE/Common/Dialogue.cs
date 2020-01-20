using ExtensionMethods;

namespace Common
{
    public class Dialogue
    {
        private string[] lines = new string[] { "", "", "" };
        private string text = ",,";

        public string Text
        {
            get => text;
            set
            {
                text = value;
                lines = value?.Split(',');
            }
        }
        public FaceId Face { get; set; } = FACE.None.ToFaceId();

        public Dialogue Format(params object[] args) => new Dialogue { Text = string.Format(this.Text, args), Face = this.Face };

        public string this[int i] => this.lines != null && i < this.lines.Length && i >= 0 ? this.lines[i] : string.Empty;

        public static implicit operator string(Dialogue d) => d.Text;
    }
}
