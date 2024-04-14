using CrudEmployee.Api.Utils;
using FastEndpoints;

namespace CrudEmployee.Api.Employee;

public class GetAllEndpoint : EndpointWithoutRequest<List<GetAllEndpoint.EndpointResponse>>
{
    public override void Configure()
    {
        Get(Constants.Routes.EmployeeBaseRoute);
        AllowAnonymous();
    }

    public override Task HandleAsync(CancellationToken ct)
    {
        if (ct.IsCancellationRequested)
        {
            return Task.FromResult(new List<EndpointResponse>());
        }
        
        Response = new List<EndpointResponse>()
        {
            new(1,"Jhon Doe", 30),
            new(2,"Mike Saul", 31),
        };
        return Task.CompletedTask;
    }
    
    public record EndpointResponse(int Id, string Name, int Age);
}