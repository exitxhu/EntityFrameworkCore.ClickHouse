# ClickHouse provider for Entity Framework Core

* All schemas will set to null on model creating, if you want to use Schemas (which will consider as Database prefix in clickhouse) you need to set them manually after base.OnModelCreating()