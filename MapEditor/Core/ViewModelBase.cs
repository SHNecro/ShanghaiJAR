using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MapEditor.Core
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void SetValue<T>(ref T property, T value, [CallerMemberName] string propertyName = null)
        {
            if (property != null)
            {
                if (property.Equals(value)) return;
            }

            property = value;
            this.OnPropertyChanged(propertyName);
        }

        public void SetValue<T>(Func<T> getAction, Action<T> setAction, T value, [CallerMemberName] string propertyName = null)
        {
            var property = getAction();
            #pragma warning disable RECS0017 // Possible compare of value type with 'null'
            if (typeof(T).IsValueType && property != null)
            {
                if (property.Equals(value)) return;
            }
            #pragma warning restore RECS0017 // Possible compare of value type with 'null'

            setAction(value);
            this.OnPropertyChanged(propertyName);
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
