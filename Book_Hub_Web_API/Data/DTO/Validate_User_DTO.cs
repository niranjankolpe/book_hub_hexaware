using System.ComponentModel.DataAnnotations;

namespace Book_Hub_Web_API.Data.DTO
{
    public class Validate_User_DTO
    {
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+(?:\.[a-zA-Z0-9._%+-]+)*@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email format.")]
        public string? Email { get; set; }

        [Required]
        [MinLength(8)]
        [MaxLength(20)]
        public string? PasswordHash { get; set; }

    }
}
