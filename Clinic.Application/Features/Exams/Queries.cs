using ApiSitemaClinico.Clinic.Application.Common;
using ApiSitemaClinico.Clinic.Domain.Entities;
using ApiSitemaClinico.Clinic.Domain.Interfaces;
using MediatR;

namespace ApiSitemaClinico.Clinic.Application.Features.Exams
{
  public record GetAllExamsQuery() : IRequest<Result<List<Exam>>>;
  public record GetExamByIdQuery(long Id) : IRequest<Result<Exam?>>;

  public class GetAllExamsHandler : IRequestHandler<GetAllExamsQuery, Result<List<Exam>>>
  {
    private readonly IExamRepository _repo;
    public GetAllExamsHandler(IExamRepository repo) { _repo = repo; }
    public async Task<Result<List<Exam>>> Handle(GetAllExamsQuery request, CancellationToken cancellationToken)
    {
      var list = await _repo.GetAllAsync(cancellationToken);
      return Result<List<Exam>>.Success(list);
    }
  }

  public class GetExamByIdHandler : IRequestHandler<GetExamByIdQuery, Result<Exam?>>
  {
    private readonly IExamRepository _repo;
    public GetExamByIdHandler(IExamRepository repo) { _repo = repo; }
    public async Task<Result<Exam?>> Handle(GetExamByIdQuery request, CancellationToken cancellationToken)
    {
      var e = await _repo.GetByIdAsync(request.Id, cancellationToken);
      return Result<Exam?>.Success(e);
    }
  }
}
