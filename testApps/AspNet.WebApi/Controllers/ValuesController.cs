using KissLog;
using System;
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

        public string Get()
        {
            _logger.Trace("Trace log");
            _logger.Debug("Debug log");
            _logger.Info("Information log");
            _logger.Warn("Warning log");
            _logger.Error("Error log");
            _logger.Critical("Critical log");

            _logger.Error(new DivideByZeroException());

            return "Hello world";
        }
    }
}
