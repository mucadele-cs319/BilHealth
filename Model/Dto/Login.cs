using System.ComponentModel.DataAnnotations;

namespace BilHealth.Model.Dto
{
    public record Login
    {
        [Required] public string UserName { get; init; } = null!;
        [Required] public string Password { get; init; } = null!;
        public bool RememberMe { get; init; } = false;
    }
}
