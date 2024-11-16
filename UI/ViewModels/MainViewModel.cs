// ******************************************************************************************
//     Assembly:                Bobo
//     Author:                  Terry D. Eppler
//     Created:                 10-18-2024
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        10-18-2024
// ******************************************************************************************
// <copyright file="MainViewModel.cs" company="Terry D. Eppler">
//    A windows presentation foundation (WPF) application for communication
//    with the Chat GPT-3.5 Turbo API and Chat GPT-4
// 
//    Copyright ©  2020-2024 Terry D. Eppler
// 
//    Permission is hereby granted, free of charge, to any person obtaining a copy
//    of this software and associated documentation files (the “Software”),
//    to deal in the Software without restriction,
//    including without limitation the rights to use,
//    copy, modify, merge, publish, distribute, sublicense,
//    and/or sell copies of the Software,
//    and to permit persons to whom the Software is furnished to do so,
//    subject to the following conditions:
// 
//    The above copyright notice and this permission notice shall be included in all
//    copies or substantial portions of the Software.
// 
//    THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
//    INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NON-INFRINGEMENT.
//    IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
//    DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
//    DEALINGS IN THE SOFTWARE.
// 
//    You can contact me at:  terryeppler@gmail.com or eppler.terry@epa.gov
// </copyright>
// <summary>
//   MainViewModel.cs
// </summary>
// ******************************************************************************************

namespace Bobo
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using RestoreWindowPlace;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    [ SuppressMessage( "ReSharper", "ValueParameterNotUsed" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    [ SuppressMessage( "ReSharper", "ClassCanBeSealed.Global" ) ]
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// The application title
        /// </summary>
        private protected string _appTitle;

        /// <summary>
        /// The application version
        /// </summary>
        private protected Version _appVersion;

        /// <summary>
        /// The dot net version
        /// </summary>
        private protected Version _dotNetVersion;

        /// <summary>
        /// The live chat view model
        /// </summary>
        private protected ChatViewModel _chatViewModel;

        /// <summary>
        /// The history view model
        /// </summary>
        private protected HistoryViewModel _historyViewModel;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="MainViewModel"/> class.
        /// </summary>
        public MainViewModel( )
        {
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="MainViewModel"/> class.
        /// </summary>
        /// <param name="historyRepo">The history repo.</param>
        /// <param name="chatGptService">The chat GPT service.</param>
        public MainViewModel( IHistoryRepo historyRepo, ChatGptService chatGptService )
        {
            _historyViewModel = new HistoryViewModel( historyRepo );
            _chatViewModel = new ChatViewModel( historyRepo, chatGptService );
            _appVersion = Assembly.GetExecutingAssembly( ).GetName( ).Version;
            _dotNetVersion = Environment.Version;
            _appTitle = CreateVersion( );
        }

        /// <summary>
        /// Gets the application title.
        /// </summary>
        /// <value>
        /// The application title.
        /// </value>
        public string AppTitle
        {
            get
            {
                return _appTitle;
            }
            set
            {
                if( _appTitle != value )
                {
                    _appTitle = value;
                    OnPropertyChanged( nameof( AppTitle ) );
                }
            }
        }

        /// <summary>
        /// Gets or sets the application version.
        /// </summary>
        /// <value>
        /// The application version.
        /// </value>
        public Version AppVersion
        {
            get
            {
                return _appVersion;
            }
            set
            {
                if( _appVersion != value )
                {
                    _appVersion = value;
                    OnPropertyChanged( nameof( AppVersion ) );
                }
            }
        }

        /// <summary>
        /// Gets or sets the dot net version.
        /// </summary>
        /// <value>
        /// The dot net version.
        /// </value>
        public Version DotNetVersion
        {
            get
            {
                return _dotNetVersion;
            }
            set
            {
                if( _dotNetVersion != value )
                {
                    _dotNetVersion = value;
                    OnPropertyChanged( nameof( DotNetVersion ) );
                }
            }
        }

        // Bind to LiveChat tab item
        /// <summary>
        /// Gets the live chat view model.
        /// </summary>
        /// <value>
        /// The live chat view model.
        /// </value>
        public ChatViewModel LiveChatViewModel
        {
            get
            {
                return _chatViewModel;
            }
            set
            {
                if( _chatViewModel != value )
                {
                    _chatViewModel = value;
                    OnPropertyChanged( nameof( ChatViewModel ) );
                }
            }
        }

        // Bind to History tab item
        /// <summary>
        /// Gets the history view model.
        /// </summary>
        /// <value>
        /// The history view model.
        /// </value>
        public HistoryViewModel HistoryViewModel
        {
            get
            {
                return _historyViewModel;
            }
            set
            {
                if( _historyViewModel != value )
                {
                    _historyViewModel = value;
                    OnPropertyChanged( nameof( HistoryViewModel ) );
                }
            }
        }

        /// <summary>
        /// Creates the version.
        /// </summary>
        /// <returns></returns>
        private protected string CreateVersion( )
        {
            try
            {
                var _msg = $"{_appVersion.Major}.{_appVersion.Minor} (.NET {_dotNetVersion.Major}";
                var _suff = $".{_dotNetVersion.Minor}.{_dotNetVersion.Build} runtime) by Terry Eppler";
                return _msg + _suff;
            }
            catch( Exception ex )
            {
                Fail( ex );
                return string.Empty;
            }
        }

        /// <summary>
        /// Handles the exception.
        /// </summary>
        /// <param name="e">The e.</param>
        private void HandleException( Exception e )
        {
            if( e == null )
            {
            }
            else
            {
                Fail( e );
                Environment.Exit( 1 );
            }
        }

        /// <summary>
        /// Fails the specified ex.
        /// </summary>
        /// <param name="ex">The ex.</param>
        private static void Fail( Exception ex )
        {
            var _error = new ErrorWindow( ex );
            _error?.SetText( );
            _error?.ShowDialog( );
        }
    }
}