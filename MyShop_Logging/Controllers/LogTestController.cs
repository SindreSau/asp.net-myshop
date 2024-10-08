using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MyShop_Logging.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogTestController(ILogger<LogTestController> logger) : ControllerBase
    {
        [HttpGet]
        [Route("info")]
        public IActionResult Get()
        {
            logger.LogInformation("This is a log message from the LogTestController");
            Console.WriteLine("This is a console writeline from the LogTestController");
            return Ok();
        }

        // .net loglevels: Trace, Debug, Information, Warning, Error, Critical

        [HttpGet]
        [Route("trace")]
        public IActionResult Trace()
        {
            logger.LogTrace("This is a log message from the LogTestController");
            return Ok();
        }

        [HttpGet]
        [Route("debug")]
        public IActionResult Debug()
        {
            logger.LogDebug("This is a log message from the LogTestController");
            return Ok();
        }

        [HttpGet]
        [Route("warning")]
        public IActionResult Warning()
        {
            logger.LogWarning("This is a log message from the LogTestController");
            return Ok();
        }

        [HttpGet]
        [Route("error")]
        public IActionResult Error()
        {
            logger.LogError("This is a log message from the LogTestController");
            return Ok();
        }

        [HttpGet]
        [Route("critical")]
        public IActionResult Critical()
        {
            logger.LogCritical("This is a log message from the LogTestController");
            return Ok();
        }
    }
}
