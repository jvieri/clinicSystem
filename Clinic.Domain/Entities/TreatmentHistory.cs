using ApiSitemaClinico.Clinic.Domain.Common;

namespace ApiSitemaClinico.Clinic.Domain.Entities
{
  public class TreatmentHistory : BaseEntity
  {
    public long TreatmentId { get; set; }
    public string Snapshot { get; set; } = null!; // JSON
    public long ChangedBy { get; set; }
    public DateTime ChangedAt { get; set; } = DateTime.UtcNow;
    public string Action { get; set; } = null!; // Created, Updated
  }
}
