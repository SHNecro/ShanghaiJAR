using MapEditor.ViewModels;

namespace MapEditor.Models.Elements.Events
{
    public class StartBGMEvent : EventBase
    {
        private string bgmName;

        public StartBGMEvent()
        {
            // TODO: MEMORY LEAK
            // However, would require propagating Dispose() all the way down Map, which would have been nice to do at the very start
            if (BGMDataViewModel.BGMDefinitions != null)
            {
                BGMDataViewModel.BGMDefinitions.CollectionChanged += (sender, args) => { this.OnPropertyChanged(nameof(this.BGMName)); };
            }
        }

        public string BGMName
        {
            get
            {
                return this.bgmName;
            }
            set
            {
                if (value != null)
                {
                    this.SetValue(ref this.bgmName, value);
                }
            }
        }

        public override string Info => "Starts playing BGM from a .ogg, with \"mod/music\" overriding \"music/\".";

        public override string Name => $"Play BGM: \"{this.BGMName}\"";

        protected override string GetStringValue()
        {
            return $"bgmon:{this.BGMName}";
        }

        protected override void SetStringValue(string value)
        {
            var entries = value.Split(':');
            if (!this.Validate(entries, $"Malformed bgm start event \"{value}\".", e => e.Length >= 2 && e[0] == "bgmon"))
            {
                return;
            }

            var newBGMName = entries[1];

            this.BGMName = newBGMName;
        }
    }
}
