using MassTransit;
using MicroExample.Common.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using ILogger = Serilog.ILogger;

namespace MicroExample.Common.Extensions;

public static class ServiceCollectionExtensions
{
    private static readonly LoggerConfiguration LoggerConfiguration =
        new LoggerConfiguration().WriteTo.Console().MinimumLevel.Information();
    
    public static IServiceCollection AddMasstransit(this IServiceCollection serviceCollection,
        ServiceBusConnectionConfiguration serviceBusConnectionConfiguration, bool isDevelopment)
    {
        serviceCollection.AddMassTransit(config =>
        {
            if (isDevelopment)
            {
                config.UsingInMemory((context, configurator) =>
                {
                    configurator.ConfigureEndpoints(context);
                });
            }
            else
            {
                config.UsingRabbitMq((context, configurator) =>
                {
                    configurator.Host(new Uri(serviceBusConnectionConfiguration.Host), rabbitConfiguratior =>
                    {
                        rabbitConfiguratior.Username(serviceBusConnectionConfiguration.Username);
                        rabbitConfiguratior.Password(serviceBusConnectionConfiguration.Password);
                    });

                    configurator.ConfigureEndpoints(context);
                });
            }
        });

        return serviceCollection;
    }

    public static IServiceCollection AddSerilog(this IServiceCollection serviceCollection,
        IConfiguration configuration, ILoggingBuilder loggingBuilder)
    {
        ILogger logger = LoggerConfiguration.ReadFrom.Configuration(configuration).CreateLogger();
        
        loggingBuilder.ClearProviders();
        serviceCollection.AddSingleton((_) => logger);

        loggingBuilder.AddSerilog(logger);

        return serviceCollection;
    }
}