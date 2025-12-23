using ApiSitemaClinico.Clinic.Domain.Entities;

namespace ApiSitemaClinico.Clinic.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task UpdateAsync(User user);
        Task<User?> GetByRefreshTokenAsync(string refreshToken);
    }
}
