using System.Data;
using CrudEmployee.Domain.Abstractions.Repositories;
using CrudEmployee.Domain.Entities;
using RepoDb;

namespace CrudEmployee.Infrastructure.Repositories;

public class EmployeeRepository(IDbConnection dbConnection) : IEmployeeRepository
{
    public async Task InsertAsync(Employee employee, CancellationToken ct)
    {
        employee.Id = (long)await dbConnection.InsertAsync(employee, cancellationToken: ct);
    }
    
    public async Task<Employee?> GetByIdAsync(long id, CancellationToken ct)
    {
        return (await dbConnection.QueryAsync<Employee>(e => e.Id == id, cancellationToken: ct)).FirstOrDefault();
    }
}