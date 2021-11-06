using KissLog;
using System;
using System.IO;
using System.Web.Http;

namespace AspNet.WebApi.Controllers
{
    public class ValuesController : ApiController
    {
        private readonly IKLogger _logger;
        public ValuesController()
        {
            _logger = Logger.Factory.Get();
        }

        public FlushLogArgs Get()
        {
            _logger.Trace("Trace log");
            _logger.Debug("Debug log");
            _logger.Info("Information log");
            _logger.Warn("Warning log");
            _logger.Error("Error log");
            _logger.Critical("Critical log");

            _logger.Error(new DivideByZeroException());

            string file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Web.config");

            _logger.LogAsFile($"Text content logged as file. Guid: {Guid.NewGuid()}", "file-01.txt");
            _logger.LogFile(file, "Web.config");

            _logger.AddCustomProperty("CorrelationId", Guid.NewGuid());
            _logger.AddCustomProperty("boolean", true);
            _logger.AddCustomProperty("date", DateTime.UtcNow);
            _logger.AddCustomProperty("integer", 100);

            _logger.LogResponseBody(true);

            Logger logger = _logger as Logger;
            FlushLogArgs args = FlushLogArgsFactory.Create(new[] { logger });

            return args;
        }
    }
}
