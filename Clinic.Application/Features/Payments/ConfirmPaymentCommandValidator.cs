using FluentValidation;

namespace ApiSitemaClinico.Clinic.Application.Features.Payments
{
  public class ConfirmPaymentCommandValidator : AbstractValidator<ConfirmPaymentCommand>
  {
    public ConfirmPaymentCommandValidator()
    {
      RuleFor(x => x.PaymentId).GreaterThan(0);
      RuleFor(x => x.AmountPaid).GreaterThan(0);
    }
  }
}
