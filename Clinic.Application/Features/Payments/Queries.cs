using ApiSitemaClinico.Clinic.Application.Common;
using ApiSitemaClinico.Clinic.Domain.Entities;
using ApiSitemaClinico.Clinic.Domain.Interfaces;
using MediatR;

namespace ApiSitemaClinico.Clinic.Application.Features.Payments
{
  public record GetPaymentsByPatientQuery(long PatientId) : IRequest<Result<List<Payment>>>;

  public class GetPaymentsByPatientHandler : IRequestHandler<GetPaymentsByPatientQuery, Result<List<Payment>>>
  {
    private readonly IPaymentRepository _repo;
    public GetPaymentsByPatientHandler(IPaymentRepository repo) { _repo = repo; }
    public async Task<Result<List<Payment>>> Handle(GetPaymentsByPatientQuery request, CancellationToken cancellationToken)
    {
      var list = await _repo.GetByPatientAsync(request.PatientId, cancellationToken);
      return Result<List<Payment>>.Success(list);
    }
  }
}
