using ApiSitemaClinico.Clinic.Domain.Entities;
using ApiSitemaClinico.Clinic.Domain.Interfaces;
using ApiSitemaClinico.Clinic.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ApiSitemaClinico.Clinic.Infrastructure.Repositories
{
  public class NotificationRepository : INotificationRepository
  {
    private readonly ClinicDbContext _context;

    public NotificationRepository(ClinicDbContext context)
    {
      _context = context;
    }

    public async Task AddAsync(Notification notification, CancellationToken cancellationToken = default)
    {
      await _context.Set<Notification>().AddAsync(notification, cancellationToken);
      await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<Notification>> GetPendingAsync(DateTime since, CancellationToken cancellationToken = default)
    {
      return await _context.Set<Notification>().Where(n => n.SentAt >= since && !n.IsRead).ToListAsync(cancellationToken);
    }

    public async Task MarkAsReadAsync(long id, CancellationToken cancellationToken = default)
    {
      var n = await _context.Set<Notification>().FindAsync(new object[] { id }, cancellationToken);
      if (n == null) return;
      n.IsRead = true;
      await _context.SaveChangesAsync(cancellationToken);
    }
  }
}
