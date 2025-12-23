using ApiSitemaClinico.Clinic.Domain.Common;
using System.Data;
using ApiSitemaClinico.Clinic.Domain.Entities;

namespace ApiSitemaClinico.Clinic.Domain.Entities
{
  public class User : BaseEntity
  {
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? DocumentType { get; set; }
    public string? DocumentNumber { get; set; }
    public string? PhoneNumber { get; set; }
    public bool IsActive { get; set; } = true;

    public long RoleId { get; set; }
    public Role Role { get; set; } = null!;
    
    // Refresh token fields
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiry { get; set; }
  }
}
