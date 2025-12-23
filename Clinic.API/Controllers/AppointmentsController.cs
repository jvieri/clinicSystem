using ApiSitemaClinico.Clinic.Application.Features.Appointments;
using ApiSitemaClinico.Clinic.Application.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiSitemaClinico.Clinic.API.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class AppointmentsController : ControllerBase
  {
    private readonly IMediator _mediator;

    public AppointmentsController(IMediator mediator)
    {
      _mediator = mediator;
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Doctor,Enfermero,Paciente")]
    public async Task<ActionResult<Result<ApiSitemaClinico.Clinic.Domain.Entities.Appointment>>> Create([FromBody] CreateAppointmentCommand command)
    {
      var res = await _mediator.Send(command);
      return res.IsSuccess ? Ok(res) : BadRequest(res.Error);
    }

    [HttpPut("reprogram")]
    [Authorize(Roles = "Admin,Doctor")]
    public async Task<ActionResult<Result<ApiSitemaClinico.Clinic.Domain.Entities.Appointment>>> Reprogram([FromBody] ReprogramAppointmentCommand command)
    {
      var res = await _mediator.Send(command);
      return res.IsSuccess ? Ok(res) : BadRequest(res.Error);
    }

    [HttpPost("cancel")]
    [Authorize(Roles = "Admin,Doctor,Enfermero")]
    public async Task<ActionResult<Result<bool>>> Cancel([FromBody] CancelAppointmentCommand command)
    {
      var res = await _mediator.Send(command);
      return res.IsSuccess ? Ok(res) : BadRequest(res.Error);
    }
  }
}
