using System.Collections.Generic;

namespace KissLog
{
    public interface IKissLoggerFactory
    {
        ILogger Get(string categoryName = Logger.DefaultCategoryName);

        IEnumerable<ILogger> GetAll();
    }
}
