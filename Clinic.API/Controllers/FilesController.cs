using ApiSitemaClinico.Clinic.Application.Features.Files;
using ApiSitemaClinico.Clinic.Application.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ApiSitemaClinico.Clinic.API.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class FilesController : ControllerBase
  {
    private readonly IMediator _mediator;

    public FilesController(IMediator mediator)
    {
      _mediator = mediator;
    }

    [HttpPost("upload")]
    [Authorize(Roles = "Admin,Doctor,Enfermero,Paciente")]
    public async Task<ActionResult<Result<object>>> Upload([FromForm] UploadFileCommand command)
    {
      var res = await _mediator.Send(command);
      return res.IsSuccess ? Ok(res) : BadRequest(res.Error);
    }

    [HttpGet("patient/{patientId}")]
    [Authorize(Roles = "Admin,Doctor,Enfermero,Paciente")]
    public async Task<ActionResult<Result<object>>> ListByPatient(long patientId)
    {
      var res = await _mediator.Send(new GetFilesByPatientQuery(patientId));
      return res.IsSuccess ? Ok(res) : BadRequest(res.Error);
    }

    [HttpGet("download/{id}")]
    [Authorize(Roles = "Admin,Doctor,Enfermero,Paciente")]
    public async Task<IActionResult> Download(long id)
    {
      var res = await _mediator.Send(new DownloadFileQuery(id));
      if (!res.IsSuccess) return BadRequest(res.Error);
      var dto = res.Value!;
      return File(dto.Stream, dto.ContentType, dto.FileName);
    }
  }
}
