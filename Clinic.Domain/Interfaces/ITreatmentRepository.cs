using ApiSitemaClinico.Clinic.Domain.Entities;

namespace ApiSitemaClinico.Clinic.Domain.Interfaces
{
  public interface ITreatmentRepository
  {
    Task AddAsync(Treatment treatment, CancellationToken cancellationToken = default);
    Task<List<Treatment>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Treatment?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
  }
}
