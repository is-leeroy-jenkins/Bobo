namespace Bobo
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    public class FontSettingViewModel : INotifyPropertyChanged, IFontSetting
    {
        private bool _useNormalFont = true;
        private bool _useBoldFont = true;
        private bool _useItalicFont = true;
        private bool _useBoldItalicFont = true;
        private string _fontName = "Arial";
        private bool _useLowerCaseLetters = true;
        private bool _useUpperCaseLetters = true;
        private bool _useNumbers = true;
        private bool _useNormalFontRotation = true;
        private double _normalFontMinRotation = -10d;
        private double _normalFontMaxRotation = 10d;
        private bool _useItalicFontRotation = true;
        private double _italicFontMinRotation = -15;
        private double _italicFontMaxRotation = 5;

        public string FontName
        {
            get
            {
                return _fontName;
            }
            set
            {
                if (value != _fontName)
                {
                    _fontName = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool UseNormalFont
        {
            get
            {
                return _useNormalFont;
            }
            set
            {
                if (value != _useNormalFont)
                {
                    _useNormalFont = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool UseBoldFont
        {
            get
            {
                return _useBoldFont;
            }
            set
            {
                if (value != _useBoldFont)
                {
                    _useBoldFont = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool UseItalicFont
        {
            get
            {
                return _useItalicFont;
            }
            set
            {
                if (value != _useItalicFont)
                {
                    _useItalicFont = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool UseBoldItalicFont
        {
            get
            {
                return _useBoldItalicFont;
            }
            set
            {
                if (value != _useBoldItalicFont)
                {
                    _useBoldItalicFont = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool UseLowerCaseLetters
        {
            get
            {
                return _useLowerCaseLetters;
            }
            set
            {
                if (value != _useLowerCaseLetters)
                {
                    _useLowerCaseLetters = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool UseUpperCaseLetters
        {
            get
            {
                return _useUpperCaseLetters;
            }
            set
            {
                if (value != _useUpperCaseLetters)
                {
                    _useUpperCaseLetters = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool UseNumbers
        {
            get
            {
                return _useNumbers;
            }
            set
            {
                if (value != _useNumbers)
                {
                    _useNumbers = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool UseNormalFontRotation
        {
            get
            {
                return _useNormalFontRotation;
            }
            set
            {
                if (value != _useNormalFontRotation)
                {
                    _useNormalFontRotation = value;
                    OnPropertyChanged();
                }
            }
        }

        public double NormalFontMinRotation
        {
            get
            {
                return _normalFontMinRotation;
            }
            set
            {
                if (value != _normalFontMinRotation)
                {
                    _normalFontMinRotation = value;
                    OnPropertyChanged();
                }
            }
        }

        public double NormalFontMaxRotation
        {
            get
            {
                return _normalFontMaxRotation;
            }
            set
            {
                if (value != _normalFontMaxRotation)
                {
                    _normalFontMaxRotation = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool UseItalicFontRotation
        {
            get
            {
                return _useItalicFontRotation;
            }
            set
            {
                if (value != _useItalicFontRotation)
                {
                    _useItalicFontRotation = value;
                    OnPropertyChanged();
                }
            }
        }

        public double ItalicFontMinRotation
        {
            get
            {
                return _italicFontMinRotation;
            }
            set
            {
                if (value != _italicFontMinRotation)
                {
                    _italicFontMinRotation = value;
                    OnPropertyChanged();
                }
            }
        }

        public double ItalicFontMaxRotation
        {
            get
            {
                return _italicFontMaxRotation;
            }
            set 
            {
                if (value != _italicFontMaxRotation)
                {
                    _italicFontMaxRotation = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( propertyName ) );
        }
    }
}
