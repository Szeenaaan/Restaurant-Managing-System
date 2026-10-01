using Microsoft.AspNetCore.Http;

namespace RMS.Application.Helpers.IFileStorage
{
    public interface IFileStorage
    {
        Task<string?> SaveAsync(IFormFile? file);

        void Delete(string? filePath);
    }
}

