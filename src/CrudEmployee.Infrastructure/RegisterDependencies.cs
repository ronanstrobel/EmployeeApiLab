using CrudEmployee.Domain.Abstractions.Repositories;
using CrudEmployee.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace CrudEmployee.Infrastructure;

public static class RegisterDependencies
{
    public static void RegisterRepositories(this IServiceCollection services)
    {
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
    }
}