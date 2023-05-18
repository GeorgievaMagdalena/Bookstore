using Microsoft.AspNetCore.WebUtilities;

namespace Bookstore.Interfaces
{
    public interface IBufferedFileUploadService
    {
        Task<bool> UploadFile(IFormFile file);
    }
}
