using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Book_Hub_Web_API.Data.Enums;

namespace Book_Hub_Web_API.Models
{
    public class Notifications
    {
        [Key]
        [Required]
        [RegularExpression(@"^[-+]?\d+$", ErrorMessage = "Only integer value allowed!")]
        public int NotificationId { get; set; }

        [Required]
        [RegularExpression(@"^[-+]?\d+$", ErrorMessage = "Only integer value allowed!")]
        public int UserId { get; set; }

        [Required]
        public Notification_Type MessageType { get; set; } = Notification_Type.Other;

        [Required]
        public string? MessageDescription { get; set; }

        [Required]
        public DateOnly SentDate { get; set; }

        // Navigation Properties
        public Users? Users { get; set; }
    }
}
