using System.ComponentModel.DataAnnotations;
using WebApplication2.Models;

namespace WebApplication2.ViewModels
{
    public class CaseClosureViewModel
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Case is required")]
        [Display(Name = "Case Number")]
        public int CaseId { get; set; }
        
        [Required(ErrorMessage = "Closure Type is required")]
        [Display(Name = "Closure Type")]
        public string ClosureType { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Justifications is required")]
        [Display(Name = "Justifications")]
        public string ClosureJustification { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Closure Date is required")]
        [Display(Name = "Closure Date")]
        [DataType(DataType.Date)]
        public DateTime ClosureDate { get; set; } = DateTime.Today;
        
        // For dropdowns
        public List<Case> Cases { get; set; } = new List<Case>();
        public List<string> ClosureTypes { get; set; } = new List<string> { "Compliance", "Cancellation" };
        
        // For file uploads
        public List<IFormFile>? Attachments { get; set; }
    }
}

