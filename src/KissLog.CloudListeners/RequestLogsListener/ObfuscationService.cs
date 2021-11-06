using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace KissLog.CloudListeners.RequestLogsListener
{
    public class ObfuscationService : IObfuscationService
    {
        public static readonly List<string> DefaultPatterns = new List<string> { "(?si)pass" };
        
        private readonly List<string> _patterns;
        public ObfuscationService() : this(DefaultPatterns)
        {
            
        }

        public ObfuscationService(IEnumerable<string> patterns)
        {
            if (patterns == null)
                throw new ArgumentNullException(nameof(patterns));

            _patterns = patterns.ToList();
        }

        public bool ShouldObfuscate(string key, string value, string propertyName)
        {
            if (string.IsNullOrWhiteSpace(key))
                return false;

            foreach(string pattern in _patterns)
            {
                if (string.IsNullOrWhiteSpace(pattern))
                    continue;

                if (Regex.IsMatch(key, pattern, RegexOptions.Compiled))
                    return true;
            }

            return false;
        }

        internal List<string> GetPatterns()
        {
            return _patterns;
        }
    }
}
