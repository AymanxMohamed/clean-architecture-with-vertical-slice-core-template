using System.Data.SqlClient;

using Microsoft.EntityFrameworkCore;

using Npgsql;

namespace ProjectName.Infrastructure.Persistence.Common.Settings;

public class DatabaseConfigurations
{
    public const string SectionName = "DatabaseConfigurations";

    public bool SqlServerEnabled { get; init; }
   
    public string Host { get; init; } = null!;

    public string DatabaseName { get; init; } = null!;

    public string UserId { get; init; } = null!;

    public string Password { get; init; } = null!;

    public int ConnectionTimeout { get; init; }

    public bool Encrypt { get; init; }
    
    public string ConnectionString => SqlServerEnabled ? SqlServerConnectionString : PostgresqlConnectionString;
    
    public string MigrationAssembly => SqlServerEnabled 
        ? $"{CurrentAssemblyName}.SqlServer" 
        : $"{CurrentAssemblyName}.Postgres";

    private string CurrentAssemblyName => ProjectNameInfrastructurePersistenceAssemblyMarker.Assembly.GetName().Name!;

    private string SqlServerConnectionString => new SqlConnectionStringBuilder
    {
        DataSource = Host,
        InitialCatalog = DatabaseName,
        UserID = UserId,
        Password = Password,
        ConnectTimeout = ConnectionTimeout,
        Encrypt = Encrypt
    }.ConnectionString;

    private string PostgresqlConnectionString => new NpgsqlConnectionStringBuilder
    {
        Host = Host,
        Database = DatabaseName,
        Username = UserId,
        Password = Password,
        Timeout = ConnectionTimeout,
        SslMode = Encrypt ? SslMode.Require : SslMode.Disable
    }.ConnectionString;
}