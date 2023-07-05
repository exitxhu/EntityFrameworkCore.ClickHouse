using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ClickHouse.EntityFrameworkCore.Storage.Engines;

public abstract class ClickHouseEngine
{
    public abstract void SpecifyEngine(MigrationCommandListBuilder builder, IModel model);
    public abstract string SpecifyEngine();
}
