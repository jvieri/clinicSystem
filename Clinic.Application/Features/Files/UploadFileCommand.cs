using ApiSitemaClinico.Clinic.Application.Common;
using ApiSitemaClinico.Clinic.Domain.Entities;
using ApiSitemaClinico.Clinic.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace ApiSitemaClinico.Clinic.Application.Features.Files
{
  public record UploadFileCommand(IFormFile File, long PatientId, long? AppointmentId) : IRequest<Result<ClinicalFile>>;

  public class UploadFileCommandHandler : IRequestHandler<UploadFileCommand, Result<ClinicalFile>>
  {
    private readonly IFileRepository _repo;
    private readonly ApiSitemaClinico.Clinic.Infrastructure.Services.IFileStorageService _storage;
    private readonly ApiSitemaClinico.Clinic.Application.Common.ICurrentUserService _currentUser;

    public UploadFileCommandHandler(IFileRepository repo, ApiSitemaClinico.Clinic.Infrastructure.Services.IFileStorageService storage, ICurrentUserService currentUser)
    {
      _repo = repo;
      _storage = storage;
      _currentUser = currentUser;
    }

    public async Task<Result<ClinicalFile>> Handle(UploadFileCommand request, CancellationToken cancellationToken)
    {
      var relative = await _storage.SaveFileAsync(request.File, "clinical-files");
      var file = new ClinicalFile
      {
        PatientId = request.PatientId,
        AppointmentId = request.AppointmentId,
        FileName = request.File.FileName,
        Path = relative,
        ContentType = request.File.ContentType,
        Size = request.File.Length,
        UploadedBy = _currentUser.UserId ?? 0,
        UploadedAt = DateTime.UtcNow
      };

      var created = await _repo.AddAsync(file, cancellationToken);
      return Result<ClinicalFile>.Success(created);
    }
  }
}
