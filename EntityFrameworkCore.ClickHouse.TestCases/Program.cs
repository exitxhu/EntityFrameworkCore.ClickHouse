
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
        Debugger.Launch();

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
    //public DbSet<Order> Order { get; set; }
    //public DbSet<Link> Links { get; set; }
    public DbSet<User> User { get; set; }
    //public DbSet<Media> Medias{ get; set; }
    public ClickHouseContext(DbContextOptions op) : base(op)
    {

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Debugger.Launch();
        base.OnModelCreating(modelBuilder);
        //var ord = modelBuilder.Entity<Order>();
        //ord.HasPostGresEngine("Order", "Order")
        //    ;
        //var link = modelBuilder.Entity<Link>();
        //link.HasPostGresEngine("Link", "Link");

        //var media = modelBuilder.Entity<Media>();
        //link.HasPostGresEngine("Media", "Media");

        //var es = modelBuilder.Entity<WebStore>();
        //es.HasPostGresEngine("WebStore", "WebStore");

        //es.Property(a => a.RecheckHeaders)
        //     .HasConversion(a => a.ToString(), a => new KeyValueVO(a));

        //es.Property(a => a.RecheckHeaders)
        //    .HasConversion(a => a.Select(n => n.ToString()).ToArray(), a => a.Select(n => new KeyValueVO(n))
        //    .ToArray());

        var user = modelBuilder.Entity<User>();
        user.HasPostGresEngine("User", "Accounting");

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
    public Link Link { get; set; }
    [ClickHouseIgnore]
    public Media Media { get; set; }
    public int? MediaId { get; set; }
    public int? WebStoreId { get; set; }
    public WebStore WebStore { get; set; }
}
public enum OrderPaymentStatus
{
    WaitingForInvoice,
    WaitingForPayment,
}
[ClickHouseTable(TableCreationStrategy.CREATE_OR_REPLACE)]
public class WebStore
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int WebStoreId { get; set; }

    public string AlternativeUrl { get; set; }

    public List<KeyValueVO> RecheckHeaders { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime RecheckFromDate { get; set; }
    public string About { get; set; }
    public string TradeName { get; set; }

    public int? DependingWebStoreId { get; set; }
    [JsonIgnore]
    public WebStore DependingWebStore { get; set; }
    [NotMapped]
    public string DependingWebStoreName { get; set; }



}
public class User 

{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int UserId { get; set; }
    [StringLength(50)]
    public string Mobile { get; set; } //username
    //[JsonIgnore]
    [JsonIgnore]
    public string PasswordHash { get; set; }
    public int[] MyProperty { get; set; }
    public bool Agreement { get; set; }
    public List<KycStatusEnum> KycStatus { get; set; }
    public KycStatusEnum Status  { get; set; }
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
public class KeyValueVO : AbstractValueObject
{
    public KeyValueVO()
    {

    }
    public KeyValueVO(string str)
    {
        var segs = str.Split(":");
        if (segs.Length != 2)
            throw new Exception("KeyValueVO needs an input like \"key:value\" which is not provided correctly");
        Key = segs[0];
        Value = segs[1];
    }
    public KeyValueVO(string key, string value)
    {
        Key = key;
        Value = value;
    }
    public string Key { get; }
    public string Value { get; }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Key;
        yield return Value;
    }
    public static implicit operator string(KeyValueVO vo)
    {
        return $"{vo.Key}:{vo.Value}";
    }
    public override string ToString()
    {
        return this;
    }

}
public enum KycStatusEnum
{
    [Description("اطلاعات شخصی")]
    PersonalInfoConfirmed = 1,
    [Description("اطلاعات شرکت")]
    CompanyInfoConfirmed = 2,
    [Description("اطلاعات بانکی")]
    BankAccountInfoConfirmed = 3,
    [Description("اطلاعات تماس")]
    ContactInfoConfirmed = 4,
    [Description("کارت ملی")]
    NationalCardConfirmed = 5,
    [Description("شناسنامه")]
    IdentityCardConfirmed = 6,
    [Description("ارزش افزوده")]
    VatDocumentConfirmed = 7,
    [Description("روزنامه رسمی")]
    NewsPaperDocumentConfirmed = 8
}