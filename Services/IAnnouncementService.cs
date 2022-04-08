using BilHealth.Model;
using BilHealth.Model.Dto;

namespace BilHealth.Services
{
    public interface IAnnouncementService
    {
        Task<Announcement> AddAnnouncement(AnnouncementDto announcement);
        /// <summary>
        /// Updates an announcement's message and title, if it exists.
        /// </summary>
        /// <param name="announcement">The announcement object containing edits</param>
        /// <returns>true if announcement exists, otherwise false</returns>
        Task<bool> UpdateAnnouncement(AnnouncementDto announcement);
        /// <summary>
        /// Removes an announcement, if it exists.
        /// </summary>
        /// <param name="announcementId">The ID of the announcement to be removed</param>
        /// <returns>true if announcement exists, otherwise false</returns>
        Task<bool> RemoveAnnouncement(Guid announcementId);
        Task<List<Announcement>> GetAllAnnouncements();
    }
}
