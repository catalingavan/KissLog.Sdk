using System;

namespace KissLog.Exceptions
{
    internal class LogFileException : Exception
    {
        public LogFileException(string sourceFilePath, Exception innerException) :
            base(ErrorMessage(sourceFilePath), innerException)
        {

        }

        private static string ErrorMessage(string sourceFilePath)
        {
            return $"KissLog: Error when trying to log the file {sourceFilePath}";
        }
    }
}
