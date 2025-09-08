using WebApplication2.Models;
using WebApplication2.ViewModels;

namespace WebApplication2.Services
{
    public interface ICaseService
    {
        Task<IEnumerable<Case>> GetAllCasesAsync();
        Task<Case?> GetCaseByIdAsync(int id);
        Task<Case?> GetCaseByNumberAsync(string caseNumber);
        Task<Case> CreateCaseAsync(CaseViewModel viewModel, string userId);
        Task<Case> UpdateCaseAsync(int id, CaseViewModel viewModel, string userId);
        Task<bool> DeleteCaseAsync(int id);
        Task<IEnumerable<Case>> GetCasesByStatusAsync(string status);
        Task<IEnumerable<Case>> GetCasesByOrganizationAsync(int organizationId);
        Task<IEnumerable<Case>> GetCasesByLegislationAsync(int legislationId);
        Task<int> GetTotalCasesCountAsync();
        Task<int> GetOpenCasesCountAsync();
        Task<int> GetClosedCasesCountAsync();
        Task<CaseUpdate> AddCaseUpdateAsync(CaseUpdateViewModel viewModel, string userId);
        Task<RemedialPlan> AddRemedialPlanAsync(RemedialPlanViewModel viewModel, string userId);
        Task<bool> CloseCaseAsync(CaseClosureViewModel viewModel, string userId);
        Task<bool> ApproveCaseAsync(int caseId, string approverId, string comments);
        Task<bool> RejectCaseAsync(int caseId, string approverId, string comments);
    }
}
