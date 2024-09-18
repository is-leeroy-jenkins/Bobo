

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
    public interface ITestProgress : IProgress
    {
        public bool IsEvaluated { get; set; }

        public double LogLoss { get; set; }

        public double LogLossReduction { get; set; }

        public double MicroAccuracy { get; set; }

        public double MacroAccuracy { get; set; }
    }
}
