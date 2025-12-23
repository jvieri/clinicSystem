using ApiSitemaClinico.Clinic.Domain.Common;

namespace ApiSitemaClinico.Clinic.Domain.Entities
{
  public class AuditLog : BaseEntity
  {
    public string TableName { get; set; } = null!;
    public string KeyValues { get; set; } = null!; // JSON
    public string? OldValues { get; set; }
    public string? NewValues { get; set; }
    public string Action { get; set; } = null!; // Insert, Update, Delete
    public long UserId { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
  }
}
