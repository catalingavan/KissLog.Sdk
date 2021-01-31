using Microsoft.Extensions.Logging;

namespace KissLog.AspNetCore
{
    public static class KissLogAspNetCoreExtensions
    {
        public static ILoggingBuilder AddKissLog(this ILoggingBuilder builder)
        {
            return builder.AddKissLog(null);
        }

        public static ILoggingBuilder AddKissLog(this ILoggingBuilder builder, KissLogAspNetCoreOptions options)
        {
            builder.AddProvider(new LoggerProvider(options));

            return builder;
        }
    }
}
