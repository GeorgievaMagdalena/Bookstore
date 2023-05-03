using System.ComponentModel.DataAnnotations;

namespace Bookstore.Models
{
    public class UserBooks
    {
        public int Id { get; set; }

        [Required]
        [StringLength(450)]
        public string AppUser { get; set; }

        public int BookId { get; set; }
        public Book Book { get; set; }
    }
}
