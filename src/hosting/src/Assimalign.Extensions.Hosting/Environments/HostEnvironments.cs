using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.Hosting.Environments
{
    /// <summary>
    /// Commonly used environment names for development, testing, and deployment.
    /// </summary>
    public class HostEnvironments
    {
        /// <summary>
        /// 
        /// </summary>
        public const string Staging = "Staging";

        /// <summary>
        /// 
        /// </summary>
        public const string Development = "Development";

        /// <summary>
        /// 
        /// </summary>
        public const string QualityAssurance = "QualityAssurance";

        /// <summary>
        /// UAT (User Acceptance Testing Environment)
        /// </summary>
        public const string UserAcceptance = "UserAcceptance";

        /// <summary>
        /// 
        /// </summary>
        public const string Production = "Production";
    }
}
