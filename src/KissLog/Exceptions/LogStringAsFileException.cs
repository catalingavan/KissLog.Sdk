using System;

namespace KissLog.Exceptions
{
    internal class LogStringAsFileException : Exception
    {
        public LogStringAsFileException(string contents, Exception innerException) :
            base(ErrorMessage(contents), innerException)
        {

        }

        private static string ErrorMessage(string contents)
        {
            int length = string.IsNullOrEmpty(contents) ? 0 : contents.Length;
            return $"KissLog: Error when trying to log as file the string with length {length}";
        }
    }
}
