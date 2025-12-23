using ApiSitemaClinico.Clinic.Domain.Common;

namespace ApiSitemaClinico.Clinic.Domain.Entities
{
  public class PaymentDetail : BaseEntity
  {
    public long PaymentId { get; set; }
    public string Concept { get; set; } = null!;
    public long? ReferenceId { get; set; }
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; } = 1;
    public decimal Subtotal => UnitPrice * Quantity;
  }
}
