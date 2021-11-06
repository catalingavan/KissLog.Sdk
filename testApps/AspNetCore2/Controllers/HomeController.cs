using KissLog.AspNetCore;
using AspNetCore2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace AspNetCore2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            _logger.LogTrace("Trace log");
            _logger.LogDebug("Debug log");
            _logger.LogInformation("Information log");
            _logger.LogWarning("Warning log");
            _logger.LogError("Error log");
            _logger.LogCritical("Critical log");

            _logger.LogError(new DivideByZeroException(), "Divide by zero ex");

            string file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");

            _logger.LogAsFile($"Text content logged as file. Guid: {Guid.NewGuid()}", "file-01.txt");
            _logger.LogFile(file, "appsettings.json");

            _logger.AddCustomProperty("CorrelationId", Guid.NewGuid());
            _logger.AddCustomProperty("boolean", true);
            _logger.AddCustomProperty("date", DateTime.UtcNow);
            _logger.AddCustomProperty("integer", 100);

            _logger.LogResponseBody(true);

            return View();
        }

        [HttpPost]
        public IActionResult Form(FormViewModel model)
        {
            _logger.LogDebug("We use this method to test if FormData parameters are logged");

            if (!ModelState.IsValid)
            {
                _logger.SetStatusCode(400);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
