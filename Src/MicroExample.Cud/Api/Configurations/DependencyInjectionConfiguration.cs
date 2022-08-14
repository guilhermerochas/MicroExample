using System.Reflection;
using MediatR;
using MicroExample.Common.Configurations;
using MicroExample.Common.Extensions;

namespace Api.Configurations;

public static class DependencyInjectionConfiguration
{
    public static void AddDependencyInjectionConfiguration(this WebApplicationBuilder builder)
    {
        var busConfiguration = builder.Configuration.GetSection("Configurations:ServiceBus").Get<ServiceBusConnectionConfiguration>();

        builder.Services.AddSerilog(builder.Configuration, builder.Logging);
        builder.Services.AddMasstransit(busConfiguration, false);

        var infrastructureAssembly = Assembly.GetExecutingAssembly().GetReferencedAssemblies().FirstOrDefault(assembly => assembly.Name is "Infrastructure");

        if (infrastructureAssembly is not null)
        {
            builder.Services.AddMediatR(Assembly.Load(infrastructureAssembly));   
        }
    }
}