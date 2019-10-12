using System;
using System.Collections.Generic;

namespace KissLog.AspNetCore
{
    public interface IOptionsBuilder
    {
        List<ILogListener> Listeners { get; }
        Options Options { get; }
        Action<string, LogLevel> InternalLog { get; set; }
    }
}
