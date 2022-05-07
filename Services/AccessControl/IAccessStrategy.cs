namespace BilHealth.Services.AccessControl
{
    public interface IAccessStrategy
    {
        Task<bool> CheckAccess(Guid accessingUserId, Guid accessedUserId);
        Task<bool> TriggerAccess(Guid accessingUserId, Guid accessedUserId);
    }
}
