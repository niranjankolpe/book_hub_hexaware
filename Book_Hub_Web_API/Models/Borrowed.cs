using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Book_Hub_Web_API.Data.Enums;

namespace Book_Hub_Web_API.Models
{
    public class Borrowed
    {
        [Key]
        [Required]
        [RegularExpression(@"^[-+]?\d+$", ErrorMessage = "Only integer value allowed!")]
        public int BorrowId { get; set; }

        [Required]
        [ForeignKey("BookId")]
        [RegularExpression(@"^[-+]?\d+$", ErrorMessage = "Only integer value allowed!")]
        public int BookId { get; set; }

        [Required]
        [ForeignKey("UserId")]
        [RegularExpression(@"^[-+]?\d+$", ErrorMessage = "Only integer value allowed!")]
        public int UserId { get; set; }

        [Required]
        public DateOnly BorrowDate { get; set; }

        [Required]
        public DateOnly ReturnDeadline { get; set; }

        public DateOnly? ReturnDate { get; set; }

        [Required]
        public Borrow_Status BorrowStatus { get; set; } = Borrow_Status.Borrowed;

        // Navigation Properties
        public Books? Book { get; set; }
        public Users? User { get; set; }

        public ICollection<Fines>? Fines { get; set; }

    }
}
