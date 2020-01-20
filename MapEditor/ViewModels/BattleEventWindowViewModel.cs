using MapEditor.Core;
using MapEditor.Models.Elements.Events;
using MapEditor.Rendering;
using System;

namespace MapEditor.ViewModels
{
    public class BattleEventWindowViewModel : ViewModelBase
    {
        public BattleEvent CurrentBattleEvent
        {
            get
            {
                return BattleEventRenderer.CurrentBattleEvent;
            }
            set
            {
                BattleEventRenderer.CurrentBattleEvent = value;
                this.OnPropertyChanged(nameof(this.CurrentBattleEvent));
            }
        }
        
        public Action<BattleEvent> BattleEventSetterAction { get; set; }


    }
}
