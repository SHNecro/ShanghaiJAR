using System;
using System.Windows.Input;

namespace MapEditor.Core
{
	public class RelayCommand : ICommand
	{
		private readonly Predicate<object> canExecute;
		private readonly Action<object> execute;

		public RelayCommand(Predicate<object> canExecute, Action<object> execute)
		{
			this.canExecute = canExecute;
			this.execute = execute;
		}

		public RelayCommand(Action<object> execute)
		{
			this.canExecute = null;
			this.execute = execute;
		}

		public RelayCommand(Action execute)
		{
			this.canExecute = null;
			this.execute = (param) => execute();
		}

		public event EventHandler CanExecuteChanged
		{
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}

		public bool CanExecute(object parameter)
		{
			return canExecute == null ? true :  canExecute(parameter);
		}

		public void Execute(object parameter)
		{
			execute(parameter);
		}
	}
}
