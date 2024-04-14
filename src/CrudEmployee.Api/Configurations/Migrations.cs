using CrudEmployee.Api.Utils;
using CrudEmployee.Infrastructure.Database.Migrations;
using FluentMigrator.Runner;

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
        MigrationsUtils.EnsureDatabaseCreated(app.Configuration, app.Logger);
        
        // Execute the migrations
        runner.MigrateUp();
    }
}