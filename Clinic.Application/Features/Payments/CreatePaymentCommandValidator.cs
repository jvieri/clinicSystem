using FluentValidation;

namespace ApiSitemaClinico.Clinic.Application.Features.Payments
{
  public class CreatePaymentCommandValidator : AbstractValidator<CreatePaymentCommand>
  {
    public CreatePaymentCommandValidator()
    {
      RuleFor(x => x.PatientId).GreaterThan(0);
      RuleFor(x => x.Items).NotEmpty().WithMessage("El pago debe contener al menos un item.");
      RuleForEach(x => x.Items).ChildRules(items =>
      {
        items.RuleFor(i => i.Concept).NotEmpty();
        items.RuleFor(i => i.UnitPrice).GreaterThan(0);
        items.RuleFor(i => i.Quantity).GreaterThan(0);
      });
      RuleFor(x => x.Method).NotEmpty().Must(m =>
      {
        return System.Enum.TryParse<ApiSitemaClinico.Clinic.Domain.Entities.PaymentMethod>(m, true, out _);
      }).WithMessage("Método de pago inválido.");
    }
  }
}
