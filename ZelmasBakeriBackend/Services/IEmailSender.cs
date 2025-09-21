namespace ZelmasBakeriBackend.Services;

public interface IEmailSender
{
    Task SendEmailAsync(string toAddress, string subject, string body);
}

