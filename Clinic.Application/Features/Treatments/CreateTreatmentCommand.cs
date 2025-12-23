using ApiSitemaClinico.Clinic.Application.Common;
using ApiSitemaClinico.Clinic.Domain.Entities;
using ApiSitemaClinico.Clinic.Domain.Interfaces;
using MediatR;

namespace ApiSitemaClinico.Clinic.Application.Features.Treatments
{
  public record CreateTreatmentCommand(long PatientId, string Description, DateTime StartDate, DateTime? EndDate, long PrescribedBy) : IRequest<Result<Treatment>>;

  public class CreateTreatmentCommandHandler : IRequestHandler<CreateTreatmentCommand, Result<Treatment>>
  {
    private readonly IUnitOfWork _uow;

    public CreateTreatmentCommandHandler(IUnitOfWork uow)
    {
      _uow = uow;
    }

    public async Task<Result<Treatment>> Handle(CreateTreatmentCommand request, CancellationToken cancellationToken)
    {
      var t = new Treatment
      {
        PatientId = request.PatientId,
        Description = request.Description,
        StartDate = request.StartDate,
        EndDate = request.EndDate,
        PrescribedBy = request.PrescribedBy
      };

      await _uow.TreatmentRepository.AddAsync(t, cancellationToken);
      await _uow.SaveChangesAsync(cancellationToken);

      return Result<Treatment>.Success(t);
    }
  }
}
