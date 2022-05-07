namespace BilHealth.Services.AccessControl
{
    public partial class AccessControlService
    {
        protected class NoAccessStrategy : IAccessStrategy
        {
            public Task<bool> TriggerAccess(Guid accessingUserId, Guid accessedUserId) => Task.FromResult(false);

            public Task<bool> CheckAccess(Guid accessingUserId, Guid accessedUserId) => Task.FromResult(false);
        }
    }
}
