using System;

namespace KissLog
{
    public class LogMessageCreatedEventArgs : EventArgs
    {
        public LogMessage LogMessage { get; set; }
    }
}
