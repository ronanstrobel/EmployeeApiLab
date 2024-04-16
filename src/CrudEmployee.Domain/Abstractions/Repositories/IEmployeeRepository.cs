using CrudEmployee.Domain.Entities;

namespace CrudEmployee.Domain.Abstractions.Repositories;

public interface IEmployeeRepository
{
    Task InsertAsync(Employee employee, CancellationToken ct);
    Task<Employee?> GetByIdAsync(long id, CancellationToken ct);
}