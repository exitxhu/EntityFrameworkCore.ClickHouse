using System;

namespace ClickHouse.EntityFrameworkCore.Storage.Engines;

public class MergeTreeSettings
{
    public const int DefaultIndexGranularity = 8192;

    public const int DefaultIndexGranularityBytes = 10 * 1024 * 1024;

    public const int DefaultMinIndexGranularityBytes = 1024;

    public const bool DefaultEnableMixedGranularityParts = false;

    public const bool DefaultUseMinimalisticPartHeaderInZookeeper = false;

    public const long DefaultMinMergeBytesToUseDirectIo = 10L * 1024L * 1024L * 1024L;

    public static readonly TimeSpan DefaultMergeWithTtlTimeout = TimeSpan.FromDays(1);

    public const bool DefaultWriteFinalMark = true;

    public const long DefaultMergeMaxBlockSize = 8192L;

    public int IndexGranularity { get; set; } = DefaultIndexGranularity;

    public int IndexGranularityBytes { get; set; } = DefaultIndexGranularityBytes;

    public int MinIndexGranularityBytes { get; set; } = DefaultMinIndexGranularityBytes;

    public bool EnableMixedGranularityParts { get; set; } = DefaultEnableMixedGranularityParts;

    public bool UseMinimalisticPartHeaderInZookeeper { get; set; } = DefaultUseMinimalisticPartHeaderInZookeeper;

    public long MinMergeBytesToUseDirectIo { get; set; } = DefaultMinMergeBytesToUseDirectIo;

    public TimeSpan MergeWithTtlTimeout { get; set; } = DefaultMergeWithTtlTimeout;

    public bool WriteFinalMark { get; set; }

    public long MergeMaxBlockSize { get; set; } = DefaultMergeMaxBlockSize;

    public string StoragePolicy { get; set; }

    public long? MinBytesForWidePart { get; set; }

    public long? MinRowsForWidePart { get; set; }

    public long? MaxPartsInTotal { get; set; }

    public long? MaxCompressBlockSize { get; set; }

    public long? MinCompressBlockSize { get; set; }

    public bool IsDefault =>
        IndexGranularity == DefaultIndexGranularity &&
        IndexGranularityBytes == DefaultIndexGranularityBytes &&
        MinIndexGranularityBytes == DefaultMinIndexGranularityBytes &&
        EnableMixedGranularityParts == DefaultEnableMixedGranularityParts &&
        UseMinimalisticPartHeaderInZookeeper == DefaultUseMinimalisticPartHeaderInZookeeper &&
        MinMergeBytesToUseDirectIo == DefaultMinMergeBytesToUseDirectIo &&
        MergeWithTtlTimeout == DefaultMergeWithTtlTimeout &&
        WriteFinalMark != DefaultWriteFinalMark &&
        MergeMaxBlockSize == DefaultMergeMaxBlockSize &&
        string.IsNullOrEmpty(StoragePolicy) &&
        MinBytesForWidePart == null &&
        MinRowsForWidePart == null &&
        MaxPartsInTotal == null &&
        MaxCompressBlockSize == null &&
        MinCompressBlockSize == null;
}
