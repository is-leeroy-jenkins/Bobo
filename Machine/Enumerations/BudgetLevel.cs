﻿// ******************************************************************************************
//     Assembly:                Bocefus
//     Author:                  Terry D. Eppler
//     Created:                 07-28-2024
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        07-28-2024
// ******************************************************************************************
// <copyright file="BudgetLevel.cs" company="Terry D. Eppler">
//    Bocefus is data analysis and reporting tool for EPA Analysts.
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
//   BudgetLevel.cs
// </summary>
// ******************************************************************************************

namespace Bobo
{
    using System;

    /// <summary>
    /// </summary>
    public enum BudgetLevel
    {
        /// <summary>
        /// The treasury
        /// </summary>
        Treasury = 0,

        /// <summary>
        /// The appropriation
        /// </summary>
        Appropriation = 1,

        /// <summary>
        /// The apportionment
        /// </summary>
        Apportionment = 2,

        /// <summary>
        /// The rpio
        /// </summary>
        RPIO = 3,

        /// <summary>
        /// The allowance holder
        /// </summary>
        AllowanceHolder = 4,

        /// <summary>
        /// The program area
        /// </summary>
        ProgramArea = 5,

        /// <summary>
        /// The program results code
        /// </summary>
        ProgramResultsCode = 6,

        /// <summary>
        /// The budget object class
        /// </summary>
        BudgetObjectClass = 7,

        /// <summary>
        /// The sub allocation
        /// </summary>
        SubAllocation = 8
    }
}