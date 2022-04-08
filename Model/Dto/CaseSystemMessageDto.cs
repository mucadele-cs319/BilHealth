using BilHealth.Utility.Enum;

namespace BilHealth.Model.Dto
{
    public record CaseSystemMessageDto
    {
        public Guid Id { get; set; }
        public CaseSystemMessageType Type { get; set; }
        public DateTime DateTime { get; set; }
        public string Content { get; set; } = String.Empty;
    }
}
