using KissLog;
using System;
using System.Web.Mvc;

namespace AspNet.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly IKLogger _logger;
        public HomeController()
        {
            _logger = Logger.Factory.Get();
        }

        public ActionResult Index()
        {
            _logger.Trace("Trace log");
            _logger.Debug("Debug log");
            _logger.Info("Information log");
            _logger.Warn("Warning log");
            _logger.Error("Error log");
            _logger.Critical("Critical log");

            _logger.Error(new DivideByZeroException());

            return Content("Hello world");
        }
    }
}