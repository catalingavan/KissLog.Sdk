using Microsoft.AspNetCore.Http;

namespace KissLog.AspNetCore.Tests
{
    public class CustomHttpContextAccessor : IHttpContextAccessor
    {
        public HttpContext HttpContext { get; set; }
    }
}
