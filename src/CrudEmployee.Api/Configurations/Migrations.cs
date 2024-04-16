using CrudEmployee.Api.Utils;
using CrudEmployee.Infrastructure.Database.Migrations;
using FluentMigrator.Runner;
using Npgsql;

namespace CrudEmployee.Api.Configurations;

public static class Migrations
{
    public static void RunMigrations(this WebApplication app)
    {
        using var serviceProvider = CreateServices(app.Configuration);
        using var scope = serviceProvider.CreateScope();
        UpdateDatabase(scope.ServiceProvider, app);
    }

    static ServiceProvider CreateServices(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("employeecrud");
        return new ServiceCollection()
            // Add common FluentMigrator services
            .AddFluentMigratorCore()
            .ConfigureRunner(rb => rb
                // Add Postgress support to FluentMigrator
                .AddPostgres()
                // Set the connection string
                .WithGlobalConnectionString(connectionString)
                // Define the assembly containing the migrations
                .ScanIn(typeof(AddEmployeeTables).Assembly).For.Migrations())
            // Enable logging to console in the FluentMigrator way
            .AddLogging(lb => lb.AddFluentMigratorConsole())
            // Build the service provider
            .BuildServiceProvider(false);
    }

    /// <summary>
    /// Update the database
    /// </summary>
    private static void UpdateDatabase(IServiceProvider serviceProvider, WebApplication app)
    {
        var runner = serviceProvider.GetRequiredService<IMigrationRunner>();
        EnsureDatabaseCreated(app.Configuration, app.Logger);
        
        // Execute the migrations
        runner.MigrateUp();
    }
    
    private static void EnsureDatabaseCreated(IConfiguration configuration, ILogger logger)
    {
        var connectionStringMasterDb = configuration.GetConnectionString("postgresMasterDb");
        
        if (connectionStringMasterDb is null)
        {
            throw new ArgumentNullException(nameof(connectionStringMasterDb));
        }
        
        using NpgsqlConnection conn = new NpgsqlConnection(connectionStringMasterDb);
        var dbnameToBeCreated = "employeecrud";
        var dbExists = VerifyIfDbExists(dbnameToBeCreated, conn);

        if (dbExists) return;
        
        logger.LogInformation("Database don't exists, creating database");
        CreateDatabaseIfNotExists(dbnameToBeCreated, conn);
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

        return resultCommandDatab is not null;
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