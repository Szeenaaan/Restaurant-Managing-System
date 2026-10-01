using Microsoft.AspNetCore.Http;
using RMS.Application.Helpers.IFileStorage;

namespace RMS.Infrastructure.FileStorage
{
    public class FileStorageHelper : IFileStorage
    {
        public async Task<string?> SaveAsync(IFormFile? file)
        {
            if (file == null)
                return null;

            var directory = Path.Combine("wwwroot", "uploads");

            Directory.CreateDirectory(directory);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

            var filePath = Path.Combine(directory, fileName);

            await using var stream = new FileStream(filePath, FileMode.Create);

            await file.CopyToAsync(stream);

            return $"/uploads/{fileName}";
        }


        public void Delete(string? filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return;

            var fileName = Path.GetFileName(filePath);

            var physicalPath = Path.Combine("wwwroot","uploads", fileName);

            if (File.Exists(physicalPath)) File.Delete(physicalPath);
        }
    }
}
