using System.Linq.Expressions;
using BilHealth.Model;

namespace BilHealth.Services.AccessControl
{
    public interface IAccessStrategy
    {
        /// <summary>
        /// Checks whether the requested access is to be granted.
        /// This method will not leave an audit trail, whereas <see cref="TriggerAccess(Guid, Guid)"/> may.
        /// </summary>
        /// <param name="accessingUserId">The ID of the user attempting to access</param>
        /// <param name="accessedUserId">The ID of the user whose profile is being accessed</param>
        /// <returns>true if the access is granted</returns>
        Task<bool> CheckAccess(Guid accessingUserId, Guid accessedUserId);
        /// <summary>
        /// This method returns the same output as that of <see cref="CheckAccess(Guid, Guid)"/>.
        /// The difference is that this method will act as if the access is actually being attempted.
        /// For example, this method may leave an audit trail, whereas <see cref="CheckAccess(Guid, Guid)"/> will not.
        /// </summary>
        /// <param name="accessingUserId">The ID of the user attempting to access</param>
        /// <param name="accessedUserId">The ID of the user whose profile is being accessed</param>
        /// <returns>true if the access is granted</returns>
        Task<bool> TriggerAccess(Guid accessingUserId, Guid accessedUserId);
        /// <summary>
        /// Get a LINQ query expression based on this access strategy.
        /// </summary>
        /// <param name="user">The user for whom to personalize this case list</param>
        /// <returns>Expression with appropiate constraints</returns>
        Task<Expression<Func<Case, bool>>> GetPersonalizedCaseQuery(DomainUser user);
    }
}
