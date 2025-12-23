namespace ApiSitemaClinico.Clinic.Application.Features.Auth
{
  public sealed class AuthResponse
  {
    public string AccessToken { get; }
    public string RefreshToken { get; }

    public AuthResponse(string accessToken, string refreshToken)
    {
      AccessToken = accessToken;
      RefreshToken = refreshToken;
    }
  }
}
