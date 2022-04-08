using BilHealth.Model;
using BilHealth.Model.Dto;
using BilHealth.Utility.Enum;

namespace BilHealth.Utility
{
    public class DtoMapper
    {
        public static UserProfileDto Map(AppUser user, UserRoleType role)
        {
            var domainUser = user.DomainUser;

            var dto = new UserProfileDto
            {
                UserType = role,
                Email = user.Email,
                FirstName = domainUser.FirstName,
                LastName = domainUser.LastName,
                Gender = domainUser.Gender,
                DateOfBirth = domainUser.DateOfBirth
            };

            if (domainUser is Patient patient)
            {
                dto.BodyWeight = patient.BodyWeight;
                dto.BodyHeight = patient.BodyHeight;
                dto.BloodType = patient.BloodType;
                dto.Vaccinations = patient.Vaccinations?.Select(Map).ToList();
                dto.TestResults = patient.TestResults?.Select(Map).ToList();
                dto.Cases = patient.Cases?.Select(Map).ToList();
                dto.Blacklisted = patient.Blacklisted;
            }
            else if (domainUser is Nurse nurse)
            {
                dto.TriageRequests = nurse.TriageRequests?.Select(Map).ToList();
            }
            else if (domainUser is Doctor doctor)
            {
                dto.Specialization = doctor.Specialization;
                dto.Campus = doctor.Campus;
                dto.Cases = doctor.Cases?.Select(Map).ToList();
            }

            dto.Cases = new();

            return dto;
        }

        public static VaccinationDto Map(Vaccination vaccination)
        {
            var dto = new VaccinationDto
            {

            };
            return dto;
        }

        public static TestResultDto Map(TestResult testResult)
        {
            var dto = new TestResultDto
            {

            };
            return dto;
        }

        public static TriageRequestDto Map(TriageRequest triageRequest)
        {
            var dto = new TriageRequestDto
            {

            };
            return dto;
        }

        public static CaseDto Map(Case _case)
        {
            var dto = new CaseDto
            {

            };
            return dto;
        }

        public static AnnouncementDto Map(Announcement announcement)
        {
            var dto = new AnnouncementDto
            {

            };
            return dto;
        }

        public static AppointmentDto Map(Appointment appointment)
        {
            var dto = new AppointmentDto
            {

            };
            return dto;
        }

        public static AppointmentVisitDto Map(AppointmentVisit appointmentVisit)
        {
            var dto = new AppointmentVisitDto
            {

            };
            return dto;
        }

        public static CaseMessageDto Map(CaseMessage message)
        {
            var dto = new CaseMessageDto
            {

            };
            return dto;
        }

        public static CaseSystemMessageDto Map(CaseSystemMessage message)
        {
            var dto = new CaseSystemMessageDto
            {

            };
            return dto;
        }

        public static NotificationDto Map(Notification notification)
        {
            var dto = new NotificationDto
            {

            };
            return dto;
        }

        public static PrescriptionDto Map(Prescription prescription)
        {
            var dto = new PrescriptionDto
            {

            };
            return dto;
        }
    }
}
