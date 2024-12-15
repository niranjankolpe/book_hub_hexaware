using Book_Hub_Web_API.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace Book_Hub_Web_API.Models
{
    public class ContactUs
    {
        [Key]
        [Required]
        public int QueryId { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+(?:\.[a-zA-Z0-9._%+-]+)*@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email format.")]
        public string? Email { get; set; }

        [Required]
        public Contact_Us_Query_Type Query_Type { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z0-9\s.,]*$")]
        public string? Description { get; set; }

        [Required]
        public DateOnly QueryCreatedDate { get; set; }

        [Required]
        public Contact_Us_Query_Status Query_Status { get; set; } = Contact_Us_Query_Status.Pending;
    }
}
