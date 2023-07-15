using System;
using System.Data.Common;
using ClickHouse.EntityFrameworkCore.Infrastructure;
using ClickHouse.EntityFrameworkCore.Infrastructure.Internal;
using ClickHouse.EntityFrameworkCore.Storage.Engines;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace ClickHouse.EntityFrameworkCore.Extensions;

public static class ClickHouseDbContextOptionsBuilderExtensions
{
    public static DbContextOptionsBuilder UseClickHouse(
        this DbContextOptionsBuilder optionsBuilder,
        Action<ClickHouseDbContextOptionsBuilder> clickHouseOptionsAction = null)
    {
        ((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(GetOrCreateExtension(optionsBuilder));
        ConfigureWarnings(optionsBuilder);
        clickHouseOptionsAction?.Invoke(new ClickHouseDbContextOptionsBuilder(optionsBuilder));
        return optionsBuilder;
    }
    
    public static DbContextOptionsBuilder UseClickHouse(
        this DbContextOptionsBuilder optionsBuilder,
        string connectionString,
        Action<ClickHouseDbContextOptionsBuilder> clickHouseOptionsAction = null,
        Action<PostgreSQLEngineOptions> postgresOpBuilder = null)
    {
        var extension = (ClickHouseOptionsExtension)GetOrCreateExtension(optionsBuilder)
            .WithConnectionString(connectionString);
        ((IDbContextOptionsBuilderInfrastructure)optionsBuilder)
            .AddOrUpdateExtension(extension);

        ConfigureWarnings(optionsBuilder);

        clickHouseOptionsAction?.Invoke(new ClickHouseDbContextOptionsBuilder(optionsBuilder));
        if(postgresOpBuilder != null)
        {
            PostgreSQLEngine.Options = new();
            postgresOpBuilder(PostgreSQLEngine.Options);
        }          
        return optionsBuilder;
    }
    
    public static DbContextOptionsBuilder UseClickHouse(
        this DbContextOptionsBuilder optionsBuilder,
        DbConnection connection,
        Action<ClickHouseDbContextOptionsBuilder> clickHouseOptionsAction = null)
    {
        var extension = (ClickHouseOptionsExtension)GetOrCreateExtension(optionsBuilder).WithConnection(connection);
        ((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(extension);

        ConfigureWarnings(optionsBuilder);

        clickHouseOptionsAction?.Invoke(new ClickHouseDbContextOptionsBuilder(optionsBuilder));

        return optionsBuilder;
    }
    
    public static DbContextOptionsBuilder<TContext> UseClickHouse<TContext>(
        this DbContextOptionsBuilder<TContext> optionsBuilder,
        Action<ClickHouseDbContextOptionsBuilder> clickHouseOptionsAction = null)
        where TContext : DbContext
        => (DbContextOptionsBuilder<TContext>)UseClickHouse(
            (DbContextOptionsBuilder)optionsBuilder, clickHouseOptionsAction);
    
    public static DbContextOptionsBuilder<TContext> UseClickHouse<TContext>(
        this DbContextOptionsBuilder<TContext> optionsBuilder,
        string connectionString,
        Action<ClickHouseDbContextOptionsBuilder> clickHouseOptionsAction = null)
        where TContext : DbContext
        => (DbContextOptionsBuilder<TContext>)UseClickHouse(
            (DbContextOptionsBuilder)optionsBuilder, connectionString, clickHouseOptionsAction);
    
    public static DbContextOptionsBuilder<TContext> UseClickHouse<TContext>(
        this DbContextOptionsBuilder<TContext> optionsBuilder,
        DbConnection connection,
        Action<ClickHouseDbContextOptionsBuilder> clickHouseOptionsAction = null)
        where TContext : DbContext
        => (DbContextOptionsBuilder<TContext>)UseClickHouse(
            (DbContextOptionsBuilder)optionsBuilder, connection, clickHouseOptionsAction);
    
    private static ClickHouseOptionsExtension GetOrCreateExtension(DbContextOptionsBuilder options)
        => options.Options.FindExtension<ClickHouseOptionsExtension>()
           ?? new ClickHouseOptionsExtension();
    
    private static void ConfigureWarnings(DbContextOptionsBuilder optionsBuilder)
    {
        var coreOptionsExtension
            = optionsBuilder.Options.FindExtension<CoreOptionsExtension>()
              ?? new CoreOptionsExtension();

        coreOptionsExtension = coreOptionsExtension.WithWarningsConfiguration(
            coreOptionsExtension.WarningsConfiguration.TryWithExplicit(
                RelationalEventId.AmbientTransactionWarning, WarningBehavior.Throw));

        ((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(coreOptionsExtension);
    }
}
