using System.Linq.Expressions;
using BilHealth.Data;
using BilHealth.Model;
using Microsoft.EntityFrameworkCore;
using NodaTime;

namespace BilHealth.Services.AccessControl
{
    public partial class AccessControlService
    {
        /// <summary>
        /// Only grants access while the user has an unexpired <see cref="TimedAccessGrant"/>.
        /// </summary>
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
                var currentTime = Clock.GetCurrentInstant();
                return DbCtx.TimedAccessGrants.AnyAsync(g =>
                    g.UserId == accessingUserId &&
                    g.PatientUserId == accessedUserId &&
                    g.Canceled == false &&
                    g.ExpiryTime.CompareTo(currentTime) > 0
                );
            }

            public async Task<Expression<Func<Case, bool>>> GetPersonalizedCaseQuery(DomainUser user)
            {
                var accessiblePatientIds = await DbCtx.TimedAccessGrants.Where(g => g.UserId == user.Id).Select(g => g.PatientUserId).ToListAsync();
                return c => accessiblePatientIds.Contains(c.PatientUserId);
            }
        }
    }
}
