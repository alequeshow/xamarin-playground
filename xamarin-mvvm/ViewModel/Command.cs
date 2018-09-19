using System;
using System.Windows.Input;

namespace XamarinMvvm.ViewModel
{
	public class Command<T> : ICommand
	{
		public event EventHandler CanExecuteChanged;

		private Action<T> _command;

		public Command(Action<T> command) 
		{
			this._command = command;
		}

		public bool CanExecute (object parameter) => true;

		public void Execute(object parameter)
		{
			this.RaiseCanExecuteChanged();
			this._command((T)parameter);
		}

		private void RaiseCanExecuteChanged() => this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);
	}
}