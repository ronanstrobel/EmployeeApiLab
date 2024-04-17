using System.Data;
using System.Data.Common;
using CrudEmployee.Api;
using CrudEmployee.Infrastructure.Database;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Npgsql;
using Testcontainers.PostgreSql;

namespace CrudEmployeeIntegrationTests;

public class CustomWebApplicationFactory
    : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgreSqlContainer = new PostgreSqlBuilder()
        .WithDatabase("employeecrud")
        .WithPassword("123456")
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("IntegrationTests");
        builder.ConfigureServices(services => 
        {
            var connectionString = _postgreSqlContainer.GetConnectionString();
            
            var dbconnectionDescriptor = services.FirstOrDefault(s => s.ServiceType == typeof(IDbConnection));

            if (dbconnectionDescriptor is not null)
            {
                services.Remove(dbconnectionDescriptor);
            }
            
            services.AddScoped<IDbConnection>((_) => new NpgsqlConnection(connectionString));
            
            var runMigrationService = services.FirstOrDefault(s => s.ServiceType == typeof(RunMigrationService));

            if (runMigrationService is not null)
            {
                services.Remove(runMigrationService);
            }
            
            services.AddTransient<RunMigrationService>(_=>
            {
                var logger = new LoggerFactory().CreateLogger<RunMigrationService>();
                
                return new RunMigrationService(logger, connectionString, connectionString);
            });
        });
    }
    public async Task InitializeAsync()
    {
        await _postgreSqlContainer.StartAsync();
    }

    public new Task DisposeAsync()
    {
        return _postgreSqlContainer.StopAsync();
    }
}