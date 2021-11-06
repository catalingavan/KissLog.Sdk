using System;

namespace KissLog
{
    public class LogMessage
    {
        public string CategoryName { get; }
        public LogLevel LogLevel { get; }
        public string Message { get; }
        public DateTime DateTime { get; }
        public string MemberType { get; }
        public string MemberName { get; }
        public int LineNumber { get; }

        internal LogMessage(CreateOptions options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            if (string.IsNullOrWhiteSpace(options.CategoryName))
                throw new ArgumentNullException(nameof(options.CategoryName));

            if (string.IsNullOrWhiteSpace(options.Message))
                throw new ArgumentNullException(nameof(options.Message));

            CategoryName = options.CategoryName;
            LogLevel = options.LogLevel;
            Message = options.Message;
            MemberType = options.MemberType;
            MemberName = options.MemberName;
            LineNumber = options.LineNumber;
            DateTime = options.DateTime;
        }

        internal class CreateOptions
        {
            public string CategoryName { get; set; }
            public LogLevel LogLevel { get; set; }
            public string Message { get; set; }
            public string MemberType { get; set; }
            public string MemberName { get; set; }
            public int LineNumber { get; set; }
            public DateTime DateTime { get; set; }

            public CreateOptions()
            {
                DateTime = DateTime.UtcNow;
            }
        }

        internal LogMessage Clone()
        {
            return new LogMessage(new CreateOptions
            {
                CategoryName = CategoryName,
                LogLevel = LogLevel,
                Message = Message,
                MemberType = MemberType,
                MemberName = MemberName,
                LineNumber = LineNumber,
                DateTime = DateTime
            });
        }
    }
}
