using System.ComponentModel.DataAnnotations;

namespace Book_Hub_Web_API.Data.DTO
{
    public class UpdateUser_DTO
    {
        [Required]
        public int UserId { get; set; }

        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Only text characters allowed!")]
        public string? Name { get; set; }

        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be exactly 10 digits.")]
        public string? Phone { get; set; }

        public string? Address { get; set; }

    }
}
