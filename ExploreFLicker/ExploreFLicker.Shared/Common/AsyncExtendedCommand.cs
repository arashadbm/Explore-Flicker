using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ExploreFlicker.Common
{

    public class AsyncExtendedCommand : BindableBase, ICommand
    {
        private readonly Func<Task> _executeFunction;
        private readonly Func<CancellationToken, Task> _cancelableExecuteFunction;

        private bool _canExecute;
        public bool CanExecute
        {
            get { return _canExecute; }
            set
            {
                if(_canExecute != value)
                {
                    _canExecute = value;
                    OnPropertyChanged();
                    EventHandler canExecuteChanged = CanExecuteChanged;
                    if(canExecuteChanged != null)
                        canExecuteChanged(this, EventArgs.Empty);
                }
            }
        }

        private bool _isBusy;
        /// <summary>
        /// Manually set this value to indicate if task is busy executing.
        /// You can bind on this value in xaml to show progress.
        /// </summary>
        public bool IsBusy
        {
            get { return _isBusy; }
            set { SetProperty(ref _isBusy, value); }
        }


        private CancellationTokenSource _cts;
        public CancellationTokenSource Cts
        {
            get { return _cts; }
            private set { _cts = value; }
        }

        public AsyncExtendedCommand ( Func<Task> executeFunction, bool canExecute = true )
        {
            this._executeFunction = executeFunction;
            this._canExecute = canExecute;
        }

        public AsyncExtendedCommand ( Func<CancellationToken, Task> cancelableExecuteFunction, bool canExecute = true )
        {
            this._cancelableExecuteFunction = cancelableExecuteFunction;
            this._canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged;

        bool ICommand.CanExecute ( object parameter )
        {
            return CanExecute;
        }

        async void ICommand.Execute ( object parameter )
        {

            if(CanExecute)
            {
                if(_executeFunction != null)
                {
                    await _executeFunction.Invoke();
                }
                else if(_cancelableExecuteFunction != null)
                {
                    Cts = new CancellationTokenSource();
                    await _cancelableExecuteFunction.Invoke(Cts.Token);
                }
                else
                {
                    throw new ArgumentNullException("Command must have execution function");
                }
            }
        }

        public void Execute ( object parameter )
        {
            (this as ICommand).Execute(parameter);
        }

        public void Cancel ()
        {
            if(Cts != null && !Cts.IsCancellationRequested)
            {
                Cts.Cancel();
            }
        }

        public ExtendedCommand CancelCommand { get { return new ExtendedCommand(Cancel); } }
    }

    public class AsyncExtendedCommand<T> : BindableBase, ICommand
    {
        private readonly Func<T, Task> _executeFunction;
        private readonly Func<T, CancellationToken, Task> _cancelableExecuteFunction;

        private bool _canExecute;
        public bool CanExecute
        {
            get { return _canExecute; }
            set
            {
                if(_canExecute != value)
                {
                    _canExecute = value;
                    OnPropertyChanged();
                    EventHandler canExecuteChanged = CanExecuteChanged;
                    if(canExecuteChanged != null)
                        canExecuteChanged(this, EventArgs.Empty);
                }
            }
        }

        private CancellationTokenSource _cts;
        public CancellationTokenSource Cts
        {
            get { return _cts; }
            private set { _cts = value; }
        }

        //parametrized Action constructors
        public AsyncExtendedCommand ( Func<T, Task> executeFunction, bool canExecute = true )
        {
            this._executeFunction = executeFunction;
            this._canExecute = canExecute;
        }

        public AsyncExtendedCommand ( Func<T, CancellationToken, Task> cancelableExecuteFunction, bool canExecute = true )
        {
            this._cancelableExecuteFunction = cancelableExecuteFunction;
            this._canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged;

        bool ICommand.CanExecute ( object parameter )
        {
            return CanExecute;
        }

        void ICommand.Execute ( object parameter )
        {
            if(CanExecute)
            {
                var genericParameter = parameter;
                if(parameter != null && parameter.GetType() != typeof(T))
                {
                    try
                    {
                        genericParameter = Convert.ChangeType(parameter, typeof(T), null);
                    }
                    catch
                    {
                        genericParameter = default(T);
                    }
                }

                if(_executeFunction != null)
                    _executeFunction.Invoke((T)genericParameter);
                else if(_cancelableExecuteFunction != null)
                {
                    Cts = new CancellationTokenSource();

                    _cancelableExecuteFunction.Invoke((T)genericParameter, Cts.Token);
                }
                else
                {
                    throw new ArgumentNullException("Command must have execution function");
                }
            }
        }

        public void Execute ( object parameter )
        {
            (this as ICommand).Execute(parameter);
        }

        public void Cancel ()
        {
            if(Cts != null && !Cts.IsCancellationRequested)
            {
                Cts.Cancel();
            }
        }

        public RelayCommand CancelCommand { get { return new RelayCommand(Cancel); } }
    }

}
