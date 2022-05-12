using NodaTime;

namespace BilHealth.Model.Dto
{
    public class CaseMessageDto
    {
        public Guid Id { get; set; }
        public Guid CaseId { get; set; }
        public SimpleUserDto User { get; set; } = null!;
        public Instant DateTime { get; set; }
        public string Content { get; set; } = null!;
    }
}
