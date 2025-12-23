using ApiSitemaClinico.Clinic.Application.Common;
using Microsoft.AspNetCore.Http;

namespace ApiSitemaClinico.Clinic.Infrastructure.Services
{
  public interface IFileStorageService
  {
    Task<string> SaveFileAsync(IFormFile file, string folder);
    Task<Stream> GetFileAsync(string relativePath);
  }

  public class FileStorageService : IFileStorageService
  {
    private readonly string _root;
    private readonly ILogger<FileStorageService> _logger;

    public FileStorageService(IConfiguration configuration, ILogger<FileStorageService> logger)
    {
      _root = configuration["FileStorage:Root"] ?? Path.Combine(Directory.GetCurrentDirectory(), "storage");
      Directory.CreateDirectory(_root);
      _logger = logger;
    }

    public async Task<string> SaveFileAsync(IFormFile file, string folder)
    {
      var dir = Path.Combine(_root, folder ?? string.Empty);
      Directory.CreateDirectory(dir);
      var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
      var path = Path.Combine(dir, fileName);
      using var stream = File.Create(path);
      await file.CopyToAsync(stream);
      _logger.LogInformation("Saved file {Path}", path);
      return Path.GetRelativePath(_root, path);
    }

    public Task<Stream> GetFileAsync(string relativePath)
    {
      var full = Path.Combine(_root, relativePath);
      var stream = File.OpenRead(full);
      return Task.FromResult<Stream>(stream);
    }
  }
}
