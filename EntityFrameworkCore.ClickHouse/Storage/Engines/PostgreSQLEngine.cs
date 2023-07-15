using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ClickHouse.EntityFrameworkCore.Storage.Engines;

public class PostgreSQLEngine : ClickHouseEngine
{
    public override string EngineType => ClickHouseEngineTypeConstants.PostgreSQLEngine;

    public PostgreSQLEngine(string table, string schema = null)
    {
        Table = table;
        Schema = schema;
    }
    [JsonIgnore]
    static internal PostgreSQLEngineOptions Options { get; set; }
    public string Table { get; }
    public string Schema { get; set; }

    public override string Serialize()
    {
        var res = JsonSerializer.Serialize(this);
        return res;
    }

    public override void SpecifyEngine(MigrationCommandListBuilder builder, IModel model)
    {
        builder.Append($" ENGINE = PostgreSQL('{Options.Host}', '{Options.DataBaseName}', '{Table}', '{Options.UserName}', '{Options.Password}'{(string.IsNullOrEmpty(Schema) ? "" : $", `{Schema}`")})").AppendLine();

    }
}
public class PostgreSQLEngineOptions
{
    public string Host { get; set; }
    public string DataBaseName { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
}