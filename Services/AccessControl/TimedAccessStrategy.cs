using BilHealth.Data;
using NodaTime;

namespace BilHealth.Services.AccessControl
{
    public partial class AccessControlService
    {
        protected class TimedAccessStrategy : DbServiceBase, IAccessStrategy
        {
            private readonly IClock Clock;

            public TimedAccessStrategy(AppDbContext dbCtx, IClock clock) : base(dbCtx)
            {
                Clock = clock;
            }

            public Task<bool> TriggerAccess(Guid accessingUserId, Guid accessedUserId) =>
                CheckAccess(accessingUserId, accessedUserId);

            public Task<bool> CheckAccess(Guid accessingUserId, Guid accessedUserId)
            {
                var grant = DbCtx.TimedAccessGrants.SingleOrDefault(g => g.UserId == accessingUserId && g.PatientUserId == accessedUserId);
                return Task.FromResult(grant is not null);
            }
        }
    }
}
