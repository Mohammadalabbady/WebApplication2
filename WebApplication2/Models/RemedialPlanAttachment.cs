namespace WebApplication2.Models
{
    public class RemedialPlanAttachment
    {
        public int Id { get; set; }
        public int RemedialPlanId { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string OriginalFileName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public long FileSize { get; set; }
        public string Description { get; set; } = string.Empty;
        public string UploadedById { get; set; } = string.Empty;
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
        
        // Navigation properties
        public virtual RemedialPlan RemedialPlan { get; set; } = null!;
        public virtual ApplicationUser UploadedBy { get; set; } = null!;
    }
}
