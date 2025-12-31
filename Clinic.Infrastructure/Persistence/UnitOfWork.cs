using ApiSitemaClinico.Clinic.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ApiSitemaClinico.Clinic.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ClinicDbContext _context;
        public IAppointmentRepository AppointmentRepository { get; }
        public IUserRepository UserRepository { get; }
        public IExamRepository ExamRepository { get; }
        public ITreatmentRepository TreatmentRepository { get; }

        public UnitOfWork(ClinicDbContext context,
            IAppointmentRepository appointmentRepository,
            IUserRepository userRepository,
            IExamRepository examRepository,
            ITreatmentRepository treatmentRepository)
        {
            _context = context;
            AppointmentRepository = appointmentRepository;
            UserRepository = userRepository;
            ExamRepository = examRepository;
            TreatmentRepository = treatmentRepository;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}