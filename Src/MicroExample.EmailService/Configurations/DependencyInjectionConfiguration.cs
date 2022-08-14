using System.Net;
using System.Net.Mail;
using MassTransit;
using MicroExample.Common.Configurations;
using MicroExample.Common.Endpoints;
using MicroExample.Common.Extensions;
using MicroExample.EmailService.Handlers;

namespace MicroExample.EmailService.Configurations;

public static class DependencyInjectionConfiguration
{
    public static void AddDependencyInjectionConfiguration(this WebApplicationBuilder builder)
    {
        builder.Services.AddSerilog(builder.Configuration, builder.Logging);
        
        var busConfiguration = builder.Configuration.GetSection("Configurations:ServiceBus").Get<ServiceBusConnectionConfiguration>();

        builder.Services.AddMassTransit(config =>
        {
            config.AddConsumer<SendEmailHandler>();
            
            config.UsingRabbitMq((context, configurator) =>
            {
                configurator.Host(new Uri(busConfiguration.Host), rabbitConfiguratior =>
                {
                    rabbitConfiguratior.Username(busConfiguration.Username);
                    rabbitConfiguratior.Password(busConfiguration.Password);
                });
                
                configurator.ReceiveEndpoint(ServiceBusEndpoints.EmailSenderEndpoint, c =>
                {
                    c.ConfigureConsumer<SendEmailHandler>(context);
                });
            });
        });

        var smtpClient = new SmtpClient
        {
            Host = builder.Configuration.GetValue<string>("Configurations:Email:Host"),
            Port = builder.Configuration.GetValue<int>("Configurations:Email:Port"),
            EnableSsl = true,
        };
        
        builder.Services
            .AddFluentEmail(builder.Configuration.GetValue<string>("Configurations:Email:DefaultEmail"))
            .AddSmtpSender(smtpClient);
    }
}