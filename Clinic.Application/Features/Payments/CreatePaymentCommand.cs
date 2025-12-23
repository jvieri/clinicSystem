using ApiSitemaClinico.Clinic.Application.Common;
using ApiSitemaClinico.Clinic.Domain.Entities;
using ApiSitemaClinico.Clinic.Domain.Interfaces;
using MediatR;

namespace ApiSitemaClinico.Clinic.Application.Features.Payments
{
  public record PaymentItem(string Concept, long? ReferenceId, decimal UnitPrice, int Quantity);

  public record CreatePaymentCommand(long PatientId, long? AppointmentId, List<PaymentItem> Items, string Method) : IRequest<Result<Payment>>;

  public class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, Result<Payment>>
  {
    private readonly IPaymentRepository _repo;

    public CreatePaymentCommandHandler(IPaymentRepository repo)
    {
      _repo = repo;
    }

    public async Task<Result<Payment>> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
    {
      if (request.Items == null || !request.Items.Any())
        return Result<Payment>.Failure("No items provided");

      var payment = new Payment
      {
        PatientId = request.PatientId,
        AppointmentId = request.AppointmentId,
        Method = Enum.TryParse<PaymentMethod>(request.Method, true, out var m) ? m : PaymentMethod.Cash,
        Status = PaymentStatus.Pending,
        Total = 0m,
        PaidAmount = 0m
      };

      foreach (var it in request.Items)
      {
        var pd = new PaymentDetail
        {
          Concept = it.Concept,
          ReferenceId = it.ReferenceId,
          UnitPrice = it.UnitPrice,
          Quantity = it.Quantity
        };
        payment.Details.Add(pd);
        payment.Total += pd.Subtotal;
      }

      var created = await _repo.AddAsync(payment, cancellationToken);
      return Result<Payment>.Success(created);
    }
  }
}
