using BilHealth.Model.Dto;
using BilHealth.Model.Dto.Incoming;
using BilHealth.Services;
using BilHealth.Utility;
using BilHealth.Utility.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BilHealth.Controllers
{
    [ApiController]
    [Route("api/[controller]s")]
    [Authorize(Roles = $"{UserType.Admin},{UserType.Staff},{UserType.Doctor}")]
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
        public async Task<AnnouncementDto> Create(AnnouncementUpdateDto details)
        {
            return DtoMapper.Map(await AnnouncementService.AddAnnouncement(details));
        }

        [HttpPut("{announcementId:guid}")]
        public async Task<AnnouncementDto> Update(Guid announcementId, AnnouncementUpdateDto details)
        {
            return DtoMapper.Map(await AnnouncementService.UpdateAnnouncement(announcementId, details));
        }

        [HttpDelete("{announcementId:guid}")]
        public async Task<IActionResult> Delete(Guid announcementId)
        {
            return await AnnouncementService.RemoveAnnouncement(announcementId) ? Ok() : NotFound();
        }
    }
}
