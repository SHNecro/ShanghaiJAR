using MapEditor.Core;
using MapEditor.Rendering;

namespace MapEditor.ViewModels
{
    public class DataWindowViewModel : ViewModelBase
    {
        private int selectedTabIndex;

        public DataWindowViewModel()
        {
            this.MessagesDataViewModel = new MessagesDataViewModel();
            this.KeyItemDataViewModel = new KeyItemDataViewModel();
            this.MailDataViewModel = new MailDataViewModel();
            this.CharacterInfoDataViewModel = CharacterInfoRenderer.ViewModel;
            this.BGMDataViewModel = BGMDataViewModel.Instance;
        }

        public int SelectedTabIndex
        {
            get { return this.selectedTabIndex; }
            set { this.SetValue(ref this.selectedTabIndex, value); }
        }

        public MessagesDataViewModel MessagesDataViewModel { get; }

        public KeyItemDataViewModel KeyItemDataViewModel { get; }

        public MailDataViewModel MailDataViewModel { get; }

        public CharacterInfoDataViewModel CharacterInfoDataViewModel { get; }

        public BGMDataViewModel BGMDataViewModel { get; }

        public void Remove()
        {
            this.BGMDataViewModel.Remove();
        }
    }
}
