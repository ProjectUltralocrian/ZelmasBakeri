using System.Net;
using System.Net.Mail;

namespace ZelmasBakeriBackend.Services;


public class GmailSender : IEmailSender
{
    public string FromAddress { get; init; } = "";
    public string SmtpHost { get; init; } = "smtp.gmail.com";
    public int SmtpPort { get; init; } = 587;
    public string UserName { get; init; } = "";
    public string Password { get; init; } = "";
    public void SendEmail(string toAddress, string subject, string body)
    {
        try
        {
            // Create a new MailMessage object
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(FromAddress);
            mail.To.Add(toAddress);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true; // Set to true if the body contains HTML

            // Create a new SmtpClient object
            SmtpClient smtpClient = new SmtpClient(SmtpHost);
            smtpClient.Port = SmtpPort;
            smtpClient.EnableSsl = true; // Enable SSL for secure connection
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.UseDefaultCredentials = false; // Important for providing specific credentials
            smtpClient.Credentials = new NetworkCredential(UserName, Password);

            // Send the email
            smtpClient.Send(mail);
            Console.WriteLine("Email sent successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending email: {ex.Message}");
        }
    }

}