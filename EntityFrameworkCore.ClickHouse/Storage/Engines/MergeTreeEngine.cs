using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ClickHouse.EntityFrameworkCore.Storage.Engines;
public class MergeTreeEngine : BaseMergeTreeEngine
{

    public override string EngineType => ClickHouseEngineTypeConstants.MergeTreeEngine;
    public MergeTreeEngine([NotNull] string orderBy) : base(orderBy)
    {
    }
    public MergeTreeEngine()
    {

    }

    public override string Serialize()
    {
        var res = JsonSerializer.Serialize(this);
        return res;
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
