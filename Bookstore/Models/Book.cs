using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;

namespace Bookstore.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Display(Name = "Title")]

        [StringLength(100)]
        public string Title { get; set; }

        [Display(Name = "Year Published")]
        public int? YearPublished { get; set; }

        [Display(Name = "Number Pages")]
        public int? NumPages { get; set; }

        public string? Description { get; set; }


        [StringLength(50)]
        [Display(Name = "Publisher")]
        public string? Publisher { get; set; }

        [Display(Name = "Front Page")]
        public string? FrontPage { get; set; }

        [Display(Name = "Download")]
        public string? DownloadURL { get; set; }


        public int AuthorId { get; set; }
        public Author? Author { get; set; }

        public ICollection<Review>? Reviews { get; set; }
        public ICollection<UserBooks>? UsersBooks { get; set; }
        public ICollection<BookGenre>? Genres { get; set; }

        [NotMapped]
        public double AverageRating { get; set; }

        
        [NotMapped]
        [Display(Name = "Rating")]
        public double CalculateRating
        {
            get
            {
                if (Reviews == null || Reviews.Count == 0)
                {
                    return 0.0;
                }

                double sum = 0;
                foreach (var review in Reviews)
                {
                    sum += review.Rating ?? 0;
                }

                return Math.Round(sum / Reviews.Count, 2);
            }
        }
    }
}
