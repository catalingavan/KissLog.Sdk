using Microsoft.AspNetCore.Builder;
using System;

namespace KissLog.AspNetCore
{
    public static class KissLogMiddlewareExtensions
    {
        static KissLogMiddlewareExtensions()
        {
            PackageInit.Init();
        }

        public static IApplicationBuilder UseKissLogMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseKissLogMiddleware(options => { });
        }

        public static IApplicationBuilder UseKissLogMiddleware(this IApplicationBuilder builder, Action<IOptionsBuilder> configureOptions)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            configureOptions?.Invoke(new OptionsBuilder());

            return builder.UseMiddleware<KissLogMiddleware>();
        }
    }
}
