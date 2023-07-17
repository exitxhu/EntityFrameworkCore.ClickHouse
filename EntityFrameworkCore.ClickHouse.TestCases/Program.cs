
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

namespace EntityFrameworkCore.ClickHouse.TestCases;
public class ClickHouseDesignTimeServices : IDesignTimeServices
{
    public void ConfigureDesignTimeServices(IServiceCollection services)
    {
        //Debugger.Launch();

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
        var builder = WebApplication.CreateBuilder(args);

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
    public DbSet<Link> Links{ get; set; }
    public ClickHouseContext(DbContextOptions op) : base(op)
    {

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var ord = modelBuilder.Entity<Order>();
        ord.HasPostGresEngine("Order", "Order")
            ;
        var link = modelBuilder.Entity<Link>();
        link.HasPostGresEngine("Link", "Link");



    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //  optionsBuilder.UseClickHouse();
        //   optionsBuilder.UseClickHouse("Host=localhost;Protocol=http;Port=8123;Database=" + TestContext.CurrentContext.Test.ClassName);

    }
}
[ClickHouseTable(TableCreationStrategy.CREATE_OR_REPLACE)]
public record Link
{
    public int LinkId { get; set; }
    public string Name { get; set; }
}
[ClickHouseTable(TableCreationStrategy.CREATE_OR_REPLACE)]
public record Media
{
    public int MediaId { get; set; }
    public string Name { get; set; }
}
[Table("Order", Schema = "Order")]
[ClickHouseTable(TableCreationStrategy.CREATE_OR_REPLACE)]
public record Order
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long OrderId { get; set; }

    public int? LinkId { get; set; }
    public Link Link{ get; set; }
    public Media Media { get; set; }
    public int? MediaId { get; set; }

    public DateTime? LastStatusUpdateDate { get; set; }

    [JsonIgnore]
    public OrderPaymentStatus PaymentStatus { get; set; } = OrderPaymentStatus.WaitingForInvoice;
    [NotMapped]
    public string MediaName { get; set; }
    [NotMapped]
    public string LinkName { get; set; }
    public int? RefererUserId { get; set; }


    [StringLength(128)]
    public string OriginalOrderId { get; set; }
    [StringLength(32)]
    public string BasketId { get; set; }
    [ForeignKey("WebStoreId")]
    public int? WebStoreId { get; set; }
    [ForeignKey("LinkWebStoreId")]
    public int? LinkWebStoreId { get; set; }
    [ForeignKey("LinkId")]
    public DateTime? FinalizeDate { get; set; }
    [ForeignKey("ClickHistoryId")]
    public long? ClickHistoryId { get; set; }
    [MaxLength(50)]
    public string AffiliateId { get; set; }
    [StringLength(128)]
    public string OriginalUserId { get; set; }
    public bool IsNewCustomer { get; set; }
    public Price Amount { get; set; }
    public Price ShippingCost { get; set; }
    public Price VoucherPrice { get; set; }
    public Price VoucherUsedAmount { get; set; }
    [StringLength(128)]
    public string Source { get; set; }
    [StringLength(128)]
    public string CloseSource { get; set; }
    public DateTime OrderDate { get; set; }
    [StringLength(128)]
    public string State { get; set; }
    [StringLength(128)]
    public string City { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public DateTime? WebStoreLastUpdate { get; set; }
    public int Index { get; set; }
    public bool IsItemsEdited { get; set; } = false;
    public Price VatPrice { get; set; }
}
public enum OrderPaymentStatus
{
    WaitingForInvoice,
    WaitingForPayment,
}
[Owned]
public class Price : AbstractValueObject
{
    public long Amount { get; set; }
    public long Discount { get; set; }


    public Price(decimal amount, decimal discount) : this((long)amount, (long)discount)
    {
    }
    public Price(long amount, long discount)
    {
        Amount = amount;
        Discount = discount;
    }
    public Price()
    {
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Amount;
        yield return Discount;
    }
}
public abstract class AbstractValueObject
{
    protected static bool EqualOperator(AbstractValueObject left, AbstractValueObject right)
    {
        if (ReferenceEquals(left, null) ^ ReferenceEquals(right, null))
        {
            return false;
        }
        return ReferenceEquals(left, null) || left.Equals(right);
    }

    protected static bool NotEqualOperator(AbstractValueObject left, AbstractValueObject right)
    {
        return !(EqualOperator(left, right));
    }

    protected abstract IEnumerable<object> GetEqualityComponents();

    public override bool Equals(object obj)
    {
        if (obj == null || obj.GetType() != GetType())
        {
            return false;
        }

        var other = (AbstractValueObject)obj;

        return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }

    public override int GetHashCode()
    {
        return GetEqualityComponents()
            .Select(x => x is not null ? x.GetHashCode() : 0)
            .Aggregate((x, y) => x ^ y);
    }
    public static bool operator ==(AbstractValueObject left, AbstractValueObject right) => EqualOperator(left, right);
    public static bool operator !=(AbstractValueObject left, AbstractValueObject right) => !EqualOperator(left, right);
}
