namespace MicroExample.Common.Commands;

public class SendEmailCommand
{
    public string Receiver { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public IEnumerable<string>? CarbonCopyCc { get; set; } = default;
}