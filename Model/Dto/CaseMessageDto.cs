namespace BilHealth.Model.Dto
{
    public record CaseMessageDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime DateTime { get; set; }
        public string Content { get; set; } = String.Empty;
    }
}
