
namespace Bobo
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Input;

    public class TrainingModelCommand : ITrainingProgress, ICommand, INotifyPropertyChanged
    {
        public TrainingModelCommand(TrainerViewModel viewmodel)
        {
            _viewmodel = viewmodel;
        }

        private int _maximum = 100;
        private int _current;
        private bool _isRunning;
        private EngineType _engine;

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
                    OnPropertyChanged(nameof(IsLoading));
                    OnPropertyChanged(nameof(IsTraining));
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
                    OnPropertyChanged(nameof(IsLoading));
                    OnPropertyChanged(nameof(IsTraining));
                }
            }
        }

        public bool IsRunning
        {
            get
            {
                return _isRunning;
            }
            set
            {
                if (_isRunning != value)
                {
                    _isRunning = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsLoading));
                    OnPropertyChanged(nameof(IsTraining));
                }
            }
        }

        public EngineType Engine
        {
            get
            {
                return _engine;
            }
            set
            {
                if (_engine != value)
                {
                    _engine = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsLoading
        {
            get
            {
                return IsRunning && _current < _maximum;
            }
        }

        public bool IsTraining
        {
            get
            {
                return IsRunning && _current == _maximum;
            }
        }

        private readonly TrainerViewModel _viewmodel;

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( propertyName ) );
        }

        public event EventHandler CanExecuteChanged;
        public bool CanExecute(object parameter)
        {
            return IsRunning == false;
        }

        public async void Execute(object parameter)
        {

            var _trainer = new Trainer( );
            IEnumerable<FontSetting> _settings = _viewmodel.TrainingFontSettingList.ToArray();
            Engine = _viewmodel.EngineSetting.SelectedEngineType;
            try
            {
                IsRunning = true;
                if (_settings.Sum(x => x.CharCount) < 2)
                {
                    MessageBox.Show($"Empty character data");
                    return;
                }
                CanExecuteChanged?.Invoke(this, new EventArgs());
                await Task.Run(() => _trainer.Train(_settings, _engine, this));
            }
            catch (NotImplementedException)
            {
                MessageBox.Show($"Engine mode {_engine} is not supported.");
            }
            finally
            {
                Current = 0;
                IsRunning = false;
                CanExecuteChanged?.Invoke(this, new EventArgs());
            }
        }
    }
}
