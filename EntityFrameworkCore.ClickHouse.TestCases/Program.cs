
using ClickHouse.EntityFrameworkCore.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Emit;
using System.Text.Json.Serialization;
using System.Diagnostics;
using ClickHouse.EntityFrameworkCore.Design.Internal;
using ClickHouse.EntityFrameworkCore.Scaffolding.Internal;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ClickHouse.EntityFrameworkCore.Migrations.Design;
using Microsoft.EntityFrameworkCore.Migrations.Design;
using ClickHouse.EntityFrameworkCore.Storage.Engines;
using ClickHouse.EntityFrameworkCore.Metadata;
using System.Reflection;
using System.Collections.Specialized;
using ClickHouse.EntityFrameworkCore.Core;
using Humanizer;
using System.ComponentModel;

namespace EntityFrameworkCore.ClickHouse.TestCases;
public class ClickHouseDesignTimeServices : IDesignTimeServices
{
    public void ConfigureDesignTimeServices(IServiceCollection services)
    {
        //Debugger.Launch();
        Console.WriteLine("IDesignTimeServices runned");
        Debug.Print("IDesignTimeServices runned");
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        services.AddEntityFrameworkClickHouseDesignTime();
    }
}

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        //Debugger.Launch();

        // Add services to the container.
        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddDbContext<ClickHouseContext>(a => a.UseClickHouse($"Host=127.0.0.1;Protocol=http;Port=8443;Database=test;Username=affilio;Password=pcIslv0JbSjLGxV;",
            postgresOpBuilder: a =>
        {
            a.Password = "netoqu6V";
            a.Host = "172.16.150.2:5432";
            a.UserName = "affilio";
            a.DataBaseName = "affilio";
        }));
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        var t = builder.Services.BuildServiceProvider();
        var m = t.GetRequiredService<ClickHouseContext>();
        // await m.Database.EnsureCreatedAsync();
        await m.Database.MigrateAsync();
        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}

public class ClickHouseContext : ClickHouseDbContext
{
    public DbSet<Order> Order { get; set; }
    //public DbSet<Link> Links { get; set; }
    //public DbSet<User> User { get; set; }
    //public DbSet<Media> Medias{ get; set; }

    //public DbSet<ClickHistory> ClicjhhfshgjkjklkHistories { get; set; }
    public ClickHouseContext(DbContextOptions op) : base(op)
    {

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Debugger.Launch();

        var ord = modelBuilder.Entity<Order>().HasCreateStrategy(TableCreationStrategy.CREATE_OR_REPLACE);
        //ord.HasKey(a=>new {a.LinkId, a.OrderId});
        ord.HasReplacingMergeTreeEngine()
            .HasPrimaryKey(a => new { a.OrderId, a.Date })
            .HasPartitionBy(a => a.Date, PartitionByDateFormat.Month);

        //var link = modelBuilder.Entity<Link>();
        //link.HasPostGresEngine("Link", "Link");

        //var ch = modelBuilder.Entity<ClickHistory>();
        //ch.HasPostGresEngine("ClickHistory", "Click");

        //var media = modelBuilder.Entity<Media>();
        //link.HasPostGresEngine("Media", "Media");

        //var es = modelBuilder.Entity<WebStore>();
        //es.HasPostGresEngine("WebStore", "WebStore");

        ////es.Property(a => a.RecheckHeaders)
        ////     .HasConversion(a => a.ToString(), a => new KeyValueVO(a));

        //es.Property(a => a.RecheckHeaders)
        //    .HasConversion(a => a.Select(n => n.ToString()).ToArray(), a => a.Select(n => new KeyValueVO(n))
        //    .ToList());

        //var user = modelBuilder.Entity<User>();
        //user.HasPostGresEngine("User", "Accounting");

        //var notif = modelBuilder.Entity<NotificationTemplates>();
        //notif.Property(a => a.Variables)
        //    .HasConversion(n => n.Select(a => a.ToString()),
        //    n => n.Select(a => new KeyValueVO(a)).ToList());
        //notif.HasPostGresEngine("NotificationTemplates", "Notification");


        //base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //  optionsBuilder.UseClickHouse();
        //   optionsBuilder.UseClickHouse("Host=localhost;Protocol=http;Port=8123;Database=" + TestContext.CurrentContext.Test.ClassName);

    }
}

[Table("Order", Schema = "Order")]
public record Order
{
    [Key]
    public long OrderId { get; set; }
    public DateTime Date { get; set; }
    public long Amount { get; set; }
}
public enum OrderPaymentStatus
{
    WaitingForInvoice,
    WaitingForPayment,
}
