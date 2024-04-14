namespace CrudEmployee.Domain.Entities;

public class Employee
{
    public long Id { get; set; }
    public string Name { get; set; }
    public short Age { get; set; }
    public decimal Salary { get; set; }
}