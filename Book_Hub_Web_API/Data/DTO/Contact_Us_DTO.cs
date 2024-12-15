using System.ComponentModel.DataAnnotations;
using Book_Hub_Web_API.Data.Enums;

namespace Book_Hub_Web_API.Data.DTO
{
    public class Contact_Us_DTO
    {
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+(?:\.[a-zA-Z0-9._%+-]+)*@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email format.")]
        public string? Email { get; set; }

        [Required]
        public Contact_Us_Query_Type Query_Type { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z0-9\s.,]*$")]
        public string? Description { get; set; }
    }
}
