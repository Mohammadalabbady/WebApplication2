namespace WebApplication2.Models
{
    public class RemedialPlan
    {
        public int Id { get; set; }
        public int CaseId { get; set; }
        public string Description { get; set; } = string.Empty;
        public string ChangeRequestNumber { get; set; } = string.Empty;
        public DateTime? ClosureDate { get; set; }
        public string Status { get; set; } = "Pending"; // Pending, In Progress, Completed, Cancelled
        public string CreatedById { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; } = true;
        
        // Navigation properties
        public virtual Case Case { get; set; } = null!;
        public virtual ApplicationUser CreatedBy { get; set; } = null!;
        public virtual ICollection<RemedialPlanAttachment> Attachments { get; set; } = new List<RemedialPlanAttachment>();
    }
}
