namespace RMS.Application.Category.DTO
{
    public class GetQueryDto
    {
        public string? SearchText { get; set; }

        public string? CategoryName { get; set; }

        public string? CategoryCode { get; set; }

        public int Offset { get; set; } = 0;

        public int Limit { get; set; } = 10;

    }
}
