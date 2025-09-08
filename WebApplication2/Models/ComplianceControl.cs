namespace WebApplication2.Models
{
    public class ComplianceControl
    {
        public int Id { get; set; }
        public int LegislationId { get; set; }
        public string ControlNumber { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty; // A/E or other categories
        public string RiskLevel { get; set; } = "Medium"; // Low, Medium, High, Critical
        public string Status { get; set; } = "Active"; // Active, Inactive, Deprecated
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public virtual Legislation Legislation { get; set; } = null!;
        public virtual ICollection<Case> Cases { get; set; } = new List<Case>();
    }
}
