using MapEditor.Core;
using System;
using System.Windows;
using System.Windows.Controls;

namespace MapEditor.Controls
{
    /// <summary>
    /// Interaction logic for TranslatedTextSelector.xaml
    /// </summary>
    public partial class TranslatedTextSelector : UserControl
    {
        public static readonly DependencyProperty TextKeyProperty = DependencyProperty.Register("TextKey", typeof(string), typeof(TranslatedTextSelector), new PropertyMetadata(null));
        public static readonly DependencyProperty InvalidKeyTextProperty = DependencyProperty.Register("InvalidKeyText", typeof(string), typeof(TranslatedTextSelector), new PropertyMetadata(null));

        public TranslatedTextSelector()
        {
            InitializeComponent();

            this.Loaded += (sender, args) => { Constants.TranslationService.KeysReloaded += this.RefreshText; };
            this.Unloaded += (sender, args) => { Constants.TranslationService.KeysReloaded -= this.RefreshText; };
        }

        public string TextKey
        {
            get
            {
                return (string)this.GetValue(TextKeyProperty);
            }

            set
            {
                this.SetValue(TextKeyProperty, value);
            }
        }

        public string InvalidKeyText
        {
            get
            {
                return (string)this.GetValue(InvalidKeyTextProperty);
            }

            set
            {
                this.SetValue(InvalidKeyTextProperty, value);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TranslationKeySelectionWindow.SetKeySetterAction(key => this.TextKey = key);
            TranslationKeySelectionWindow.ShowWindow(this.TextKey);
            this.TranslatedTextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
        }

        private void RefreshText(object sender, EventArgs args)
        {
            this.TranslatedTextBox.GetBindingExpression(TextBox.TextProperty).UpdateTarget();
        }
    }
}
