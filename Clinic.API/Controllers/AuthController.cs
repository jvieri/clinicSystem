using ApiSitemaClinico.Clinic.Application.Features.Auth;
using ApiSitemaClinico.Clinic.Application.Common;
using ApiSitemaClinico.Clinic.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ApiSitemaClinico.Clinic.API.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class AuthController : ControllerBase
  {
    private readonly IMediator _mediator;
    private readonly IAuthService _authService;

    public AuthController(IMediator mediator, IAuthService authService)
    {
      _mediator = mediator;
      _authService = authService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<Result<AuthResponse>>> Login(LoginUserCommand command)
    {
      var result = await _mediator.Send(command);
      return result.IsSuccess ? Ok(result) : BadRequest(result.Error);
    }

    [HttpPost("refresh")]
    public async Task<ActionResult<Result<AuthResponse>>> Refresh([FromBody] RefreshTokenRequest request)
    {
      var result = await _authService.RefreshTokenAsync(request.RefreshToken);
      return result.IsSuccess ? Ok(result) : BadRequest(result.Error);
    }
  }
}
