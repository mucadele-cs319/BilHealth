using System.ComponentModel.DataAnnotations;

namespace BilHealth.Model.Transaction
{
    public class Register
    {
        [Required] public string? UserName { get; set; }
        [Required] public string? Password { get; set; }
        [Required] public string? FirstName { get; set; }
        [Required] public string? LastName { get; set; }
        [Required] public string? Email { get; set; }
    }
}
