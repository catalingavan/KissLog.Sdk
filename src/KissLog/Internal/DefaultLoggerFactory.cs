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
            var logger = new Logger(categoryName: categoryName, url: url);
            logger.DataContainer.AddProperty(Constants.FactoryNameProperty, nameof(DefaultLoggerFactory));

            return logger;
        }
    }
}
