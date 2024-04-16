using System.Data;
using CrudEmployee.Api.Utils;
using CrudEmployee.Domain.Abstractions.Repositories;
using FastEndpoints;
using RepoDb;

namespace CrudEmployee.Api.Employee;

public class CreateEndpoint(IEmployeeRepository employeeRepository) : Endpoint<CreateEndpoint.Request, long>
{
    public override void Configure()
    {
        Post(Constants.Routes.EmployeeBaseRoute);
        AllowAnonymous();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        if (ct.IsCancellationRequested)
        {
            return ;
        }

        var employee = new Domain.Entities.Employee(req.Name, req.Age, req.Salary);
        
        await employeeRepository.InsertAsync(employee, ct);

        Response = employee.Id;
    }
    
    public record Request(string Name, short Age, decimal Salary);
}
