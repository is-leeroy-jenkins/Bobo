﻿// ******************************************************************************************
//     Assembly:                Bobo
//     Author:                  Terry D. Eppler
//     Created:                 10-16-2024
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        10-16-2024
// ******************************************************************************************
// <copyright file="Message.cs" company="Terry D. Eppler">
//    A windows presentation foundation (WPF) app to communicate with the Chat GPT-3.5 Turbo API
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
//   Message.cs
// </summary>
// ******************************************************************************************

namespace Bobo
{
    using System.Diagnostics.CodeAnalysis;
    using System.Windows;
    using CommunityToolkit.Mvvm.ComponentModel;

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    ///  <seealso cref="T:CommunityToolkit.Mvvm.ComponentModel.ObservableObject" />
    [ SuppressMessage( "ReSharper", "ClassCanBeSealed.Global" ) ]
    public partial class Message : ObservableObject
    {
        /// <summary>
        /// Gets or sets the sender.
        /// </summary>
        /// <value>
        /// The sender.
        /// </value>
        public string Sender { get; set; }

        // For appending text in Streaming Mode
        /// <summary>
        /// The text
        /// </summary>
        [ ObservableProperty ]
        private string _text = string.Empty;

        /// <summary>
        /// Gets or sets the copy button visibility.
        /// </summary>
        /// <value>
        /// The copy button visibility.
        /// </value>
        public Visibility CopyButtonVisibility { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Message"/> class.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="text">The text.</param>
        public Message( string sender, string text )
        {
            Sender = sender;
            Text = text;
            CopyButtonVisibility = Sender == "Bot"
                ? Visibility.Visible
                : Visibility.Collapsed;
        }
    }
}