using System;
using System.Diagnostics.CodeAnalysis;
using ClickHouse.EntityFrameworkCore.Metadata;
using ClickHouse.EntityFrameworkCore.Storage.Engines;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClickHouse.EntityFrameworkCore.Extensions;

public static class EntityTypeBuilderExtensions
{
    public static EntityTypeBuilder<T> HasMergeTreeEngine<T>(
        [NotNull] this EntityTypeBuilder<T> builder,
        [NotNull] string orderBy) where T : class
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        if (orderBy == null)
        {
            throw new ArgumentNullException(nameof(orderBy));
        }

        return builder.HasMergeTreeEngine(orderBy, e => { });
    }

    public static EntityTypeBuilder<T> HasMergeTreeEngine<T>(
        [NotNull] this EntityTypeBuilder<T> builder,
        [NotNull] string orderBy,
        [NotNull] Action<MergeTreeEngine> configure) where T : class
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
        configure(engine);

        builder.Metadata.SetOrRemoveAnnotation(engine.EngineType, engine);
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
