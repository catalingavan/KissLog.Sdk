using System.Reflection;

namespace KissLog
{
    internal static class ModuleInitializer
    {
        public static void Init()
        {
            AssemblyName assembly = typeof(ModuleInitializer).Assembly.GetName();
            KissLogPackage package = new KissLogPackage(assembly.Name, assembly.Version);

            KissLogConfiguration.KissLogPackages.Add(package);
        }
    }
}
