using FluentValidation;

namespace ApiSitemaClinico.Clinic.Application.Features.Exams
{
  public class CreateExamCommandValidator : AbstractValidator<CreateExamCommand>
  {
    public CreateExamCommandValidator()
    {
      RuleFor(x => x.PatientId).GreaterThan(0);
      RuleFor(x => x.Type).NotEmpty().MaximumLength(150);
    }
  }
}
