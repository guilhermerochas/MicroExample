using MicroExample.Common.Commands;

namespace MicroExample.EmailService.Services;

public interface IMailingService
{
    public Task SendEmail(SendEmailCommand sendEmailCommand);
}