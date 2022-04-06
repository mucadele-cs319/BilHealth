using System.Security.Claims;
using BilHealth.Data;
using BilHealth.Model;
using BilHealth.Model.Dto;
using BilHealth.Utility.Enum;
using Microsoft.AspNetCore.Identity;

namespace BilHealth.Services
{
    public class AppointmentService : DbServiceBase, IAppointmentService
    {
        public AppointmentService(AppDbContext dbCtx) : base(dbCtx)
        {
        }

        public Task CreateAppointmentRequest(Appointment appointment)
        {
            throw new NotImplementedException();
        }

        public Task<AppointmentVisit> CreateVisit(AppointmentVisitDto details)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsPatientBlacklisted(Patient patientUser)
        {
            throw new NotImplementedException();
        }

        public Task SetAppointmentApproval(Appointment appointment, ApprovalStatus approval)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SetNotesOfVisit(AppointmentVisitDto details)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SetPatientBlacklistState(Guid patientUserId, bool newState)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdatePatientVisitDetails(AppointmentVisitDto details)
        {
            throw new NotImplementedException();
        }
    }
}
