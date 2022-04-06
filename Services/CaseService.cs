using System.Security.Claims;
using BilHealth.Data;
using BilHealth.Model;
using BilHealth.Model.Dto;
using BilHealth.Utility.Enum;
using Microsoft.AspNetCore.Identity;

namespace BilHealth.Services
{
    public class CaseService : DbServiceBase, ICaseService
    {
        public CaseService(AppDbContext dbCtx) : base(dbCtx)
        {
        }

        public Task CloseCase(Guid caseId)
        {
            throw new NotImplementedException();
        }

        public Task CreateCase(CaseDto details)
        {
            throw new NotImplementedException();
        }

        public Task CreateMessage(CaseMessageDto details)
        {
            throw new NotImplementedException();
        }

        public Task CreatePrescription(PrescriptionDto details)
        {
            throw new NotImplementedException();
        }

        public Task CreateSystemMessage(CaseSystemMessageDto details)
        {
            throw new NotImplementedException();
        }

        public Task CreateTriageRequest(TriageRequestDto details)
        {
            throw new NotImplementedException();
        }

        public Task ReopenCase(Guid caseId)
        {
            throw new NotImplementedException();
        }

        public Task SetCaseState(Guid caseId, CaseState newState)
        {
            throw new NotImplementedException();
        }

        public Task SetTriageRequestApproval(TriageRequestDto details, ApprovalStatus approval)
        {
            throw new NotImplementedException();
        }

        public Task UpdatePrescription(PrescriptionDto details)
        {
            throw new NotImplementedException();
        }
    }
}
