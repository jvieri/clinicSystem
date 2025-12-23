using ApiSitemaClinico.Clinic.Domain.Entities;

namespace ApiSitemaClinico.Clinic.Domain.Interfaces
{
  public interface IPatientRepository
  {
    Task<Patient> AddAsync(Patient patient, CancellationToken cancellationToken = default);
    Task<Patient?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<List<Patient>> GetAllAsync(CancellationToken cancellationToken = default);
    Task UpdateAsync(Patient patient, CancellationToken cancellationToken = default);
    Task DeleteAsync(Patient patient, CancellationToken cancellationToken = default);
  }
}
