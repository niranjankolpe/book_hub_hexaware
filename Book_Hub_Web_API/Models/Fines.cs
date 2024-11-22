using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Book_Hub_Web_API.Data.Enums;

namespace Book_Hub_Web_API.Models
{
    public class Fines
    {
        [Key]
        [Required]
        [RegularExpression(@"^[-+]?\d+$", ErrorMessage = "Only integer value allowed!")]
        public int FineId { get; set; }

        [Required]
        [ForeignKey("BorrowId")]
        [RegularExpression(@"^[-+]?\d+$", ErrorMessage = "Only integer value allowed!")]
        public int BorrowId { get; set; }

        [Required]
        public Fine_Type FineType { get; set; }

        [Required]
        public decimal FineAmount { get; set; } = 0;

        [Required]
        public Fine_Paid_Status FinePaidStatus { get; set; }

        public DateOnly? PaidDate { get; set; }

        // Navigation Properties
        public Borrowed? Borrowed { get; set; }
    }
}
