using Infrastructure.Commands;
using MassTransit;
using MediatR;
using MicroExample.Common.Commands;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("[controller]/[action]")]
[ApiController]
public class MailerController : ControllerBase
{
    private readonly IMediator _mediator;

    public MailerController(IMediator mediator)
    {
        _mediator = mediator; 
    }

    [HttpPost]
    public async Task<IActionResult> PostSupplierSendEmail(SupplierEmailSendCommand supplierEmailSendCommand)
    {
        await _mediator.Publish(supplierEmailSendCommand);
        return Ok();
    }
}