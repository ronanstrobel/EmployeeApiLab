using System.Data.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Testcontainers.PostgreSql;

namespace CrudEmployeeIntegrationTests;

public class CustomWebApplicationFactory
    : WebApplicationFactory<Program>, IAsyncLifetime
{

    private readonly PostgreSqlContainer _postgreSqlContainer = new PostgreSqlBuilder()
        .WithPassword("123456")
        .WithPortBinding("5432","5432")
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");
    }

    public Task InitializeAsync()
    {
        return _postgreSqlContainer.StartAsync();
    }

    public new Task DisposeAsync()
    {
        return _postgreSqlContainer.StopAsync();
    }
}