# ClickHouse provider for Entity Framework Core

* Support Migrations.
  
* All schemas will set to null on model creating, if you want to use Schemas (which will consider as Database prefix in clickhouse) you need to set them manually after base.OnModelCreating()

* add, drop and rename column not supported by some table engine type, so the only way is to recreate them, u need to use .HasCreateStrategy(TableCreationStrategy.CREATE_OR_REPLACE) FluentApi on the entityBuilder.

* to specify engine type you can use these methods in onModelCreating :
1. HasMergeTreeEngine()
2. HasReplacingMergeTreeEngine()
3. HasStripeLogEngine()
4. HasPostGresEngine()

like this:
`var user = modelBuilder.Entity<User>();`
`user.HasPostGresEngine("User", "Accounting");` 

- if you dont provide any argument for HasPostGresEngine(), it will invoke GetTableName() and GetSchema() to map.

* To apply basic options for merge tree falmily use these:

1. HasOrderBy(a=> new{a.ID, a.Date})
2. HasPartitionBy(a=> new{a.ID, a.Date}) 
3. HasPrimaryKey(a=> new{a.ID, a.Date})
4. HasSampleBy(a=> new{a.ID, a.Date})

you can set partitioning expression as string as well.
and if you partition by a date/datetime column you can use PartitionByDateFormat enum to further config it:

`HasPartitionBy(a=> a.Date,PartitionByDateFormat.Month)`
which is : 
`toYYYYMM(Date)`

you can chain them as well.

* to Configure advance setting for merge tree family you can use this:

`        user.HasMergeTreeEngine(a =>
        {
            a.Settings = new();
            a.Settings.MergeMaxBlockSize = 2048;
            a.Settings.IndexGranularityBytes = 1024;
            a.Settings.MinRowsForWidePart = 512;
        });`


* to configure entity types dynamically you can use these extension methods:

        var entityTypes = modelBuilder.Model.GetEntityTypes();
        foreach (var entityType in entityTypes)
        {
            entityType.HasCreateStrategy(TableCreationStrategy.CREATE);
            entityType.HasPostGresEngine();
        }


# Limitations:
* Clickhouse dont suport some fimiliar syntaxes (atleast right now) like putting condition in subqueries:
`
select a.name,(select sum(b.count) from b where b.id = a.id) from a
`
which means you cannot use
` context.A.Select(a=>new {a.name, count = a.Bs.Sum(b=>b.count)})`
