using ApiSitemaClinico.Clinic.Domain.Interfaces;
using FluentValidation;
using System;
namespace ApiSitemaClinico.Clinic.Application.Features.Appointments
{
  public record CreateAppointmentCommand(
          long PatientId,
          long DoctorId,
          DateOnly Date,
          TimeOnly StartTime,
          TimeOnly EndTime,
          string Reason
    ) : MediatR.IRequest<ApiSitemaClinico.Clinic.Application.Common.Result<ApiSitemaClinico.Clinic.Domain.Entities.Appointment>>;

  public class CreateAppointmentCommandValidator : FluentValidation.AbstractValidator<CreateAppointmentCommand>
  {
    public CreateAppointmentCommandValidator(IAppointmentRepository appointmentRepo)
    {
      RuleFor(x => x.DoctorId).GreaterThan(0);
      RuleFor(x => x.PatientId).GreaterThan(0);
      RuleFor(x => x.Date).Must(date => date >= DateOnly.FromDateTime(DateTime.UtcNow)).WithMessage("La fecha debe ser hoy o futura.");
      RuleFor(x => x).MustAsync(async (cmd, ct) =>
          !(await appointmentRepo.IsOverlappingAsync(cmd.DoctorId, cmd.Date,
              cmd.StartTime, cmd.EndTime, cancellationToken: ct)))
          .WithMessage("El horario seleccionado no está disponible.");
    }
  }
}
