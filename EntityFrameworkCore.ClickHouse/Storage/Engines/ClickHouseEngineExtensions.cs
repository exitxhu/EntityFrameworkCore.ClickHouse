using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Text.Json;

namespace ClickHouse.EntityFrameworkCore.Storage.Engines;

public static class ClickHouseEngineExtensions
{
    public static ClickHouseEngine DeserializeToProperType(this string ser, Type engineType)
    {
        var res = JsonSerializer.Deserialize(ser, engineType);

        return (ClickHouseEngine)res;

    }
    public static void MergeTreeSettingBuilder(this MergeTreeSettings settings, MigrationCommandListBuilder builder)
    {
        if (settings != null && !settings.IsDefault)
        {
            builder.AppendLine("SETTINGS");

            using (builder.Indent())
            {
                if (settings.IndexGranularity != MergeTreeSettings.DefaultIndexGranularity)
                {
                    builder.AppendLine("index_granularity = " + settings.IndexGranularity);
                }

                if (settings.IndexGranularityBytes != MergeTreeSettings.DefaultIndexGranularityBytes)
                {
                    builder.AppendLine("index_granularity_bytes = " + settings.IndexGranularityBytes);
                }

                if (settings.MinIndexGranularityBytes != MergeTreeSettings.DefaultMinIndexGranularityBytes)
                {
                    builder.AppendLine("min_index_granularity_bytes = " + settings.MinIndexGranularityBytes);
                }

                if (settings.EnableMixedGranularityParts != MergeTreeSettings.DefaultEnableMixedGranularityParts)
                {
                    builder.AppendLine("enable_mixed_granularity_parts = " + Convert.ToInt32(settings.EnableMixedGranularityParts));
                }

                if (settings.UseMinimalisticPartHeaderInZookeeper != MergeTreeSettings.DefaultUseMinimalisticPartHeaderInZookeeper)
                {
                    builder.AppendLine("use_minimalistic_part_header_in_zookeeper = " +
                                       Convert.ToInt32(settings.UseMinimalisticPartHeaderInZookeeper));
                }

                if (settings.MinMergeBytesToUseDirectIo != MergeTreeSettings.DefaultMinMergeBytesToUseDirectIo)
                {
                    builder.AppendLine("min_merge_bytes_to_use_direct_io = " + settings.MinMergeBytesToUseDirectIo);
                }

                if ( settings.MergeWithTtlTimeout != MergeTreeSettings.DefaultMergeWithTtlTimeout)
                {
                    builder.AppendLine("merge_with_ttl_timeout = " + (int)settings.MergeWithTtlTimeout.TotalSeconds);
                }

                if ( settings.WriteFinalMark != MergeTreeSettings.DefaultWriteFinalMark)
                {
                    builder.AppendLine("write_final_mark = " + Convert.ToInt32(settings.WriteFinalMark));
                }

                if (settings.MergeMaxBlockSize != MergeTreeSettings.DefaultMergeMaxBlockSize)
                {
                    builder.AppendLine("merge_max_block_size = " + settings.MergeMaxBlockSize);
                }

                if (!string.IsNullOrEmpty(settings.StoragePolicy))
                {
                    builder.AppendLine("storage_policy = " + settings.StoragePolicy);
                }

                if (settings.MinBytesForWidePart.HasValue)
                {
                    builder.AppendLine("min_bytes_for_wide_part = " + settings.MinBytesForWidePart.Value);
                }

                if (settings.MinRowsForWidePart.HasValue)
                {
                    builder.AppendLine("min_rows_for_wide_part = " + settings.MinRowsForWidePart.Value);
                }

                if (settings.MaxPartsInTotal.HasValue)
                {
                    builder.AppendLine("max_parts_in_total = " + settings.MaxPartsInTotal.Value);
                }

                if (settings.MaxCompressBlockSize.HasValue)
                {
                    builder.AppendLine("max_compress_block_size = " + settings.MaxCompressBlockSize.Value);
                }

                if (settings.MinCompressBlockSize.HasValue)
                {
                    builder.AppendLine("min_compress_block_size = " + settings.MinCompressBlockSize.Value);
                }
            }
        }
    }
}
