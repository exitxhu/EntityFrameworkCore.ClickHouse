using ClickHouse.EntityFrameworkCore.Extensions;
using ClickHouse.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ClickHouse.EntityFrameworkCore.Core;

public class ClickHouseDbContext : DbContext
{
    public ClickHouseDbContext(DbContextOptions options) : base(options)
    {
    }

    protected ClickHouseDbContext()
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

    }
}
