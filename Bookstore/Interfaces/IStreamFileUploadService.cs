using Microsoft.AspNetCore.WebUtilities;

namespace Bookstore.Interfaces
{
    public interface IStreamFileUploadService
    {
        Task<bool> UploadFile(MultipartReader reader, MultipartSection section);
    }

}
