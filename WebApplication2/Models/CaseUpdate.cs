namespace WebApplication2.Models
{
    public class CaseUpdate
    {
        public int Id { get; set; }
        public int CaseId { get; set; }
        public string UpdateType { get; set; } = string.Empty; // Status, Closure Date, Likelihood, Impact
        public string OldValue { get; set; } = string.Empty;
        public string NewValue { get; set; } = string.Empty;
        public string DetailsOfChange { get; set; } = string.Empty;
        public string UpdatedById { get; set; } = string.Empty;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
        
        // Navigation properties
        public virtual Case Case { get; set; } = null!;
        public virtual ApplicationUser UpdatedBy { get; set; } = null!;
    }
}
