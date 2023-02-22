using Microsoft.Extensions.Logging;

namespace dotnet6_ConsoleApp.Services
{
    internal class FooService : IFooService
    {
        private readonly ILogger<FooService> _logger;
        public FooService(ILogger<FooService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void Foo()
        {
            _logger.LogInformation("Doing Foo()");
        }
    }
}
