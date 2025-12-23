using FluentValidation;

namespace ApiSitemaClinico.Clinic.Application.Features.Treatments
{
  public class CreateTreatmentCommandValidator : AbstractValidator<CreateTreatmentCommand>
  {
    public CreateTreatmentCommandValidator()
    {
      RuleFor(x => x.PatientId).GreaterThan(0);
      RuleFor(x => x.Description).NotEmpty().MaximumLength(1000);
      RuleFor(x => x.StartDate).LessThanOrEqualTo(DateTime.UtcNow).WithMessage("La fecha de inicio no puede ser futura.");
    }
  }
}
