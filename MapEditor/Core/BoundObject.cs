using System.Windows;

namespace MapEditor.Core
{
    public class BoundObject : DependencyObject
    {
        public static readonly DependencyProperty BindingProperty = DependencyProperty.Register("Binding", typeof(object), typeof(BoundObject), new PropertyMetadata(null));

        public object Binding
        {
            get { return this.GetValue(BindingProperty); }
            set { this.SetValue(BindingProperty, value); }
        }
    }
}
