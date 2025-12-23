using ApiSitemaClinico.Clinic.Domain.Entities;
using ApiSitemaClinico.Clinic.Domain.Interfaces;
using ApiSitemaClinico.Clinic.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ApiSitemaClinico.Clinic.Infrastructure.Repositories
{
  public class ExamRepository : IExamRepository
  {
    private readonly ClinicDbContext _context;

    public ExamRepository(ClinicDbContext context)
    {
      _context = context;
    }

    public async Task AddAsync(Exam exam, CancellationToken cancellationToken = default)
    {
      await _context.Set<Exam>().AddAsync(exam, cancellationToken);
      await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<Exam>> GetAllAsync(CancellationToken cancellationToken = default)
    {
      return await _context.Set<Exam>().ToListAsync(cancellationToken);
    }

    public async Task<Exam?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
      return await _context.Set<Exam>().FindAsync(new object[] { id }, cancellationToken);
    }
  }
}
