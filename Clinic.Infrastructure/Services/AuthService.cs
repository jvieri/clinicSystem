using ApiSitemaClinico.Clinic.Domain.Interfaces;
using ApiSitemaClinico.Clinic.Application.Interfaces;
using ApiSitemaClinico.Clinic.Application.Features.Auth;
using ApiSitemaClinico.Clinic.Application.Common;
using ApiSitemaClinico.Clinic.Domain.Entities;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Cryptography;


namespace ApiSitemaClinico.Clinic.Infrastructure.Services
{
  public class AuthService : ApiSitemaClinico.Clinic.Application.Interfaces.IAuthService
  {
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;

    public AuthService(IUserRepository userRepository, IConfiguration configuration)
    {
      _userRepository = userRepository;
      _configuration = configuration;
    }

    public async Task<Result<AuthResponse>> LoginAsync(string email, string password, CancellationToken ct = default)
    {
      var user = await _userRepository.GetByEmailAsync(email);
      if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
        return Result<AuthResponse>.Failure("Credenciales inválidas.");

      var token = GenerateJwtToken(user);
      var refreshToken = GenerateRefreshToken();

      user.RefreshToken = refreshToken;
      user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
      await _userRepository.UpdateAsync(user);

      return Result<AuthResponse>.Success(new AuthResponse(token, refreshToken));
    }

    public async Task<Result<AuthResponse>> RefreshTokenAsync(string refreshToken, CancellationToken ct = default)
    {
      var user = await _userRepository.GetByRefreshTokenAsync(refreshToken);
      if (user == null || user.RefreshTokenExpiry == null || user.RefreshTokenExpiry < DateTime.UtcNow)
        return Result<AuthResponse>.Failure("Refresh token inválido o expirado.");

      var token = GenerateJwtToken(user);
      var newRefresh = GenerateRefreshToken();

      user.RefreshToken = newRefresh;
      user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
      await _userRepository.UpdateAsync(user);

      return Result<AuthResponse>.Success(new AuthResponse(token, newRefresh));
    }

    private string GenerateRefreshToken()
    {
      return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }

    private string GenerateJwtToken(User user)
    {
      var key = _configuration["Jwt:Key"] ?? "fallback_secret_key";
      var issuer = _configuration["Jwt:Issuer"] ?? "clinic";

      var claims = new[] {
        new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
        new Claim(ClaimTypes.Email, user.Email),
        new Claim(ClaimTypes.Role, user.Role?.Name ?? string.Empty)
      };

      var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
      var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

      var token = new JwtSecurityToken(
          issuer,
          issuer,
          claims,
          expires: DateTime.UtcNow.AddMinutes(30),
          signingCredentials: creds
      );

      return new JwtSecurityTokenHandler().WriteToken(token);
    }
  }
}
