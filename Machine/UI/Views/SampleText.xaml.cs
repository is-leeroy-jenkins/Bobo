

namespace Bobo
{
    using System.Windows;
    using System.Windows.Controls;

    public partial class SampleText : UserControl
    {
        public SampleText()
        {
            InitializeComponent( );
        }

        public static readonly DependencyProperty ShowLowerCaseProperty =  DependencyProperty.Register(nameof(ShowLowerCase), typeof(bool), typeof(SampleText), new PropertyMetadata(true));
        public static readonly DependencyProperty ShowUpperCaseProperty = DependencyProperty.Register(nameof(ShowUpperCase), typeof(bool), typeof(SampleText), new PropertyMetadata(true));
        public static readonly DependencyProperty ShowNumericsProperty = DependencyProperty.Register(nameof(ShowNumerics), typeof(bool), typeof(SampleText), new PropertyMetadata(true));
        public bool ShowLowerCase
        {
            get
            {
                return ( bool )GetValue( ShowLowerCaseProperty );
            }
            set
            {
                SetValue( ShowLowerCaseProperty, value );
            }
        }

        public bool ShowUpperCase
        {
            get
            {
                return ( bool )GetValue( ShowUpperCaseProperty );
            }
            set
            {
                SetValue( ShowUpperCaseProperty, value );
            }
        }

        public bool ShowNumerics
        {
            get
            {
                return ( bool )GetValue( ShowNumericsProperty );
            }
            set
            {
                SetValue( ShowNumericsProperty, value );
            }
        }
    }
}