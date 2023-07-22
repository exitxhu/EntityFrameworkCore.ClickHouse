using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using ClickHouse.EntityFrameworkCore.Metadata;
using ClickHouse.EntityFrameworkCore.Storage.Engines;
using ClickHouse.EntityFrameworkCore.Storage.Engines.Configur;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClickHouse.EntityFrameworkCore.Extensions;

public static class EntityTypeBuilderExtensions
{
    public static ClickHouseEntityMergeTreeConigurationBuilder<T> HasMergeTreeEngine<T>(
    [NotNull] this EntityTypeBuilder<T> builder,
    Action<MergeTreeEngine> configure = null)
        where T : class
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        var engine = new MergeTreeEngine();

        if (configure != null)
            configure(engine);
        builder.Metadata.SetOrRemoveAnnotation(engine.EngineType, engine.Serialize());

        return new()
        {
            Builder = builder,
            Engine = engine,
        };
    }
    public static ClickHouseEntityMergeTreeConigurationBuilder<T> HasReplacingMergeTreeEngine<T>(
   [NotNull] this EntityTypeBuilder<T> builder,
   Action<ReplacingMergeTreeEngine> configure = null)
       where T : class
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }


        var engine = new ReplacingMergeTreeEngine();

        if (configure != null)
            configure(engine);
        builder.Metadata.SetOrRemoveAnnotation(engine.EngineType, engine.Serialize());

        return new()
        {
            Builder = builder,
            Engine = engine,
        };
    }
    public static ClickHouseEntityMergeTreeConigurationBuilder<T> HasOrderBy<T, U>(
        [NotNull] this ClickHouseEntityMergeTreeConigurationBuilder<T> builder,
        [NotNull] Expression<Func<T, U>> orderBy)
   where T : class
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        if (orderBy == null)
        {
            throw new ArgumentNullException(nameof(orderBy));
        }
        string orderByString = orderBy.Body is MemberExpression mem
            ? mem.MemberObject()
            : orderBy.Body is NewExpression nex
                ? nex.AnynomousObject()
                : throw new Exception("Clickhouse table, Expression type is not valid in this context");
        builder.Engine.OrderBy = orderByString;
        builder.Builder.Metadata.SetOrRemoveAnnotation(builder.Engine.EngineType, builder.Engine.Serialize());
        return builder;
    }
    public static ClickHouseEntityMergeTreeConigurationBuilder<T> HasPartitionBy<T, U>(
        [NotNull] this ClickHouseEntityMergeTreeConigurationBuilder<T> builder,
        [NotNull] Expression<Func<T, U>> PartitionBy,
        PartitionByDateFormat? format = null
        )
   where T : class
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        if (PartitionBy == null)
        {
            throw new ArgumentNullException(nameof(PartitionBy));
        }
        string partitionByString = PartitionBy.Body is MemberExpression mem
            ? mem.MemberObject()
            : PartitionBy.Body is NewExpression nex
                ? nex.AnynomousObject()
                : throw new Exception("Clickhouse table, Expression type is not valid in this context");
        builder.Engine.PartitionBy = format switch
        {
            PartitionByDateFormat.Second => $"toYYYYMMDDhhmmss({partitionByString})",
            PartitionByDateFormat.Day => $"toYYYYMMDD({partitionByString})",
            PartitionByDateFormat.Month => $"toYYYYMM({partitionByString})",
            null => partitionByString
        };
        builder.Builder.Metadata.SetOrRemoveAnnotation(builder.Engine.EngineType, builder.Engine.Serialize());
        return builder;
    }
    public static ClickHouseEntityMergeTreeConigurationBuilder<T> HasPartitionBy<T, U>(
    [NotNull] this ClickHouseEntityMergeTreeConigurationBuilder<T> builder,
    string partitionByString
    )
where T : class
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        if (string.IsNullOrEmpty(partitionByString))
        {
            throw new ArgumentNullException(nameof(partitionByString));
        }

        builder.Engine.PartitionBy = partitionByString;
        builder.Builder.Metadata.SetOrRemoveAnnotation(builder.Engine.EngineType, builder.Engine.Serialize());
        return builder;
    }

    public static ClickHouseEntityMergeTreeConigurationBuilder<T> HasPrimaryKey<T, U>(
        [NotNull] this ClickHouseEntityMergeTreeConigurationBuilder<T> builder,
        [NotNull] Expression<Func<T, U>> PrimaryKey)
   where T : class
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        if (PrimaryKey == null)
        {
            throw new ArgumentNullException(nameof(PrimaryKey));
        }
        string PrimaryKeyString = PrimaryKey.Body is MemberExpression mem
            ? mem.MemberObject()
            : PrimaryKey.Body is NewExpression nex
                ? nex.AnynomousObject()
                : throw new Exception("Clickhouse table, Expression type is not valid in this context");
        builder.Engine.PrimaryKey = PrimaryKeyString;
        builder.Builder.Metadata.SetOrRemoveAnnotation(builder.Engine.EngineType, builder.Engine.Serialize());
        return builder;
    }
    public static ClickHouseEntityMergeTreeConigurationBuilder<T> HasSampleBy<T, U>(
        [NotNull] this ClickHouseEntityMergeTreeConigurationBuilder<T> builder,
        [NotNull] Expression<Func<T, U>> SampleBy)
   where T : class
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        if (SampleBy == null)
        {
            throw new ArgumentNullException(nameof(SampleBy));
        }
        string PrimaryKeyString = SampleBy.Body is MemberExpression mem
            ? mem.MemberObject()
            : SampleBy.Body is NewExpression nex
                ? nex.AnynomousObject()
                : throw new Exception("Clickhouse table, Expression type is not valid in this context");
        builder.Engine.SampleBy = PrimaryKeyString;
        builder.Builder.Metadata.SetOrRemoveAnnotation(builder.Engine.EngineType, builder.Engine.Serialize());
        return builder;
    }
    public static EntityTypeBuilder<T> HasStripeLogEngine<T>([NotNull] this EntityTypeBuilder<T> builder)
        where T : class
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        var engine = new StripeLogEngine();
        builder.Metadata.SetOrRemoveAnnotation(ClickHouseEngineTypeConstants.StripeLogEngine, engine);
        return builder;
    }

    public static EntityTypeBuilder<T> HasPostGresEngine<T>(
    [NotNull] this EntityTypeBuilder<T> builder,
    [NotNull] string TableName,
    string Schema = null)
    where T : class
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        var engine = new PostgreSQLEngine(
        TableName,
        Schema);

        builder.Metadata.SetOrRemoveAnnotation(engine.EngineType, engine.Serialize());

        return builder;
    }
    /// <summary>
    /// Get table and schema From builder.MetaData
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="builder"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static EntityTypeBuilder<T> HasPostGresEngine<T>(
[NotNull] this EntityTypeBuilder<T> builder)
where T : class
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        var engine = new PostgreSQLEngine(
        builder.Metadata.GetDefaultTableName(),
        builder.Metadata.GetSchema());

        builder.Metadata.SetOrRemoveAnnotation(engine.EngineType, engine.Serialize());

        return builder;
    }
}
internal static class ClickHouseEngineConfigExtensions
{
    internal static string MemberObject(this MemberExpression exp)
    {
        var des = exp.Member.Name;
        return des;
    }
    internal static string AnynomousObject(this NewExpression exp)
    {
        var des = string.Join(",", exp.Members.Select(n => n.Name));
        return des;
    }
}

public enum PartitionByDateFormat
{
    /// <summary>
    /// toYYYYMMDDhhmmss()
    /// </summary>
    Second,
    /// <summary>
    /// toYYYYMMDD()
    /// </summary>
    Day,
    /// <summary>
    /// toYYYYMM()
    /// </summary>
    Month,
}