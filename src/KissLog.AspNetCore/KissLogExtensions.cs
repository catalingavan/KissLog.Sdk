using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using System;

namespace KissLog.AspNetCore
{
    public static class KissLogExtensions
    {
        public static IApplicationBuilder UseKissLogMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseKissLogMiddleware(null);
        }

        public static IApplicationBuilder UseKissLogMiddleware(this IApplicationBuilder builder, Action<IOptionsBuilder> configure)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            configure?.Invoke(new OptionsBuilder());

            return builder.UseMiddleware<KissLogMiddleware>();
        }

        public static ILoggingBuilder AddKissLog(this ILoggingBuilder builder)
        {
            return builder.AddKissLog(null);
        }

        public static ILoggingBuilder AddKissLog(this ILoggingBuilder builder, Action<LoggerOptions> configure)
        {
            LoggerOptions options = new LoggerOptions();
            configure?.Invoke(options);

            builder.AddProvider(new LoggerProvider(options));

            return builder;
        }
    }
}
