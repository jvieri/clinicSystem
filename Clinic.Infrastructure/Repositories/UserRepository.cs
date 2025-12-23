using ApiSitemaClinico.Clinic.Domain.Entities;
using ApiSitemaClinico.Clinic.Domain.Interfaces;
using ApiSitemaClinico.Clinic.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ApiSitemaClinico.Clinic.Infrastructure.Repositories
{
  public class UserRepository : IUserRepository
  {
    private readonly ClinicDbContext _context;
    private readonly ILogger<UserRepository> _logger;

    public UserRepository(ClinicDbContext context, ILogger<UserRepository> logger)
    {
      _context = context;
      _logger = logger;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
      if (string.IsNullOrWhiteSpace(email)) return null;
      _logger.LogDebug("GetByEmailAsync for {Email}", email);
      return await _context.Users
        .Include(u => u.Role)
        .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
    }

    public async Task<User?> GetByRefreshTokenAsync(string refreshToken)
    {
      if (string.IsNullOrWhiteSpace(refreshToken)) return null;
      _logger.LogDebug("GetByRefreshTokenAsync checking token");
      return await _context.Users
        .Include(u => u.Role)
        .FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
    }

    public async Task UpdateAsync(User user)
    {
      _logger.LogDebug("Updating user {UserId}", user.Id);
      _context.Users.Update(user);
      await _context.SaveChangesAsync();
    }
  }
}
