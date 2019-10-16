using System;

namespace KissLog
{
    public static class KissLogConfiguration
    {
        public static ListenersContainer Listeners { get; } = new ListenersContainer();

        public static Options Options { get; } = new Options();

        public static Action<string, LogLevel> InternalLog { get; set; }

        static KissLogConfiguration()
        {
            Internal.PackageInit.Init();
        }
    }
}
