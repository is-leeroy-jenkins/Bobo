﻿// ******************************************************************************************
//     Assembly:                Bocefus
//     Author:                  Terry D. Eppler
//     Created:                 08-01-2024
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        08-01-2024
// ******************************************************************************************
// <copyright file="MetroTreeGrid.cs" company="Terry D. Eppler">
//    Bocefus is data analysis and reporting tool for EPA Analysts
//    based on WPF, NET6.0, and written in C-Sharp.
// 
//    Copyright ©  2024  Terry D. Eppler
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
//    You can contact me at: terryeppler@gmail.com or eppler.terry@epa.gov
// </copyright>
// <summary>
//   MetroTreeGrid.cs
// </summary>
// ******************************************************************************************

namespace Bobo
{
    using Syncfusion.UI.Xaml.TreeGrid;
    using System;
    using System.Diagnostics.CodeAnalysis;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <seealso cref="T:Syncfusion.UI.Xaml.TreeGrid.SfTreeGrid" />
    [ SuppressMessage( "ReSharper", "UnusedType.Global" ) ]
    [ SuppressMessage( "ReSharper", "InconsistentNaming" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    public class MetroTreeGrid : SfTreeGrid
    {
        /// <summary>
        /// The theme
        /// </summary>
        private protected DarkMode _theme = new DarkMode( );

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:Bocefus.UI.Controls.MetroTreeGrid" /> class.
        /// </summary>
        public MetroTreeGrid( )
            : base( )
        {
            // Control Properties
            SetResourceReference( MetroTreeGrid.StyleProperty, typeof( SfTreeGrid ) );
            Width = 400;
            Height = 400;
            FontSize = _theme.FontSize;
            FontFamily = _theme.FontFamily;
            Background = _theme.DarkBlueBrush;
            Foreground = _theme.Foreground;
            BorderBrush = _theme.BorderBrush;
        }

        /// <summary>
        /// Fails the specified _ex.
        /// </summary>
        /// <param name="_ex">The _ex.</param>
        private protected void Fail( Exception _ex )
        {
            var _error = new ErrorWindow( _ex );
            _error?.SetText( );
            _error?.ShowDialog( );
        }
    }
}