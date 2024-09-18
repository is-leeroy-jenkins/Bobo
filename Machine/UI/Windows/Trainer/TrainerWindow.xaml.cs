
namespace Bobo
{
    using System.Windows;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <seealso cref="T:System.Windows.Window" />
    /// <seealso cref="T:System.Windows.Markup.IComponentConnector" />
    [ SuppressMessage( "ReSharper", "ClassCanBeSealed.Global" ) ]
    public partial class TrainerWindow : Window
    {
        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Bobo.TrainerWindow" /> class.
        /// </summary>
        public TrainerWindow( )
        {
            InitializeComponent( );
        }

        /// <inheritdoc />
        /// <summary>
        /// Raises the
        /// <see cref="E:System.Windows.Window.Closing" /> event.
        /// </summary>
        /// <param name="e">A
        /// <see cref="T:System.ComponentModel.CancelEventArgs" />
        /// hat contains the event data.</param>
        protected override void OnClosing( CancelEventArgs e )
        {
            if(DataContext is TrainerViewModel _trainer)
            {
                AppSettings.SaveSettings( _trainer );
            }

            base.OnClosing( e );
        }
    }
}
