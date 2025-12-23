using ApiSitemaClinico.Clinic.Application.Common;
using ApiSitemaClinico.Clinic.Domain.Entities;
using ApiSitemaClinico.Clinic.Domain.Interfaces;
using MediatR;

namespace ApiSitemaClinico.Clinic.Application.Features.Appointments
{
  public record ReprogramAppointmentCommand(long AppointmentId, DateOnly Date, TimeOnly StartTime, TimeOnly EndTime) : IRequest<Result<Appointment>>;

  public class ReprogramAppointmentCommandHandler : IRequestHandler<ReprogramAppointmentCommand, Result<Appointment>>
  {
    private readonly IAppointmentRepository _repo;

    public ReprogramAppointmentCommandHandler(IAppointmentRepository repo)
    {
      _repo = repo;
    }

    public async Task<Result<Appointment>> Handle(ReprogramAppointmentCommand request, CancellationToken cancellationToken)
    {
      var appt = await _repo.GetByIdAsync(request.AppointmentId, cancellationToken);
      if (appt == null) return Result<Appointment>.Failure("Cita no encontrada");

      // Check overlap excluding current appointment
      var overlap = await _repo.IsOverlappingAsync(appt.DoctorId, request.Date, request.StartTime, request.EndTime, appt.Id, cancellationToken);
      if (overlap) return Result<Appointment>.Failure("El horario seleccionado no está disponible.");

      appt.Date = request.Date;
      appt.StartTime = request.StartTime;
      appt.EndTime = request.EndTime;
      appt.Status = "Scheduled";

      await _repo.UpdateAsync(appt, cancellationToken);
      return Result<Appointment>.Success(appt);
    }
  }
}
