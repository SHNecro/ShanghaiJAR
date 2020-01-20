namespace MapEditor.Core
{
    public class MapDisplayOptions : ViewModelBase
    {
        private bool isAnimating;
        private bool isOutlining;
        private bool isShowingMoves;
        private bool isShowingIDs;

        public MapDisplayOptions()
        {
            this.isAnimating = true;
            this.isOutlining = true;
            this.isShowingMoves = true;
            this.isShowingIDs = false;
        }

        public bool IsAnimating
        {
            get { return this.isAnimating; }
            set { this.SetValue(ref this.isAnimating, value); }
        }

        public bool IsOutlining
        {
            get { return this.isOutlining; }
            set { this.SetValue(ref this.isOutlining, value); }
        }

        public bool IsShowingMoves
        {
            get { return this.isShowingMoves; }
            set { this.SetValue(ref this.isShowingMoves, value); }
        }

        public bool IsShowingIDs
        {
            get { return this.isShowingIDs; }
            set { this.SetValue(ref this.isShowingIDs, value); }
        }
    }
}
