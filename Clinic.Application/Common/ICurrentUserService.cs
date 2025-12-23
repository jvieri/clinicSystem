namespace ApiSitemaClinico.Clinic.Application.Common
{
  public interface ICurrentUserService
  {
    long? UserId { get; }
    string? UserRole { get; }
  }
}
