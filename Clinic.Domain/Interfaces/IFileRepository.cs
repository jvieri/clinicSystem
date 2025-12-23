using ApiSitemaClinico.Clinic.Domain.Entities;

namespace ApiSitemaClinico.Clinic.Domain.Interfaces
{
  public interface IFileRepository
  {
    Task<ClinicalFile> AddAsync(ClinicalFile file, CancellationToken cancellationToken = default);
    Task<ClinicalFile?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<List<ClinicalFile>> GetByPatientAsync(long patientId, CancellationToken cancellationToken = default);
  }
}
