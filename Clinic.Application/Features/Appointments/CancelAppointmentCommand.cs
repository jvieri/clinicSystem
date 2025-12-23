using ApiSitemaClinico.Clinic.Application.Common;
using MediatR;

namespace ApiSitemaClinico.Clinic.Application.Features.Appointments
{
  public record CancelAppointmentCommand(long AppointmentId) : IRequest<Result<bool>>;

  public class CancelAppointmentCommandHandler : IRequestHandler<CancelAppointmentCommand, Result<bool>>
  {
    private readonly ApiSitemaClinico.Clinic.Domain.Interfaces.IAppointmentRepository _repo;

    public CancelAppointmentCommandHandler(ApiSitemaClinico.Clinic.Domain.Interfaces.IAppointmentRepository repo)
    {
      _repo = repo;
    }

    public async Task<Result<bool>> Handle(CancelAppointmentCommand request, CancellationToken cancellationToken)
    {
      var appt = await _repo.GetByIdAsync(request.AppointmentId, cancellationToken);
      if (appt == null) return Result<bool>.Failure("Cita no encontrada");
      appt.Status = "Cancelled";
      await _repo.UpdateAsync(appt, cancellationToken);
      return Result<bool>.Success(true);
    }
  }
}
