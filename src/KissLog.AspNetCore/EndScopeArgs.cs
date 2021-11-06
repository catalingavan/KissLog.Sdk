using System;
using System.Collections.Generic;

namespace KissLog.AspNetCore
{
    public class EndScopeArgs
    {
        public Logger Logger { get; }
        public IDictionary<string, object> ScopeData { get; }

        public EndScopeArgs(Logger logger, Dictionary<string, object> scopeData)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            ScopeData = scopeData ?? throw new ArgumentNullException(nameof(scopeData));
        }
    }
}
