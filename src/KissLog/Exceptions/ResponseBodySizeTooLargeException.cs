using System;

namespace KissLog.Exceptions
{
    internal class ResponseBodySizeTooLargeException : Exception
    {
        public ResponseBodySizeTooLargeException(long contentLength, long maximumAllowedResponseBodySize) : base(ErrorMessage(contentLength, maximumAllowedResponseBodySize))
        {
            
        }

        private static string ErrorMessage(long contentLength, long maximumAllowedResponseBodySize)
        {
            return $"KissLog: The response body cannot be logged because the contentLength {contentLength} exceeds the maximum allowed size of {maximumAllowedResponseBodySize} bytes";
        }
    }
}
