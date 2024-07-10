using System;
using System.Collections.Generic;

namespace CapsValidationProyect.Persistence.CapsValidationProyect.Domain.Models;

public partial class Department
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int Description { get; set; }

    public bool IsActive { get; set; }

    public DateTime AddedDate { get; set; }

    public string AddedBy { get; set; } = null!;

    public DateTime LastUpdatedDate { get; set; }

    public string LastUpdatedBy { get; set; } = null!;

    public DateTime? DeletedDate { get; set; }

    public string? DeletedBy { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
