using KissLog.AspNetCore.ReadInputStream;
using System;
using System.Linq;
using System.Reflection;

namespace KissLog.AspNetCore
{
    internal static class ModuleInitializer
    {
        public static IReadInputStreamProvider ReadInputStreamProvider = new NullReadInputStreamProvider();

        public static void Init()
        {
            AssemblyName assembly = typeof(ModuleInitializer).Assembly.GetName();
            KissLogPackage package = new KissLogPackage(assembly.Name, assembly.Version);

            KissLogConfiguration.KissLogPackages.Add(package);

            SetReadInputStreamProvider();
        }

        private static void SetReadInputStreamProvider()
        {
            bool hasEnableBuffering = KissLog.InternalHelpers.WrapInTryCatch(() => HasEnableBuffering());
            if (hasEnableBuffering)
            {
                ReadInputStreamProvider = new EnableBufferingReadInputStreamProvider();
            }

            string type = ReadInputStreamProvider.GetType().Name;
            InternalLogger.Log($"ReadInputStreamProvider: {type}", LogLevel.Information);
        }

        private static bool HasEnableBuffering()
        {
            bool result = false;

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

            return result;
        }

        private static bool HasEnableRewind()
        {
            bool result = false;

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

            return result;
        }
    }
}
