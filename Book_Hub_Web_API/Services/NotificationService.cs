using Book_Hub_Web_API.Data;
using Book_Hub_Web_API.Data.Enums;
using Book_Hub_Web_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Book_Hub_Web_API.Services
{
    public class NotificationService : INotificationService
    {
        private readonly BookHubDBContext _dbContext;

        private IEmailService _emailService;


        public NotificationService(BookHubDBContext dbContext, IEmailService emailService)
        {
            _dbContext = dbContext;
            _emailService = emailService;
        }

        //public NotificationService(BookHubDBContext dbContext)
        //{
        //    _dbContext = dbContext;
        //}

        public async void SendNotification(int userId, Notification_Type messageType = Notification_Type.Other, string messageDescription = "No description provided.")
        {
            Notifications notification = new Notifications()
            {
                UserId = userId,
                MessageType = messageType,
                MessageDescription = messageDescription,
                SentDate = DateOnly.FromDateTime(DateTime.Now)
            };

            Users users = _dbContext.Users.First(u => u.UserId == userId);

            List<string> emailRecipients =
            [
                users.Email
            ];

            _dbContext.Notifications.Add(notification);
            await _dbContext.SaveChangesAsync();

            _emailService.SendEmail(receiverEmailAddressList: emailRecipients, subject: notification.MessageType.ToString(), body: messageDescription);
        }
    }
}
