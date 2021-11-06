using System;

namespace KissLog.AspNetCore
{
    internal class OptionsBuilder : IOptionsBuilder
    {
        public LogListenersContainer Listeners => KissLogConfiguration.Listeners;
        public Options Options => KissLogConfiguration.Options;
        public Action<string> InternalLog
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
