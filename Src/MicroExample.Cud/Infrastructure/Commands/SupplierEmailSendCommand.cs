using MediatR;

namespace Infrastructure.Commands;

public class SupplierEmailSendCommand : INotification
{
    public string Email { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
}