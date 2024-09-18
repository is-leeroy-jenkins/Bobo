

namespace Bobo
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using static System.Windows.Forms.AxHost;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="IAccuracyProgress" />
    /// <seealso cref="System.Windows.Input.ICommand" />
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    public class CheckAccuracyCommand : IAccuracyProgress, ICommand, INotifyPropertyChanged
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CheckAccuracyCommand"/> class.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        public CheckAccuracyCommand(TrainerViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        private readonly TrainerViewModel _viewModel;

        private float _accuracy;

        private float _averageScore;

        private int _maximum;

        private int _current;

        private TestEvaluationState _state;

        private string _error;

        public float Accuracy
        {
            get
            {
                return _accuracy;
            }
            set
            {
                if (value != _accuracy)
                {
                    _accuracy = value;
                    OnPropertyChanged();
                }
            }
        }

        public float AverageScore
        {
            get
            {
                return _averageScore;
            }
            set
            {
                if (value != _averageScore)
                {
                    _averageScore = value;
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
                if (value != _maximum)
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
                if (value != _current)
                {
                    _current = value;
                    OnPropertyChanged();
                    if (_current >= _maximum)
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

        public bool CanExecute(object parameter)
        {
            return State is TestEvaluationState.Ideal or TestEvaluationState.Evaluated;
        }

        public async void Execute(object parameter)
        {
            var _trainer = new Trainer( );
            var _settings = new FontSetting( _viewModel.FontSetting );
            var _engine = _viewModel.EngineSetting.SelectedEngineType;
            try
            {
                State = TestEvaluationState.Loading;
                await Task.Run(() => _trainer.EvaluateAccuracy(_settings, _engine, this));
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
