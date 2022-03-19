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

        public DbSet<Patient> PatientUsers { get; set; } = null!;
        public DbSet<Doctor> DoctorUsers { get; set; } = null!;
        public DbSet<Notification> Notifications { get; set; } = null!;
        public DbSet<Case> Cases { get; set; } = null!;
        public DbSet<Prescription> Prescriptions { get; set; } = null!;
        public DbSet<Vaccination> Vaccinations { get; set; } = null!;
        public DbSet<TestResult> TestResults { get; set; } = null!;
        public DbSet<Appointment> Appointments { get; set; } = null!;
        public DbSet<TriageRequest> TriageRequests { get; set; } = null!;
        public DbSet<Announcement> Announcements { get; set; } = null!;
    }
}
