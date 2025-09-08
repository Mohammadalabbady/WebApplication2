using System.ComponentModel.DataAnnotations;
using WebApplication2.Models;

namespace WebApplication2.ViewModels
{
    public class CaseUpdateViewModel
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Case is required")]
        [Display(Name = "Case Number")]
        public int CaseId { get; set; }
        
        [Required(ErrorMessage = "Update Type is required")]
        [Display(Name = "Type of Update")]
        public string UpdateType { get; set; } = string.Empty;
        
        [Display(Name = "Old Value")]
        public string OldValue { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "New Value is required")]
        [Display(Name = "New Value")]
        public string NewValue { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Details of Change is required")]
        [Display(Name = "Details of Change")]
        public string DetailsOfChange { get; set; } = string.Empty;
        
        // For dropdowns
        public List<Case> Cases { get; set; } = new List<Case>();
        public List<string> UpdateTypes { get; set; } = new List<string> { "Status", "Closure Date", "Likelihood", "Impact", "Priority", "Expected Fine" };
        
        // For file uploads
        public List<IFormFile>? Attachments { get; set; }
    }
}
