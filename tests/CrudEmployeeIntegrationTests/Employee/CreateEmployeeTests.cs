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

public class CreateEmployeeTests(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    private readonly Fixture _fixture = new();

    [Fact]
    public async Task CreateEmployeeShouldReturnOkStatusCode()
    {
        // Arrange
        var client = factory.CreateClient();
        var request = _fixture.Build<RequestPost>().Create();
        //Act
        var apiResponse = await client.PostAsJsonAsync($"{Constants.Routes.EmployeeBaseRoute}",request);
        
        //Assert
        apiResponse.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task CreateEmployeeShouldPersistTheExactlyDataPassedInRequestBody()
    {
        // Arrange
        var client = factory.CreateClient();
        var request = _fixture.Build<RequestPost>().Create();
        //Act
        var apiResponse = await client.PostAsJsonAsync($"{Constants.Routes.EmployeeBaseRoute}",request);
        
        //Assert
        var createdId = await apiResponse.Content.ReadFromJsonAsync<long>();
        
        using var scope = factory.Services.CreateScope();
        var employeeRepository = scope.ServiceProvider.GetRequiredService<IEmployeeRepository>();
        var createdEmployee = await employeeRepository.GetByIdAsync(createdId, CancellationToken.None); 
        
        createdEmployee.Should().NotBeNull();
        createdEmployee!.Name.Should().Be(request.Name);
        createdEmployee.Age.Should().Be(request.Age);
        createdEmployee.Salary.Should().Be(request.Salary);
        createdEmployee.Salary.Should().Be(request.Salary);
    }
    
    record RequestPost(string Name, short Age, decimal Salary);
}