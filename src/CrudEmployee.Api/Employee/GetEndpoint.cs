using CrudEmployee.Api.Utils;
using CrudEmployee.Domain.Abstractions.Repositories;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CrudEmployee.Api.Employee;

public class GetEndpoint(IEmployeeRepository employeeRepository):Endpoint<GetEndpoint.GetRequest,Results<Ok<GetEndpoint.GetResponse>, 
    NotFound, 
    ProblemDetails>>
{
    public override void Configure()
    {
        Get($"{Constants.Routes.EmployeeBaseRoute}/{{Id}}");
        AllowAnonymous();
    }

    public override async Task<Results<Ok<GetResponse>, NotFound, ProblemDetails>> ExecuteAsync(GetRequest req, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();
        var employee = await employeeRepository.GetByIdAsync(req.Id, ct);
        if (employee is null)
        {
            return TypedResults.NotFound();
        }
        
        return TypedResults.Ok(new GetResponse(employee.Id, employee.Name, employee.Age, employee.Salary));
    }

    public record GetRequest(long Id);
    public record GetResponse(long Id, string Name, short Age, decimal Salary);
}