using Book_Hub_Web_API.Data.Enums;

namespace Book_Hub_Web_API.Services
{
    public interface INotificationService
    {
        void SendNotification(int userId, Notification_Type messageType, string messageDescription);
    }
}
