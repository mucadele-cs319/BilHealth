using BilHealth.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BilHealth.Data
{
    public class AppDbContext : IdentityDbContext<User, Role, Guid>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Patient> PatientUsers => Set<Patient>();
        public DbSet<Doctor> DoctorUsers => Set<Doctor>();
        public DbSet<Notification> Notifications => Set<Notification>();
        public DbSet<Case> Cases => Set<Case>();
        public DbSet<Prescription> Prescriptions => Set<Prescription>();
        public DbSet<Vaccination> Vaccinations => Set<Vaccination>();
        public DbSet<TestResult> TestResults => Set<TestResult>();
        public DbSet<Appointment> Appointments => Set<Appointment>();
        public DbSet<TriageRequest> TriageRequests => Set<TriageRequest>();
        public DbSet<Announcement> Announcements => Set<Announcement>();
    }
}
