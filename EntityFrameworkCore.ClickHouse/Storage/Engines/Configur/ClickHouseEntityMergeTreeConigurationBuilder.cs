using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClickHouse.EntityFrameworkCore.Storage.Engines.Configur;

public class ClickHouseEntityMergeTreeConigurationBuilder<T> where T : class
{
    public EntityTypeBuilder<T> Builder{ get; init; }
    public BaseMergeTreeEngine Engine { get; init; }
}
