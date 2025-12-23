using ApiSitemaClinico.Clinic.Application.Common;
using ApiSitemaClinico.Clinic.Domain.Entities;
using ApiSitemaClinico.Clinic.Domain.Interfaces;
using MediatR;

namespace ApiSitemaClinico.Clinic.Application.Features.Patients
{
  public record GetAllPatientsQuery() : IRequest<Result<List<Patient>>>;
  public record GetPatientByIdQuery(long Id) : IRequest<Result<Patient?>>;

  public class GetAllPatientsHandler : IRequestHandler<GetAllPatientsQuery, Result<List<Patient>>>
  {
    private readonly IPatientRepository _repo;
    public GetAllPatientsHandler(IPatientRepository repo) { _repo = repo; }
    public async Task<Result<List<Patient>>> Handle(GetAllPatientsQuery request, CancellationToken cancellationToken)
    {
      var list = await _repo.GetAllAsync(cancellationToken);
      return Result<List<Patient>>.Success(list);
    }
  }

  public class GetPatientByIdHandler : IRequestHandler<GetPatientByIdQuery, Result<Patient?>>
  {
    private readonly IPatientRepository _repo;
    public GetPatientByIdHandler(IPatientRepository repo) { _repo = repo; }
    public async Task<Result<Patient?>> Handle(GetPatientByIdQuery request, CancellationToken cancellationToken)
    {
      var p = await _repo.GetByIdAsync(request.Id, cancellationToken);
      return Result<Patient?>.Success(p);
    }
  }
}
