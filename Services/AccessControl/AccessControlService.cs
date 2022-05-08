using BilHealth.Data;
using BilHealth.Model;
using BilHealth.Model.Dto.Incoming;
using BilHealth.Services.Users;
using BilHealth.Utility.Enum;
using Microsoft.EntityFrameworkCore;
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

        protected async Task<IAccessStrategy> GetAccessStrategy(Guid accessingUserId) =>
            AccessStrategyFactory(await AuthenticationService.GetUserType(accessingUserId));

        protected async Task<bool> BaseHandler(Guid accessingUserId, Guid accessedUserId, Func<Guid, Guid, Task<bool>> accessStrategyMethod)
        {
            if (accessedUserId == accessingUserId) return true;

            if (await AuthenticationService.GetUserType(accessedUserId) == UserType.Patient)
                return await accessStrategyMethod(accessingUserId, accessedUserId);
            return true;
        }

        public async Task<bool> AccessGuard(Guid accessingUserId, Guid accessedUserId) =>
            await BaseHandler(accessingUserId, accessedUserId, (await GetAccessStrategy(accessingUserId)).TriggerAccess);

        public async Task<bool> Profile(Guid accessingUserId, Guid accessedUserId) =>
            await BaseHandler(accessingUserId, accessedUserId, (await GetAccessStrategy(accessingUserId)).CheckAccess);

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

        public Task<List<AuditTrail>> GetRecentAuditTrails(int count = 100) =>
            DbCtx.AuditTrails.OrderByDescending(a => a.AccessTime).Take(count).ToListAsync();

        public async Task GrantTimedAccess(TimedAccessGrantCreateDto details)
        {
            var grant = new TimedAccessGrant
            {
                PatientUserId = details.PatientUserId,
                UserId = details.UserId,
                Canceled = false,
                Period = details.Period,
                ExpiryTime = Clock.GetCurrentInstant() + details.Period.ToDuration(),
            };
            DbCtx.TimedAccessGrants.Add(grant);
            await DbCtx.SaveChangesAsync();
        }

        public async Task CancelTimedAccessGrant(Guid grantId)
        {
            var grant = await DbCtx.TimedAccessGrants.FindOrThrowAsync(grantId);
            grant.Canceled = true;
            await DbCtx.SaveChangesAsync();
        }

        public async Task<List<Case>> GetPersonalizedCaseList(DomainUser user) =>
            await DbCtx.Cases.Where(await AccessStrategyFactory(user.Discriminator).GetPersonalizedCaseQuery(user))
                             .Include(c => c.Messages)
                             .ToListAsync();
    }
}
