using System;

namespace KissLog.AspNet.Web
{
    public static class LoggerFactory
    {
        [Obsolete("This method is obsolete. Use 'Logger.Factory.GetInstance()' instead.", true)]
        public static ILogger GetInstance(string categoryName = Logger.DefaultCategoryName)
        {
            return Logger.Factory.Get(categoryName);
        }
    }
}
