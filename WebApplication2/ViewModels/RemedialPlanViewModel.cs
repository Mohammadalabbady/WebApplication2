using System.ComponentModel.DataAnnotations;
using WebApplication2.Models;

namespace WebApplication2.ViewModels
{
    public class RemedialPlanViewModel
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Case is required")]
        [Display(Name = "Case Number")]
        public int CaseId { get; set; }
        
        [Required(ErrorMessage = "Description is required")]
        [Display(Name = "Description of Plan")]
        public string Description { get; set; } = string.Empty;
        
        [Display(Name = "Change Request Number")]
        public string ChangeRequestNumber { get; set; } = string.Empty;
        
        [Display(Name = "Closure Date")]
        [DataType(DataType.Date)]
        public DateTime? ClosureDate { get; set; }
        
        [Display(Name = "Status")]
        public string Status { get; set; } = "Pending";
        
        // For dropdowns
        public List<Case> Cases { get; set; } = new List<Case>();
        
        // For file uploads
        public List<IFormFile>? Attachments { get; set; }
    }
}
