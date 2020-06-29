using MapEditor.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapEditor.ViewModels
{
    public class KeyItemDataViewModel : ViewModelBase
    {
        private bool isDirty;

        public bool IsDirty
        {
            get { return this.isDirty; }
            set { this.SetValue(ref this.isDirty, value); }
        }
    }
}
