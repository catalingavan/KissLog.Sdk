using System;

namespace KissLog.AspNetCore
{
    public interface IOptionsBuilder
    {
        LogListenersContainer Listeners { get; }
        Options Options { get; }
        Action<string> InternalLog { get; set; }
    }
}
