using KissLog.Exceptions;
using System;

namespace KissLog.LogResponseBody
{
    internal class LogResponseBodySizeTooLargeException : ILogResponseBodyStrategy
    {
        private readonly Logger _logger;
        private readonly long _contentLength;
        private readonly long _maximumAllowedFileSizeInBytes;
        public LogResponseBodySizeTooLargeException(Logger logger, long contentLength, long maximumAllowedFileSizeInBytes)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            if (contentLength < 0)
                throw new ArgumentException(nameof(contentLength));

            if (contentLength <= maximumAllowedFileSizeInBytes)
                throw new ArgumentException(nameof(maximumAllowedFileSizeInBytes));

            _logger = logger;
            _contentLength = contentLength;
            _maximumAllowedFileSizeInBytes = maximumAllowedFileSizeInBytes;
        }

        public void Execute()
        {
            _logger.Warn(new ResponseBodySizeTooLargeException(_contentLength, _maximumAllowedFileSizeInBytes));
        }
    }
}
