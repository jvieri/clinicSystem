using ApiSitemaClinico.Clinic.Application.Common;
using ApiSitemaClinico.Clinic.Domain.Entities;
using ApiSitemaClinico.Clinic.Domain.Interfaces;
using MediatR;

namespace ApiSitemaClinico.Clinic.Application.Features.Patients
{
  public record CreatePatientCommand(string FirstName, string LastName, DateTime BirthDate, string? Gender, string? Phone, string? Address) : IRequest<Result<Patient>>;

  public class CreatePatientCommandHandler : IRequestHandler<CreatePatientCommand, Result<Patient>>
  {
    private readonly IPatientRepository _repo;

    public CreatePatientCommandHandler(IPatientRepository repo)
    {
      _repo = repo;
    }

    public async Task<Result<Patient>> Handle(CreatePatientCommand request, CancellationToken cancellationToken)
    {
      var p = new Patient
      {
        FirstName = request.FirstName,
        LastName = request.LastName,
        BirthDate = request.BirthDate,
        Gender = request.Gender,
        Phone = request.Phone,
        Address = request.Address
      };

      var created = await _repo.AddAsync(p, cancellationToken);
      return Result<Patient>.Success(created);
    }
  }
}
