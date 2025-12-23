using System.Security.Claims;
using ApiSitemaClinico.Clinic.Application.Common;
using Microsoft.AspNetCore.Http;

namespace ApiSitemaClinico.Clinic.Infrastructure.Services
{
  public class CurrentUserService : ICurrentUserService
  {
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
      _httpContextAccessor = httpContextAccessor;
    }

    public long? UserId
    {
      get
      {
        var http = _httpContextAccessor.HttpContext;
        if (http == null) return null;

        var sub = http.User?.FindFirst(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub)?.Value;
        if (string.IsNullOrEmpty(sub))
        {
          sub = http.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        if (long.TryParse(sub, out var id)) return id;
        return null;
      }
    }

    public string? UserRole => _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Role)?.Value;
  }
}
