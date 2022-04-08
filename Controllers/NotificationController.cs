using BilHealth.Model.Dto;
using BilHealth.Services.Users;
using BilHealth.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BilHealth.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize]
    public class NotificationController : ControllerBase
    {
        private readonly IAuthenticationService AuthenticationService;
        private readonly INotificationService NotificationService;

        public NotificationController(IAuthenticationService authenticationService, INotificationService notificationService)
        {
            AuthenticationService = authenticationService;
            NotificationService = notificationService;
        }

        [HttpGet]
        public async Task<List<NotificationDto>> All()
        {
            var user = await AuthenticationService.GetUser(User);
            var notifications = await NotificationService.GetAllNotifications(user.DomainUser);
            return notifications.Select(DtoMapper.Map).ToList();
        }

        [HttpGet]
        public async Task<List<NotificationDto>> Unread()
        {
            var user = await AuthenticationService.GetUser(User);
            var notifications = await NotificationService.GetUnreadNotifications(user.DomainUser);
            return notifications.Select(DtoMapper.Map).ToList();
        }

        [HttpPatch]
        public async Task<IActionResult> MarkRead(Guid? notificationId, bool all = false)
        {
            var user = await AuthenticationService.GetUser(User);

            if (all)
            {
                await NotificationService.MarkAllNotificationsRead(user.DomainUser);
                return Ok();
            }

            if (notificationId is null) return BadRequest("Missing notification ID");

            await NotificationService.MarkNotificationRead((Guid)notificationId);
            return Ok();
        }
    }
}
