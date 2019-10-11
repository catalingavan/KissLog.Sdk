using System;
using System.Collections.Generic;

namespace KissLog
{
    public static class KissLogConfiguration
    {
        public static List<ILogListener> Listeners = new List<ILogListener>();

        public static Options Options { get; } = new Options();

        public static Action<string, LogLevel> InternalLog { get; set; }

        static KissLogConfiguration()
        {
            Internal.PackageInit.Init();
        }
    }
}
