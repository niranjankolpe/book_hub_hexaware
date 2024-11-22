using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Book_Hub_Web_API.Models
{
    public class Genres
    {
        [Key]
        [Required]
        public int GenreId { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Description { get; set; }

        // Navigation Properties
        public ICollection<Books>? Books { get; set; }
    }
}
