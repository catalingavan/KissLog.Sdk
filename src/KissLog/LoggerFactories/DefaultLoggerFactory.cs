using System.Collections.Generic;
using System.Linq;

namespace KissLog.LoggerFactories
{
    internal class DefaultLoggerFactory : ILoggerFactory
    {
        public Logger Get(string categoryName = null, string url = null)
        {
            return new Logger(categoryName: categoryName, url: url);
        }

        public IEnumerable<Logger> GetAll()
        {
            return Enumerable.Empty<Logger>();
        }
    }
}
