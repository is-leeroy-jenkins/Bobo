
namespace Bobo
{
    using Microsoft.ML.Data;

    /// <summary>
    /// 
    /// </summary>
    public class PredictionData
    {
        /// <summary>
        /// Gets or sets the character identifier.
        /// </summary>
        /// <value>
        /// The character identifier.
        /// </value>
        [ColumnName("PredictedLabel")]
        public int CharacterId { get; set; }

        /// <summary>
        /// Gets or sets the score.
        /// </summary>
        /// <value>
        /// The score.
        /// </value>
        [ColumnName("Score")]
        public float[] Score { get; set; }
    }
}