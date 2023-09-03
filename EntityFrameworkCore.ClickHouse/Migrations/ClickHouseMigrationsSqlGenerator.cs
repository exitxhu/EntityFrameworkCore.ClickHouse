using ClickHouse.EntityFrameworkCore.Metadata;
using ClickHouse.EntityFrameworkCore.Migrations.Operations;
using ClickHouse.EntityFrameworkCore.Storage.Engines;
using ClickHouse.EntityFrameworkCore.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ClickHouse.EntityFrameworkCore.Migrations;
using System.Diagnostics;

namespace Microsoft.EntityFrameworkCore.Migrations.Internal;


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
    #region Skup Operation
    protected override void Generate(EnsureSchemaOperation operation, IModel model, MigrationCommandListBuilder builder)
    {
    }
    protected override void Generate(AddForeignKeyOperation operation, IModel model, MigrationCommandListBuilder builder, bool terminate = true)
    {
    }
    protected override void Generate(AddUniqueConstraintOperation operation, IModel model, MigrationCommandListBuilder builder)
    {
    }
    protected override void Generate(AddPrimaryKeyOperation operation, IModel model, MigrationCommandListBuilder builder, bool terminate = true)
    {
    }
    protected override void Generate(CreateIndexOperation operation, IModel model, MigrationCommandListBuilder builder, bool terminate = true)
    {
    }
    protected override void Generate(DropForeignKeyOperation operation, IModel model, MigrationCommandListBuilder builder, bool terminate = true)
    {
    }
    protected override void Generate(DropIndexOperation operation, IModel model, MigrationCommandListBuilder builder, bool terminate = true)
    {
    }
    protected override void Generate(DropPrimaryKeyOperation operation, IModel model, MigrationCommandListBuilder builder, bool terminate = true)
    {
    }
    protected override void Generate(DropUniqueConstraintOperation operation, IModel model, MigrationCommandListBuilder builder)
    {
    }
    #endregion
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
    protected override void Generate(AddColumnOperation operation, IModel model, MigrationCommandListBuilder builder, bool terminate = true)
    {
        var models = model.GetEntityTypes();
        var thisType = models.FirstOrDefault(a => a.GetTableName() == operation.Table);
        var thisAnnotaions = thisType.GetAnnotations().ToList();
        TableCreationStrategy createAnnotation = GetCreateStrategy(thisAnnotaions);
        var engineAnnotation = thisType.GetAnnotations()?.FirstOrDefault(a => a.Name.EndsWith(ClickHouseAnnotationNames.Engine));
        var engine = engineAnnotation != null && engineAnnotation.Value != null
            ? ClickHouseEngine.Deserialize(engineAnnotation.Value.ToString(), engineAnnotation.Name)
            : new StripeLogEngine();

        if (engine?.EngineType != ClickHouseEngineTypeConstants.MergeTreeEngine &&
            createAnnotation != TableCreationStrategy.CREATE_OR_REPLACE
            )
        {
            throw new InvalidOperationException($"Click house dont support Add/Drop column in table engin type of {engine.EngineType}, please specify create strategy of {TableCreationStrategy.CREATE_OR_REPLACE} on table, to recreate the tablek");
        }
        else if (engine?.EngineType != ClickHouseEngineTypeConstants.MergeTreeEngine &&
            createAnnotation == TableCreationStrategy.CREATE_OR_REPLACE)
        {
            CreateTableOperation createTableOperation = GetTableCreateModel(operation.Table, model);
            Generate(createTableOperation, model, builder);
            return;
        }
        else
            base.Generate(operation, model, builder, terminate);
    }
    protected override void Generate(RenameColumnOperation operation, IModel model, MigrationCommandListBuilder builder)
    {
        var models = model.GetEntityTypes();
        var thisType = models.FirstOrDefault(a => a.GetTableName() == operation.Table);
        var thisAnnotaions = thisType.GetAnnotations().ToList();
        TableCreationStrategy createAnnotation = GetCreateStrategy(thisAnnotaions);
        var engineAnnotation = thisType.GetAnnotations()?.FirstOrDefault(a => a.Name.EndsWith(ClickHouseAnnotationNames.Engine));
        var engine = engineAnnotation != null && engineAnnotation.Value != null
            ? ClickHouseEngine.Deserialize(engineAnnotation.Value.ToString(), engineAnnotation.Name)
            : new StripeLogEngine();

        if (engine?.EngineType != ClickHouseEngineTypeConstants.MergeTreeEngine &&
            createAnnotation != TableCreationStrategy.CREATE_OR_REPLACE
            )
        {
            throw new InvalidOperationException($"Click house dont support Add/Drop column in table engin type of {engine.EngineType}, please specify create strategy of {TableCreationStrategy.CREATE_OR_REPLACE} on table, to recreate the tablek");
        }
        else if (engine?.EngineType != ClickHouseEngineTypeConstants.MergeTreeEngine &&
            createAnnotation == TableCreationStrategy.CREATE_OR_REPLACE)
        {
            CreateTableOperation createTableOperation = GetTableCreateModel(operation.Table, model);
            Generate(createTableOperation, model, builder);
            return;
        }
        else
            base.Generate(operation, model, builder);
    }
    protected override void Generate(DropColumnOperation operation, IModel model, MigrationCommandListBuilder builder, bool terminate = true)
    {
        //PostgresTable needs to be recreated

        var models = model.GetEntityTypes();
        var thisType = models.FirstOrDefault(a => a.GetTableName() == operation.Table);
        var thisAnnotaions = thisType.GetAnnotations().ToList();
        TableCreationStrategy createAnnotation = GetCreateStrategy(thisAnnotaions);
        var engineAnnotation = thisType.GetAnnotations()?.FirstOrDefault(a => a.Name.EndsWith(ClickHouseAnnotationNames.Engine));
        var engine = engineAnnotation != null && engineAnnotation.Value != null
            ? ClickHouseEngine.Deserialize(engineAnnotation.Value.ToString(), engineAnnotation.Name)
            : new StripeLogEngine();

        if (engine?.EngineType != ClickHouseEngineTypeConstants.MergeTreeEngine &&
            createAnnotation != TableCreationStrategy.CREATE_OR_REPLACE
            )
        {
            throw new InvalidOperationException($"Click house dont support Add/Drop column in table engin type of {engine.EngineType}, please specify create strategy of {TableCreationStrategy.CREATE_OR_REPLACE} on table, to recreate the tablek");
        }
        else if (engine?.EngineType != ClickHouseEngineTypeConstants.MergeTreeEngine &&
            createAnnotation == TableCreationStrategy.CREATE_OR_REPLACE)
        {
            CreateTableOperation createTableOperation = GetTableCreateModel(operation.Table, model);
            Generate(createTableOperation, model, builder);
        }
        else
            base.Generate(operation, model, builder);

    }

    private CreateTableOperation GetTableCreateModel(string name, IModel model, string schema = null)
    {
        var t = model.GetRelationalModel().Tables.FirstOrDefault(t => t.Name == name);
        var createTableOperation = new CreateTableOperation
        {
            Schema = schema,
            Name = name,
        };
        createTableOperation.AddAnnotations(t.GetAnnotations());

        createTableOperation.Columns.AddRange(
            GetSortedColumns(t).SelectMany(p => Add(p, inline: true)).Cast<AddColumnOperation>());

        var primaryKey = t.PrimaryKey;
        if (primaryKey != null)
        {
            createTableOperation.PrimaryKey = Add(primaryKey).Cast<AddPrimaryKeyOperation>().Single();
        }

        createTableOperation.UniqueConstraints.AddRange(
            t.UniqueConstraints.Where(c => !c.GetIsPrimaryKey()).SelectMany(c => Add(c))
                .Cast<AddUniqueConstraintOperation>());
        createTableOperation.CheckConstraints.AddRange(
            t.CheckConstraints.SelectMany(c => Add(c))
                .Cast<AddCheckConstraintOperation>());
        return createTableOperation;
    }

    static TableCreationStrategy GetCreateStrategy(List<Infrastructure.IAnnotation> thisAnnotaions)
    {
        Enum.TryParse<TableCreationStrategy>(thisAnnotaions?.FirstOrDefault(a => a.Name == nameof(ClickHouseTableAttribute))?.Value.ToString(), out var createAnnotation);
        return createAnnotation;
    }
    protected override void Generate(AlterTableOperation operation, IModel model, MigrationCommandListBuilder builder)
    {
        base.Generate(operation, model, builder);
    }
    protected override void Generate(AlterColumnOperation operation, IModel model, MigrationCommandListBuilder builder)
    {
        var models = model.GetEntityTypes();
        var thisType = models.FirstOrDefault(a => a.GetTableName() == operation.Table);
        var thisAnnotaions = thisType.GetAnnotations().ToList();
        TableCreationStrategy createAnnotation = GetCreateStrategy(thisAnnotaions);
        var engineAnnotation = thisType.GetAnnotations()?.FirstOrDefault(a => a.Name.EndsWith(ClickHouseAnnotationNames.Engine));
        var engine = engineAnnotation != null && engineAnnotation.Value != null
            ? ClickHouseEngine.Deserialize(engineAnnotation.Value.ToString(), engineAnnotation.Name)
            : new StripeLogEngine();

        if (engine?.EngineType != ClickHouseEngineTypeConstants.MergeTreeEngine &&
            createAnnotation != TableCreationStrategy.CREATE_OR_REPLACE
            )
        {
            throw new InvalidOperationException($"Click house dont support alter column in table engin type of {engine.EngineType}, please specify create strategy of {TableCreationStrategy.CREATE_OR_REPLACE} on table, to recreate the table");
        }
        else if (engine?.EngineType != ClickHouseEngineTypeConstants.MergeTreeEngine &&
            createAnnotation == TableCreationStrategy.CREATE_OR_REPLACE)
        {
            CreateTableOperation createTableOperation = GetTableCreateModel(operation.Table, model);
            Generate(createTableOperation, model, builder);
            return;
        }
        else
            base.Generate(operation, model, builder);
    }
    protected override void Generate(CreateTableOperation operation, IModel model, MigrationCommandListBuilder builder, bool terminate = true)
    {
        var models = model.GetEntityTypes();
        var thisType = models.FirstOrDefault(a => a.GetTableName() == operation.Name);
        var thisAnnotaions = thisType.GetAnnotations().ToList();
        TableCreationStrategy createAnnotation = GetCreateStrategy(thisAnnotaions);

        // Enum.TryParse<TableCreationStrategy>(createAnnotation, out var createAttr);
        string statement = createAnnotation switch
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
    protected override void Generate(RenameTableOperation operation, IModel model, MigrationCommandListBuilder builder)
    {
        var models = model.GetEntityTypes();
        var thisType = models.FirstOrDefault(a => a.GetTableName() == operation.NewName);
        var thisAnnotaions = thisType.GetAnnotations().ToList();
        TableCreationStrategy createAnnotation = GetCreateStrategy(thisAnnotaions);
        var engineAnnotation = thisType.GetAnnotations()?.FirstOrDefault(a => a.Name.EndsWith(ClickHouseAnnotationNames.Engine));
        var engine = engineAnnotation != null && engineAnnotation.Value != null
            ? ClickHouseEngine.Deserialize(engineAnnotation.Value.ToString(), engineAnnotation.Name)
            : new StripeLogEngine();

        if (engine?.EngineType != ClickHouseEngineTypeConstants.MergeTreeEngine &&
            createAnnotation != TableCreationStrategy.CREATE_OR_REPLACE
            )
        {
            throw new InvalidOperationException($"Click house dont support Add/Drop column in table engin type of {engine.EngineType}, please specify create strategy of {TableCreationStrategy.CREATE_OR_REPLACE} on table, to recreate the table");
        }
        else if (engine?.EngineType != ClickHouseEngineTypeConstants.MergeTreeEngine &&
            createAnnotation == TableCreationStrategy.CREATE_OR_REPLACE)
        {
            DropTableOperation dropTableOperation = new DropTableOperation()
            {
                Name = operation.Name,
            };
            Generate(dropTableOperation, model, builder);
            CreateTableOperation createTableOperation = GetTableCreateModel(operation.NewName, model);
            Generate(createTableOperation, model, builder);
        }
        else
            base.Generate(operation, model, builder);
    }

    private static IEnumerable<IColumn> GetSortedColumns(ITable table)
    {
        var columns = table.Columns.Where(x => x is not JsonColumn).ToHashSet();
        var sortedColumns = new List<IColumn>(columns.Count);
        foreach (var property in GetSortedProperties(GetMainType(table).GetRootType(), table))
        {
            var column = table.FindColumn(property)!;
            if (columns.Remove(column))
            {
                sortedColumns.Add(column);
            }
        }

        Check.DebugAssert(columns.Count == 0, "columns is not empty");

        // issue #28539
        // ideally we should inject JSON column in the place corresponding to the navigation that maps to it in the clr type
        var jsonColumns = table.Columns.Where(x => x is JsonColumn).OrderBy(x => x.Name);

        return sortedColumns.Where(c => c.Order.HasValue).OrderBy(c => c.Order)
            .Concat(sortedColumns.Where(c => !c.Order.HasValue))
            .Concat(columns)
            .Concat(jsonColumns);
    }

    protected virtual IEnumerable<MigrationOperation> Add(
        IColumn target,
        bool inline = false)
    {
        var table = target.Table;

        var operation = new AddColumnOperation
        {
            Schema = table.Schema,
            Table = table.Name,
            Name = target.Name
        };

        if (target is not JsonColumn)
        {
            if (!inline && target.Order.HasValue)
            {
                operation.AddAnnotation(RelationalAnnotationNames.ColumnOrder, target.Order.Value);
            }
        }

        yield return operation;
    }
    private static IEnumerable<IProperty> GetSortedProperties(IEntityType entityType, ITable table)
    {
        var leastPriorityProperties = new List<IProperty>();
        var leastPriorityPrimaryKeyProperties = new List<IProperty>();
        var primaryKeyPropertyGroups = new Dictionary<PropertyInfo, IProperty>();
        var groups = new Dictionary<PropertyInfo, List<IProperty>>();
        var unorderedGroups = new Dictionary<PropertyInfo, SortedDictionary<(int, string), IProperty>>();
        var types = new Dictionary<Type, SortedDictionary<int, PropertyInfo>>();

        foreach (var property in entityType.GetDeclaredProperties())
        {
            var clrProperty = property.PropertyInfo;
            if (clrProperty == null
                || clrProperty.IsIndexerProperty())
            {
                if (property.IsPrimaryKey())
                {
                    leastPriorityPrimaryKeyProperties.Add(property);

                    continue;
                }

                var foreignKey = property.GetContainingForeignKeys()
                    .FirstOrDefault(fk => fk.DependentToPrincipal?.PropertyInfo != null);
                if (foreignKey == null)
                {
                    leastPriorityProperties.Add(property);

                    continue;
                }

                clrProperty = foreignKey.DependentToPrincipal!.PropertyInfo!;
                var groupIndex = foreignKey.Properties.IndexOf(property);

                unorderedGroups.GetOrAddNew(clrProperty).Add((groupIndex, property.Name), property);
            }
            else
            {
                if (property.IsPrimaryKey())
                {
                    primaryKeyPropertyGroups.Add(clrProperty, property);
                }

                groups.Add(
                    clrProperty, new List<IProperty> { property });
            }

            var clrType = clrProperty.DeclaringType!;
            var index = clrType.GetTypeInfo().DeclaredProperties
                .IndexOf(clrProperty, PropertyInfoEqualityComparer.Instance);

            Check.DebugAssert(clrType != null, "clrType is null");
            types.GetOrAddNew(clrType)[index] = clrProperty;
        }

        foreach (var (propertyInfo, properties) in unorderedGroups)
        {
            groups.Add(propertyInfo, properties.Values.ToList());
        }

        if (table.EntityTypeMappings.Any(m => m.EntityType == entityType))
        {
            foreach (var linkingForeignKey in table.GetReferencingRowInternalForeignKeys(entityType))
            {
                // skip JSON entities, their properties are not mapped to anything
                if (linkingForeignKey.DeclaringEntityType.IsMappedToJson())
                {
                    continue;
                }

                var linkingNavigationProperty = linkingForeignKey.PrincipalToDependent?.PropertyInfo;
                var properties = GetSortedProperties(linkingForeignKey.DeclaringEntityType, table).ToList();
                if (linkingNavigationProperty == null
                    || (linkingForeignKey.PrincipalToDependent!.IsIndexerProperty()))
                {
                    leastPriorityProperties.AddRange(properties);

                    continue;
                }

                groups.Add(linkingNavigationProperty, properties);

                var clrType = linkingNavigationProperty.DeclaringType!;
                var index = clrType.GetTypeInfo().DeclaredProperties
                    .IndexOf(linkingNavigationProperty, PropertyInfoEqualityComparer.Instance);

                Check.DebugAssert(clrType != null, "clrType is null");
                types.GetOrAddNew(clrType)[index] = linkingNavigationProperty;
            }
        }

        var graph = new Multigraph<Type, object?>();
        graph.AddVertices(types.Keys);

        foreach (var left in types.Keys)
        {
            var found = false;
            foreach (var baseType in left.GetBaseTypes())
            {
                foreach (var right in types.Keys)
                {
                    if (right == baseType)
                    {
                        graph.AddEdge(right, left, null);
                        found = true;

                        break;
                    }
                }

                if (found)
                {
                    break;
                }
            }
        }

        var sortedPropertyInfos = graph.TopologicalSort().SelectMany(e => types[e].Values).ToList();

        return sortedPropertyInfos
            .Select(pi => primaryKeyPropertyGroups.ContainsKey(pi) ? primaryKeyPropertyGroups[pi] : null)
            // ReSharper disable once RedundantEnumerableCastCall
            .Where(e => e != null).Cast<IProperty>()
            .Concat(leastPriorityPrimaryKeyProperties)
            .Concat(
                sortedPropertyInfos
                    .Where(pi => !primaryKeyPropertyGroups.ContainsKey(pi) && entityType.ClrType.IsAssignableFrom(pi.DeclaringType))
                    .SelectMany(p => groups[p]))
            .Concat(leastPriorityProperties)
            .Concat(entityType.GetDirectlyDerivedTypes().SelectMany(et => GetSortedProperties(et, table)))
            .Concat(
                sortedPropertyInfos
                    .Where(pi => !primaryKeyPropertyGroups.ContainsKey(pi) && !entityType.ClrType.IsAssignableFrom(pi.DeclaringType))
                    .SelectMany(p => groups[p]));
    }
    private static IEntityType GetMainType(ITable table)
        => table.EntityTypeMappings.First(t => t.IsSharedTablePrincipal ?? true).EntityType;
    protected virtual IEnumerable<MigrationOperation> Add(IUniqueConstraint target)
    {
        if (target.GetIsPrimaryKey())
        {
            yield return AddPrimaryKeyOperation.CreateFrom((IPrimaryKeyConstraint)target);
        }
        else
        {
            yield return AddUniqueConstraintOperation.CreateFrom(target);
        }
    }
    protected virtual IEnumerable<MigrationOperation> Add(ICheckConstraint target)
    {
        yield return AddCheckConstraintOperation.CreateFrom(target);
    }
    private sealed class PropertyInfoEqualityComparer : IEqualityComparer<PropertyInfo>
    {
        private PropertyInfoEqualityComparer()
        {
        }

        public static readonly PropertyInfoEqualityComparer Instance = new();

        public bool Equals(PropertyInfo? x, PropertyInfo? y)
            => x.IsSameAs(y);

        public int GetHashCode(PropertyInfo obj)
            => throw new NotSupportedException();
    }

}