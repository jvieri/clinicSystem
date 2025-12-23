using ApiSitemaClinico.Clinic.Domain.Common;
using ApiSitemaClinico.Clinic.Domain.Entities;
using ApiSitemaClinico.Clinic.Application.Common;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Reflection.Emit;

namespace ApiSitemaClinico.Clinic.Infrastructure.Persistence
{
  public class ClinicDbContext : DbContext
  {
    private readonly ICurrentUserService _currentUserService;

    public ClinicDbContext(DbContextOptions<ClinicDbContext> options, ICurrentUserService currentUserService)
        : base(options)
    {
      _currentUserService = currentUserService;
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Patient> Patients => Set<Patient>();
    public DbSet<Appointment> Appointments => Set<Appointment>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<PaymentDetail> PaymentDetails => Set<PaymentDetail>();
    public DbSet<ClinicalFile> ClinicalFiles => Set<ClinicalFile>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
    public DbSet<TreatmentCatalog> TreatmentCatalogs => Set<TreatmentCatalog>();
    public DbSet<ExamCatalog> ExamCatalogs => Set<ExamCatalog>();
    public DbSet<Treatment> Treatments => Set<Treatment>();
    public DbSet<Exam> Exams => Set<Exam>();
    public DbSet<TreatmentHistory> TreatmentHistories => Set<TreatmentHistory>();
    public DbSet<ExamHistory> ExamHistories => Set<ExamHistory>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
      base.OnModelCreating(builder);

      // Soft delete global
      foreach (var entityType in builder.Model.GetEntityTypes())
      {
        if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
        {
          // Apply a global query filter for soft-delete for all BaseEntity types
          var method = typeof(ClinicDbContext).GetMethod(nameof(ApplySoftDeleteFilter), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.MakeGenericMethod(entityType.ClrType);
          method?.Invoke(this, new object[] { builder });
        }
      }

      // Seed roles (solo en desarrollo)
      builder.Entity<Role>().HasData(
          new Role { Id = 1, Name = "Admin" },
          new Role { Id = 2, Name = "Doctor" },
          new Role { Id = 3, Name = "Enfermero" },
          new Role { Id = 4, Name = "Paciente" }
      );

      // Configure relationships
      builder.Entity<Payment>()
        .HasMany(p => p.Details)
        .WithOne()
        .HasForeignKey(d => d.PaymentId)
        .OnDelete(DeleteBehavior.Cascade);

      builder.Entity<Treatment>()
        .HasOne<User>()
        .WithMany()
        .HasForeignKey(t => t.PrescribedBy)
        .OnDelete(DeleteBehavior.NoAction);

      builder.Entity<Exam>()
        .HasOne<Appointment>()
        .WithMany()
        .HasForeignKey(e => e.AppointmentId)
        .OnDelete(DeleteBehavior.Cascade);
    }

    // Generic method invoked via reflection to apply an EF Core HasQueryFilter for IsDeleted==false
    private void ApplySoftDeleteFilter<TEntity>(ModelBuilder builder) where TEntity : BaseEntity
    {
      builder.Entity<TEntity>().HasQueryFilter(e => !e.IsDeleted);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
      var entries = ChangeTracker.Entries<BaseEntity>()
          .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

      var userId = _currentUserService.UserId ?? 1; // Superadmin fallback

      foreach (var entry in entries)
      {
        if (entry.State == EntityState.Added)
        {
          entry.Entity.CreatedBy = userId;
          entry.Entity.CreatedDate = DateTime.UtcNow;
          entry.Entity.ModifiedBy = userId;
          entry.Entity.ModifiedDate = DateTime.UtcNow;
        }
        else
        {
          entry.Entity.ModifiedBy = userId;
          entry.Entity.ModifiedDate = DateTime.UtcNow;
          entry.Property("CreatedDate").IsModified = false;
        }
      }

      // Create audit logs for changes
      var auditEntries = new List<AuditLog>();
      foreach (var entry in ChangeTracker.Entries<BaseEntity>())
      {
        if (entry.State == EntityState.Added || entry.State == EntityState.Modified || entry.State == EntityState.Deleted)
        {
          var keyDict = entry.Properties.Where(p => p.Metadata.IsPrimaryKey()).ToDictionary(p => p.Metadata.Name, p => p.CurrentValue);
          var audit = new AuditLog
          {
            TableName = entry.Metadata.GetTableName() ?? string.Empty,
            KeyValues = System.Text.Json.JsonSerializer.Serialize(keyDict),
            Action = entry.State.ToString(),
            UserId = userId,
            Timestamp = DateTime.UtcNow
          };

          if (entry.State == EntityState.Modified)
          {
            audit.OldValues = System.Text.Json.JsonSerializer.Serialize(entry.Properties.ToDictionary(p => p.Metadata.Name, p => p.OriginalValue));
            audit.NewValues = System.Text.Json.JsonSerializer.Serialize(entry.Properties.ToDictionary(p => p.Metadata.Name, p => p.CurrentValue));
          }

          if (entry.State == EntityState.Added)
          {
            audit.NewValues = System.Text.Json.JsonSerializer.Serialize(entry.Properties.ToDictionary(p => p.Metadata.Name, p => p.CurrentValue));
          }

          auditEntries.Add(audit);

          // For treatments and exams, also create history snapshot on create/update
          if (entry.Entity is Treatment treatment && (entry.State == EntityState.Added || entry.State == EntityState.Modified))
          {
            var history = new TreatmentHistory
            {
              TreatmentId = treatment.Id,
              Snapshot = System.Text.Json.JsonSerializer.Serialize(treatment),
              ChangedBy = userId,
              ChangedAt = DateTime.UtcNow,
              Action = entry.State == EntityState.Added ? "Created" : "Updated"
            };
            auditEntries.Add(new AuditLog()); // placeholder to keep operations consistent
            TreatmentHistories.Add(history);
          }

          if (entry.Entity is Exam exam && (entry.State == EntityState.Added || entry.State == EntityState.Modified))
          {
            var history = new ExamHistory
            {
              ExamId = exam.Id,
              Snapshot = System.Text.Json.JsonSerializer.Serialize(exam),
              ChangedBy = userId,
              ChangedAt = DateTime.UtcNow,
              Action = entry.State == EntityState.Added ? "Created" : "Updated"
            };
            auditEntries.Add(new AuditLog());
            ExamHistories.Add(history);
          }
        }
      }


      var result = await base.SaveChangesAsync(cancellationToken);

      if (auditEntries.Any())
      {
        // save audits without triggering recursion
        foreach (var a in auditEntries)
        {
          AuditLogs.Add(a);
        }
        await base.SaveChangesAsync(cancellationToken);
      }

      return result;
    }
  }
}
