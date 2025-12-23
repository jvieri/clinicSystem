using ApiSitemaClinico.Clinic.Domain.Common;

namespace ApiSitemaClinico.Clinic.Domain.Entities
{
  public class Exam : BaseEntity
  {
    public long PatientId { get; set; }
    public long? AppointmentId { get; set; }
    public long? CatalogId { get; set; }
    public string Type { get; set; } = null!;
    public string? Result { get; set; }
    public DateTime? PerformedAt { get; set; }
    public decimal? Price { get; set; }
  }
}
