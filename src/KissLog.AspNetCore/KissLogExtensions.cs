using Microsoft.Extensions.Logging;

namespace KissLog.AspNetCore
{
    public static class KissLogExtensions
    {
        public static ILoggingBuilder AddKissLog(this ILoggingBuilder builder)
        {
            builder.AddProvider(new LoggerProvider());
            
            return builder;
        }
    }
}
