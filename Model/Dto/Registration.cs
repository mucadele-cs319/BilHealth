using System.ComponentModel.DataAnnotations;

namespace BilHealth.Model.Dto
{
    public record Registration
    {
        [Required] public string UserName { get; init; } = null!;
        [Required] public string Password { get; init; } = null!;
        [Required] public string FirstName { get; init; } = null!;
        [Required] public string LastName { get; init; } = null!;
        [Required] public string Email { get; init; } = null!;
    }
}