

namespace Bobo
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Threading;
    using Microsoft.ML;
    using Microsoft.ML.Data;
    using OpenCvSharp;

    /// <summary>
    /// 
    /// </summary>
    [ SuppressMessage( "ReSharper", "ClassCanBeSealed.Global" ) ]
    public class OpticalCharacterData
    {
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="OpticalCharacterData"/> class.
        /// </summary>
        public OpticalCharacterData( )
        {
        }

        /// <summary>
        /// Gets or sets the character.
        /// </summary>
        /// <value>
        /// The character.
        /// </value>
        [LoadColumn(0)]
        public float Character { get; set; }

        /// <summary>
        /// Gets or sets the pixel values.
        /// </summary>
        /// <value>
        /// The pixel values.
        /// </value>
        public byte[] PixelValues { get; set; }
    }
}
