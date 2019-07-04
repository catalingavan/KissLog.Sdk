using System.Collections.Generic;
using System.Linq;

namespace KissLog.Internal
{
    public class DefaultLoggerFactory : IKissLoggerFactory
    {
        public IEnumerable<ILogger> GetAll()
        {
            return Enumerable.Empty<ILogger>();
        }

        public ILogger Get(string categoryName = null, string url = null)
        {
            return new Logger(categoryName: categoryName, url: url);
        }
    }
}
