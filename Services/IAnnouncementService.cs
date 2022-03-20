using BilHealth.Model;
using BilHealth.Model.Dto;
using BilHealth.Utility.Enum;

namespace BilHealth.Services
{
    public interface IAnnouncementService
    {
        public Task AddAnnouncement();
        public Task UpdateAnnouncement();
    }
}
