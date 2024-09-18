namespace Bobo
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Windows.Media;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    public class TrainerViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="TrainerViewModel"/> class.
        /// </summary>
        public TrainerViewModel()
        {
            AvailableFonts = GetFontNames().OrderBy(x => x).ToList();
            FontSetting = new FontSettingViewModel();
            SelectedFontIndex = 0;
            ChangeFontCommand = new ChangeFontCommand(this);
            TrainingFontSettingList = new ObservableCollection<FontSetting>();
            AddFontSettingCommand = new AddFontSettingCommand(this);
            RemoveFontSettingCommand = new RemoveFontSettingsCommand(this);
            EngineSetting = new EngineSetting();
            TrainingModelCommand = new TrainingModelCommand(this);
            EvaluateModelCommand = new EvaluateModelCommand(this);
            CheckAccuracyCommand = new CheckAccuracyCommand(this);
            ClearFontSettingsCommand = new ClearFontSettingsCommand(this);
            SaveSettingsCommand = new SaveSettingsCommand(this);
            AppSettings.LoadSettings(this);
        }

        private int _selectedFontIndex = -1;

        public List<string> AvailableFonts { get; }

        public EngineSetting EngineSetting { get; }

        public FontSettingViewModel FontSetting { get; }

        public AddFontSettingCommand AddFontSettingCommand { get; }

        public RemoveFontSettingsCommand RemoveFontSettingCommand { get; }

        public ObservableCollection<FontSetting> TrainingFontSettingList { get; }

        public ChangeFontCommand ChangeFontCommand { get; }

        public TrainingModelCommand TrainingModelCommand { get; }

        public EvaluateModelCommand EvaluateModelCommand { get; }

        public CheckAccuracyCommand CheckAccuracyCommand { get; }

        public ClearFontSettingsCommand ClearFontSettingsCommand { get; }

        public SaveSettingsCommand SaveSettingsCommand { get; }

        public string SelectedFont
        {
            get
            {
                return AvailableFonts[ SelectedFontIndex ];
            }
        }

        public int SelectedFontIndex
        {
            get
            {
                return _selectedFontIndex;
            }
            set
            {
                if (value == _selectedFontIndex) return;

                _selectedFontIndex = value switch
                {
                    int _n when (_n >= AvailableFonts.Count) => 0,
                    int _n when (_n < 0) => AvailableFonts.Count - 1,
                    _ => value,
                };

                FontSetting.FontName = SelectedFont;
                OnPropertyChanged();
                OnPropertyChanged(nameof(SelectedFont));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( propertyName ) );
        }

        public static IEnumerable<string> GetFontNames(string culture = "en-us")
        {
            foreach (var _fontfamily in Fonts.SystemFontFamilies)
            {
                foreach (var _font in _fontfamily.FamilyNames
                    .Where(x => x.Key.ToString().Trim().ToLower() == culture)
                    .Select(x => x.Value).Distinct())
                {
                    yield return _font;
                }
            }
        }
    }
}
