using FluentEmail.Core;
using MicroExample.EmailService.Configurations;
using MicroExample.EmailService.Services;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IMailingService, MailingService>();

builder.AddDependencyInjectionConfiguration();

var app = builder.Build();

app.MapPost("/SendEmail", async ([FromServices] IFluentEmail fluentEmail) =>
{
    await fluentEmail.To("bob@teste.com").Body("Have a nice day bob").SendAsync();
    return Results.Ok();
});

app.UseSwagger();
app.UseSwaggerUI();

await app.RunAsync();