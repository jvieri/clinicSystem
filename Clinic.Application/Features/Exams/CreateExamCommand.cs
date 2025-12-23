using ApiSitemaClinico.Clinic.Application.Common;
using ApiSitemaClinico.Clinic.Domain.Entities;
using ApiSitemaClinico.Clinic.Domain.Interfaces;
using MediatR;

namespace ApiSitemaClinico.Clinic.Application.Features.Exams
{
  public record CreateExamCommand(long PatientId, string Type, string? Result, DateTime? PerformedAt) : IRequest<Result<Exam>>;

  public class CreateExamCommandHandler : IRequestHandler<CreateExamCommand, Result<Exam>>
  {
    private readonly IUnitOfWork _uow;

    public CreateExamCommandHandler(IUnitOfWork uow)
    {
      _uow = uow;
    }

    public async Task<Result<Exam>> Handle(CreateExamCommand request, CancellationToken cancellationToken)
    {
      var exam = new Exam
      {
        PatientId = request.PatientId,
        Type = request.Type,
        Result = request.Result,
        PerformedAt = request.PerformedAt
      };

      await _uow.ExamRepository.AddAsync(exam, cancellationToken);
      await _uow.SaveChangesAsync(cancellationToken);

      return Result<Exam>.Success(exam);
    }
  }
}
