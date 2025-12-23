namespace ApiSitemaClinico.Clinic.Application.Features.Auth
{
  public record LoginUserCommand(string Email, string Password) : MediatR.IRequest<ApiSitemaClinico.Clinic.Application.Common.Result<AuthResponse>>;

  public class LoginUserCommandHandler : MediatR.IRequestHandler<LoginUserCommand, ApiSitemaClinico.Clinic.Application.Common.Result<AuthResponse>>
  {
    private readonly ApiSitemaClinico.Clinic.Application.Interfaces.IAuthService _authService;

    public LoginUserCommandHandler(ApiSitemaClinico.Clinic.Application.Interfaces.IAuthService authService)
    {
      _authService = authService;
    }

    public async Task<ApiSitemaClinico.Clinic.Application.Common.Result<AuthResponse>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
      return await _authService.LoginAsync(request.Email, request.Password, cancellationToken);
    }
  }
}
