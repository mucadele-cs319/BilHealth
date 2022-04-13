using BilHealth.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BilHealth.Data
{
    public class AppDbContext : IdentityDbContext<AppUser, Role, Guid>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<DomainUser> DomainUsers => Set<DomainUser>();
        public DbSet<Admin> AdminUsers => Set<Admin>();
        public DbSet<Doctor> DoctorUsers => Set<Doctor>();
        public DbSet<Nurse> NurseUsers => Set<Nurse>();
        public DbSet<Staff> StaffUsers => Set<Staff>();
        public DbSet<Patient> PatientUsers => Set<Patient>();

        public DbSet<Notification> Notifications => Set<Notification>();
        public DbSet<Case> Cases => Set<Case>();
        public DbSet<Prescription> Prescriptions => Set<Prescription>();
        public DbSet<Vaccination> Vaccinations => Set<Vaccination>();
        public DbSet<TestResult> TestResults => Set<TestResult>();
        public DbSet<Appointment> Appointments => Set<Appointment>();
        public DbSet<AppointmentVisit> AppointmentVisits => Set<AppointmentVisit>();
        public DbSet<TriageRequest> TriageRequests => Set<TriageRequest>();
        public DbSet<Announcement> Announcements => Set<Announcement>();
    }
}
