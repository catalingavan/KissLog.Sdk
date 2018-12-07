using Microsoft.AspNetCore.Http;
using System;
using System.Reflection;

namespace KissLog.AspNetCore
{
    internal static class PackageInit
    {
        private const string SdkName = "KissLog.AspNetCore";

        static string GetSdkVersion()
        {
            try
            {
                Version version = typeof(PackageInit).Assembly.GetName().Version;
                return $"{version.Major}.{version.Minor}.{version.Build}";
            }
            catch
            {
                return "1.0.0";
            }
        }

        public static void Init()
        {
            SetFactory();

            InternalHelpers.SdkName = SdkName;
            InternalHelpers.SdkVersion = GetSdkVersion();
        }

        private static void SetFactory()
        {
            IKissLoggerFactory loggerFactory = new AspNetCoreLoggerFactory();

            PropertyInfo factoryProperty = typeof(Logger).GetProperty("Factory");
            factoryProperty.SetValue(Logger.Factory, loggerFactory, null);
        }
    }
}
