using BilHealth.Model;
using BilHealth.Model.Dto;
using BilHealth.Utility.Enum;

namespace BilHealth.Services.Users
{
    public interface INotificationService
    {
        public Task CreateNotification();
        public Task MarkNotificationRead();
    }
}
