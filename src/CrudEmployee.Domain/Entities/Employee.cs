
namespace CrudEmployee.Domain.Entities;

public class Employee(string name, short age, decimal salary)
{
    public long Id { get; set; }
    
    public string Name { get; set; } = name;
    
    public short Age { get; set; } = age;
    
    public decimal Salary { get; set; } = salary;
}