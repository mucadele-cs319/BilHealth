using BilHealth.Data;
using BilHealth.Utility.Enum;

namespace BilHealth.Services.AccessControl
{
    public partial class AccessControlService
    {
        protected class RelationalAccessStrategy : DbServiceBase, IAccessStrategy
        {
            public RelationalAccessStrategy(AppDbContext dbCtx) : base(dbCtx)
            {
            }

            public Task<bool> TriggerAccess(Guid accessingUserId, Guid accessedUserId) =>
                CheckAccess(accessingUserId, accessedUserId);

            public Task<bool> CheckAccess(Guid accessingUserId, Guid accessedUserId) =>
                Task.FromResult(DbCtx.Cases.Any(c =>
                    c.PatientUserId == accessedUserId &&
                    c.DoctorUserId == accessingUserId &&
                    c.State != CaseState.Closed
                ));
        }
    }
}
