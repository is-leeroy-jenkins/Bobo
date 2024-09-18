

namespace Bobo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="IProgress" />
    public interface IAccuracyProgress : IProgress
    {
        /// <summary>
        /// Gets or sets the accuracy.
        /// </summary>
        /// <value>
        /// The accuracy.
        /// </value>
        public float Accuracy { get; set; }

        /// <summary>
        /// Gets or sets the average score.
        /// </summary>
        /// <value>
        /// The average score.
        /// </value>
        public float AverageScore { get; set; }
    }
}
