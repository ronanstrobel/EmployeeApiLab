using System.Data;
using CrudEmployee.Api.Configurations;
using CrudEmployee.Infrastructure;
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
builder.Services.AddScoped<IDbConnection>((_) => new NpgsqlConnection(connectionString));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseFastEndpoints();
app.RunMigrations();
app.Run();

namespace CrudEmployee.Api
{
    public class Program
    {
        protected Program()
        { }
    }
}