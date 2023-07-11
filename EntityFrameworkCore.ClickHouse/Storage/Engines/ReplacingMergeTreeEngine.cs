﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ClickHouse.EntityFrameworkCore.Storage.Engines;
public class ReplacingMergeTreeEngine : BaseMergeTreeEngine
{
    public ReplacingMergeTreeEngine([NotNull] string orderBy) :base(orderBy)
    {
    }

    public override string EngineType => ClickHouseEngineTypeConstants.ReplacingMergeTreeEngine;
   

    public override string Serialize()
    {
        var res = JsonSerializer.Serialize(this);
        return res;
    }

    public override void SpecifyEngine(MigrationCommandListBuilder builder, IModel model)
    {
        builder.Append(" ENGINE = ReplacingMergeTree()").AppendLine();

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
    }

}