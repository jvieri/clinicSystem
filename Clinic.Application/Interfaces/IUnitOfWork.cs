using ApiSitemaClinico.Clinic.Domain.Entities;

namespace ApiSitemaClinico.Clinic.Domain.Interfaces
{
  public interface IUnitOfWork
  {
    IAppointmentRepository AppointmentRepository { get; }
    IUserRepository UserRepository { get; }
    IExamRepository ExamRepository { get; }
    ITreatmentRepository TreatmentRepository { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
  }
}
