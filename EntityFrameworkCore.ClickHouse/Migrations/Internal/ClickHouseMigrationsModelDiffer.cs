using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Update.Internal;
using Microsoft.EntityFrameworkCore.Update;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Migrations.Internal;
using ClickHouse.EntityFrameworkCore.Utilities;
using Microsoft.EntityFrameworkCore.Utilities;
namespace ClickHouse.EntityFrameworkCore.Migrations.Internal;

public class ClickHouseMigrationsModelDiffer : MigrationsModelDiffer
{
    public ClickHouseMigrationsModelDiffer(IRelationalTypeMappingSource typeMappingSource, IMigrationsAnnotationProvider migrationsAnnotationProvider, IRowIdentityMapFactory rowIdentityMapFactory, CommandBatchPreparerDependencies commandBatchPreparerDependencies) : base(typeMappingSource, migrationsAnnotationProvider, rowIdentityMapFactory, commandBatchPreparerDependencies)
    {
    }
    protected override IEnumerable<MigrationOperation> Diff(ICheckConstraint source, ICheckConstraint target, DiffContext diffContext)
    {
        return base.Diff(source, target, diffContext);
    }
    protected override IEnumerable<MigrationOperation> Diff(IEnumerable<ITable> source, IEnumerable<ITable> target, DiffContext diffContext)
    {
        return base.Diff(source, target, diffContext);
    }
}