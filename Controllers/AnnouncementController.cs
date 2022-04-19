using BilHealth.Model.Dto;
using BilHealth.Services;
using BilHealth.Utility;
using BilHealth.Utility.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BilHealth.Controllers
{
    [ApiController]
    [Route("api/[controller]s")]
    [Authorize(Roles = $"{UserRoleType.Constant.Admin},{UserRoleType.Constant.Staff},{UserRoleType.Constant.Doctor}")]
    [Produces("application/json")]
    public class AnnouncementController : ControllerBase
    {
        private readonly IAnnouncementService AnnouncementService;

        public AnnouncementController(IAnnouncementService announcementService)
        {
            AnnouncementService = announcementService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<List<AnnouncementDto>> All()
        {
            var announcements = await AnnouncementService.GetAllAnnouncements();
            return announcements.Select(DtoMapper.Map).ToList();
        }

        [HttpPost]
        public async Task<AnnouncementDto> Create(AnnouncementDto announcement)
        {
            return DtoMapper.Map(await AnnouncementService.AddAnnouncement(announcement));
        }

        [HttpPut("{announcementId:guid}")]
        public async Task<IActionResult> Update(Guid announcementId, AnnouncementDto announcement)
        {
            announcement.Id = announcementId;
            return await AnnouncementService.UpdateAnnouncement(announcement) ? Ok() : NotFound();
        }

        [HttpDelete("{announcementId:guid}")]
        public async Task<IActionResult> Delete(Guid announcementId)
        {
            return await AnnouncementService.RemoveAnnouncement(announcementId) ? Ok() : NotFound();
        }
    }
}
