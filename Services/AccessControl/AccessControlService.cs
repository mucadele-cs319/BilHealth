using BilHealth.Data;
using BilHealth.Services.Users;
using BilHealth.Utility.Enum;
using NodaTime;

namespace BilHealth.Services.AccessControl
{
    public partial class AccessControlService : DbServiceBase, IAccessControlService
    {
        private readonly IClock Clock;
        private readonly IAuthenticationService AuthenticationService;

        public AccessControlService(AppDbContext dbCtx, IClock clock, IAuthenticationService authenticationService) : base(dbCtx)
        {
            Clock = clock;
            AuthenticationService = authenticationService;
        }

        protected IAccessStrategy AccessStrategyFactory(string userType) => userType switch
        {
            UserType.Admin => new TrackedAccessStrategy(DbCtx, Clock),
            UserType.Staff => new TrackedAccessStrategy(DbCtx, Clock),
            UserType.Doctor => new RelationalAccessStrategy(DbCtx),
            UserType.Nurse => new TimedAccessStrategy(DbCtx, Clock),
            UserType.Patient => new NoAccessStrategy(),
            _ => throw new ArgumentOutOfRangeException(nameof(userType), "Invalid user type"),
        };

        protected IAccessStrategy GetAccessStrategy(Guid accessingUserId) =>
            AccessStrategyFactory(AuthenticationService.GetUserType(accessingUserId));

        protected Task<bool> BaseHandler(Guid accessingUserId, Guid accessedUserId, Func<Guid, Guid, Task<bool>> accessStrategyMethod)
        {
            if (accessedUserId == accessingUserId) return Task.FromResult(true);

            if (AuthenticationService.GetUserType(accessedUserId) == UserType.Patient)
                return accessStrategyMethod(accessingUserId, accessedUserId);
            return Task.FromResult(true);
        }

        public Task<bool> AccessGuard(Guid accessingUserId, Guid accessedUserId) =>
            BaseHandler(accessingUserId, accessedUserId, GetAccessStrategy(accessingUserId).TriggerAccess);

        public Task<bool> Profile(Guid accessingUserId, Guid accessedUserId) =>
            BaseHandler(accessingUserId, accessedUserId, GetAccessStrategy(accessingUserId).CheckAccess);

        public async Task<bool> TestResult(Guid accessingUserId, Guid testResultId)
        {
            var testResult = await DbCtx.TestResults.FindOrThrowAsync(testResultId);
            return await Profile(accessingUserId, testResult.PatientUserId);
        }

        public async Task<bool> Case(Guid accessingUserId, Guid caseId)
        {
            var _case = await DbCtx.Cases.FindOrThrowAsync(caseId);
            return await Profile(accessingUserId, _case.PatientUserId);
        }
    }
}
