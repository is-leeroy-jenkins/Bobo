﻿// ******************************************************************************************
//     Assembly:                Bobo
//     Author:                  Terry D. Eppler
//     Created:                 10-16-2024
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        10-16-2024
// ******************************************************************************************
// <copyright file="ExtensionHelper.cs" company="Terry D. Eppler">
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
//   ExtensionHelper.cs
// </summary>
// ******************************************************************************************

namespace Bobo
{
    using System.Collections;

    public static class ExtensionHelper
    {
        public static bool IsBlank( this string str )
        {
            return string.IsNullOrEmpty( str );
        }

        public static bool IsNotBlank( this string str )
        {
            return !IsBlank( str );
        }

        public static bool IsEmpty( this ICollection c )
        {
            return c == null || c.Count == 0;
        }

        public static bool IsNotEmpty( this ICollection c )
        {
            return !IsEmpty( c );
        }

        public static bool In<T>( this T item, params T[ ] testValues )
        {
            if( item != null )
            {
                foreach( var test_value in testValues )
                {
                    if( test_value != null
                        && test_value.Equals( item ) )
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public static bool NotIn<T>( this T item, params T[ ] testValues )
        {
            return !In( item, testValues );
        }
    }
}