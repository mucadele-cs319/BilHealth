namespace BilHealth.Model.Dto
{
    public record AnnouncementDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Message { get; set; } = null!;
    }
}
