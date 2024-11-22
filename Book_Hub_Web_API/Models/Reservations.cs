using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Book_Hub_Web_API.Data.Enums;

namespace Book_Hub_Web_API.Models
{
    public class Reservations
    {
        [Key]
        [Required]
        [RegularExpression(@"^[-+]?\d+$", ErrorMessage = "Only integer value allowed!")]
        public int ReservationId { get; set; }

        [Required]
        [ForeignKey("BookId")]
        [RegularExpression(@"^[-+]?\d+$", ErrorMessage = "Only integer value allowed!")]

        public int BookId { get; set; }

        [Required]
        [ForeignKey("UserId")]
        [RegularExpression(@"^[-+]?\d+$", ErrorMessage = "Only integer value allowed!")]
        public int UserId { get; set; }

        [Required]
        public DateTime ApplicationTimestamp { get; set; }

        public DateOnly? ExpectedAvailabilityDate { get; set; }

        public DateOnly? ReservationExpiryDate { get; set; }

        [Required]
        public Reservation_Status ReservationStatus { get; set; } = Reservation_Status.Pending;

        // Navigation Properties
        public Books? Book { get; set; }

        public Users? Users { get; set; }
    }
}
