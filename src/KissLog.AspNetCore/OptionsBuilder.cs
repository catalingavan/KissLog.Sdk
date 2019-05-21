using System.Collections.Generic;

namespace KissLog.AspNetCore
{
    internal class OptionsBuilder : IOptionsBuilder
    {
        public List<ILogListener> Listeners => KissLogConfiguration.Listeners;
        public Options Options => KissLogConfiguration.Options;
    }
}
