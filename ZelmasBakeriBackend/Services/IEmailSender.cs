namespace ZelmasBakeriBackend.Services;

public interface IEmailSender
{
    void SendEmail(string toAddress, string subject, string body);
}

