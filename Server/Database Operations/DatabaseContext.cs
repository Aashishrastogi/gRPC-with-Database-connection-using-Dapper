using Microsoft.Data.SqlClient;

namespace Server.Database_Operations;

using Dapper;

public class DatabaseContext
{
    private readonly IConfiguration _config;
    private readonly ILogger<DatabaseContext> _log;


    public DatabaseContext(ILogger<DatabaseContext> log, IConfiguration config)
    {
        _log = log;
        _config = config;
    }

    public IEnumerable<T> Query<T>(string sql, object parameters = null)
    {
        using var connection = CreateConnection();
        return connection.Query<T>(sql, parameters);
    }

    public int Execute(string sql, object parameters = null)
    {
        using var connection = CreateConnection();
        return connection.Execute(sql, parameters);
    }

    private SqlConnection CreateConnection()
    {
        return new SqlConnection(_config.GetValue<string>("connectionstrings"));
    }

    public bool Preprocessing_Database(int operations)
    {
        switch (operations)
        {
            case 1:
                _log.LogInformation("Database cleaned as requested by the User");
                return true;
            default:
                return false;
        }
    }
}