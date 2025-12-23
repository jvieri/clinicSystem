using ApiSitemaClinico.Clinic.Application.Features.Treatments;
using ApiSitemaClinico.Clinic.Application.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ApiSitemaClinico.Clinic.API.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class TreatmentsController : ControllerBase
  {
    private readonly IMediator _mediator;

    public TreatmentsController(IMediator mediator)
    {
      _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<Result<object>>> Create([FromBody] CreateTreatmentCommand command)
    {
      var res = await _mediator.Send(command);
      return res.IsSuccess ? Ok(res) : BadRequest(res.Error);
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Doctor,Enfermero")]
    public async Task<ActionResult<Result<object>>> GetAll()
    {
      var res = await _mediator.Send(new GetAllTreatmentsQuery());
      return res.IsSuccess ? Ok(res) : BadRequest(res.Error);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Doctor,Enfermero")]
    public async Task<ActionResult<Result<object>>> Get(long id)
    {
      var res = await _mediator.Send(new GetTreatmentByIdQuery(id));
      return res.IsSuccess ? Ok(res) : BadRequest(res.Error);
    }
  }
}
