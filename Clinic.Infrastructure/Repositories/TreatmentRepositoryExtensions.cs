using ApiSitemaClinico.Clinic.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApiSitemaClinico.Clinic.Infrastructure.Repositories
{
  public static class TreatmentRepositoryExtensions
  {
    public static async Task<List<Treatment>> GetAllAsync(this TreatmentRepository repo, CancellationToken cancellationToken = default)
    {
      var ctx = (ApiSitemaClinico.Clinic.Infrastructure.Persistence.ClinicDbContext)typeof(TreatmentRepository)
        .GetField("_context", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
        ?.GetValue(repo);

      return await ctx.Set<Treatment>().ToListAsync(cancellationToken);
    }

    public static async Task<Treatment?> GetByIdAsync(this TreatmentRepository repo, long id, CancellationToken cancellationToken = default)
    {
      var ctx = (ApiSitemaClinico.Clinic.Infrastructure.Persistence.ClinicDbContext)typeof(TreatmentRepository)
        .GetField("_context", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
        ?.GetValue(repo);

      return await ctx.Set<Treatment>().FindAsync(new object[] { id }, cancellationToken);
    }
  }
}
