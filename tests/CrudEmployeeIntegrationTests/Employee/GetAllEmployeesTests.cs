using System.Net;
using System.Net.Http.Json;
using CrudEmployee.Api.Utils;
using FluentAssertions;

namespace CrudEmployeeIntegrationTests.Employee;

public class GetAllEmployeesTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public GetAllEmployeesTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetAllShouldReturnAllEmployees()
    {
        // Arrange
        var client = _factory.CreateClient();
        
        //Act
        var apiResponse = await client.GetAsync($"{Constants.Routes.EmployeeBaseRoute}");
        
        //Assert
        apiResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var expectedResponse = new List<ExpectedResponse>()
        {
            new(1, "Jhon Doe", 30),
            new(2, "Mike Saul", 31),
        };
        var response = await apiResponse.Content.ReadFromJsonAsync<List<ExpectedResponse>>();
        response.Should().BeEquivalentTo(expectedResponse);
    }
    record ExpectedResponse(int Id, string Name, int Age);
}