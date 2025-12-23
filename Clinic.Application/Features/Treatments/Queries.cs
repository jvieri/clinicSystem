using ApiSitemaClinico.Clinic.Application.Common;
using ApiSitemaClinico.Clinic.Domain.Entities;
using ApiSitemaClinico.Clinic.Domain.Interfaces;
using MediatR;

namespace ApiSitemaClinico.Clinic.Application.Features.Treatments
{
  public record GetAllTreatmentsQuery() : IRequest<Result<List<Treatment>>>;
  public record GetTreatmentByIdQuery(long Id) : IRequest<Result<Treatment?>>;

  public class GetAllTreatmentsHandler : IRequestHandler<GetAllTreatmentsQuery, Result<List<Treatment>>>
  {
    private readonly ITreatmentRepository _repo;
    public GetAllTreatmentsHandler(ITreatmentRepository repo) { _repo = repo; }
    public async Task<Result<List<Treatment>>> Handle(GetAllTreatmentsQuery request, CancellationToken cancellationToken)
    {
      var list = await ((ApiSitemaClinico.Clinic.Infrastructure.Repositories.TreatmentRepository)_repo).GetAllAsync(cancellationToken);
      return Result<List<Treatment>>.Success(list);
    }
  }

  public class GetTreatmentByIdHandler : IRequestHandler<GetTreatmentByIdQuery, Result<Treatment?>>
  {
    private readonly ITreatmentRepository _repo;
    public GetTreatmentByIdHandler(ITreatmentRepository repo) { _repo = repo; }
    public async Task<Result<Treatment?>> Handle(GetTreatmentByIdQuery request, CancellationToken cancellationToken)
    {
      var e = await ((ApiSitemaClinico.Clinic.Infrastructure.Repositories.TreatmentRepository)_repo).GetByIdAsync(request.Id, cancellationToken);
      return Result<Treatment?>.Success(e);
    }
  }
}
