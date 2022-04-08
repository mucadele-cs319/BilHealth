using System.ComponentModel.DataAnnotations;
using NodaTime;

namespace BilHealth.Model
{
    public class CaseMessage
    {
        [Required] public Guid Id { get; private set; }
        [Required] public Guid UserId { get; set; }
        [Required] public Instant DateTime { get; set; }
        [Required] public string Content { get; set; } = String.Empty;

        [Required] public Guid CaseId { get; set; }
        public Case? Case { get; set; }
    }
}
