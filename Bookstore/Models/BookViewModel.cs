using Microsoft.AspNetCore.Http;

namespace Bookstore.Models
{
    public class BookViewModel
    {
        public string Title { get; set; }
        public int? YearPublished { get; set; }
        public int? NumPages { get; set; }
        public string Description { get; set; }
        public string Publisher { get; set; }
        public IFormFile FrontPageFile { get; set; }
        public string DownloadURL { get; set; }
        public int AuthorId { get; set; }
    }
}
