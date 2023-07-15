using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using ClickHouse.EntityFrameworkCore.Metadata;
using ClickHouse.EntityFrameworkCore.Storage.Engines;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClickHouse.EntityFrameworkCore.Extensions;

public static class EntityTypeBuilderExtensions
{
    public static EntityTypeBuilder<T> HasMergeTreeEngine<T>(
    [NotNull] this EntityTypeBuilder<T> builder,
    [NotNull] Expression<Func<T, object>> orderBy,
    Action<MergeTreeEngine> configure = null)
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
        string result = orderBy.Body is MemberExpression mem
            ? mem.MemberObject()
            : orderBy.Body is NewExpression nex
                ? nex.AnynomousObject()
                : throw new Exception("Clickhouse table, Expression type is not valid in this context");

        return builder.HasMergeTreeEngine(result, configure);
    }
    public static EntityTypeBuilder<T> HasMergeTreeEngine<T>(
        [NotNull] this EntityTypeBuilder<T> builder,
        [NotNull] string orderBy,
        Action<MergeTreeEngine> configure = null) where T : class
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        if (orderBy == null)
        {
            throw new ArgumentNullException(nameof(orderBy));
        }

        if (configure == null)
        {
            throw new ArgumentNullException(nameof(configure));
        }

        var engine = new MergeTreeEngine(orderBy);
        if (configure != null)
            configure(engine);

        builder.Metadata.SetOrRemoveAnnotation(engine.EngineType, engine.Serialize());
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