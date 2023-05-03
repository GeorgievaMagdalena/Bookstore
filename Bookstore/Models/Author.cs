using System.ComponentModel.DataAnnotations;

namespace Bookstore.Models
{
    public class Author
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set;}

        [Required]
        [StringLength(50)]
        public string? Nationality { get; set; }

        [Required]
        [StringLength(50)]
        public string? Gender { get; set; }

        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }

        public ICollection<Book>? Books { get; set; }

    }
}
