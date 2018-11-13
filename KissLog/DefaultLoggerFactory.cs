using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace KissLog
{
    internal class DefaultLoggerFactory : IKissLoggerFactory
    {
        private static readonly ConcurrentDictionary<string, ILogger> StaticInstances = new ConcurrentDictionary<string, ILogger>();

        public IEnumerable<ILogger> GetAll()
        {
            return StaticInstances.Values.ToList();
        }

        public ILogger Get(string categoryName = "Default")
        {
            return StaticInstances.GetOrAdd(categoryName, (key) => new Logger(key));
        }
    }
}
