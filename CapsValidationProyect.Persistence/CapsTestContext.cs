using System;
using System.Collections.Generic;
using CapsValidationProyect.Persistence.CapsValidationProyect.Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CapsValidationProyect.Persistence;

public partial class CapsTestContext : IdentityDbContext<EmployeeUser, IdentityRole<int>, int>
{
    public CapsTestContext()
    {
    }

    public CapsTestContext(DbContextOptions<CapsTestContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<EmployeeUser> EmployeeUsers { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Departments");

            entity.HasIndex(e => e.Description, "IX_Departments");

            entity.Property(e => e.AddedBy).HasMaxLength(200);
            entity.Property(e => e.AddedDate).HasColumnType("datetime");
            entity.Property(e => e.DeletedBy).HasMaxLength(200);
            entity.Property(e => e.DeletedDate).HasColumnType("datetime");
            entity.Property(e => e.LastUpdatedBy).HasMaxLength(200);
            entity.Property(e => e.LastUpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(200);
        });

        modelBuilder.Entity<Employee>(entity =>
        {

            entity.HasKey(e => e.Id).HasName("PK_Employees");

            entity.HasIndex(e => new { e.DepartmentId, e.FullName }, "IX_Employee").IsUnique();

            entity.HasIndex(e => new { e.Id, e.AddedDate }, "IX_Employee_AddedDate");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.AddedBy).HasMaxLength(200);
            entity.Property(e => e.AddedDate).HasColumnType("datetime");
            entity.Property(e => e.DeletedBy).HasMaxLength(200);
            entity.Property(e => e.DeletedDate).HasColumnType("datetime");
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.FullName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.LastUpdatedBy).HasMaxLength(200);
            entity.Property(e => e.LastUpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.MiddleName).HasMaxLength(50);
            entity.Property(e => e.MothersLastName).HasMaxLength(50);

            entity.HasOne(d => d.Department).WithMany(p => p.Employees)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Employee_Departments");
        });

        modelBuilder.Entity<EmployeeUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_EmployeeUsers");
            entity.Property(e => e.AddedBy).HasMaxLength(200);
            entity.Property(e => e.AddedDate).HasColumnType("datetime");
            entity.Property(e => e.DeletedBy).HasMaxLength(200);
            entity.Property(e => e.DeletedDate).HasColumnType("datetime");
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.LastUpdatedBy).HasMaxLength(200);
            entity.Property(e => e.LastUpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.Employee).WithMany(p => p.EmployeeUsers)
              .HasForeignKey(d => d.EmployeeId)
              .OnDelete(DeleteBehavior.ClientSetNull)
              .HasConstraintName("FK_User_Employee");
        });

        modelBuilder.Entity<IdentityUserLogin<int>>(entity =>
        {
            entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });
        });

        base.OnModelCreating(modelBuilder);
    }

    //partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
