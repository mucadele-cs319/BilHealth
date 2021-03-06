using NodaTime;

namespace BilHealth.Model.Dto
{
    public class AnnouncementDto
    {
        public Guid Id { get; set; }
        public Instant DateTime { get; set; }
        public string Title { get; set; } = null!;
        public string Message { get; set; } = null!;
    }
}
