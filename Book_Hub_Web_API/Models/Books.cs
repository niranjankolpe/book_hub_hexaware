using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;


namespace Book_Hub_Web_API.Models
{
    public class Books
    {
        [Key]
        [Required]
        [RegularExpression(@"^[-+]?\d+$", ErrorMessage = "Only integer value allowed!")]
        public int BookId { get; set; }

        [Required]
        public string? Isbn { get; set; }

        [Required]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Only text characters allowed!")]
        public string? Title { get; set; }

        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Only text characters allowed!")]
        public string? Author { get; set; }

        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Only text characters allowed!")]
        public string? Publication { get; set; }

        public DateOnly PublishedDate { get; set; }

        public string? Edition { get; set; }

        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Only text characters allowed!")]
        public string? Language { get; set; }

        public string? Description { get; set; }

        [Required]
        public decimal Cost { get; set; }

        [Required]
        public int AvailableQuantity { get; set; }

        [Required]
        public int TotalQuantity { get; set; }

        [Required]
        [ForeignKey("GenreId")]
        public int GenreId { get; set; }

        // Navigation Properties
        public Genres? Genres { get; set; }

        public ICollection<Borrowed>? Borrowed { get; set; }

        public ICollection<Reservations>? Reservations { get; set; }
    }
}
