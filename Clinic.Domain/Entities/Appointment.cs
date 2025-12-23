using ApiSitemaClinico.Clinic.Domain.Common;

namespace ApiSitemaClinico.Clinic.Domain.Entities
{

  public class Appointment : BaseEntity
  {
    public long PatientId { get; set; }
    public long DoctorId { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public string Status { get; set; } = "Scheduled";
    public string Reason { get; set; } = null!;
    public string? Notes { get; set; }

    public Patient Patient { get; set; } = null!;
    public User Doctor { get; set; } = null!;
  }
}
