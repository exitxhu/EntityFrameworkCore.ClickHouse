using ClickHouse.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        Debugger.Launch();
        var entityTypes = modelBuilder.Model.GetEntityTypes();
        foreach (var entityType in entityTypes)
        {
            entityType.SetSchema(null);
            var t = entityType.ClrType.GetCustomAttribute<ClickHouseTableAttribute>()
                ?? new ClickHouseTableAttribute(TableCreationStrategy.CREATE);
            entityType.SetOrRemoveAnnotation(nameof(ClickHouseTableAttribute), t);
            var ttt = entityType.GetNavigations().ToList();
            foreach (var nav in ttt)
            {
                var a = entityTypes.FirstOrDefault(a => a.Name == nav.Name);
                if (nav.IsOnDependent && a is null)
                    entityType.AddIgnored(nav.Name);

            }
        }
    }
}
