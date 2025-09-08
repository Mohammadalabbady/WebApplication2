using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Models;
using WebApplication2.Services;
using WebApplication2.ViewModels;

namespace WebApplication2.Controllers
{
    //[Authorize]
    public class CasesController : Controller
    {
        private readonly ICaseService _caseService;
        private readonly ApplicationDbContext _context;

        public CasesController(ICaseService caseService, ApplicationDbContext context)
        {
            _caseService = caseService;
            _context = context;
        }

        // GET: Cases
        public async Task<IActionResult> Index(string status = "", string organization = "", string legislation = "")
        {
            ViewBag.Statuses = new List<string> { "Open", "Under Review", "Approved", "Rejected", "Closed" };
            ViewBag.Organizations = await _context.Organizations.Where(o => o.IsActive).ToListAsync();
            ViewBag.Legislations = await _context.Legislations.Where(l => l.IsActive).ToListAsync();

            var cases = await _caseService.GetAllCasesAsync();

            if (!string.IsNullOrEmpty(status))
                cases = cases.Where(c => c.Status == status);

            if (!string.IsNullOrEmpty(organization))
                cases = cases.Where(c => c.Organization.Name.Contains(organization));

            if (!string.IsNullOrEmpty(legislation))
                cases = cases.Where(c => c.Legislation.Name.Contains(legislation));

            ViewBag.TotalCases = await _caseService.GetTotalCasesCountAsync();
            ViewBag.OpenCases = await _caseService.GetOpenCasesCountAsync();
            ViewBag.ClosedCases = await _caseService.GetClosedCasesCountAsync();

            return View(cases);
        }

        // GET: Cases/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var caseEntity = await _caseService.GetCaseByIdAsync(id.Value);
            if (caseEntity == null)
                return NotFound();

            return View(caseEntity);
        }

        // GET: Cases/Create
        public async Task<IActionResult> Create()
        {
            var viewModel = new CaseViewModel
            {
                Organizations = await _context.Organizations.Where(o => o.IsActive).ToListAsync(),
                Legislations = await _context.Legislations.Where(l => l.IsActive).ToListAsync(),
                ComplianceControls = await _context.ComplianceControls.Where(cc => cc.IsActive).ToListAsync(),
                Users = await _context.Users.ToListAsync()
            };

            return View(viewModel);
        }

        // POST: Cases/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CaseViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userId = User.Identity?.Name ?? "a20c5656-acbb-4c9c-8c87-40fa9dddef20";
                    await _caseService.CreateCaseAsync(viewModel, userId);
                    TempData["Success"] = "Case created successfully and is pending approval.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while creating the case: " + ex.Message);
                }
            }

            // Reload dropdown data if validation fails
            viewModel.Organizations = await _context.Organizations.Where(o => o.IsActive).ToListAsync();
            viewModel.Legislations = await _context.Legislations.Where(l => l.IsActive).ToListAsync();
            viewModel.ComplianceControls = await _context.ComplianceControls.Where(cc => cc.IsActive).ToListAsync();
            viewModel.Users = await _context.Users.ToListAsync();

            return View(viewModel);
        }

        // GET: Cases/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var caseEntity = await _caseService.GetCaseByIdAsync(id.Value);
            if (caseEntity == null)
                return NotFound();

            var viewModel = new CaseViewModel
            {
                Id = caseEntity.Id,
                CaseNumber = caseEntity.CaseNumber,
                OrganizationId = caseEntity.OrganizationId,
                LegislationId = caseEntity.LegislationId,
                ComplianceControlId = caseEntity.ComplianceControlId,
                ArticleNumber = caseEntity.ArticleNumber,
                ComplianceClause = caseEntity.ComplianceClause,
                NonComplianceStatus = caseEntity.NonComplianceStatus,
                Channels = caseEntity.Channels,
                IsRelatedToJoi = caseEntity.IsRelatedToJoi,
                Source = caseEntity.Source,
                OwningUnitSector = caseEntity.OwningUnitSector,
                MonitoringTeam = caseEntity.MonitoringTeam,
                DateOfSectorNotification = caseEntity.DateOfSectorNotification,
                Priority = caseEntity.Priority,
                ExpectedFine = caseEntity.ExpectedFine,
                AssignedToId = caseEntity.AssignedToId,
                Organizations = await _context.Organizations.Where(o => o.IsActive).ToListAsync(),
                Legislations = await _context.Legislations.Where(l => l.IsActive).ToListAsync(),
                ComplianceControls = await _context.ComplianceControls.Where(cc => cc.IsActive).ToListAsync(),
                Users = await _context.Users.ToListAsync()
            };

            return View(viewModel);
        }

        // POST: Cases/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CaseViewModel viewModel)
        {
            if (id != viewModel.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var userId = User.Identity?.Name ?? "System";
                    await _caseService.UpdateCaseAsync(id, viewModel, userId);
                    TempData["Success"] = "Case updated successfully.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while updating the case: " + ex.Message);
                }
            }

            // Reload dropdown data if validation fails
            viewModel.Organizations = await _context.Organizations.Where(o => o.IsActive).ToListAsync();
            viewModel.Legislations = await _context.Legislations.Where(l => l.IsActive).ToListAsync();
            viewModel.ComplianceControls = await _context.ComplianceControls.Where(cc => cc.IsActive).ToListAsync();
            viewModel.Users = await _context.Users.ToListAsync();

            return View(viewModel);
        }

        // GET: Cases/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var caseEntity = await _caseService.GetCaseByIdAsync(id.Value);
            if (caseEntity == null)
                return NotFound();

            return View(caseEntity);
        }

        // POST: Cases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _caseService.DeleteCaseAsync(id);
            if (result)
            {
                TempData["Success"] = "Case deleted successfully.";
            }
            else
            {
                TempData["Error"] = "Failed to delete case.";
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Cases/Update/5
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null)
                return NotFound();

            var caseEntity = await _caseService.GetCaseByIdAsync(id.Value);
            if (caseEntity == null)
                return NotFound();

            var viewModel = new CaseUpdateViewModel
            {
                CaseId = caseEntity.Id,
                Cases = await _context.Cases.Where(c => c.IsActive).ToListAsync()
            };

            return View(viewModel);
        }

        // POST: Cases/Update/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(CaseUpdateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userId = User.Identity?.Name ?? "System";
                    await _caseService.AddCaseUpdateAsync(viewModel, userId);
                    TempData["Success"] = "Case update recorded successfully.";
                    return RedirectToAction(nameof(Details), new { id = viewModel.CaseId });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while recording the update: " + ex.Message);
                }
            }

            viewModel.Cases = await _context.Cases.Where(c => c.IsActive).ToListAsync();
            return View(viewModel);
        }

        // GET: Cases/AddRemedialPlan/5
        public async Task<IActionResult> AddRemedialPlan(int? id)
        {
            if (id == null)
                return NotFound();

            var caseEntity = await _caseService.GetCaseByIdAsync(id.Value);
            if (caseEntity == null)
                return NotFound();

            var viewModel = new RemedialPlanViewModel
            {
                CaseId = caseEntity.Id,
                Cases = await _context.Cases.Where(c => c.IsActive).ToListAsync()
            };

            return View(viewModel);
        }

        // POST: Cases/AddRemedialPlan/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRemedialPlan(RemedialPlanViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userId = User.Identity?.Name ?? "System";
                    await _caseService.AddRemedialPlanAsync(viewModel, userId);
                    TempData["Success"] = "Remedial plan added successfully.";
                    return RedirectToAction(nameof(Details), new { id = viewModel.CaseId });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while adding the remedial plan: " + ex.Message);
                }
            }

            viewModel.Cases = await _context.Cases.Where(c => c.IsActive).ToListAsync();
            return View(viewModel);
        }

        // GET: Cases/Close/5
        public async Task<IActionResult> Close(int? id)
        {
            if (id == null)
                return NotFound();

            var caseEntity = await _caseService.GetCaseByIdAsync(id.Value);
            if (caseEntity == null)
                return NotFound();

            var viewModel = new CaseClosureViewModel
            {
                CaseId = caseEntity.Id,
                Cases = await _context.Cases.Where(c => c.IsActive).ToListAsync()
            };

            return View(viewModel);
        }

        // POST: Cases/Close/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Close(CaseClosureViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userId = User.Identity?.Name ?? "System";
                    var result = await _caseService.CloseCaseAsync(viewModel, userId);
                    if (result)
                    {
                        TempData["Success"] = "Case closed successfully.";
                        return RedirectToAction(nameof(Details), new { id = viewModel.CaseId });
                    }
                    else
                    {
                        ModelState.AddModelError("", "Failed to close case.");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while closing the case: " + ex.Message);
                }
            }

            viewModel.Cases = await _context.Cases.Where(c => c.IsActive).ToListAsync();
            return View(viewModel);
        }

        // GET: Cases/Workflow
        //[Authorize(Roles = "RegulationsAdmin")]
        public async Task<IActionResult> Workflow()
        {
            var pendingWorkflows = await _context.CaseWorkflows
                .Include(w => w.Case)
                .Include(w => w.Case.Organization)
                .Include(w => w.Case.Legislation)
                .Include(w => w.RequestedBy)
                .Where(w => w.Status == "Pending" && w.IsActive)
                .OrderBy(w => w.RequestedAt)
                .ToListAsync();

            return View(pendingWorkflows);
        }

        // POST: Cases/Approve/5
        [HttpPost]
        [Authorize(Roles = "RegulationsAdmin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int id, string comments)
        {
            try
            {
                var approverId = User.Identity?.Name ?? "System";
                var result = await _caseService.ApproveCaseAsync(id, approverId, comments);
                if (result)
                {
                    TempData["Success"] = "Case approved successfully.";
                }
                else
                {
                    TempData["Error"] = "Failed to approve case.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred: " + ex.Message;
            }

            return RedirectToAction(nameof(Workflow));
        }

        // POST: Cases/Reject/5
        [HttpPost]
        [Authorize(Roles = "RegulationsAdmin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(int id, string comments)
        {
            try
            {
                var approverId = User.Identity?.Name ?? "System";
                var result = await _caseService.RejectCaseAsync(id, approverId, comments);
                if (result)
                {
                    TempData["Success"] = "Case rejected successfully.";
                }
                else
                {
                    TempData["Error"] = "Failed to reject case.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred: " + ex.Message;
            }

            return RedirectToAction(nameof(Workflow));
        }
    }
}
