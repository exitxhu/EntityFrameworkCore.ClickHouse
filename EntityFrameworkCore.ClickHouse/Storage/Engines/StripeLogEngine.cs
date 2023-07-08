using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System.Text.Json;

namespace ClickHouse.EntityFrameworkCore.Storage.Engines;

public class StripeLogEngine : ClickHouseEngine
{
    public override string EngineType => ClickHouseEngineTypeConstants.StripeLogEngine;

    public override void SpecifyEngine(MigrationCommandListBuilder builder, IModel model)
    {
        builder.AppendLine(" ENGINE = StripeLog;");
    }

    public override string Serialize()
    {
        return JsonSerializer.Serialize(this);
    }

}
