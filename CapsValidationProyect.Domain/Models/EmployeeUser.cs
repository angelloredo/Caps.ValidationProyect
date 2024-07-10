using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace CapsValidationProyect.Persistence.CapsValidationProyect.Domain.Models;

public partial class EmployeeUser : IdentityUser<int>
{
    public int Id { get; set; }
    public int? EmployeeId { get; set; }

    public string FullName { get; set; } = null!;

    public byte[]? PorfilePhoto { get; set; }

    public bool IsActive { get; set; }

    public DateTime AddedDate { get; set; }

    public string AddedBy { get; set; } = null!;

    public DateTime LastUpdatedDate { get; set; }

    public string LastUpdatedBy { get; set; } = null!;

    public DateTime? DeletedDate { get; set; }

    public string? DeletedBy { get; set; }
    public virtual Employee Employee { get; set; } = null!;
}
