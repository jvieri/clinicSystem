using ApiSitemaClinico.Clinic.Application.Common;
using ApiSitemaClinico.Clinic.Application.Features.Auth;
using System.Threading;

namespace ApiSitemaClinico.Clinic.Application.Interfaces
{
  public interface IAuthService
  {
    Task<Result<AuthResponse>> LoginAsync(string email, string password, CancellationToken ct = default);
    Task<Result<AuthResponse>> RefreshTokenAsync(string refreshToken, CancellationToken ct = default);
  }
}
