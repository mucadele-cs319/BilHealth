using BilHealth.Data;
using BilHealth.Model;
using BilHealth.Model.Dto;
using BilHealth.Utility.Enum;
using Microsoft.EntityFrameworkCore;

namespace BilHealth.Services.Users
{
    public class ProfileService : DbServiceBase, IProfileService
    {
        public ProfileService(AppDbContext dbCtx) : base(dbCtx)
        {
        }

        public async Task<List<Case>> GetOpenCases(DomainUser user)
        {
            if (user is Patient patient)
            {
                return await DbCtx.Cases.Where(c => c.PatientUserId == patient.Id && c.State != CaseState.Closed).ToListAsync();
            }
            else if (user is Doctor doctor)
            {
                return await DbCtx.Cases.Where(c => c.DoctorUserId == doctor.Id && c.State == CaseState.Open).ToListAsync();
            }
            else
            {
                throw new ArgumentException("This user type is not supported yet: " + user.GetType());
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
                throw new ArgumentException("This user type is not supported yet: " + user.GetType());
            }
        }

        public async Task UpdateProfile(DomainUser user, UserProfileDto newProfile)
        {
            user.Gender = newProfile.Gender;

            if (user is Patient patient) {
                patient.BodyWeight = newProfile.BodyWeight;
                patient.BodyHeight = newProfile.BodyHeight;
                patient.BloodType = newProfile.BloodType;
            } else if (user is Doctor doctor) {
                doctor.Specialization = newProfile.Specialization;
                doctor.Campus = newProfile.Campus;
            }

            await DbCtx.SaveChangesAsync();
        }
    }
}
