using ApiSitemaClinico.Clinic.Application.Common;
using ApiSitemaClinico.Clinic.Domain.Entities;
using ApiSitemaClinico.Clinic.Domain.Interfaces;
using MediatR;

namespace ApiSitemaClinico.Clinic.Application.Features.Appointments
{
  public class CreateAppointmentRequestHandler : IRequestHandler<CreateAppointmentCommand, Result<Appointment>>
  {
    private readonly IAppointmentRepository _repo;

    public CreateAppointmentRequestHandler(IAppointmentRepository repo)
    {
      _repo = repo;
    }

    public async Task<Result<Appointment>> Handle(CreateAppointmentCommand request, CancellationToken cancellationToken)
    {
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
