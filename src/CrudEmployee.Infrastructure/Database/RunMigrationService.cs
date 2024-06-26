using CrudEmployee.Infrastructure.Database.Migrations;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace CrudEmployee.Infrastructure.Database;

public class RunMigrationService(ILogger<RunMigrationService> logger, string? connectionString, string? connectionStringMasterDb)
{
    private readonly ILogger<RunMigrationService> _logger = logger;
    private readonly string? _connectionString = connectionString ?? throw new ArgumentNullException(connectionString);
    private readonly string? _connectionStringMasterDb = connectionStringMasterDb ?? throw new ArgumentNullException(connectionStringMasterDb);

    public  void RunMigrations()
    {
        using var serviceProvider = CreateServices();
        using var scope = serviceProvider.CreateScope();
        UpdateDatabase(scope.ServiceProvider);
    }

    private ServiceProvider CreateServices()
    {
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
    private void UpdateDatabase(IServiceProvider serviceProvider)
    {
        var runner = serviceProvider.GetRequiredService<IMigrationRunner>();
        EnsureDatabaseCreated();
        runner.MigrateUp();
    }
    
    private void EnsureDatabaseCreated()
    {
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