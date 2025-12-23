using ApiSitemaClinico.Clinic.Application.Common;
using ApiSitemaClinico.Clinic.Domain.Entities;
using ApiSitemaClinico.Clinic.Domain.Interfaces;
using MediatR;

namespace ApiSitemaClinico.Clinic.Application.Features.Payments
{
  public record ConfirmPaymentCommand(long PaymentId, decimal AmountPaid) : IRequest<Result<Payment>>;

  public class ConfirmPaymentCommandHandler : IRequestHandler<ConfirmPaymentCommand, Result<Payment>>
  {
    private readonly IPaymentRepository _repo;

    public ConfirmPaymentCommandHandler(IPaymentRepository repo)
    {
      _repo = repo;
    }

    public async Task<Result<Payment>> Handle(ConfirmPaymentCommand request, CancellationToken cancellationToken)
    {
      var p = await _repo.GetByIdAsync(request.PaymentId, cancellationToken);
      if (p == null) return Result<Payment>.Failure("Payment not found");

      p.PaidAmount += request.AmountPaid;
      if (p.PaidAmount >= p.Total) p.Status = PaymentStatus.Paid;
      else if (p.PaidAmount > 0) p.Status = PaymentStatus.PartiallyPaid;

      await _repo.UpdateAsync(p, cancellationToken);
      return Result<Payment>.Success(p);
    }
  }
}
