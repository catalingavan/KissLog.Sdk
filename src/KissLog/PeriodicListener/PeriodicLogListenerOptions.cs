using KissLog.Listeners;
using System;

namespace KissLog.PeriodicListener
{
    public class PeriodicLogListenerOptions
    {
        public TimeSpan TriggerInterval { get; set; }
        public ITextFormatter TextFormatter { get; set; }

        public PeriodicLogListenerOptions()
        {
            TriggerInterval = TimeSpan.FromSeconds(2);
            TextFormatter = new DefaultTextFormatter();
        }
    }
}
