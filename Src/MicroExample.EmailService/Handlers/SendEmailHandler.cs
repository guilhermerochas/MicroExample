using FluentEmail.Core;
using FluentEmail.Core.Models;
using MassTransit;
using MicroExample.Common.Commands;
using ILogger = Serilog.ILogger;

namespace MicroExample.EmailService.Handlers;

public class SendEmailHandler : IConsumer<SendEmailCommand>
{
    private readonly IFluentEmail _fluentEmail;
    private readonly ILogger _logger;
    
    public SendEmailHandler(IFluentEmail fluentEmail, ILogger logger)
    {
        _fluentEmail = fluentEmail;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<SendEmailCommand> context)
    {
        _logger.Information("Sending email to {Receiver}", context.Message.Receiver);

        var emailBuilder = _fluentEmail.To(context.Message.Receiver).Body(context.Message.Body);

        if (context.Message.CarbonCopyCc is not null)
        {
            emailBuilder.CC(context.Message.CarbonCopyCc?.Select(carbonCopy => new Address
            {
                Name = string.Empty,
                EmailAddress = carbonCopy
            }));
        }

        await emailBuilder.SendAsync();
    }
}