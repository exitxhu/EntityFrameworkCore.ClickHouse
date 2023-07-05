using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ClickHouse.EntityFrameworkCore.Storage.Engines;

public class MergeTreeEngine<T> : ClickHouseEngine
{
    public MergeTreeEngine([NotNull] string orderBy)
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

    public MergeTreeEngine<T> WithPartitionBy([NotNull] string partitionBy)
    {
        if (partitionBy == null)
        {
            throw new ArgumentNullException(nameof(partitionBy));
        }

        PartitionBy = partitionBy;
        return this;
    }

    public MergeTreeEngine<T> WithPrimaryKey([NotNull] string primaryKey)
    {
        if (primaryKey == null)
        {
            throw new ArgumentNullException(nameof(primaryKey));
        }

        PrimaryKey = primaryKey;
        return this;
    }

    public MergeTreeEngine<T> WithSampleBy([NotNull] string sampleBy)
    {
        if (sampleBy == null)
        {
            throw new ArgumentNullException(nameof(sampleBy));
        }

        SampleBy = sampleBy;
        return this;
    }

    public MergeTreeEngine<T> WithSettings([NotNull] Action<MergeTreeSettings> configure)
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

    public override string SpecifyEngine()
    {
        var builder = new IndentedStringBuilder();
        builder.Append(" ENGINE = MergeTree()").AppendLine();

        if (OrderBy != null)
        {
            builder.AppendLine($"ORDER BY ({OrderBy})");
        }

        if (PartitionBy != null)
        {
            builder.AppendLine($"$PARTITION BY ({PartitionBy})");
        }

        if (PrimaryKey != null)
        {
            builder.AppendLine($"PRIMARY KEY ({PrimaryKey})");
        }

        if (SampleBy != null)
        {
            builder.AppendLine($"SAMPLE BY ({SampleBy})");
        }

        if (Settings != null && !Settings.IsDefault)
        {
            builder.AppendLine("SETTINGS");

            using (builder.Indent())
            {
                if (Settings.IndexGranularity != MergeTreeSettings.DefaultIndexGranularity)
                {
                    builder.AppendLine("index_granularity = " + Settings.IndexGranularity);
                }

                if (Settings.IndexGranularityBytes != MergeTreeSettings.DefaultIndexGranularityBytes)
                {
                    builder.AppendLine("index_granularity_bytes = " + Settings.IndexGranularityBytes);
                }

                if (Settings.MinIndexGranularityBytes != MergeTreeSettings.DefaultMinIndexGranularityBytes)
                {
                    builder.AppendLine("min_index_granularity_bytes = " + Settings.MinIndexGranularityBytes);
                }

                if (Settings.EnableMixedGranularityParts != MergeTreeSettings.DefaultEnableMixedGranularityParts)
                {
                    builder.AppendLine("enable_mixed_granularity_parts = " + Convert.ToInt32(Settings.EnableMixedGranularityParts));
                }

                if (Settings.UseMinimalisticPartHeaderInZookeeper != MergeTreeSettings.DefaultUseMinimalisticPartHeaderInZookeeper)
                {
                    builder.AppendLine("use_minimalistic_part_header_in_zookeeper = " +
                                       Convert.ToInt32(Settings.UseMinimalisticPartHeaderInZookeeper));
                }

                if (Settings.MinMergeBytesToUseDirectIo != MergeTreeSettings.DefaultMinMergeBytesToUseDirectIo)
                {
                    builder.AppendLine("min_merge_bytes_to_use_direct_io = " + Settings.MinMergeBytesToUseDirectIo);
                }

                if (Settings.MergeWithTtlTimeout != MergeTreeSettings.DefaultMergeWithTtlTimeout)
                {
                    builder.AppendLine("merge_with_ttl_timeout = " + (int)Settings.MergeWithTtlTimeout.TotalSeconds);
                }

                if (Settings.WriteFinalMark != MergeTreeSettings.DefaultWriteFinalMark)
                {
                    builder.AppendLine("write_final_mark = " + Convert.ToInt32(Settings.WriteFinalMark));
                }

                if (Settings.MergeMaxBlockSize != MergeTreeSettings.DefaultMergeMaxBlockSize)
                {
                    builder.AppendLine("merge_max_block_size = " + Settings.MergeMaxBlockSize);
                }

                if (!string.IsNullOrEmpty(Settings.StoragePolicy))
                {
                    builder.AppendLine("storage_policy = " + Settings.StoragePolicy);
                }

                if (Settings.MinBytesForWidePart != null)
                {
                    builder.AppendLine("min_bytes_for_wide_part = " + Settings.MinBytesForWidePart.Value);
                }

                if (Settings.MinRowsForWidePart != null)
                {
                    builder.AppendLine("min_rows_for_wide_part = " + Settings.MinRowsForWidePart.Value);
                }

                if (Settings.MaxPartsInTotal != null)
                {
                    builder.AppendLine("max_parts_in_total = " + Settings.MaxPartsInTotal.Value);
                }

                if (Settings.MaxCompressBlockSize != null)
                {
                    builder.AppendLine("max_compress_block_size = " + Settings.MaxCompressBlockSize.Value);
                }

                if (Settings.MinCompressBlockSize != null)
                {
                    builder.AppendLine("min_compress_block_size = " + Settings.MinCompressBlockSize.Value);
                }
            }
        }
        return builder.ToString();
    }

    public override void SpecifyEngine(MigrationCommandListBuilder builder, IModel model)
    {
        builder.Append(" ENGINE = MergeTree()").AppendLine();

        if (OrderBy != null)
        {
            builder.AppendLine($"ORDER BY ({OrderBy})");
        }

        if (PartitionBy != null)
        {
            builder.AppendLine($"$PARTITION BY ({PartitionBy})");
        }

        if (PrimaryKey != null)
        {
            builder.AppendLine($"PRIMARY KEY ({PrimaryKey})");
        }

        if (SampleBy != null)
        {
            builder.AppendLine($"SAMPLE BY ({SampleBy})");
        }

        if (Settings != null && !Settings.IsDefault)
        {
            builder.AppendLine("SETTINGS");

            using (builder.Indent())
            {
                if (Settings.IndexGranularity != MergeTreeSettings.DefaultIndexGranularity)
                {
                    builder.AppendLine("index_granularity = " + Settings.IndexGranularity);
                }

                if (Settings.IndexGranularityBytes != MergeTreeSettings.DefaultIndexGranularityBytes)
                {
                    builder.AppendLine("index_granularity_bytes = " + Settings.IndexGranularityBytes);
                }

                if (Settings.MinIndexGranularityBytes != MergeTreeSettings.DefaultMinIndexGranularityBytes)
                {
                    builder.AppendLine("min_index_granularity_bytes = " + Settings.MinIndexGranularityBytes);
                }

                if (Settings.EnableMixedGranularityParts != MergeTreeSettings.DefaultEnableMixedGranularityParts)
                {
                    builder.AppendLine("enable_mixed_granularity_parts = " + Convert.ToInt32(Settings.EnableMixedGranularityParts));
                }

                if (Settings.UseMinimalisticPartHeaderInZookeeper != MergeTreeSettings.DefaultUseMinimalisticPartHeaderInZookeeper)
                {
                    builder.AppendLine("use_minimalistic_part_header_in_zookeeper = " +
                                       Convert.ToInt32(Settings.UseMinimalisticPartHeaderInZookeeper));
                }

                if (Settings.MinMergeBytesToUseDirectIo != MergeTreeSettings.DefaultMinMergeBytesToUseDirectIo)
                {
                    builder.AppendLine("min_merge_bytes_to_use_direct_io = " + Settings.MinMergeBytesToUseDirectIo);
                }

                if (Settings.MergeWithTtlTimeout != MergeTreeSettings.DefaultMergeWithTtlTimeout)
                {
                    builder.AppendLine("merge_with_ttl_timeout = " + (int) Settings.MergeWithTtlTimeout.TotalSeconds);
                }

                if (Settings.WriteFinalMark != MergeTreeSettings.DefaultWriteFinalMark)
                {
                    builder.AppendLine("write_final_mark = " + Convert.ToInt32(Settings.WriteFinalMark));
                }

                if (Settings.MergeMaxBlockSize != MergeTreeSettings.DefaultMergeMaxBlockSize)
                {
                    builder.AppendLine("merge_max_block_size = " + Settings.MergeMaxBlockSize);
                }

                if (!string.IsNullOrEmpty(Settings.StoragePolicy))
                {
                    builder.AppendLine("storage_policy = " + Settings.StoragePolicy);
                }

                if (Settings.MinBytesForWidePart != null)
                {
                    builder.AppendLine("min_bytes_for_wide_part = " + Settings.MinBytesForWidePart.Value);
                }

                if (Settings.MinRowsForWidePart != null)
                {
                    builder.AppendLine("min_rows_for_wide_part = " + Settings.MinRowsForWidePart.Value);
                }

                if (Settings.MaxPartsInTotal != null)
                {
                    builder.AppendLine("max_parts_in_total = " + Settings.MaxPartsInTotal.Value);
                }

                if (Settings.MaxCompressBlockSize != null)
                {
                    builder.AppendLine("max_compress_block_size = " + Settings.MaxCompressBlockSize.Value);
                }

                if (Settings.MinCompressBlockSize != null)
                {
                    builder.AppendLine("min_compress_block_size = " + Settings.MinCompressBlockSize.Value);
                }
            }
        }
    }
}
