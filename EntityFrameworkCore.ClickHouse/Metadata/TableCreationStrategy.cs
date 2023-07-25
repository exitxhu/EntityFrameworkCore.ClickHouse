namespace ClickHouse.EntityFrameworkCore.Metadata;

public enum TableCreationStrategy
{
    CREATE = 1,
    CREATE_IF_NOT_EXISTS,
    CREATE_OR_REPLACE
}