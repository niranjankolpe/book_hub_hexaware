using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Book_Hub_Web_API.Data.DTO
{
    public class Add_Book_DTO
    {
       
      

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
       
        public int GenreId { get; set; }
    }
}
