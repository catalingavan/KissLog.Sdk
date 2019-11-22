using System;
using System.Diagnostics;

namespace KissLog
{
    public static class KissLogConfiguration
    {
        public static ListenersContainer Listeners { get; } = new ListenersContainer();

        public static Options Options { get; } = new Options();

        public static Action<string> InternalLog { get; set; } = (string message) => Debug.WriteLine(message);

        static KissLogConfiguration()
        {
            Internal.PackageInit.Init();
        }
    }
}
