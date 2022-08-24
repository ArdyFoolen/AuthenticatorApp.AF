using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticatorApp.AF
{
    public sealed class CacheKeyAttribute : Attribute
    {
        /// <summary>
        /// Cache key parameter used as cache prefix
        /// </summary>
        public string KeyPrefix { get; set; }

        /// <summary>
        /// Cache version parameter used together with key and can be incremented to force cache refresh
        /// </summary>
        public string Version { get; set; }
    }
}
