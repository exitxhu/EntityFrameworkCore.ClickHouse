using CHUseCases.Infra;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace CHUseCases.Controllers;

[ApiController]
[Route("[controller]")]
public class TickController : ControllerBase
{

    private readonly ILogger<TickController> _logger;
    private readonly DataContext _dataContext;
    public TickController(ILogger<TickController> logger, DataContext dataContext)
    {
        _logger = logger;
        _dataContext = dataContext;
    }

    [HttpGet("[action]")]
    public IEnumerable<User> Get()
    {
        var t = _dataContext.Users.Sum(a => a.Count);

        var tt = _dataContext.Users
            .Join(_dataContext.Balances, a => a.Id, b => b.UserId, (a, b) => new
            {
                a.Id,
                a.Name,
                b.Count
            })
            .GroupBy(a => a.Id)
            .Select(a => new { a.Key, Sum = a.Sum(b => b.Count) })
            .ToList();

        return _dataContext.Users
             .Where(a => a.Name == "asd")
             .ToList();
    }
    [HttpGet("[action]")]
    public object Sum()
    {
        return _dataContext.Users
             .Where(a => a.Name.Contains("asd"));
        //             .Sum(a => a.Count);
    }
    [HttpGet("[action]")]
    public User Set(string name)
    {
        var id = _dataContext.Users.Max(a => a.Id);
        var user = new User { Name = name ?? "default", Id = id + 1, Count = new Random().Next(0, 555) };
        _dataContext.Users.Add(user);
        _dataContext.SaveChanges();
        return user;
    }
}
