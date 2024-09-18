

namespace Bobo
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.ML.Data;

    /// <summary>
    /// 
    /// </summary>
    [ SuppressMessage( "ReSharper", "ClassCanBeSealed.Global" ) ]
    public class CharacterPrediction
    {
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="CharacterPrediction"/> class.
        /// </summary>
        public CharacterPrediction( )
        {
        }

        /// <summary>
        /// Gets or sets the character.
        /// </summary>
        /// <value>
        /// The character.
        /// </value>
        [ColumnName( "PredictedLabel" ) ]
        public float Character { get; set; }

        /// <summary>
        /// Gets or sets the score.
        /// </summary>
        /// <value>
        /// The score.
        /// </value>
        [ColumnName( "Score" ) ]
        public float[ ] Score { get; set; }
    }
}
