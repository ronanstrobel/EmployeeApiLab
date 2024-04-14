using CrudEmployee.Api.Utils;
using FastEndpoints;

namespace CrudEmployee.Api.Employee;

public class CreateEndpoint : Endpoint<CreateEndpoint.Request, int>
{
    public override void Configure()
    {
        Post(Constants.Routes.EmployeeBaseRoute);
        AllowAnonymous();
    }

    public override Task HandleAsync(Request req, CancellationToken ct)
    {
        if (ct.IsCancellationRequested)
        {
            return Task.FromResult(0);
        }
        
        
        return Task.FromResult(1);
    }
    
    public record Request(string Name, int Age);
}
