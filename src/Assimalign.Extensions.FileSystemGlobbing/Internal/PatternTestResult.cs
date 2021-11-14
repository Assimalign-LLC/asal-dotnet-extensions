using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.FileSystemGlobbing.Internal
{
    /// <summary>
    /// This API supports infrastructure and is not intended to be used
    /// directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public struct PatternTestResult
    {
        public static readonly PatternTestResult Failed = new PatternTestResult(isSuccessful: false, stem: null);

        public bool IsSuccessful { get; }
        public string Stem { get; }

        private PatternTestResult(bool isSuccessful, string stem)
        {
            IsSuccessful = isSuccessful;
            Stem = stem;
        }

        public static PatternTestResult Success(string stem)
        {
            return new PatternTestResult(isSuccessful: true, stem: stem);
        }
    }
}
