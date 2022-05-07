namespace BilHealth.Model.Dto
{
    public class SimpleUserDto
    {
        public Guid Id { get; set; }
        public string UserType { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
    }
}
