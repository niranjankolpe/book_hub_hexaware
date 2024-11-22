using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Book_Hub_Web_API.Data.Enums;

namespace Book_Hub_Web_API.Models
{
    public class LogUserActivity
    {
        [Key]
        [Required]
        [RegularExpression(@"^[-+]?\d+$", ErrorMessage = "Only integer value allowed!")]
        public int LogId { get; set; }

        [Required]
        [ForeignKey("UserId")]
        [RegularExpression(@"^[-+]?\d+$", ErrorMessage = "Only integer value allowed!")]
        public int UserId { get; set; }

        [Required]
        public Action_Type ActionType { get; set; }

        [Required]
        public DateTime Timestamp { get; set; }

        // Navigation Properties
        public Users? User { get; set; }
    }
}
