namespace RMS.Application.Category.DTO
{
    public class GetAllDto
    {
        public int Id { get; set; }

        public string CategoryName { get; set; } = string.Empty;

        public string CategoryCode { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string? DisplayImage { get; set; }

        public bool IsActive { get; set; }

        public string PresentStatus { get; set; } = string.Empty;

    }
}
