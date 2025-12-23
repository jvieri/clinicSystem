using ApiSitemaClinico.Clinic.Domain.Common;

namespace ApiSitemaClinico.Clinic.Domain.Entities
{
  public enum PaymentMethod
  {
    Cash,
    Card,
    Transfer
  }

  public enum PaymentStatus
  {
    Pending,
    PartiallyPaid,
    Paid,
    Failed
  }

  public class Payment : BaseEntity
  {
    public long PatientId { get; set; }
    public long? AppointmentId { get; set; }
    public decimal Total { get; set; }
    public decimal PaidAmount { get; set; }
    public PaymentMethod Method { get; set; }
    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
    public DateTime Date { get; set; } = DateTime.UtcNow;

    public List<PaymentDetail> Details { get; set; } = new();
  }
}
