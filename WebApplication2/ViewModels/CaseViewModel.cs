using System.ComponentModel.DataAnnotations;
using WebApplication2.Models;

namespace WebApplication2.ViewModels
{
    public class CaseViewModel
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Case Number is required")]
        [Display(Name = "Case Number")]
        public string CaseNumber { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Organization is required")]
        [Display(Name = "Organization")]
        public int OrganizationId { get; set; }
        
        [Required(ErrorMessage = "Legislation is required")]
        [Display(Name = "Legislation")]
        public int LegislationId { get; set; }
        
        [Display(Name = "Compliance Control")]
        public int? ComplianceControlId { get; set; }
        
        [Required(ErrorMessage = "Article Number is required")]
        [Display(Name = "Article Number")]
        public string ArticleNumber { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Compliance Clause is required")]
        [Display(Name = "Compliance Clause (A/E)")]
        public string ComplianceClause { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Non-compliance Status is required")]
        [Display(Name = "Non-compliance Status (A/E)")]
        public string NonComplianceStatus { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Channels is required")]
        [Display(Name = "Channels")]
        public string Channels { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Please specify if related to Joi")]
        [Display(Name = "Is it related to Joi?")]
        public bool IsRelatedToJoi { get; set; }
        
        [Required(ErrorMessage = "Source is required")]
        [Display(Name = "Source")]
        public string Source { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Owning Unit/Sector is required")]
        [Display(Name = "Owning Unit/Sector")]
        public string OwningUnitSector { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Monitoring Team is required")]
        [Display(Name = "Monitoring Team")]
        public string MonitoringTeam { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Date of Sector Notification is required")]
        [Display(Name = "Date of Sector Notification")]
        [DataType(DataType.Date)]
        public DateTime DateOfSectorNotification { get; set; } = DateTime.Today;
        
        [Display(Name = "Priority")]
        public string Priority { get; set; } = "Medium";
        
        [Display(Name = "Expected Fine")]
        [DataType(DataType.Currency)]
        public decimal? ExpectedFine { get; set; }
        
        [Display(Name = "Assigned To")]
        public string? AssignedToId { get; set; }
        
        // For dropdowns
        public List<Organization> Organizations { get; set; } = new List<Organization>();
        public List<Legislation> Legislations { get; set; } = new List<Legislation>();
        public List<ComplianceControl> ComplianceControls { get; set; } = new List<ComplianceControl>();
        public List<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();
        
        // For file uploads
        public List<IFormFile>? Attachments { get; set; }
    }
}
