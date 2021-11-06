using System;
using System.Collections.Generic;

namespace KissLog.AspNetCore
{
    internal class KissLogScope<TState> : IDisposable
    {
        private readonly Logger _logger;
        private readonly LoggerOptions _options;
        private readonly Dictionary<string, object> _scopeData;

        public KissLogScope(TState state, Logger logger, LoggerOptions options)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _scopeData = new Dictionary<string, object>();

            _options.OnBeginScope?.Invoke(new BeginScopeArgs(state, logger, _scopeData));
        }


        public void Dispose()
        {
            _options.OnEndScope?.Invoke(new EndScopeArgs(_logger, _scopeData));
        }
    }
}
