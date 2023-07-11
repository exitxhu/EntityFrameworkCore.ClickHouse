using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System.Collections.Generic;
namespace ClickHouse.EntityFrameworkCore.Migrations.Internal;

public class ClickHouseMigrationsAnnotationProvider : MigrationsAnnotationProvider
{
    public ClickHouseMigrationsAnnotationProvider(MigrationsAnnotationProviderDependencies dependencies)
        : base(dependencies)
    {
    }

    /// <inheritdoc />
    public override IEnumerable<IAnnotation> ForRemove(ITable table)
        => table.GetAnnotations();


    public override IEnumerable<IAnnotation> ForRename(ITable table)
    {
        return base.ForRemove(table);
    }

}
