using BilHealth.Data;
using BilHealth.Model;
using BilHealth.Model.Dto.Incoming;
using BilHealth.Services.Users;
using BilHealth.Utility;
using BilHealth.Utility.Enum;
using Microsoft.EntityFrameworkCore;
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

            await DbCtx.Entry(_case).Reference(c => c.PatientUser).LoadAsync();
            await DbCtx.Entry(_case).Reference(c => c.DoctorUser).LoadAsync();

            await DbCtx.Entry(_case).Collection(c => c.Messages!).Query().Include(m => m.User).LoadAsync();
            await DbCtx.Entry(_case).Collection(c => c.SystemMessages!).LoadAsync();
            await DbCtx.Entry(_case).Collection(c => c.Appointments!).Query().Include(a => a.Visit).LoadAsync();
            await DbCtx.Entry(_case).Collection(c => c.Prescriptions!).Query().Include(p => p.DoctorUser).LoadAsync();
            await DbCtx.Entry(_case).Collection(c => c.TriageRequests!).Query().Include(t => t.RequestingUser).Include(t => t.DoctorUser).LoadAsync();

            return _case;
        }

        public async Task<Case> CreateCase(CaseCreateDto details)
        {
            var _case = new Case
            {
                DateTime = Clock.GetCurrentInstant(),
                PatientUserId = details.PatientUserId,
                Title = details.Title,
                Type = details.Type,
                State = CaseState.WaitingTriage,
            };

            DbCtx.Cases.Add(_case);
            await DbCtx.SaveChangesAsync();
            return await GetCase(_case.Id);
        }

        public async Task<CaseMessage> CreateMessage(Guid caseId, Guid userId, CaseMessageUpdateDto details)
        {
            var message = new CaseMessage
            {
                CaseId = caseId,
                UserId = userId,
                Content = details.Content,
                DateTime = Clock.GetCurrentInstant()
            };
            DbCtx.Add(message);
            await NotificationService.AddNewCaseMessageNotification(message);
            await DbCtx.SaveChangesAsync();
            await DbCtx.Entry(message).Reference(m => m.User).LoadAsync();
            return message;
        }

        public async Task<Prescription> CreatePrescription(Guid caseId, Guid doctorUserId, PrescriptionUpdateDto details)
        {
            var prescription = new Prescription
            {
                CaseId = caseId,
                DateTime = Clock.GetCurrentInstant(),
                DoctorUserId = doctorUserId,
                Item = details.Item
            };
            DbCtx.Prescriptions.Add(prescription);

            var patientUserId = await DbCtx.Cases.Where(c => c.Id == caseId).Select(c => c.PatientUserId).SingleOrDefaultAsync();
            NotificationService.AddNewPrescriptionNotification(patientUserId, prescription);

            await DbCtx.SaveChangesAsync();
            await DbCtx.Entry(prescription).Reference(p => p.DoctorUser).LoadAsync();
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

        public async Task<TriageRequest> CreateTriageRequest(Guid caseId, Guid requestingUserId, Guid doctorUserId)
        {
            var _case = await DbCtx.Cases.FindOrThrowAsync(caseId);
            if (_case.State != CaseState.WaitingTriage)
                throw new InvalidOperationException("Cannot request triage without finalizing existing ones");

            _case.State = CaseState.WaitingTriageApproval;

            var triageRequest = new TriageRequest
            {
                ApprovalStatus = ApprovalStatus.Waiting,
                CaseId = caseId,
                DoctorUserId = doctorUserId,
                RequestingUserId = requestingUserId,
                DateTime = Clock.GetCurrentInstant()
            };

            DbCtx.TriageRequests.Add(triageRequest);
            await DbCtx.SaveChangesAsync();
            await DbCtx.Entry(triageRequest).Reference(t => t.DoctorUser).LoadAsync();
            await DbCtx.Entry(triageRequest).Reference(t => t.RequestingUser).LoadAsync();
            return triageRequest;
        }

        public async Task<CaseMessage> EditMessage(Guid messageId, CaseMessageUpdateDto details)
        {
            var message = (CaseMessage)await DbCtx.FindOrThrowAsync(typeof(CaseMessage), messageId);

            message.Content = details.Content ?? message.Content;
            await DbCtx.SaveChangesAsync();
            await DbCtx.Entry(message).Reference(m => m.User).LoadAsync();
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
                $"The prescription for '{prescription.Item}' was removed.");
            await DbCtx.SaveChangesAsync();
            return true;
        }

        public async Task SetCaseState(Guid caseId, CaseState newState)
        {
            var _case = await DbCtx.Cases.FindOrThrowAsync(caseId);

            if (newState == CaseState.Closed)
            {
                await DbCtx.TriageRequests
                    .Where(t => t.CaseId == caseId && t.ApprovalStatus == ApprovalStatus.Waiting)
                    .ForEachAsync(t => t.ApprovalStatus = ApprovalStatus.Rejected);

                NotificationService.AddCaseClosedNotification(_case.PatientUserId, _case);
            }

            CreateSystemMessage(
                _case.Id,
                CaseSystemMessageType.CaseStateUpdated,
                $"{_case.State} --> {newState}");
            _case.State = newState;
            await DbCtx.SaveChangesAsync();
        }

        public async Task SetTriageRequestApproval(Guid caseId, ApprovalStatus approval)
        {
            var triageRequest = await DbCtx.TriageRequests.Where(t => t.CaseId == caseId).OrderBy(t => t.DateTime).Include(t => t.Case).LastAsync();

            triageRequest.ApprovalStatus = approval;

            switch (triageRequest.ApprovalStatus)
            {
                case ApprovalStatus.Waiting:
                    throw new ArgumentException("Unexpectedly received 'Waiting' approval", nameof(approval));
                case ApprovalStatus.Approved:
                    triageRequest.Case.DoctorUserId = triageRequest.DoctorUserId;
                    triageRequest.Case.State = CaseState.Ongoing;
                    NotificationService.AddCaseTriagedNotification(triageRequest.Case.PatientUserId, triageRequest.Case!);
                break;
                case ApprovalStatus.Rejected:
                    triageRequest.Case.State = CaseState.WaitingTriage;
                break;
            }

            await DbCtx.SaveChangesAsync();
        }

        public async Task<Prescription> UpdatePrescription(Guid prescriptionId, PrescriptionUpdateDto details)
        {
            var prescription = await DbCtx.Prescriptions.FindOrThrowAsync(prescriptionId);

            prescription.Item = details.Item ?? prescription.Item;
            await DbCtx.SaveChangesAsync();
            await DbCtx.Entry(prescription).Reference(p => p.DoctorUser).LoadAsync();
            return prescription;
        }

        public async Task<bool> SetDiagnosis(Guid caseId, CaseDiagnosisUpdateDto details)
        {
            Case _case;
            try
            {
                _case = await DbCtx.Cases.FindOrThrowAsync(caseId);
            }
            catch (IdNotFoundException) { return false; }
            _case.Diagnosis = details.Content;
            await DbCtx.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UnassignDoctor(Guid caseId)
        {
            Case _case;
            try
            {
                _case = await DbCtx.Cases.FindOrThrowAsync(caseId);
            }
            catch (IdNotFoundException) { return false; }
            if (_case.DoctorUserId is null) return false;

            _case.DoctorUserId = null;
            _case.State = CaseState.WaitingTriage;
            NotificationService.AddCaseDoctorResignedNotification(_case.PatientUserId, _case);
            await DbCtx.SaveChangesAsync();
            return true;
        }
    }
}
