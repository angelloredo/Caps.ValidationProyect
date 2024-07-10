using System;

namespace CapsValidationProyect.Persistence.DapperConnection.Employee
{
    public class EmployeeModel
    {
   public int Id { get; set; }

    public int DepartmentId { get; set; }

    public string FullName { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string MiddleName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string MothersLastName { get; set; } = null!;

    }
}