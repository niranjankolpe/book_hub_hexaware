using Book_Hub_Web_API.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace Book_Hub_Web_API.Data.DTO
{
    public class Create_User_DTO
    {
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
        [MinLength(8)]
        [MaxLength(20)]
        public string? PasswordHash { get; set; }

       

    }
}
