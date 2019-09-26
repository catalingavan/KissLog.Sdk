using Microsoft.AspNetCore.Http;

namespace KissLog.AspNetCore.ReadInputStream
{
    internal interface IReadInputStreamProvider
    {
        string ReadInputStream(HttpRequest request);
    }
}
