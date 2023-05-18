using System.ComponentModel.DataAnnotations;

namespace Bookstore.Models
{
    public class Review
    {
        public int Id { get; set; }

     
        [StringLength(450)]
        public string AppUser { get; set; }

        [StringLength(500)]
        public string Comment { get; set; }
        public int? Rating { get; set;}

        public int BookId { get; set; }
        public Book? Book { get; set; }

        
    }
}
