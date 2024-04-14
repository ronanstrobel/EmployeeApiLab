using Npgsql;

namespace CrudEmployee.Api.Utils;

public static class MigrationsUtils
{
    public static void EnsureDatabaseCreated(IConfiguration configuration, ILogger logger)
    {
        var connectionStringMasterDb = configuration.GetConnectionString("postgresMasterDb");
        
        if (connectionStringMasterDb is null)
        {
            throw new ArgumentNullException(nameof(connectionStringMasterDb));
        }
        
        using NpgsqlConnection conn = new NpgsqlConnection(connectionStringMasterDb);
        var dbnameToBeCreated = "employeecrud";
        var dbExists = VerifyIfDbExists(dbnameToBeCreated, conn);

        if (dbExists)
        {
            logger.LogInformation("Database don't exists, creating database");
            CreateDatabaseIfNotExists(dbnameToBeCreated, conn);
        }
        
    }

    private static bool VerifyIfDbExists(string dbnameToBeCreated, NpgsqlConnection conn)
    {
        object? resultCommandDatab;
        string sql = $"SELECT DATNAME FROM pg_catalog.pg_database WHERE DATNAME = '{dbnameToBeCreated}'";
        using NpgsqlCommand command = new NpgsqlCommand(sql, conn);
        try
        {
            conn.Open();
            resultCommandDatab = command.ExecuteScalar();
            conn.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
            throw;
        }

        return resultCommandDatab is null;
    }
    
    private static void CreateDatabaseIfNotExists(string dbnameToBeCreated, NpgsqlConnection conn)
    {
        var createDatabaseCommand = $"CREATE DATABASE {dbnameToBeCreated}";
        using NpgsqlCommand command = new NpgsqlCommand(createDatabaseCommand, conn);
        try
        {
            conn.Open();
            command.ExecuteScalar();
            conn.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
            throw;
        }
    }
}