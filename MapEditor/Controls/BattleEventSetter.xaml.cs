using MapEditor.Core;
using MapEditor.Models.Elements.Events;
using System.Windows;
using System.Windows.Controls;

namespace MapEditor.Controls
{
    /// <summary>
    /// Interaction logic for BattleEventSetter.xaml
    /// </summary>
    public partial class BattleEventSetter : UserControl
    {
        public static readonly DependencyProperty BattleEventProperty = DependencyProperty.Register("BattleEvent", typeof(BattleEvent), typeof(BattleEventSetter), new PropertyMetadata(null));

        public BattleEventSetter()
        {
            InitializeComponent();
        }

        public BattleEvent BattleEvent
        {
            get
            {
                return (BattleEvent)this.GetValue(BattleEventProperty);
            }

            set
            {
                this.SetValue(BattleEventProperty, value);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            BattleEventWindow.SetBattleEventSetterAction((page) =>
            {
                this.BattleEvent = page;
            });
            BattleEventWindow.ShowWindow(this.BattleEvent);
        }
    }
}
