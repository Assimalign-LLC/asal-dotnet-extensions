using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.Hosting.Environments
{
    /// <summary>
    /// Commonly used environment names.
    /// </summary>
    public class HostEnvironments
    {
        public const string Staging = "Staging";
        public const string Development = "Development";
        /// <summary>
        /// UAT (User Acceptance Testing Environment)
        /// </summary>
        public const string UserAcceptance = "UserAcceptance";
        public const string QualityAssurance = "QualityAssurance";
        public const string Production = "Production";
    }
}
