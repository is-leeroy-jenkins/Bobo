
namespace Bobo
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    public class EngineSetting : INotifyPropertyChanged
    {
        private bool _useSdcaMaximumEntropy = true;
        private bool _useSdcaNonCalibrated;
        private bool _useLbfgsMaximumEntropy;
        private bool _useNaiveBayes;
        private bool _useImageClassification;
        private bool _useLightGmb;
        private EngineType _selectedEngineType = EngineType.SdcaMaximumEntropy;

        public EngineType SelectedEngineType
        {
            get
            {
                return _selectedEngineType;
            }
            set
            {
                if (value != _selectedEngineType)
                {
                    _selectedEngineType = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool UseSdcaMaximumEntropy
        {
            get
            {
                return _useSdcaMaximumEntropy;
            }
            set
            {
                if (value != _useSdcaMaximumEntropy)
                {
                    _useSdcaMaximumEntropy = value;
                    OnPropertyChanged();

                    if (value == true)
                    {
                        SelectedEngineType = EngineType.SdcaMaximumEntropy;
                    }
                }
            }
        }

        public bool UseSdcaNonCalibrated
        {
            get
            {
                return _useSdcaNonCalibrated;
            }
            set
            {
                if (value != _useSdcaNonCalibrated)
                {
                    _useSdcaNonCalibrated = value;
                    OnPropertyChanged();

                    if (value == true)
                    {
                        SelectedEngineType = EngineType.SdcaNonCalibrated;
                    }
                }
            }
        }

        public bool UseLbfgsMaximumEntropy
        {
            get
            {
                return _useLbfgsMaximumEntropy;
            }
            set
            {
                if (value != _useLbfgsMaximumEntropy)
                {
                    _useLbfgsMaximumEntropy = value;
                    OnPropertyChanged();

                    if (value == true)
                    {
                        SelectedEngineType = EngineType.LbfgsMaximumEntropy;
                    }
                }

            }
        }

        public bool UseNaiveBayes
        {
            get
            {
                return _useNaiveBayes;
            }
            set
            {
                if (value != _useNaiveBayes)
                {
                    _useNaiveBayes = value;
                    OnPropertyChanged();

                    if (value == true)
                    {
                        SelectedEngineType = EngineType.NaiveBayes;
                    }
                }
            }
        }

        public bool UseImageClassification
        {
            get
            {
                return _useImageClassification;
            }
            set
            {
                if (value != _useImageClassification)
                {
                    _useImageClassification = value;
                    OnPropertyChanged();

                    if (value == true)
                    {
                        SelectedEngineType = EngineType.ImageClassification;
                    }
                }
            }
        }

        public bool UseLightGmb
        {
            get
            {
                return _useLightGmb;
            }
            set
            {
                if (value != _useLightGmb)
                {
                    _useLightGmb = value;
                    OnPropertyChanged();

                    if (value == true)
                    {
                        SelectedEngineType = EngineType.LightGbm;
                    }
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( propertyName ) );
        }
    }
}
