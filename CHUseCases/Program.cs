using Microsoft.Extensions.Configuration;
using Renci.SshNet;
using ClickHouse.EntityFrameworkCore.Extensions;
using ClickHouse.EntityFrameworkCore.Storage.Engines;
using Microsoft.EntityFrameworkCore;
using CHUseCases.Infra;
using Microsoft.EntityFrameworkCore.Design;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var sshs = builder.Configuration.GetSection("SSHSetting");
var config = sshs?.Get<SshSetting>();

config?.StartTunnel();

var chs = builder.Configuration.GetSection("ClickHouseSetting");
var chc = chs?.Get<ClickHouseSetting>();
builder.Services.AddClickHouse(chc.ConnectionString, a =>
{
    a.UserName = chc.MapPostgres.UserName;
    a.Password = chc.MapPostgres.Password;
    a.DataBaseName = chc.MapPostgres.DataBaseName;
    a.Host = chc.MapPostgres.Host;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public static class Tunneler
{
    public static void StartTunnel(this SshSetting setting)
    {
        if (setting is null || !setting.ForceTunnel)
            return;
        var client = new SshClient(setting.HostIp, setting.UserName, setting.Password);
        client.Connect();
        // Display error
        Console.WriteLine(!client.IsConnected ? "Client not connected!" : "Client connected!");
        var port = new ForwardedPortLocal("127.0.0.1", setting.BoundedPort, setting.HostTunnleIp ?? "127.0.0.1", setting.HostPort);
        client.AddForwardedPort(port);
        port.Start();

    }

    public static void StartTunnel(this SshSetting[] settings)
    {
        if (settings is null || settings.Length == 0)
            return;
        foreach (var setting in settings)
            setting.StartTunnel();
    }
    public static IServiceCollection AddClickHouse(this IServiceCollection services, string constr, Action<PostgreSQLEngineOptions> op = null)
    {
        services.AddDbContext<DataContext>(a => a.UseClickHouse(constr, postgresOpBuilder: op));

        return services;
    }
}
public class SshSetting
{
    public bool ForceTunnel { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public uint HostPort { get; set; }
    public string HostIp { get; set; }
    public string HostTunnleIp { get; set; }
    public uint BoundedPort { get; set; }
}


public class ClickHouseSetting
{
    public string ConnectionString { get; set; }
    public PostgreSQLEngineOptions MapPostgres { get; set; }
}
public class ClickHouseDesignTimeServices : IDesignTimeServices
{
    public void ConfigureDesignTimeServices(IServiceCollection services)
    {
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }
        services.AddEntityFrameworkClickHouseDesignTime();
    }
}