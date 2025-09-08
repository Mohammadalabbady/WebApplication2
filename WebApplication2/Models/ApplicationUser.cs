using Microsoft.AspNetCore.Identity;

namespace WebApplication2.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
        
        // Navigation properties
        public virtual ICollection<Case> CreatedCases { get; set; } = new List<Case>();
        public virtual ICollection<Case> AssignedCases { get; set; } = new List<Case>();
        public virtual ICollection<CaseUpdate> CaseUpdates { get; set; } = new List<CaseUpdate>();
    }
}
