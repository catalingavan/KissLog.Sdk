using System;

namespace KissLog.Internal
{
    internal static class PackageInit
    {
        private const string SdkName = "KissLog";

        static string GetSdkVersion()
        {
            try
            {
                Version version = new Version("1.0.0");

                #if !NETSTANDARD1_3 && !NETSTANDARD1_4

                version = typeof(PackageInit).Assembly.GetName().Version;

                #endif

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
