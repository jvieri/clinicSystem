using ApiSitemaClinico.Clinic.Application.Features.Patients;
using ApiSitemaClinico.Clinic.Application.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ApiSitemaClinico.Clinic.API.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class PatientsController : ControllerBase
  {
    private readonly IMediator _mediator;

    public PatientsController(IMediator mediator)
    {
      _mediator = mediator;
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Doctor,Enfermero")]
    public async Task<ActionResult<Result<object>>> Create([FromBody] CreatePatientCommand command)
    {
      var res = await _mediator.Send(command);
      return res.IsSuccess ? Ok(res) : BadRequest(res.Error);
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Doctor,Enfermero")]
    public async Task<ActionResult<Result<object>>> GetAll()
    {
      var res = await _mediator.Send(new GetAllPatientsQuery());
      return res.IsSuccess ? Ok(res) : BadRequest(res.Error);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Doctor,Enfermero")]
    public async Task<ActionResult<Result<object>>> Get(long id)
    {
      var res = await _mediator.Send(new GetPatientByIdQuery(id));
      return res.IsSuccess ? Ok(res) : BadRequest(res.Error);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Doctor")]
    public async Task<ActionResult<Result<object>>> Update(long id, [FromBody] UpdatePatientCommand command)
    {
      var res = await _mediator.Send(command with { Id = id });
      return res.IsSuccess ? Ok(res) : BadRequest(res.Error);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Result<object>>> Delete(long id)
    {
      var res = await _mediator.Send(new DeletePatientCommand(id));
      return res.IsSuccess ? Ok(res) : BadRequest(res.Error);
    }
  }
}
