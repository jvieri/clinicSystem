using ApiSitemaClinico.Clinic.Domain.Entities;

namespace ApiSitemaClinico.Clinic.Domain.Interfaces
{
  public interface IPaymentRepository
  {
    Task<Payment> AddAsync(Payment payment, CancellationToken cancellationToken = default);
    Task<Payment?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task UpdateAsync(Payment payment, CancellationToken cancellationToken = default);
    Task<List<Payment>> GetByPatientAsync(long patientId, CancellationToken cancellationToken = default);
  }
}
