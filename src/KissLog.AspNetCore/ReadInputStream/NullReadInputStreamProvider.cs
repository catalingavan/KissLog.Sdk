using Microsoft.AspNetCore.Http;

namespace KissLog.AspNetCore.ReadInputStream
{
    internal class NullReadInputStreamProvider : IReadInputStreamProvider
    {
        public string ReadInputStream(HttpRequest request)
        {
            return string.Empty;
        }
    }
}
