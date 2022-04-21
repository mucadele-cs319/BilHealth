using BilHealth.Data;
using BilHealth.Model;
using BilHealth.Model.Dto;
using BilHealth.Utility;
using BilHealth.Utility.Enum;
using Microsoft.EntityFrameworkCore;
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

        public async Task<UserProfileDto> GetFilteredUser(DomainUser requestingUser, Guid requestedUserId)
        {
            var requestedUser = await AuthenticationService.GetDomainUser(requestedUserId);
            var dto = DtoMapper.Map(requestedUser);

            switch (requestingUser)
            {
                case Patient:
                    if (requestedUser is Patient && requestedUserId != requestingUser.Id)
                        throw new InvalidOperationException("Patient cannot access other patient profiles");
                    break;
                case Nurse:
                case Doctor:
                case Staff:
                case Admin:
                    break;
            }

            return dto;
        }

        public async Task<List<Case>> GetOpenCases(DomainUser user)
        {
            if (user is Patient patient)
            {
                return await DbCtx.Cases.Where(c => c.PatientUserId == patient.Id && c.State != CaseState.Closed).ToListAsync();
            }
            else if (user is Doctor doctor)
            {
                return await DbCtx.Cases.Where(c => c.DoctorUserId == doctor.Id && c.State != CaseState.Closed).ToListAsync();
            }
            else
            {
                throw new ArgumentException("This user type is not supported yet", nameof(user));
            }
        }

        public async Task<List<Case>> GetPastCases(DomainUser user)
        {
            if (user is Patient patient)
            {
                return await DbCtx.Cases.Where(c => c.PatientUserId == patient.Id && c.State == CaseState.Closed).ToListAsync();
            }
            else if (user is Doctor doctor)
            {
                return await DbCtx.Cases.Where(c => c.DoctorUserId == doctor.Id && c.State == CaseState.Closed).ToListAsync();
            }
            else
            {
                throw new ArgumentException("This user type is not supported yet", nameof(user));
            }
        }

        public async Task UpdateProfile(DomainUser user, UserProfileDto newProfile)
        {
            user.Gender = newProfile.Gender ?? user.Gender;

            switch (user)
            {
                case Patient patient:
                    patient.BodyWeight = newProfile.BodyWeight ?? patient.BodyWeight;
                    patient.BodyHeight = newProfile.BodyHeight ?? patient.BodyHeight;
                    patient.BloodType = newProfile.BloodType ?? patient.BloodType;
                    break;
                case Doctor doctor:
                    doctor.Specialization = newProfile.Specialization ?? doctor.Specialization;
                    doctor.Campus = newProfile.Campus ?? doctor.Campus;
                    break;
            }

            await DbCtx.SaveChangesAsync();
        }

        public async Task UpdateProfile(Guid userId, UserProfileDto newProfile)
        {
            var user = await DbCtx.DomainUsers.FindOrThrowAsync(userId);
            await UpdateProfile(user, newProfile);
        }

        public async Task SetPatientBlacklistState(Guid patientUserId, bool newState)
        {
            var patientUser = await DbCtx.DomainUsers.FindOrThrowAsync(patientUserId);

            if (patientUser is Patient patient)
                patient.Blacklisted = newState;
            else throw new ArgumentException($"Given ID {patientUserId} belongs to non-patient user");

            await DbCtx.SaveChangesAsync();
        }

        public async Task AddVaccination(VaccinationDto details)
        {
            var vaccination = new Vaccination
            {
                DateTime = Clock.GetCurrentInstant(),
                PatientUserId = details.PatientUserId,
                Type = details.Type
            };
            DbCtx.Vaccinations.Add(vaccination);
            await DbCtx.SaveChangesAsync();
        }

        public async Task<bool> UpdateVaccination(VaccinationDto details)
        {
            var vaccination = await DbCtx.Vaccinations.SingleOrDefaultAsync(v => v.Id == details.Id);
            if (vaccination is null)
                return false;

            vaccination.DateTime = details.DateTime;
            vaccination.Type = details.Type;
            await DbCtx.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveVaccination(Guid vaccinationId)
        {
            var vaccination = await DbCtx.Vaccinations.FindAsync(vaccinationId);
            if (vaccination is null)
                return false;

            DbCtx.Vaccinations.Remove(vaccination);
            await DbCtx.SaveChangesAsync();
            return true;
        }
    }
}
