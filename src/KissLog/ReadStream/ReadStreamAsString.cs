using System;
using System.IO;
using System.Text;

namespace KissLog.ReadStream
{
    internal class ReadStreamAsString : IReadStreamStrategy
    {
        private readonly Stream _stream;
        private readonly Encoding _encoding;
        public ReadStreamAsString(Stream stream, Encoding encoding)
        {
            _stream = stream ?? throw new ArgumentNullException(nameof(stream));
            _encoding = encoding ?? throw new ArgumentNullException(nameof(encoding));
        }

        public ReadStreamResult Read()
        {
            if (!_stream.CanRead)
                return new ReadStreamResult();

            string content = null;
            using (StreamReader reader = new StreamReader(_stream, _encoding, true))
            {
                _stream.Position = 0;
                content = reader.ReadToEnd();
            }

            return new ReadStreamResult
            {
                Content = content
            };
        }
    }
}
