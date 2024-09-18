/*
 MIT License

Copyright (c) 2023 Mahendra Goyal

Permission is hereby granted, free of charge, to any person obtaining 
a copy of this software and associated documentation files (the "Software"), 
to deal in the Software without restriction, including without limitation 
the rights to use, copy, modify, merge, publish, distribute, sublicense, 
and/or sell copies of the Software, and to permit persons to whom the Software 
is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included 
in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR 
PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE 
LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, 
TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR 
OTHER DEALINGS IN THE SOFTWARE.
 */

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
