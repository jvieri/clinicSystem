using ApiSitemaClinico.Clinic.Domain.Entities;

namespace ApiSitemaClinico.Clinic.Domain.Interfaces
{
  public interface IExamRepository
  {
    Task AddAsync(Exam exam, CancellationToken cancellationToken = default);
    Task<List<Exam>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Exam?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
  }
}
