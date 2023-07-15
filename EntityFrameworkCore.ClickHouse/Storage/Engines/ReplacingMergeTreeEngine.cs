using System;
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
    public ReplacingMergeTreeEngine()
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
            builder.AppendLine($"PARTITION BY ({PartitionBy})");
        }

        if (PrimaryKey != null)
        {
            builder.AppendLine($"PRIMARY KEY ({PrimaryKey})");
        }

        if (SampleBy != null)
        {
            builder.AppendLine($"SAMPLE BY ({SampleBy})");
        }

        Settings.MergeTreeSettingBuilder(builder);
    }

}