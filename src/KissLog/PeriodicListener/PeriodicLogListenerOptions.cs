using KissLog.Formatting;
using System;

namespace KissLog.PeriodicListener
{
    public class PeriodicLogListenerOptions
    {
        public TimeSpan TriggerInterval { get; set; }
        public TextFormatter TextFormatter { get; set; }

        public PeriodicLogListenerOptions()
        {
            TriggerInterval = TimeSpan.FromSeconds(2);
            TextFormatter = new TextFormatter();
        }
    }
}
