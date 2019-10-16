using System;
using System.Collections.Generic;

namespace KissLog.AspNetCore
{
    internal class OptionsBuilder : IOptionsBuilder
    {
        public ListenersContainer Listeners => KissLogConfiguration.Listeners;
        public Options Options => KissLogConfiguration.Options;
        public Action<string, LogLevel> InternalLog
        {
            get
            {
                return KissLogConfiguration.InternalLog;
            }
            set
            {
                KissLogConfiguration.InternalLog = value;
            }
        }
    }
}
