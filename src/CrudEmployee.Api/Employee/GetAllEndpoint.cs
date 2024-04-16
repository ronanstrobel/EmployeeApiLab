using System.Data;
using CrudEmployee.Api.Utils;
using FastEndpoints;
using RepoDb;

namespace CrudEmployee.Api.Employee;

public class GetAllEndpoint(IDbConnection dbConnection) : EndpointWithoutRequest<List<GetAllEndpoint.EndpointResponse>>
{
    private readonly IDbConnection _dbConnection = dbConnection;

    public override void Configure()
    {
        Get(Constants.Routes.EmployeeBaseRoute);
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        if (ct.IsCancellationRequested)
        {
            Response = new List<EndpointResponse>();
        }

        const string sql = "SELECT \"Id\", \"Name\", \"Age\", \"Salary\" FROM \"Employee\"";
        var result = (await _dbConnection.ExecuteQueryAsync<EndpointResponse>(sql, cancellationToken: ct)).ToList();
        Response = result;
    }
    
    public record EndpointResponse(int Id, string Name, int Age, decimal Salary);
}