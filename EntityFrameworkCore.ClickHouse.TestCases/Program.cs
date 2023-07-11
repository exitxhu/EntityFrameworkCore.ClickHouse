
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

namespace EntityFrameworkCore.ClickHouse.TestCases;
public class ClickHouseDesignTimeServices : IDesignTimeServices
{
    public void ConfigureDesignTimeServices(IServiceCollection services)
    {
        Debugger.Launch();
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        services.AddEntityFrameworkClickHouse()
            .AddSingleton<IAnnotationCodeGenerator, ClickHouseAnnotationCodeGenerator>()
            .AddSingleton<IDatabaseModelFactory, ClickHouseDatabaseModelFactory>()
            .AddSingleton<ICSharpHelper, ClickHouseCSharpHelper>()
            .AddSingleton<AnnotationCodeGeneratorDependencies, AnnotationCodeGeneratorDependencies>()
            .AddSingleton<ICSharpMigrationOperationGenerator, ClickHouseCSharpMigrationOperationGenerator>()
            .AddSingleton<IMigrationsCodeGenerator, ClickHouseCSharpMigrationsGenerator>()


            ;

    }
}

public class Program
{
    public static async Task Main(string[] args)
    {
        var tf = new MergeTreeEngine<Order>("");
        var sds = ClickHouseEngineTypeConstants.MergeTreeEngine;
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddDbContext<ClickHouseContext>(a => a.UseClickHouse($"Host=127.0.0.1;Protocol=http;Port=8443;Database=test;Username=affilio;Password=pcIslv0JbSjLGxV;"));
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        var t = builder.Services.BuildServiceProvider();
        var m = t.GetRequiredService<ClickHouseContext>();
        await m.Database.EnsureCreatedAsync();
        await m.Database.MigrateAsync();
        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}

public class ClickHouseContext : DbContext
{
    public DbSet<Order> Order { get; set; }
    public DbSet<Det> Dets { get; set; }
    public ClickHouseContext(DbContextOptions op) : base(op)
    {

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>().HasKey(e => e.OrderId).HasAnnotation("myann","so do");
        modelBuilder.Entity<Order>().Property(e => e.OrderId).ValueGeneratedNever();
        modelBuilder.Model.AddAnnotation("asd","Asdasd");
        modelBuilder.Entity<Order>()
            .HasMergeTreeEngine("OrderId,MediaId");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //  optionsBuilder.UseClickHouse();
        //   optionsBuilder.UseClickHouse("Host=localhost;Protocol=http;Port=8123;Database=" + TestContext.CurrentContext.Test.ClassName);

    }
}
public record Det
{
    public int Id { get; set; }
    public int? Null { get; set; }
}
public record Order
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long OrderId { get; set; }

    public int MediaId { get; set; }
    public int LinkId { get; set; }
    public DateTime? LastStatusUpdateDate { get; set; }

    public OrderPaymentStatus PaymentStatus { get; set; } = OrderPaymentStatus.WaitingForInvoice;
    public string MediaName { get; set; }
    public string LinkName { get; set; }
    public int? RefererUserId { get; set; }
    public long? Samad { get; set; }
}
public enum OrderPaymentStatus
{
    WaitingForInvoice,
    WaitingForPayment,
}

