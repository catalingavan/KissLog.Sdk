using System;

namespace KissLog.AspNetCore
{
    public class FormatterArgs
    {
        public object State { get; }
        public Exception Exception { get; }
        public string DefaultValue { get; }
        public Logger Logger { get; }

        internal FormatterArgs(CreateOptions options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            State = options.State;
            Exception = options.Exception;
            DefaultValue = options.DefaultValue;
            Logger = options.Logger ?? throw new ArgumentNullException(nameof(options.Logger));
        }

        internal class CreateOptions
        {
            public object State { get; set; }
            public Exception Exception { get; set; }
            public string DefaultValue { get; set; }
            public Logger Logger { get; set; }
        }
    }
}
