using System.Threading.Tasks;

namespace ApiSitemaClinico.Clinic.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IAppointmentRepository AppointmentRepository { get; }
        IUserRepository UserRepository { get; }
        IExamRepository ExamRepository { get; }
        ITreatmentRepository TreatmentRepository { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}