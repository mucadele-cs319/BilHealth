using BilHealth.Model.Dto;
using BilHealth.Services.Users;
using BilHealth.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BilHealth.Controllers
{
    [ApiController]
    [Route("api/[controller]s")]
    [Authorize]
    [Produces("application/json")]
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
        public async Task<List<NotificationDto>> Get(bool unread = false)
        {
            var user = await AuthenticationService.GetUser(User);
            var notifications = await NotificationService.GetNotifications(user, unread);
            return notifications.Select(DtoMapper.Map).ToList();
        }

        [HttpPatch("{notificationId:guid}")]
        public async Task<IActionResult> MarkRead(Guid notificationId, bool all = false)
        {
            await NotificationService.MarkNotificationRead((Guid)notificationId);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> MarkAllRead()
        {
            var user = await AuthenticationService.GetUser(User);
            await NotificationService.MarkAllNotificationsRead(user);
            return Ok();
        }
    }
}
