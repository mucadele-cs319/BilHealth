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

        public async Task<List<Case>> GetOpenCases(User user)
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

        public async Task<List<Case>> GetPastCases(User user)
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

        public async Task UpdateProfile(Patient patientUser, PatientProfileEdit newProfile)
        {
            patientUser.BodyWeight = newProfile.BodyWeight;
            patientUser.BodyHeight = newProfile.BodyHeight;
            patientUser.BloodType = newProfile.BloodType;
            await DbCtx.SaveChangesAsync();
        }

        public async Task UpdateProfile(Doctor doctorUser, DoctorProfileEdit newProfile)
        {
            doctorUser.Specialization = newProfile.Specialization;
            doctorUser.Campus = newProfile.Campus;
            await DbCtx.SaveChangesAsync();
        }
    }
}
