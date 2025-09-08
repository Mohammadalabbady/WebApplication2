namespace WebApplication2.Models
{
    public class Legislation
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Year { get; set; }
        public string Status { get; set; } = "Active"; // Active, Inactive, Pending
        public DateTime EffectiveDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public virtual ICollection<ComplianceControl> ComplianceControls { get; set; } = new List<ComplianceControl>();
        public virtual ICollection<Case> Cases { get; set; } = new List<Case>();
    }
}
