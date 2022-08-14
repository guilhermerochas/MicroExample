using Infrastructure.Commands;
using MassTransit;
using MediatR;
using MicroExample.Common.Commands;
using MicroExample.Common.Endpoints;
using Serilog;

namespace Infrastructure.Handlers;

public class SupplierEmailSendCommandHandler : INotificationHandler<SupplierEmailSendCommand>
{
    private readonly IBus _serviceBus;
    private readonly ILogger _logger;

    public SupplierEmailSendCommandHandler(IBus serviceBus, ILogger logger)
    {
        _serviceBus = serviceBus;
        _logger = logger;
    }

    public async Task Handle(SupplierEmailSendCommand notification, CancellationToken cancellationToken)
    {
        _logger.Information("Sending email to supplier");
        
        var sendEndpoint = await _serviceBus.GetSendEndpoint(new Uri($"queue:{ServiceBusEndpoints.EmailSenderEndpoint}"));
        
        await sendEndpoint.Send(new SendEmailCommand
        {
            Receiver = notification.Email,
            Body = notification.Body,
        }, cancellationToken);
    }
}