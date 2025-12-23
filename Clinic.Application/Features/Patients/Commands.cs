using ApiSitemaClinico.Clinic.Application.Common;
using ApiSitemaClinico.Clinic.Domain.Entities;
using ApiSitemaClinico.Clinic.Domain.Interfaces;
using MediatR;

namespace ApiSitemaClinico.Clinic.Application.Features.Patients
{
  public record UpdatePatientCommand(long Id, string FirstName, string LastName, DateTime BirthDate, string? Gender, string? Phone, string? Address) : IRequest<Result<Patient>>;
  public record DeletePatientCommand(long Id) : IRequest<Result<bool>>;

  public class UpdatePatientHandler : IRequestHandler<UpdatePatientCommand, Result<Patient>>
  {
    private readonly IPatientRepository _repo;
    public UpdatePatientHandler(IPatientRepository repo) { _repo = repo; }

    public async Task<Result<Patient>> Handle(UpdatePatientCommand request, CancellationToken cancellationToken)
    {
      var p = await _repo.GetByIdAsync(request.Id, cancellationToken);
      if (p == null) return Result<Patient>.Failure("Paciente no encontrado");
      p.FirstName = request.FirstName;
      p.LastName = request.LastName;
      p.BirthDate = request.BirthDate;
      p.Gender = request.Gender;
      p.Phone = request.Phone;
      p.Address = request.Address;

      await _repo.UpdateAsync(p, cancellationToken);
      return Result<Patient>.Success(p);
    }
  }

  public class DeletePatientHandler : IRequestHandler<DeletePatientCommand, Result<bool>>
  {
    private readonly IPatientRepository _repo;
    public DeletePatientHandler(IPatientRepository repo) { _repo = repo; }
    public async Task<Result<bool>> Handle(DeletePatientCommand request, CancellationToken cancellationToken)
    {
      var p = await _repo.GetByIdAsync(request.Id, cancellationToken);
      if (p == null) return Result<bool>.Failure("Paciente no encontrado");
      await _repo.DeleteAsync(p, cancellationToken);
      return Result<bool>.Success(true);
    }
  }
}
