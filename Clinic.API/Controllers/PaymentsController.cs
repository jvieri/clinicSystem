using ApiSitemaClinico.Clinic.Application.Features.Payments;
using ApiSitemaClinico.Clinic.Application.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiSitemaClinico.Clinic.API.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class PaymentsController : ControllerBase
  {
    private readonly IMediator _mediator;

    public PaymentsController(IMediator mediator) { _mediator = mediator; }

    [HttpPost]
    [Authorize(Roles = "Admin,Secretaria,Doctor")]
    public async Task<ActionResult<Result<object>>> Create([FromBody] CreatePaymentCommand command)
    {
      var res = await _mediator.Send(command);
      return res.IsSuccess ? Ok(res) : BadRequest(res.Error);
    }

    [HttpPost("confirm")]
    [Authorize(Roles = "Admin,Secretaria")]
    public async Task<ActionResult<Result<object>>> Confirm([FromBody] ConfirmPaymentCommand command)
    {
      var res = await _mediator.Send(command);
      return res.IsSuccess ? Ok(res) : BadRequest(res.Error);
    }

    [HttpGet("patient/{patientId}")]
    [Authorize(Roles = "Admin,Doctor,Secretaria,Paciente")]
    public async Task<ActionResult<Result<object>>> GetByPatient(long patientId)
    {
      var res = await _mediator.Send(new GetPaymentsByPatientQuery(patientId));
      return res.IsSuccess ? Ok(res) : BadRequest(res.Error);
    }

    [HttpGet("{paymentId}/receipt")]
    [Authorize(Roles = "Admin,Secretaria,Doctor,Paciente")]
    public async Task<IActionResult> Receipt(long paymentId)
    {
      var res = await _mediator.Send(new ApiSitemaClinico.Clinic.Application.Features.Payments.GetPaymentsByPatientQuery(0));
      // get payment directly via repo handler for simplicity
      var p = await _mediator.Send(new ApiSitemaClinico.Clinic.Application.Features.Payments.GetPaymentsByPatientQuery(0));
      // Instead query payment repository
      var payment = await HttpContext.RequestServices.GetRequiredService<ApiSitemaClinico.Clinic.Domain.Interfaces.IPaymentRepository>().GetByIdAsync(paymentId);
      if (payment == null) return NotFound();
      var pdf = ApiSitemaClinico.Clinic.Application.Features.Payments.PdfGenerator.GeneratePaymentReceipt(payment);
      return File(pdf, "application/pdf", $"receipt_{payment.Id}.pdf");
    }
  }
}
