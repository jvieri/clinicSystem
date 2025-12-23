using ApiSitemaClinico.Clinic.Domain.Entities;
using ApiSitemaClinico.Clinic.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ApiSitemaClinico.Clinic.Infrastructure.Services
{
  public interface IDataSeeder
  {
    Task SeedAsync(CancellationToken ct = default);
  }

  public class DataSeeder : IDataSeeder
  {
    private readonly ClinicDbContext _context;
    private readonly ILogger<DataSeeder> _logger;

    public DataSeeder(ClinicDbContext context, ILogger<DataSeeder> logger)
    {
      _context = context;
      _logger = logger;
    }

    public async Task SeedAsync(CancellationToken ct = default)
    {
      _logger.LogInformation("Starting database seeding...");

      // Ensure database created
      await _context.Database.EnsureCreatedAsync(ct);

      // Seed roles if missing
      if (!await _context.Roles.AnyAsync(ct))
      {
        _context.Roles.AddRange(
          new Role { Id = 1, Name = "Admin" },
          new Role { Id = 2, Name = "Doctor" },
          new Role { Id = 3, Name = "Enfermero" },
          new Role { Id = 4, Name = "Paciente" }
        );
        await _context.SaveChangesAsync(ct);
        _logger.LogInformation("Seeded roles.");
      }

      // Seed admin user
      var adminEmail = "admin@clinic.local";
      if (!await _context.Users.AnyAsync(u => u.Email == adminEmail, ct))
      {
        var password = "Admin@123";
        var hashed = BCrypt.Net.BCrypt.HashPassword(password);

        var admin = new User
        {
          Email = adminEmail,
          PasswordHash = hashed,
          FirstName = "Super",
          LastName = "Admin",
          RoleId = 1,
          IsActive = true
        };

        _context.Users.Add(admin);
        await _context.SaveChangesAsync(ct);
        _logger.LogInformation("Created admin user {Email} with default password.", adminEmail);
      }

      _logger.LogInformation("Database seeding completed.");
    }
  }
}
