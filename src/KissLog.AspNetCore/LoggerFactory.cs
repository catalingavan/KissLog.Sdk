using Microsoft.AspNetCore.Http;
using System;

namespace KissLog.AspNetCore
{
    public static class LoggerFactory
    {
        [Obsolete("This method is obsolete. Use 'Logger.Factory.GetInstance()' instead.", true)]
        public static ILogger GetInstance(IHttpContextAccessor httpContextAccessor, string categoryName = Logger.DefaultCategoryName)
        {
            return Logger.Factory.Get(categoryName);
        }

        [Obsolete("This method is obsolete. Use 'Logger.Factory.GetInstance()' instead.", true)]
        public static ILogger GetInstance(string categoryName = Logger.DefaultCategoryName)
        {
            return Logger.Factory.Get(categoryName);
        }
    }
}
