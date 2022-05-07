using BilHealth.Data;
using BilHealth.Utility.Enum;
using Microsoft.EntityFrameworkCore;

namespace BilHealth.Services.AccessControl
{
    public partial class AccessControlService
    {
        /// <summary>
        /// Only grants access if user has an active relation to the accessed entity.
        /// For example, an open case in which they are currently assigned.
        /// </summary>
        protected class RelationalAccessStrategy : DbServiceBase, IAccessStrategy
        {
            public RelationalAccessStrategy(AppDbContext dbCtx) : base(dbCtx)
            {
            }

            public Task<bool> TriggerAccess(Guid accessingUserId, Guid accessedUserId) =>
                CheckAccess(accessingUserId, accessedUserId);

            public async Task<bool> CheckAccess(Guid accessingUserId, Guid accessedUserId) =>
                await DbCtx.Cases.AnyAsync(c =>
                    c.PatientUserId == accessedUserId &&
                    c.DoctorUserId == accessingUserId &&
                    c.State != CaseState.Closed
                );
        }
    }
}
