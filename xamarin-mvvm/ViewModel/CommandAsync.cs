using System;
using System.Windows.Input;
using System.Threading.Tasks;

namespace XamarinMvvm.ViewModel
{
	public abstract class BaseCommandAsync: ICommand
	{
		public event EventHandler CanExecuteChanged;

		protected BaseViewModel BaseViewModel { get; private set;}

		public BaseCommandAsync(BaseViewModel baseViewModel)
		{
			this.BaseViewModel = baseViewModel;
			this.BaseViewModel.IsBusy = false;
		}

		public bool CanExecute (object parameter) => this.BaseViewModel.IsNotBusy;

		public abstract void Execute(object parameter);

		protected void RaiseCanExecuteChanged() => this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);
	}

	public class CommandAsync : BaseCommandAsync
	{
		private Func<Task> _command;

		public CommandAsync(BaseViewModel baseViewModel, Func<Task> command) : base(baseViewModel)
		{
			this._command = command;
		}

		public override async void Execute(object parameter)
		{
			this.RaiseCanExecuteChanged();

			this.BaseViewModel.IsBusy = true;
			await this._command();
			this.BaseViewModel.IsBusy = false;
		}
	}

	public class CommandAsync<TParameter> : BaseCommandAsync
	{
		private Func<TParameter, Task> _command;

		public CommandAsync(BaseViewModel baseViewModel, Func<TParameter, Task> command) : base(baseViewModel)
		{
			this._command = command;
		}

		public override async void Execute(object parameter)
		{
			this.RaiseCanExecuteChanged();

			this.BaseViewModel.IsBusy = true;
			await this._command((TParameter) parameter);
			this.BaseViewModel.IsBusy = false;
		}
	}
}