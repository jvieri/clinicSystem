using ApiSitemaClinico.Clinic.Domain.Interfaces;
using ApiSitemaClinico.Clinic.Domain.Entities;
using ApiSitemaClinico.Clinic.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ApiSitemaClinico.Clinic.Infrastructure.Services
{
  public class AppointmentReminderService : BackgroundService
  {
    private readonly IServiceProvider _provider;
    private readonly ILogger<AppointmentReminderService> _logger;

    public AppointmentReminderService(IServiceProvider provider, ILogger<AppointmentReminderService> logger)
    {
      _provider = provider;
      _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
      _logger.LogInformation("AppointmentReminderService started.");
      while (!stoppingToken.IsCancellationRequested)
      {
        try
        {
          using var scope = _provider.CreateScope();
          var repo = scope.ServiceProvider.GetRequiredService<IAppointmentRepository>();
          var notifRepo = scope.ServiceProvider.GetRequiredService<INotificationRepository>();

          // find appointments starting in next 30 minutes
          var now = DateTime.UtcNow;
          var windowStart = now;
          var windowEnd = now.AddMinutes(30);

          var db = scope.ServiceProvider.GetRequiredService<ApiSitemaClinico.Clinic.Infrastructure.Persistence.ClinicDbContext>();
          var upcoming = await db.Appointments
            .Where(a => a.Date == DateOnly.FromDateTime(now) && a.StartTime >= TimeOnly.FromDateTime(now) && a.StartTime <= TimeOnly.FromDateTime(windowEnd) && a.Status == "Scheduled")
            .ToListAsync(stoppingToken);

          foreach (var appt in upcoming)
          {
            var n = new Notification
            {
              UserId = appt.PatientId,
              Type = "AppointmentReminder",
              Payload = $"{appt.Id}",
              IsRead = false,
              SentAt = DateTime.UtcNow,
              AppointmentId = appt.Id
            };

            await notifRepo.AddAsync(n, stoppingToken);
            _logger.LogInformation("Queued reminder for appointment {Id}", appt.Id);
          }
        }
        catch (Exception ex)
        {
          _logger.LogError(ex, "Error in reminder loop");
        }

        await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
      }
    }
  }
}
