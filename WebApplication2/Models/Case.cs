using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class Case
    {
        public int Id { get; set; }
        
        [Required]
        public string CaseNumber { get; set; } = string.Empty;
        
        [Required]
        public int OrganizationId { get; set; }
        
        [Required]
        public int LegislationId { get; set; }
        
        public int? ComplianceControlId { get; set; }
        
        [Required]
        public string ArticleNumber { get; set; } = string.Empty;
        
        [Required]
        public string ComplianceClause { get; set; } = string.Empty;
        
        [Required]
        public string NonComplianceStatus { get; set; } = string.Empty;
        
        [Required]
        public string Channels { get; set; } = string.Empty;
        
        [Required]
        public bool IsRelatedToJoi { get; set; }
        
        [Required]
        public string Source { get; set; } = string.Empty; // Authority Matrix/Inspection/etc.
        
        [Required]
        public string OwningUnitSector { get; set; } = string.Empty;
        
        [Required]
        public string MonitoringTeam { get; set; } = string.Empty; // Operation/Assurance/Planning
        
        [Required]
        public DateTime DateOfSectorNotification { get; set; }
        
        public string Status { get; set; } = "Open"; // Open, Under Review, Approved, Rejected, Closed
        
        public string Priority { get; set; } = "Medium"; // Low, Medium, High, Critical
        
        public decimal? ExpectedFine { get; set; }
        
        public DateTime? ClosureDate { get; set; }
        
        public string? ClosureType { get; set; } // Compliance, Cancellation
        
        public string? ClosureJustification { get; set; }
        
        public string? CreatedById { get; set; } = string.Empty;
        
        public string? AssignedToId { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? UpdatedAt { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        // Navigation properties
        public virtual Organization Organization { get; set; } = null!;
        public virtual Legislation Legislation { get; set; } = null!;
        public virtual ComplianceControl? ComplianceControl { get; set; }
        public virtual ApplicationUser? CreatedBy { get; set; } = null!;
        public virtual ApplicationUser? AssignedTo { get; set; }
        public virtual ICollection<CaseAttachment> Attachments { get; set; } = new List<CaseAttachment>();
        public virtual ICollection<CaseUpdate> Updates { get; set; } = new List<CaseUpdate>();
        public virtual ICollection<RemedialPlan> RemedialPlans { get; set; } = new List<RemedialPlan>();
        public virtual ICollection<CaseWorkflow> Workflows { get; set; } = new List<CaseWorkflow>();
    }
}
