﻿// ******************************************************************************************
//     Assembly:                Bobo
//     Author:                  Terry D. Eppler
//     Created:                 10-16-2024
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        10-16-2024
// ******************************************************************************************
// <copyright file="None.cs" company="Terry D. Eppler">
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
//   None.cs
// </summary>
// ******************************************************************************************

namespace Bobo
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    /// <inheritdoc/>
    /// <summary> </summary>
    /// <typeparam name="_"> </typeparam>
    /// <seealso cref="T:Bobo.IOption`1"/>
    /// <seealso cref="T:Bobo.IOption`1"/>
    [ SuppressMessage( "ReSharper", "MemberCanBeInternal" ) ]
    [ SuppressMessage( "ReSharper", "ClassCanBeSealed.Global" ) ]
    [ SuppressMessage( "ReSharper", "UnassignedGetOnlyAutoProperty" ) ]
    [ SuppressMessage( "ReSharper", "UnusedType.Global" ) ]
    public class None<_> : Option<_>
    {
        /// <inheritdoc/>
        /// <summary>
        /// Maps the specified function.
        /// </summary>
        /// <typeparam name="_result"> The type of the result. </typeparam>
        /// <param name="func"> The function. </param>
        /// <returns> </returns>
        public override Option<_result> Map<_result>( Func<_, _result> func )
        {
            try
            {
                return new None<_result>( );
            }
            catch( Exception ex )
            {
                Fail( ex );
                return default( Option<_result> );
            }
        }

        /// <inheritdoc/>
        /// <summary> Matches the specified some function. </summary>
        /// <typeparam name="_result"> The type of the result. </typeparam>
        /// <param name="someFunc"> Some function. </param>
        /// <param name="noneFunc"> The none function. </param>
        /// <returns> </returns>
        public override _result Match<_result>( Func<_, _result> someFunc, Func<_result> noneFunc )
        {
            try
            {
                return noneFunc( );
            }
            catch( Exception ex )
            {
                Fail( ex );
                return default( _result );
            }
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="None{T}"/>
        /// class.
        /// </summary>
        public None( )
        {
        }

        /// <inheritdoc/>
        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value> The value. </value>
        public override _ Value { get; }

        /// <summary> Gets the default. </summary>
        /// <value> The default. </value>
        public static _ Default { get; }

        /// <inheritdoc/>
        /// <summary>
        /// Gets a value indicating whether this instance is some.
        /// </summary>
        /// <value>
        /// <c> true </c>
        /// if this instance is some; otherwise,
        /// <c> false </c>
        /// .
        /// </value>
        public override bool IsSome
        {
            get { return false; }
        }

        /// <inheritdoc/>
        /// <summary>
        /// Gets a value indicating whether this instance is none.
        /// </summary>
        /// <value>
        /// <c> true </c>
        /// if this instance is none; otherwise,
        /// <c> false </c>
        /// .
        /// </value>
        public override bool IsNone
        {
            get { return true; }
        }
    }
}