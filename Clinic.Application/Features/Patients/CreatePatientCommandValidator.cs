using FluentValidation;

namespace ApiSitemaClinico.Clinic.Application.Features.Patients
{
  public class CreatePatientCommandValidator : AbstractValidator<CreatePatientCommand>
  {
    public CreatePatientCommandValidator()
    {
      RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100);
      RuleFor(x => x.LastName).NotEmpty().MaximumLength(100);
      RuleFor(x => x.BirthDate).LessThanOrEqualTo(DateTime.UtcNow.Date).WithMessage("La fecha de nacimiento no puede ser futura.");
      RuleFor(x => x.Phone).MaximumLength(20);
      RuleFor(x => x.Address).MaximumLength(250);
    }
  }
}
