using System.Collections.Generic;

namespace KissLog
{
    public interface IKissLoggerFactory
    {
        ILogger Get(string categoryName = null, string url = null);

        IEnumerable<ILogger> GetAll();
    }
}
