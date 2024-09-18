namespace Bobo
{
    using System.Collections.Generic;

    public readonly record struct FontSetting(
        string FontName,
        bool UseNormalFont,
        bool UseBoldFont,
        bool UseItalicFont,
        bool UseBoldItalicFont,
        bool UseLowerCaseLetters,
        bool UseUpperCaseLetters,
        bool UseNumbers,
        bool UseNormalFontRotation,
        double NormalFontMinRotation,
        double NormalFontMaxRotation,
        bool UseItalicFontRotation,
        double ItalicFontMinRotation,
        double ItalicFontMaxRotation) : IFontSetting
    {
        public FontSetting(IFontSetting settings) : this(settings.FontName, settings.UseNormalFont, settings.UseBoldFont, settings.UseItalicFont, settings.UseBoldItalicFont, settings.UseLowerCaseLetters, settings.UseUpperCaseLetters, settings.UseNumbers, settings.UseNormalFontRotation, settings.NormalFontMinRotation, settings.NormalFontMaxRotation, settings.UseItalicFontRotation, settings.ItalicFontMinRotation, settings.ItalicFontMaxRotation) { }

        public const double ROTATION_STEP = 4.2d;

        public const string UPPER_CASE_LETTERS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public const string LOWER_CASE_LETTERS = "abcdefghijklmnopqrstuvwxyz";

        public const string NUMBERS = "0123456789\\/-_⟋⟍⧵⁄╱╲⬥.<>‹› ";

        public int CharCount
        {
            get
            {
                var _stylesCount = 0;
                if (UseNormalFont) _stylesCount++;
                if (UseBoldFont) _stylesCount++;
                if (UseItalicFont) _stylesCount++;
                if (UseBoldItalicFont) _stylesCount++;

                var _charCount = 0;

                if (UseLowerCaseLetters) _charCount += 26;
                if (UseUpperCaseLetters) _charCount += 26;
                if (UseNumbers) _charCount += 27;

                var _rotationsCount = 0;

                if (UseNormalFontRotation)
                {
                    _rotationsCount += (int)((NormalFontMaxRotation - NormalFontMinRotation) / ROTATION_STEP) + 1;
                }
                else
                {
                    _rotationsCount++;
                }

                if (UseItalicFontRotation)
                {
                    _rotationsCount += (int)((ItalicFontMaxRotation - ItalicFontMinRotation) / ROTATION_STEP) + 1;
                }
                else
                {
                    _rotationsCount++;
                }

                return _stylesCount * _charCount * _rotationsCount;
            }
        }

        public IEnumerable<Alphabet> GetAlphabets()
        {
            foreach (var _character in GetLetters())
            {
                foreach (var _rotation in GetNormalFontRotations())
                {
                    if (UseNormalFont == true)
                    {
                        yield return new Alphabet(FontName, _character, DrawStyle.Normal, _rotation);
                    }

                    if (UseBoldFont == true)
                    {
                        yield return new Alphabet(FontName, _character, DrawStyle.Bold, _rotation);
                    }
                }

                foreach (var _rotation in GetItalicFontRotations())
                {
                    if (UseItalicFont == true)
                    {
                        yield return new Alphabet(FontName, _character, DrawStyle.Italic, _rotation);
                    }

                    if (UseBoldItalicFont == true)
                    {
                        yield return new Alphabet(FontName, _character, DrawStyle.BoldItalic, _rotation);
                    }
                }
            }
        }

        private IEnumerable<double> GetNormalFontRotations()
        {
            if (UseNormalFontRotation)
            {
                var _offset = ((NormalFontMaxRotation - NormalFontMinRotation) % ROTATION_STEP) / 2d;
                for (var _rotation = NormalFontMinRotation; _rotation <= NormalFontMaxRotation; _rotation += ROTATION_STEP)
                {
                    yield return _rotation + _offset;
                }
            }
            else
            {
                yield return 0;
            }
        }

        private IEnumerable<double> GetItalicFontRotations()
        {
            if (UseItalicFontRotation)
            {
                var _offset = ((ItalicFontMaxRotation - ItalicFontMinRotation) % ROTATION_STEP) / 2d;
                for (var _rotation = ItalicFontMinRotation; _rotation <= ItalicFontMaxRotation; _rotation += ROTATION_STEP)
                {
                    yield return _rotation + _offset;
                }
            }
            else
            {
                yield return 0;
            }
        }

        private IEnumerable<char> GetLetters()
        {
            if (UseLowerCaseLetters)
            {
                foreach (var _letter in UPPER_CASE_LETTERS)
                {
                    yield return _letter;
                }
            }

            if (UseUpperCaseLetters)
            {
                foreach (var _letter in LOWER_CASE_LETTERS)
                {
                    yield return _letter;
                }
            }

            if (UseNumbers)
            {
                foreach (var _number in NUMBERS)
                {
                    yield return _number;
                }
            }
        }
    }
}