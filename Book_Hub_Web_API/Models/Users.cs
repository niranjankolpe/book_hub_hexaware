using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Book_Hub_Web_API.Data.Enums;

namespace Book_Hub_Web_API.Models
{
    public class Users
    {
        [Key]
        [Required]
        [RegularExpression(@"^[-+]?\d+$", ErrorMessage = "Only integer value allowed!")]
        public int UserId { get; set; }

        [Required]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Only text characters allowed!")]
        public string? Name { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+(?:\.[a-zA-Z0-9._%+-]+)*@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email format.")]
        public string? Email { get; set; }

        [Required]
        [RegularExpression(@"^[-+]?\d+$", ErrorMessage = "Only integer value allowed!")]
        [Length(10, 10, ErrorMessage = "Phone number must be of 10 digits only")]
        public string? Phone { get; set; }

        [Required]
        public string? Address { get; set; }

        [Required]
        public string? PasswordHash { get; set; }

        [Required]
        public User_Role Role { get; set; } = User_Role.Consumer;

        [Required]
        public DateOnly AccountCreatedDate { get; set; }

        // Navigation Properties

        public ICollection<Borrowed>? Borrowed { get; set; }

        public ICollection<Reservations>? Reservations { get; set; }

        public ICollection<Notifications>? Notifications { get; set; }

        public ICollection<LogUserActivity>? LogUserActivity { get; set; }
    }
}
