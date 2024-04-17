using System.Data;
using CrudEmployee.Infrastructure;
using CrudEmployee.Infrastructure.Database;
using FastEndpoints;
using Npgsql;
using RepoDb;

GlobalConfiguration
    .Setup()
    .UsePostgreSql();

var builder = WebApplication.CreateBuilder(args);

builder.Services.RegisterRepositories();
builder.Services.AddFastEndpoints();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("employeecrud");
var connectionStringMasterDb = builder.Configuration.GetConnectionString("postgresMasterDb");
builder.Services.AddScoped<IDbConnection>((_) => new NpgsqlConnection(connectionString));
builder.Services.AddTransient<RunMigrationService>(_ =>
{
    var logger = new LoggerFactory().CreateLogger<RunMigrationService>();

    return new RunMigrationService(logger, connectionString, connectionStringMasterDb);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseFastEndpoints();
RunMigrations(app);
app.Run();
return;

static void RunMigrations(WebApplication app)
{
    using var serviceScope = app.Services.CreateScope();
    var runMigrationService = serviceScope.ServiceProvider.GetRequiredService<RunMigrationService>();
    runMigrationService.RunMigrations();
}

public partial class Program
{
    protected Program()
    {
        
    }
}