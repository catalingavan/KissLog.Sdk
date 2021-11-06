using System;

namespace KissLog.Exceptions
{
    internal class LogByteArrayAsFileException : Exception
    {
        public LogByteArrayAsFileException(byte[] contents, Exception innerException) :
            base(ErrorMessage(contents), innerException)
        {

        }

        private static string ErrorMessage(byte[] contents)
        {
            int length = contents == null ? 0 : contents.Length;
            return $"KissLog: Error when trying to log as file the byte[] with length {length}";
        }
    }
}
