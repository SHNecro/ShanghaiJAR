using MapEditor.Core;
using MapEditor.Rendering;

namespace MapEditor.ViewModels
{
    public class DataWindowViewModel : ViewModelBase
    {
        public DataWindowViewModel()
        {
            this.MessagesDataViewModel = new MessagesDataViewModel();
            this.KeyItemDataViewModel = new KeyItemDataViewModel();
            this.MailDataViewModel = new MailDataViewModel();
            this.CharacterInfoDataViewModel = CharacterInfoRenderer.ViewModel;
        }

        public MessagesDataViewModel MessagesDataViewModel { get; }

        public KeyItemDataViewModel KeyItemDataViewModel { get; }

        public MailDataViewModel MailDataViewModel { get; }

        public CharacterInfoDataViewModel CharacterInfoDataViewModel { get; }
    }
}
