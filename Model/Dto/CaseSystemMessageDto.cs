using BilHealth.Utility.Enum;
using NodaTime;

namespace BilHealth.Model.Dto
{
    public class CaseSystemMessageDto
    {
        public Guid Id { get; set; }
        public Guid CaseId { get; set; }
        public CaseSystemMessageType Type { get; set; }
        public Instant DateTime { get; set; }
        public string Content { get; set; } = String.Empty;
    }
}
