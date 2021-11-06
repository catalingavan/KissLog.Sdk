using System;

namespace KissLog
{
    public class CapturedException
    {
        public string Type { get; }
        public string Message { get; }
        public string ExceptionString { get; }

        public CapturedException(Exception ex)
        {
            if (ex == null)
                throw new ArgumentNullException(nameof(ex));

            Type = ex.GetType().FullName;
            Message = ex.Message;
            ExceptionString = ex.ToString();
        }
    }
}
