using System.Data;
using System.Net;
using System.Net.Http.Json;
using AutoFixture;
using CrudEmployee.Api.Utils;
using CrudEmployee.Domain.Abstractions.Repositories;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using RepoDb;

namespace CrudEmployeeIntegrationTests.Employee;

public class GetAllEmployeesTests(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    private readonly Fixture _fixture = new();
    
    [Fact]
    public async Task GetAllShouldReturnAllEmployees()
    {
        // Arrange
        var client = factory.CreateClient();
        var expectedResponse = _fixture.Build<ExpectedResponse>().CreateMany().ToList();
        await InsertEmployees(expectedResponse);
        
        //Act
        var apiResponse = await client.GetAsync($"{Constants.Routes.EmployeeBaseRoute}");
        
        //Assert
        apiResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        
        
        var response = await apiResponse.Content.ReadFromJsonAsync<List<ExpectedResponse>>();
        response.Should().BeEquivalentTo(expectedResponse);
    }

    private async Task InsertEmployees(List<ExpectedResponse> responses)
    {
        using var scope = factory.Services.CreateScope();
        
        var employeeRepository = scope.ServiceProvider.GetRequiredService<IEmployeeRepository>();
        foreach (var response in responses)
        {
            var employee = new CrudEmployee.Domain.Entities.Employee(response.Name, response.Age, response.Salary);
            await employeeRepository.InsertAsync(employee, CancellationToken.None);
            response.Id = employee.Id;
        }
    }
    
    private class ExpectedResponse
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public short Age { get; set; }
        public decimal Salary { get; set; }
    }
}