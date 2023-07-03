using System.Collections.Generic;
using ClickHouse.EntityFrameworkCore.Extensions;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace ClickHouse.EntityFrameworkCore.Infrastructure.Internal;

public class ClickHouseOptionsExtension : RelationalOptionsExtension
{
    private DbContextOptionsExtensionInfo _info;
    
    public ClickHouseOptionsExtension()
    {
    }

    protected ClickHouseOptionsExtension(ClickHouseOptionsExtension copyFrom) : base(copyFrom)
    {
    }

    protected override RelationalOptionsExtension Clone()
    {
        return new ClickHouseOptionsExtension(this);
    }

    public override void ApplyServices(IServiceCollection services)
    {
        services.AddEntityFrameworkClickHouse();
    }

    public virtual string SystemDataBase { get; set; } = "system";

    public override int? MaxBatchSize => 1;

    public override DbContextOptionsExtensionInfo Info => _info ??= new ExtensionInfo(this);

    private sealed class ExtensionInfo : RelationalExtensionInfo
    {
        public ExtensionInfo(IDbContextOptionsExtension extension)
            : base(extension)
        {
        }

        private new ClickHouseOptionsExtension Extension
            => (ClickHouseOptionsExtension)base.Extension;

        public override bool IsDatabaseProvider => true;

        public override string LogFragment => string.Empty;

        public override void PopulateDebugInfo(IDictionary<string, string> debugInfo)
        {
        }
    }
}
