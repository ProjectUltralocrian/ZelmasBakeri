using Microsoft.Extensions.Logging;

namespace ZelmasBakeriBackend.Services;

public class DummyEmailSender(ILogger<DummyEmailSender> logger) : IEmailSender
{
    public async Task SendEmailAsync(string toAddress, string subject, string body)
    {
        logger.LogWarning("Sending dummy email to {Address}, with subject: {Subject}, and body: {Body}",
            toAddress, subject, body);
    }
}
