using System.Collections.Generic;

namespace KissLog.AspNetCore
{
    public interface IOptionsBuilder
    {
        List<ILogListener> Listeners { get; }
        Options Options { get; }
    }
}
