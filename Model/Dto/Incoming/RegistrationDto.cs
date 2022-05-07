using System.ComponentModel.DataAnnotations;

namespace BilHealth.Model.Dto.Incoming
{
    public class RegistrationDto
    {
        [Required] public string UserType { get; init; } = null!;
        [Required] public string UserName { get; init; } = null!;
        [Required] public string Password { get; init; } = null!;
        [Required] public string FirstName { get; init; } = null!;
        [Required] public string LastName { get; init; } = null!;
        [Required] public string Email { get; init; } = null!;
    }
}
