using ApiSitemaClinico.Clinic.Domain.Common;

namespace ApiSitemaClinico.Clinic.Domain.Entities
{
  public class Notification : BaseEntity
  {
    public long UserId { get; set; }
    public string Type { get; set; } = null!;
    public string Payload { get; set; } = null!; // JSON
    public bool IsRead { get; set; } = false;
    public DateTime SentAt { get; set; } = DateTime.UtcNow;
    public long? AppointmentId { get; set; }
  }
}
