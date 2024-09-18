namespace Bobo
{
    public interface IFontSetting
    {
        public string FontName { get; }
        public bool UseBoldFont { get; }
        public bool UseBoldItalicFont { get; }
        public bool UseItalicFont { get; }
        public bool UseNormalFont { get; }
        public bool UseLowerCaseLetters { get; }
        public bool UseUpperCaseLetters { get; }
        public bool UseNumbers { get; }
        public bool UseNormalFontRotation { get; }
        public double NormalFontMinRotation { get; }
        public double NormalFontMaxRotation { get; }
        public bool UseItalicFontRotation { get; }
        public double ItalicFontMinRotation { get; }
        public double ItalicFontMaxRotation { get; }
    }
}
