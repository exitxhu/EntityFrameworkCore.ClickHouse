using ClickHouse.EntityFrameworkCore.Design.Internal;
using ClickHouse.EntityFrameworkCore.Extensions;
using ClickHouse.EntityFrameworkCore.Scaffolding.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Renci.SshNet;
using System;
using System.Diagnostics;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EntityFrameworkCore.ClickHouse.IntegrationTests;


public class ClickHouseContext : DbContext
{
    public ClickHouseContext()
    {
    }
    public DbSet<SimpleEntity> SimpleEntities { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SimpleEntity>().HasKey(e => e.Id);
        modelBuilder.Entity<SimpleEntity>().Property(e => e.Id).ValueGeneratedNever();

        modelBuilder.Entity<SimpleEntity>()
            .HasMergeTreeEngine()
            .HasOrderBy(a => a.Id);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.LogTo(TestContext.WriteLine);
        optionsBuilder.UseClickHouse($"Host=127.0.0.1;Protocol=http;Port=8443;Database=test;Username=affilio;Password=pcIslv0JbSjLGxV;");
        //   optionsBuilder.UseClickHouse("Host=localhost;Protocol=http;Port=8123;Database=" + TestContext.CurrentContext.Test.ClassName);

    }
}
public class SimpleEntity
{
    public const float FloatValue = 3.14f;
    public const double DoubleValue = 3.14;
    public static readonly DateTime DateTimeValue = DateTime.Today;
    public const sbyte Int8Value = 1;
    public const short Int16Value = 2;
    public const int Int32Value = 3;
    public const long Int64Value = 4;
    public const byte UInt8Value = 5;
    public const ushort UInt16Value = 6;
    public const uint UInt32Value = 7;
    public const ulong UInt64Value = 8;

    public int Id { get; set; }

    public string Text { get; set; }

    public string FloatAsString { get; set; } = FloatValue.ToString(CultureInfo.InvariantCulture);

    public string DoubleAsString { get; set; } = DoubleValue.ToString(CultureInfo.InvariantCulture);

    public string DateTimeAsString { get; set; } = DateTimeValue.ToString("yyyy-MM-dd");

    public string Int8AsString { get; set; } = Int8Value.ToString(CultureInfo.InvariantCulture);

    public string Int16AsString { get; set; } = Int16Value.ToString(CultureInfo.InvariantCulture);

    public string Int32AsString { get; set; } = Int32Value.ToString(CultureInfo.InvariantCulture);

    public string Int64AsString { get; set; } = Int64Value.ToString(CultureInfo.InvariantCulture);

    public string UInt8AsString { get; set; } = UInt8Value.ToString(CultureInfo.InvariantCulture);

    public string UInt16AsString { get; set; } = UInt16Value.ToString(CultureInfo.InvariantCulture);

    public string UInt32AsString { get; set; } = UInt32Value.ToString(CultureInfo.InvariantCulture);

    public string UInt64AsString { get; set; } = UInt64Value.ToString(CultureInfo.InvariantCulture);
}
