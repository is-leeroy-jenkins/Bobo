

namespace Bobo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using OpenCvSharp;
    using System.Drawing;
    using Tensorflow;
    using Microsoft.ML.Data;

    /// <summary>
    /// 
    /// </summary>
    public class CharacterOpticalData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterOpticalData"/> class.
        /// </summary>
        public CharacterOpticalData( )
        {
        }

        /// <summary>
        /// Gets or sets the character identifier.
        /// </summary>
        /// <value>
        /// The character identifier.
        /// </value>
        [LoadColumn(0)]
        public int CharacterId { get; set; }

        /// <summary>
        /// Gets or sets the pixels.
        /// </summary>
        /// <value>
        /// The pixels.
        /// </value>
        public byte[] Pixels { get; set; }
    }
}
