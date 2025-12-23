using ApiSitemaClinico.Clinic.Domain.Common;

namespace ApiSitemaClinico.Clinic.Domain.Entities
{
  public class ClinicalFile : BaseEntity
  {
    public long PatientId { get; set; }
    public long? AppointmentId { get; set; }
    public string FileName { get; set; } = null!;
    public string Path { get; set; } = null!;
    public string ContentType { get; set; } = null!;
    public long Size { get; set; }
    public long UploadedBy { get; set; }
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
  }
}
