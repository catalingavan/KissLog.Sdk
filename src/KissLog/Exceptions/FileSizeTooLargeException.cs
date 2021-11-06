using System;

namespace KissLog.Exceptions
{
    internal class FileSizeTooLargeException : Exception
    {
        public FileSizeTooLargeException(long fileSize, long maximumAllowedFileSize) : base(ErrorMessage(fileSize, maximumAllowedFileSize))
        {

        }

        private static string ErrorMessage(long fileSize, long maximumAllowedFileSize)
        {
            return $"KissLog: The file cannot be logged because the file size {fileSize} exceeds the maximum allowed size of {maximumAllowedFileSize} bytes";
        }
    }
}
