using CrudEmployee.Api.Utils;
using FastEndpoints;

namespace CrudEmployee.Api.Employee;

public class UpdateEndpoint : Endpoint<UpdateEndpoint.UpdateEnpointRequest, UpdateEndpoint.UpdateEnpointResponse>
{
    public override void Configure()
    {
        Put($"{Constants.Routes.EmployeeBaseRoute}/{{Id}}");
        AllowAnonymous();
    }

    public override Task HandleAsync(UpdateEnpointRequest req, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();
        var id = Route<int>("Id");
        
        if (id == 1)
        {
            Response = new UpdateEnpointResponse(1, "Jhon Doe", 30);
        }
        else if (id == 2)
        {
            Response = new UpdateEnpointResponse(2, "Mike Saul", 31);
        }
        return Task.CompletedTask;
    
    }

    public record UpdateEnpointRequest(string Name, int Age);
    public record UpdateEnpointResponse(int Id, string Name, int Age);
}