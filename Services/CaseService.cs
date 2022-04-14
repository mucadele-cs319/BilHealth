using System.Security.Claims;
using BilHealth.Data;
using BilHealth.Model;
using BilHealth.Model.Dto;
using BilHealth.Utility.Enum;
using Microsoft.AspNetCore.Identity;
using NodaTime;

namespace BilHealth.Services
{
    public class CaseService : DbServiceBase, ICaseService
    {
        private readonly IClock Clock;

        public CaseService(AppDbContext dbCtx, IClock clock) : base(dbCtx)
        {
            Clock = clock;
        }

        public async Task<Case> CreateCase(CaseDto details)
        {
            var _case = new Case
            {
                PatientUserId = details.PatientUserId,
                DateTime = Clock.GetCurrentInstant(),
                State = CaseState.WaitingTriage,
                Type = details.Type
            };
            DbCtx.Cases.Add(_case);
            await DbCtx.SaveChangesAsync();
            return _case;
        }

        public async Task<CaseMessage> CreateMessage(CaseMessageDto details)
        {
            var message = new CaseMessage
            {
                CaseId = details.CaseId,
                UserId = details.UserId,
                Content = details.Content,
                DateTime = Clock.GetCurrentInstant()
            };
            DbCtx.Add(message);
            await DbCtx.SaveChangesAsync();
            return message;
        }

        public async Task<Prescription> CreatePrescription(PrescriptionDto details)
        {
            var prescription = new Prescription
            {
                CaseId = details.CaseId,
                DateTime = Clock.GetCurrentInstant(),
                DoctorUserId = details.DoctorUserId,
                Item = details.Item
            };
            DbCtx.Prescriptions.Add(prescription);
            await DbCtx.SaveChangesAsync();
            return prescription;
        }

        public void CreateSystemMessage(CaseSystemMessageDto details)
        {
            var message = new CaseSystemMessage
            {
                CaseId = details.CaseId,
                Content = details.Content,
                DateTime = Clock.GetCurrentInstant(),
                Type = details.Type
            };
            DbCtx.Add(message);
            // await DbCtx.SaveChangesAsync(); // Do not save as this should not be a front-facing method
        }

        public async Task<TriageRequest> CreateTriageRequest(TriageRequestDto details)
        {
            var triageRequest = new TriageRequest
            {
                ApprovalStatus = ApprovalStatus.Waiting,
                CaseId = details.CaseId,
                DoctorUserId = details.DoctorUserId,
                NurseUserId = details.NurseUserId
            };
            DbCtx.TriageRequests.Add(triageRequest);
            await DbCtx.SaveChangesAsync();
            return triageRequest;
        }

        public async Task<CaseMessage> EditMessage(CaseMessageDto details)
        {
            var message = await DbCtx.FindAsync(typeof(CaseMessage), details.Id) as CaseMessage;
            if (message is null) throw new ArgumentException("No message with ID " + details.Id);

            message.Content = details.Content ?? message.Content;
            await DbCtx.SaveChangesAsync();
            return message;
        }

        public async Task<bool> RemoveMessage(Guid messageId)
        {
            var message =  await DbCtx.FindAsync(typeof(CaseMessage), messageId) as CaseMessage;
            if (message is null) return false;

            DbCtx.Remove(message);
            await DbCtx.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemovePrescription(Guid prescriptionId)
        {
            var prescription =  await DbCtx.Prescriptions.FindAsync(prescriptionId);
            if (prescription is null) return false;

            DbCtx.Prescriptions.Remove(prescription);
            await DbCtx.SaveChangesAsync();
            return true;
        }

        public async Task SetCaseState(Guid caseId, CaseState newState)
        {
            var _case = await DbCtx.Cases.FindAsync(caseId);
            if (_case is null) throw new ArgumentException("No case with ID " + caseId);

            _case.State = newState;
            await DbCtx.SaveChangesAsync();
        }

        public async Task SetTriageRequestApproval(TriageRequestDto details)
        {
            var triageRequest = await DbCtx.TriageRequests.FindAsync(details.Id);
            if (triageRequest is null) throw new ArgumentException("No triage request with ID " + details.Id);

            triageRequest.ApprovalStatus = details.ApprovalStatus;
            await DbCtx.SaveChangesAsync();
        }

        public async Task<Prescription> UpdatePrescription(PrescriptionDto details)
        {
            var prescription = await DbCtx.Prescriptions.FindAsync(details.Id);
            if (prescription is null) throw new ArgumentException("No prescription with ID " + details.Id);

            prescription.Item = details.Item ?? prescription.Item;
            await DbCtx.SaveChangesAsync();
            return prescription;
        }
    }
}
