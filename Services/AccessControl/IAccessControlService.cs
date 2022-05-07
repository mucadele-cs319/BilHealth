using BilHealth.Model;

namespace BilHealth.Services.AccessControl
{
    public interface IAccessControlService
    {
        /// <summary>
        /// This method returns the same output as that of <see cref="Profile(Guid, Guid)"/>.
        /// The difference is that this method will act as if the access is actually being attempted.
        /// For example, this method may leave an audit trail, whereas <see cref="Profile(Guid, Guid)"/> will not.
        /// </summary>
        /// <param name="accessingUserId">The ID of the user attempting to access</param>
        /// <param name="accessedUserId">The ID of the user whose profile is being accessed</param>
        /// <returns>true if the access is granted</returns>
        Task<bool> AccessGuard(Guid accessingUserId, Guid accessedUserId);
        /// <summary>
        /// Checks whether the requested access is to be granted.
        /// This method will not leave an audit trail, whereas <see cref="AccessGuard(Guid, Guid)"/> may.
        /// </summary>
        /// <param name="accessingUserId">The ID of the user attempting to access</param>
        /// <param name="accessedUserId">The ID of the user whose profile is being accessed</param>
        /// <returns>true if the access is granted</returns>
        Task<bool> Profile(Guid accessingUserId, Guid accessedUserId);
        /// <summary>
        /// Checks whether the requested access is to be granted.
        /// This method will not leave an audit trail.
        /// </summary>
        /// <param name="accessingUserId">The ID of the user attempting to access</param>
        /// <param name="testResultId">The ID of the test result being accessed</param>
        /// <returns>true if the access is granted</returns>
        Task<bool> TestResult(Guid accessingUserId, Guid testResultId);
        /// <summary>
        /// Checks whether the requested access is to be granted.
        /// This method will not leave an audit trail.
        /// </summary>
        /// <param name="accessingUserId">The ID of the user attempting to access</param>
        /// <param name="caseId">The ID of the case being accessed</param>
        /// <returns>true if the access is granted</returns>
        Task<bool> Case(Guid accessingUserId, Guid caseId);

        /// <summary>
        /// Lists the most recent audit trails.
        /// </summary>
        /// <param name="count">The max amount of audit trails to include</param>
        /// <returns>List of <see cref="AuditTrail"/></returns>
        Task<List<AuditTrail>> GetRecentAuditTrails(int count = 100);
    }
}
