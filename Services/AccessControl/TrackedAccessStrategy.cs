using BilHealth.Data;
using BilHealth.Model;
using NodaTime;

namespace BilHealth.Services.AccessControl
{
    public partial class AccessControlService
    {
        protected class TrackedAccessStrategy : DbServiceBase, IAccessStrategy
        {
            private readonly IClock Clock;

            public TrackedAccessStrategy(AppDbContext dbCtx, IClock clock) : base(dbCtx)
            {
                Clock = clock;
            }

            public Task<bool> TriggerAccess(Guid accessingUserId, Guid accessedUserId)
            {
                var access = CheckAccess(accessingUserId, accessedUserId);

                var trail = new AuditTrail
                {
                    AccessTime = Clock.GetCurrentInstant(),
                    UserId = accessingUserId,
                    AccessedPatientUserId = accessedUserId,
                };
                DbCtx.AuditTrails.Add(trail);
                DbCtx.SaveChangesAsync();

                return access;
            }

            public Task<bool> CheckAccess(Guid accessingUserId, Guid accessedUserId) => Task.FromResult(true);
        }
    }
}
