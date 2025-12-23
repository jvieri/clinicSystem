using ApiSitemaClinico.Clinic.Domain.Entities;
using ApiSitemaClinico.Clinic.Domain.Interfaces;
using System.Net.Mail;
using System.Net;

namespace ApiSitemaClinico.Clinic.Infrastructure.Services
{
  public interface INotificationService
  {
    Task SendAsync(Notification notification, CancellationToken cancellationToken = default);
  }

  public class NotificationService : INotificationService
  {
    private readonly INotificationRepository _repo;
    private readonly ILogger<NotificationService> _logger;
    private readonly IConfiguration _configuration;

    public NotificationService(INotificationRepository repo, ILogger<NotificationService> logger, IConfiguration configuration)
    {
      _repo = repo;
      _logger = logger;
      _configuration = configuration;
    }

    public async Task SendAsync(Notification notification, CancellationToken cancellationToken = default)
    {
      // Persist notification record
      await _repo.AddAsync(notification, cancellationToken);

      // Attempt to send email via SMTP when configuration present
      try
      {
        var smtpHost = _configuration["Smtp:Host"];
        if (!string.IsNullOrWhiteSpace(smtpHost))
        {
          var smtpPort = int.TryParse(_configuration["Smtp:Port"], out var p) ? p : 25;
          var enableSsl = bool.TryParse(_configuration["Smtp:EnableSsl"], out var s) ? s : true;
          var from = _configuration["Smtp:From"] ?? "no-reply@clinic.local";
          var user = _configuration["Smtp:User"];
          var pass = _configuration["Smtp:Pass"];

          // Build mail message. Payload expected to be simple text or JSON containing details.
          var mail = new MailMessage();
          mail.From = new MailAddress(from);
          // determine recipient email: for now assume Payload contains recipient email or UserId maps to user's email (not implemented)
          // If payload is an email address, send to it; otherwise log and skip sending.
          var recipient = notification.Payload;
          if (!string.IsNullOrWhiteSpace(recipient) && recipient.Contains("@"))
          {
            mail.To.Add(recipient);
            mail.Subject = notification.Type;
            mail.Body = notification.Payload;
            mail.IsBodyHtml = false;

            using var client = new SmtpClient(smtpHost, smtpPort)
            {
              EnableSsl = enableSsl
            };

            if (!string.IsNullOrWhiteSpace(user))
            {
              client.Credentials = new NetworkCredential(user, pass);
            }

            await client.SendMailAsync(mail, cancellationToken);
            _logger.LogInformation("Sent email notification to {Recipient}", recipient);
          }
          else
          {
            _logger.LogWarning("Notification payload does not contain an email recipient; skipping SMTP send. Payload: {Payload}", notification.Payload);
          }
        }
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Failed to send SMTP notification for user {UserId}", notification.UserId);
      }
    }
  }
}
