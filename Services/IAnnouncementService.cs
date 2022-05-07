using BilHealth.Model;
using BilHealth.Model.Dto.Incoming;

namespace BilHealth.Services
{
    public interface IAnnouncementService
    {
        Task<Announcement> AddAnnouncement(AnnouncementUpdateDto details);
        /// <summary>
        /// Updates an announcement's message and title, if it exists.
        /// </summary>
        /// <param name="details">The announcement object containing edits</param>
        /// <returns>The updated announcement</returns>
        Task<Announcement> UpdateAnnouncement(Guid announcementId, AnnouncementUpdateDto details);
        /// <summary>
        /// Removes an announcement, if it exists.
        /// </summary>
        /// <param name="announcementId">The ID of the announcement to be removed</param>
        /// <returns>true if announcement exists, otherwise false</returns>
        Task<bool> RemoveAnnouncement(Guid announcementId);
        Task<List<Announcement>> GetAllAnnouncements();
    }
}
