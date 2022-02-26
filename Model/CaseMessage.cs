using System.ComponentModel.DataAnnotations;

namespace BilHealth.Model
{
    public class CaseMessage
    {
        [Required] public Guid Id { get; private set; }
        [Required] public Guid UserId { get; set; }
        [Required] public DateTime DateTime { get; set; }
        [Required] public string Content { get; set; } = null!;
    }
}
