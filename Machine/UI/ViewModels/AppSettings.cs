
namespace Bobo
{
    using Newtonsoft.Json;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// 
    /// </summary>
    public class AppSettings
    {
        public const string SETTINGS_PATH = "settings.json";

        public string SelectedFont { get; set; }

        public FontSetting[] SelectedFonts { get; set; }

        public FontSetting FontSetting { get; set; }

        public EngineType SelectedEngine { get; set; }

        public static AppSettings ReadFromConfig()
        {
            return JsonConvert.DeserializeObject<AppSettings>( File.ReadAllText( SETTINGS_PATH ) );
        }

        public static void LoadSettings(TrainerViewModel viewmodel)
        {
            if (File.Exists(SETTINGS_PATH))
            {
                var _settings = ReadFromConfig();
                switch (_settings.SelectedEngine)
                {
                    case EngineType.SdcaNonCalibrated:
                        viewmodel.EngineSetting.UseSdcaNonCalibrated = true;
                        break;
                    case EngineType.LbfgsMaximumEntropy:
                        viewmodel.EngineSetting.UseLbfgsMaximumEntropy = true;
                        break;
                    case EngineType.NaiveBayes:
                        viewmodel.EngineSetting.UseNaiveBayes = true;
                        break;
                    case EngineType.ImageClassification:
                        viewmodel.EngineSetting.UseImageClassification = true;
                        break;
                    case EngineType.LightGbm:
                        viewmodel.EngineSetting.UseLightGmb = true;
                        break;
                    case EngineType.SdcaMaximumEntropy:
                    default:
                        viewmodel.EngineSetting.UseSdcaMaximumEntropy = true;
                        break;
                }

                viewmodel.TrainingFontSettingList.Clear();
                foreach (var _setting in _settings.SelectedFonts)
                {
                    viewmodel.TrainingFontSettingList.Add(_setting);
                }

                viewmodel.FontSetting.FontName = _settings.FontSetting.FontName;
                viewmodel.FontSetting.UseNormalFont = _settings.FontSetting.UseNormalFont;
                viewmodel.FontSetting.UseBoldFont = _settings.FontSetting.UseBoldFont;
                viewmodel.FontSetting.UseItalicFont = _settings.FontSetting.UseItalicFont;
                viewmodel.FontSetting.UseBoldItalicFont = _settings.FontSetting.UseBoldItalicFont;
                viewmodel.FontSetting.UseUpperCaseLetters = _settings.FontSetting.UseUpperCaseLetters;
                viewmodel.FontSetting.UseLowerCaseLetters = _settings.FontSetting.UseLowerCaseLetters;
                viewmodel.FontSetting.UseNumbers = _settings.FontSetting.UseNumbers;
                viewmodel.FontSetting.UseNormalFontRotation = _settings.FontSetting.UseNormalFontRotation;
                viewmodel.FontSetting.NormalFontMinRotation = _settings.FontSetting.NormalFontMinRotation;
                viewmodel.FontSetting.NormalFontMaxRotation = _settings.FontSetting.NormalFontMaxRotation;
                viewmodel.FontSetting.UseItalicFontRotation = _settings.FontSetting.UseItalicFontRotation;
                viewmodel.FontSetting.ItalicFontMinRotation = _settings.FontSetting.ItalicFontMinRotation;
                viewmodel.FontSetting.ItalicFontMaxRotation = _settings.FontSetting.ItalicFontMaxRotation;
                viewmodel.SelectedFontIndex = viewmodel.AvailableFonts.IndexOf(_settings.SelectedFont);
            }
        }

        public static void SaveSettings(TrainerViewModel viewmodel)
        {
            var _settings = new AppSettings
            {
                SelectedEngine = viewmodel.EngineSetting.SelectedEngineType,
                FontSetting = new FontSetting(viewmodel.FontSetting),
                SelectedFonts = viewmodel.TrainingFontSettingList.ToArray(),
                SelectedFont = viewmodel.SelectedFont
            };

            var _json = JsonConvert.SerializeObject(_settings, Formatting.Indented);
            File.WriteAllText(SETTINGS_PATH, _json);
        }
    }
}
