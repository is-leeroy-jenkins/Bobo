namespace Bobo
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;
    using System.Windows.Input;

    public enum TestEvaluationState { Ideal, Loading, Evaluating, Evaluated, Error }

    public class EvaluateModelCommand : ITestProgress, ICommand, INotifyPropertyChanged
    {
        public EvaluateModelCommand(TrainerViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        private readonly TrainerViewModel _viewModel;

        private bool _isEvaluated;

        private double _logLoss;

        private double _logLossReduction;

        private double _microAccuracy;

        private double _macroAccuracy;

        private int _maximum;

        private int _current;

        private TestEvaluationState _state;

        private string _error;

        public bool IsEvaluated
        {
            get
            {
                return _isEvaluated;
            }
            set
            {
                if (_isEvaluated != value)
                {
                    _isEvaluated = value;
                    OnPropertyChanged();
                    State = TestEvaluationState.Evaluated;
                }
            }
        }

        public double LogLoss
        {
            get
            {
                return _logLoss;
            }
            set
            {
                if (_logLoss != value)
                {
                    _logLoss = value;
                    OnPropertyChanged();
                }
            }
        }

        public double LogLossReduction
        {
            get
            {
                return _logLossReduction;
            }
            set
            {
                if (_logLossReduction != value)
                {
                    _logLossReduction = value;
                    OnPropertyChanged();
                }
            }
        }

        public double MicroAccuracy
        {
            get
            {
                return _microAccuracy;
            }
            set
            {
                if (_microAccuracy != value)
                {
                    _microAccuracy = value;
                    OnPropertyChanged();
                }
            }
        }

        public double MacroAccuracy
        {
            get
            {
                return _macroAccuracy;
            }
            set
            {
                if (_macroAccuracy != value)
                {
                    _macroAccuracy = value;
                    OnPropertyChanged();
                }
            }
        }

        public int Maximum
        {
            get
            {
                return _maximum;
            }
            set
            {
                if (_maximum != value)
                {
                    _maximum = value;
                    OnPropertyChanged();
                }
            }
        }

        public int Current
        {
            get
            {
                return _current;
            }
            set
            {
                if (_current != value)
                {
                    _current = value;
                    OnPropertyChanged();
                    if (_current == _maximum)
                    {
                        State = TestEvaluationState.Evaluating;
                    }
                }
            }
        }

        public TestEvaluationState State
        {
            get
            {
                return _state;
            }
            set
            {
                if (_state != value)
                {
                    _state = value;
                    OnPropertyChanged();
                    CanExecuteChanged?.Invoke(this, new EventArgs());
                }
            }
        }

        public string Error
        {
            get
            {
                return _error;
            }
            set
            {
                if (_error == value)
                {
                    _error = value;
                    OnPropertyChanged();
                }
            }
        }

        public event EventHandler CanExecuteChanged;
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( propertyName ) );
        }

        public bool CanExecute(object _)
        {
            return State is TestEvaluationState.Ideal or TestEvaluationState.Evaluated;
        }

        public async void Execute(object _)
        {

            var _trainer = new Trainer( );
            var _settings = new FontSetting( _viewModel.FontSetting );
            var _engine = _viewModel.EngineSetting.SelectedEngineType;
            try
            {
                State = TestEvaluationState.Loading;
                await Task.Run(() => _trainer.Evaluate(_settings, _engine, this));
                State = TestEvaluationState.Evaluated;
            }
            catch (NotImplementedException)
            {
                Error = $"Engine mode {_engine} is not supported";
                State = TestEvaluationState.Error;
            }
            catch (Exception)
            {
                Error = $"Unable to evaluate model, make sure if model is avalable for {_engine}";
                State = TestEvaluationState.Error;
            }
            finally
            {
                if (State is not TestEvaluationState.Ideal && State is not TestEvaluationState.Evaluated)
                {
                    await Task.Delay(5000);
                    State = TestEvaluationState.Ideal;
                }
            }
        }
    }
}
