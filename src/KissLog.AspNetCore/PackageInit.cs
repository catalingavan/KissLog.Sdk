using KissLog.AspNetCore.ReadInputStream;
using KissLog.Internal;
using System;
using System.Linq;
using System.Reflection;
using System.Text;

namespace KissLog.AspNetCore
{
    internal static class PackageInit
    {
        private const string SdkName = "KissLog.AspNetCore";

        internal static IReadInputStreamProvider ReadInputStreamProvider = new NullReadInputStreamProvider();

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
            SetReadInputStreamProvider();

            InternalHelpers.SdkName = SdkName;
            InternalHelpers.SdkVersion = GetSdkVersion();
        }

        private static void SetFactory()
        {
            IKissLoggerFactory loggerFactory = new AspNetCoreLoggerFactory();

            PropertyInfo factoryProperty = typeof(Logger).GetProperty("Factory");
            factoryProperty.SetValue(Logger.Factory, loggerFactory, null);
        }

        private static void SetReadInputStreamProvider()
        {
            bool hasEnableBuffering = HasEnableBuffering();
            if(hasEnableBuffering)
            {
                ReadInputStreamProvider = new EnableBufferingReadInputStreamProvider();
                KissLog.Internal.InternalHelpers.Log($"ReadInputStreamProvider: {nameof(EnableBufferingReadInputStreamProvider)}", LogLevel.Information);

                return;
            }

            bool hasEnableRewind = HasEnableRewind();
            if(hasEnableRewind)
            {
                ReadInputStreamProvider = new EnableRewindReadInputStreamProvider();
                KissLog.Internal.InternalHelpers.Log($"ReadInputStreamProvider: {nameof(EnableRewindReadInputStreamProvider)}", LogLevel.Information);

                return;
            }

            ReadInputStreamProvider = new NullReadInputStreamProvider();
            KissLog.Internal.InternalHelpers.Log($"ReadInputStreamProvider: {nameof(NullReadInputStreamProvider)}", LogLevel.Warning);
        }

        private static bool HasEnableBuffering()
        {
            bool result = false;

            try
            {
                string assemblyName = "Microsoft.AspNetCore.Http";
                string typeName = "Microsoft.AspNetCore.Http.HttpRequestRewindExtensions";
                string assemblyQualifiedName = Assembly.CreateQualifiedName(assemblyName, typeName);

                Type type = Type.GetType(assemblyQualifiedName, false);
                if (type != null)
                {
                    var enableBuffering = type
                        .GetMethods(BindingFlags.Static | BindingFlags.Public)
                        .FirstOrDefault(m => m.Name == "EnableBuffering");

                    result = enableBuffering != null;
                }
            }
            catch(Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Error trying to calculate HasEnableBuffering()");
                sb.AppendLine(ex.ToString());

                KissLog.Internal.InternalHelpers.Log(sb.ToString(), LogLevel.Error);
            }

            return result;
        }

        private static bool HasEnableRewind()
        {
            bool result = false;

            try
            {
                string assemblyName = "Microsoft.AspNetCore.Http";
                string typeName = "Microsoft.AspNetCore.Http.Internal.BufferingHelper";
                string assemblyQualifiedName = Assembly.CreateQualifiedName(assemblyName, typeName);

                Type type = Type.GetType(assemblyQualifiedName, false);
                if (type != null)
                {
                    var enableRewind = type
                        .GetMethods(BindingFlags.Static | BindingFlags.Public)
                        .FirstOrDefault(m => m.Name == "EnableRewind");

                    result = enableRewind != null;
                }
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Error trying to calculate HasEnableRewind()");
                sb.AppendLine(ex.ToString());

                KissLog.Internal.InternalHelpers.Log(sb.ToString(), LogLevel.Error);
            }

            return result;
        }
    }
}
