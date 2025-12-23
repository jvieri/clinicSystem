using ApiSitemaClinico.Clinic.Domain.Entities;
using ApiSitemaClinico.Clinic.Domain.Interfaces;
using ApiSitemaClinico.Clinic.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ApiSitemaClinico.Clinic.Infrastructure.Repositories
{
  public class AppointmentRepository : IAppointmentRepository
  {
    private readonly ClinicDbContext _context;
    private readonly ILogger<AppointmentRepository> _logger;

    public AppointmentRepository(ClinicDbContext context, ILogger<AppointmentRepository> logger)
    {
      _context = context;
      _logger = logger;
    }

    public async Task<bool> IsOverlappingAsync(long doctorId, DateOnly date, TimeOnly start, TimeOnly end, long? excludeAppointmentId = null, CancellationToken cancellationToken = default)
    {
      _logger.LogDebug("Checking overlap for doctor {DoctorId} on {Date} {Start}-{End}", doctorId, date, start, end);

      var q = _context.Appointments.Where(a => a.DoctorId == doctorId && a.Date == date && a.Status == "Scheduled");
      if (excludeAppointmentId.HasValue) q = q.Where(a => a.Id != excludeAppointmentId.Value);
      var overlaps = await q.AnyAsync(a => !(a.EndTime <= start || a.StartTime >= end), cancellationToken);

      _logger.LogDebug("Overlap result: {Overlaps}", overlaps);
      return overlaps;
    }

    public async Task<Appointment> AddAsync(Appointment appointment, CancellationToken cancellationToken = default)
    {
      await _context.Appointments.AddAsync(appointment, cancellationToken);
      await _context.SaveChangesAsync(cancellationToken);
      return appointment;
    }

    public async Task<Appointment?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
      return await _context.Appointments.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task UpdateAsync(Appointment appointment, CancellationToken cancellationToken = default)
    {
      _context.Appointments.Update(appointment);
      await _context.SaveChangesAsync(cancellationToken);
    }
  }
}
