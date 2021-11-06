using System;
using System.Collections.Generic;

namespace KissLog.AspNetCore
{
    public class BeginScopeArgs
    {
        public object State { get; }
        public Logger Logger { get; }
        public IDictionary<string, object> ScopeData { get; }

        public BeginScopeArgs(object state, Logger logger, Dictionary<string, object> scopeData)
        {
            State = state;
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            ScopeData = scopeData ?? throw new ArgumentNullException(nameof(scopeData));
        }
    }
}
