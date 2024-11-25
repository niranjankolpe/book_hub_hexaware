using System.ComponentModel.DataAnnotations;

namespace Book_Hub_Web_API.Data.DTO
{
    public class Reset_Password_DTO
    {
        public int UserId { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9._%+-]+(?:\.[a-zA-Z0-9._%+-]+)*@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email format.")]
        public string? Email { get; set; }

        public string? OldPassword { get; set; }

        public string? NewPassword { get; set; }
    }
}
