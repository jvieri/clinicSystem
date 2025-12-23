using ApiSitemaClinico.Clinic.Application.Features.Dashboard;
using ApiSitemaClinico.Clinic.Application.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiSitemaClinico.Clinic.API.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  [Authorize]
  public class DashboardController : ControllerBase
  {
    private readonly IMediator _mediator;

    public DashboardController(IMediator mediator)
    {
      _mediator = mediator;
    }

    [HttpGet("today-summary")]
    public async Task<ActionResult<Result<TodaySummaryDto>>> GetTodaySummary()
    {
      // Query handler not implemented yet; return placeholder empty result
      var dto = new TodaySummaryDto(new List<PatientTodayDto>(), new DailyStatsDto(0,0,0,0,0));
      return Ok(Result<TodaySummaryDto>.Success(dto));
    }
  }
}
