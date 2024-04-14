using CrudEmployee.Api.Utils;
using FastEndpoints;

namespace CrudEmployee.Api.Employee;

public class GetEndpoint:Endpoint<GetEndpoint.GetRequest,GetEndpoint.GetResponse>
{
    public override void Configure()
    {
        Get($"{Constants.Routes.EmployeeBaseRoute}/{{Id}}");
        AllowAnonymous();
    }

    public override Task HandleAsync(GetRequest req, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();
        if (req.Id == 1)
        {
            Response = new GetResponse(1, "Jhon Doe", 30,new GetEndpointResponseAddress("Street 1", "City 1", "State 1", "Country 1"));
        }
        else if (req.Id == 2)
        {
            Response = new GetResponse(2, "Mike Saul", 31, new GetEndpointResponseAddress("Street 2", "City 2", "State 2", "Country 2"));
        }
        
        return Task.CompletedTask;
    }

    public record GetRequest(int Id);
    public record GetResponse(int Id, string Name, int Age, GetEndpointResponseAddress Address);
    public record GetEndpointResponseAddress(string street, string city, string state, string country);
}