using BilHealth.Data;
using BilHealth.Model;
using BilHealth.Model.Dto;
using BilHealth.Model.Dto.Incoming;
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

        public async Task<Announcement> AddAnnouncement(AnnouncementUpdateDto details)
        {
            var newAnnouncement = new Announcement
            {
                DateTime = Clock.GetCurrentInstant(),
                Title = details.Title,
                Message = details.Message
            };
            DbCtx.Announcements.Add(newAnnouncement);
            await DbCtx.SaveChangesAsync();
            return newAnnouncement;
        }

        public async Task<bool> RemoveAnnouncement(Guid announcementId)
        {
            var announcement = await DbCtx.Announcements.FindOrThrowAsync(announcementId);

            DbCtx.Announcements.Remove(announcement);
            await DbCtx.SaveChangesAsync();
            return true;
        }

        public async Task<Announcement> UpdateAnnouncement(Guid announcementId, AnnouncementUpdateDto details)
        {
            var announcement = await DbCtx.Announcements.FindOrThrowAsync(announcementId);

            announcement.Title = details.Title;
            announcement.Message = details.Message;
            await DbCtx.SaveChangesAsync();
            return announcement;
        }

        public Task<List<Announcement>> GetAllAnnouncements()
        {
            return DbCtx.Announcements.OrderByDescending(a => a.DateTime).ToListAsync();
        }
    }
}
