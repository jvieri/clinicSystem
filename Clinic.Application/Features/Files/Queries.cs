using ApiSitemaClinico.Clinic.Application.Common;
using ApiSitemaClinico.Clinic.Domain.Entities;
using ApiSitemaClinico.Clinic.Domain.Interfaces;
using MediatR;

namespace ApiSitemaClinico.Clinic.Application.Features.Files
{
  public record GetFilesByPatientQuery(long PatientId) : IRequest<Result<List<ClinicalFile>>>;
  public sealed class FileDownloadDto
  {
    public System.IO.Stream Stream { get; set; } = null!;
    public string ContentType { get; set; } = null!;
    public string FileName { get; set; } = null!;
  }

  public record DownloadFileQuery(long Id) : IRequest<Result<FileDownloadDto>>;

  public class GetFilesByPatientHandler : IRequestHandler<GetFilesByPatientQuery, Result<List<ClinicalFile>>>
  {
    private readonly IFileRepository _repo;
    public GetFilesByPatientHandler(IFileRepository repo) { _repo = repo; }
    public async Task<Result<List<ClinicalFile>>> Handle(GetFilesByPatientQuery request, CancellationToken cancellationToken)
    {
      var list = await _repo.GetByPatientAsync(request.PatientId, cancellationToken);
      return Result<List<ClinicalFile>>.Success(list);
    }
  }

  public class DownloadFileHandler : IRequestHandler<DownloadFileQuery, Result<FileDownloadDto>>
  {
    private readonly IFileRepository _repo;
    private readonly ApiSitemaClinico.Clinic.Infrastructure.Services.IFileStorageService _storage;

    public DownloadFileHandler(IFileRepository repo, ApiSitemaClinico.Clinic.Infrastructure.Services.IFileStorageService storage)
    {
      _repo = repo;
      _storage = storage;
    }

    public async Task<Result<FileDownloadDto>> Handle(DownloadFileQuery request, CancellationToken cancellationToken)
    {
      var f = await _repo.GetByIdAsync(request.Id, cancellationToken);
      if (f == null) return Result<FileDownloadDto>.Failure("File not found");
      var stream = await _storage.GetFileAsync(f.Path);
      var dto = new FileDownloadDto { Stream = stream, ContentType = f.ContentType, FileName = f.FileName };
      return Result<FileDownloadDto>.Success(dto);
    }
  }
}
