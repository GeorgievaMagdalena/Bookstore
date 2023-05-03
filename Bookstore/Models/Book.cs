using System.ComponentModel.DataAnnotations;
using System.IO;

namespace Bookstore.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }
        public int? YearPublished { get; set; }
        public int? NumPages { get; set; }
        public string? Description { get; set; }

        [Required]
        [StringLength(50)]
        public string? Publisher { get; set; }
        public string? FrontPage { get; set; }
        public string? DownloadURL { get; set; }


        public int AuthorId { get; set; }
        public Author Author { get; set; }

        public ICollection<Review>? Reviews { get; set; }
        public ICollection<UserBooks>? UsersBooks { get; set; }
        public ICollection<BookGenre>? Genres { get; set; }

    }
}
