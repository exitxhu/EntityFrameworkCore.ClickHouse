using ClickHouse.Client.ADO;
using System.Data.Common;
using System.Net.Http;

namespace ClickHouse.EntityFrameworkCore.Storage.Internal;

public class ClickHouseDbConnection : ClickHouseConnection
{
    public ClickHouseDbConnection()
    {
    }

    public ClickHouseDbConnection(string connectionString) : base(connectionString)
    {
    }

    public ClickHouseDbConnection(string connectionString, HttpClient httpClient) : base(connectionString, httpClient)
    {
    }

    public ClickHouseDbConnection(string connectionString, IHttpClientFactory httpClientFactory, string httpClientName = "") : base(connectionString, httpClientFactory, httpClientName)
    {
    }
}
