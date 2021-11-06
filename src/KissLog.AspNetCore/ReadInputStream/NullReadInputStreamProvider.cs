using Microsoft.AspNetCore.Http;

namespace KissLog.AspNetCore.ReadInputStream
{
    public class NullReadInputStreamProvider : IReadInputStreamProvider
    {
        public string ReadInputStream(HttpRequest request)
        {
            return null;
        }
    }
}
