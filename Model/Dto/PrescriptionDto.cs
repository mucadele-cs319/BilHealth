namespace BilHealth.Model.Dto
{
    public record PrescriptionDto
    {
        public Guid Id { get; set; }
        public DateTime DateTime { get; set; }
        public Guid DoctorUserId { get; set; }

        public string Item { get; set; } = null!;
    }
}
