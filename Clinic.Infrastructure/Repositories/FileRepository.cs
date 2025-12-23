using ApiSitemaClinico.Clinic.Domain.Entities;
using ApiSitemaClinico.Clinic.Domain.Interfaces;
using ApiSitemaClinico.Clinic.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ApiSitemaClinico.Clinic.Infrastructure.Repositories
{
  public class FileRepository : IFileRepository
  {
    private readonly ClinicDbContext _context;

    public FileRepository(ClinicDbContext context)
    {
      _context = context;
    }

    public async Task<ClinicalFile> AddAsync(ClinicalFile file, CancellationToken cancellationToken = default)
    {
      await _context.Set<ClinicalFile>().AddAsync(file, cancellationToken);
      await _context.SaveChangesAsync(cancellationToken);
      return file;
    }

    public async Task<ClinicalFile?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
      return await _context.Set<ClinicalFile>().FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<List<ClinicalFile>> GetByPatientAsync(long patientId, CancellationToken cancellationToken = default)
    {
      return await _context.Set<ClinicalFile>().Where(f => f.PatientId == patientId).ToListAsync(cancellationToken);
    }
  }
}
