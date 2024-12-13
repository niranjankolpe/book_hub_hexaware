using System.Net.Mail;

namespace Book_Hub_Web_API.Services
{
    public interface IEmailService
    {
        void SendEmail(List<string> receiverEmailAddressList, string subject, string body);
    }
}
