using BilHealth.Model;
using BilHealth.Model.Dto;

namespace BilHealth.Utility
{
    public class DtoMapper
    {
        public static UserProfileDto Map(DomainUser user)
        {

            var dto = new UserProfileDto
            {
                Id = user.Id,
                UserType = user.Discriminator,
                Email = user.AppUser.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Gender = user.Gender,
                DateOfBirth = user.DateOfBirth
            };

            if (user is Patient patient)
            {
                dto.BodyWeight = patient.BodyWeight;
                dto.BodyHeight = patient.BodyHeight;
                dto.BloodType = patient.BloodType;
                dto.Vaccinations = patient.Vaccinations?.Select(Map).ToList();
                dto.TestResults = patient.TestResults?.Select(Map).ToList();
                dto.Cases = patient.Cases?.Select(MapSimpleCase).ToList();
                dto.Blacklisted = patient.Blacklisted;
            }
            else if (user is Doctor doctor)
            {
                dto.Specialization = doctor.Specialization;
                dto.Campus = doctor.Campus;
                dto.Cases = doctor.Cases?.Select(MapSimpleCase).ToList();
            }

            return dto;
        }

        public static SimpleUserDto MapSimpleUser(DomainUser user)
        {
            var dto = new SimpleUserDto
            {
                Id = user.Id,
                UserType = user.Discriminator,
                UserName = user.AppUser.UserName,
                Email = user.AppUser.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
            };
            return dto;
        }

        public static VaccinationDto Map(Vaccination vaccination)
        {
            var dto = new VaccinationDto
            {
                Id = vaccination.Id,
                PatientUserId = vaccination.PatientUserId,
                DateTime = vaccination.DateTime,
                Type = vaccination.Type
            };
            return dto;
        }

        public static TestResultDto Map(TestResult testResult)
        {
            var dto = new TestResultDto
            {
                Id = testResult.Id,
                PatientUserId = testResult.PatientUserId,
                DateTime = testResult.DateTime,
                Type = testResult.Type
            };
            return dto;
        }

        public static TriageRequestDto Map(TriageRequest triageRequest)
        {
            var dto = new TriageRequestDto
            {
                Id = triageRequest.Id,
                CaseId = triageRequest.CaseId,
                ApprovalStatus = triageRequest.ApprovalStatus,
                DoctorUserId = triageRequest.DoctorUserId,
                RequestingUserId = triageRequest.RequestingUserId
            };
            return dto;
        }

        public static CaseDto Map(Case _case)
        {
            var dto = new CaseDto
            {
                Id = _case.Id,
                DateTime = _case.DateTime,
                Title = _case.Title,
                State = _case.State,
                Type = _case.Type,
                PatientUserId = _case.PatientUserId,
                DoctorUserId = _case.DoctorUserId,
                Diagnosis = _case.Diagnosis
            };

            if (_case.Appointments is not null)
                dto.Appointments = _case.Appointments.Select(Map).ToList();

            if (_case.Messages is not null)
                dto.Messages = _case.Messages.Select(Map).ToList();

            if (_case.SystemMessages is not null)
                dto.SystemMessages = _case.SystemMessages.Select(Map).ToList();

            if (_case.Prescriptions is not null)
                dto.Prescriptions = _case.Prescriptions.Select(Map).ToList();

            if (_case.TriageRequests is not null)
                dto.TriageRequests = _case.TriageRequests.Select(Map).ToList();

            return dto;
        }

        public static SimpleCaseDto MapSimpleCase(Case _case)
        {
            var dto = new SimpleCaseDto
            {
                Id = _case.Id,
                DateTime = _case.DateTime,
                State = _case.State,
                PatientUserId = _case.PatientUserId,
                DoctorUserId = _case.DoctorUserId
            };

            if (_case.Messages is not null)
                dto.MessageCount = _case.Messages.Count;

            return dto;
        }

        public static AnnouncementDto Map(Announcement announcement)
        {
            var dto = new AnnouncementDto
            {
                Id = announcement.Id,
                DateTime = announcement.DateTime,
                Title = announcement.Title,
                Message = announcement.Message
            };
            return dto;
        }

        public static AppointmentDto Map(Appointment appointment)
        {
            var dto = new AppointmentDto
            {
                Id = appointment.Id,
                RequestedById = appointment.RequestedById,
                CaseId = appointment.CaseId,
                ApprovalStatus = appointment.ApprovalStatus,
                Attended = appointment.Attended,
                Cancelled = appointment.Cancelled,
                CreatedAt = appointment.CreatedAt,
                DateTime = appointment.DateTime,
                Description = appointment.Description,
                Visit = appointment.Visit is not null ? Map(appointment.Visit) : null
            };
            return dto;
        }

        public static AppointmentVisitDto Map(AppointmentVisit appointmentVisit)
        {
            var dto = new AppointmentVisitDto
            {
                Id = appointmentVisit.Id,
                AppointmentId = appointmentVisit.AppointmentId,
                BloodPressure = appointmentVisit.BloodPressure,
                BodyTemperature = appointmentVisit.BodyTemperature,
                BPM = appointmentVisit.BPM,
                Notes = appointmentVisit.Notes
            };
            return dto;
        }

        public static CaseMessageDto Map(CaseMessage message)
        {
            var dto = new CaseMessageDto
            {
                Id = message.Id,
                CaseId = message.CaseId,
                UserId = message.UserId,
                DateTime = message.DateTime,
                Content = message.Content
            };
            return dto;
        }

        public static CaseSystemMessageDto Map(CaseSystemMessage message)
        {
            var dto = new CaseSystemMessageDto
            {
                Id = message.Id,
                CaseId = message.CaseId,
                DateTime = message.DateTime,
                Type = message.Type,
                Content = message.Content
            };
            return dto;
        }

        public static NotificationDto Map(Notification notification)
        {
            var dto = new NotificationDto
            {
                Id = notification.Id,
                DateTime = notification.DateTime,
                UserId = notification.UserId,
                Type = notification.Type,
                Read = notification.Read,
                ReferenceId1 = notification.ReferenceId1,
                ReferenceId2 = notification.ReferenceId2
            };
            return dto;
        }

        public static PrescriptionDto Map(Prescription prescription)
        {
            var dto = new PrescriptionDto
            {
                Id = prescription.Id,
                CaseId = prescription.CaseId,
                DateTime = prescription.DateTime,
                DoctorUserId = prescription.DoctorUserId,
                Item = prescription.Item
            };
            return dto;
        }
    }
}
