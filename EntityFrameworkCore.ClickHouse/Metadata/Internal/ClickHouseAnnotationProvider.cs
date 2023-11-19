using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ClickHouse.EntityFrameworkCore.Metadata.Internal
{
    public class ClickHouseAnnotationProvider : RelationalAnnotationProvider
    {
        public ClickHouseAnnotationProvider(RelationalAnnotationProviderDependencies dependencies) : base(dependencies)
        {
        }

        public override IEnumerable<IAnnotation> For(ITable table, bool designTime)
        {

            var entityType = table.EntityTypeMappings.First().TypeBase.ContainingEntityType;

            foreach (var annotation in entityType.GetAnnotations()
                .Where(e => e.Name.StartsWith(ClickHouseAnnotationNames.Prefix)))
            {
                yield return annotation;
            }
        }
    }
}