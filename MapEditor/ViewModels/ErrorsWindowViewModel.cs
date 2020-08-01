using MapEditor.Core;

namespace MapEditor.ViewModels
{
    public class ErrorsWindowViewModel : ViewModelBase
    {
        private string text;

        public string Text
        {
            get { return this.text; }
            set { this.SetValue(ref this.text, value); }
        }

        public void Refresh()
        {
            this.text = MainWindowViewModel.GetInstance().CurrentMap.StringValue;
        }
    }
}
