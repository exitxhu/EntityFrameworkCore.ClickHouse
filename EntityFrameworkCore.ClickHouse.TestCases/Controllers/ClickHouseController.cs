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
            var cmd = _clickHouseContext.Database.GetDbConnection().CreateCommand();
            cmd.CommandText = "select * from Order";
            var r = cmd.ExecuteReader();
            while (r.Read())
            {
                var v = r.GetFieldValue<DateOnly>(1);
            }
            var t = _clickHouseContext.Order
                //.Include(a => a.WebStore)
                //  .Where(a => a.MediaId == 2191)
                //   .OrderByDescending(a => a.OrderId)
                .Take(40).ToList();
            return Ok(t);
        }
        [HttpPost]
        public async Task<IActionResult> Add()
        {
            var r = new Random();
            var tt = Enumerable.Range(0, 10)
                .Select(a => new Order
                {
                    Amount = r.Next(1000, 100000),
                    //Date = new DateOnly(2018 + r.Next(0, 4), r.Next(1, 12), r.Next(1, 29)),
                    OrderId = r.Next(int.MaxValue),
                });
            //_clickHouseContext.Order.AddRange(tt);

            var cmd = _clickHouseContext.Database.GetDbConnection().CreateCommand();
            cmd.CommandText = "INSERT INTO test.\"Order\" (\"OrderId\", \"Amount\", \"Date\") VALUES ({p3:Int64}, {p4:Int64}, {p5:Date})";
            //cmd.Parameters.Add(new { "p3", tt.First().OrderId });
            _clickHouseContext.Order.Add(tt.First());

            _clickHouseContext.SaveChanges();
            return Ok();
        }
    }
}