using ApiSitemaClinico.Clinic.Domain.Entities;
using ApiSitemaClinico.Clinic.Domain.Interfaces;
using ApiSitemaClinico.Clinic.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ApiSitemaClinico.Clinic.Infrastructure.Repositories
{
  public class PatientRepository : IPatientRepository
  {
    private readonly ClinicDbContext _context;

    public PatientRepository(ClinicDbContext context)
    {
      _context = context;
    }

    public async Task<Patient> AddAsync(Patient patient, CancellationToken cancellationToken = default)
    {
      await _context.Patients.AddAsync(patient, cancellationToken);
      await _context.SaveChangesAsync(cancellationToken);
      return patient;
    }

    public async Task DeleteAsync(Patient patient, CancellationToken cancellationToken = default)
    {
      _context.Patients.Remove(patient);
      await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<Patient>> GetAllAsync(CancellationToken cancellationToken = default)
    {
      return await _context.Patients.ToListAsync(cancellationToken);
    }

    public async Task<Patient?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
      return await _context.Patients.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task UpdateAsync(Patient patient, CancellationToken cancellationToken = default)
    {
      _context.Patients.Update(patient);
      await _context.SaveChangesAsync(cancellationToken);
    }
  }
}
