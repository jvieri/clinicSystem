using ApiSitemaClinico.Clinic.Domain.Entities;

namespace ApiSitemaClinico.Clinic.Domain.Interfaces
{
    public interface IAppointmentRepository
    {
        Task<bool> IsOverlappingAsync(long doctorId, DateOnly date, TimeOnly start, TimeOnly end, long? excludeAppointmentId = null, CancellationToken cancellationToken = default);
        Task<Appointment> AddAsync(Appointment appointment, CancellationToken cancellationToken = default);
        Task<Appointment?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
        Task UpdateAsync(Appointment appointment, CancellationToken cancellationToken = default);
    }
}
