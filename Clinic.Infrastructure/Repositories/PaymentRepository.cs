using ApiSitemaClinico.Clinic.Domain.Entities;
using ApiSitemaClinico.Clinic.Domain.Interfaces;
using ApiSitemaClinico.Clinic.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ApiSitemaClinico.Clinic.Infrastructure.Repositories
{
  public class PaymentRepository : IPaymentRepository
  {
    private readonly ClinicDbContext _context;

    public PaymentRepository(ClinicDbContext context)
    {
      _context = context;
    }

    public async Task<Payment> AddAsync(Payment payment, CancellationToken cancellationToken = default)
    {
      await _context.Set<Payment>().AddAsync(payment, cancellationToken);
      await _context.SaveChangesAsync(cancellationToken);
      return payment;
    }

    public async Task<Payment?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
      return await _context.Set<Payment>().Include(p => p.Details).FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task UpdateAsync(Payment payment, CancellationToken cancellationToken = default)
    {
      _context.Set<Payment>().Update(payment);
      await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<Payment>> GetByPatientAsync(long patientId, CancellationToken cancellationToken = default)
    {
      return await _context.Set<Payment>().Where(p => p.PatientId == patientId).Include(p => p.Details).ToListAsync(cancellationToken);
    }
  }
}
