using NLog;
using System.Web.Mvc;

namespace NLog_AspNet.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger _logger;
        public HomeController()
        {
            _logger = LogManager.GetCurrentClassLogger();
        }

        public ActionResult Index()
        {
            _logger.Trace("Trace log");
            _logger.Debug("Debug log");
            _logger.Info("Info log");
            _logger.Warn("Warning log");
            _logger.Error("Error log");
            _logger.Fatal("Fatal log");

            return View();
        }
    }
}