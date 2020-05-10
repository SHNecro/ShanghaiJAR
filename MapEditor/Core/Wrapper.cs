using MapEditor.ExtensionMethods;
using System;

namespace MapEditor.Core
{
    public class Wrapper<T> : ViewModelBase, ICloneable
    {
        private T value;

        public Wrapper(T value = default(T))
        {
            this.Value = value;
        }

        public T Value
        {
            get { return this.value; }
            set { this.SetValue(ref this.value, value); }
        }

        public object Clone()
        {
            var clonableValue = (this.Value as ICloneable);
            return (clonableValue == null ? this.Value : (T)clonableValue.Clone()).Wrap<T>();
        }
    }
}
