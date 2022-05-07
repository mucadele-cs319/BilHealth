using BilHealth.Data;
using BilHealth.Model;
using NodaTime;

namespace BilHealth.Services.AccessControl
{
    public partial class AccessControlService
    {
        /// <summary>
        /// Always grants access, however, all access is tracked in an <see cref="AuditTrail"/>.
        /// </summary>
        protected class TrackedAccessStrategy : DbServiceBase, IAccessStrategy
        {
            private readonly IClock Clock;

            public TrackedAccessStrategy(AppDbContext dbCtx, IClock clock) : base(dbCtx)
            {
                Clock = clock;
            }

            public async Task<bool> TriggerAccess(Guid accessingUserId, Guid accessedUserId)
            {
                var access = await CheckAccess(accessingUserId, accessedUserId);

                if (access)
                {
                    DbCtx.AuditTrails.Add(new AuditTrail
                    {
                        AccessTime = Clock.GetCurrentInstant(),
                        UserId = accessingUserId,
                        AccessedPatientUserId = accessedUserId,
                    });
                    await DbCtx.SaveChangesAsync();
                }

                return access;
            }

            public Task<bool> CheckAccess(Guid accessingUserId, Guid accessedUserId) => Task.FromResult(true);
        }
    }
}
