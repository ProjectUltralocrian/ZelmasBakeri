using System.Net;
using System.Net.Mail;

namespace ZelmasBakeriBackend.Services;


public class GmailSender : IEmailSender
{
    public const string SmtpHost = "smtp.gmail.com";
    public const int SmtpPort = 587;
    public string FromAddress { get; init; } = "";
    public string UserName { get; init; } = "";
    public string Password { get; init; } = "";
    public async Task SendEmailAsync(string toAddress, string subject, string body)
    {
        try
        {
            // Create a new MailMessage object
            MailMessage mail = new()
            {
                From = new MailAddress(FromAddress)
            };
            mail.To.Add(toAddress);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true; // Set to true if the body contains HTML

            // Create a new SmtpClient object
            SmtpClient smtpClient = new(SmtpHost)
            {
                Port = SmtpPort,
                EnableSsl = true, // Enable SSL for secure connection
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false, // Important for providing specific credentials
                Credentials = new NetworkCredential(UserName, Password)
            };

            var userState = $"Email to {toAddress} with subject '{subject}'";

            // Send the email
            smtpClient.SendAsync(mail, userState);
            Console.WriteLine("Email sent successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending email: {ex.Message}");
        }
    }

}