using Microsoft.Data.SqlClient;

namespace PlacementDashboard.Data;

public sealed class DbConnectionFactory
{
    private readonly string _connectionString;
    public DbConnectionFactory(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' was not found.");
    }
    public SqlConnection CreateConnection() => new(_connectionString);
}
