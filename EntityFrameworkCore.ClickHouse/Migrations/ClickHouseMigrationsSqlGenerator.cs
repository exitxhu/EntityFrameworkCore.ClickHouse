using ClickHouse.EntityFrameworkCore.Metadata;
using ClickHouse.EntityFrameworkCore.Migrations.Operations;
using ClickHouse.EntityFrameworkCore.Storage.Engines;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System;
using System.Linq;

namespace ClickHouse.EntityFrameworkCore.Migrations;

public class ClickHouseMigrationsSqlGenerator : MigrationsSqlGenerator
{
    public ClickHouseMigrationsSqlGenerator(MigrationsSqlGeneratorDependencies dependencies) : base(dependencies)
    {
    }

    protected override void ColumnDefinition(string schema, string table, string name, ColumnOperation operation, IModel model,
        MigrationCommandListBuilder builder)
    {
        var columnType = operation.ColumnType ?? GetColumnType(schema, table, name, operation, model);
        builder
            .Append(Dependencies.SqlGenerationHelper.DelimitIdentifier(name))
            .Append(" ")
            .Append(operation.IsNullable && !operation.ClrType.IsArray ? $" Nullable({columnType})" : columnType);
    }
    protected override void Generate(EnsureSchemaOperation operation, IModel model, MigrationCommandListBuilder builder)
    {

    }
    protected override void Generate(MigrationOperation operation, IModel model, MigrationCommandListBuilder builder)
    {
        if (operation is ClickHouseCreateDatabaseOperation cdo)
        {
            Generate(cdo, model, builder);
            return;
        }

        if (operation is ClickHouseDropDatabaseOperation ddo)
        {
            Generate(ddo, model, builder);
            return;
        }

        base.Generate(operation, model, builder);
    }

    protected virtual void Generate(ClickHouseCreateDatabaseOperation operation, IModel model, MigrationCommandListBuilder builder)
    {
        builder
            .Append("CREATE DATABASE ")
            .Append(Dependencies.SqlGenerationHelper.DelimitIdentifier(operation.Name))
            .AppendLine(";");
        EndStatement(builder, true);
    }

    protected virtual void Generate(ClickHouseDropDatabaseOperation operation, IModel model, MigrationCommandListBuilder builder)
    {
        builder.Append("DROP DATABASE ")
            .Append(Dependencies.SqlGenerationHelper.DelimitIdentifier(operation.Name))
            .AppendLine(";");
        EndStatement(builder, true);
    }

    protected override void CreateTableConstraints(CreateTableOperation operation, IModel model, MigrationCommandListBuilder builder)
    {
        CreateTableCheckConstraints(operation, model, builder);
    }

    protected override void Generate(CreateTableOperation operation, IModel model, MigrationCommandListBuilder builder, bool terminate = true)
    {


        var models = model.GetEntityTypes();
        var thisType = models.FirstOrDefault(a => a.GetTableName() == operation.Name);
        var thisAnnotaions = thisType.GetAnnotations().ToList();
        var createAnnotation = thisAnnotaions?.FirstOrDefault(a => a.Name == nameof(ClickHouseTableCreationStrategyAttribute))?.Value 
            as ClickHouseTableCreationStrategyAttribute;
        // Enum.TryParse<TableCreationStrategy>(createAnnotation, out var createAttr);
        string statement = createAnnotation?.Strategy switch
        {
            TableCreationStrategy.CREATE_IF_NOT_EXISTS => "CREATE TABLE IF NOT EXISTS ",
            TableCreationStrategy.CREATE_OR_REPLACE => "CREATE OR REPLACE TABLE ",
            TableCreationStrategy.CREATE => "CREATE TABLE ",
            _ => "CREATE TABLE IF NOT EXISTS ",
        };

        builder
            .Append(statement)
            .Append(Dependencies.SqlGenerationHelper.DelimitIdentifier(operation.Name))
            .AppendLine(" (");

        using (builder.Indent())
        {
            CreateTableColumns(operation, model, builder);
            CreateTableConstraints(operation, model, builder);
            builder.AppendLine();
        }

        builder.Append(")");

        var engineAnnotation = thisType.GetAnnotations()?.FirstOrDefault(a => a.Name.EndsWith(ClickHouseAnnotationNames.Engine));
        var engine = engineAnnotation != null && engineAnnotation.Value != null
            ? ClickHouseEngine.Deserialize(engineAnnotation.Value.ToString(), engineAnnotation.Name)
            : new StripeLogEngine();

        engine.SpecifyEngine(builder, model);
        builder.AppendLine(Dependencies.SqlGenerationHelper.StatementTerminator);
        EndStatement(builder);
    }
    protected override void Generate(RenameColumnOperation operation, IModel model, MigrationCommandListBuilder builder)
    {
        base.Generate(operation, model, builder);
    }
}
