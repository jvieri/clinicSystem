using ApiSitemaClinico.Clinic.Domain.Common;

namespace ApiSitemaClinico.Clinic.Domain.Entities
{
  public class TreatmentCatalog : BaseEntity
  {
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public decimal PriceBase { get; set; }
    public int DurationMinutes { get; set; }
    public bool IsActive { get; set; } = true;
  }
}
