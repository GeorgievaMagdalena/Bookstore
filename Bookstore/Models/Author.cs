using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookstore.Models
{
    public class Author
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string FirstName { get; set; }

    
        [StringLength(50)]
        public string LastName { get; set;}


        [StringLength(50)]
        public string? Nationality { get; set; }

       
        [StringLength(50)]
        public string? Gender { get; set; }

        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }

        [NotMapped]
        public string FullName {
            get { return String.Format("{0} {1}", FirstName, LastName); }
        }

        public ICollection<Book>? Books { get; set; }

    }
}
