using System.IO;
using System.Text;

namespace KissLog.AspNet.Web
{
    internal class ResponseSniffer : Stream
    {
        private readonly Stream _streamToCapture;
        private readonly Encoding _responseEncoding;
        private readonly StringBuilder _streamContent;

        public ResponseSniffer(Stream streamToCapture, Encoding responseEncoding)
        {
            _responseEncoding = responseEncoding;
            _streamToCapture = streamToCapture;
            _streamContent = new StringBuilder();
        }

        public string GetContent()
        {
            return _streamContent.ToString();
        }

        public override bool CanRead => _streamToCapture.CanRead;

        public override bool CanSeek => _streamToCapture.CanSeek;

        public override bool CanWrite => _streamToCapture.CanWrite;

        public override void Flush()
        {
            _streamToCapture.Flush();
        }

        public override long Length => _streamToCapture.Length;

        public override long Position
        {
            get => _streamToCapture.Position;
            set => _streamToCapture.Position = value;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return _streamToCapture.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return _streamToCapture.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            _streamToCapture.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            _streamContent.Append(_responseEncoding.GetString(buffer));
            _streamToCapture.Write(buffer, offset, count);
        }

        public override void Close()
        {
            _streamToCapture.Close();
            base.Close();
        }
    }
}
