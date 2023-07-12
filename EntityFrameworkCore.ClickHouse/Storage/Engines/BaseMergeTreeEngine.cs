using System;
using System.Diagnostics.CodeAnalysis;

namespace ClickHouse.EntityFrameworkCore.Storage.Engines;

public abstract class BaseMergeTreeEngine : ClickHouseEngine
{

    public BaseMergeTreeEngine([NotNull] string orderBy)
    {
        if (orderBy == null)
        {
            throw new ArgumentNullException(nameof(orderBy));
        }

        OrderBy = orderBy;
    }

    [NotNull]
    public string OrderBy { get; set; }

    [AllowNull]
    public string PartitionBy { get; set; }

    [AllowNull]
    public string PrimaryKey { get; set; }

    [AllowNull]
    public string SampleBy { get; set; }

    [AllowNull]
    public MergeTreeSettings Settings { get; set; }


    public BaseMergeTreeEngine WithPartitionBy([NotNull] string partitionBy)
    {
        if (partitionBy == null)
        {
            throw new ArgumentNullException(nameof(partitionBy));
        }

        PartitionBy = partitionBy;
        return this;
    }

    public BaseMergeTreeEngine WithPrimaryKey([NotNull] string primaryKey)
    {
        if (primaryKey == null)
        {
            throw new ArgumentNullException(nameof(primaryKey));
        }

        PrimaryKey = primaryKey;
        return this;
    }

    public BaseMergeTreeEngine WithSampleBy([NotNull] string sampleBy)
    {
        if (sampleBy == null)
        {
            throw new ArgumentNullException(nameof(sampleBy));
        }

        SampleBy = sampleBy;
        return this;
    }

    public BaseMergeTreeEngine WithSettings([NotNull] Action<MergeTreeSettings> configure)
    {
        if (configure == null)
        {
            throw new ArgumentNullException(nameof(configure));
        }

        if (Settings == null)
        {
            Settings = new MergeTreeSettings();
        }

        configure(Settings);
        return this;
    }
}
