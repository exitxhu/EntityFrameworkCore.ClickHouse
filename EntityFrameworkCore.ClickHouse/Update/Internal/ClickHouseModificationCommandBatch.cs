using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using ClickHouse.Client.ADO.Readers;
using ClickHouse.EntityFrameworkCore.Storage.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Update;

namespace ClickHouse.EntityFrameworkCore.Update.Internal
{
    public class ClickHouseModificationCommandBatch : ReaderModificationCommandBatch
    {
        private const int DefaultBatchSize = 1;
        
        private int _maxBatchSize;

        private int _parameterCount;
        
        public ClickHouseModificationCommandBatch(ModificationCommandBatchFactoryDependencies dependencies, int? maxBatchSize) : base(dependencies)
        {
            _maxBatchSize = maxBatchSize ?? DefaultBatchSize;
        }

        public override bool TryAddCommand(IReadOnlyModificationCommand modificationCommand)
        {
            if (ModificationCommands.Count >= _maxBatchSize)
                return false;

            var newParamCount = (long)_parameterCount + modificationCommand.ColumnModifications.Count;
            if (newParamCount > int.MaxValue)
                return false;

            _parameterCount = (int)newParamCount;
            return true;
        }

        protected override bool IsValid() => true;

        protected override void Consume(RelationalDataReader reader)
        {
            var clickHouseReader = (ClickHouseDataReader)reader.DbDataReader;
            var commandIndex = 0;

            try
            {
                while (true)
                {
                    // Find the next propagating command, if any
                    int nextPropagating;
                    for (nextPropagating = commandIndex;
                        nextPropagating < ModificationCommands.Count 
                        //!ModificationCommands[nextPropagating].RequiresResultPropagation
                        ;
                        nextPropagating++) ;

                    if (nextPropagating == ModificationCommands.Count)
                    {
                        Debug.Assert(!clickHouseReader.NextResult(), "Expected less resultsets");
                        break;
                    }

                    // Propagate to results from the reader to the ModificationCommand

                    var modificationCommand = ModificationCommands[commandIndex++];

                    if (!reader.Read())
                    {
                        throw new DbUpdateConcurrencyException(
                            RelationalStrings.UpdateConcurrencyException(1, 0),
                            modificationCommand.Entries);
                    }

                    modificationCommand.PropagateResults(reader);

                    clickHouseReader.NextResult();
                }
            }
            catch (DbUpdateException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DbUpdateException(
                    RelationalStrings.UpdateStoreException,
                    ex,
                    ModificationCommands[commandIndex].Entries);
            }
        }

        protected override async Task ConsumeAsync(RelationalDataReader reader, CancellationToken cancellationToken = new CancellationToken())
        {
            var npgsqlReader = (ClickHouseDataReader)reader.DbDataReader;
            var commandIndex = 0;

            try
            {
                while (true)
                {
                    // Find the next propagating command, if any
                    int nextPropagating;
                    for (nextPropagating = commandIndex;
                        nextPropagating < ModificationCommands.Count;
                        nextPropagating++)
                        ;

                    if (nextPropagating == ModificationCommands.Count)
                    {
                        Debug.Assert(!(await npgsqlReader.NextResultAsync(cancellationToken)), "Expected less resultsets");
                        break;
                    }

                    // Extract result from the command and propagate it

                    var modificationCommand = ModificationCommands[commandIndex++];

                    if (!(await reader.ReadAsync(cancellationToken)))
                    {
                        throw new DbUpdateConcurrencyException(
                            RelationalStrings.UpdateConcurrencyException(1, 0),
                            modificationCommand.Entries
                        );
                    }

                    modificationCommand.PropagateResults(reader);

                    await npgsqlReader.NextResultAsync(cancellationToken);
                }
            }
            catch (DbUpdateException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DbUpdateException(
                    RelationalStrings.UpdateStoreException,
                    ex,
                    ModificationCommands[commandIndex].Entries);
            }
        }

    }
}
