using ClickHouse.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ClickHouse.EntityFrameworkCore.Core;

public class ClickHouseDbContext : DbContext
{
    public ClickHouseDbContext(DbContextOptions options) : base(options)
    {
    }

    protected ClickHouseDbContext()
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //  Debugger.Launch();
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            entityType.SetSchema(null);
            var t = entityType.ClrType.GetCustomAttribute<ClickHouseTableCreationStrategyAttribute>()
                ?? new ClickHouseTableCreationStrategyAttribute(TableCreationStrategy.CREATE);
            entityType.SetOrRemoveAnnotation(nameof(ClickHouseTableCreationStrategyAttribute), t);
        }
    }
}
