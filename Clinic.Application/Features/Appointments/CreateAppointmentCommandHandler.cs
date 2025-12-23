using ApiSitemaClinico.Clinic.Application.Common;
using ApiSitemaClinico.Clinic.Application.Features.Appointments;
using ApiSitemaClinico.Clinic.Domain.Entities;
using ApiSitemaClinico.Clinic.Domain.Interfaces;
using MediatR;

namespace ApiSitemaClinico.Clinic.Application.Features.Appointments
{
  public record CreateAppointmentRequest(long PatientId, long DoctorId, DateOnly Date, TimeOnly StartTime, TimeOnly EndTime, string Reason) : IRequest<Result<Appointment>>;

  public class CreateAppointmentCommandHandler : IRequestHandler<CreateAppointmentRequest, Result<Appointment>>
  {
    private readonly IAppointmentRepository _repo;

    public CreateAppointmentCommandHandler(IAppointmentRepository repo)
    {
      _repo = repo;
    }

    public async Task<Result<Appointment>> Handle(CreateAppointmentRequest request, CancellationToken cancellationToken)
    {
      // Check overlap again at handler level
      var overlapping = await _repo.IsOverlappingAsync(request.DoctorId, request.Date, request.StartTime, request.EndTime, null, cancellationToken);
      if (overlapping) return Result<Appointment>.Failure("El horario seleccionado no está disponible.");

      var appointment = new Appointment
      {
        PatientId = request.PatientId,
        DoctorId = request.DoctorId,
        Date = request.Date,
        StartTime = request.StartTime,
        EndTime = request.EndTime,
        Reason = request.Reason,
        Status = "Scheduled"
      };

      var created = await _repo.AddAsync(appointment, cancellationToken);
      return Result<Appointment>.Success(created);
    }
  }
}
