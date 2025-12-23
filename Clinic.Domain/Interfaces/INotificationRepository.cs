using ApiSitemaClinico.Clinic.Domain.Entities;

namespace ApiSitemaClinico.Clinic.Domain.Interfaces
{
  public interface INotificationRepository
  {
    Task AddAsync(Notification notification, CancellationToken cancellationToken = default);
    Task<List<Notification>> GetPendingAsync(DateTime since, CancellationToken cancellationToken = default);
    Task MarkAsReadAsync(long id, CancellationToken cancellationToken = default);
  }
}
