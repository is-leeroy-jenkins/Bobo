﻿// ******************************************************************************************
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
        /// The live chat view model
        /// </summary>
        private protected LiveChatViewModel _liveChatViewModel;

        /// <summary>
        /// The history view model
        /// </summary>
        private protected HistoryViewModel _historyViewModel;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="MainViewModel"/> class.
        /// </summary>
        /// <param name="historyRepo">The history repo.</param>
        /// <param name="chatGptService">The chat GPT service.</param>
        public MainViewModel( IHistoryRepo historyRepo, ChatGptService chatGptService )
        {
            _historyViewModel = new HistoryViewModel( historyRepo );
            _liveChatViewModel = new LiveChatViewModel( historyRepo, chatGptService );

            // <Version>1.3</Version> in .csproj
            var _appVer = Assembly.GetExecutingAssembly( ).GetName( ).Version!;
            var _dotnetVer = Environment.Version;
            _appTitle =
                $"C# WPF ChatGPT v{_appVer.Major}.{_appVer.Minor} (.NET {_dotnetVer.Major}.{_dotnetVer.Minor}.{_dotnetVer.Build} runtime) by Terry Eppler";
#if DEBUG
            AppTitle += " - DEBUG";
#endif
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

        // Bind to LiveChat tab item
        /// <summary>
        /// Gets the live chat view model.
        /// </summary>
        /// <value>
        /// The live chat view model.
        /// </value>
        public LiveChatViewModel LiveChatViewModel
        {
            get
            {
                return _liveChatViewModel;
            }
            set
            {
                if( _liveChatViewModel != value )
                {
                    _liveChatViewModel = value;
                    OnPropertyChanged( nameof( LiveChatViewModel ) );
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
    }
}