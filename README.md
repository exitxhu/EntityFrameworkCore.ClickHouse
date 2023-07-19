# ClickHouse provider for Entity Framework Core

* All schemas will set to null on model creating, if you want to use Schemas (which will consider as Database prefix in clickhouse) you need to set them manually after base.OnModelCreating()

* add, drop and rename column not supported by some table engine type, so the only way is to recreate them, u need to use [ClickHouseTable(TableCreationStrategy.CREATE_OR_REPLACE)] attribute on the entity.
