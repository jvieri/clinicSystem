using ApiSitemaClinico.Clinic.Domain.Common;

namespace ApiSitemaClinico.Clinic.Domain.Entities
{
  public class Treatment : BaseEntity
  {
    public long PatientId { get; set; }
    public string Description { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public long PrescribedBy { get; set; }
  }
}
