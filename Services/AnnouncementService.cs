using System.Security.Claims;
using BilHealth.Data;
using BilHealth.Model;
using BilHealth.Model.Dto;
using BilHealth.Utility.Enum;
using Microsoft.EntityFrameworkCore;

namespace BilHealth.Services
{
    public class AnnouncementService : DbServiceBase, IAnnouncementService
    {
        public AnnouncementService(AppDbContext dbCtx) : base(dbCtx)
        {
        }

        public async Task AddAnnouncement(AnnouncementDto announcement)
        {
            var newAnnouncement = new Announcement {
                DateTime = DateTime.Now,
                Title = announcement.Title,
                Message = announcement.Message
            };
            DbCtx.Announcements.Add(newAnnouncement);
            await DbCtx.SaveChangesAsync();
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
