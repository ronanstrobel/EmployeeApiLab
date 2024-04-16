using System.Net;
using System.Net.Http.Json;
using AutoFixture;
using CrudEmployee.Api.Utils;
using CrudEmployee.Domain.Abstractions.Repositories;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace CrudEmployeeIntegrationTests.Employee;

public class GetmployeeTests(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    private readonly Fixture _fixture = new();
    
    [Fact]
    public async Task GetShouldReturnOkResultEmployee()
    {
        // Arrange
        var client = factory.CreateClient();
        var expectedResponse =_fixture.Build<ExpectedResponse>().Create();
        var id = await InsertEmployee(expectedResponse);
        
        //Act
        var apiResponse = await client.GetAsync($"{Constants.Routes.EmployeeBaseRoute}/{id}");
        
        //Assert
        apiResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var response = await apiResponse.Content.ReadFromJsonAsync<ExpectedResponse>();
        response.Should().BeEquivalentTo(expectedResponse);
    }
    
    private async Task<long> InsertEmployee(ExpectedResponse response)
    {
        using var scope = factory.Services.CreateScope();
        
        var employeeRepository = scope.ServiceProvider.GetRequiredService<IEmployeeRepository>();
        
        var employee = new CrudEmployee.Domain.Entities.Employee(response.Name, response.Age, response.Salary);
        await employeeRepository.InsertAsync(employee, CancellationToken.None);
        response.Id = employee.Id;
        return response.Id;
    }
    
    private class ExpectedResponse
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public short Age { get; set; }
        public decimal Salary { get; set; }
    }
}