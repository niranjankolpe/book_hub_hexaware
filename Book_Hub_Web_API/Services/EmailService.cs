
using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using Book_Hub_Web_API.Data;
using Microsoft.Extensions.Configuration;

namespace Book_Hub_Web_API.Services
{
    public class EmailService : IEmailService
    {
        private IConfiguration _configuration;

        private string _hostEmailAddress = string.Empty;

        private string _hostAppPassword = string.Empty;

        private SmtpClient _smtpClient;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
            _hostEmailAddress = _configuration["EmailService:HostEmailAddress"];
            _hostAppPassword = _configuration["EmailService:HostAppPassword"];
            if (_hostEmailAddress == null || _hostAppPassword == null)
            {
                throw new Exception("Host Email or Password for Email Service are empty!");
            }
            _smtpClient = new SmtpClient()
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                Credentials = new NetworkCredential(_hostEmailAddress, _hostAppPassword)
            };
        }

        public void SendEmail(List<string> receiverEmailAddressList, string subject, string body)
        {
            MailMessage email = new MailMessage();
            email.From = new MailAddress(_hostEmailAddress);
            email.Subject = subject;
            receiverEmailAddressList.ForEach(receiverEmailAddress =>
            {
                email.To.Add(new MailAddress(receiverEmailAddress));
            });
            email.Body = body;

            try
            {
                _smtpClient.Send(email);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"\n\nError while sending Email: {ex.Message}\nDetails: {ex}\n\n");
            }
        }
    }
}
