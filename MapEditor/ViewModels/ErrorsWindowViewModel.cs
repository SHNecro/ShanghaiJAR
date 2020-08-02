using MapEditor.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace MapEditor.ViewModels
{
    public class ErrorsWindowViewModel : ViewModelBase
    {
        private Tuple<Tuple<StringRepresentation, string>[], string> selectedError;

        public ErrorsWindowViewModel()
        {
            this.Errors = new ObservableCollection<Tuple<Tuple<StringRepresentation, string>[], string>>();
        }

        public ObservableCollection<Tuple<Tuple<StringRepresentation, string>[], string>> Errors { get; }

        public ICommand RefreshCommand => new RelayCommand(this.Refresh);

        public Tuple<Tuple<StringRepresentation, string>[], string> SelectedError
        {
            get
            {
                return this.selectedError;
            }

            set
            {
                this.SetValue(ref this.selectedError, value);
            }
        }

        public void Refresh()
        {
            this.Errors.Clear();
            var errors = MainWindowViewModel.GetInstance().CurrentMap.Errors;

            foreach (var err in errors)
            {
                var items = err.Item1.Reverse().Select(sr =>
                {
                    var label = sr.TypeName;
                    return Tuple.Create(sr, label);
                }).ToArray();
                var entry = Tuple.Create(items, err.Item2);

                this.Errors.Add(entry);
            }

            this.OnPropertyChanged(nameof(this.Errors));
            this.SelectedError = this.Errors.FirstOrDefault();
        }
    }
}
