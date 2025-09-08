using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Legislation> Legislations { get; set; }
        public DbSet<ComplianceControl> ComplianceControls { get; set; }
        public DbSet<Case> Cases { get; set; }
        public DbSet<CaseAttachment> CaseAttachments { get; set; }
        public DbSet<CaseUpdate> CaseUpdates { get; set; }
        public DbSet<RemedialPlan> RemedialPlans { get; set; }
        public DbSet<RemedialPlanAttachment> RemedialPlanAttachments { get; set; }
        public DbSet<CaseWorkflow> CaseWorkflows { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure relationships and constraints
            builder.Entity<Case>()
                .HasOne(c => c.Organization)
                .WithMany(o => o.Cases)
                .HasForeignKey(c => c.OrganizationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Case>()
                .HasOne(c => c.Legislation)
                .WithMany(l => l.Cases)
                .HasForeignKey(c => c.LegislationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Case>()
                .HasOne(c => c.ComplianceControl)
                .WithMany(cc => cc.Cases)
                .HasForeignKey(c => c.ComplianceControlId)
                .OnDelete(DeleteBehavior.Restrict);

          /*  builder.Entity<Case>()
                .HasOne(c => c.CreatedBy)
                .WithMany(u => u.CreatedCases)
                .HasForeignKey(c => c.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);*/

            builder.Entity<Case>()
                .HasOne(c => c.AssignedTo)
                .WithMany(u => u.AssignedCases)
                .HasForeignKey(c => c.AssignedToId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<CaseAttachment>()
                .HasOne(ca => ca.Case)
                .WithMany(c => c.Attachments)
                .HasForeignKey(ca => ca.CaseId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<CaseUpdate>()
                .HasOne(cu => cu.Case)
                .WithMany(c => c.Updates)
                .HasForeignKey(cu => cu.CaseId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<RemedialPlan>()
                .HasOne(rp => rp.Case)
                .WithMany(c => c.RemedialPlans)
                .HasForeignKey(rp => rp.CaseId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<RemedialPlanAttachment>()
                .HasOne(rpa => rpa.RemedialPlan)
                .WithMany(rp => rp.Attachments)
                .HasForeignKey(rpa => rpa.RemedialPlanId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<CaseWorkflow>()
                .HasOne(cw => cw.Case)
                .WithMany(c => c.Workflows)
                .HasForeignKey(cw => cw.CaseId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure indexes
            builder.Entity<Case>()
                .HasIndex(c => c.CaseNumber)
                .IsUnique();

            builder.Entity<Organization>()
                .HasIndex(o => o.Code)
                .IsUnique();

            builder.Entity<Legislation>()
                .HasIndex(l => l.Code)
                .IsUnique();

            builder.Entity<ComplianceControl>()
                .HasIndex(cc => new { cc.LegislationId, cc.ControlNumber })
                .IsUnique();
        }
    }
}
