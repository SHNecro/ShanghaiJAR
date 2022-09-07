using System;
using Avalonia.Threading;
using System.Runtime.CompilerServices;
using System.ComponentModel;

namespace KeyConfigLinux.Common
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        
        protected void SetValue<TValue>(
            ref TValue field,
            TValue value,
            [CallerMemberName] string propertyName = "")
        {
            field = value;
            this.OnPropertyChanged(propertyName);
        }
        
        protected void SetValue<TValue>(
            Action<TValue> fieldSetter,
            TValue value,
            [CallerMemberName] string propertyName = "")
        {
            fieldSetter.Invoke(value);
            this.OnPropertyChanged(propertyName);
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
