

namespace Bobo
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Windows.Data;
    using System.Windows.Media;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <seealso cref="T:System.Windows.Data.IValueConverter" />
    [ SuppressMessage( "ReSharper", "UnusedType.Global" ) ]
    public class NameToFontConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is string _fontname
                ? new FontFamily( _fontname )
                : ( object )new FontFamily( "Arial" );
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is FontFamily _font
                ? _font.Source
                : ( object )"Arial";
        }
    }
}
