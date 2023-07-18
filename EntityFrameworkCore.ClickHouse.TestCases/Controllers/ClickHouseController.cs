using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace EntityFrameworkCore.ClickHouse.TestCases.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClickHouseController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<ClickHouseController> _logger;
        private readonly ClickHouseContext _clickHouseContext;

        public ClickHouseController(ILogger<ClickHouseController> logger, ClickHouseContext clickHouseContext)
        {
            _logger = logger;
            _clickHouseContext = clickHouseContext;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var t = _clickHouseContext.User
                //.Include(a => a.WebStore)
              //  .Where(a => a.MediaId == 2191)
             //   .OrderByDescending(a => a.OrderId)
                .Take(40).ToList();
            return Ok(t);
        }
    }
}