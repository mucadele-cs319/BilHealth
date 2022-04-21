using BilHealth.Data;
using BilHealth.Model;
using BilHealth.Model.Dto;
using BilHealth.Services.Users;
using BilHealth.Utility.Enum;
using NodaTime;

namespace BilHealth.Services
{
    public class CaseService : DbServiceBase, ICaseService
    {
        private readonly INotificationService NotificationService;
        private readonly IClock Clock;

        public CaseService(AppDbContext dbCtx, INotificationService notificationService, IClock clock) : base(dbCtx)
        {
            NotificationService = notificationService;
            Clock = clock;
        }

        public async Task<Case> GetCase(Guid caseId)
        {
            var _case = await DbCtx.Cases.FindOrThrowAsync(caseId);

            await DbCtx.Entry(_case).Collection(c => c.Messages!).LoadAsync();
            await DbCtx.Entry(_case).Collection(c => c.SystemMessages!).LoadAsync();
            await DbCtx.Entry(_case).Collection(c => c.Appointments!).LoadAsync();
            await DbCtx.Entry(_case).Collection(c => c.Prescriptions!).LoadAsync();

            if (_case.Appointments is not null)
                foreach (var appointment in _case.Appointments)
                    await DbCtx.Entry(appointment).Reference(a => a.Visit).LoadAsync();

            return _case;
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
            await NotificationService.AddNewCaseMessageNotification(message);
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
            NotificationService.AddNewPrescriptionNotification(prescription.Case.PatientUser.AppUserId, prescription);
            await DbCtx.SaveChangesAsync();
            return prescription;
        }

        public void CreateSystemMessage(Guid caseId, CaseSystemMessageType type, string content)
        {
            var message = new CaseSystemMessage
            {
                DateTime = Clock.GetCurrentInstant(),
                CaseId = caseId,
                Type = type,
                Content = content
            };
            DbCtx.Add(message);
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
            var message = (CaseMessage)await DbCtx.FindOrThrowAsync(typeof(CaseMessage), details.Id);

            message.Content = details.Content ?? message.Content;
            await DbCtx.SaveChangesAsync();
            return message;
        }

        public async Task<bool> RemoveMessage(Guid messageId)
        {
            var message = (CaseMessage)await DbCtx.FindOrThrowAsync(typeof(CaseMessage), messageId);

            DbCtx.Remove(message);
            await DbCtx.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemovePrescription(Guid prescriptionId)
        {
            var prescription = await DbCtx.Prescriptions.FindOrThrowAsync(prescriptionId);

            DbCtx.Prescriptions.Remove(prescription);
            CreateSystemMessage(
                prescription.CaseId,
                CaseSystemMessageType.PrescriptionRemoved,
                $"The prescription with ID {prescription.Id} was removed.");
            await DbCtx.SaveChangesAsync();
            return true;
        }

        public async Task SetCaseState(Guid caseId, CaseState newState)
        {
            var _case = await DbCtx.Cases.FindOrThrowAsync(caseId);

            if (newState == CaseState.Closed)
                NotificationService.AddCaseClosedNotification(_case.PatientUserId, _case);

            CreateSystemMessage(
                _case.Id,
                CaseSystemMessageType.CaseStateUpdated,
                $"{_case.State} --> {newState}");
            _case.State = newState;
            await DbCtx.SaveChangesAsync();
        }

        public async Task SetTriageRequestApproval(TriageRequestDto details)
        {
            var triageRequest = await DbCtx.TriageRequests.FindOrThrowAsync(details.Id);
            await DbCtx.Entry(triageRequest).Reference(t => t.Case).LoadAsync();

            triageRequest.ApprovalStatus = details.ApprovalStatus;

            if (triageRequest.ApprovalStatus == ApprovalStatus.Approved)
            {
                triageRequest.Case!.DoctorUserId = triageRequest.DoctorUserId;
                NotificationService.AddCaseTriagedNotification(triageRequest.Case.PatientUserId, triageRequest.Case!);
            }

            await DbCtx.SaveChangesAsync();
        }

        public async Task<Prescription> UpdatePrescription(PrescriptionDto details)
        {
            var prescription = await DbCtx.Prescriptions.FindOrThrowAsync(details.Id);

            prescription.Item = details.Item ?? prescription.Item;
            await DbCtx.SaveChangesAsync();
            return prescription;
        }
    }
}
