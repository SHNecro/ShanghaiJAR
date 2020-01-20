using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace MapEditor.Controls
{
    /// <summary>
    /// Interaction logic for StringEditableExpander.xaml
    /// </summary>
    public partial class StringEditableExpander : Expander
    {
        private TextBox StringTextBox { get; set; }

        public static readonly DependencyProperty IsEditingTextProperty = DependencyProperty.Register("IsEditingText", typeof(bool), typeof(StringEditableExpander), new PropertyMetadata(false));

        public StringEditableExpander()
        {
            InitializeComponent();
        }

        public bool IsEditingText
        {
            get { return (bool)this.GetValue(IsEditingTextProperty); }
            set { this.SetValue(IsEditingTextProperty, value); }
        }

        private void EditModeButton_Click(object sender, RoutedEventArgs args)
        {
            this.IsEditingText = !this.IsEditingText;

            // Expander doesn't load contents until first expanded
            try
            {
                BindingOperations.GetBindingExpression(this.StringTextBox, TextBox.TextProperty).UpdateTarget();
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.GetType().ToString()}: {e.Message}");
            }
        }

        private void StringTextBox_Loaded(object sender, RoutedEventArgs args)
        {
            this.StringTextBox = (TextBox)sender;
        }
    }
}
