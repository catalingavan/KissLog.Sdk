using System;

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
            InternalHelpers.SdkName = SdkName;
            InternalHelpers.SdkVersion = GetSdkVersion();
        }
    }
}
