namespace KissLog
{
    public class LogResponseBodyArgument
    {
        private bool? _result = null;
        internal bool? GetValue()
        {
            return _result;
        }

        protected LogResponseBodyArgument()
        {
            
        }

        public static LogResponseBodyArgument True => new LogResponseBodyArgument { _result = true };
        public static LogResponseBodyArgument False => new LogResponseBodyArgument { _result = false };
        public static LogResponseBodyArgument Default => new LogResponseBodyArgument { _result = null };
    }
}
