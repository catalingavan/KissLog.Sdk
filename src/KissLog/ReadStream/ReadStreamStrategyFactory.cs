using System;
using System.IO;
using System.Linq;
using System.Text;

namespace KissLog.ReadStream
{
    internal static class ReadStreamStrategyFactory
    {
        public static IReadStreamStrategy Create(Stream stream, Encoding encoding, string contentType)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            if (encoding == null)
                throw new ArgumentNullException(nameof(encoding));

            if (!stream.CanRead)
                return new NullReadStream();

            if (ShouldUseReadStreamAsString(stream.Length, contentType))
                return new ReadStreamAsString(stream, encoding);

            return new ReadStreamToTemporaryFile(stream);
        }

        private static bool ShouldUseReadStreamAsString(long contentLength, string contentType)
        {
            if (string.IsNullOrEmpty(contentType))
                return false;

            string[] allowedContentTypes = new[] { "application/json", "application/xml", "text/plain", "text/xml" };

            contentType = contentType.ToLowerInvariant();

            bool match = allowedContentTypes.Any(p => contentType.Contains(p));
            if (!match)
                return false;

            if (contentLength <= Constants.ReadStreamAsStringMaxContentLengthInBytes)
                return true;

            return false;
        }
    }
}
