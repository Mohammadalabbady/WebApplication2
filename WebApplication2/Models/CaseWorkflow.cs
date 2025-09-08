namespace WebApplication2.Models
{
    public class CaseWorkflow
    {
        public int Id { get; set; }
        public int CaseId { get; set; }
        public string WorkflowType { get; set; } = string.Empty; // Add Case, Update Case, Add Remedial Plan, Close Case
        public string Status { get; set; } = "Pending"; // Pending, Approved, Rejected
        public string RequestedById { get; set; } = string.Empty;
        public string? ApprovedById { get; set; }
        public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ApprovedAt { get; set; }
        public string? Comments { get; set; }
        public bool IsActive { get; set; } = true;
        
        // Navigation properties
        public virtual Case Case { get; set; } = null!;
        public virtual ApplicationUser RequestedBy { get; set; } = null!;
        public virtual ApplicationUser? ApprovedBy { get; set; }
    }
}
