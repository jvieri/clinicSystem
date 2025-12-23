using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ApiSitemaClinico.Clinic.Infrastructure.Persistence
{
  // Design-time factory to allow EF tools to create DbContext without building the entire app DI.
  public class ClinicDbContextFactory : IDesignTimeDbContextFactory<ClinicDbContext>
  {
    public ClinicDbContext CreateDbContext(string[] args)
    {
      var builder = new DbContextOptionsBuilder<ClinicDbContext>();
      var conn = Environment.GetEnvironmentVariable("CLINIC_DEFAULT_CONN")
                 ?? "Server=localhost;Database=clinic_db;User=root;Password=;";
      builder.UseMySql(conn, new MySqlServerVersion(new Version(8, 0, 33)));
      // For design-time, pass null for ICurrentUserService; ClinicDbContext has a constructor with ICurrentUserService, but EF will use the parameterless when available.
      return new ClinicDbContext(builder.Options, new ClinicDbContextDesignTimeUser());
    }

    private class ClinicDbContextDesignTimeUser : ApiSitemaClinico.Clinic.Application.Common.ICurrentUserService
    {
      public long? UserId => 1;
      public string? UserRole => "Admin";
    }
  }
}
