using ApiSitemaClinico.Clinic.Domain.Entities;
using ApiSitemaClinico.Clinic.Domain.Interfaces;
using ApiSitemaClinico.Clinic.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ApiSitemaClinico.Clinic.Infrastructure.Repositories
{
  public class TreatmentRepository : ITreatmentRepository
  {
    private readonly ClinicDbContext _context;

    public TreatmentRepository(ClinicDbContext context)
    {
      _context = context;
    }

    public async Task AddAsync(Treatment treatment, CancellationToken cancellationToken = default)
    {
      await _context.Set<Treatment>().AddAsync(treatment, cancellationToken);
      await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<Treatment>> GetAllAsync(CancellationToken cancellationToken = default)
    {
      return await _context.Set<Treatment>().ToListAsync(cancellationToken);
    }

    public async Task<Treatment?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
      return await _context.Set<Treatment>().FindAsync(new object[] { id }, cancellationToken);
    }
  }
}
