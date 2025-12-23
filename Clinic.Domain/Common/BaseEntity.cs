namespace ApiSitemaClinico.Clinic.Domain.Common
{
  public abstract class BaseEntity
  {
    public long Id { get; protected set; }
    public bool IsDeleted { get; set; }
    public long CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public long ModifiedBy { get; set; }
    public DateTime ModifiedDate { get; set; }
  }
}

