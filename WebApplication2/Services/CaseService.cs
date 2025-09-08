using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Models;
using WebApplication2.ViewModels;

namespace WebApplication2.Services
{
    public class CaseService : ICaseService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CaseService(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IEnumerable<Case>> GetAllCasesAsync()
        {
            return await _context.Cases
                .Include(c => c.Organization)
                .Include(c => c.Legislation)
                .Include(c => c.ComplianceControl)
                .Include(c => c.CreatedBy)
                .Include(c => c.AssignedTo)
                .Include(c => c.Attachments)
                .Where(c => c.IsActive)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task<Case?> GetCaseByIdAsync(int id)
        {
            return await _context.Cases
                .Include(c => c.Organization)
                .Include(c => c.Legislation)
                .Include(c => c.ComplianceControl)
                .Include(c => c.CreatedBy)
                .Include(c => c.AssignedTo)
                .Include(c => c.Attachments)
                .Include(c => c.Updates)
                .Include(c => c.RemedialPlans)
                .Include(c => c.Workflows)
                .FirstOrDefaultAsync(c => c.Id == id && c.IsActive);
        }

        public async Task<Case?> GetCaseByNumberAsync(string caseNumber)
        {
            return await _context.Cases
                .Include(c => c.Organization)
                .Include(c => c.Legislation)
                .Include(c => c.ComplianceControl)
                .Include(c => c.CreatedBy)
                .Include(c => c.AssignedTo)
                .FirstOrDefaultAsync(c => c.CaseNumber == caseNumber && c.IsActive);
        }

        public async Task<Case> CreateCaseAsync(CaseViewModel viewModel, string userId)
        {
            var caseEntity = new Case
            {
                CaseNumber = viewModel.CaseNumber,
                OrganizationId = viewModel.OrganizationId,
                LegislationId = viewModel.LegislationId,
                ComplianceControlId = viewModel.ComplianceControlId,
                ArticleNumber = viewModel.ArticleNumber,
                ComplianceClause = viewModel.ComplianceClause,
                NonComplianceStatus = viewModel.NonComplianceStatus,
                Channels = viewModel.Channels,
                IsRelatedToJoi = viewModel.IsRelatedToJoi,
                Source = viewModel.Source,
                OwningUnitSector = viewModel.OwningUnitSector,
                MonitoringTeam = viewModel.MonitoringTeam,
                DateOfSectorNotification = viewModel.DateOfSectorNotification,
                Priority = viewModel.Priority,
                ExpectedFine = viewModel.ExpectedFine,
                AssignedToId = viewModel.AssignedToId,
                CreatedById = userId,
                Status = "Open"
            };

            _context.Cases.Add(caseEntity);
            await _context.SaveChangesAsync();

            // Create workflow for approval
            var workflow = new CaseWorkflow
            {
                CaseId = caseEntity.Id,
                WorkflowType = "Add Case",
                Status = "Pending",
                RequestedById = userId
            };

            _context.CaseWorkflows.Add(workflow);

            // Handle file uploads
            if (viewModel.Attachments != null)
            {
                foreach (var file in viewModel.Attachments)
                {
                    if (file.Length > 0)
                    {
                        var fileName = await SaveFileAsync(file);
                        var attachment = new CaseAttachment
                        {
                            CaseId = caseEntity.Id,
                            FileName = fileName,
                            OriginalFileName = file.FileName,
                            FilePath = fileName,
                            ContentType = file.ContentType,
                            FileSize = file.Length,
                            UploadedById = userId
                        };
                        _context.CaseAttachments.Add(attachment);
                    }
                }
            }

            await _context.SaveChangesAsync();
            return caseEntity;
        }

        public async Task<Case> UpdateCaseAsync(int id, CaseViewModel viewModel, string userId)
        {
            var caseEntity = await _context.Cases.FindAsync(id);
            if (caseEntity == null)
                throw new ArgumentException("Case not found");

            // Track changes
            var oldStatus = caseEntity.Status;
            var oldPriority = caseEntity.Priority;
            var oldExpectedFine = caseEntity.ExpectedFine;

            caseEntity.OrganizationId = viewModel.OrganizationId;
            caseEntity.LegislationId = viewModel.LegislationId;
            caseEntity.ComplianceControlId = viewModel.ComplianceControlId;
            caseEntity.ArticleNumber = viewModel.ArticleNumber;
            caseEntity.ComplianceClause = viewModel.ComplianceClause;
            caseEntity.NonComplianceStatus = viewModel.NonComplianceStatus;
            caseEntity.Channels = viewModel.Channels;
            caseEntity.IsRelatedToJoi = viewModel.IsRelatedToJoi;
            caseEntity.Source = viewModel.Source;
            caseEntity.OwningUnitSector = viewModel.OwningUnitSector;
            caseEntity.MonitoringTeam = viewModel.MonitoringTeam;
            caseEntity.DateOfSectorNotification = viewModel.DateOfSectorNotification;
            caseEntity.Priority = viewModel.Priority;
            caseEntity.ExpectedFine = viewModel.ExpectedFine;
            caseEntity.AssignedToId = viewModel.AssignedToId;
            caseEntity.UpdatedAt = DateTime.UtcNow;

            // Create update records for significant changes
            if (oldStatus != caseEntity.Status)
            {
                var update = new CaseUpdate
                {
                    CaseId = caseEntity.Id,
                    UpdateType = "Status",
                    OldValue = oldStatus,
                    NewValue = caseEntity.Status,
                    DetailsOfChange = "Status updated",
                    UpdatedById = userId
                };
                _context.CaseUpdates.Add(update);
            }

            if (oldPriority != caseEntity.Priority)
            {
                var update = new CaseUpdate
                {
                    CaseId = caseEntity.Id,
                    UpdateType = "Priority",
                    OldValue = oldPriority,
                    NewValue = caseEntity.Priority,
                    DetailsOfChange = "Priority updated",
                    UpdatedById = userId
                };
                _context.CaseUpdates.Add(update);
            }

            if (oldExpectedFine != caseEntity.ExpectedFine)
            {
                var update = new CaseUpdate
                {
                    CaseId = caseEntity.Id,
                    UpdateType = "Expected Fine",
                    OldValue = oldExpectedFine?.ToString() ?? "Not Set",
                    NewValue = caseEntity.ExpectedFine?.ToString() ?? "Not Set",
                    DetailsOfChange = "Expected fine updated",
                    UpdatedById = userId
                };
                _context.CaseUpdates.Add(update);
            }

            await _context.SaveChangesAsync();
            return caseEntity;
        }

        public async Task<bool> DeleteCaseAsync(int id)
        {
            var caseEntity = await _context.Cases.FindAsync(id);
            if (caseEntity == null)
                return false;

            caseEntity.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Case>> GetCasesByStatusAsync(string status)
        {
            return await _context.Cases
                .Include(c => c.Organization)
                .Include(c => c.Legislation)
                .Include(c => c.CreatedBy)
                .Where(c => c.Status == status && c.IsActive)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Case>> GetCasesByOrganizationAsync(int organizationId)
        {
            return await _context.Cases
                .Include(c => c.Organization)
                .Include(c => c.Legislation)
                .Include(c => c.CreatedBy)
                .Where(c => c.OrganizationId == organizationId && c.IsActive)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Case>> GetCasesByLegislationAsync(int legislationId)
        {
            return await _context.Cases
                .Include(c => c.Organization)
                .Include(c => c.Legislation)
                .Include(c => c.CreatedBy)
                .Where(c => c.LegislationId == legislationId && c.IsActive)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task<int> GetTotalCasesCountAsync()
        {
            return await _context.Cases.Where(c => c.IsActive).CountAsync();
        }

        public async Task<int> GetOpenCasesCountAsync()
        {
            return await _context.Cases.Where(c => c.Status == "Open" && c.IsActive).CountAsync();
        }

        public async Task<int> GetClosedCasesCountAsync()
        {
            return await _context.Cases.Where(c => c.Status == "Closed" && c.IsActive).CountAsync();
        }

        public async Task<CaseUpdate> AddCaseUpdateAsync(CaseUpdateViewModel viewModel, string userId)
        {
            var update = new CaseUpdate
            {
                CaseId = viewModel.CaseId,
                UpdateType = viewModel.UpdateType,
                OldValue = viewModel.OldValue,
                NewValue = viewModel.NewValue,
                DetailsOfChange = viewModel.DetailsOfChange,
                UpdatedById = userId
            };

            _context.CaseUpdates.Add(update);
            await _context.SaveChangesAsync();
            return update;
        }

        public async Task<RemedialPlan> AddRemedialPlanAsync(RemedialPlanViewModel viewModel, string userId)
        {
            var remedialPlan = new RemedialPlan
            {
                CaseId = viewModel.CaseId,
                Description = viewModel.Description,
                ChangeRequestNumber = viewModel.ChangeRequestNumber,
                ClosureDate = viewModel.ClosureDate,
                Status = viewModel.Status,
                CreatedById = userId
            };

            _context.RemedialPlans.Add(remedialPlan);
            await _context.SaveChangesAsync();

            // Handle file uploads
            if (viewModel.Attachments != null)
            {
                foreach (var file in viewModel.Attachments)
                {
                    if (file.Length > 0)
                    {
                        var fileName = await SaveFileAsync(file);
                        var attachment = new RemedialPlanAttachment
                        {
                            RemedialPlanId = remedialPlan.Id,
                            FileName = fileName,
                            OriginalFileName = file.FileName,
                            FilePath = fileName,
                            ContentType = file.ContentType,
                            FileSize = file.Length,
                            UploadedById = userId
                        };
                        _context.RemedialPlanAttachments.Add(attachment);
                    }
                }
            }

            await _context.SaveChangesAsync();
            return remedialPlan;
        }

        public async Task<bool> CloseCaseAsync(CaseClosureViewModel viewModel, string userId)
        {
            var caseEntity = await _context.Cases.FindAsync(viewModel.CaseId);
            if (caseEntity == null)
                return false;

            caseEntity.Status = "Closed";
            caseEntity.ClosureDate = viewModel.ClosureDate;
            caseEntity.ClosureType = viewModel.ClosureType;
            caseEntity.ClosureJustification = viewModel.ClosureJustification;
            caseEntity.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ApproveCaseAsync(int caseId, string approverId, string comments)
        {
            var workflow = await _context.CaseWorkflows
                .FirstOrDefaultAsync(w => w.CaseId == caseId && w.Status == "Pending");
            
            if (workflow == null)
                return false;

            workflow.Status = "Approved";
            workflow.ApprovedById = approverId;
            workflow.ApprovedAt = DateTime.UtcNow;
            workflow.Comments = comments;

            var caseEntity = await _context.Cases.FindAsync(caseId);
            if (caseEntity != null)
            {
                caseEntity.Status = "Approved";
                caseEntity.UpdatedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RejectCaseAsync(int caseId, string approverId, string comments)
        {
            var workflow = await _context.CaseWorkflows
                .FirstOrDefaultAsync(w => w.CaseId == caseId && w.Status == "Pending");
            
            if (workflow == null)
                return false;

            workflow.Status = "Rejected";
            workflow.ApprovedById = approverId;
            workflow.ApprovedAt = DateTime.UtcNow;
            workflow.Comments = comments;

            var caseEntity = await _context.Cases.FindAsync(caseId);
            if (caseEntity != null)
            {
                caseEntity.Status = "Rejected";
                caseEntity.UpdatedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        private async Task<string> SaveFileAsync(IFormFile file)
        {
            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return uniqueFileName;
        }
    }
}
