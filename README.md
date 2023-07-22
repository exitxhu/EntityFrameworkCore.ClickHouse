# ClickHouse provider for Entity Framework Core

* All schemas will set to null on model creating, if you want to use Schemas (which will consider as Database prefix in clickhouse) you need to set them manually after base.OnModelCreating()

* add, drop and rename column not supported by some table engine type, so the only way is to recreate them, u need to use [ClickHouseTable(TableCreationStrategy.CREATE_OR_REPLACE)] attribute on the entity.

* to specify engine type you can use these methods in onModelCreating :
1. HasMergeTreeEngine()
2. HasReplacingMergeTreeEngine()
3. HasStripeLogEngine()
4. HasPostGresEngine()

like this:
`var user = modelBuilder.Entity<User>();`
`user.HasPostGresEngine("User", "Accounting");`

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