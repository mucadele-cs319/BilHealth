namespace BilHealth.Services.AccessControl
{
    public partial class AccessControlService
    {
        /// <summary>
        /// Will not allow access under any circumstances.
        /// </summary>
        protected class NoAccessStrategy : IAccessStrategy
        {
            public Task<bool> TriggerAccess(Guid accessingUserId, Guid accessedUserId) => Task.FromResult(false);

            public Task<bool> CheckAccess(Guid accessingUserId, Guid accessedUserId) => Task.FromResult(false);
        }
    }
}
