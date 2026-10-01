namespace RMS.Persistence
{
    public class Category
    {
        public int Id { get; set; }

        public string CategoryName { get; set; } = string.Empty;

        public string CategoryCode { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string? DisplayImage { get; set; }

        public int SortOrder { get; set; }

        public bool IsActive { get; set; }

        public string PresentStatus { get; set; } = string.Empty;

        public DateTime? CreatedAt { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public int? UpdatedBy { get; set; }

        public int Version { get; set; }
    }
}

