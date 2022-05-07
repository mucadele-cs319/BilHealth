namespace BilHealth.Services.AccessControl
{
    public interface IAccessControlService
    {
        Task<bool> AccessGuard(Guid accessingUserId, Guid accessedUserId);
        Task<bool> Profile(Guid accessingUserId, Guid accessedUserId);
        Task<bool> TestResult(Guid accessingUserId, Guid testResultId);
        Task<bool> Case(Guid accessingUserId, Guid caseId);
    }
}
