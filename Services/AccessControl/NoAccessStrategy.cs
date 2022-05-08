using System.Linq.Expressions;
using BilHealth.Model;

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

            public Task<Expression<Func<Case, bool>>> GetPersonalizedCaseQuery(DomainUser user) =>
                Task.FromResult<Expression<Func<Case, bool>>>(c => c.PatientUserId == user.Id);
        }
    }
}
