using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using ClickHouse.Client.ADO;
using ClickHouse.EntityFrameworkCore.Infrastructure.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace ClickHouse.EntityFrameworkCore.Storage.Internal
{
    public class ClickHouseRelationalConnection : RelationalConnection, IClickHouseRelationalConnection
    {
        public ClickHouseRelationalConnection(RelationalConnectionDependencies dependencies) : base(dependencies)
        {
        }

        protected override DbConnection CreateDbConnection()
        {
            return new ClickHouseDbConnection(ConnectionString);
        }

        public IClickHouseRelationalConnection CreateReadOnlyConnection()
        {
            throw new System.NotImplementedException();
        }

        public IClickHouseRelationalConnection CreateMasterConnection()
        {
            var systemDb = Dependencies.ContextOptions.FindExtension<ClickHouseOptionsExtension>()?.SystemDataBase ?? "system";

            var csb = new ClickHouseConnectionStringBuilder(ConnectionString)
            {
                Database = systemDb
            };

            var relationalOptions = RelationalOptionsExtension.Extract(Dependencies.ContextOptions);
            var connectionString = csb.ToString();
            relationalOptions = relationalOptions.WithConnectionString(connectionString);
            var optionsBuilder = new DbContextOptionsBuilder();
            ((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(relationalOptions);

            return new ClickHouseRelationalConnection(Dependencies);
        }

        public override IDbContextTransaction BeginTransaction(IsolationLevel isolationLevel)
        {
            return new ClickHouseTransaction();
        }

        public override IDbContextTransaction BeginTransaction()
        {
            return BeginTransaction(IsolationLevel.Unspecified);
        }

        public override async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return await BeginTransactionAsync(IsolationLevel.Unspecified, cancellationToken);
        }

        public override Task<IDbContextTransaction> BeginTransactionAsync(IsolationLevel isolationLevel,
            CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.FromResult<IDbContextTransaction>(new ClickHouseTransaction());
        }
    }
}
