using Microsoft.AspNetCore.Http;

namespace RMS.Application.Category.DTO
{
    public class CategoryDto
    {
        public int? Id { get; set; }
        public string CategoryName { get; set; } = string.Empty;

        public string CategoryCode { get; set; } = string.Empty;

        public string? Description { get; set; }

        public IFormFile? DisplayImage { get; set; }
    }
}
