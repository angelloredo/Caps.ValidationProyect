using System;
using System.Collections.Generic;

namespace CapsValidationProyect.Persistence.CapsValidationProyect.Domain.Models;

public partial class Employee
{
    public int Id { get; set; }

    public int DepartmentId { get; set; }

    public string FullName { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string MiddleName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string MothersLastName { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime AddedDate { get; set; }

    public string AddedBy { get; set; } = null!;

    public DateTime LastUpdatedDate { get; set; }

    public string LastUpdatedBy { get; set; } = null!;

    public DateTime? DeletedDate { get; set; }

    public string? DeletedBy { get; set; }

    public virtual Department Department { get; set; } = null!;
    public virtual ICollection<EmployeeUser> EmployeeUsers { get; set; } = new List<EmployeeUser>();
}
