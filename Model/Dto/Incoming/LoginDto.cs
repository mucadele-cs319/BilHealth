using System.ComponentModel.DataAnnotations;

namespace BilHealth.Model.Dto.Incoming
{
    public class LoginDto
    {
        [Required] public string UserName { get; init; } = null!;
        [Required] public string Password { get; init; } = null!;
        public bool RememberMe { get; init; } = false;
    }
}
