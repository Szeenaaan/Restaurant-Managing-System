namespace RMS.Application.Category.DTO
{
    public class GetIdDto
    {
        public int Id { get; set; }

        public string CategoryName { get; set; } = string.Empty;

        public string CategoryCode { get; set; } = string.Empty;

        public string? DisplayImage { get; set; }
    }
}
