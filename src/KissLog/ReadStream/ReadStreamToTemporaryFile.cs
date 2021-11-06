using System;
using System.IO;

namespace KissLog.ReadStream
{
    internal class ReadStreamToTemporaryFile : IReadStreamStrategy
    {
        private readonly Stream _stream;
        public ReadStreamToTemporaryFile(Stream stream)
        {
            _stream = stream ?? throw new ArgumentNullException(nameof(stream));
        }

        public ReadStreamResult Read()
        {
            if (!_stream.CanRead)
                return new ReadStreamResult();

            TemporaryFile file = new TemporaryFile();

            try
            {
                _stream.Position = 0;
                using (var fs = File.OpenWrite(file.FileName))
                {
                    _stream.CopyTo(fs);
                }
            }
            catch
            {
                file.Dispose();
                throw;
            }

            return new ReadStreamResult
            {
                TemporaryFile = file
            };
        }
    }
}
