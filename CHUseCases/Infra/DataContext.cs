using ClickHouse.EntityFrameworkCore.Extensions;
using ClickHouse.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CHUseCases.Infra;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> op) : base(op)
    {

    }
    protected DataContext()
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Balance> Balances { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var user = modelBuilder.Entity<User>()
    .HasCreateStrategy(TableCreationStrategy.CREATE_OR_REPLACE)
    .HasReplacingMergeTreeEngine()
    .HasPrimaryKey(a => a.Id);
        var balance = modelBuilder.Entity<Balance>()
    .HasCreateStrategy(TableCreationStrategy.CREATE_OR_REPLACE)
    .HasReplacingMergeTreeEngine()
    .HasPartitionBy(a => a.Date, PartitionByDateFormat.Month)
    .HasPrimaryKey(a => a.Id);
    }
}
[ClickHouseTable(TableCreationStrategy.CREATE)]
public class User
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public int Count { get; set; }
    public List<Balance> Balances { get; set; }
}

[ClickHouseTable(TableCreationStrategy.CREATE)]
public class Balance
{
    [Key]
    public int Id { get; set; }
    public int Count { get; set; }
    public User User { get; set; }
    public int UserId { get; set; }
    public DateTime Date { get; set; }
}