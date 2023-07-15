using Microsoft.AspNetCore.Mvc;

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
            await _clickHouseContext.Order.AddRangeAsync(new List<Order>{ new Order
            {
                LastStatusUpdateDate = DateTime.Now,
                MediaName = "asdasdf",
                OrderId = 123,
                PaymentStatus = OrderPaymentStatus.WaitingForInvoice,
                RefererUserId = 1,
            } });
            _clickHouseContext.SaveChanges();
            return Ok(_clickHouseContext.Order);
        }
    }
}