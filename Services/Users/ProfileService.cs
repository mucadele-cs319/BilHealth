using BilHealth.Data;
using BilHealth.Model;
using BilHealth.Model.Dto.Incoming;
using BilHealth.Utility.Enum;
using NodaTime;

namespace BilHealth.Services.Users
{
    public class ProfileService : DbServiceBase, IProfileService
    {
        private readonly IAuthenticationService AuthenticationService;
        private readonly IClock Clock;

        public ProfileService(AppDbContext dbCtx, IAuthenticationService authenticationService, IClock clock) : base(dbCtx)
        {
            AuthenticationService = authenticationService;
            Clock = clock;
        }

        public async Task UpdateProfile(DomainUser user, UserProfileUpdateDto details, bool fullyEdit)
        {
            if (fullyEdit)
            {
                user.Gender = details.Gender ?? Gender.Unspecified;
                user.FirstName = details.FirstName ?? user.FirstName;
                user.LastName = details.LastName ?? user.LastName;
                user.DateOfBirth = details.DateOfBirth;
            }

            switch (user)
            {
                case Patient patient:
                    patient.BodyWeight = details.BodyWeight;
                    patient.BodyHeight = details.BodyHeight;
                    patient.BloodType = details.BloodType ?? BloodType.Unspecified;
                    break;
                case Doctor doctor:
                    doctor.Specialization = details.Specialization ?? String.Empty;
                    doctor.Campus = details.Campus ?? Campus.Unspecified;
                    break;
            }

            await DbCtx.SaveChangesAsync();
        }

        public async Task UpdateProfile(Guid userId, UserProfileUpdateDto details, bool fullyEdit)
        {
            var user = await DbCtx.DomainUsers.FindOrThrowAsync(userId);
            await UpdateProfile(user, details, fullyEdit);
        }

        public async Task SetPatientBlacklistState(Guid patientUserId, bool newState)
        {
            var patientUser = await DbCtx.DomainUsers.FindOrThrowAsync(patientUserId);

            if (patientUser is Patient patient)
                patient.Blacklisted = newState;
            else throw new ArgumentException($"Given ID {patientUserId} belongs to non-patient user");

            await DbCtx.SaveChangesAsync();
        }

        public async Task AddVaccination(Guid patientUserId, VaccinationUpdateDto details)
        {
            var vaccination = new Vaccination
            {
                PatientUserId = patientUserId,
                DateTime = details.DateTime,
                Type = details.Type
            };

            DbCtx.Vaccinations.Add(vaccination);
            await DbCtx.SaveChangesAsync();
        }

        public async Task<bool> UpdateVaccination(Guid vaccinationId, VaccinationUpdateDto details)
        {
            var vaccination = await DbCtx.Vaccinations.FindOrThrowAsync(vaccinationId);

            vaccination.DateTime = details.DateTime;
            vaccination.Type = details.Type;
            await DbCtx.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveVaccination(Guid vaccinationId)
        {
            var vaccination = await DbCtx.Vaccinations.FindOrThrowAsync(vaccinationId);

            DbCtx.Vaccinations.Remove(vaccination);
            await DbCtx.SaveChangesAsync();
            return true;
        }
    }
}
