using MapEditor.Core;
using MapEditor.Rendering;

namespace MapEditor.ViewModels
{
    public class DataWindowViewModel : ViewModelBase
    {
        public DataWindowViewModel()
        {
            this.MessagesDataViewModel = new MessagesDataViewModel();
            this.CharacterInfoDataViewModel = CharacterInfoRenderer.ViewModel;
        }

        public MessagesDataViewModel MessagesDataViewModel { get; }

        public CharacterInfoDataViewModel CharacterInfoDataViewModel { get; }
    }
}
