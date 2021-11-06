using KissLog.ReadStream;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace KissLog.LogResponseBody
{
    internal class LogResponseBodyStrategyFactory
    {
        public static ILogResponseBodyStrategy Create(Stream stream, Encoding encoding, Logger logger)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            if (encoding == null)
                throw new ArgumentNullException(nameof(encoding));

            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            if (!stream.CanRead)
                return new NullLogResponseBody();

            if(stream.Length > Constants.MaximumAllowedFileSizeInBytes)
            {
                return new LogResponseBodySizeTooLargeException(logger, stream.Length, Constants.MaximumAllowedFileSizeInBytes);
            }

            var headers = logger.DataContainer.HttpProperties.Response?.Properties?.Headers;
            string contentType = headers?.FirstOrDefault(p => string.Compare(p.Key, "Content-Type", StringComparison.OrdinalIgnoreCase) == 0).Value;
            string responseFileName = InternalHelpers.GenerateResponseFileName(headers);

            IReadStreamStrategy strategy = ReadStreamStrategyFactory.Create(stream, encoding, contentType);
            return new LogResponseBody(logger, responseFileName, strategy);
        }
    }
}
