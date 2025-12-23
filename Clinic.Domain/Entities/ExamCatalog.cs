using ApiSitemaClinico.Clinic.Domain.Common;

namespace ApiSitemaClinico.Clinic.Domain.Entities
{
  public class ExamCatalog : BaseEntity
  {
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public bool IsActive { get; set; } = true;
  }
}
