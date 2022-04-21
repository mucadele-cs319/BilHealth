using BilHealth.Data;
using BilHealth.Model;
using BilHealth.Model.Dto;
using Microsoft.EntityFrameworkCore;
using NodaTime;

namespace BilHealth.Services
{
    public class AnnouncementService : DbServiceBase, IAnnouncementService
    {
        private readonly IClock Clock;
        public AnnouncementService(AppDbContext dbCtx, IClock clock) : base(dbCtx)
        {
            Clock = clock;
        }

        public async Task<Announcement> AddAnnouncement(AnnouncementDto announcement)
        {
            var newAnnouncement = new Announcement
            {
                DateTime = Clock.GetCurrentInstant(),
                Title = announcement.Title,
                Message = announcement.Message
            };
            DbCtx.Announcements.Add(newAnnouncement);
            await DbCtx.SaveChangesAsync();
            return newAnnouncement;
        }

        public async Task<bool> RemoveAnnouncement(Guid announcementId)
        {
            var announcement = await DbCtx.Announcements.FindAsync(announcementId);
            if (announcement is null)
                return false;

            DbCtx.Announcements.Remove(announcement);
            await DbCtx.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAnnouncement(AnnouncementDto announcement)
        {
            var existingAnnouncement = await DbCtx.Announcements.SingleOrDefaultAsync(a => a.Id == announcement.Id);
            if (existingAnnouncement is null)
                return false;

            existingAnnouncement.Title = announcement.Title;
            existingAnnouncement.Message = announcement.Message;
            await DbCtx.SaveChangesAsync();
            return true;
        }

        public async Task<List<Announcement>> GetAllAnnouncements()
        {
            return await DbCtx.Announcements.OrderByDescending(a => a.DateTime).ToListAsync();
        }
    }
}
